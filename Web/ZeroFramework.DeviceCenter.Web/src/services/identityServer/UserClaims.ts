// @ts-ignore
/* eslint-disable */
import { request } from 'umi';

/** 此处后端没有提供注释 GET /api/UserClaims/${param0} */
export async function getUserClaims(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getUserClaimsParams,
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
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.postUserClaimsParams,
  options?: { [key: string]: any },
) {
  const { userId: param0, ...queryParams } = params;
  return request<API.UserClaimModel[]>(`/api/UserClaims/${param0}`, {
    method: 'POST',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 DELETE /api/UserClaims/${param0} */
export async function deleteUserClaims(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.deleteUserClaimsParams,
  options?: { [key: string]: any },
) {
  const { userId: param0, ...queryParams } = params;
  return request<any>(`/api/UserClaims/${param0}`, {
    method: 'DELETE',
    params: { ...queryParams },
    ...(options || {}),
  });
}
