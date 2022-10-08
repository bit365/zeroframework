import { useModel } from 'umi';
import React, { useEffect } from 'react';
import { getConfigurations } from '../services/user-service';
import { userManager } from '../services/AuthorizeService';
import { PageLoading } from '@ant-design/pro-layout';

const UserLoginCallback: React.FC = () => {
  const { initialState, setInitialState } = useModel('@@initialState');
  useEffect(() => {
    async function fetchRedirect() {
      const user = await userManager.signinRedirectCallback();
      if (user) {
        const appConfigs = await getConfigurations();
        setInitialState({ ...initialState, currentUser: user, appConfigs: appConfigs });
        window.location.replace(user.state);
      }
    }
    fetchRedirect();
  }, []);

  return <PageLoading tip='Loading...' />;
};

export default UserLoginCallback;