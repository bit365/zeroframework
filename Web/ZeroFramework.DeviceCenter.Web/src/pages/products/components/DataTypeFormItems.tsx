import type { RadioChangeEvent} from 'antd';
import { Form, Input, InputNumber, Space } from 'antd';
import { FormattedMessage, useIntl } from 'umi';
import { ProFormDigit, ProFormRadio, ProFormSelect } from '@ant-design/pro-form';
import { getMeasurementUnits } from '@/services/deviceCenter/MeasurementUnits';
import { useState } from 'react';
import { MinusCircleOutlined, PlusOutlined } from '@ant-design/icons';
import DataParameterArea from './DataParameterArea';

// eslint-disable-next-line @typescript-eslint/ban-types
export type DataTypeFormItemsProps = {

};

const DataTypeFormItems: React.FC<DataTypeFormItemsProps> = () => {

  const intl = useIntl();
  const [selectedDataType, setSelectedDataType] = useState<string>();
  const [selectedArrayStructTypeElement, setSelectedArrayStructTypeElement] = useState<boolean>(false);

  return <>
    <ProFormSelect
      name={['dataType', 'type']}
      label={intl.formatMessage({ id: 'pages.table.product.thing.dataType' })}
      valueEnum={{
        int32: 'Int32',
        int64: 'Int64',
        float: 'Float',
        double: 'Double',
        enum: 'Enum',
        bool: 'Bool',
        string: 'String',
        date: 'Date',
        struct: 'Struct',
        array: 'Array',
      }}
      getValueProps={
        value => {
          if (value != selectedDataType) {
            setSelectedDataType(value);
          }
          return value;
        }
      }
      rules={[{ required: true, }]}
      fieldProps={{
        onChange: (value) => {
          setSelectedDataType(value);
        },
        value: selectedDataType,
      }}
    />
    {(() => {
      switch (selectedDataType) {
        case 'int32':
        case 'int64':
        case 'float':
        case 'double':
          return <>
            <Form.Item
              label={intl.formatMessage({ id: 'pages.table.product.thing.valueRange' })}>
              <Form.Item noStyle name={['dataType', 'specs', 'minValue']} >
                <InputNumber
                  style={{ width: 200 }}
                  placeholder={intl.formatMessage({ id: 'pages.table.product.thing.minValue' })}
                  precision={selectedDataType == 'int32' || selectedDataType == 'int64' ? 0 : 2}
                />
              </Form.Item>
              <Form.Item noStyle >
                <label> ~ </label>
              </Form.Item>
              <Form.Item noStyle name={['dataType', 'specs', 'maxValue']}>
                <InputNumber
                  style={{ width: 200 }}
                  placeholder={intl.formatMessage({ id: 'pages.table.product.thing.maxValue' })}
                  precision={selectedDataType == 'int32' || selectedDataType == 'int64' ? 0 : 2}
                />
              </Form.Item>
            </Form.Item>
            <ProFormSelect<API.MeasurementUnitGetResponseModel>
              name={['dataType', 'specs', 'unit']}
              label={intl.formatMessage({ id: 'pages.table.product.thing.unit' })}
              showSearch
              request={async ({ keyWords }) => {
                const parameter = { pageSize: 50, keyword: keyWords };
                const result = await getMeasurementUnits(parameter);
                const unitList: any[] = [];
                result.items?.forEach(item => {
                  if (item.unit && item.id) {
                    unitList.push({ label: `${item.unitName} / ${item.unit}`, value: item.unit });
                  }
                });
                return unitList;
              }}
              rules={[{ required: true, }]}
            /></>
        case 'enum':
          return <>
            <Form.Item name={['dataType', 'specs']} label={intl.formatMessage({ id: 'pages.table.product.thing.enumType.enumItem' })} rules={[{ required: true }]}>
              <Form.List name={['dataType', 'specs']}>
                {(fields, { add, remove }) => (
                  <>
                    {fields.map(({ key, name, fieldKey, ...restField }) => (
                      <Space key={key} style={{ display: 'flex' }} align="baseline">
                        <Form.Item
                          {...restField}
                          name={[name, 'value']}
                          rules={[
                            {
                              required: true,
                              message: intl.formatMessage({ id: 'pages.table.product.inputPlaceholder' }),
                            }]}
                        >
                          <InputNumber
                            min={-32768}
                            max={32767}
                            precision={0}
                            placeholder={intl.formatMessage({ id: 'pages.table.product.thing.enumType.itemValue' })}
                            style={{ width: 200 }} />
                        </Form.Item>
                        <Form.Item
                          {...restField}
                          name={[name, 'description']}
                          rules={[
                            {
                              required: true,
                              message: intl.formatMessage({ id: 'pages.table.product.inputPlaceholder' }),
                            }]}
                        >
                          <Input
                            placeholder={intl.formatMessage({ id: 'pages.table.product.thing.enumType.itemDescription' })}
                            style={{ width: 200 }}
                          />
                        </Form.Item>
                        <MinusCircleOutlined colSpan={1} onClick={() => remove(name)} />
                      </Space>
                    ))}
                    <a type="link" onClick={() => add()}>
                      <PlusOutlined /> <FormattedMessage id='pages.table.product.thing.enumType.addItem' />
                    </a>
                  </>
                )}
              </Form.List>
            </Form.Item>
          </>;
        case 'bool':
          return <>
            <Form.Item
              name='specs'
              label={intl.formatMessage({ id: 'pages.table.product.thing.boolType' })
              }
            >
              <Input.Group compact>
                <Form.Item name={['dataType', 'specs', 'false']} style={{ marginBottom: 0 }} rules={[
                  {
                    required: true,
                    message: intl.formatMessage({ id: 'pages.table.product.inputPlaceholder' }),
                  }]}>
                  <Input addonBefore="0" />
                </Form.Item>
                <Form.Item name={['dataType', 'specs', 'true']} style={{ marginBottom: 0 }} rules={[
                  {
                    required: true,
                    message: intl.formatMessage({ id: 'pages.table.product.inputPlaceholder' }),
                  }]}>
                  <Input addonBefore="1" />
                </Form.Item>
              </Input.Group>
            </Form.Item>
          </>
        case 'string':
          return <>
            <ProFormDigit
              name={['dataType', 'specs', 'maxLength']}
              label={intl.formatMessage({ id: 'pages.table.product.thing.stringType.maxLength' })}
              min={0}
              max={255}
              fieldProps={{ precision: 0 }}
              rules={[
                {
                  required: true,
                },
              ]}
            />
          </>
        case 'date':
          return <>
            <Form.Item
              name='dateFormat'
              label={intl.formatMessage({ id: 'pages.table.product.thing.dateType.format' })}
            >
              <Input disabled readOnly defaultValue={intl.formatMessage({ id: 'pages.table.product.thing.dateType.defaultValue' })} />
            </Form.Item>
          </>
        case 'struct':
          return <Form.Item
            name={['dataType', 'specs']}
            label={intl.formatMessage({ id: 'pages.table.product.thing.structType' })}
          >
            <DataParameterArea />
          </Form.Item>
        case 'array':
          return <>
            <ProFormRadio.Group
              name={['dataType', 'specs', 'elementType']}
              label={intl.formatMessage({ id: 'pages.table.product.thing.arrayType.elementType' })}
              options={[
                {
                  label: 'Int32',
                  value: 'int32',
                },
                {
                  label: 'Int64',
                  value: 'int64',
                },
                {
                  label: 'Float',
                  value: 'float',
                },
                {
                  label: 'Double',
                  value: 'double',
                },
                {
                  label: 'String',
                  value: 'string',
                },
                {
                  label: 'Struct',
                  value: 'struct',
                },
              ]}
              rules={[
                {
                  required: true,
                },
              ]}
              fieldProps={{
                onChange: (e: RadioChangeEvent) => {
                  if (e.target.value == 'struct') {
                    setSelectedArrayStructTypeElement(true);
                  }
                  else {
                    setSelectedArrayStructTypeElement(false);
                  }
                }
              }}
            />
            <ProFormDigit
              name={['dataType', 'specs', 'maxLength']}
              label={intl.formatMessage({ id: 'pages.table.product.thing.arrayType.elementMaxLength' })}
              min={0}
              max={10}
              fieldProps={{ precision: 0 }}
              rules={[
                {
                  required: true,
                },
              ]}
            />
            {
              selectedArrayStructTypeElement &&
              <Form.Item
                name='jsonObject'
                label={intl.formatMessage({ id: 'pages.table.product.thing.structType' })}
              >
                <DataParameterArea />
              </Form.Item>
            }
          </>
        default:
          return null;
      }
    }
    )()}
  </>
}

export default DataTypeFormItems;