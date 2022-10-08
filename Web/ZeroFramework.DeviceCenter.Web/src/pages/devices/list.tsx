import { Badge, Button, Card, Col, Drawer, FormInstance, message, Popconfirm, Row, Space, Statistic } from 'antd';
import type { ActionType, ProColumns } from '@ant-design/pro-table';
import ProTable from '@ant-design/pro-table';
import { PlusOutlined } from '@ant-design/icons';
import { FooterToolbar, PageContainer } from '@ant-design/pro-layout';
import { useIntl, FormattedMessage, Link } from 'umi';
import { deleteDevice, getDevice, getDevices, getStatistic, postDevice, putDevice } from '@/services/deviceCenter/Devices';
import { useEffect, useRef, useState } from 'react';
import ProDescriptions, { ProDescriptionsItemProps } from '@ant-design/pro-descriptions';
import { ModalForm, ProFormSelect, ProFormText, ProFormTextArea } from '@ant-design/pro-form';
import UpdateForm from './components/UpdateForm';
import { getProducts } from '@/services/deviceCenter/Products';

export default (props: any) => {
    const [createModalVisible, setCreateModalVisible] = useState<boolean>(false);
    const [updateModalVisible, setUpdateModalVisible] = useState<boolean>(false);
    const [currentRow, setCurrentRow] = useState<API.DeviceGetResponseModel>();
    const [selectedRows, setSelectedRows] = useState<API.DeviceGetResponseModel[]>([]);
    const [showDetail, setShowDetail] = useState<boolean>(false);
    const intl = useIntl();
    const tableActionRef = useRef<ActionType>();
    const createFormRef = useRef<FormInstance>();
    const [deviceStatistic, setDeviceStatistic] = useState<API.DeviceStatisticGetResponseModel>();

    const fetchStatisticApi = async () => {
        let result = await getStatistic({});
        setDeviceStatistic(result);
    }

    useEffect(() => {
        fetchStatisticApi();
    }, []);

    const handleRemove = async (selectedRows: API.DeviceGetResponseModel[]) => {
        const hide = message.loading(intl.formatMessage({ id: 'pages.table.processing' }));
        if (!selectedRows) return true;
        try {
            await Promise.all(selectedRows.map(async (value) => {
                if (value.id) {
                    await deleteDevice({ id: value.id });
                }
            }));
            hide();
            message.success(intl.formatMessage({ id: 'pages.table.successful' }));
            tableActionRef.current?.reload();
            await fetchStatisticApi();
            return true;
        } catch (error) {
            hide();
            message.error(intl.formatMessage({ id: 'pages.table.failed' }));
            return false;
        }
    };

    const columns: ProColumns<API.DeviceGetResponseModel>[] = [
        {
            title: <FormattedMessage id='pages.table.device.id' />,
            dataIndex: 'id',
            valueType: 'text',
            sorter: true,
            search: false,
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
            title: <FormattedMessage id='pages.table.device.name' />,
            dataIndex: 'name',
            valueType: 'text',
            sorter: true,
        },
        {
            title: <FormattedMessage id='pages.table.device.product' />,
            dataIndex: 'productName',
            valueType: 'text',
            sorter: false,
            renderFormItem: (_, { type, defaultRender }, form) => {
                return (<ProFormSelect<API.MeasurementUnitGetResponseModel>
                    name='productId'
                    key='productIdSearch'
                    showSearch
                    request={async ({ keyWords }: any) => {
                        const parameter = { pageSize: 20, keyword: keyWords };
                        let result = await getProducts(parameter);
                        let list: any[] = [];
                        result.items?.forEach(item => {
                            if (item.name && item.id) {
                                list.push({ label: item.name, value: item.id });
                            }
                        });
                        return list;
                    }}
                    initialValue={props.location.state?.id}
                />)
            },
            render: (_, entity) => entity.product?.name,
        },
        {
            title: <FormattedMessage id='pages.table.product.nodeType' />,
            dataIndex: 'nodeType',
            valueType: 'text',
            sorter: false,
            search: false,
            render: (dom, entity) => {
                switch (entity.product?.nodeType) {
                    case 'direct':
                        return intl.formatMessage({ id: 'pages.table.product.directlyDevice' });
                    case 'gateway':
                        return intl.formatMessage({ id: 'pages.table.product.gatewayDevice' });
                    case 'subset':
                        return intl.formatMessage({ id: 'pages.table.product.gatewaySubDevice' });
                    default:
                        return null;
                }
            }
        },
        {
            title: <FormattedMessage id='pages.table.device.status' />,
            dataIndex: 'status',
            valueType: 'text',
            sorter: false,
            valueEnum: {
                unactive: { text: intl.formatMessage({ id: 'pages.table.device.status.unactive' }), status: 'Processing' },
                online: { text: intl.formatMessage({ id: 'pages.table.device.status.online' }), status: 'Success' },
                offline: { text: intl.formatMessage({ id: 'pages.table.device.status.offline' }), status: 'Default' },
            },
        },
        {
            title: <FormattedMessage id='pages.table.device.remark' />,
            dataIndex: 'remark',
            valueType: 'text',
            sorter: false,
            search: false,
            hideInTable: true,
        },
        {
            title: <FormattedMessage id='pages.table.device.lastOnlineTime' />,
            dataIndex: 'lastOnlineTime',
            valueType: 'dateTime',
            sorter: true,
            search: false,
        },
        {
            title: <FormattedMessage id='pages.table.device.creationTime' />,
            dataIndex: 'creationTime',
            valueType: 'dateTime',
            sorter: true,
            defaultSortOrder: 'descend',
            search: false,
        },
        {
            title: <FormattedMessage id="pages.searchTable.titleOption" />,
            dataIndex: 'option',
            valueType: 'option',
            render: (_, record) => [
                <Link
                    onClick={async () => {
                        setCurrentRow(record);
                    }}
                    key={`view${record.id}`}
                    to={{
                        pathname: '/device/device/view',
                        state: { ...record },
                    }}
                >
                    <FormattedMessage id="pages.table.view" />
                </Link>,
                <a
                    key={`edit${record.id}`}
                    onClick={async () => {
                        setCurrentRow(record);
                        setUpdateModalVisible(true);
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

                            await deleteDevice({ id: record.id });
                            await fetchStatisticApi();
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
            <Row gutter={24}>
                <Col span={6}>
                    <Card>
                        <Statistic
                            title={intl.formatMessage({ id: 'pages.table.device.statistic.totalCount' })}
                            value={deviceStatistic?.totalCount}
                            precision={0}
                            valueStyle={{ color: '#3f8600' }}
                            prefix={<Badge color="cyan" />}
                        />
                    </Card>
                </Col>
                <Col span={6}>
                    <Card>
                        <Statistic
                            title={intl.formatMessage({ id: 'pages.table.device.statistic.onlineCount' })}
                            value={deviceStatistic?.onlineCount}
                            precision={0}
                            valueStyle={{ color: '#3f8600' }}
                            prefix={<Badge status='success' />}
                        />
                    </Card>
                </Col>
                <Col span={6}>
                    <Card>
                        <Statistic
                            title={intl.formatMessage({ id: 'pages.table.device.statistic.offlineCount' })}
                            value={deviceStatistic?.offlineCount}
                            precision={0}
                            valueStyle={{ color: '#3f8600' }}
                            prefix={<Badge status='default' />}
                        />
                    </Card>
                </Col>
                <Col span={6}>
                    <Card>
                        <Statistic
                            title={intl.formatMessage({ id: 'pages.table.device.statistic.unactiveCount' })}
                            value={deviceStatistic?.unactiveCount}
                            precision={0}
                            valueStyle={{ color: '#3f8600' }}
                            prefix={<Badge color='blue' />}
                        />
                    </Card>
                </Col>
            </Row>
            <ProTable<API.DeviceGetResponseModel>
                style={{ marginTop: 10, }}
                columns={columns}
                rowKey='id'
                actionRef={tableActionRef}
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
                    let result = await getDevices(parameter);
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
                    search: false,
                    fullScreen: true,
                }}
                rowSelection={{
                    onChange: (_, selectedRows) => {
                        setSelectedRows(selectedRows);
                    },
                }}
                search={{ labelWidth: 'auto' }}
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
                    <ProDescriptions<API.DeviceGetResponseModel>
                        column={1}
                        title={currentRow?.id}
                        request={async () => {
                            if (currentRow.id) {
                                return { data: await getDevice({ id: currentRow.id }) };
                            }
                            return { data: currentRow || {}, };
                        }}
                        params={{
                            id: currentRow?.id,
                        }}
                        columns={columns as ProDescriptionsItemProps<API.DeviceGetResponseModel>[]}
                    />
                )}
            </Drawer>
            <ModalForm<API.DeviceCreateRequestModel>
                autoComplete="off"
                visible={createModalVisible}
                onVisibleChange={setCreateModalVisible}
                title={intl.formatMessage({
                    id: 'pages.table.device.createForm.title',
                })}
                formRef={createFormRef}
                onFinish={async (value: any) => {
                    const result = await postDevice(value as API.DeviceCreateRequestModel);
                    if (result) {
                        setCreateModalVisible(false);
                        createFormRef.current?.resetFields();
                        tableActionRef.current?.reload();
                        message.success(intl.formatMessage({ id: 'pages.table.successful' }));
                        await fetchStatisticApi();
                        return true;
                    }
                    message.error(intl.formatMessage({ id: 'pages.table.failed' }));
                    return false;
                }}
                submitter={{
                    render: (props: any, dom: JSX.Element[]) => {
                        return (
                            <Space style={{
                                width: '100%',
                                display: 'flex',
                                justifyContent: 'flex-end',
                            }}>
                                {dom.reverse()}
                            </Space>
                        )
                    },
                }}
            >
                <ProFormSelect<API.MeasurementUnitGetResponseModel>
                    name='productId'
                    label={intl.formatMessage({ id: 'pages.table.device.product' })}
                    showSearch
                    request={async ({ keyWords }: any) => {
                        const parameter = { pageSize: 20, keyword: keyWords };
                        let result = await getProducts(parameter);
                        let list: any[] = [];
                        result.items?.forEach(item => {
                            if (item.name && item.id) {
                                list.push({ label: item.name, value: item.id });
                            }
                        });
                        return list;
                    }}
                    rules={[{ required: true, }]}
                />
                <ProFormText
                    name="name"
                    label={intl.formatMessage({ id: 'pages.table.device.name' })}
                    rules={[
                        {
                            type: 'string',
                            required: true,
                            min: 5,
                            max: 20,
                        },
                    ]}
                />
                <ProFormText
                    name="coordinate"
                    label={intl.formatMessage({ id: 'pages.table.device.coordinate' })}
                    tooltip={{
                        title: <a href='https://lbs.amap.com/tools/picker' target='_blank' style={{ color: 'white' }}>Click Map Picker</a>,
                        color: 'cyan',
                    }}
                    rules={[
                        {
                            type: 'string',
                            required: true,
                            min: 6,
                            max: 30,
                        },
                        {
                            pattern: /^[-\+]?\d+(\.\d+)\,[-\+]?\d+(\.\d+)$/,
                            message: intl.formatMessage({ id: 'pages.table.device.invalidFormat' }),
                        },
                    ]}
                />
                <ProFormTextArea
                    name="remark"
                    label={intl.formatMessage({ id: 'pages.table.device.remark' })}
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
                        const result = await putDevice({ id: currentRow.id }, formData);
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