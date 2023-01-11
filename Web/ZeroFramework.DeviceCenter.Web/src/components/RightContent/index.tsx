import { Tag, Space } from 'antd';
import { QuestionCircleOutlined } from '@ant-design/icons';
import React from 'react';
import { useModel } from 'umi';
import Avatar from './AvatarDropdown';
import HeaderSearch from '../HeaderSearch';
import styles from './index.less';
import { SelectLang } from './SelectLang'

export type SiderTheme = 'light' | 'dark';

const ENVTagColor = {
  dev: 'orange',
  test: 'green',
  pre: '#87d068',
};

const GlobalHeaderRight: React.FC = () => {
  const { initialState } = useModel('@@initialState');

  if (!initialState || !initialState.settings) {
    return null;
  }

  const { navTheme, layout } = initialState.settings;
  let className = styles.right;

  if ((navTheme === 'dark' && layout === 'top') || layout === 'mix') {
    className = `${styles.right}  ${styles.dark}`;
  }
  return (
    <Space className={className}>
      <HeaderSearch
        className={`${styles.action} ${styles.search}`}
        placeholder="站内搜索"
        defaultValue="水乙方云平台"
        options={[
          {
            label: <a href="https://www.syifang.com">水乙方</a>,
            value: '水乙方云平台'
          },
          {
            label: <a href="https://www.syifang.com">物联网平台</a>,
            value: '物联网',
          },
          {
            label: <a href="https://www.syifang.com">工业互联网</a>,
            value: '工业互联',
          },
          {
            label: <a href="https://www.syifang.com">人工智能</a>,
            value: '人工智能',
          },
        ]}
      // onSearch={value => {
      //   console.log('input', value);
      // }}
      />
      <span
        className={styles.action}
        onClick={() => {
          window.open('https://www.syifang.com');
        }}
      >
        <QuestionCircleOutlined />
      </span>
      <Avatar menu={true} />
      {REACT_APP_ENV && (
        <span>
          <Tag color={ENVTagColor[REACT_APP_ENV]}>{REACT_APP_ENV}</Tag>
        </span>
      )}
      <SelectLang className={styles.action} />
    </Space>
  );
};
export default GlobalHeaderRight;
