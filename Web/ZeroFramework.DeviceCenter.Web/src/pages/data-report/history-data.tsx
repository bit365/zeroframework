import { PageContainer } from '@ant-design/pro-layout';
import { useIntl } from 'umi';
import { useEffect, useRef, useState } from 'react';
import { ProFormDateTimeRangePicker, ProFormDependency, ProFormSelect, QueryFilter } from '@ant-design/pro-form';
import { getProduct, getProducts } from '@/services/deviceCenter/Products';
import type { FormInstance } from 'antd';
import { Button, Card, Empty, Form, Input, Space, Table, Tabs } from 'antd';
import { getDevices } from '@/services/deviceCenter/Devices';
import { getDevicePropertyHistoryValues } from '@/services/deviceCenter/Measurements';
import moment from 'moment';
import { Line } from '@ant-design/charts';
import { DownloadOutlined, LeftOutlined, ReloadOutlined, RightOutlined } from '@ant-design/icons';

const { TabPane } = Tabs;

export default (props: any) => {

    const intl = useIntl();

    const formRef = useRef<FormInstance>();
    const [tabsActiveKey, setTabsActiveKey] = useState<string>("1");
    const [dataSourceState, setDataSourceState] = useState<{ loading: boolean, data?: API.DevicePropertyValue[] }>({ loading: false, data: [] });
    const [paginationState, setPaginationState] = useState({ hasPrevious: false, hasNext: false, pageNumber: 1 });

    useEffect(() => {
        if (props.location.state && props.location.state.deviceId) {
            formRef.current?.setFieldsValue({ deviceId: props.location.state.deviceId });
        }
    }, [props.location.state]);


    const fetchHistoryData = async () => {

        setDataSourceState({ loading: true, data: dataSourceState.data });

        const values = formRef.current?.getFieldsValue();

        const result = await getDevicePropertyHistoryValues({
            productId: values.productId,
            deviceId: values.deviceId,
            identifier: values.identifier,
            startTime: values.dateTimeRang[0].toISOString(),
            endTime: values.dateTimeRang[1].toISOString(),
            sorting: 'ascending' as API.SortingOrder,
            pageNumber: values.pageNumber,
            pageSize: 15,
        }, { errorHandler: () => { } });

        paginationState.hasPrevious = values.pageNumber > 1;
        paginationState.hasNext = result.nextOffset != null;
        paginationState.pageNumber = values.pageNumber;

        setPaginationState(paginationState);

        setDataSourceState({
            loading: false,
            data: result.items || [],
        });
    };

    return (
        <PageContainer>
            <Card style={{ marginBottom: 16 }}>
                <QueryFilter
                    defaultCollapsed={false}
                    layout="horizontal"
                    formRef={formRef}
                    onFinish={async () => {
                        formRef.current?.setFieldsValue({ pageNumber: 1 });
                        await fetchHistoryData();
                    }}
                    onReset={() => {
                        setDataSourceState({ loading: false, data: [] });
                        setPaginationState({ hasPrevious: false, hasNext: false, pageNumber: 1 });
                    }}
                    labelWidth='auto'
                >
                    <ProFormSelect<API.ProductGetResponseModel>
                        label={intl.formatMessage({ id: 'pages.table.product.name' })}
                        name='productId'
                        key='productId'
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
                        rules={[{ required: false }]}
                        fieldProps={{
                            onChange: async () => {
                                formRef.current?.resetFields(['deviceId']);
                            },
                        }}
                    />
                    <ProFormDependency name={['productId']}>
                        {({ productId }: any) => {
                            return <ProFormSelect<API.DeviceGetResponseModel>
                                label={intl.formatMessage({ id: 'pages.table.device.name' })}
                                name='deviceId'
                                key='deviceId'
                                showSearch
                                params={{ productId: productId }}
                                request={async ({ keyWords }: any) => {
                                    const parameter = { pageSize: 20, keyword: keyWords, productId: productId };
                                    const result = await getDevices(parameter);
                                    const list: any[] = [];
                                    result.items?.forEach(item => {
                                        if (item.name && item.id) {
                                            list.push({ label: item.name, value: item.id, productid: item.productId });
                                        }
                                    });
                                    return list;
                                }}
                                rules={[{ required: true }]}
                                fieldProps={{
                                    onChange: async (selectedValue: any, option: any) => {
                                        if (option?.productid) {
                                            formRef.current?.setFieldsValue({ productId: option.productid });
                                        }
                                    },
                                }}
                            />
                        }}
                    </ProFormDependency>
                    <ProFormDependency name={['productId']}>
                        {({ productId }: any) => {
                            return <ProFormSelect<API.DeviceGetResponseModel>
                                label={intl.formatMessage({ id: 'pages.table.product.thing.property' })}
                                name='identifier'
                                key='identifier'
                                params={{ productId: productId }}
                                request={async () => {
                                    const list: any[] = [];
                                    if (productId) {
                                        const parameter = { id: productId };
                                        const result = await getProduct(parameter);
                                        result.features?.properties?.forEach(item => {
                                            if (item.name && item.identifier) {
                                                list.push({ label: item.name, value: item.identifier });
                                            }
                                        });
                                    }
                                    return list;
                                }}
                                rules={[{ required: true }]}
                            />
                        }}
                    </ProFormDependency>
                    <ProFormDateTimeRangePicker
                        name='dateTimeRang'
                        label={intl.formatMessage({ id: 'pages.devices.view.properties.history.dateTime' })} rules={[{ required: true }]}
                        fieldProps={
                            {
                                ranges: {
                                    [intl.formatMessage({ id: 'pages.devices.view.properties.history.date.hour' })]: [moment().startOf('hour'), moment().endOf('hour')],
                                    [intl.formatMessage({ id: 'pages.devices.view.properties.history.date.day' })]: [moment().startOf('day'), moment().endOf('day')],
                                    [intl.formatMessage({ id: 'pages.devices.view.properties.history.date.yesterday' })]: [moment().startOf('day').subtract(1, 'days'), moment().endOf('day').subtract(1, 'days')],
                                    [intl.formatMessage({ id: 'pages.devices.view.properties.history.date.week' })]: [moment().startOf('week'), moment().endOf('week')],
                                    [intl.formatMessage({ id: 'pages.devices.view.properties.history.date.month' })]: [moment().startOf('month'), moment().endOf('month')],
                                },
                                format: "YYYY-MM-DD HH:mm",
                                onChange: (values) => {
                                    if (values && values[0] && values[1]) {

                                    }
                                },
                            }
                        }
                        initialValue={[moment().startOf('day'), moment().endOf('day')]}
                        required
                    />
                    <Form.Item name='pageNumber' initialValue={1} >
                        <Input type='hidden' />
                    </Form.Item>
                </QueryFilter>
            </Card>
            <Card
                actions={[<Space key="paging2" hidden={!paginationState.hasPrevious && !paginationState.hasNext}>
                    <Button style={{ fontSize: 12 }} disabled={!paginationState.hasPrevious} onClick={async () => {
                        formRef.current?.setFieldsValue({ pageNumber: paginationState.pageNumber - 1 });
                        await fetchHistoryData();
                    }} >
                        <LeftOutlined />
                    </Button>
                    <Button style={{ fontSize: 12, marginRight: 8, }} disabled={!paginationState.hasNext} onClick={async () => {
                        formRef.current?.setFieldsValue({ pageNumber: paginationState.pageNumber + 1 });
                        await fetchHistoryData();
                    }} >
                        <RightOutlined />
                    </Button>
                </Space>
                ]}>
                <Tabs activeKey={tabsActiveKey} onChange={key => setTabsActiveKey(key)}
                    tabBarExtraContent={
                        <Space hidden={!dataSourceState.data || !dataSourceState.data.length}>
                            <Button size='small' icon={<DownloadOutlined />} />
                            <Button size='small' onClick={() => { formRef.current?.submit() }} icon={<ReloadOutlined />} />
                        </Space>
                    }>
                    <TabPane tab={intl.formatMessage({ id: 'pages.devices.view.properties.history.showType.chart' })} key="1">
                        {dataSourceState.data && dataSourceState.data.length ?
                            <Line
                                data={dataSourceState.data.map(function (e) {
                                    return {
                                        time: moment(e.timestamp).format('YYYY-MM-DD HH:mm:ss'),
                                        value: e.value
                                    }
                                })}
                                xField='time'
                                yField='value'
                                point={{
                                    size: 5,
                                    style: {
                                        lineWidth: 1,
                                        fillOpacity: 1
                                    },
                                    shape: 'circle',
                                }}
                                meta={{
                                    time: {
                                        formatter: function (v: string) { return moment(v).format('YYYY-MM-DD HH:mm:ss'); }
                                    },
                                    value: {
                                        alias: intl.formatMessage({ id: 'pages.devices.view.properties.history.value' }),
                                        formatter: function (v: number) { return v.toFixed(3); }
                                    },
                                }}
                                xAxis={{
                                    label: {
                                        autoHide: true,
                                        autoRotate: false,
                                    },
                                }}
                                slider={{
                                    start: 0,
                                    end: 1
                                }
                                }
                                animation={{
                                    appear: {
                                        duration: 2000,
                                    },
                                }}
                                loading={dataSourceState.loading}
                            />
                            : <Empty image={Empty.PRESENTED_IMAGE_SIMPLE} />}
                    </TabPane>
                    <TabPane tab={intl.formatMessage({ id: 'pages.devices.view.properties.history.showType.table' })} key="2">
                        <Table
                            columns={[{
                                key: 'timestamp',
                                title: intl.formatMessage({ id: 'pages.devices.view.properties.history.dateTime' }),
                                dataIndex: 'timestamp',
                                render: text => moment(text).format('YYYY-MM-DD HH:mm:ss'),
                            },
                            {
                                key: 'value',
                                title: intl.formatMessage({ id: 'pages.devices.view.properties.history.value' }),
                                dataIndex: 'value',
                            },]}
                            rowKey='timestamp'
                            dataSource={dataSourceState.data}
                            loading={dataSourceState.loading}
                            bordered
                            pagination={false}
                            size='small'
                        />
                    </TabPane>
                </Tabs>
            </Card>
        </PageContainer>
    );
};