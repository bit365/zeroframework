import { useIntl } from 'umi';
import { ModalForm, ProFormRadio, ProFormText, ProFormTextArea } from '@ant-design/pro-form';
import { FormInstance, RadioChangeEvent, Space } from 'antd';
import { useEffect, useRef, useState } from 'react';
import PropertyFeatureFormItems from './PropertyFeatureFormItems';
import ServiceFeatureFormItems from './ServiceFeatureFormItems';
import EventFeatureFormItems from './EventFeatureFormItems';

export type FeatureFormProps = {
  onCancel: () => void;
  onSubmit: (formData: Record<string, any>) => Promise<boolean | void>;
  visible: boolean;
  initialValues?: any;
};

const FeatureForm: React.FC<FeatureFormProps> = (props) => {
  const [featureType, setFeatureType] = useState<string>();
  const formRef = useRef<FormInstance>();

  useEffect(() => {
    if (props.initialValues) {
      setFeatureType(props.initialValues.featureType);
      formRef.current?.setFieldsValue({featureType:props.initialValues.featureType});
      formRef.current?.setFieldsValue({ ...props.initialValues.feature });
    }
    else {
      setFeatureType('property');
      formRef.current?.setFieldsValue({featureType:'property'});
    }
  }, [props.visible]);

  const intl = useIntl();

  return <ModalForm
    autoComplete="off"
    formRef={formRef}
    visible={props.visible}
    onFinish={props.onSubmit}
    onVisibleChange={visible => {
      formRef.current?.resetFields();
      if (!visible) {
        props.onCancel();
      }
    }}
    title={intl.formatMessage({
      id: 'pages.table.product.thing.createForm.title',
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
    <ProFormRadio.Group
      name="featureType"
      label={intl.formatMessage({ id: 'pages.table.product.thing.featureType' })}
      radioType='button'
      options={[
        {
          label: intl.formatMessage({ id: 'pages.table.product.thing.property' }),
          value: 'property',
        },
        {
          label: intl.formatMessage({ id: 'pages.table.product.thing.service' }),
          value: 'service',
        },
        {
          label: intl.formatMessage({ id: 'pages.table.product.thing.event' }),
          value: 'event',
        },
      ]}
      rules={[{ required: true, }]}
      fieldProps={{
        onChange: (e: RadioChangeEvent) => {
          formRef.current?.resetFields();
          setFeatureType(e.target.value);
          formRef.current?.setFieldsValue({featureType:e.target.value});
        }
      }}
    />
    <ProFormText
      name="name"
      label={intl.formatMessage({ id: 'pages.table.product.thing.featureName' })}
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
    {(() => {
      switch (featureType) {
        case 'property': return <PropertyFeatureFormItems />;
        case 'service': return <ServiceFeatureFormItems />;
        case 'event': return <EventFeatureFormItems />;
        default: return null;
      }
    }
    )()}
    <ProFormTextArea
      name="remark"
      label={intl.formatMessage({ id: 'pages.table.product.remark' })}
      rules={[
        {
          type: 'string',
          required: false,
          min: 2,
          max: 100,
        },
      ]}
    />
  </ModalForm>
}

export default FeatureForm;