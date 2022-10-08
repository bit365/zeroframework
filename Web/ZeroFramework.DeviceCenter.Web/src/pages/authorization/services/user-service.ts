import { request } from 'umi';
import { userManager } from './AuthorizeService'
import { get as getAppConfigs } from '@/services/deviceCenter/Configurations'

/** 获取当前的用户 GET /api/currentUser */
export async function currentUser(options?: { [key: string]: any }) {
  return new Promise<API.CurrentUser>(async (resolve, reject) => {
    let user = (await userManager.getUser())?.profile;
    if (user) {
      let currentUser: API.CurrentUser = {
        name: user.name,
        avatar: 'https://gw.alipayobjects.com/zos/antfincdn/XAosXuNZyF/BiazfanxmamNRoxxVxka.png',
        userid: user.sub,
        email: 'antdesign@alipay.com',
        signature: user.given_name,
        title: user.family_name,
        group: '蚂蚁金服－某某某事业群－某某平台部－某某技术部－UED',
        tags: [
          {
            key: '0',
            label: user.website,
          },
        ],
        notifyCount: 12,
        unreadCount: 11,
        country: 'China',
        access: 'admin',
        geographic: {
          province: {
            label: '浙江省',
            key: '330000',
          },
          city: {
            label: '杭州市',
            key: '330100',
          },
        },
        address: '西湖区工专路 77 号',
        phone: '0752-268888888',
      };
      resolve(currentUser);
    }
    else {
      reject('current is null');
    }
  });
}

/** 此处后端没有提供注释 GET /api/notices */
export async function getNotices(options?: { [key: string]: any }) {
  return request<API.NoticeIconList>('/api/notices', {
    method: 'GET',
    ...(options || {}),
  });
}

export async function getAccessToken() {
  return (await userManager.getUser())?.access_token;
}

export async function getConfigurations() {
  return new Promise<API.ApplicationConfiguration>(async (resolve, reject) => {
    let appConfigs = await getAppConfigs();
    if (appConfigs) {
      resolve(appConfigs);
    }
    else {
      reject('appConfig is null');
    }
  });
}