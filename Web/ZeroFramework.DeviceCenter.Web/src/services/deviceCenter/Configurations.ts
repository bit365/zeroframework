// @ts-ignore
/* eslint-disable */
import { request } from 'umi';

/** 此处后端没有提供注释 GET /api/Configurations */
export async function get(options?: { [key: string]: any }) {
  return request<API.ApplicationConfiguration>('/api/Configurations', {
    method: 'GET',
    ...(options || {}),
  });
}
