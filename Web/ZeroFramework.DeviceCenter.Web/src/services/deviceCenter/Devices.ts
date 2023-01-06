// @ts-ignore
/* eslint-disable */
import { request } from 'umi';

/** 此处后端没有提供注释 GET /api/Devices */
export async function getDevices(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getDevicesParams,
  options?: { [key: string]: any },
) {
  return request<API.DeviceGetResponseModelPagedResponseModel>('/api/Devices', {
    method: 'GET',
    params: {
      ...params,
    },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 POST /api/Devices */
export async function postDevice(
  body: API.DeviceCreateRequestModel,
  options?: { [key: string]: any },
) {
  return request<API.DeviceGetResponseModel>('/api/Devices', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 GET /api/Devices/${param0} */
export async function getDevice(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getDeviceParams,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<API.DeviceGetResponseModel>(`/api/Devices/${param0}`, {
    method: 'GET',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 PUT /api/Devices/${param0} */
export async function putDevice(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.putDeviceParams,
  body: API.DeviceUpdateRequestModel,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<API.DeviceGetResponseModel>(`/api/Devices/${param0}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    params: { ...queryParams },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 DELETE /api/Devices/${param0} */
export async function deleteDevice(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.deleteDeviceParams,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<any>(`/api/Devices/${param0}`, {
    method: 'DELETE',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 GET /api/Devices/statistic */
export async function getStatistic(options?: { [key: string]: any }) {
  return request<API.DeviceStatisticGetResponseModel>('/api/Devices/statistic', {
    method: 'GET',
    ...(options || {}),
  });
}
