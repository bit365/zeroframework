import { useEffect, useState } from 'react';
import { Button, message, Space } from 'antd';

import { useIntl } from 'umi';
import { CloudUploadOutlined } from '@ant-design/icons';
import TextArea from 'antd/lib/input/TextArea';

export type ShadowProps = {
    device: API.DeviceGetResponseModel;
};

export default (props: ShadowProps) => {
    const intl = useIntl();
    const [deviceInfo, setDeviceInfo] = useState<API.DeviceGetResponseModel>();

    const fetchDeviceInfoApi = async () => {
        setDeviceInfo(props.device);
    }

    useEffect(() => {
        fetchDeviceInfoApi();
    }, []);

    return (
        <Space direction="vertical" style={{ width: '100%' }} size='large'>
            <TextArea rows={20} value={JSON.stringify(deviceInfo?.product?.features?.properties, null, 2)} >

            </TextArea>
            <Button
                icon={<CloudUploadOutlined />}
                onClick={async () => { message.success(intl.formatMessage({ id: 'pages.table.successful' })); }}>
                {intl.formatMessage({ id: 'pages.devices.view.shadow.update' })}
            </Button>
        </Space>
    );
};