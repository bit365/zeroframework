import { ConfigProvider, FormInstance, Space } from 'antd';
import { useIntl } from 'umi';
import { ProFormText, DrawerForm, ProFormTextArea, ProFormDateTimePicker } from '@ant-design/pro-form';
import React, { useContext, useEffect, useRef } from 'react';
import { getDevice } from '@/services/deviceCenter/Devices';

export type UpdateFormProps = {
  onCancel: () => void;
  onSubmit: (formData: API.DeviceUpdateRequestModel) => Promise<void>;
  visible: boolean;
  initialValues: Partial<API.DeviceGetResponseModel>;
};

const UpdateForm: React.FC<UpdateFormProps> = (props) => {
  const formRef = useRef<FormInstance>();
  const context = useContext(ConfigProvider.ConfigContext);
  useEffect(() => {
    if (props.initialValues.id) {
      getDevice({ id: props.initialValues.id }).then(result => {
        formRef.current?.setFieldsValue({
          ...result,
        });
      });
    }
  }, [props.visible]);

  const intl = useIntl();
  return (<DrawerForm<API.DeviceUpdateRequestModel>
    title={intl.formatMessage({
      id: 'pages.table.device.editForm.title',
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
      label={intl.formatMessage({ id: 'pages.table.device.id' })}
      readonly
    />
    <ProFormText
      name="name"
      label={intl.formatMessage({ id: 'pages.table.device.name' })}
      rules={[
        {
          type: 'string',
          required: true,
          min: 5,
          max: 20,
        },
      ]}
    />
    <ProFormText
      name="coordinate"
      label={intl.formatMessage({ id: 'pages.table.device.coordinate' })}
      tooltip={{
        title: <a href='https://lbs.amap.com/tools/picker' target='_blank' style={{ color: 'white' }}>Click Map Picker</a>,
        color: 'cyan',
      }}
      rules={[
        {
          type: 'string',
          required: true,
          min: 6,
          max: 30,
        },
        {
          pattern: /^[-\+]?\d+(\.\d+)\,[-\+]?\d+(\.\d+)$/,
          message: intl.formatMessage({ id: 'pages.table.device.invalidFormat' }),
        },
      ]}
    />
    <ProFormTextArea
      name="remark"
      label={intl.formatMessage({ id: 'pages.table.device.remark' })}
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
      label={intl.formatMessage({ id: 'pages.table.device.creationTime' })}
      readonly
    />
  </DrawerForm>);
}

export default UpdateForm;