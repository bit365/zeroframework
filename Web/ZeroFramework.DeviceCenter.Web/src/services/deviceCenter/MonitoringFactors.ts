// @ts-ignore
/* eslint-disable */
import { request } from 'umi';

/** 此处后端没有提供注释 GET /api/MonitoringFactors */
export async function getMonitoringFactors(
  params: {
    // query
    keyword?: string;
    sorter?: string;
    pageNumber?: number;
    pageSize?: number;
  },
  options?: { [key: string]: any },
) {
  return request<API.MonitoringFactorGetResponseModelPagedResponseModel>('/api/MonitoringFactors', {
    method: 'GET',
    params: {
      ...params,
    },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 POST /api/MonitoringFactors */
export async function postMonitoringFactor(
  body: API.MonitoringFactorCreateRequestModel,
  options?: { [key: string]: any },
) {
  return request<API.MonitoringFactorGetResponseModel>('/api/MonitoringFactors', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 GET /api/MonitoringFactors/${param0} */
export async function getMonitoringFactor(
  params: {
    // path
    id: number;
  },
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<API.MonitoringFactorGetResponseModel>(`/api/MonitoringFactors/${param0}`, {
    method: 'GET',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 PUT /api/MonitoringFactors/${param0} */
export async function putMonitoringFactor(
  params: {
    // path
    id: number;
  },
  body: API.MonitoringFactorUpdateRequestModel,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<API.MonitoringFactorGetResponseModel>(`/api/MonitoringFactors/${param0}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    params: { ...queryParams },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 DELETE /api/MonitoringFactors/${param0} */
export async function deleteMonitoringFactor(
  params: {
    // path
    id: number;
  },
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<any>(`/api/MonitoringFactors/${param0}`, {
    method: 'DELETE',
    params: { ...queryParams },
    ...(options || {}),
  });
}
