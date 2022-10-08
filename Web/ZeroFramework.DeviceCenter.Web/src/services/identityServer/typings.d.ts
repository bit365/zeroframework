// @ts-ignore
/* eslint-disable */

declare namespace API {
  type IdentityTenantClaim = {
    id?: number;
    tenantId?: string;
    claimType?: string;
    claimValue?: string;
  };

  type RoleCreateRequestModel = {
    roleName: string;
    displayName?: string;
  };

  type RoleGetResponseModel = {
    id?: number;
    name?: string;
    tenantRoleName?: string;
    displayName?: string;
    creationTime?: string;
  };

  type RoleGetResponseModelPagedResponseModel = {
    items?: RoleGetResponseModel[];
    totalCount?: number;
  };

  type RoleUpdateRequestModel = {
    displayName?: string;
  };

  type TenantClaimModel = {
    id?: number;
    tenantId?: string;
    claimType?: string;
    claimValue?: string;
  };

  type TenantCreateRequestModel = {
    name?: string;
    displayName?: string;
    adminUserName?: string;
    adminPassword?: string;
    connectionString?: string;
  };

  type TenantGetResponseModel = {
    id?: string;
    name?: string;
    displayName?: string;
    connectionString?: string;
    creationTime?: string;
  };

  type TenantGetResponseModelPagedResponseModel = {
    items?: TenantGetResponseModel[];
    totalCount?: number;
  };

  type TenantUpdateRequestModel = {
    id?: string;
    name?: string;
    displayName?: string;
    connectionString?: string;
  };

  type UserClaimModel = {
    claimType?: string;
    claimValue?: string;
  };

  type UserCreateRequestModel = {
    userName: string;
    password: string;
    phoneNumber: string;
    displayName?: string;
  };

  type UserGetResponseModel = {
    id?: number;
    userName?: string;
    tenantUserName?: string;
    phoneNumber?: string;
    lockoutEnabled?: boolean;
    lockoutEnd?: string;
    displayName?: string;
    creationTime?: string;
  };

  type UserGetResponseModelPagedResponseModel = {
    items?: UserGetResponseModel[];
    totalCount?: number;
  };

  type UserUpdateRequestModel = {
    userName: string;
    password: string;
    phoneNumber: string;
    displayName?: string;
  };
}
