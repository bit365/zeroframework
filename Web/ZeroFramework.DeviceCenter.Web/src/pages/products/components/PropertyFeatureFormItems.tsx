import { useIntl } from 'umi';
import {ProFormRadio } from '@ant-design/pro-form';
import React  from 'react';
import DataTypeFormItems from './DataTypeFormItems';

export type PropertyFeatureFormItemsProps = {
  initialDataType?:string;
};

const PropertyFeatureFormItems: React.FC<PropertyFeatureFormItemsProps> = (props) => {

  const intl = useIntl();
  return <>
    <DataTypeFormItems/>
    <ProFormRadio.Group
      name="accessMode"
      label={intl.formatMessage({ id: 'pages.table.product.thing.propertyAccessMode' })}
      rules={[{ required: true, }]}
      options={[
        {
          label: intl.formatMessage({ id: 'pages.table.product.thing.propertyAccessMode.readWrite' }),
          value: 'readWrite',
        },
        {
          label: intl.formatMessage({ id: 'pages.table.product.thing.propertyAccessMode.read' }),
          value: 'read',
        },
      ]}
    />
  </>
}

export default PropertyFeatureFormItems;