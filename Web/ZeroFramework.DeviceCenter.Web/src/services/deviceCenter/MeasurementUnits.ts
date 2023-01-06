// @ts-ignore
/* eslint-disable */
import { request } from 'umi';

/** 此处后端没有提供注释 GET /api/MeasurementUnits */
export async function getMeasurementUnits(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getMeasurementUnitsParams,
  options?: { [key: string]: any },
) {
  return request<API.MeasurementUnitGetResponseModelPagedResponseModel>('/api/MeasurementUnits', {
    method: 'GET',
    params: {
      ...params,
    },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 POST /api/MeasurementUnits */
export async function postMeasurementUnit(
  body: API.MeasurementUnitCreateRequestModel,
  options?: { [key: string]: any },
) {
  return request<API.MeasurementUnitGetResponseModel>('/api/MeasurementUnits', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 GET /api/MeasurementUnits/${param0} */
export async function getMeasurementUnit(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.getMeasurementUnitParams,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<API.MeasurementUnitGetResponseModel>(`/api/MeasurementUnits/${param0}`, {
    method: 'GET',
    params: { ...queryParams },
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 PUT /api/MeasurementUnits/${param0} */
export async function putMeasurementUnit(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.putMeasurementUnitParams,
  body: API.MeasurementUnitUpdateRequestModel,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<API.MeasurementUnitGetResponseModel>(`/api/MeasurementUnits/${param0}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    params: { ...queryParams },
    data: body,
    ...(options || {}),
  });
}

/** 此处后端没有提供注释 DELETE /api/MeasurementUnits/${param0} */
export async function deleteMeasurementUnit(
  // 叠加生成的Param类型 (非body参数swagger默认没有生成对象)
  params: API.deleteMeasurementUnitParams,
  options?: { [key: string]: any },
) {
  const { id: param0, ...queryParams } = params;
  return request<any>(`/api/MeasurementUnits/${param0}`, {
    method: 'DELETE',
    params: { ...queryParams },
    ...(options || {}),
  });
}
