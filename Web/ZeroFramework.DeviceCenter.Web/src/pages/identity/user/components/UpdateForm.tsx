import { ConfigProvider, FormInstance, Space } from 'antd';
import { useIntl } from 'umi';
import { ProFormText, DrawerForm } from '@ant-design/pro-form';
import React, { useContext, useEffect, useRef } from 'react';
import { getUser } from '@/services/identityServer/Users';

export type UpdateFormProps = {
  onCancel: () => void;
  onSubmit: (formData:API.UserUpdateRequestModel) => Promise<void>;
  visible: boolean;
  initialValues: Partial<API.UserGetResponseModel>;
};

const UpdateForm: React.FC<UpdateFormProps> = (props) => {

  const formRef = useRef<FormInstance>();

  useEffect(() => {
    if (props.initialValues.id) {
      getUser({ id: props.initialValues.id }).then(result => {
        formRef.current?.setFieldsValue({
          ...result, userName: result.tenantUserName,
        });
      });
    }
  }, [props.visible]);

  const context = useContext(ConfigProvider.ConfigContext);

  const intl = useIntl();
  return (<DrawerForm<API.UserUpdateRequestModel>
    title={intl.formatMessage({
      id: 'pages.table.user.updateForm.title',
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
    initialValues={{ ...props.initialValues, userName: props.initialValues.tenantUserName }}
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
      label={intl.formatMessage({ id: 'pages.table.user.id' })}
      readonly
    />
    <ProFormText
      name="userName"
      label={intl.formatMessage({ id: 'pages.table.user.tenantUserName' })}
      rules={[
        {
          type: 'string',
          required: true,
          min: 6,
          max: 20,
        },
        {
          pattern: RegExp('^[a-z]+$'),
          message: intl.formatMessage({ id: 'pages.table.user.invalidUserName' }),
        }
      ]}
    />
    <ProFormText
      name="displayName"
      label={intl.formatMessage({ id: 'pages.table.user.displayName' })}
      rules={[
        {
          type: 'string',
          required: true,
          min: 6,
          max: 20,
        },
      ]}
    />
    <ProFormText
      name="phoneNumber"
      label={intl.formatMessage({ id: 'pages.table.user.phoneNumber' })}
      rules={[
        {
          type: 'string',
          required: true,
          len: 11,
        },
        {
          type: 'string',
          pattern: RegExp('^1\\d{10}$'),
          message: intl.formatMessage({ id: 'pages.table.user.invalidPhoneNumber' }),
        },
      ]}
      fieldProps={{ autoComplete: "new-phoneNumber" }}
    />
    <ProFormText.Password
      name="password"
      label={intl.formatMessage({ id: 'pages.table.user.password' })}
      rules={[
        {
          type: 'string',
          required: true,
          min: 5,
          max: 15,
        },
      ]}
      fieldProps={{ autoComplete: "new-password" }}
    />
  </DrawerForm>);
}

export default UpdateForm;