// @ts-ignore
/* eslint-disable */

declare namespace API {
  type ApplicationConfiguration = {
    permissions?: PermissionConfiguration;
    localizations?: LocalizationConfiguration;
  };

  type CreateProductCommand = {
    name?: string;
    nodeType?: ProductNodeType;
    netType?: ProductNetType;
    protocolType?: ProductProtocolType;
    dataFormat?: ProductDataFormat;
    features?: ProductFeatures;
    remark?: string;
    creationTime?: string;
  };

  type DataParameter = {
    identifier?: string;
    parameterName?: string;
    dataType?: DataType;
  };

  type DataType = {
    type?: DataTypeDefinitions;
    specs?: Record<string, any>;
  };

  type DataTypeDefinitions =
    | 'int32'
    | 'int64'
    | 'float'
    | 'double'
    | 'enum'
    | 'bool'
    | 'string'
    | 'date'
    | 'struct'
    | 'array';

  type DeviceCreateRequestModel = {
    name?: string;
    status?: DeviceStatus;
    productId?: string;
    coordinate?: string;
    remark?: string;
    lastOnlineTime?: string;
    creationTime?: string;
  };

  type DeviceGetResponseModel = {
    id?: number;
    name?: string;
    status?: DeviceStatus;
    productId?: string;
    product?: ProductGetResponseModel;
    coordinate?: string;
    remark?: string;
    lastOnlineTime?: string;
    creationTime?: string;
  };

  type DeviceGetResponseModelPagedResponseModel = {
    items?: DeviceGetResponseModel[];
    totalCount?: number;
  };

  type DeviceGroupCreateRequestModel = {
    name?: string;
    remark?: string;
    creationTime?: string;
    parentId?: number;
  };

  type DeviceGroupGetResponseModel = {
    id?: number;
    name?: string;
    remark?: string;
    creationTime?: string;
    parentId?: number;
    children?: DeviceGroupGetResponseModel[];
  };

  type DeviceGroupGetResponseModelPagedResponseModel = {
    items?: DeviceGroupGetResponseModel[];
    totalCount?: number;
  };

  type DeviceGroupUpdateRequestModel = {
    id?: number;
    name?: string;
    remark?: string;
    parentId?: number;
  };

  type DevicePropertyLastValue = {
    timestamp?: number;
    value?: any;
    identifier?: string;
    name?: string;
    unit?: string;
  };

  type DevicePropertyReport = {
    date?: string;
    minValue?: number;
    averageValue?: number;
    maxValue?: number;
  };

  type DevicePropertyReportPageableListResposeModel = {
    items?: DevicePropertyReport[];
    nextOffset?: number;
  };

  type DevicePropertyValue = {
    timestamp?: number;
    value?: any;
  };

  type DevicePropertyValuePageableListResposeModel = {
    items?: DevicePropertyValue[];
    nextOffset?: number;
  };

  type DeviceStatisticGetResponseModel = {
    totalCount?: number;
    onlineCount?: number;
    offlineCount?: number;
    unactiveCount?: number;
  };

  type DeviceStatus = 'unactive' | 'online' | 'offline';

  type DeviceUpdateRequestModel = {
    id?: number;
    name?: string;
    coordinate?: string;
    remark?: string;
  };

  type EventFeature = {
    identifier?: string;
    name?: string;
    desc?: string;
    outputData?: DataParameter[];
    eventType?: EventType;
  };

  type EventType = 'info' | 'alert' | 'error';

  type LocalizationConfiguration = {
    supportedCultures?: string[];
    currentCulture?: string;
    values?: Record<string, any>;
  };

  type MeasurementUnitCreateRequestModel = {
    unitName?: string;
    unit?: string;
    remark?: string;
  };

  type MeasurementUnitGetResponseModel = {
    id?: number;
    unitName?: string;
    unit?: string;
    remark?: string;
  };

  type MeasurementUnitGetResponseModelPagedResponseModel = {
    items?: MeasurementUnitGetResponseModel[];
    totalCount?: number;
  };

  type MeasurementUnitUpdateRequestModel = {
    id?: number;
    unitName?: string;
    unit?: string;
    remark?: string;
  };

  type MonitoringFactorCreateRequestModel = {
    factorCode?: string;
    chineseName?: string;
    englishName?: string;
    unit?: string;
    decimals?: number;
    remarks?: string;
  };

