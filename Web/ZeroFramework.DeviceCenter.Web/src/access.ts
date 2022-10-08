import { User } from "oidc-client";

/**
 * @see https://umijs.org/zh-CN/plugins/plugin-access
 * */
export default function access(initialState: {
  currentUser?: User | undefined;
  appConfigs?: API.ApplicationConfiguration;
}) {
  const { currentUser, appConfigs } = initialState || {};
  let roles: string[] = currentUser?.profile.role || [];
  let policies = appConfigs?.permissions?.policies;
  let grantedPolicies = appConfigs?.permissions?.grantedPolicies;

  let permissions = {};

  for (const key in policies) {
    if (Object.prototype.hasOwnProperty.call(policies, key)) {
      permissions[key] = Object.prototype.hasOwnProperty.call(grantedPolicies, key) && grantedPolicies[key]
    }
  }

  return {
    canTenantManager: roles.indexOf('TenantOwner') > -1,
    canIdentityManager: roles.findIndex(r => r.startsWith('TenantOwner')) > -1,
    ...permissions,
  };
}