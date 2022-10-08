// @ts-ignore
/* eslint-disable */
import { request } from 'umi';

/** 此处后端没有提供注释 GET /api/ResourceGroups */
export async function getResourceGroups(
  params: {
    // query
    keyword?: string;
    sorter?: string;
    pageNumber?: number;
    pageSize?: number;
  },
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
  params: {
    // path
    id: string;
  },
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
  params: {
    // path
    id: string;
  },
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
  params: {
    // path
    id: string;
  },
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<any>(`/api/ResourceGroups/${param0}`, {
    method: 'DELETE',
    params: { ...queryParams },
    ...(options || {}),
  });
}
