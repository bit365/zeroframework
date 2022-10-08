// @ts-ignore
/* eslint-disable */
import { request } from 'umi';

/** 此处后端没有提供注释 GET /api/TenantClaims/${param0} */
export async function getTenantClaims(
  params: {
    // path
    tenantId: string;
  },
  options?: { [key: string]: any },
) {
  const { tenantId: param0, ...queryParams } = params;
  return request<API.TenantClaimModel[]>(`/api/TenantClaims/${param0}`, {
    method: 'GET',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 GET /api/TenantClaims */
export async function getTenantClaims_2(
  params: {
    // path
    tenantId: string;
  },
  options?: { [key: string]: any },
) {
  const { tenantId: param0, ...queryParams } = params;
  return request<API.TenantClaimModel[]>('/api/TenantClaims', {
    method: 'GET',
    params: { ...queryParams },
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
export async function getTenantClaim(
  params: {
    // path
    id: number;
  },
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
  params: {
    // path
    id: number;
  },
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
  params: {
    // path
    id: number;
  },
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<any>(`/api/TenantClaims/${param0}`, {
    method: 'DELETE',
    params: { ...queryParams },
    ...(options || {}),
  });
}
