import { Button, Drawer, FormInstance, message, Popconfirm, Space } from 'antd';
import type { ActionType, ProColumns } from '@ant-design/pro-table';
import ProTable from '@ant-design/pro-table';
import { PlusOutlined } from '@ant-design/icons';
import { FooterToolbar, PageContainer } from '@ant-design/pro-layout';
import { useIntl, FormattedMessage } from 'umi';
import { deleteTenant, getTenants, postTenant, putTenant } from '@/services/identityServer/Tenants';
import { ModalForm, ProFormText, ProFormSwitch, } from '@ant-design/pro-form';
import { useRef, useState } from 'react';
import UpdateForm from './components/UpdateForm';
import ProDescriptions, { ProDescriptionsItemProps } from '@ant-design/pro-descriptions';
import styles from '../components/index.less';

export default () => {
    const [createModalVisible, setCreateModalVisible] = useState<boolean>(false);
    const [updateModalVisible, setUpdateModalVisible] = useState<boolean>(false);
    const [currentRow, setCurrentRow] = useState<API.TenantGetResponseModel>();
    const [connectionStringHidden, setConnectionStringHidden] = useState<boolean>(true);
    const [selectedRows, setSelectedRows] = useState<API.TenantGetResponseModel[]>([]);
    const [showDetail, setShowDetail] = useState<boolean>(false);
    const intl = useIntl();
    const actionRef = useRef<ActionType>();
    const createModalRef = useRef<FormInstance>();

    const handleRemove = async (selectedRows: API.TenantGetResponseModel[]) => {
        const hide = message.loading(intl.formatMessage({ id: 'pages.table.processing' }));
        if (!selectedRows) return true;
        try {
            await Promise.all(selectedRows.map(async (value) => {
                if (value.id) {
                    await deleteTenant({ id: value.id });
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

    const columns: ProColumns<API.TenantGetResponseModel>[] = [
        {
            title: <FormattedMessage id='pages.table.tenant.id' />,
            dataIndex: 'id',
            valueType: 'text',
            hideInTable: true,
        },
        {
            title: <FormattedMessage id='pages.table.tenant.tenantName' />,
            dataIndex: 'name',
            valueType: 'text',
            sorter: true,
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
            title: <FormattedMessage id='pages.table.tenant.displayName' />,
            dataIndex: 'displayName',
            valueType: 'text',
        },
        {
            title: <FormattedMessage id='pages.table.creationTime' />,
            dataIndex: 'creationTime',
            valueType: 'dateTime',
            sorter: true,
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
                <Popconfirm
                    key={`del${record.id}`}
                    title={<FormattedMessage id="pages.table.deleteConfirm" />}
                    onConfirm={async () => {
                        const hide = message.loading(intl.formatMessage({ id: 'pages.table.processing' }));
                        if (record.id) {

                            await deleteTenant({ id: record.id });
                            hide();
                            message.success(intl.formatMessage({ id: 'pages.table.successful' }));
                            actionRef.current?.reload();
                        }
                    }}
                    overlayClassName={styles.popconfirm}
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
            <ProTable<API.TenantGetResponseModel>
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
                    let result = await getTenants(parameter);
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
            <ModalForm
                visible={createModalVisible}
                onVisibleChange={setCreateModalVisible}
                title={intl.formatMessage({
                    id: 'pages.table.tenant.createForm.title',
                })}
                formRef={createModalRef}
                onFinish={async (value) => {
                    const result = await postTenant(value as API.TenantCreateRequestModel, { skipErrorHandler: true });
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
                    name="name"
                    label={intl.formatMessage({ id: 'pages.table.tenant.tenantName' })}
                    tooltip="租户名称只能是小写字母"
                    rules={[
                        {
                            type: 'string',
                            required: true,
                            min: 5,
                            max: 15,
                        },
                        {
                            pattern: RegExp('^[a-z]+$'),
                            message: intl.formatMessage({ id: 'pages.table.tenant.invalidTenantName' }),
                        }
                    ]}
                />
                <ProFormText
                    name="displayName"
                    label={intl.formatMessage({ id: 'pages.table.tenant.displayName' })}
                    rules={[
                        {
                            type: 'string',
                            required: true,
                            min: 5,
                            max: 15,
                        },
                    ]}
                />
                <ProFormText
                    name="adminUserName"
                    label={intl.formatMessage({ id: 'pages.table.tenant.adminUserName' })}
                    rules={[
                        {
                            type: 'string',
                            required: true,
                            min: 5,
                            max: 15,
                        },
                    ]}
                />
                <ProFormText.Password
                    name="adminPassword"
                    label={intl.formatMessage({ id: 'pages.table.tenant.adminPassword' })}
                    rules={[
                        {
                            type: 'string',
                            required: true,
                            min: 5,
                            max: 15,
                        },
                    ]}
                />
                <ProFormSwitch
                    name="useIndependentDatabase"
                    initialValue={!connectionStringHidden}
                    label={intl.formatMessage({ id: 'pages.table.tenant.useIndependentDatabase' })}
                    fieldProps={{
                        onChange: checked => setConnectionStringHidden(!checked),
                    }}
                />
                <ProFormText
                    hidden={connectionStringHidden}
                    name="connectionString"
                    label={intl.formatMessage({ id: 'pages.table.tenant.connectionString' })}
                    rules={[
                        {
                            type: 'string',
                            required: false,
                            min: 30,
                        },
                    ]}
                />
            </ModalForm>
            <UpdateForm
                onSubmit={async (formData) => {
                    if (currentRow && currentRow.id) {
                        await putTenant({ id: currentRow.id }, formData);
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
                    <ProDescriptions<API.TenantGetResponseModel>
                        column={1}
                        title={currentRow?.name}
                        request={async () => ({
                            data: currentRow || {},
                        })}
                        params={{
                            id: currentRow?.id,
                        }}
                        columns={columns as ProDescriptionsItemProps<API.TenantGetResponseModel>[]}
                    />
                )}
            </Drawer>
        </PageContainer>
    );
};