// @ts-ignore
/* eslint-disable */
import { request } from 'umi';

/** 此处后端没有提供注释 GET /api/Measurements/property-values */
export async function getDevicePropertyValues(
  params: {
    // query
    productId?: string;
    deviceId?: number;
  },
  options?: { [key: string]: any },
) {
  return request<API.DevicePropertyLastValue[]>('/api/Measurements/property-values', {
    method: 'GET',
    params: {
      ...params,
    },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 PUT /api/Measurements/property-values */
export async function setDevicePropertyValues(
  params: {
    // query
    productId?: string;
    deviceId?: number;
  },
  body: API.StringDevicePropertyValueKeyValuePair[],
  options?: { [key: string]: any },
) {
  return request<any>('/api/Measurements/property-values', {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    params: {
      ...params,
    },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 GET /api/Measurements/property-history-values */
export async function getDevicePropertyHistoryValues(
  params: {
    // query
    productId?: string;
    deviceId?: number;
    identifier?: string;
    startTime?: string;
    endTime?: string;
    sorting?: API.SortingOrder;
    pageNumber?: number;
    pageSize?: number;
  },
  options?: { [key: string]: any },
) {
  return request<API.DevicePropertyValuePageableListResposeModel>(
    '/api/Measurements/property-history-values',
    {
      method: 'GET',
      params: {
        ...params,
      },
      ...(options || {}),
    },
  );
}

/** 此处后端没有提供注释 GET /api/Measurements/property-reports */
export async function getDevicePropertyReports(
  params: {
    // query
    productId?: string;
    deviceId?: number;
    identifier?: string;
    startTime?: string;
    endTime?: string;
    reportType?: string;
    pageNumber?: number;
    pageSize?: number;
  },
  options?: { [key: string]: any },
) {
  return request<API.DevicePropertyReportPageableListResposeModel>(
    '/api/Measurements/property-reports',
    {
      method: 'GET',
      params: {
        ...params,
      },
      ...(options || {}),
    },
  );
}

/** 此处后端没有提供注释 PUT /api/Measurements/property-value */
export async function setDevicePropertyValue(
  params: {
    // query
    productId?: string;
    deviceId?: number;
    identifier?: string;
  },
  body: API.DevicePropertyValue,
  options?: { [key: string]: any },
) {
  return request<any>('/api/Measurements/property-value', {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    params: {
      ...params,
    },
    data: body,
    ...(options || {}),
  });
}
