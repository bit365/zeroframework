import type { ActionType, ProColumns } from '@ant-design/pro-table';
import ProTable from '@ant-design/pro-table';
import { useIntl, FormattedMessage } from 'umi';
import { getDevices } from '@/services/deviceCenter/Devices';
import { useRef } from 'react';
import { ProFormSelect } from '@ant-design/pro-form';
import { getProducts } from '@/services/deviceCenter/Products';

export type DeviceListProps = {
    rowSelectionChange: (selectDevices: API.DeviceGetResponseModel[]) => void;
};

export default (props: DeviceListProps) => {
    const intl = useIntl();
    const tableActionRef = useRef<ActionType>();

    const columns: ProColumns<API.DeviceGetResponseModel>[] = [
        {
            title: <FormattedMessage id='pages.table.device.id' />,
            dataIndex: 'id',
            valueType: 'text',
            sorter: true,
            search: false,
            hideInTable:true,
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
                />)
            },
            render: (_, entity) => entity?.product?.name,
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
            title: <FormattedMessage id='pages.table.device.lastOnlineTime' />,
            dataIndex: 'lastOnlineTime',
            valueType: 'dateTime',
            sorter: true,
            search: false,
        },
    ];

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
                request={async (params, sort, filter) => {
                    params = Object.assign(params, {
                        sorter: sort,
                        pageNumber: params.current,
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
                toolBarRender={false}
                options={{
                    search: false,
                    fullScreen: true,
                }}
                rowSelection={{
                    onChange: (_, selectedRows) => {
                        props.rowSelectionChange(selectedRows);
                    },
                }}
                search={{ labelWidth: 'auto' }}
            />
        </>
    );
};