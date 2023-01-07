import { Button, Result } from 'antd';
import React from 'react';
import { FormattedMessage, history } from 'umi';

const NoFoundPage: React.FC = () => (
  <Result
    status="404"
    title="404"
    subTitle={<FormattedMessage id="pages.error.notfound" />}
    extra={
      <Button type="primary" onClick={() => history.push('/')}>     
        <FormattedMessage id="pages.error.back" />
      </Button>
    }
  />
);

export default NoFoundPage;
