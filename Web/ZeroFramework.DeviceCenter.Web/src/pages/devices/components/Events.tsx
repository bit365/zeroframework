import { useEffect, useState } from 'react';

import { useIntl } from 'umi';
import moment from 'moment';
import { Table } from 'antd';

export type EventsProps = {
    device: API.DeviceGetResponseModel;
};

export default () => {
    const intl = useIntl();
    const [tableListDataSource, setTableListDataSource] = useState<any>([]);

    useEffect(() => {
        const initialValues: any[] = [];
        for (let i = 0; i < 50; i += 1) {
            initialValues.push({
                key: i.toString(),
                dateTime: Date.now() + Math.floor(i * 100000),
                identifier: `event${i}`,
                featureName: `事件${i}`,
                outputParameter: `{p1:${Math.floor(Math.random() * 100)},p2:${Math.floor(Math.random() * 100)}}`,
            });
        }
        setTableListDataSource(initialValues);
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return (
        <Table
            columns={[{
                title: intl.formatMessage({ id: 'pages.devices.view.properties.history.dateTime' }),
                dataIndex: 'dateTime',
                render: text => moment(text).format('YYYY-MM-DD HH:mm:ss'),
            },
            {
                title: intl.formatMessage({ id: 'pages.table.product.thing.identifier' }),
                dataIndex: 'identifier',
            },
            {
                title: intl.formatMessage({ id: 'pages.table.product.thing.event' }),
                dataIndex: 'featureName',
            },
            {
                title: intl.formatMessage({ id: 'pages.table.product.thing.outputParameter' }),
                dataIndex: 'outputParameter',
            },]}
            dataSource={tableListDataSource}
            bordered
            pagination={{ pageSize: 10 }}
        />
    );
};