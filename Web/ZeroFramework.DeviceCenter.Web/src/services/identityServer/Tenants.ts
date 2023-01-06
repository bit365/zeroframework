// @ts-ignore
/* eslint-disable */
import { request } from 'umi';

/** 此处后端没有提供注释 GET /api/Tenants */
export async function getTenants(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getTenantsParams,
  options?: { [key: string]: any },
) {
  return request<API.TenantGetResponseModelPagedResponseModel>('/api/Tenants', {
    method: 'GET',
    params: {
      // pageNumber has a default value: 1
      pageNumber: '1',
      // pageSize has a default value: 20
      pageSize: '20',
      ...params,
    },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 POST /api/Tenants */
export async function postTenant(
  body: API.TenantCreateRequestModel,
  options?: { [key: string]: any },
) {
  return request<API.TenantCreateRequestModel>('/api/Tenants', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 GET /api/Tenants/${param0} */
export async function getTenant(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getTenantParams,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<API.TenantGetResponseModel>(`/api/Tenants/${param0}`, {
    method: 'GET',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 PUT /api/Tenants/${param0} */
export async function putTenant(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.putTenantParams,
  body: API.TenantUpdateRequestModel,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<any>(`/api/Tenants/${param0}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    params: { ...queryParams },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 DELETE /api/Tenants/${param0} */
export async function deleteTenant(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.deleteTenantParams,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<any>(`/api/Tenants/${param0}`, {
    method: 'DELETE',
    params: { ...queryParams },
    ...(options || {}),
  });
}
