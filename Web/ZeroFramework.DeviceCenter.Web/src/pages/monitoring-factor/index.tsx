import type { FormInstance} from 'antd';
import { Button, Drawer, message, Popconfirm, Space } from 'antd';
import type { ActionType, ProColumns } from '@ant-design/pro-table';
import ProTable from '@ant-design/pro-table';
import { PlusOutlined } from '@ant-design/icons';
import { FooterToolbar, PageContainer } from '@ant-design/pro-layout';
import { useIntl, FormattedMessage } from 'umi';
import { deleteMonitoringFactor, getMonitoringFactor, getMonitoringFactors, postMonitoringFactor, putMonitoringFactor } from '@/services/deviceCenter/MonitoringFactors';
import { useRef, useState } from 'react';
import type { ProDescriptionsItemProps } from '@ant-design/pro-descriptions';
import ProDescriptions from '@ant-design/pro-descriptions';
import { ModalForm, ProFormDigit, ProFormText, ProFormTextArea } from '@ant-design/pro-form';
import UpdateForm from './components/UpdateForm';

export default () => {
    const [createModalVisible, setCreateModalVisible] = useState<boolean>(false);
    const [updateModalVisible, setUpdateModalVisible] = useState<boolean>(false);
    const [currentRow, setCurrentRow] = useState<API.MonitoringFactorGetResponseModel>();
    const [selectedRows, setSelectedRows] = useState<API.MonitoringFactorGetResponseModel[]>([]);
    const [showDetail, setShowDetail] = useState<boolean>(false);
    const intl = useIntl();
    const tableActionRef = useRef<ActionType>();
    const createFormRef = useRef<FormInstance>();

    // eslint-disable-next-line @typescript-eslint/no-shadow
    const handleRemove = async (selectedRows: API.MonitoringFactorGetResponseModel[]) => {
        const hide = message.loading(intl.formatMessage({ id: 'pages.table.processing' }));
        if (!selectedRows) return true;
        try {
            await Promise.all(selectedRows.map(async (value) => {
                if (value.id) {
                    await deleteMonitoringFactor({ id: value.id });
                }
            }));
            hide();
            message.success(intl.formatMessage({ id: 'pages.table.successful' }));
            tableActionRef.current?.reload();
            return true;
        } catch (error) {
            hide();
            message.error(intl.formatMessage({ id: 'pages.table.failed' }));
            return false;
        }
    };

    const columns: ProColumns<API.MonitoringFactorGetResponseModel>[] = [
        {
            title: <FormattedMessage id='pages.table.monitoringFactor.id' />,
            dataIndex: 'id',
            valueType: 'text',
            sorter: { multiple: 1 },
            search: false,
            hideInTable: true,
        },
        {
            title: <FormattedMessage id='pages.table.monitoringFactor.factorCode' />,
            dataIndex: 'factorCode',
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
            title: <FormattedMessage id='pages.table.monitoringFactor.chineseName' />,
            dataIndex: 'chineseName',
            valueType: 'text',
            sorter: { multiple: 2 },
            search: { transform: () => 'keyword' },
        },
        {
            title: <FormattedMessage id='pages.table.monitoringFactor.englishName' />,
            dataIndex: 'englishName',
            valueType: 'text',
            sorter: { multiple: 2 },
            search: { transform: () => 'keyword' },
        },
        {
            title: <FormattedMessage id='pages.table.monitoringFactor.unit' />,
            dataIndex: 'unit',
            valueType: 'text',
            sorter: false,
            search: { transform: () => 'keyword' },
        },
        {
            title: <FormattedMessage id='pages.table.monitoringFactor.decimals' />,
            dataIndex: 'decimals',
            valueType: 'text',
            sorter: false,
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
                <Popconfirm
                    key={record.id}
                    title={<FormattedMessage id="pages.table.deleteConfirm" />}
                    onConfirm={async () => {
                        const hide = message.loading(intl.formatMessage({ id: 'pages.table.processing' }));
                        if (record.id) {

                            await deleteMonitoringFactor({ id: record.id });
                            hide();
                            message.success(intl.formatMessage({ id: 'pages.table.successful' }));
                            tableActionRef.current?.reload();
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
            <ProTable<API.MonitoringFactorGetResponseModel>
                columns={columns}
                rowKey='id'
                search={false}
                actionRef={tableActionRef}
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
                    const result = await getMonitoringFactors(parameter);
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
                            tableActionRef.current?.reloadAndRest?.();
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
                {currentRow?.id && (
                    <ProDescriptions<API.MonitoringFactorGetResponseModel>
                        column={1}
                        title={currentRow?.chineseName || currentRow.englishName}
                        request={async () => {
                            if (currentRow.id) {
                                return { data: await getMonitoringFactor({ id: currentRow.id }) };
                            }
                            return { data: currentRow || {}, };
                        }}
                        params={{
                            id: currentRow?.id,
                        }}
                        columns={columns.concat({
                            title: <FormattedMessage id='pages.table.monitoringFactor.remarks' />,
                            dataIndex: 'remarks',
                            valueType: 'text',
                        }) as ProDescriptionsItemProps<API.MonitoringFactorGetResponseModel>[]}
                    />
                )}
            </Drawer>
            <ModalForm<API.MonitoringFactorCreateRequestModel>
                autoComplete="off"
                visible={createModalVisible}
                onVisibleChange={setCreateModalVisible}
                title={intl.formatMessage({
                    id: 'pages.table.monitoringFactor.createForm.title',
                })}
                formRef={createFormRef}
                onFinish={async (value) => {
                    const result = await postMonitoringFactor(value as API.MonitoringFactorCreateRequestModel, { skipErrorHandler: true });
                    if (result) {
                        setCreateModalVisible(false);
                        createFormRef.current?.resetFields();
                        tableActionRef.current?.reload();
                        message.success(intl.formatMessage({ id: 'pages.table.successful' }));
                        return true;
                    }
                    message.error(intl.formatMessage({ id: 'pages.table.failed' }));
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
                    name="factorCode"
                    label={intl.formatMessage({ id: 'pages.table.monitoringFactor.factorCode' })}
                    rules={[
                        {
                            type: 'string',
                            required: true,
                            len: 6
                        },
                        {
                            pattern: RegExp('^[a-z]\\d{5}$'),
                            message: intl.formatMessage({ id: 'pages.table.monitoringFactor.invalidFormat' }),
                        }
                    ]}
                />
                <ProFormText
                    name="chineseName"
                    label={intl.formatMessage({ id: 'pages.table.monitoringFactor.chineseName' })}
                    rules={[
                        {
                            type: 'string',
                            required: true,
                            min: 2,
                            max: 15,
                        },
                    ]}
                />
                <ProFormText
                    name="englishName"
                    label={intl.formatMessage({ id: 'pages.table.monitoringFactor.englishName' })}
                    rules={[
                        {
                            type: 'string',
                            required: true,
                            min: 2,
                            max: 15,
                        },
                    ]}
                />
                <ProFormDigit
                    name="decimals"
                    label={intl.formatMessage({ id: 'pages.table.monitoringFactor.decimals' })}
                    min={0}
                    max={4}
                    fieldProps={{ precision: 0 }}
                    initialValue={2}
                />
                <ProFormText
                    name="unit"
                    label={intl.formatMessage({ id: 'pages.table.monitoringFactor.unit' })}
                    rules={[
                        {
                            type: 'string',
                            required: false,
                            min: 0,
                            max: 10,
                        },
                    ]}
                />
                <ProFormTextArea
                    name="remarks"
                    label={intl.formatMessage({ id: 'pages.table.monitoringFactor.remarks' })}
                    rules={[
                        {
                            type: 'string',
                            required: false,
                            min: 2,
                            max: 100,
                        },
                    ]}
                />
            </ModalForm>
            <UpdateForm
                onSubmit={async (formData) => {
                    if (currentRow?.id) {
                        const result = await putMonitoringFactor({ id: currentRow.id }, formData);
                        if (result) {
                            message.success(intl.formatMessage({ id: 'pages.table.successful' }));
                            setUpdateModalVisible(false);
                            setCurrentRow(undefined);
                            tableActionRef.current?.reload();
                            return;
                        }
                        message.error(intl.formatMessage({ id: 'pages.table.failed' }));
                    }
                }}
                onCancel={() => {
                    setUpdateModalVisible(false);
                    setCurrentRow(undefined);
                }}
                visible={updateModalVisible}
                initialValues={currentRow || {}}
            />
        </PageContainer>
    );
};