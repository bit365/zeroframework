// @ts-ignore
/* eslint-disable */
import { request } from 'umi';

/** 此处后端没有提供注释 GET /api/ResourceGroups */
export async function getResourceGroups(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getResourceGroupsParams,
  options?: { [key: string]: any },
) {
  return request<API.ResourceGroupGetResponseModelPagedResponseModel>('/api/ResourceGroups', {
    method: 'GET',
    params: {
      ...params,
    },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 POST /api/ResourceGroups */
export async function postResourceGroup(
  body: API.ResourceGroupCreateRequestModel,
  options?: { [key: string]: any },
) {
  return request<API.ResourceGroupGetResponseModel>('/api/ResourceGroups', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 GET /api/ResourceGroups/${param0} */
export async function getResourceGroup(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getResourceGroupParams,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<API.ResourceGroupGetResponseModel>(`/api/ResourceGroups/${param0}`, {
    method: 'GET',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 PUT /api/ResourceGroups/${param0} */
export async function putResourceGroup(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.putResourceGroupParams,
  body: API.ResourceGroupUpdateRequestModel,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<API.ResourceGroupGetResponseModel>(`/api/ResourceGroups/${param0}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    params: { ...queryParams },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 DELETE /api/ResourceGroups/${param0} */
export async function deleteResourceGroup(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.deleteResourceGroupParams,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<any>(`/api/ResourceGroups/${param0}`, {
    method: 'DELETE',
    params: { ...queryParams },
    ...(options || {}),
  });
}
