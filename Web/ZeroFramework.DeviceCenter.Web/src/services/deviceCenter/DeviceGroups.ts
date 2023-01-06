// @ts-ignore
/* eslint-disable */
import { request } from 'umi';

/** 此处后端没有提供注释 GET /api/DeviceGroups */
export async function getDeviceGroups(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getDeviceGroupsParams,
  options?: { [key: string]: any },
) {
  return request<API.DeviceGroupGetResponseModelPagedResponseModel>('/api/DeviceGroups', {
    method: 'GET',
    params: {
      ...params,
    },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 POST /api/DeviceGroups */
export async function postDeviceGroup(
  body: API.DeviceGroupCreateRequestModel,
  options?: { [key: string]: any },
) {
  return request<API.DeviceGroupGetResponseModel>('/api/DeviceGroups', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 GET /api/DeviceGroups/${param0} */
export async function getDeviceGroup(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getDeviceGroupParams,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<API.DeviceGroupGetResponseModel>(`/api/DeviceGroups/${param0}`, {
    method: 'GET',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 PUT /api/DeviceGroups/${param0} */
export async function putDeviceGroup(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.putDeviceGroupParams,
  body: API.DeviceGroupUpdateRequestModel,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<API.DeviceGroupGetResponseModel>(`/api/DeviceGroups/${param0}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    params: { ...queryParams },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 DELETE /api/DeviceGroups/${param0} */
export async function deleteDeviceGroup(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.deleteDeviceGroupParams,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<any>(`/api/DeviceGroups/${param0}`, {
    method: 'DELETE',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 PUT /api/DeviceGroups/Devices */
export async function putDevicesToGroup(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.putDevicesToGroupParams,
  body: number[],
  options?: { [key: string]: any },
) {
  return request<any>('/api/DeviceGroups/Devices', {
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

/** 此处后端没有提供注释 DELETE /api/DeviceGroups/Devices */
export async function deleteDevicesFromGroup(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.deleteDevicesFromGroupParams,
  body: number[],
  options?: { [key: string]: any },
) {
  return request<any>('/api/DeviceGroups/Devices', {
    method: 'DELETE',
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
