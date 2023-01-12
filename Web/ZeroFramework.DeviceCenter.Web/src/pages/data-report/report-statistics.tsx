import { PageContainer } from '@ant-design/pro-layout';
import { useIntl } from 'umi';
import { useRef, useState } from 'react';
import { ProFormDependency, ProFormSelect, QueryFilter } from '@ant-design/pro-form';
import { getProduct, getProducts } from '@/services/deviceCenter/Products';
import type { FormInstance } from 'antd';
import { Button, Card, DatePicker, Empty, Form, Input, Space, Table, Tabs } from 'antd';
import { getDevices } from '@/services/deviceCenter/Devices';
import { getDevicePropertyReports } from '@/services/deviceCenter/Measurements';
import moment from 'moment';
import { Line } from '@ant-design/charts';
import { DownloadOutlined, LeftOutlined, ReloadOutlined, RightOutlined } from '@ant-design/icons';
const { RangePicker } = DatePicker;
const { TabPane } = Tabs;

export default () => {

    const intl = useIntl();

    const formRef = useRef<FormInstance>();
    const [tabsActiveKey, setTabsActiveKey] = useState<string>("1");
    const [dataSourceState, setDataSourceState] = useState<{ loading: boolean, data?: API.DevicePropertyReport[] }>({ loading: false, data: [] });
    const [paginationState, setPaginationState] = useState({ hasPrevious: false, hasNext: false, pageNumber: 1 });

    const fetchDataApi = async () => {

        setDataSourceState({ loading: true, data: dataSourceState.data });

        const values = formRef.current?.getFieldsValue();

        const result = await getDevicePropertyReports({
            productId: values.productId,
            deviceId: values.deviceId,
            identifier: values.identifier,
            reportType: values.reportType,
            startTime: values.dateRang[0].toISOString(),
            endTime: values.dateRang[1].toISOString(),
            pageSize: 15,
            pageNumber: values.pageNumber
        }, { errorHandler: () => { } });

        paginationState.hasPrevious = values.pageNumber > 1;
        paginationState.hasNext = result.offset != null;
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
                    formRef={formRef}
                    onFinish={async () => {
                        formRef.current?.setFieldsValue({ pageNumber: 1 });
                        await fetchDataApi();
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
                                // eslint-disable-next-line @typescript-eslint/no-shadow
                                request={async ({ keyWords, productId }: any) => {
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
                                // eslint-disable-next-line @typescript-eslint/no-shadow
                                request={async ({ productId }: any) => {
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
                    <ProFormSelect
                        valueEnum={{
                            hour: intl.formatMessage({ id: 'pages.dataReport.reportStatistics.hour' }),
                            day: intl.formatMessage({ id: 'pages.dataReport.reportStatistics.day' }),
                            month: intl.formatMessage({ id: 'pages.dataReport.reportStatistics.month' }),
                            year: intl.formatMessage({ id: 'pages.dataReport.reportStatistics.year' }),
                        }}
                        name="reportType"
                        label={intl.formatMessage({ id: 'pages.dataReport.reportStatistics.type' })}
                        rules={[{ required: true }]}
                        fieldProps={{
                            onChange: value => {
                                switch (value) {
                                    case 'hour':
                                        formRef.current?.setFieldsValue({ dateRang: [moment().startOf('day'), moment().endOf('day')] });
                                        break;
                                    case 'day':
                                        formRef.current?.setFieldsValue({ dateRang: [moment().startOf('week'), moment().endOf('week')] });
                                        break;
                                    case 'month':
                                        formRef.current?.setFieldsValue({ dateRang: [moment().startOf('year'), moment().endOf('year')] });
                                        break;
                                    case 'year':
                                        formRef.current?.setFieldsValue({ dateRang: [moment().startOf('year').subtract(3, 'year'), moment().endOf('year')] });
                                        break;
                                }
                            }
                        }}
                    />
                    <ProFormDependency name={['reportType']}>
                        {({ reportType }: any) => {

                            let rangePicker = <></>;

                            switch (reportType) {
                                case 'hour':
                                    rangePicker = <RangePicker showTime showMinute={false} showSecond={false} format='YYYY-MM-DD HH:00' value={[moment().startOf('day'), moment().endOf('day')]} />;
                                    break;
                                case 'day':
                                    rangePicker = <RangePicker value={[moment().startOf('week'), moment().endOf('week')]} />;
                                    break;
                                case 'month':
                                    rangePicker = <RangePicker picker="month" value={[moment().startOf('year'), moment().endOf('year')]} />;
                                    break;
                                case 'year':
                                    rangePicker = <RangePicker picker="year" value={[moment().startOf('year').subtract(3, 'year'), moment().endOf('year')]} />
                                    break;
                            }

                            return <Form.Item
                                name='dateRang'
                                label={intl.formatMessage({ id: 'pages.devices.view.properties.history.dateTime' })}
                                hidden={!formRef.current?.getFieldValue('reportType')}
                            >
                                {rangePicker}
                            </Form.Item>
                        }}
                    </ProFormDependency>
                    <Form.Item name='pageNumber' initialValue={1} >
                        <Input type='hidden' />
                    </Form.Item>
                </QueryFilter>
            </Card>
            <Card
                actions={[<Space key={10} hidden={!paginationState.hasPrevious && !paginationState.hasNext}>
                    <Button style={{ fontSize: 12 }} disabled={!paginationState.hasPrevious} onClick={async () => {
                        formRef.current?.setFieldsValue({ pageNumber: paginationState.pageNumber - 1 });
                        await fetchDataApi();
                    }} >
                        <LeftOutlined />
                    </Button>
                    <Button style={{ fontSize: 12, marginRight: 8, }} disabled={!paginationState.hasNext} onClick={async () => {
                        formRef.current?.setFieldsValue({ pageNumber: paginationState.pageNumber + 1 });
                        await fetchDataApi();
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
                                data={dataSourceState.data.flatMap(function (e) {
                                    return [{
                                        time: e.time,
                                        value: e.average,
                                        type: intl.formatMessage({ id: 'pages.dataReport.reportStatistics.averageValue' }),
                                    }, {
                                        time: e.time,
                                        value: e.min,
                                        type: intl.formatMessage({ id: 'pages.dataReport.reportStatistics.minValue' }),
                                    }, {
                                        time: e.time,
                                        value: e.max,
                                        type: intl.formatMessage({ id: 'pages.dataReport.reportStatistics.maxValue' }),
                                    }]
                                }).sort((a, b) => a.time?.localeCompare(b?.time || '') || 1)}
                                xField='time'
                                yField='value'
                                seriesField='type'
                                legend={{
                                    position: 'top',
                                }}
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
                                        formatter: function (v: string) { return v; }
                                    },
                                    value: {
                                        formatter: function (v: number) { return v.toFixed(2); },
                                    },
                                }}
                                xAxis={{
                                    label: {
                                        autoHide: true,
                                        autoRotate: false,
                                    },
                                    type: 'cat'
                                }}
                                lineStyle={e => {
                                    if (e.type != intl.formatMessage({ id: 'pages.dataReport.reportStatistics.average' })) {
                                        return {
                                            lineDash: [4, 4],
                                            opacity: 0.8,
                                        };
                                    }
                                    return {};
                                }}
                                color={['#5AD8A6', '#5B8FF9', '#FAAD14']}
                                animation={{
                                    appear: {
                                        duration: 2000
                                    },
                                    enter: false,
                                    update: false,
                                    leave: false,
                                }}
                                loading={dataSourceState.loading}
                            />
                            : <Empty image={Empty.PRESENTED_IMAGE_SIMPLE} />}
                    </TabPane>
                    <TabPane tab={intl.formatMessage({ id: 'pages.devices.view.properties.history.showType.table' })} key="2">
                        <Table
                            columns={[{
                                key: 'date',
                                title: intl.formatMessage({ id: 'pages.devices.view.properties.history.dateTime' }),
                                dataIndex: 'time',
                            },
                            {
                                key: 'average',
                                title: intl.formatMessage({ id: 'pages.dataReport.reportStatistics.averageValue' }),
                                dataIndex: 'average',
                                render: (dom, entity) => `${entity.average?.toFixed(2)}`,
                            },
                            {
                                key: 'min',
                                title: intl.formatMessage({ id: 'pages.dataReport.reportStatistics.minValue' }),
                                dataIndex: 'min',
                                render: (dom, entity) => `${entity.min?.toFixed(2)}`,
                            },
                            {
                                key: 'max',
                                title: intl.formatMessage({ id: 'pages.dataReport.reportStatistics.maxValue' }),
                                dataIndex: 'max',
                                render: (dom, entity) => `${entity.max?.toFixed(2)}`,
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
        </PageContainer >
    );
};