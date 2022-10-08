/**
 * 在生产环境 代理是无法生效的，所以这里没有生产环境的配置
 * The agent cannot take effect in the production environment
 * so there is no configuration of the production environment
 * For details, please see
 * https://pro.ant.design/docs/deploy
 */
export default {
  dev: {
    '/api/User': {
      target: 'https://localhost:5001',
      changeOrigin: true,
      pathRewrite: { '^': '' },
      secure: false,
    },
    '/api/Roles': {
      target: 'https://localhost:5001',
      changeOrigin: true,
      pathRewrite: { '^': '' },
      secure: false,
    },
    '/api/rule': {
      target: 'https://proapi.azurewebsites.net',
      changeOrigin: true,
      pathRewrite: { '^': '' },
    },
    '/api/currentUser': {
      target: 'https://proapi.azurewebsites.net',
      changeOrigin: true,
      pathRewrite: { '^': '' },
      secure: false,
    },
    '/api/geographic/province': {
      target: 'https://proapi.azurewebsites.net',
      changeOrigin: true,
      pathRewrite: { '^': '' },
      secure: false,
    },
    '/api/fake_list': {
      target: 'https://proapi.azurewebsites.net',
      changeOrigin: true,
      pathRewrite: { '^': '' },
      secure: false,
    },
    '/api/Tenants': {
      target: 'https://localhost:5001',
      changeOrigin: true,
      pathRewrite: { '^': '' },
      secure: false,
    },
    '/api': {
      target: 'https://localhost:6001',
      changeOrigin: true,
      pathRewrite: { '^': '' },
      secure: false,
    },
  },
  test: {
    '/api/': {
      target: 'https://preview.pro.ant.design',
      changeOrigin: true,
      pathRewrite: { '^': '' },
    },
  },
  pre: {
    '/api/': {
      target: 'your pre url',
      changeOrigin: true,
      pathRewrite: { '^': '' },
    },
  },
};