  type MonitoringFactorGetResponseModel = {
    id?: number;
    factorCode?: string;
    chineseName?: string;
    englishName?: string;
    unit?: string;
    decimals?: number;
    remarks?: string;
  };

  type MonitoringFactorGetResponseModelPagedResponseModel = {
    items?: MonitoringFactorGetResponseModel[];
    totalCount?: number;
  };

  type MonitoringFactorUpdateRequestModel = {
    id?: number;
    factorCode?: string;
    chineseName?: string;
    englishName?: string;
    unit?: string;
    decimals?: number;
    remarks?: string;
  };

  type PermissionConfiguration = {
    policies?: Record<string, any>;
    grantedPolicies?: Record<string, any>;
  };

  type PermissionGrantInfoModel = {
    name?: string;
    isGranted?: boolean;
  };

  type PermissionGrantModel = {
    name?: string;
    displayName?: string;
    parentName?: string;
    isGranted?: boolean;
    allowedProviders?: string[];
  };

  type PermissionGroupModel = {
    name?: string;
    displayName?: string;
    permissions?: PermissionGrantModel[];
  };

  type PermissionListResponseModel = {
    entityDisplayName?: string;
    groups?: PermissionGroupModel[];
  };

  type PermissionProviderInfoModel = {
    providerName?: string;
    providerKey?: string;
  };

  type PermissionUpdateRequestModel = {
    providerInfos?: PermissionProviderInfoModel[];
    permissionGrantInfos?: PermissionGrantInfoModel[];
    resourceGroupId?: string;
  };

  type ProblemDetails = {
    type?: string;
    title?: string;
    status?: number;
    detail?: string;
    instance?: string;
  };

  type ProductDataFormat = 'json' | 'custom';

  type ProductFeatures = {
    properties?: PropertyFeature[];
    services?: ServiceFeature[];
    events?: EventFeature[];
  };

  type ProductGetResponseModel = {
    id?: string;
    name?: string;
    nodeType?: ProductNodeType;
    netType?: ProductNetType;
    protocolType?: ProductProtocolType;
    dataFormat?: ProductDataFormat;
    features?: ProductFeatures;
    remark?: string;
    creationTime?: string;
  };

  type ProductGetResponseModelPagedResponseModel = {
    items?: ProductGetResponseModel[];
    totalCount?: number;
  };

  type ProductNetType = 'wifi' | 'cellular' | 'ethernet' | 'other';

  type ProductNodeType = 'direct' | 'gateway' | 'subset';

  type ProductProtocolType =
    | 'mqtt'
    | 'coap'
    | 'lwm2m'
    | 'http'
    | 'modbus'
    | 'opcua'
    | 'ble'
    | 'zigbee'
    | 'other';

  type ProductUpdateRequestModel = {
    id?: string;
    name?: string;
    features?: ProductFeatures;
    remark?: string;
  };

  type ProjectCreateOrUpdateRequestModel = {
    id?: number;
    name?: string;
    creationTime?: string;
  };

  type ProjectGetResponseModel = {
    id?: number;
    name?: string;
    creationTime?: string;
  };

  type ProjectGetResponseModelPagedResponseModel = {
    items?: ProjectGetResponseModel[];
    totalCount?: number;
  };

  type PropertyAccessMode = 'read' | 'write' | 'readWrite';

  type PropertyFeature = {
    identifier?: string;
    name?: string;
    desc?: string;
    dataType?: DataType;
    accessMode?: PropertyAccessMode;
  };

  type ResourceGroupCreateRequestModel = {
    name?: string;
    displayName?: string;
  };

  type ResourceGroupGetResponseModel = {
    id?: string;
    name?: string;
    displayName?: string;
    tenantId?: string;
  };

  type ResourceGroupGetResponseModelPagedResponseModel = {
    items?: ResourceGroupGetResponseModel[];
    totalCount?: number;
  };

  type ResourceGroupUpdateRequestModel = {
    id?: string;
    name?: string;
    displayName?: string;
  };

  type ServiceFeature = {
    identifier?: string;
    name?: string;
    desc?: string;
    invokeMethod?: ServiceInvokeMethod;
    inputData?: DataParameter[];
    outputData?: DataParameter[];
  };

  type ServiceInvokeMethod = 'async' | 'await';

  type SortingOrder = 'ascending' | 'descending';

  type StringDevicePropertyValueKeyValuePair = {
    key?: string;
    value?: DevicePropertyValue;
  };
}
