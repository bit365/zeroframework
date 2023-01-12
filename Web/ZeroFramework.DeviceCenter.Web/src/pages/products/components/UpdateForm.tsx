import type { FormInstance} from 'antd';
import { ConfigProvider, Space } from 'antd';
import { useIntl } from 'umi';
import { ProFormText, DrawerForm, ProFormTextArea, ProFormDateTimePicker } from '@ant-design/pro-form';
import React, { useContext, useEffect, useRef } from 'react';
import { getProduct } from '@/services/deviceCenter/Products';

export type UpdateFormProps = {
  onCancel: () => void;
  onSubmit: (formData: API.ProductUpdateRequestModel) => Promise<void>;
  visible: boolean;
  initialValues: Partial<API.ProductGetResponseModel>;
};

const UpdateForm: React.FC<UpdateFormProps> = (props) => {
  const formRef = useRef<FormInstance>();
  const context = useContext(ConfigProvider.ConfigContext);
  useEffect(() => {
    if (props.initialValues.id) {
      getProduct({ id: props.initialValues.id }).then(result => {
        formRef.current?.setFieldsValue({
          ...result,
        });
      });
    }
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.visible]);

  const intl = useIntl();
  return (<DrawerForm<API.ProductUpdateRequestModel>
    title={intl.formatMessage({
      id: 'pages.table.product.editForm.title',
    })}
    formRef={formRef}
    onFinish={props.onSubmit}
    onVisibleChange={visible => {
      formRef.current?.resetFields();
      if (!visible) {
        props.onCancel();
      }
    }}
    visible={props.visible}
    initialValues={props.initialValues}
    submitter={{
      searchConfig: {
        submitText: context.locale?.Modal?.okText,
        resetText: context.locale?.Modal?.cancelText,
      },
      // eslint-disable-next-line @typescript-eslint/no-shadow
      render: (props, doms) => {
        return (
          <Space style={{ width: '100%' }}>
            {doms.reverse()}
          </Space>
        )
      },
    }}
  >
    <ProFormText
      name="id"
      label={intl.formatMessage({ id: 'pages.table.product.id' })}
      readonly
    />
    <ProFormText
      name="name"
      label={intl.formatMessage({ id: 'pages.table.product.name' })}
      rules={[
        {
          type: 'string',
          required: true,
          min: 5,
          max: 20,
        },
      ]}
    />
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
    <ProFormDateTimePicker
      name="creationTime"
      label={intl.formatMessage({ id: 'pages.table.product.creationTime' })}
      readonly
    />
  </DrawerForm>);
}

export default UpdateForm;