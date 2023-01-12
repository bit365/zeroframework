import { PageContainer } from '@ant-design/pro-layout';
import { FormattedMessage, Link, useIntl } from 'umi';
import { useRef, useState } from 'react';
import { ProFormDependency, ProFormSelect, QueryFilter } from '@ant-design/pro-form';
import { getProducts } from '@/services/deviceCenter/Products';
import type { FormInstance} from 'antd';
import { Badge, Card, Col, Empty, Row, Spin, Statistic } from 'antd';
import { getDevices } from '@/services/deviceCenter/Devices';
import { getDevicePropertyValues } from '@/services/deviceCenter/Measurements';

export default () => {

    const intl = useIntl();

    const [loading, setLoading] = useState<boolean>(false);
    const formRef = useRef<FormInstance>();
    const [devicePropertyValues, setDevicePropertyValues] = useState<API.DevicePropertyLastValue[]>();

    const fetchDevicePropertyValuesApi = async (productId?: number, deviceId?: number) => {
        setLoading(true);
        const result = await getDevicePropertyValues({ productId, deviceId }, {
            errorHandler: () => {
                setDevicePropertyValues(undefined);
            }
        });
        if (result) {
            setDevicePropertyValues(result);
        }
        else {
            setDevicePropertyValues(undefined);
        }
        setLoading(false);
    }

    return (
        <PageContainer>
            <Card style={{ marginBottom: 16 }}>
                <QueryFilter
                    defaultCollapsed={false}
                    layout="horizontal"
                    formRef={formRef}
                    onFinish={async (values: any) => {
                        await fetchDevicePropertyValuesApi(values.productId, values.deviceId);
                    }}
                    onReset={() => { setDevicePropertyValues(undefined); }}
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
                </QueryFilter>
            </Card>
            <Card>
                <Spin spinning={loading} >
                    <Row gutter={[24, 24]} >
                        {
                            devicePropertyValues?.map(e => {

                                return <Col xs={24} sm={24} md={12} lg={12} xl={8} xxl={6} key={e.identifier}>
                                    <Card>
                                        <Link
                                            key={e.identifier}
                                            style={{ float: 'right' }}
                                            to={{
                                                pathname: '/data-report/history-data',
                                                state: { deviceId: formRef.current?.getFieldValue('deviceId') },
                                            }}
                                        >
                                            <FormattedMessage id="pages.devices.view.properties.history" />
                                        </Link>
                                        <Statistic
                                            title={e.name}
                                            value={e.value}
                                            precision={2}
                                            valueStyle={{ color: '#3f8600' }}
                                            prefix={<Badge color="cyan" />}
                                            suffix={`${e.unit || ''}`}
                                        />
                                    </Card>
                                </Col>

                            }) || <Col span={24}><Empty image={Empty.PRESENTED_IMAGE_SIMPLE} /></Col>}
                    </Row>
                </Spin>
            </Card>
        </PageContainer>
    );
};