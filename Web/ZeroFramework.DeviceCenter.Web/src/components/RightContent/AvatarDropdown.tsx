import { LogoutOutlined, SettingOutlined, UserOutlined } from '@ant-design/icons';
import { Avatar, Menu, Spin } from 'antd';
import { history, useModel } from 'umi';
import HeaderDropdown from '../HeaderDropdown';
import styles from './index.less';
import { FormattedMessage } from 'umi'

export type GlobalHeaderRightProps = {
  menu?: boolean;
};

/**
 * 退出登录，并且将当前的 url 保存
 */
const loginOut = async () => {
  history.push(`/authorization/logout?returnUrl=${encodeURIComponent(window.location.href)}`);
};

const AvatarDropdown: React.FC<GlobalHeaderRightProps> = ({ menu }) => {
  const { initialState } = useModel('@@initialState');

  const loading = (
    <span className={`${styles.action} ${styles.account}`}>
      <Spin
        size="small"
        style={{
          marginLeft: 8,
          marginRight: 8,
        }}
      />
    </span>
  );

  if (!initialState) {
    return loading;
  }

  const { currentUser } = initialState;

  if (!currentUser || !currentUser.profile.name) {
    return loading;
  }

  const menuHeaderDropdown = (
    <Menu className={styles.menu} selectedKeys={[]} onClick={(info) => {
      const { key } = info;
      if (key === 'logout' && initialState) {
        loginOut();
        return;
      }
      history.push(`/account/${key}`);
    }}>
      {menu && (
        <Menu.Item key="center">
          <UserOutlined />
          <FormattedMessage id='menu.account.center' />
        </Menu.Item>
      )}
      {menu && (
        <Menu.Item key="settings">
          <SettingOutlined />
          <FormattedMessage id='menu.account.settings' />
        </Menu.Item>
      )}
      {menu && <Menu.Divider />}

      <Menu.Item key="logout">
        <LogoutOutlined />
        <FormattedMessage id='menu.account.logout' />
      </Menu.Item>
    </Menu>
  );
  return (
    <HeaderDropdown overlay={menuHeaderDropdown}>
      <span className={`${styles.action} ${styles.account}`}>
        <Avatar size="small" className={styles.avatar} src={<UserOutlined />} alt="avatar" />
        <span className={`${styles.name} anticon`}>{currentUser.profile.name}</span>
      </span>
    </HeaderDropdown>
  );
};

export default AvatarDropdown;
