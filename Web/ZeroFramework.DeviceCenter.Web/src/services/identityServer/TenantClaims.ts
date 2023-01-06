// @ts-ignore
/* eslint-disable */
import { request } from 'umi';

/** 此处后端没有提供注释 GET /api/TenantClaims */
export async function getTenantClaims_2(options?: { [key: string]: any }) {
  return request<API.TenantClaimModel[]>('/api/TenantClaims', {
    method: 'GET',
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 POST /api/TenantClaims */
export async function postTenantClaim(
  body: API.TenantClaimModel,
  options?: { [key: string]: any },
) {
  return request<API.IdentityTenantClaim>('/api/TenantClaims', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 GET /api/TenantClaims/${param0} */
export async function getTenantClaims(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getTenantClaimsParams,
  options?: { [key: string]: any },
) {
  const { tenantId: param0, ...queryParams } = params;
  return request<API.TenantClaimModel[]>(`/api/TenantClaims/${param0}`, {
    method: 'GET',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 GET /api/TenantClaims/${param0} */
export async function getTenantClaim(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getTenantClaimParams,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<API.TenantClaimModel>(`/api/TenantClaims/${param0}`, {
    method: 'GET',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 PUT /api/TenantClaims/${param0} */
export async function putTenantClaim(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.putTenantClaimParams,
  body: API.TenantClaimModel,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<any>(`/api/TenantClaims/${param0}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    params: { ...queryParams },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 DELETE /api/TenantClaims/${param0} */
export async function deleteTenantClaim(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.deleteTenantClaimParams,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<any>(`/api/TenantClaims/${param0}`, {
    method: 'DELETE',
    params: { ...queryParams },
    ...(options || {}),
  });
}
