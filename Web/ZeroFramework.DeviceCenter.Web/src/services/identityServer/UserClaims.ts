// @ts-ignore
/* eslint-disable */
import { request } from 'umi';

/** 此处后端没有提供注释 GET /api/UserClaims/${param0} */
export async function getUserClaims(
  params: {
    // path
    userId: number;
  },
  options?: { [key: string]: any },
) {
  const { userId: param0, ...queryParams } = params;
  return request<API.UserClaimModel[]>(`/api/UserClaims/${param0}`, {
    method: 'GET',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 POST /api/UserClaims/${param0} */
export async function postUserClaims(
  params: {
    // path
    userId: number;
  },
  body: API.UserClaimModel[],
  options?: { [key: string]: any },
) {
  const { userId: param0, ...queryParams } = params;
  return request<API.UserClaimModel[]>(`/api/UserClaims/${param0}`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    params: { ...queryParams },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 DELETE /api/UserClaims/${param0} */
export async function deleteUserClaims(
  params: {
    // path
    userId: number;
  },
  body: API.UserClaimModel[],
  options?: { [key: string]: any },
) {
  const { userId: param0, ...queryParams } = params;
  return request<any>(`/api/UserClaims/${param0}`, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
    },
    params: { ...queryParams },
    data: body,
    ...(options || {}),
  });
}
