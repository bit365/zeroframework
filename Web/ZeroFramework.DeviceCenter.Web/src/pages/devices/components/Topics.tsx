import { useEffect, useState } from 'react';
import {Table } from 'antd';
import { useIntl } from 'umi';

export type EventsProps = {
    device: API.DeviceGetResponseModel;
};

export default (props: EventsProps) => {
    const intl = useIntl();
    const [deviceInfo, setDeviceInfo] = useState<API.DeviceGetResponseModel>();

    useEffect(() => {
        setDeviceInfo(props.device);
    }, []);

    const dataSource = [
        {
            category: intl.formatMessage({ id: 'pages.table.product.thing.property' }),
            feature: intl.formatMessage({ id: 'pages.devices.view.topics.property.post' }),
            description: intl.formatMessage({ id: 'pages.devices.view.topics.description.request' }),
            topic: `sys/${deviceInfo?.product?.id}/${deviceInfo?.id}/thing/property/post`,
            operation: intl.formatMessage({ id: 'pages.devices.view.topics.operation.publish' }),
        },
        {
            category: intl.formatMessage({ id: 'pages.table.product.thing.property' }),
            feature: intl.formatMessage({ id: 'pages.devices.view.topics.property.post' }),
            description: intl.formatMessage({ id: 'pages.devices.view.topics.description.response' }),
            topic: `sys/${deviceInfo?.product?.id}/${deviceInfo?.id}/thing/property/post/reply`,
            operation: intl.formatMessage({ id: 'pages.devices.view.topics.operation.subscribe' }),
        },
        {
            category: intl.formatMessage({ id: 'pages.table.product.thing.property' }),
            feature: intl.formatMessage({ id: 'pages.devices.view.topics.property.set' }),
            description: intl.formatMessage({ id: 'pages.devices.view.topics.description.request' }),
            topic: `sys/${deviceInfo?.product?.id}/${deviceInfo?.id}/thing/property/set`,
            operation: intl.formatMessage({ id: 'pages.devices.view.topics.operation.subscribe' }),
        },
        {
            category: intl.formatMessage({ id: 'pages.table.product.thing.property' }),
            feature: intl.formatMessage({ id: 'pages.devices.view.topics.property.set' }),
            description: intl.formatMessage({ id: 'pages.devices.view.topics.description.response' }),
            topic: `sys/${deviceInfo?.product?.id}/${deviceInfo?.id}/thing/property/set/reply`,
            operation: intl.formatMessage({ id: 'pages.devices.view.topics.operation.publish' }),
        },
        {
            category: intl.formatMessage({ id: 'pages.table.product.thing.event' }),
            feature: intl.formatMessage({ id: 'pages.devices.view.topics.event.post' }),
            description: intl.formatMessage({ id: 'pages.devices.view.topics.description.request' }),
            topic: `sys/${deviceInfo?.product?.id}/${deviceInfo?.id}/thing/event/{identifier}/post`,
            operation: intl.formatMessage({ id: 'pages.devices.view.topics.operation.publish' }),
        },
        {
            category: intl.formatMessage({ id: 'pages.table.product.thing.event' }),
            feature: intl.formatMessage({ id: 'pages.devices.view.topics.event.post' }),
            description: intl.formatMessage({ id: 'pages.devices.view.topics.description.response' }),
            topic: `sys/${deviceInfo?.product?.id}/${deviceInfo?.id}/thing/event/{identifier}/post/reply`,
            operation: intl.formatMessage({ id: 'pages.devices.view.topics.operation.subscribe' }),
        },
        {
            category: intl.formatMessage({ id: 'pages.table.product.thing.service' }),
            feature: intl.formatMessage({ id: 'pages.devices.view.topics.service.invoke' }),
            description: intl.formatMessage({ id: 'pages.devices.view.topics.description.request' }),
            topic: `sys/${deviceInfo?.product?.id}/${deviceInfo?.id}/thing/service/{identifier}/invoke`,
            operation: intl.formatMessage({ id: 'pages.devices.view.topics.operation.publish' }),
        },
        {
            category: intl.formatMessage({ id: 'pages.table.product.thing.service' }),
            feature: intl.formatMessage({ id: 'pages.devices.view.topics.service.invoke' }),
            description: intl.formatMessage({ id: 'pages.devices.view.topics.description.response' }),
            topic: `sys/${deviceInfo?.product?.id}/${deviceInfo?.id}/thing/service/{identifier}/invoke/reply`,
            operation: intl.formatMessage({ id: 'pages.devices.view.topics.operation.subscribe' }),
        },
    ];

    const columns = [
        {
            title: intl.formatMessage({ id: 'pages.devices.view.topics.category' }),
            dataIndex: 'category',
            key: 'category',
            render: (value: any, row: any, index: number) => {
                const obj = {
                    children: value,
                    props: { rowSpan: 0 },
                };
                if (index === 0) {
                    obj.props.rowSpan = 4;
                }
                if (index === 4 || index === 6) {
                    obj.props.rowSpan = 2;
                }
                return obj;
            },
        },
        {
            title: intl.formatMessage({ id: 'pages.devices.view.topics.feature' }),
            dataIndex: 'feature',
            key: 'feature',
            render: (value: any, row: any, index: number) => {
                const obj = {
                    children: value,
                    props: { rowSpan: 0 },
                };
                if (index % 2 === 0) {
                    obj.props.rowSpan = 2;
                }
                return obj;
            },
        },
        {
            title: intl.formatMessage({ id: 'pages.devices.view.topics.description' }),
            dataIndex: 'description',
            key: 'description',
        },
        {
            title: intl.formatMessage({ id: 'pages.devices.view.topics.topic' }),
            dataIndex: 'topic',
            key: 'topic',
        },
        {
            title: intl.formatMessage({ id: 'pages.devices.view.topics.operation' }),
            dataIndex: 'operation',
            key: 'operation',
        },
    ];

    return (
        <>
            <Table rowKey='topic' columns={columns} dataSource={dataSource} bordered pagination={false} />
        </>
    );
};