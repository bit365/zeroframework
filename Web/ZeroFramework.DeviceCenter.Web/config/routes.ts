export default [
  {
    path: '/authorization',
    layout: false,
    routes: [
      {
        name: 'login',
        path: '/authorization/login',
        component: './authorization/login',
      },
      {
        path: '/authorization/login-callback',
        component: './authorization/login-callback',
      },
      {
        path: '/authorization/logout',
        component: './authorization/logout',
      },
      {
        path: '/authorization/logout-callback',
        component: './authorization/logout-callback',
      },
      {
        component: './404',
      }
    ],
  },
  {
    path: '/welcome',
    name: 'welcome',
    icon: 'HomeOutlined',
    component: './Welcome',
  },
  {
    path: '/device',
    name: 'device.manager',
    icon: 'cluster',
    routes: [
      {
        path: '/device/product',
        name: 'product.list',
        access: 'ProductManager.Products',
        component: './products/list',
      },
      {
        name: 'thing.list',
        path: '/device/product/thing',
        access: 'ProductManager.Products',
        component: './products/thing',
        hideInMenu: true,
      },
      {
        path: '/device/device',
        name: 'device.list',
        access: 'DeviceManager.Devices',
        component: './devices/list',
      },
      {
        name: 'device.view',
        path: '/device/device/view',
        access: 'DeviceManager.Devices',
        component: './devices/view',
        hideInMenu: true,
      },
      {
        path: '/device/device-group',
        name: 'deviceGroup.list',
        component: './device-group',
        access: 'DeviceGroupManager.DeviceGroups',
      },
      {
        name: 'deviceGroup.view',
        path: '/device/device-group/view',
        access: 'DeviceGroupManager.DeviceGroups',
        component: './device-group/view',
        hideInMenu: true,
      },
      {
        path: '/device/monitoring-factor',
        name: 'monitoringFactor.list',
        access: 'MonitoringFactorManager.MonitoringFactors',
        component: './monitoring-factor',
      },
      {
        component: './404',
      }
    ],
  },
  {
    path: '/data-report',
    name: 'dataReport',
    icon: 'BarChartOutlined',
    access: 'MeasurementManager.Measurements',
    routes: [
      {
        path: '/data-report/real-time',
        name: 'realTimeData',
        access: 'MeasurementManager.Measurements.DevicePropertyValues',
        component: './data-report/real-time',
      },
      {
        path: '/data-report/history-data',
        name: 'historyData',
        access: 'MeasurementManager.Measurements.DevicePropertyHistoryValues',
        component: './data-report/history-data',
      },
      {
        path: '/data-report/report-statistics',
        name: 'reportStatistics',
        access: 'MeasurementManager.Measurements.DevicePropertyReports',
        component: './data-report/report-statistics',
      },
      {
        path: '/data-report/alarm-record',
        name: 'alarmRecord',
        access: 'MeasurementManager.Measurements.DevicePropertyReports',
        component: './404',
      },
      {
        component: './404',
      }
    ],
  },
  {
    path: '/data-visualization',
    name: 'dataVisualization',
    icon: 'SettingOutlined',
    routes: [
      {
        path: '/data-visualization/map',
        name: 'mapView',
        access: 'MeasurementManager.Measurements.DevicePropertyValues',
        component: './data-visualization/maps',
      },
      {
        path: '/data-visualization/monitor',
        name: 'monitorView',
        component: './404',
      },
      {
        component: './404',
      }
    ],
  },
  {
    path: '/rule-engine',
    name: 'ruleEngine',
    icon: 'FunnelPlotOutlined',
    routes: [
      {
        path: '/rule-engine/scene-orchestration',
        name: 'sceneOrchestration',
        component: './404',
      },
      {
        path: '/rule-engine/data-forwarding',
        name: 'dataForwarding',
        component: './404',
      },
      {
        path: '/rule-engine/data-Subscription',
        name: 'dataSubscription',
        component: './404',
      },
      {
        component: './404',
      }
    ],
  },
  {
    path: '/maintenance',
    name: 'maintenance',
    icon: 'ExperimentOutlined',
    routes: [
      {
        path: '/maintenance/device-log',
        name: 'deviceLog',
        component: './404',
      },
      {
        path: '/maintenance/ota-update',
        name: 'otaUpdate',
        component: './404',
      },
      {
        path: '/maintenance/device-debug',
        name: 'deviceDebug',
        component: './404',
      },
      {
        component: './404',
      }
    ],
  },
  {
    path: '/identity',
    name: 'identity.manager',
    icon: 'user',
    access: 'canIdentityManager',
    routes: [
      {
        path: '/identity/user',
        name: 'user.list',
        component: './identity/user',
      },
      {
        path: '/identity/role',
        name: 'role.list',
        component: './identity/role',
      },
      {
        path: '/identity/tenant',
        name: 'tenant.list',
        access: 'canTenantManager',
        component: './identity/tenant',
      },
      {
        path: '/identity/resource-group',
        name: 'resourceGroup.list',
        access: 'ResourceGroupManager.ResourceGroups',
        component: './resource-group',
      },
      {
        component: './404',
      }
    ],
  },
  {
    path: '/video-service',
    name: 'videoService',
    icon: 'VideoCameraOutlined',
    component: './404',
  },
  {
    path: '/settings',
    name: 'settings',
    icon: 'SettingOutlined',
    routes: [
      {
        path: '/settings/account',
        name: 'account',
        component: './404',
      },
      {
        path: '/settings/help',
        name: 'help',
        component: './404',
      },
      {
        component: './404',
      }
    ],
  },
  {
    path: '/open-platform',
    name: 'openPlatform',
    icon: 'CloudSyncOutlined',
    routes: [
      {
        path: '/open-platform/identity-server',
        name: 'identityServer',
        component: './404',
      },
      {
        path: '/open-platform/device-center',
        name: 'deviceCenter',
        component: './404',
      },
      {
        component: './404',
      }
    ],
  },
  {
    path: '/',
    redirect: '/device/product',
  },
  {
    component: './404',
  },
];