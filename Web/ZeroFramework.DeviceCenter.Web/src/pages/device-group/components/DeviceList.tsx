import type { FormInstance } from 'antd';
import { Button, Card, ConfigProvider, Drawer, message, Popconfirm, Space } from 'antd';
import type { ActionType, ProColumns } from '@ant-design/pro-table';
import ProTable from '@ant-design/pro-table';
import { PlusOutlined } from '@ant-design/icons';
import { useIntl, FormattedMessage, Link } from 'umi';
import { getDevice, getDevices } from '@/services/deviceCenter/Devices';
import { useContext, useEffect, useRef, useState } from 'react';
import type { ProDescriptionsItemProps } from '@ant-design/pro-descriptions';
import ProDescriptions from '@ant-design/pro-descriptions';
import { DrawerForm, ProFormSelect } from '@ant-design/pro-form';
import { getProducts } from '@/services/deviceCenter/Products';
import { deleteDevicesFromGroup, putDevicesToGroup } from '@/services/deviceCenter/DeviceGroups';
import AddDeviceToGroup from './AddDeviceToGroup';

export type DeviceListProps = {
    groupId: number;
};

export default (props: DeviceListProps) => {
    const [createModalVisible, setCreateModalVisible] = useState<boolean>(false);
    const createFormRef = useRef<FormInstance>();
    const [currentRow, setCurrentRow] = useState<API.DeviceGetResponseModel>();
    const [selectedRows, setSelectedRows] = useState<API.DeviceGetResponseModel[]>([]);
    const [showDetail, setShowDetail] = useState<boolean>(false);
    const intl = useIntl();
    const tableActionRef = useRef<ActionType>();
    const [addDeviceToGroupSelectedRows, setAddDeviceToGroupSelectedRows] = useState<API.DeviceGetResponseModel[]>([]);

    useEffect(() => {
        tableActionRef.current?.reloadAndRest?.();
    }, [props.groupId]);

    // eslint-disable-next-line @typescript-eslint/no-shadow
    const handleRemove = async (selectedRows: API.DeviceGetResponseModel[]) => {
        const hide = message.loading(intl.formatMessage({ id: 'pages.table.processing' }));
        if (!selectedRows) return true;
        try {
            const ids = selectedRows.map(e => e.id || 0).filter(e => e > 0);
            await deleteDevicesFromGroup({ deviceGroupId: props.groupId }, ids);
            setSelectedRows([]);
            hide();
            message.success(intl.formatMessage({ id: 'pages.table.successful' }));
            tableActionRef.current?.clearSelected?.();
            tableActionRef.current?.reload();
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
            renderFormItem: () => {
                return (<ProFormSelect<API.MeasurementUnitGetResponseModel>
                    name='productId'
                    key='productIdSearch'
                    showSearch
                    request={async ({ keyWords }: any) => {
                        const parameter = { pageSize: 20, keyword: keyWords };
                        const result = await getProducts(parameter);
                        const list: any[] = [];
                        result.items?.forEach(item => {
                            if (item.name && item.id) {
                                list.push({ label: item.name, value: item.id });
                            }
                        });
                        return list;
                    }}
                />)
            },
            render: (_, entity) => entity?.product?.name,
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
                <Popconfirm
                    key={record.id}
                    title={<FormattedMessage id="pages.table.deleteConfirm" />}
                    onConfirm={async () => {
                        if (record.id) {
                            await handleRemove([record]);
                        }
                    }}
                >
                    <a href="#">
                        <FormattedMessage id="pages.deviceGroup.view.removeDeviceFromGroup" />
                    </a>
                </Popconfirm>,
            ],
        },
    ];

    const context = useContext(ConfigProvider.ConfigContext);

    return (
        <>
            <ProTable<API.DeviceGetResponseModel>
                style={{ marginTop: 10, }}
                columns={columns}
                rowKey='id'
                actionRef={tableActionRef}
                pagination={{
                    showSizeChanger: true,
                    showQuickJumper: true,
                }}
                request={async (params, sort) => {
                    const assignParams = Object.assign(params, {
                        sorter: JSON.stringify(sort),
                        pageNumber: params.current,
                        deviceGroupId: props.groupId,
                    });
                    const { current, ...parameter } = assignParams
                    const result = await getDevices(parameter);
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
                        <FormattedMessage id="pages.deviceGroup.view.addDeviceToGroup" />
                    </Button>,
                ]}
                options={{
                    search: false,
                    fullScreen: true,
                }}
                rowSelection={{
                    // eslint-disable-next-line @typescript-eslint/no-shadow
                    onChange: (_, selectedRows) => {
                        setSelectedRows(selectedRows);
                    },
                }}
                search={{ labelWidth: 'auto' }}
                footer={() =>
                    selectedRows?.length > 0 && (
                        <Button type="default"
                            onClick={async () => {
                                await handleRemove(selectedRows);
                            }}
                        >
                            <FormattedMessage id="pages.deviceGroup.view.removeDeviceFromGroup" />
                        </Button>
                    )
                }
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
            <DrawerForm<API.DeviceCreateRequestModel>
                autoComplete="off"
                visible={createModalVisible}
                onVisibleChange={setCreateModalVisible}
                title={intl.formatMessage({
                    id: 'pages.deviceGroup.view.addDeviceToGroup',
                })}
                formRef={createFormRef}
                onFinish={async () => {
                    const ids = addDeviceToGroupSelectedRows.map(e => e.id || 0).filter(e => e > 0);
                    await putDevicesToGroup({ deviceGroupId: props.groupId }, ids);
                    setCreateModalVisible(false);
                    createFormRef.current?.resetFields();
                    tableActionRef.current?.reload();
                    message.success(intl.formatMessage({ id: 'pages.table.successful' }));
                    return true;
                }}
                submitter={{
                    searchConfig: {
                        submitText: context.locale?.Modal?.okText,
                        resetText: context.locale?.Modal?.cancelText,
                    },
                    // eslint-disable-next-line @typescript-eslint/no-shadow
                    render: (props: any, doms: JSX.Element[]) => {
                        return (
                            <Space style={{ width: '100%' }}>
                                {doms.reverse()}
                            </Space>
                        )
                    },
                }}
            >
                <Card bordered>
                    <AddDeviceToGroup rowSelectionChange={devices => {
                        setAddDeviceToGroupSelectedRows(devices);
                    }} />
                </Card>
            </DrawerForm>
        </>
    );
};