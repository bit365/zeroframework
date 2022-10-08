import { Button, Drawer, FormInstance, message, Popconfirm, Space } from 'antd';
import type { ActionType, ProColumns } from '@ant-design/pro-table';
import ProTable from '@ant-design/pro-table';
import { PlusOutlined } from '@ant-design/icons';
import { FooterToolbar, PageContainer } from '@ant-design/pro-layout';
import { useIntl, FormattedMessage } from 'umi';
import { deleteRole, getRole, getRoles, postRole, putRole } from '@/services/identityServer/Roles';
import { useRef, useState } from 'react';
import ProDescriptions, { ProDescriptionsItemProps } from '@ant-design/pro-descriptions';
import { ModalForm, ProFormText } from '@ant-design/pro-form';
import AddPermissions from '../components/AddPermissions';
import UpdateForm from './components/UpdateForm';

export default () => {
    const [createModalVisible, setCreateModalVisible] = useState<boolean>(false);
    const [updateModalVisible, setUpdateModalVisible] = useState<boolean>(false);
    const [addPermissionsVisible, setAddPermissionsVisible] = useState<boolean>(false);
    const [currentRow, setCurrentRow] = useState<API.RoleGetResponseModel>();
    const [selectedRows, setSelectedRows] = useState<API.RoleGetResponseModel[]>([]);
    const [showDetail, setShowDetail] = useState<boolean>(false);
    const intl = useIntl();
    const actionRef = useRef<ActionType>();
    const createModalRef = useRef<FormInstance>();

    const handleRemove = async (selectedRows: API.RoleGetResponseModel[]) => {
        const hide = message.loading(intl.formatMessage({ id: 'pages.table.processing' }));
        if (!selectedRows) return true;
        try {
            await Promise.all(selectedRows.map(async (value) => {
                if (value.id) {
                    await deleteRole({ id: value.id });
                }
            }));

            hide();
            message.success(intl.formatMessage({ id: 'pages.table.successful' }));
            actionRef.current?.reload();
            return true;
        } catch (error) {
            hide();
            message.error(intl.formatMessage({ id: 'pages.table.failed' }));
            return false;
        }
    };

    const columns: ProColumns<API.RoleGetResponseModel>[] = [
        {
            title: <FormattedMessage id='pages.table.role.id' />,
            dataIndex: 'id',
            valueType: 'text',
            sorter: { multiple: 1 },
            search: false,
            hideInTable: true,
        },
        {
            title: <FormattedMessage id='pages.table.role.tenantRoleName' />,
            dataIndex: 'tenantRoleName',
            valueType: 'text',
            sorter: { multiple: 2 },
            search: { transform: (value) => 'keyword' },
            render: (dom, entity) => {
                return (
                    <a
                        onClick={() => {
                            setCurrentRow(entity);
                            setShowDetail(true);
                        }}
                    >
                        {dom}
                    </a>
                );
            },
        },
        {
            title: <FormattedMessage id='pages.table.role.name' />,
            dataIndex: 'name',
            valueType: 'text',
            sorter: false,
        },
        {
            title: <FormattedMessage id='pages.table.role.displayName' />,
            dataIndex: 'displayName',
            valueType: 'text',
            sorter: { multiple: 2 },
            search: { transform: (value) => 'keyword' },
        },
        {
            title: <FormattedMessage id='pages.table.creationTime' />,
            dataIndex: 'creationTime',
            valueType: 'dateTime',
        },
        {
            title: <FormattedMessage id="pages.searchTable.titleOption" />,
            dataIndex: 'option',
            valueType: 'option',
            render: (_, record) => [
                <a
                    key={`edit${record.id}`}
                    onClick={async () => {
                        setUpdateModalVisible(true);
                        setCurrentRow(record);
                    }}
                >
                    <FormattedMessage id="pages.table.edit" />
                </a>,
                <a
                    key={`addPermissions${record.id}`}
                    onClick={async () => {
                        setAddPermissionsVisible(true);
                        setCurrentRow(record);
                    }}
                >
                    <FormattedMessage id="pages.table.user.addPermissions" />
                </a>,
                <Popconfirm
                    key={record.id}
                    title={<FormattedMessage id="pages.table.deleteConfirm" />}
                    onConfirm={async () => {
                        const hide = message.loading(intl.formatMessage({ id: 'pages.table.processing' }));
                        if (record.id) {

                            await deleteRole({ id: record.id });
                            hide();
                            message.success(intl.formatMessage({ id: 'pages.table.successful' }));
                            actionRef.current?.reload();
                        }
                    }}
                >
                    <a href="#">
                        <FormattedMessage id="pages.table.delete" />
                    </a>
                </Popconfirm>,
            ],
        },
    ];

    return (
        <PageContainer>
            <ProTable<API.RoleGetResponseModel>
                columns={columns}
                rowKey='id'
                search={false}
                actionRef={actionRef}
                pagination={{
                    showSizeChanger: true,
                    showQuickJumper: true,
                }}
                request={async (params, sort, filter) => {
                    params = Object.assign(params, {
                        sorter: sort,
                        pageNumber: params.current
                    });
                    const { current, ...parameter } = params
                    let result = await getRoles(parameter);
                    return {
                        data: result.items,
                        total: result.totalCount,
                    }
                }}
                dateFormatter="string"
                headerTitle={intl.formatMessage({
                    id: 'pages.searchTable.title'
                })}
                toolBarRender={() => [
                    <Button
                        type="primary"
                        key="primary"
                        onClick={() => {
                            setCreateModalVisible(true);
                        }}
                    >
                        <PlusOutlined />
                        <FormattedMessage id="pages.searchTable.new" />
                    </Button>,
                ]}
                options={{
                    search: true,
                }}
                rowSelection={{
                    onChange: (_, selectedRows) => {
                        setSelectedRows(selectedRows);
                    },
                }}
            />
            {selectedRows?.length > 0 && (
                <FooterToolbar
                    extra={
                        <div>
                            <FormattedMessage id="pages.searchTable.chosen" defaultMessage="已选择" />{' '}
                            <a style={{ fontWeight: 600 }}>{selectedRows.length}</a>{' '}
                            <FormattedMessage id="pages.searchTable.item" defaultMessage="项" />
                        </div>
                    }
                >
                    <Button type="primary"
                        onClick={async () => {
                            await handleRemove(selectedRows);
                            setSelectedRows([]);
                            actionRef.current?.reloadAndRest?.();
                        }}
                    >
                        <FormattedMessage id="pages.searchTable.batchDeletion" />
                    </Button>
                </FooterToolbar>
            )}
            <Drawer
                width={400}
                visible={showDetail}
                onClose={() => {
                    setCurrentRow(undefined);
                    setShowDetail(false);
                }}
                closable={false}
            >
                {currentRow?.name && (
                    <ProDescriptions<API.RoleGetResponseModel>
                        column={1}
                        title={currentRow?.tenantRoleName}
                        request={async () => {
                            if (currentRow.id) {
                                return { data: await getRole({ id: currentRow.id }) };
                            }
                            return { data: currentRow || {}, };
                        }}
                        params={{
                            id: currentRow?.id,
                        }}
                        columns={columns as ProDescriptionsItemProps<API.RoleGetResponseModel>[]}
                    />
                )}
            </Drawer>
            <ModalForm<API.RoleCreateRequestModel>
                autoComplete="off"
                visible={createModalVisible}
                onVisibleChange={setCreateModalVisible}
                title={intl.formatMessage({
                    id: 'pages.table.role.createForm.title',
                })}
                formRef={createModalRef}
                onFinish={async (value) => {
                    const result = await postRole(value as API.RoleCreateRequestModel, { skipErrorHandler: true });
                    if (result) {
                        setCreateModalVisible(false);
                        createModalRef.current?.resetFields();
                        actionRef.current?.reload();
                        return true;
                    }
                    return false;
                }}
                submitter={{
                    render: (props, doms) => {
                        return (
                            <Space style={{
                                width: '100%',
                                display: 'flex',
                                justifyContent: 'flex-end',
                            }}>
                                {doms.reverse()}
                            </Space>
                        )
                    },
                }}
            >
                <ProFormText
                    name="roleName"
                    label={intl.formatMessage({ id: 'pages.table.role.tenantRoleName' })}
                    rules={[
                        {
                            type: 'string',
                            required: true,
                            min: 6,
                            max: 20,
                        },
                        {
                            pattern: RegExp('^[a-z]+$'),
                            message: intl.formatMessage({ id: 'pages.table.role.invalidRoleName' }),
                        }
                    ]}
                />
                <ProFormText
                    name="displayName"
                    label={intl.formatMessage({ id: 'pages.table.role.displayName' })}
                    rules={[
                        {
                            type: 'string',
                            required: true,
                            min: 6,
                            max: 20,
                        },
                    ]}
                />
            </ModalForm>
            <UpdateForm
                onSubmit={async (formData) => {
                    if (currentRow?.id) {
                        await putRole({ id: currentRow.id }, formData);
                        setUpdateModalVisible(false);
                        setCurrentRow(undefined);
                        if (actionRef.current) {
                            actionRef.current.reload();
                        }
                    }
                }}
                onCancel={() => {
                    setUpdateModalVisible(false);
                    setCurrentRow(undefined);
                }}
                visible={updateModalVisible}
                initialValues={currentRow || {}}
            />
            <AddPermissions
                visible={addPermissionsVisible}
                providerKey={currentRow?.name?.toString() || ''}
                providerName='Role'
                onSubmit={async () => {
                    if (currentRow?.id) {
                        setUpdateModalVisible(false);
                        setCurrentRow(undefined);
                    }
                }}
                onCancel={() => {
                    setAddPermissionsVisible(false);
                    setCurrentRow(undefined);
                }}
            />
        </PageContainer>
    );
};