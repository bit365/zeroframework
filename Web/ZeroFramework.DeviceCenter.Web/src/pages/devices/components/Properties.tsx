import { useEffect, useRef, useState } from 'react';
import {
  Badge,
  Button,
  Card,
  Col,
  Form,
  Row,
  Statistic,
  Modal,
  Empty,
  Table,
  Input,
  FormInstance,
  Space,
} from 'antd';

import { useIntl } from 'umi';
import { ProFormDateTimeRangePicker, ProFormRadio, QueryFilter } from '@ant-design/pro-form';
import moment from 'moment';
import {
  getDevicePropertyHistoryValues,
  getDevicePropertyValues,
} from '@/services/deviceCenter/Measurements';
import { Spin } from 'antd';
import { LeftOutlined, RightOutlined } from '@ant-design/icons';
import { Line } from '@ant-design/charts';

export type PropertiesProps = {
  device: API.DeviceGetResponseModel;
};

export default (props: PropertiesProps) => {
  const intl = useIntl();
  const [deviceInfo, setDeviceInfo] = useState<API.DeviceGetResponseModel>();
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [propertyIdentifier, setPropertyIdentifier] = useState<string>();
  const [devicePropertyValues, setDevicePropertyValues] = useState<API.DevicePropertyLastValue[]>();
  const [loading, setLoading] = useState<boolean>(true);
  const [showType, setShowType] = useState<string>('table');

  const [dataSourceState, setDataSourceState] = useState<{
    loading: boolean;
    data?: API.DevicePropertyValue[];
  }>({ loading: false, data: [] });

  const [paginationState, setPaginationState] = useState({
    hasPrevious: false,
    hasNext: false,
    pageNumber: 1,
  });

  const [dateTimeRang, setDateTimeRang] = useState({
    from: moment().startOf('day'),
    to: moment().endOf('day'),
  });

  const formRef = useRef<FormInstance>();

  const fetchHistoryData = async () => {
    setDataSourceState({ loading: true, data: dataSourceState.data });

    const values = formRef.current?.getFieldsValue();

    const result = await getDevicePropertyHistoryValues(
      {
        productId: deviceInfo?.productId,
        deviceId: deviceInfo?.id,
        identifier: propertyIdentifier,
        startTime: dateTimeRang.from.toISOString(),
        endTime: dateTimeRang.to.toISOString(),
        sorting: 'descending' as API.SortingOrder,
        pageNumber: values.pageNumber,
        pageSize: 10,
      },
      { errorHandler: () => {} },
    );

    paginationState.hasPrevious = values.pageNumber > 1;
    paginationState.hasNext = result.nextOffset != null;
    paginationState.pageNumber = values.pageNumber;

    setPaginationState(paginationState);

    setDataSourceState({
      loading: false,
      data: result.items || [],
    });
  };

  const fetchDevicePropertyValuesApi = async (productId: string, deviceId?: number) => {
    setLoading(true);
    const result = await getDevicePropertyValues(
      { productId, deviceId },
      { errorHandler: () => {} },
    );
    if (result) {
      setDevicePropertyValues(result);
    }
    setLoading(false);
  };

  useEffect(() => {
    setDeviceInfo(props.device);
    if (props.device.product?.id) {
      fetchDevicePropertyValuesApi(props.device.product.id, props.device.id);
    }
  }, []);

  useEffect(() => {
    if (isModalVisible) {
      fetchHistoryData();
    } else {
      setDataSourceState({ loading: false, data: [] });
      setPaginationState({ hasPrevious: false, hasNext: false, pageNumber: 1 });
    }
  }, [isModalVisible, dateTimeRang]);

  const chartConfig = {
    data: dataSourceState.data || [],
    xField: 'timestamp',
    yField: 'value',
    point: {
      size: 4,
      shape: 'circle',
      style: {
        fill: 'white',
        stroke: '#5B8FF9',
        lineWidth: 1,
      },
      visible: true,
    },
    meta: {
      timestamp: {
        formatter: function (v: number) {
          return moment(v).format('YYYY-MM-DD HH:mm:ss');
        },
      },
      value: {
        alias: intl.formatMessage({ id: 'pages.devices.view.properties.history.value' }),
        formatter: function (v: number) {
          return v.toFixed(3);
        },
      },
    },
    xAxis: {
      label: {
        autoHide: true,
        autoRotate: false,
      },
    },
    loading: dataSourceState.loading,
  };

  return (
    <>
      <Spin spinning={loading}>
        <Row gutter={[24, 24]}>
          {devicePropertyValues?.map((e) => {
            return (
              <Col xs={24} sm={24} md={12} lg={12} xl={8} xxl={6} key={e.identifier}>
                <Card>
                  <a
                    style={{ float: 'right' }}
                    onClick={(_) => {
                      setPropertyIdentifier(e.identifier);
                      setIsModalVisible(true);
                    }}
                  >
                    {intl.formatMessage({ id: 'pages.devices.view.properties.history' })}
                  </a>
                  <Statistic
                    title={e.name}
                    value={e.value}
                    precision={2}
                    valueStyle={{ color: '#3f8600' }}
                    prefix={<Badge color="cyan" />}
                    suffix={`${e.unit || ''}`}
                  />
                </Card>
              </Col>
            );
          }) || (
            <Col span={24}>
              <Empty image={Empty.PRESENTED_IMAGE_SIMPLE} />
            </Col>
          )}
        </Row>
      </Spin>
      <Modal
        width={1000}
        title={intl.formatMessage({ id: 'pages.devices.view.properties.history' })}
        visible={isModalVisible}
        onOk={() => {
          setIsModalVisible(false);
        }}
        onCancel={() => {
          setIsModalVisible(false);
        }}
        footer={[
          <Space key="page1" hidden={!paginationState.hasPrevious && !paginationState.hasNext}>
            <Button
              style={{ fontSize: 12 }}
              disabled={!paginationState.hasPrevious}
              onClick={async () => {
                formRef.current?.setFieldsValue({ pageNumber: paginationState.pageNumber - 1 });
                await fetchHistoryData();
              }}
            >
              <LeftOutlined />
            </Button>
            <Button
              style={{ fontSize: 12, marginRight: 8 }}
              disabled={!paginationState.hasNext}
              onClick={async () => {
                formRef.current?.setFieldsValue({ pageNumber: paginationState.pageNumber + 1 });
                await fetchHistoryData();
              }}
            >
              <RightOutlined />
            </Button>
          </Space>,
        ]}
      >
        <QueryFilter
          formRef={formRef}
          defaultCollapsed={false}
          layout="horizontal"
          optionRender={(searchConfig, props, dom) => {
            return [];
          }}
        >
          <ProFormRadio.Group
            key="flter1"
            name="showType"
            radioType="button"
            options={[
              {
                label: intl.formatMessage({
                  id: 'pages.devices.view.properties.history.showType.table',
                }),
                value: 'table',
              },
              {
                label: intl.formatMessage({
                  id: 'pages.devices.view.properties.history.showType.chart',
                }),
                value: 'chart',
              },
            ]}
            initialValue={showType}
            rules={[{ required: true }]}
            fieldProps={{
              onChange: (v) => {
                setShowType(v.target.value);
              },
            }}
          />
          <ProFormDateTimeRangePicker
            fieldProps={{
              ranges: {
                [intl.formatMessage({ id: 'pages.devices.view.properties.history.date.hour' })]: [
                  moment().startOf('hour'),
                  moment().endOf('hour'),
                ],
                [intl.formatMessage({ id: 'pages.devices.view.properties.history.date.day' })]: [
                  moment().startOf('day'),
                  moment().endOf('day'),
                ],
                [intl.formatMessage({
                  id: 'pages.devices.view.properties.history.date.yesterday',
                })]: [
                  moment().startOf('day').subtract(1, 'days'),
                  moment().endOf('day').subtract(1, 'days'),
                ],
                [intl.formatMessage({ id: 'pages.devices.view.properties.history.date.week' })]: [
                  moment().startOf('week'),
                  moment().endOf('week'),
                ],
                [intl.formatMessage({ id: 'pages.devices.view.properties.history.date.month' })]: [
                  moment().startOf('month'),
                  moment().endOf('month'),
                ],
              },
              format: 'YYYY-MM-DD HH:mm',
              onChange: (values, dateStrings) => {
                formRef.current?.setFieldsValue({ pageNumber: 1 });
                if (values && values[0] && values[1]) {
                  setDateTimeRang({ from: values[0], to: values[1] });
                }
              },
              value: [dateTimeRang.from, dateTimeRang.to],
            }}
          />
          <Form.Item name="pageNumber" initialValue={1}>
            <Input type="hidden" />
          </Form.Item>
        </QueryFilter>
        {showType == 'chart' ? (
          chartConfig.data && chartConfig.data.length ? (
            <Line {...chartConfig} />
          ) : (
            <Empty image={Empty.PRESENTED_IMAGE_SIMPLE} />
          )
        ) : (
          <Table
            columns={[
              {
                key: 'timestamp',
                title: intl.formatMessage({ id: 'pages.devices.view.properties.history.dateTime' }),
                dataIndex: 'timestamp',
                render: (text) => moment(text).format('YYYY-MM-DD HH:mm:ss'),
              },
              {
                key: 'value',
                title: intl.formatMessage({ id: 'pages.devices.view.properties.history.value' }),
                dataIndex: 'value',
              },
            ]}
            rowKey="timestamp"
            dataSource={dataSourceState.data}
            loading={dataSourceState.loading}
            bordered
            pagination={false}
            size="small"
          />
        )}
      </Modal>
    </>
  );
};
