// @ts-ignore
/* eslint-disable */
import { request } from 'umi';

/** 此处后端没有提供注释 GET /api/Measurements/property-history-values */
export async function getDevicePropertyHistoryValues(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getDevicePropertyHistoryValuesParams,
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
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getDevicePropertyReportsParams,
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

/** 此处后端没有提供注释 GET /api/Measurements/property-values */
export async function getDevicePropertyValues(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getDevicePropertyValuesParams,
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
export async function setDevicePropertyValue(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.setDevicePropertyValueParams,
  body: Record<string, any>,
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
