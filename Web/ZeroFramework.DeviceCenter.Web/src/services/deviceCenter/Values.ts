// @ts-ignore
/* eslint-disable */
import { request } from 'umi';

/** 此处后端没有提供注释 GET /api/Values */
export async function get(options?: { [key: string]: any }) {
  return request<any>('/api/Values', {
    method: 'GET',
    ...(options || {}),
  });
}
