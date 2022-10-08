import { ConfigProvider, FormInstance, Space } from 'antd';
import { useIntl } from 'umi';
import { getTenant } from '@/services/identityServer/Tenants';
import { ProFormText, ProFormSwitch, DrawerForm, ProFormDateTimePicker, } from '@ant-design/pro-form';
import React, { useContext, useEffect, useRef, useState } from 'react';

export type UpdateFormProps = {
  onCancel: () => void;
  onSubmit: (formData: API.TenantUpdateRequestModel) => Promise<void>;
  visible: boolean;
  initialValues: Partial<API.TenantGetResponseModel>;
};

const UpdateForm: React.FC<UpdateFormProps> = (props) => {

  const [connectionStringHidden, setConnectionStringHidden] = useState<boolean>(true);
  const formRef = useRef<FormInstance>();

  useEffect(() => {
    if (props.initialValues.id) {
      getTenant({ id: props.initialValues.id }).then(result => {
        formRef.current?.setFieldsValue({
          ...result,
          useIndependentDatabase: result.connectionString
        });
        if (result.connectionString) {
          setConnectionStringHidden(false);
        }
        else {
          setConnectionStringHidden(true);
        }
      });
    }
  }, [props.visible]);

  const intl = useIntl();

  const context = useContext(ConfigProvider.ConfigContext);

  return (<DrawerForm<API.TenantUpdateRequestModel>
    title={intl.formatMessage({
      id: 'pages.table.tenant.updateForm.title',
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
      label={intl.formatMessage({ id: 'pages.table.tenant.id' })}
      readonly
    />
    <ProFormText
      name="name"
      label={intl.formatMessage({ id: 'pages.table.tenant.tenantName' })}
      rules={[
        {
          type: 'string',
          required: true,
          min: 5,
          max: 15,
        },
        {
          pattern: RegExp('^[a-z]+$'),
          message: intl.formatMessage({ id: 'pages.table.tenant.invalidTenantName' }),
        }
      ]}
      readonly
    />
    <ProFormText
      name="displayName"
      label={intl.formatMessage({ id: 'pages.table.tenant.displayName' })}
      rules={[
        {
          type: 'string',
          required: true,
          min: 5,
          max: 15,
        },
      ]}
    />
    <ProFormSwitch
      name="useIndependentDatabase"
      initialValue={!connectionStringHidden}
      label={intl.formatMessage({ id: 'pages.table.tenant.useIndependentDatabase' })}
      fieldProps={{
        onChange: checked => {
          setConnectionStringHidden(!checked);
          if (!checked) {
            formRef.current?.setFieldsValue({ connectionString: null });
          }
        },
      }}
    />
    <ProFormText
      hidden={connectionStringHidden}
      name="connectionString"
      label={intl.formatMessage({ id: 'pages.table.tenant.connectionString' })}
      rules={[
        {
          type: 'string',
          required: false,
          min: 30,
        },
      ]}
    />
    <ProFormDateTimePicker
      name="creationTime"
      label={intl.formatMessage({ id: 'pages.table.tenant.creationTime' })}
      readonly
    />
  </DrawerForm>);
}

export default UpdateForm;