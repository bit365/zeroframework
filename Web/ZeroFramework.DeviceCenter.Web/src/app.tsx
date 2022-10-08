import type { Settings as LayoutSettings } from '@ant-design/pro-layout';
import { PageLoading } from '@ant-design/pro-layout';
import { notification } from 'antd';
import { RequestConfig, RunTimeLayoutConfig } from 'umi';
import type { RequestOptionsInit } from 'umi-request'
import { history } from 'umi';
import RightContent from '@/components/RightContent';
import Footer from '@/components/Footer';
import type { ResponseError } from 'umi-request';
import { getConfigurations } from '@/pages/authorization/services/user-service';
import { FormattedMessage, useIntl } from 'umi';
import { getLanguage } from '@ant-design/pro-layout/lib/locales';
import { User } from 'oidc-client';
import { userManager } from './pages/authorization/services/AuthorizeService';

/** 获取用户信息比较慢的时候会展示一个 loading */
export const initialStateConfig = {
  loading: <PageLoading />,
};

/**
 * @see  https://umijs.org/zh-CN/plugins/plugin-initial-state
 * */
export async function getInitialState(): Promise<{
  settings?: Partial<LayoutSettings>;
  appConfigs?: API.ApplicationConfiguration;
  currentUser?: User;
}> {
  const appConfigs = await getConfigurations();
  return {
    currentUser: await userManager.getUser() ?? undefined,
    settings: {},
    appConfigs: appConfigs,
  };
}

// https://umijs.org/zh-CN/plugins/plugin-layout
export const layout: RunTimeLayoutConfig = ({ initialState }) => {
  return {
    headerTitleRender: (logo) => (<a>{logo}<h1><FormattedMessage id='site.title' /></h1></a>),
    pageTitleRender: (_props, _defaultPageTitle, info) => `${info?.pageName}-${useIntl().formatMessage({ id: 'site.title' })}`,
    rightContentRender: () => <RightContent />,
    disableContentMargin: false,
    footerRender: () => <Footer />,
    onPageChange: () => {
      const { location } = history;
      // 如果没有登录，重定向到 login
      if (!initialState?.currentUser && !location.pathname.startsWith('/authorization')) {
        history.push(`/authorization/login?returnUrl=${encodeURIComponent(window.location.href)}`);
      }
    },
    links: [],
    menuHeaderRender: undefined,
    // 自定义 403 页面
    // unAccessible: <div>unAccessible</div>,
    ...initialState?.settings,
  };
};

const codeMessage = {
  200: '服务器成功返回请求的数据。',
  201: '新建或修改数据成功。',
  202: '一个请求已经进入后台排队（异步任务）。',
  204: '删除数据成功。',
  400: '发出的请求有错误，服务器没有进行新建或修改数据的操作。',
  401: '用户没有权限（令牌、用户名、密码错误）。',
  403: '用户得到授权，但是访问是被禁止的。',
  404: '发出的请求针对的是不存在的记录，服务器没有进行操作。',
  405: '请求方法不被允许。',
  406: '请求的格式不可得。',
  410: '请求的资源被永久删除，且不会再得到的。',
  422: '当创建一个对象时，发生一个验证错误。',
  500: '服务器发生错误，请检查服务器。',
  502: '网关错误。',
  503: '服务不可用，服务器暂时过载或维护。',
  504: '网关超时。',
};

/** 异常处理程序
 * @see https://beta-pro.ant.design/docs/request-cn
 */
const errorHandler = (error: ResponseError) => {
  const { response, data } = error;

  // https://tools.ietf.org/html/rfc7807
  if (data && data.status && data.title) {
    if (data.status < 400) {
      notification.info({
        message: data.title,
        description: data.detail,
      });
    }
    else if (data.status < 500) {
      notification.warn({
        message: data.title,
        description: data.detail,
      });
    }
    else {
      notification.error({
        message: data.title,
        description: data.detail,
      });
    }
    return;
  }

  if (response && response.status) {
    const errorText = codeMessage[response.status] || response.statusText;
    const { status, url } = response;

    if (status === 401) {
      history.push(`/authorization/login?returnUrl=${encodeURIComponent(window.location.href)}`);
      return;
    }

    notification.error({
      message: `请求错误 ${status}: ${url}`,
      description: errorText,
    });
  }

  if (!response) {
    notification.error({
      description: '您的网络发生异常，无法连接服务器',
      message: '网络异常',
    });
  }
  throw error;
};

// https://umijs.org/zh-CN/plugins/plugin-request
export const request: RequestConfig = {
  errorHandler,
  middlewares: [
    async function (ctx, next) {
      const user = await userManager.getUser();
      let token = user?.access_token;
      if (token) {
        const authHeader = { Authorization: `Bearer ${token}` };
        ctx.req.options.headers = { ...ctx.req.options.headers, ...authHeader };
        ctx.req = {
          url: `${ctx.req.url}`,
          options: { ...ctx.req.options },
        }
      }
      await next();
    }
  ],
  requestInterceptors: [(url: string, options: RequestOptionsInit) => {
    options.headers = { ...options.headers, "accept-language": getLanguage() };
    return {
      url: `${url}`,
      options: { ...options },
    };
  }],
  responseInterceptors: [(response: Response, options: RequestOptionsInit) => {
    console.log(response);
    return response;
  }],
  errorConfig: {
    adaptor: (resData, ctx) => {
      // https://tools.ietf.org/html/rfc7807
      if (resData && resData.status && resData.title) {
        return {
          ...resData,
          success: false,
          errorMessage: `${resData.title}「${resData.detail}」`,
          errorCode: resData.status,
          showType: 1,
          traceId: resData.traceId,
        };
      }
      return {
        ...resData,
        success: true,
      };
    }
  },
};