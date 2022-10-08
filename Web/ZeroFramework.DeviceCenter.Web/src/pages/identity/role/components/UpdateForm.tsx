import { ConfigProvider, FormInstance, Space } from 'antd';
import { useIntl } from 'umi';
import { ProFormText, DrawerForm } from '@ant-design/pro-form';
import React, { useContext, useEffect, useRef } from 'react';
import { getRole } from '@/services/identityServer/Roles';

export type UpdateFormProps = {
  onCancel: () => void;
  onSubmit: (formData: API.RoleUpdateRequestModel) => Promise<void>;
  visible: boolean;
  initialValues: Partial<API.RoleGetResponseModel>;
};

const UpdateForm: React.FC<UpdateFormProps> = (props) => {
  const formRef = useRef<FormInstance>();
  const context = useContext(ConfigProvider.ConfigContext);
  useEffect(() => {
    if (props.initialValues.id) {
      getRole({ id: props.initialValues.id }).then(result => {
        formRef.current?.setFieldsValue({
          ...result,
        });
      });
    }
  }, [props.visible]);

  const intl = useIntl();
  return (<DrawerForm<API.RoleUpdateRequestModel>
    title={intl.formatMessage({
      id: 'pages.table.role.editForm.title',
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
      label={intl.formatMessage({ id: 'pages.table.role.id' })}
      readonly
    />
    <ProFormText
      name="tenantRoleName"
      label={intl.formatMessage({ id: 'pages.table.role.tenantRoleName' })}
      readonly
    />
    <ProFormText
      name="name"
      label={intl.formatMessage({ id: 'pages.table.role.name' })}
      readonly
    />
    <ProFormText
      name="displayName"
      label={intl.formatMessage({ id: 'pages.table.role.displayName' })}
      rules={[
        {
          type: 'string',
          required: true,
          min: 6,
          max: 20,
        },
      ]}
    />
  </DrawerForm>);
}

export default UpdateForm;