import { useEffect, useRef, useState } from 'react';
import { Button, Space } from 'antd';

import type { ProDescriptionsActionType } from '@ant-design/pro-descriptions';
import ProDescriptions from '@ant-design/pro-descriptions';
import { getDevice } from '@/services/deviceCenter/Devices';
import { useIntl } from 'umi';
import { ReloadOutlined } from '@ant-design/icons';

export type DeviceInfoProps = {
    devideId: number;
};

export default (props: DeviceInfoProps) => {
    const actionRef = useRef<ProDescriptionsActionType>();
    const intl = useIntl();
    const [deviceInfo, setDeviceInfo] = useState<API.DeviceGetResponseModel>();
    const [loading, setLoading] = useState<boolean>(true);

    const fetchDeviceInfoApi = async () => {
        setLoading(true);
        let result = await getDevice({ id: props.devideId });
        setDeviceInfo(result);
        setLoading(false);
    }

    useEffect(() => {
        fetchDeviceInfoApi();
    }, []);

    return (
        <Space direction="vertical" style={{ width: '100%' }} size='large'>
            <ProDescriptions
                actionRef={actionRef}
                title={intl.formatMessage({ id: 'pages.devices.view.info' })}
                dataSource={deviceInfo}
                bordered
                loading={loading}
            >
                <ProDescriptions.Item
                    dataIndex="id"
                    label={intl.formatMessage({ id: 'pages.table.device.id' })} />
                <ProDescriptions.Item
                    dataIndex="name"
                    label={intl.formatMessage({ id: 'pages.table.device.name' })}
                    valueType='text'
                />
                <ProDescriptions.Item
                    dataIndex="status"
                    label={intl.formatMessage({ id: 'pages.table.device.status' })}
                    valueEnum={{
                        unactive: { text: intl.formatMessage({ id: 'pages.table.device.status.unactive' }), status: 'Processing' },
                        online: { text: intl.formatMessage({ id: 'pages.table.device.status.online' }), status: 'Success' },
                        offline: { text: intl.formatMessage({ id: 'pages.table.device.status.offline' }), status: 'Default' },
                    }}
                />
                <ProDescriptions.Item dataIndex="creationTime" label={intl.formatMessage({ id: 'pages.table.device.creationTime' })} valueType="dateTime" />
                <ProDescriptions.Item dataIndex="lastOnlineTime" label={intl.formatMessage({ id: 'pages.table.device.lastOnlineTime' })} valueType="dateTime" />
                <ProDescriptions.Item dataIndex="coordinate" label={intl.formatMessage({ id: 'pages.table.device.coordinate' })} />
                <ProDescriptions.Item dataIndex="remark" label={intl.formatMessage({ id: 'pages.table.device.remark' })} />
                <ProDescriptions.Item  valueType="option">
                    <Button
                        type="primary"
                        onClick={async () => {
                            await fetchDeviceInfoApi();
                        }}
                        key="reload"
                        icon={<ReloadOutlined />}
                    >
                       {intl.formatMessage({id:'pages.table.reload'})}
                    </Button>
                </ProDescriptions.Item>
            </ProDescriptions>
            <ProDescriptions
                actionRef={actionRef}
                title={intl.formatMessage({ id: 'pages.table.device.product' })}
                dataSource={deviceInfo?.product}
                bordered
                loading={loading}
            >
                <ProDescriptions.Item
                    dataIndex="id"
                    label={intl.formatMessage({ id: 'pages.table.product.id' })} />
                <ProDescriptions.Item
                    dataIndex="name"
                    label={intl.formatMessage({ id: 'pages.table.product.name' })}
                    valueType='text'
                />
                <ProDescriptions.Item dataIndex="creationTime" label={intl.formatMessage({ id: 'pages.table.product.creationTime' })} valueType="dateTime" />
                <ProDescriptions.Item
                    dataIndex="nodeType"
                    label={intl.formatMessage({ id: 'pages.table.product.nodeType' })}
                    valueEnum={{
                        direct: { text: intl.formatMessage({ id: 'pages.table.product.directlyDevice' }), },
                        gateway: { text: intl.formatMessage({ id: 'pages.table.product.gatewayDevice' }), },
                        subset: { text: intl.formatMessage({ id: 'pages.table.product.gatewaySubDevice' }), },
                    }}
                />
                 <ProDescriptions.Item
                    dataIndex="netType"
                    label={intl.formatMessage({ id: 'pages.table.product.netType' })}
                    valueEnum={{
                        wifi: { text: 'Wi-Fi', },
                        cellular: { text: intl.formatMessage({ id: 'pages.table.product.netTypeCellular' }), },
                        ethernet: { text: intl.formatMessage({ id: 'pages.table.product.netTypeEthernet' }), },
                        other: { text: intl.formatMessage({ id: 'pages.table.product.gatewaySubDevice' }), },
                    }}
                />
                <ProDescriptions.Item
                    dataIndex="protocolType"
                    label={intl.formatMessage({ id: 'pages.table.product.protocolType' })}
                    valueEnum={{
                        mqtt: { text: 'MQTT', },
                        coap: { text: 'CoAP', },
                        lwm2m: { text: 'LwM2M', },
                        http: { text: 'HTTP', },
                        modbus: { text: 'Modbus', },
                        opcua: { text: 'OPCUA', },
                        ble: { text: 'BLE', },
                        zigbee: { text: 'Zigbee', },
                        other: { text: 'Other', },
                    }}
                />
                <ProDescriptions.Item
                    dataIndex="dataFormat"
                    label={intl.formatMessage({ id: 'pages.table.product.dataFormat' })}
                    valueEnum={{
                        json: { text: intl.formatMessage({ id: 'pages.table.product.dataFormatJson' }), },
                        custom: { text: intl.formatMessage({ id: 'pages.table.product.dataFormatCustom' }), },
                    }}
                />
                <ProDescriptions.Item dataIndex="remark" label={intl.formatMessage({ id: 'pages.table.product.remark' })} />
            </ProDescriptions>
        </Space>
    );
};