import { Badge, Button, Drawer, FormInstance, message, Popconfirm, Space } from 'antd';
import type { ActionType, ProColumns } from '@ant-design/pro-table';
import ProTable from '@ant-design/pro-table';
import { PlusOutlined } from '@ant-design/icons';
import { FooterToolbar, PageContainer } from '@ant-design/pro-layout';
import { useIntl, FormattedMessage } from 'umi';
import { deleteUser, getUser, getUsers, postUser, putUser } from '@/services/identityServer/Users';
import { useRef, useState } from 'react';
import ProDescriptions, { ProDescriptionsItemProps } from '@ant-design/pro-descriptions';
import { ModalForm, ProFormText } from '@ant-design/pro-form';
import UpdateForm from './components/UpdateForm';
import AddToRoles from './components/AddToRoles';
import AddPermissions from '../components/AddPermissions';

export default () => {
    const [createModalVisible, setCreateModalVisible] = useState<boolean>(false);
    const [updateModalVisible, setUpdateModalVisible] = useState<boolean>(false);
    const [addToRolesVisible, setAddToRolesVisible] = useState<boolean>(false);
    const [addPermissionsVisible, setAddPermissionsVisible] = useState<boolean>(false);
    const [currentRow, setCurrentRow] = useState<API.UserGetResponseModel>();
    const [selectedRows, setSelectedRows] = useState<API.UserGetResponseModel[]>([]);
    const [showDetail, setShowDetail] = useState<boolean>(false);
    const intl = useIntl();
    const actionRef = useRef<ActionType>();
    const createFormRef = useRef<FormInstance>();

    const handleRemove = async (selectedRows: API.UserGetResponseModel[]) => {
        const hide = message.loading(intl.formatMessage({ id: 'pages.table.processing' }));
        if (!selectedRows) return true;
        try {
            await Promise.all(selectedRows.map(async (value) => {
                if (value.id) {
                    await deleteUser({ id: value.id });
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

    const columns: ProColumns<API.UserGetResponseModel>[] = [
        {
            title: <FormattedMessage id='pages.table.user.id' />,
            dataIndex: 'id',
            valueType: 'text',
            sorter: true,
            search: false,
            hideInTable: true,
        },
        {
            title: <FormattedMessage id='pages.table.user.tenantUserName' />,
            dataIndex: 'tenantUserName',
            valueType: 'text',
            sorter: true,
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
            title: <FormattedMessage id='pages.table.user.userName' />,
            dataIndex: 'userName',
            valueType: 'text',
            sorter: true,
            search: false,
        },
        {
            title: <FormattedMessage id='pages.table.user.phoneNumber' />,
            dataIndex: 'phoneNumber',
            valueType: 'text',
            search: { transform: (value) => 'keyword' }
        },
        {
            title: <FormattedMessage id='pages.table.user.displayName' />,
            dataIndex: 'displayName',
            valueType: 'text',
            sorter: true,
            search: { transform: (value) => 'keyword' }
        },
        {
            title: <FormattedMessage id='pages.table.user.lockoutEnd' />,
            dataIndex: 'lockoutEnd',
            valueType: 'text',
            render: (_, item) => {
                if (item.lockoutEnd) {
                    return <Badge status='error' text='禁用' />;
                }
                return <Badge status='success' text='正常' />;
            },
            search: false,
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
                    key={`addToRoles${record.id}`}
                    onClick={async () => {
                        setAddToRolesVisible(true);
                        setCurrentRow(record);
                    }}
                >
                    <FormattedMessage id="pages.table.user.addToRoles" />
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

                            await deleteUser({ id: record.id });
                            hide();
                            message.success(intl.formatMessage({ id: 'pages.table.successful' }));
                            actionRef.current?.reload();
                        }
                    }}
                    okButtonProps={{}}
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
            <ProTable<API.UserGetResponseModel>
                columns={columns}
                rowKey='id'
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
                    let result = await getUsers(parameter, { skipErrorHandler: true });
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
                search={false}// {labelWidth: 'auto',}}
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
                {currentRow?.userName && (
                    <ProDescriptions<API.UserGetResponseModel>
                        column={1}
                        title={currentRow?.tenantUserName}
                        request={async () => {
                            if (currentRow.id) {
                                return { data: await getUser({ id: currentRow.id }) };
                            }
                            return { data: currentRow || {}, };
                        }}
                        params={{
                            id: currentRow?.id,
                        }}
                        columns={columns as ProDescriptionsItemProps<API.UserGetResponseModel>[]}
                    />
                )}
            </Drawer>
            <ModalForm<API.UserCreateRequestModel>
                autoComplete="off"
                visible={createModalVisible}
                onVisibleChange={setCreateModalVisible}
                title={intl.formatMessage({
                    id: 'pages.table.tenant.createForm.title',
                })}
                formRef={createFormRef}
                modalProps={{
                    onCancel: () => console.log('run'),
                }}
                onFinish={async (value) => {
                    const result = await postUser(value, { skipErrorHandler: true });
                    if (result) {
                        setCreateModalVisible(false);
                        createFormRef.current?.resetFields();
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
                    name="userName"
                    label={intl.formatMessage({ id: 'pages.table.user.tenantUserName' })}
                    rules={[
                        {
                            type: 'string',
                            required: true,
                            min: 6,
                            max: 20,
                        },
                        {
                            pattern: RegExp('^[a-z]+$'),
                            message: intl.formatMessage({ id: 'pages.table.user.invalidUserName' }),
                        }
                    ]}
                />
                <ProFormText
                    name="displayName"
                    label={intl.formatMessage({ id: 'pages.table.user.displayName' })}
                    rules={[
                        {
                            type: 'string',
                            required: true,
                            min: 6,
                            max: 20,
                        },
                    ]}
                />
                <ProFormText
                    name="phoneNumber"
                    label={intl.formatMessage({ id: 'pages.table.user.phoneNumber' })}
                    rules={[
                        {
                            type: 'string',
                            required: true,
                            len: 11,
                        },
                        {
                            type: 'string',
                            pattern: RegExp('^1\\d{10}$'),
                            message: intl.formatMessage({ id: 'pages.table.user.invalidPhoneNumber' }),
                        },
                    ]}
                    fieldProps={{ autoComplete: "new-phoneNumber" }}
                />
                <ProFormText.Password
                    name="password"
                    label={intl.formatMessage({ id: 'pages.table.user.password' })}
                    rules={[
                        {
                            type: 'string',
                            required: true,
                            min: 5,
                            max: 15,
                        },
                    ]}
                    fieldProps={{ autoComplete: "new-password" }}
                />
            </ModalForm>
            <UpdateForm
                onSubmit={async (formData) => {
                    if (currentRow?.id) {
                        await putUser({ id: currentRow.id }, formData);
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
            <AddToRoles
                visible={addToRolesVisible}
                initialValues={currentRow || {}}
                onSubmit={async () => {
                    if (currentRow?.id) {
                        setUpdateModalVisible(false);
                        setCurrentRow(undefined);
                    }
                }}
                onCancel={() => {
                    setAddToRolesVisible(false);
                    setCurrentRow(undefined);
                }}
            />
            <AddPermissions
                visible={addPermissionsVisible}
                providerKey={currentRow?.id?.toString() || ''}
                providerName='User'
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