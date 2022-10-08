import { FormattedMessage } from 'umi';
import { useEffect, useState } from 'react';
import { List } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import DataParameterModalForm from './DataParameterModalForm';
import styles from './DataParameterArea.less'

export type DataParameterAreaProps = {
  onChange?: (formData: API.DataParameter[]) => Promise<void>;
  value?: any;
};

const DataParameterArea: React.FC<DataParameterAreaProps> = (props) => {

  const [dataParameterFormVisible, setDataParameterFormVisible] = useState<boolean>(false);
  const [parameters, setParameters] = useState<API.DataParameter[]>([]);
  const [currentRow, setCurrentRow] = useState<API.DataParameter>();

  useEffect(() => {
    if (props.value) {
      setParameters(props.value);
    }
  }, []);

  return <>
    <List<API.DataParameter>
      size="small"
      footer={
        <a
          onClick={async () => {
            setDataParameterFormVisible(true);
          }}>
          <PlusOutlined /> <FormattedMessage id='pages.table.product.thing.addParameter' />
        </a>}
      bordered
      className={styles.antlist}
      dataSource={parameters}
      renderItem={item =>
        <List.Item
          actions={
            [
              <a key="edit" onClick={async () => {
                setCurrentRow(item);
                setDataParameterFormVisible(true);
              }}>
                <FormattedMessage id="pages.table.edit" />
              </a>,
              <a key="delete" onClick={async () => {
                setParameters(parameters.filter(v => v.identifier != item.identifier));
              }}>
                <FormattedMessage id="pages.table.delete" />
              </a>
            ]
          }>
          {item?.identifier}-{item?.parameterName}-{item?.dataType?.type}
        </List.Item>}
    />
    <DataParameterModalForm
      onSubmit={async (formData: API.DataParameter) => {
        setCurrentRow(undefined);
        let parameterList = [...parameters]
        const existedParameter = parameters.find(p => p.identifier == formData.identifier);
        if (existedParameter) {
          parameterList = parameters.filter(v => v.identifier != formData.identifier);
        }
        parameterList = [formData, ...parameterList];
        setParameters(parameterList);
        setDataParameterFormVisible(false);
        if (props.onChange) {
          props.onChange(parameterList);
        }
        alert(JSON.stringify(formData));
        return true;
      }}
      onCancel={() => {
        setCurrentRow(undefined);
        setDataParameterFormVisible(false);
      }}
      visible={dataParameterFormVisible}
      initialValues={currentRow || {}} />
  </>
}

export default DataParameterArea;