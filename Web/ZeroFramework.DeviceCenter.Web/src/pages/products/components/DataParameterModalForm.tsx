import { FormInstance, Space } from 'antd';
import { useIntl } from 'umi';
import { ModalForm, ProFormText } from '@ant-design/pro-form';
import { useEffect, useRef } from 'react';
import DataTypeFormItems from './DataTypeFormItems';

export type DataParameterModalFormProps = {
  onCancel: () => void;
  onSubmit: (formData: API.DataParameter) => Promise<boolean | void>;
  initialValues?: Partial<API.DataParameter>;
  visible: boolean;
};

const DataParameterModalForm: React.FC<DataParameterModalFormProps> = (props) => {

  const intl = useIntl();
  const formRef = useRef<FormInstance>();

  useEffect(() => {
    if (props.initialValues) {
      formRef.current?.setFieldsValue({
        ...props.initialValues,
      });
    }
  }, [props.visible]);

  return <ModalForm<API.DataParameter>
    autoComplete="off"
    visible={props.visible}
    formRef={formRef}
    onVisibleChange={visible => {
      formRef.current?.resetFields();
      if (!visible) {
        props.onCancel();
      }
    }}
    onFinish={async (value) => {
      formRef.current?.resetFields();
      return props.onSubmit(value);
    }}
    title={intl.formatMessage({
      id: 'pages.table.product.thing.addParameter',
    })}
    submitter={{
      render: (props, doms) => {
        return (
          <Space style={{
            width: '100%',
            display: 'flex',
            justifyContent: 'flex-end',
          }}>
            {doms.reverse()}
          </Space>
        )
      },
    }}
  >
    <ProFormText
      name="parameterName"
      label={intl.formatMessage({ id: 'pages.table.product.thing.parameterName' })}
      rules={[
        {
          type: 'string',
          required: true,
          min: 3,
          max: 15,
        },
      ]}
    />
    <ProFormText
      name="identifier"
      label={intl.formatMessage({ id: 'pages.table.product.thing.identifier' })}
      rules={[
        {
          type: 'string',
          required: true,
          min: 3,
          max: 15,
        },
        {
          pattern: RegExp('^[a-zA-Z]+$'),
          message: intl.formatMessage({ id: 'pages.table.product.thing.identifierInvalid' }),
        }
      ]}
    />
    <DataTypeFormItems />
  </ModalForm>
}

export default DataParameterModalForm;