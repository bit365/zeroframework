import { useEffect, useRef, useState } from 'react';
import type { InfoWindowProps } from 'react-amap';
import { Map, InfoWindow, Markers } from 'react-amap';
import MapPlugin from './MapPlugin';
import { EnvironmentTwoTone, LoadingOutlined } from '@ant-design/icons';
import styles from './index.less';
import { ProFormSelect, QueryFilter } from '@ant-design/pro-form';
import { useIntl, history } from 'umi';
import { getProducts } from '@/services/deviceCenter/Products';
import { FormInstance, Space, Table, Tag } from 'antd';
import { Form } from 'antd';
import { Card, TreeSelect } from 'antd';
import { getDevices } from '@/services/deviceCenter/Devices';
import { getDevicePropertyValues } from '@/services/deviceCenter/Measurements';
import { PageContainer } from '@ant-design/pro-layout';
import { getDeviceGroups } from '@/services/deviceCenter/DeviceGroups';
import type { DataNode } from 'antd/lib/tree';
import moment from 'moment';

export default (props: any) => {

    const [markers, setMarkers] = useState<any>();
    const [infoWindowState, setInfoWindowState] = useState<InfoWindowProps>({ visible: false, position: [0, 0] });
    const [currentDevice, setCurrentDevice] = useState<API.DeviceGetResponseModel>();

    const intl = useIntl();

    const [loading, setLoading] = useState<boolean>(false);
    const formRef = useRef<FormInstance>();
    const [devicePropertyValues, setDevicePropertyValues] = useState<API.DevicePropertyLastValue[]>();
    const [deviceGroupTreeData, setDeviceGroupTreeData] = useState<DataNode[]>([]);

    const fetchDeviceGroupListApi = async (parentId?: number) => {
        const result = await getDeviceGroups({ parentId: parentId, pageNumber: 1, pageSize: 100 }, { errorHandler: () => { } });
        if (result) {
            const parentDataNodes: any[] = [];
            result.items?.forEach(e => {
                if (e.name && e.id) {
                    parentDataNodes.push({ id: e.id, title: e.name, key: e.id, pId: parentId || 0, value: e.id, isLeaf: !e.children?.length });
                }
            });
            return parentDataNodes;
        }
        return [];
    }

    const fetchDevicePropertyValuesApi = async (productId?: string, deviceId?: number) => {
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

    const markersEvents = {
        click: (e: any, marker: any) => {
            setDevicePropertyValues(undefined);
            marker.setAnimation('AMAP_ANIMATION_DROP');
            const extData = marker.getExtData();
            setCurrentDevice(extData.device);

            setInfoWindowState({
                position: e.lnglat,
                visible: true,
            });

            fetchDevicePropertyValuesApi(extData.device.productId, extData.device.id);
        }
    };

    const deviceStatusEnums = {
        'unactive': { text: intl.formatMessage({ id: 'pages.table.device.status.unactive' }), status: 'Processing' },
        'online': { text: intl.formatMessage({ id: 'pages.table.device.status.online' }), status: 'Success' },
        'offline': { text: intl.formatMessage({ id: 'pages.table.device.status.offline' }), status: 'Default' },
    };

    const getDeviceStatuTag = (status?: string) => {
        if (status && deviceStatusEnums.hasOwnProperty(status)) {
            const statusEnum = deviceStatusEnums[status];
            return <Tag color={statusEnum.status.toLowerCase()}>{statusEnum.text}</Tag>
        }
        return null;
    }

    useEffect(() => {
        fetchDeviceGroupListApi().then(e => { setDeviceGroupTreeData(e) });
        formRef.current?.submit();
    }, []);

    return (
        <PageContainer pageHeaderRender={() => null}>
            <Card>
                <QueryFilter
                    defaultCollapsed={false}
                    layout="horizontal"
                    formRef={formRef}
                    onFinish={async (values: any) => {

                        const result = await getDevices({
                            name: values.name,
                            productId: values.productId,
                            deviceGroupId: values.deviceGroupId,
                            status: values.status,
                            pageNumber: 1,
                            pageSize: 100,
                        });

                        const positions = result.items?.map((e, idx) => {
                            const position = {
                                longitude: e.coordinate?.split(',')[0],
                                latitude: e.coordinate?.split(',')[1]
                            };
                            return {
                                position,
                                device: e,
                            }
                        });

                        setMarkers(positions);
                    }}
                    onReset={() => {
                        setDevicePropertyValues(undefined);
                        setInfoWindowState({ visible: false, position: infoWindowState.position });
                    }}
                    span={{ xs: 24, sm: 24, md: 12, lg: 12, xl: 4, xxl: 4, }}
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
                    <Form.Item name='deviceGroupId' label={intl.formatMessage({ id: 'pages.table.deviceGroup.name' })} >
                        <TreeSelect
                            treeDataSimpleMode
                            style={{ width: '100%' }}
                            value={undefined}
                            dropdownStyle={{ maxHeight: 400, overflow: 'auto' }}
                            treeData={deviceGroupTreeData}
                            placeholder={intl.formatMessage({ id: 'pages.table.device.selectPlaceholder' })}
                            allowClear
                            onChange={() => {

                            }}
                            loadData={async ({ id }) => {
                                const children = await fetchDeviceGroupListApi(id);
                                const state = [...deviceGroupTreeData].concat(children);
                                const treeData = state.filter((node, i, arr) => arr.findIndex(t => t.key === node.key) === i);
                                setDeviceGroupTreeData(treeData);
                            }}
                        />
                    </Form.Item>
                    <ProFormSelect
                        key='status'
                        name="status"
                        label={intl.formatMessage({ id: 'pages.table.device.status' })}
                        valueEnum={deviceStatusEnums}
                    />

                    <ProFormSelect<API.DeviceGetResponseModel>
                        label={intl.formatMessage({ id: 'pages.table.device.name' })}
                        name='deviceId'
                        key='deviceId'
                        showSearch
                        dependencies={['productId', 'deviceGroupId', 'status']}
                        request={async ({ keyWords, productId, deviceGroupId, status }: any) => {
                            const parameter = {
                                pageSize: 20,
                                keyword: keyWords,
                                productId: productId,
                                name: keyWords,
                                deviceGroupId: deviceGroupId,
                                status: status,
                            };
                            const result = await getDevices(parameter);
                            const list: any[] = [];
                            result.items?.forEach(item => {
                                if (item.name && item.id) {
                                    list.push({ label: item.name, value: item.id, productid: item.productId });
                                }
                            });
                            return list;
                        }}
                        fieldProps={{
                            onChange: (selectedValue: any, option: any) => {
                                for (let index = 0; index < markers.length; index++) {
                                    const element = markers[index];
                                    if (element.device.id == selectedValue) {
                                        setDevicePropertyValues(undefined);
                                        setCurrentDevice(element.device);
                                        setInfoWindowState({
                                            position: element.position,
                                            visible: true,
                                        });
                                        fetchDevicePropertyValuesApi(element.device.productId, element.device.id);
                                        return;
                                    }
                                }

                                setDevicePropertyValues(undefined);
                                setInfoWindowState({ visible: false, position: infoWindowState.position });
                            },
                        }}
                    />
                </QueryFilter>

                <div style={{ width: '100%', height: innerHeight - 200 }} className={styles.mapview} >
                    <Map amapkey='keykeykeykeykeykeykeykey' version='1.4.15' mapStyle='amap://styles/macaron' zoom={5} loading={<LoadingOutlined />} >
                        <InfoWindow
                            visible={infoWindowState.visible}
                            position={infoWindowState.position}
                            events={{
                                close: () => {
                                    setInfoWindowState({ visible: false, position: [0, 0] });
                                }
                            }}
                            //showShadow
                            closeWhenClickMap
                            offset={[2, -25]}
                        >
                            <Card
                                title={<Space>
                                    <> {currentDevice?.name}</>
                                    <> {getDeviceStatuTag(currentDevice?.status)}</>
                                </Space>}
                                extra={<a href="#" target='_blank' onClick={() => {
                                    history.push('/data-report/history-data', { deviceId: currentDevice?.id });
                                }}>{intl.formatMessage({ id: 'pages.devices.view.properties.history' })}</a>}
                                style={{ marginTop: 20, width: 600 }}
                                bodyStyle={{ padding: 5, paddingTop: 10, paddingBottom: 10 }}
                            >
                                <Table dataSource={devicePropertyValues} loading={loading} columns={[
                                    {
                                        title: intl.formatMessage({ id: 'pages.table.product.thing.property' }),
                                        dataIndex: 'name',
                                        key: 'name',
                                    },
                                    {
                                        title: intl.formatMessage({ id: 'pages.table.product.thing.identifier' }),
                                        dataIndex: 'identifier',
                                        key: 'identifier',
                                    },
                                    {
                                        title: intl.formatMessage({ id: 'pages.devices.view.properties.history.value' }),
                                        dataIndex: 'value',
                                        key: 'value',
                                    },
                                    {
                                        title: intl.formatMessage({ id: 'pages.table.product.thing.unit' }),
                                        dataIndex: 'unit',
                                        key: 'unit',
                                    },
                                    {
                                        title: intl.formatMessage({ id: 'pages.devices.view.properties.history.dateTime' }),
                                        dataIndex: 'timestamp',
                                        key: 'timestamp',
                                        render: (value: any, record: any, index: number) => moment(value).format('YYYY-MM-DD HH:mm:ss'),
                                    },
                                ]} bordered size='small' pagination={false} />
                            </Card>
                        </InfoWindow>
                        <MapPlugin />
                        <Markers markers={markers}
                            useCluster={{ renderClusterMarker: null, zoomOnClick: true, }}
                            events={markersEvents}
                            render={(extData: API.DeviceGetResponseModel) => {

                                let markerColor = '#4a90e2';

                                const markercolors = {
                                    unactive: '#4a90e2',
                                    online: '#389e0d',
                                    offline: '#d0021b',
                                };

                                if (extData.status && markercolors.hasOwnProperty(extData.status)) {
                                    markerColor = markercolors[extData.status];
                                }

                                return <EnvironmentTwoTone
                                    twoToneColor={markerColor}
                                    style={{ fontSize: '24px', color: 'white' }}
                                />
                            }} />
                    </Map>
                </div>
            </Card>
        </PageContainer >
    );
};
