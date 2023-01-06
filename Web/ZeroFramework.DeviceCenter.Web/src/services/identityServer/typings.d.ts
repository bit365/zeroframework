declare namespace API {
  type deleteRoleParams = {
    id: number;
  };

  type deleteTenantClaimParams = {
    id: number;
  };

  type deleteTenantParams = {
    id: string;
  };

  type deleteUserClaimsParams = {
    userId: number;
  };

  type deleteUserParams = {
    id: number;
  };

  type getRoleParams = {
    id: number;
  };

  type getRolesParams = {
    keyword?: string;
    sorter?: string;
    pageNumber?: number;
    pageSize?: number;
  };

  type getTenantClaimParams = {
    id: number;
  };

  type getTenantClaimsParams = {
    tenantId: string;
  };

  type getTenantParams = {
    id: string;
  };

  type getTenantsParams = {
    keyword?: string;
    sorter?: string;
    pageNumber?: number;
    pageSize?: number;
  };

  type getUserClaimsParams = {
    userId: number;
  };

  type getUserParams = {
    id: number;
  };

  type getUserRolesParams = {
    userId: string;
  };

  type getUsersParams = {
    keyword?: string;
    sorter?: string;
    pageNumber?: number;
    pageSize?: number;
  };

  type IdentityTenantClaim = {
    id?: number;
    tenantId?: string;
    claimType?: string;
    claimValue?: string;
  };

  type postUserClaimsParams = {
    userId: number;
  };

  type putRoleParams = {
    id: number;
  };

  type putTenantClaimParams = {
    id: number;
  };

  type putTenantParams = {
    id: string;
  };

  type putUserParams = {
    id: number;
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

  type updateUserRolesParams = {
    userId: string;
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
