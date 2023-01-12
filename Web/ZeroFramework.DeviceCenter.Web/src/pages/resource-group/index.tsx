import type { FormInstance} from 'antd';
import { Button, Drawer, message, Popconfirm, Space } from 'antd';
import type { ActionType, ProColumns } from '@ant-design/pro-table';
import ProTable from '@ant-design/pro-table';
import { PlusOutlined } from '@ant-design/icons';
import { FooterToolbar, PageContainer } from '@ant-design/pro-layout';
import { useIntl, FormattedMessage } from 'umi';
import { deleteResourceGroup, getResourceGroup, getResourceGroups, postResourceGroup, putResourceGroup } from '@/services/deviceCenter/ResourceGroups';
import { useRef, useState } from 'react';
import type { ProDescriptionsItemProps } from '@ant-design/pro-descriptions';
import ProDescriptions from '@ant-design/pro-descriptions';
import { ModalForm, ProFormText } from '@ant-design/pro-form';
import UpdateForm from './components/UpdateForm';
import AddPermissions from '../identity/components/AddPermissions';

export default () => {
    const [createModalVisible, setCreateModalVisible] = useState<boolean>(false);
    const [updateModalVisible, setUpdateModalVisible] = useState<boolean>(false);
    const [addPermissionsVisible, setAddPermissionsVisible] = useState<boolean>(false);
    const [currentRow, setCurrentRow] = useState<API.ResourceGroupGetResponseModel>();
    const [selectedRows, setSelectedRows] = useState<API.ResourceGroupGetResponseModel[]>([]);
    const [showDetail, setShowDetail] = useState<boolean>(false);
    const intl = useIntl();
    const actionRef = useRef<ActionType>();
    const createModalRef = useRef<FormInstance>();

    // eslint-disable-next-line @typescript-eslint/no-shadow
    const handleRemove = async (selectedRows: API.ResourceGroupGetResponseModel[]) => {
        const hide = message.loading(intl.formatMessage({ id: 'pages.table.processing' }));
        if (!selectedRows) return true;
        try {
            await Promise.all(selectedRows.map(async (value) => {
                if (value.id) {
                    await deleteResourceGroup({ id: value.id });
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

    const columns: ProColumns<API.ResourceGroupGetResponseModel>[] = [
        {
            title: <FormattedMessage id='pages.table.resourceGroup.id' />,
            dataIndex: 'id',
            valueType: 'text',
            sorter: { multiple: 1 },
            search: false,
            hideInTable: true,
        },
        {
            title: <FormattedMessage id='pages.table.resourceGroup.name' />,
            dataIndex: 'name',
            valueType: 'text',
            sorter: { multiple: 2 },
            search: { transform: () => 'keyword' },
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
            title: <FormattedMessage id='pages.table.resourceGroup.displayName' />,
            dataIndex: 'displayName',
            valueType: 'text',
            sorter: { multiple: 2 },
            search: { transform: () => 'keyword' },
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

                            await deleteResourceGroup({ id: record.id });
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
            <ProTable<API.ResourceGroupGetResponseModel>
                columns={columns}
                rowKey='id'
                search={false}
                actionRef={actionRef}
                pagination={{
                    showSizeChanger: true,
                    showQuickJumper: true,
                }}
                request={async (params, sort) => {
                    // eslint-disable-next-line no-param-reassign
                    params = Object.assign(params, {
                        sorter: sort,
                        pageNumber: params.current
                    });
                    const { current, ...parameter } = params
                    const result = await getResourceGroups(parameter);
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
                    // eslint-disable-next-line @typescript-eslint/no-shadow
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
                    <ProDescriptions<API.ResourceGroupGetResponseModel>
                        column={1}
                        title={currentRow?.displayName || currentRow.name}
                        request={async () => {
                            if (currentRow.id) {
                                return { data: await getResourceGroup({ id: currentRow.id }) };
                            }
                            return { data: currentRow || {}, };
                        }}
                        params={{
                            id: currentRow?.id,
                        }}
                        columns={columns as ProDescriptionsItemProps<API.ResourceGroupGetResponseModel>[]}
                    />
                )}
            </Drawer>
            <ModalForm<API.ResourceGroupCreateRequestModel>
                autoComplete="off"
                visible={createModalVisible}
                onVisibleChange={setCreateModalVisible}
                title={intl.formatMessage({
                    id: 'pages.table.resourceGroup.createForm.title',
                })}
                formRef={createModalRef}
                onFinish={async (value) => {
                    const result = await postResourceGroup(value as API.ResourceGroupCreateRequestModel, { skipErrorHandler: true });
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
                    label={intl.formatMessage({ id: 'pages.table.resourceGroup.name' })}
                    rules={[
                        {
                            type: 'string',
                            required: true,
                            min: 6,
                            max: 20,
                        },
                        {
                            pattern: RegExp('^[a-z]+$'),
                            message: intl.formatMessage({ id: 'pages.table.resourceGroup.invalidGroupName' }),
                        }
                    ]}
                />
                <ProFormText
                    name="displayName"
                    label={intl.formatMessage({ id: 'pages.table.resourceGroup.displayName' })}
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
                        await putResourceGroup({ id: currentRow.id }, formData);
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
                resourceGroupId={currentRow?.id}
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