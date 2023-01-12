import { useEffect, useRef, useState } from 'react';
import { Button, Space } from 'antd';

import type { ProDescriptionsActionType } from '@ant-design/pro-descriptions';
import ProDescriptions from '@ant-design/pro-descriptions';
import { getDeviceGroup } from '@/services/deviceCenter/DeviceGroups';
import { useIntl } from 'umi';
import { ReloadOutlined } from '@ant-design/icons';

export type DeviceGroupInfoProps = {
    groupId: number;
};

export default (props: DeviceGroupInfoProps) => {
    const actionRef = useRef<ProDescriptionsActionType>();
    const intl = useIntl();
    const [deviceGroupInfo, setDeviceGroupInfo] = useState<API.DeviceGroupGetResponseModel>();
    const [loading, setLoading] = useState<boolean>(true);

    const fetchDeviceGroupInfoApi = async () => {
        setLoading(true);
        const result = await getDeviceGroup({ id: props.groupId });
        setDeviceGroupInfo(result);
        setLoading(false);
    }

    useEffect(() => {
        fetchDeviceGroupInfoApi();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [props.groupId]);

    return (
        <Space direction="vertical" style={{ width: '100%' }} size='large'>
            <ProDescriptions
                actionRef={actionRef}
                title={intl.formatMessage({ id: 'pages.deviceGroup.view.info' })}
                dataSource={deviceGroupInfo}
                bordered
                loading={loading}
            >
                <ProDescriptions.Item
                    dataIndex="id"
                    label={intl.formatMessage({ id: 'pages.table.deviceGroup.id' })} />
                <ProDescriptions.Item
                    dataIndex="name"
                    label={intl.formatMessage({ id: 'pages.table.deviceGroup.name' })}
                    valueType='text'
                />
                <ProDescriptions.Item dataIndex="creationTime" label={intl.formatMessage({ id: 'pages.table.deviceGroup.creationTime' })} valueType="dateTime" />
                <ProDescriptions.Item dataIndex="remark" label={intl.formatMessage({ id: 'pages.table.deviceGroup.remark' })} />
                <ProDescriptions.Item valueType="option">
                    <Button
                        type="primary"
                        onClick={async () => {
                            await fetchDeviceGroupInfoApi();
                        }}
                        key="reload"
                        icon={<ReloadOutlined />}
                    >
                        {intl.formatMessage({ id: 'pages.table.reload' })}
                    </Button>
                </ProDescriptions.Item>
            </ProDescriptions>
        </Space>
    );
};