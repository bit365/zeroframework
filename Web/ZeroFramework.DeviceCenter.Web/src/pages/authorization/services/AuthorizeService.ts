import type { UserManagerSettings } from "oidc-client";
import { UserManager } from "oidc-client";

const userManagerSettings: UserManagerSettings = {
    authority: REACT_APP_ENV === 'dev' ? 'https://localhost:5001' : 'https://identityserver.helloworldnet.com',
    client_id: 'devicecenterweb',
    redirect_uri: `${window.location.origin}/authorization/login-callback`,
    post_logout_redirect_uri: `${window.location.origin}/authorization/logout-callback`,
    response_type: 'code',
    scope: 'openid profile openapi userinfo',
    automaticSilentRenew: true,
    includeIdTokenInSilentRenew: true,
};

export const userManager = new UserManager(userManagerSettings);
