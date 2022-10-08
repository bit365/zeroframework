import { PageLoading } from '@ant-design/pro-layout';
import React, { useEffect } from 'react';
import { userManager } from '../services/AuthorizeService';

const UserLogin: React.FC = () => {
  useEffect(() => {
    async function signinRedirect() {
      await userManager.removeUser();
      await userManager.clearStaleState();

      const params = new URLSearchParams(window.location.search);
      const returnUrl = params.get('returnUrl') || window.location.origin;

      await userManager.signinRedirect({ state: returnUrl });
    }
    signinRedirect();
  }, []);
  return <PageLoading tip='Loading...' />;
};

export default UserLogin;