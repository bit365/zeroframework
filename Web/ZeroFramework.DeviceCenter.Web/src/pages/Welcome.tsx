import React from 'react';
import { PageContainer } from '@ant-design/pro-layout';
import { Card, Alert, Typography, Space } from 'antd';
import { useIntl, FormattedMessage } from 'umi';
import styles from './Welcome.less';

const CodePreview: React.FC = ({ children }) => (
  <pre className={styles.pre}>
    <code>
      <Typography.Text copyable>{children}</Typography.Text>
    </code>
  </pre>
);

export default (): React.ReactNode => {
  const intl = useIntl();
  return (
    <PageContainer>
      <Space direction="vertical" size="middle" style={{ display: 'flex' }} >
        <Card>
          <Alert
            message={intl.formatMessage({
              id: 'pages.welcome.alertMessage'
            })}
            type="success"
            showIcon
          />
        </Card>
        <Card title={intl.formatMessage({
          id: 'pages.welcome.zeroframework'
        })}>
          <Typography.Text strong>
            <FormattedMessage id="pages.welcome.video" />{' '}
            <a
              href="https://www.xcode.me/video/list/1000225681"
              rel="noopener noreferrer"
              target="__blank"
            >
              <FormattedMessage id="pages.welcome.link" defaultMessage="欢迎使用" />
            </a>
          </Typography.Text>
          <CodePreview>https://www.xcode.me/video/list/1000225681</CodePreview>
          <Typography.Text
            strong
            style={{
              marginBottom: 12,
            }}
          >
            <FormattedMessage id="pages.welcome.framework" />{' '}
            <a
              href="https://www.xcode.me/business/we-framework"
              rel="noopener noreferrer"
              target="__blank"
            >
              <FormattedMessage id="pages.welcome.link" defaultMessage="欢迎使用" />
            </a>
          </Typography.Text>
          <CodePreview>https://www.xcode.me/business/we-framework</CodePreview>
        </Card>
        <Card title={intl.formatMessage({
          id: 'pages.welcome.openapi'
        })}>
        <Typography.Text strong>
            <FormattedMessage id="pages.welcome.identityserver" />{' '}
            <a
              href="https://localhost:5001/swagger"
              rel="noopener noreferrer"
              target="__blank"
            >
              <FormattedMessage id="pages.welcome.link" defaultMessage="欢迎使用" />
            </a>
          </Typography.Text>
          <CodePreview>https://localhost:5001/swagger</CodePreview>
          <Typography.Text
            strong
            style={{
              marginBottom: 12,
            }}
          >
            <FormattedMessage id="pages.welcome.devicecenter" />{' '}
            <a
              href="https://localhost:6001/swagger"
              rel="noopener noreferrer"
              target="__blank"
            >
              <FormattedMessage id="pages.welcome.link" defaultMessage="欢迎使用" />
            </a>
          </Typography.Text>
          <CodePreview>https://localhost:6001/swagger</CodePreview>
        </Card>
      </Space>
    </PageContainer>
  );
};
