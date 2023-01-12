import type { FormInstance, RadioChangeEvent} from 'antd';
import { Button, Drawer, Form, message, Select, Space } from 'antd';
import type { ActionType, ProColumns } from '@ant-design/pro-table';
import ProTable from '@ant-design/pro-table';
import { PlusOutlined } from '@ant-design/icons';
import { FooterToolbar, PageContainer } from '@ant-design/pro-layout';
import { useIntl, FormattedMessage, Link } from 'umi';
import { deleteProduct, getProduct, getProducts, postProduct, putProduct } from '@/services/deviceCenter/Products';
import { useRef, useState } from 'react';
import type { ProDescriptionsItemProps } from '@ant-design/pro-descriptions';
import ProDescriptions from '@ant-design/pro-descriptions';
import { ModalForm, ProFormRadio, ProFormSelect, ProFormText, ProFormTextArea } from '@ant-design/pro-form';
import UpdateForm from './components/UpdateForm';

const { Option } = Select;

export default () => {
    const [createModalVisible, setCreateModalVisible] = useState<boolean>(false);
    const [updateModalVisible, setUpdateModalVisible] = useState<boolean>(false);
    const [currentRow, setCurrentRow] = useState<API.ProductGetResponseModel>();
    const [selectedRows, setSelectedRows] = useState<API.ProductGetResponseModel[]>([]);
    const [showDetail, setShowDetail] = useState<boolean>(false);
    const intl = useIntl();
    const tableActionRef = useRef<ActionType>();
    const createFormRef = useRef<FormInstance>();

    const [protocolTypes, setProtocolTypes] = useState({});

    // eslint-disable-next-line @typescript-eslint/no-shadow
    const handleRemove = async (selectedRows: API.ProductGetResponseModel[]) => {
        const hide = message.loading(intl.formatMessage({ id: 'pages.table.processing' }));
        if (!selectedRows) return true;
        try {
            await Promise.all(selectedRows.map(async (value) => {
                if (value.id) {
                    await deleteProduct({ id: value.id });
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

    const columns: ProColumns<API.ProductGetResponseModel>[] = [
        {
            title: <FormattedMessage id='pages.table.product.id' />,
            dataIndex: 'id',
            valueType: 'text',
            sorter: false,
            search: false,
            hideInTable: false,
        },
        {
            title: <FormattedMessage id='pages.table.product.name' />,
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
            title: <FormattedMessage id='pages.table.product.nodeType' />,
            dataIndex: 'nodeType',
            valueType: 'text',
            sorter: false,
            valueEnum: {
                direct: { text: intl.formatMessage({ id: 'pages.table.product.directlyDevice' }), },
                gateway: { text: intl.formatMessage({ id: 'pages.table.product.gatewayDevice' }), },
                subset: { text: intl.formatMessage({ id: 'pages.table.product.gatewaySubDevice' }), },
            },
        },
        {
            title: <FormattedMessage id='pages.table.product.netType' />,
            dataIndex: 'netType',
            valueType: 'text',
            sorter: false,
            valueEnum: {
                wifi: { text: 'Wi-Fi', },
                cellular: { text: intl.formatMessage({ id: 'pages.table.product.netTypeCellular' }), },
                ethernet: { text: intl.formatMessage({ id: 'pages.table.product.netTypeEthernet' }), },
                other: { text: intl.formatMessage({ id: 'pages.table.product.gatewaySubDevice' }), },
            },
        },
        {
            title: <FormattedMessage id='pages.table.product.protocolType' />,
            dataIndex: 'protocolType',
            valueType: 'text',
            sorter: false,
            valueEnum: {
                mqtt: { text: 'MQTT', },
                coap: { text: 'CoAP', },
                lwm2m: { text: 'LwM2M', },
                http: { text: 'HTTP', },
                modbus: { text: 'Modbus', },
                opcua: { text: 'OPCUA', },
                ble: { text: 'BLE', },
                zigbee: { text: 'Zigbee', },
                other: { text: 'Other', },
            },
        },
        {
            title: <FormattedMessage id='pages.table.product.dataFormat' />,
            dataIndex: 'dataFormat',
            valueType: 'text',
            sorter: false,
            valueEnum: {
                json: { text: intl.formatMessage({ id: 'pages.table.product.dataFormatJson' }), },
                custom: { text: intl.formatMessage({ id: 'pages.table.product.dataFormatCustom' }), },
            },
        },
        {
            title: <FormattedMessage id='pages.table.product.remark' />,
            dataIndex: 'remark',
            valueType: 'text',
            sorter: false,
            search: { transform: () => 'keyword' },
            hideInTable: true,
        },
        {
            title: <FormattedMessage id='pages.table.product.creationTime' />,
            dataIndex: 'creationTime',
            valueType: 'dateTime',
            sorter: true,
            defaultSortOrder: 'descend',
            search: { transform: () => 'keyword' },
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
                    key={`thing${record.id}`}
                    to={{
                        pathname: '/device/product/thing',
                        state: { ...record },
                    }}
                >
                    <FormattedMessage id="pages.table.product.thing" />
                </Link>,
                <Link
                    onClick={async () => {
                        setCurrentRow(record);
                    }}
                    key={`device${record.id}`}
                    to={{
                        pathname: '/device/device',
                        state: { ...record },
                    }}
                >
                    <FormattedMessage id="menu.device.manager.device.list" />
                </Link>,
                <a
                    key={`${record.id}`}
                    onClick={async () => {
                        setCurrentRow(record);
                        setUpdateModalVisible(true);
                    }}
                >
                    <FormattedMessage id="pages.table.edit" />
                </a>,

            ],
        },
    ];

    return (
        <PageContainer>
            <ProTable<API.ProductGetResponseModel>
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
                    const result = await getProducts(parameter);
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
                    fullScreen: true,
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
                    <ProDescriptions<API.ProductGetResponseModel>
                        column={1}
                        title={currentRow?.name}
                        request={async () => {
                            if (currentRow.id) {
                                return { data: await getProduct({ id: currentRow.id }) };
                            }
                            return { data: currentRow || {}, };
                        }}
                        params={{
                            id: currentRow?.id,
                        }}
                        columns={columns as ProDescriptionsItemProps<API.ProductGetResponseModel>[]}
                    />
                )}
            </Drawer>
            <ModalForm<API.CreateProductCommand>
                autoComplete="off"
                visible={createModalVisible}
                onVisibleChange={setCreateModalVisible}
                title={intl.formatMessage({
                    id: 'pages.table.product.createForm.title',
                })}
                formRef={createFormRef}
                onFinish={async (value) => {
                    const result = await postProduct({}, value as API.CreateProductCommand, { skipErrorHandler: true });
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
                    name="name"
                    label={intl.formatMessage({ id: 'pages.table.product.name' })}
                    rules={[
                        {
                            type: 'string',
                            required: true,
                            min: 5,
                            max: 20,
                        },
                    ]}
                />
                <ProFormRadio.Group
                    name="nodeType"
                    label={intl.formatMessage({ id: 'pages.table.product.nodeType' })}
                    radioType='button'
                    options={[
                        {
                            label: intl.formatMessage({ id: 'pages.table.product.directlyDevice' }),
                            value: 'direct',
                        },
                        {
                            label: intl.formatMessage({ id: 'pages.table.product.gatewayDevice' }),
                            value: 'gateway',
                        },
                        {
                            label: intl.formatMessage({ id: 'pages.table.product.gatewaySubDevice' }),
                            value: 'subset',
                        },
                    ]}
                    rules={[{ required: true, }]}
                    fieldProps={{
                        onChange: (e: RadioChangeEvent) => {
                            if (e.target.value != 'subset') {
                                setProtocolTypes({
                                    mqtt: 'MQTT',
                                    coap: 'CoAP',
                                    lwm2m: 'LwM2M',
                                    http: 'HTTP',
                                    other: intl.formatMessage({ id: 'pages.table.product.other' }),
                                });
                            }
                            else {
                                setProtocolTypes({
                                    modbus: 'Modbus',
                                    opcua: 'OPC UA',
                                    ble: 'BLE',
                                    zigbee: 'ZigBee',
                                    other: intl.formatMessage({ id: 'pages.table.product.other' }),
                                });
                            }

                            createFormRef.current?.resetFields(['protocolType']);
                        }
                    }}
                />
                <ProFormSelect
                    name="netType"
                    label={intl.formatMessage({ id: 'pages.table.product.netType' })}
                    valueEnum={{
                        cellular: intl.formatMessage({ id: 'pages.table.product.netTypeCellular' }),
                        wifi: 'Wi-Fi',
                        ethernet: intl.formatMessage({ id: 'pages.table.product.netTypeEthernet' }),
                        other: intl.formatMessage({ id: 'pages.table.product.other' }),
                    }}
                    rules={[{ required: true, }]}
                />
                <Form.Item name="protocolType"
                    label={intl.formatMessage({ id: 'pages.table.product.protocolType' })}
                    rules={[{ required: true }]}>
                    <Select placeholder={intl.formatMessage({ id: 'pages.table.product.selectPlaceholder' })}>
                        {
                            Object.keys(protocolTypes).map(k => (<Option key={k} value={k}>{protocolTypes[k]}</Option>))
                        }
                    </Select>
                </Form.Item>
                <ProFormSelect
                    name="dataFormat"
                    label={intl.formatMessage({ id: 'pages.table.product.dataFormat' })}
                    valueEnum={{
                        json: intl.formatMessage({ id: 'pages.table.product.dataFormatJson' }),
                        custom: intl.formatMessage({ id: 'pages.table.product.dataFormatCustom' }),
                    }}
                    rules={[{ required: true, }]}
                />
                <ProFormTextArea
                    name="remark"
                    label={intl.formatMessage({ id: 'pages.table.product.remark' })}
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
                        const result = await putProduct({ id: currentRow.id }, formData);
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