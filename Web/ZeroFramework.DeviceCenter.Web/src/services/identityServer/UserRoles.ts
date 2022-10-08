// @ts-ignore
/* eslint-disable */
import { request } from 'umi';

/** 此处后端没有提供注释 GET /api/UserRoles/${param0} */
export async function getUserRoles(
  params: {
    // path
    userId: string;
  },
  options?: { [key: string]: any },
) {
  const { userId: param0, ...queryParams } = params;
  return request<string[]>(`/api/UserRoles/${param0}`, {
    method: 'GET',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 PUT /api/UserRoles/${param0} */
export async function updateUserRoles(
  params: {
    // path
    userId: string;
  },
  body: string[],
  options?: { [key: string]: any },
) {
  const { userId: param0, ...queryParams } = params;
  return request<any>(`/api/UserRoles/${param0}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    params: { ...queryParams },
    data: body,
    ...(options || {}),
  });
}
