import type { FormInstance} from 'antd';
import { ConfigProvider, Space } from 'antd';
import { useIntl } from 'umi';
import { ProFormText, DrawerForm, ProFormTextArea, ProFormDigit } from '@ant-design/pro-form';
import React, { useContext, useEffect, useRef } from 'react';
import { getMonitoringFactor } from '@/services/deviceCenter/MonitoringFactors';

export type UpdateFormProps = {
  onCancel: () => void;
  onSubmit: (formData: API.MonitoringFactorUpdateRequestModel) => Promise<void>;
  visible: boolean;
  initialValues: Partial<API.MonitoringFactorGetResponseModel>;
};

const UpdateForm: React.FC<UpdateFormProps> = (props) => {
  const formRef = useRef<FormInstance>();
  const context = useContext(ConfigProvider.ConfigContext);
  useEffect(() => {
    if (props.initialValues.id) {
      getMonitoringFactor({ id: props.initialValues.id }).then(result => {
        formRef.current?.setFieldsValue({
          ...result,
        });
      });
    }
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.visible]);

  const intl = useIntl();
  return (<DrawerForm<API.MonitoringFactorUpdateRequestModel>
    title={intl.formatMessage({
      id: 'pages.table.monitoringFactor.editForm.title',
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
      label={intl.formatMessage({ id: 'pages.table.monitoringFactor.id' })}
      readonly
    />
    <ProFormText
      name="factorCode"
      label={intl.formatMessage({ id: 'pages.table.monitoringFactor.factorCode' })}
      rules={[
        {
          type: 'string',
          required: true,
          len: 6
        },
        {
          pattern: RegExp('^[a-z]\\d{5}$'),
          message: intl.formatMessage({ id: 'pages.table.monitoringFactor.invalidFormat' }),
        }
      ]}
    />
    <ProFormText
      name="chineseName"
      label={intl.formatMessage({ id: 'pages.table.monitoringFactor.chineseName' })}
      rules={[
        {
          type: 'string',
          required: true,
          min: 2,
          max: 15,
        },
      ]}
    />
    <ProFormText
      name="englishName"
      label={intl.formatMessage({ id: 'pages.table.monitoringFactor.englishName' })}
      rules={[
        {
          type: 'string',
          required: true,
          min: 2,
          max: 15,
        },
      ]}
    />
    <ProFormDigit
      name="decimals"
      label={intl.formatMessage({ id: 'pages.table.monitoringFactor.decimals' })}
      min={0}
      max={4}
      fieldProps={{ precision: 0 }}
      initialValue={2}
    />
    <ProFormText
      name="unit"
      label={intl.formatMessage({ id: 'pages.table.monitoringFactor.unit' })}
      rules={[
        {
          type: 'string',
          required: false,
          min: 0,
          max: 10,
        },
      ]}
    />
    <ProFormTextArea
      name="remarks"
      label={intl.formatMessage({ id: 'pages.table.monitoringFactor.remarks' })}
      rules={[
        {
          type: 'string',
          required: false,
          min: 2,
          max: 100,
        },
      ]}
    />
  </DrawerForm>);
}

export default UpdateForm;