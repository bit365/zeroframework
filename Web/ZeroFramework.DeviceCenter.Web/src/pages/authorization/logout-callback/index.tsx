import { PageLoading } from "@ant-design/pro-layout";
import React, { useEffect } from "react";
import { userManager } from "../services/AuthorizeService";

const UserLogoutCallback: React.FC = () => {
  useEffect(() => {
    async function fetchRedirect() {
      const result = await userManager.signoutRedirectCallback();
      window.location.replace(result.state);
    }
    fetchRedirect();
  }, []);

  return <PageLoading tip='Loading...' />;
};

export default UserLogoutCallback;