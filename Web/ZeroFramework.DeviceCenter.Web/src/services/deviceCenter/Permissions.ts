// @ts-ignore
/* eslint-disable */
import { request } from 'umi';

/** 此处后端没有提供注释 GET /api/Permissions */
export async function get(
  params: {
    // query
    providerName?: string;
    providerKey?: string;
    resourceGroupId?: string;
  },
  options?: { [key: string]: any },
) {
  return request<API.PermissionListResponseModel>('/api/Permissions', {
    method: 'GET',
    params: {
      ...params,
    },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 PUT /api/Permissions */
export async function update(
  body: API.PermissionUpdateRequestModel,
  options?: { [key: string]: any },
) {
  return request<any>('/api/Permissions', {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}
