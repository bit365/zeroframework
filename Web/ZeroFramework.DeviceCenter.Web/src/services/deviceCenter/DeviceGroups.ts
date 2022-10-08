// @ts-ignore
/* eslint-disable */
import { request } from 'umi';

/** 此处后端没有提供注释 GET /api/DeviceGroups */
export async function getDeviceGroups(
  params: {
    // query
    keyword?: string;
    parentId?: number;
    sorter?: string;
    pageNumber?: number;
    pageSize?: number;
  },
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

/** 此处后端没有提供注释 PUT /api/DeviceGroups/Devices */
export async function putDevicesToGroup(
  params: {
    // query
    deviceGroupId?: number;
  },
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
  params: {
    // query
    deviceGroupId?: number;
  },
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

/** 此处后端没有提供注释 GET /api/DeviceGroups/${param0} */
export async function getDeviceGroup(
  params: {
    // path
    id: number;
  },
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
  params: {
    // path
    id: number;
  },
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
  params: {
    // path
    id: number;
  },
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<any>(`/api/DeviceGroups/${param0}`, {
    method: 'DELETE',
    params: { ...queryParams },
    ...(options || {}),
  });
}
