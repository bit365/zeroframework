import { useEffect, useState } from 'react';
import { Table } from 'antd';

import { useIntl } from 'umi';
import moment from 'moment';

export type ServicesProps = {
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
                identifier: `service${i}`,
                featureName: `服务${i}`,
                inputParameter: `{p1:${Math.floor(Math.random() * 100)},p2:${Math.floor(Math.random() * 100)}}`,
                outputParameter: `{p3:${Math.floor(Math.random() * 100)},p4:${Math.floor(Math.random() * 100)}}`,
            });
        }
        setTableListDataSource(initialValues);
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
                title: intl.formatMessage({ id: 'pages.table.product.thing.service' }),
                dataIndex: 'featureName',
            },
            {
                title: intl.formatMessage({ id: 'pages.table.product.thing.inputParameter' }),
                dataIndex: 'inputParameter',
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