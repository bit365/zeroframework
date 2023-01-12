import { useIntl } from 'umi';
import { ProFormRadio } from '@ant-design/pro-form';
import React from 'react';
import DataParameterArea from './DataParameterArea';
import { Form } from 'antd';
// eslint-disable-next-line @typescript-eslint/ban-types
export type ServiceFeatureFormItemsProps = {
};

const ServiceFeatureFormItems: React.FC<ServiceFeatureFormItemsProps> = () => {

  const intl = useIntl();
  return <>
    <ProFormRadio.Group
      name="invokeMethod"
      label={intl.formatMessage({ id: 'pages.table.product.thing.invokeMethod' })}
      rules={[{ required: true, }]}
      options={[
        {
          label: intl.formatMessage({ id: 'pages.table.product.thing.invokeMethod.async' }),
          value: 'async',
        },
        {
          label: intl.formatMessage({ id: 'pages.table.product.thing.invokeMethod.await' }),
          value: 'await',
        },
      ]}
    />
    <Form.Item
      name='inputData'
      label={intl.formatMessage({ id: 'pages.table.product.thing.inputParameter' })}
    >
      <DataParameterArea />
    </Form.Item>
    <Form.Item
      name='outputData'
      label={intl.formatMessage({ id: 'pages.table.product.thing.outputParameter' })}
    >
      <DataParameterArea />
    </Form.Item>
  </>
}

export default ServiceFeatureFormItems;