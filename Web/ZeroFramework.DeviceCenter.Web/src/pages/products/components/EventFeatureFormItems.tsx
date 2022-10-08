import { useIntl } from 'umi';
import { ProFormRadio } from '@ant-design/pro-form';
import React from 'react';
import DataParameterArea from './DataParameterArea';
import { Form } from 'antd';

export type EventFeatureFormItemsProps = {

};

const EventFeatureFormItems: React.FC<EventFeatureFormItemsProps> = (props) => {

  const intl = useIntl();
  return <>
    <ProFormRadio.Group
      name="eventType"
      label={intl.formatMessage({ id: 'pages.table.product.thing.eventType' })}
      rules={[{ required: true, }]}
      options={[
        {
          label: intl.formatMessage({ id: 'pages.table.product.thing.eventType.info' }),
          value: 'info',
        },
        {
          label: intl.formatMessage({ id: 'pages.table.product.thing.eventType.alert' }),
          value: 'alert',
        },
        {
          label: intl.formatMessage({ id: 'pages.table.product.thing.eventType.error' }),
          value: 'error',
        },
      ]}
    />
    <Form.Item
      name='outputData'
      label={intl.formatMessage({ id: 'pages.table.product.thing.outputParameter' })}
    >
      <DataParameterArea />
    </Form.Item>
  </>
}

export default EventFeatureFormItems;