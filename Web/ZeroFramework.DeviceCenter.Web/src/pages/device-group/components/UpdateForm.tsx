import type { FormInstance} from 'antd';
import { ConfigProvider, Form, Space, TreeSelect } from 'antd';
import { useIntl } from 'umi';
import { ProFormText, DrawerForm, ProFormTextArea } from '@ant-design/pro-form';
import React, { useContext, useEffect, useRef, useState } from 'react';
import { getDeviceGroup, getDeviceGroups } from '@/services/deviceCenter/DeviceGroups';
import type { DataNode } from 'antd/lib/tree';

export type UpdateFormProps = {
  onCancel: () => void;
  onSubmit: (formData: API.DeviceGroupUpdateRequestModel) => Promise<void>;
  visible: boolean;
  initialValues: Partial<API.DeviceGroupGetResponseModel>;
};

const UpdateForm: React.FC<UpdateFormProps> = (props) => {

  const formRef = useRef<FormInstance>();
  const context = useContext(ConfigProvider.ConfigContext);
  const [deviceGroupTreeData, setDeviceGroupTreeData] = useState<DataNode[]>([]);

  useEffect(() => {
    if (props.initialValues.id) {
      getDeviceGroup({ id: props.initialValues.id }).then(result => {
        formRef.current?.setFieldsValue({
          ...result,
        });
      });
    }
    if (props.visible) {
      // eslint-disable-next-line @typescript-eslint/no-use-before-define
      fetchDeviceGroupListApi().then(e => { setDeviceGroupTreeData(e) });
    }
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.visible]);

  const intl = useIntl();

  const fetchDeviceGroupListApi = async (parentId?: number) => {
    const result = await getDeviceGroups({ parentId: parentId, pageNumber: 1, pageSize: 100 });
    if (result) {
      const parentDataNodes: any[] = [];
      result.items?.forEach(e => {
        if (e.name && e.id) {
          parentDataNodes.push({ id: e.id, title: e.name, key: e.id, pId: parentId || 0, value: e.id, isLeaf: false });
        }
      });
      return parentDataNodes;
    }
    return [];
  }

  return (<DrawerForm<API.DeviceGroupUpdateRequestModel>
    title={intl.formatMessage({
      id: 'pages.table.deviceGroup.editForm.title',
    })}
    formRef={formRef}
    onFinish={props.onSubmit}
    onVisibleChange={(visible: any) => {
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
      render: (props: any, doms: JSX.Element[]) => {
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
      label={intl.formatMessage({ id: 'pages.table.deviceGroup.id' })}
      readonly
    />
    <Form.Item name='parentId' label={intl.formatMessage({ id: 'pages.table.deviceGroup.parentGroup' })} required >
      <TreeSelect
        treeDataSimpleMode
        style={{ width: '100%' }}
        value={undefined}
        dropdownStyle={{ maxHeight: 400, overflow: 'auto' }}
        treeData={deviceGroupTreeData}
        placeholder={intl.formatMessage({ id: 'pages.table.device.selectPlaceholder' })}
        allowClear
        onChange={() => {

        }}
        loadData={async ({ id }) => {
          const children = await fetchDeviceGroupListApi(id);
          const state = [...deviceGroupTreeData].concat(children);
          const treeData = state.filter((node, i, arr) => arr.findIndex(t => t.key === node.key) === i);
          setDeviceGroupTreeData(treeData);
        }}
      />
    </Form.Item>

    <ProFormText
      name="name"
      label={intl.formatMessage({ id: 'pages.table.deviceGroup.name' })}
      rules={[
        {
          type: 'string',
          required: true,
          min: 5,
          max: 15,
        },
      ]}
    />
    <ProFormTextArea
      name="remark"
      label={intl.formatMessage({ id: 'pages.table.deviceGroup.remark' })}
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