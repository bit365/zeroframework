import { ConfigProvider, FormInstance, Space, Transfer } from 'antd';
import { useIntl } from 'umi';
import { DrawerForm, } from '@ant-design/pro-form';
import React, { useContext, useEffect, useRef, useState } from 'react';
import { getRoles } from '@/services/identityServer/Roles';
import { getUserRoles, updateUserRoles } from '@/services/identityServer/UserRoles';

export type AddToRolesProps = {
    onCancel: () => void;
    onSubmit: () => Promise<void>;
    visible: boolean;
    initialValues: Partial<API.UserGetResponseModel>;
};

const AddToRoles: React.FC<AddToRolesProps> = (props) => {

    const [dataSource, setDataSource] = useState<API.RoleGetResponseModel[]>();
    const formRef = useRef<FormInstance>();
    const [targetKeys, setTargetKeys] = useState<string[]>();

    const intl = useIntl();

    useEffect(() => {
        const fetchUserRoles = async () => {
            if (props.initialValues.id) {
                const userRoles = await getUserRoles({ userId: props.initialValues.id.toString() });
                setTargetKeys(userRoles);
            }
        };

        const fetchRoles = async () => {
            const roles = await getRoles({ pageSize: 100 });
            setDataSource(roles.items);
            await fetchUserRoles();
        };

        if (props.visible) {
            fetchRoles();
        }
        else {
            setTargetKeys([]);
        }
    }, [props.visible]);

    const context = useContext(ConfigProvider.ConfigContext);

    return (<DrawerForm
        title={intl.formatMessage({
            id: 'pages.table.user.addToRoles',
        })}
        formRef={formRef}
        onFinish={
            async () => {
                if (props.initialValues.id && targetKeys) {
                    await updateUserRoles({ userId: props.initialValues.id.toString() }, targetKeys);
                }
                props.onCancel();
            }
        }
        onVisibleChange={visible => {
            if (!visible) {
                props.onCancel();
            }
        }}
        visible={props.visible}
        initialValues={props.initialValues}
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
        <Transfer<API.RoleGetResponseModel>
            dataSource={dataSource}
            showSearch
            rowKey={e => e.name!}
            filterOption={(inputValue, option) => option.tenantRoleName!.toLowerCase().indexOf(inputValue) > -1}
            targetKeys={targetKeys}
            onSearch={(direction, value) => console.info(value)}
            render={item => item.tenantRoleName!}
            listStyle={{ width: 400 }}
            onChange={targetKeys => {
                setTargetKeys(targetKeys);
            }}
        />
    </DrawerForm>
    );
}

export default AddToRoles;