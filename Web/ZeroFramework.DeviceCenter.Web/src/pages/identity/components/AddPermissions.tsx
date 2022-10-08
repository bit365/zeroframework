import { Card, ConfigProvider, FormInstance, Space, Tabs, Tree } from 'antd';
import { useIntl } from 'umi';
import { DrawerForm, ProFormSelect, } from '@ant-design/pro-form';
import React, { Key, useContext, useEffect, useRef, useState } from 'react';
import { get, update } from '@/services/deviceCenter/Permissions';
import { DataNode } from 'antd/lib/tree';
import { getResourceGroups } from '@/services/deviceCenter/ResourceGroups';
import { getUsers } from '@/services/identityServer/Users';
import { getRoles } from '@/services/identityServer/Roles';

export type AddPermissionsProps = {
    onCancel: () => void;
    onSubmit: () => Promise<void>;
    visible: boolean;
    providerName?: string;
    providerKey?: string;
    resourceGroupId?: string;
};

const { TabPane } = Tabs;

const AddPermissions: React.FC<AddPermissionsProps> = (props) => {

    const [permissionList, setPermissionList] = useState<API.PermissionListResponseModel>();
    const [selectedPermissionKeys, setSelectedPermissionKeys] = useState<{ [key: string]: string[] }>({});
    const formRef = useRef<FormInstance>();
    const intl = useIntl();
    const [selectedResourceGroupId, setSelectedResourceGroupId] = useState<string>();
    const [selectedProviderInfos, setSelectedProviderInfos] = useState<API.PermissionProviderInfoModel[]>([]);

    const fetchPermissions = async () => {
        let providerName = undefined;
        let providerKey = undefined;
        if (selectedProviderInfos.length) {
            providerName = selectedProviderInfos[0].providerName;
            providerKey = selectedProviderInfos[0].providerKey;
        }
        const result = await get({
            providerName: providerName,
            providerKey: providerKey,
            resourceGroupId: selectedResourceGroupId != '0' ? selectedResourceGroupId : undefined
        });
        setPermissionList(result);
        const permissionKeys: { [key: string]: string[] } = {};
        result?.groups?.forEach(g => {
            if (g.name) {
                permissionKeys[g.name] = [];
            }
            g.permissions?.forEach(p => {
                if (p.isGranted && p.name && g.name) {
                    permissionKeys[g.name].push(p.name);
                }
            });
        });
        setSelectedPermissionKeys(permissionKeys);
    };

    useEffect(() => {
        if (props.visible) {
            if (props.providerName && props.providerKey) {
                setSelectedProviderInfos([{ providerName: props.providerName, providerKey: props.providerKey }]);
                formRef.current?.setFieldsValue({
                    principal: [`${props.providerName}:${props.providerKey}`],
                });
            }
            if (props.resourceGroupId) {
                setSelectedResourceGroupId(props.resourceGroupId);
                formRef.current?.setFieldsValue({
                    resourceGroup: props.resourceGroupId,
                });
            }
            else {
                formRef.current?.setFieldsValue({
                    resourceGroup: '0',
                });
            }
        }
        else {
            setSelectedProviderInfos([]);
            setSelectedResourceGroupId('0');
            setPermissionList({});
            formRef.current?.resetFields();
        }
    }, [props.visible]);

    useEffect(() => {
        if (props.visible) {
            fetchPermissions();
        }
    }, [selectedResourceGroupId, selectedProviderInfos]);

    const treeDataNodeRecursively = (dataNode: DataNode, group: API.PermissionGroupModel) => {
        const children: DataNode[] = [];
        group.permissions?.forEach(p => {
            if (p.parentName == dataNode.key && p.name) {
                children.push({ title: p.displayName, key: p.name });
            }
        });
        dataNode.children = children;
        dataNode.children?.forEach(item => treeDataNodeRecursively(item, group));
    }

    const treeData = (group: API.PermissionGroupModel): DataNode[] => {

        const parentDataNodes: DataNode[] = [];

        group.permissions?.forEach(p => {
            if (!p.parentName && p.name) {
                parentDataNodes.push({ title: p.displayName, key: p.name });
            }
        });

        parentDataNodes.forEach(p => treeDataNodeRecursively(p, group));

        return [
            {
                title: group.displayName,
                key: group.name!,
                children: parentDataNodes,
            }
        ]
    };

    const context = useContext(ConfigProvider.ConfigContext);

    return (<DrawerForm
        title={intl.formatMessage({
            id: 'pages.table.user.addPermissions',
        })}
        formRef={formRef}
        onFinish={
            async () => {
                const permissionGrantInfos: API.PermissionGrantInfoModel[] = [];
                permissionList?.groups?.forEach(g => {
                    g.permissions?.forEach(p => {
                        if (g.name && selectedPermissionKeys && selectedPermissionKeys[g.name].find(sk => sk.toString() === p.name)) {
                            permissionGrantInfos.push({ name: p.name, isGranted: true });
                        }
                        else {
                            permissionGrantInfos.push({ name: p.name, isGranted: false });
                        }
                    });
                });

                const updateModel: API.PermissionUpdateRequestModel = {
                    permissionGrantInfos: permissionGrantInfos,
                    resourceGroupId: selectedResourceGroupId != '0' ? selectedResourceGroupId : undefined,
                    providerInfos: selectedProviderInfos,
                };

                await update(updateModel);
                props.onCancel();
            }
        }
        onVisibleChange={visible => {
            if (!visible) {
                props.onCancel();
            }
        }}
        visible={props.visible}
        submitter={{
            searchConfig: {
                submitText: context.locale?.Modal?.okText,
                resetText: context.locale?.Modal?.cancelText,
            },
            render: (props, doms) => {
                return (
                    <Space style={{ width: '100%' }}>
                        {doms.reverse()}
                    </Space>
                )
            },
        }}
    >
        <ProFormSelect
            name='resourceGroup'
            label={intl.formatMessage({ id: 'identity.components.resourceGroup' })}
            rules={[{ required: true }]}
            request={async ({ keyWords }) => {
                const parameter = { pageSize: 100, keyword: keyWords };
                let result = await getResourceGroups(parameter);
                let permissionList: any[] = [{ label: intl.formatMessage({ id: 'identity.components.allResource' }), value: '0' }];
                result.items?.forEach(item => {
                    if (item.name && item.id) {
                        let text = item.name;
                        if (item.displayName) {
                            text += `-${item.displayName}`;
                        }
                        permissionList.push({ label: text, value: item.id });
                    }
                });
                return permissionList;
            }}
            fieldProps={{
                onChange: async selectedValue => {
                    setSelectedResourceGroupId(selectedValue);
                },
            }}
        />
        <ProFormSelect
            name='principal'
            label={intl.formatMessage({ id: 'identity.components.principal' })}
            request={async ({ keyWords }) => {
                let permissionList: any = [];
                const parameter = { pageSize: 100, keyword: keyWords };
                let rolesResult = await getRoles(parameter);
                if (rolesResult.items && rolesResult.items.length) {
                    permissionList.push({ label: intl.formatMessage({ id: 'identity.components.principal.role' }), optionType: 'optGroup', value: 'roleList' });
                }
                rolesResult.items?.forEach(item => {
                    if (item.name && item.id) {
                        let text = `${intl.formatMessage({ id: 'identity.components.principal.role' })}-${item.tenantRoleName}`;
                        if (item.displayName) {
                            text += `-${item.displayName}`;
                        }
                        permissionList.push({ label: text, value: `Role:${item.name}` });
                    }
                });

                let usersResult = await getUsers(parameter);
                if (usersResult.items && usersResult.items.length) {
                    permissionList.push({ label: intl.formatMessage({ id: 'identity.components.principal.user' }), optionType: 'optGroup', value: 'userList' });
                }
                usersResult.items?.forEach(item => {
                    if (item.userName && item.id) {
                        let text = `${intl.formatMessage({ id: 'identity.components.principal.user' })}-${item.tenantUserName}`;
                        if (item.displayName) {
                            text += `-${item.displayName}`;
                        }
                        permissionList.push({ label: text, value: `User:${item.id}` });
                    }
                });
                return permissionList;
            }}
            fieldProps={{
                mode: 'multiple',
                onChange: selectedValues => {
                    const providerInfos: API.PermissionProviderInfoModel[] = [];
                    selectedValues.forEach((element: string) => {
                        const separatingStrings = element.split(':');
                        providerInfos.push({ providerName: separatingStrings[0], providerKey: separatingStrings[1] });
                    });
                    setSelectedProviderInfos(providerInfos);
                },
            }}
            rules={[
                { required: true, type: 'array' },
            ]}
        />
        <Card title={intl.formatMessage({ id: 'identity.components.permissionPolicy' })} size="small">
            <Tabs tabPosition='left'>
                {
                    permissionList?.groups?.map((g, i) => {
                        return <TabPane tab={g.displayName} key={g.name}>
                            <Tree
                                key={g.name}
                                checkable
                                defaultExpandAll
                                checkedKeys={g.name ? selectedPermissionKeys[g.name] : []}
                                onCheck={(checked, info) => {
                                    let keys = checked as React.Key[];
                                    if (!keys) {
                                        keys = (checked as { checked: Key[]; halfChecked: Key[]; }).checked;
                                    }
                                    if (info?.halfCheckedKeys) {
                                        keys = [...keys];
                                    }
                                    if (g.name) {
                                        let selectedKeys = { ...selectedPermissionKeys };
                                        selectedKeys[g.name] = keys.map(p => p.toString());
                                        setSelectedPermissionKeys(selectedKeys);
                                    }
                                }}
                                treeData={treeData(g)}
                            />
                        </TabPane>
                    })
                }
            </Tabs>
        </Card>
    </DrawerForm>);
}

export default AddPermissions;