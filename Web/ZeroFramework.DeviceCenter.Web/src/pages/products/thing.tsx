import { Button, Drawer, Empty, message, Popconfirm, Space, Tag, } from 'antd';
import type { ActionType, ProColumns } from '@ant-design/pro-table';
import ProTable from '@ant-design/pro-table';
import { PlusOutlined } from '@ant-design/icons';
import { FooterToolbar, PageContainer } from '@ant-design/pro-layout';
import { useIntl, FormattedMessage, history } from 'umi';
import { getProduct, putProduct } from '@/services/deviceCenter/Products';
import { useEffect, useRef, useState } from 'react';
import ProDescriptions, { ProDescriptionsItemProps } from '@ant-design/pro-descriptions';
import FeatureForm from './components/FeatureForm';
import { ModalForm } from '@ant-design/pro-form';

export type TableColumnType = {
    featureType?: string,
    featureName?: string,
    identifier?: string,
    dataType?: string,
    dataDefinition?: string,
    remark?: string,
    feature: any;
};

export default (props: any) => {
    const [featureFormVisible, setFeatureFormVisible] = useState<boolean>(false);
    const [currentRow, setCurrentRow] = useState<TableColumnType>();
    const [selectedRows, setSelectedRows] = useState<TableColumnType[]>([]);
    const [showDetail, setShowDetail] = useState<boolean>(false);
    const intl = useIntl();
    const tableActionRef = useRef<ActionType>();
    const [tslFormVisible, setTslFormVisible] = useState<boolean>(false);

    const [productFeatures, setProductFeatures] = useState<API.ProductFeatures>();

    const handleRemove = async (selectedRows: TableColumnType[]) => {
        const hide = message.loading(intl.formatMessage({ id: 'pages.table.processing' }));
        if (!selectedRows) {
            return true;
        }
        try {
            let features = { ...productFeatures }

            await Promise.all(selectedRows.map(async (value) => {
                if (value.featureType == 'property') {
                    let propertiesList: API.PropertyFeature[] = [];
                    if (features?.properties) {
                        propertiesList = [...features.properties]
                    }
                    propertiesList = propertiesList.filter(e => e.identifier != value.identifier);
                    features = { ...features, properties: propertiesList }
                }
                else if (value.featureType == 'service') {
                    let serviceList: API.ServiceFeature[] = [];
                    if (features?.services) {
                        serviceList = [...features.services]
                    }
                    serviceList = serviceList.filter(e => e.identifier != value.identifier);
                    features = { ...features, services: serviceList }
                }
                else if (value.featureType == 'event') {
                    let eventList: API.EventFeature[] = [];
                    if (features?.events) {
                        eventList = [...features.events]
                    }
                    eventList = eventList.filter(e => e.identifier != value.identifier);

                    features = { ...features, events: eventList }
                }
                setProductFeatures(features);
            }));
            hide();
            message.success(intl.formatMessage({ id: 'pages.table.successful' }));
            return true;
        } catch (error) {
            hide();
            message.error(intl.formatMessage({ id: 'pages.table.failed' }));
            return false;
        }
    };

    const featuresToTableColumns = (features?: API.ProductFeatures): TableColumnType[] => {

        let properties = features?.properties;
        let services = features?.services;
        let events = features?.events;

        let datas: TableColumnType[] = [];

        if (properties) {
            properties.forEach(item => {
                const dataType = item.dataType as API.DataType;
                datas.push({
                    featureType: 'property',
                    featureName: item.name,
                    identifier: item.identifier,
                    dataType: dataType.type,
                    dataDefinition: dataType.type,
                    remark: item.desc,
                    feature: item,
                });
            });
        }

        if (services) {
            services.forEach(item => {
                datas.push({
                    featureType: 'service',
                    featureName: item.name,
                    identifier: item.identifier,
                    dataDefinition: item.invokeMethod,
                    remark: item.desc,
                    feature: item,
                });
            });
        }

        if (events) {
            events.forEach(item => {
                datas.push({
                    featureType: 'event',
                    featureName: item.name,
                    identifier: item.identifier,
                    dataType: undefined,
                    dataDefinition: item.eventType,
                    remark: item.desc,
                    feature: item,
                });
            });
        }

        return datas;
    }

    const fetchApi = async () => {
        if (props.location.state?.id) {
            let result = await getProduct({ id: props.location.state.id });
            setProductFeatures(result.features);
        }
    }

    useEffect(() => {
        fetchApi();
    }, []);

    const columns: ProColumns<TableColumnType>[] = [
        {
            title: <FormattedMessage id='pages.table.product.thing.featureType' />,
            dataIndex: 'featureType',
            valueType: 'text',
            sorter: false,
            search: false,
            render: (_, record) => {
                switch (record.featureType) {
                    case 'property':
                        return <Tag color='blue'><FormattedMessage id='pages.table.product.thing.property' /></Tag>
                    case 'service':
                        return <Tag color='cyan'><FormattedMessage id='pages.table.product.thing.service' /></Tag>
                    case 'event':
                        return <Tag color='green'><FormattedMessage id='pages.table.product.thing.event' /></Tag>
                    default:
                        return null;
                }
            },
        },
        {
            title: <FormattedMessage id='pages.table.product.thing.featureName' />,
            dataIndex: 'featureName',
            valueType: 'text',
            sorter: false,
            search: false,
        },
        {
            title: <FormattedMessage id='pages.table.product.thing.identifier' />,
            dataIndex: 'identifier',
            valueType: 'text',
            sorter: false,
            search: false,
        },
        {
            title: <FormattedMessage id='pages.table.product.thing.dataType' />,
            dataIndex: 'dataType',
            valueType: 'text',
            sorter: false,
            search: false,
        },
        {
            title: <FormattedMessage id='pages.table.product.thing.dataDefinition' />,
            dataIndex: 'dataDefinition',
            valueType: 'text',
            sorter: false,
            search: false,
        },
        {
            title: <FormattedMessage id='pages.table.product.thing.remark' />,
            dataIndex: 'remark',
            valueType: 'text',
            sorter: false,
            search: { transform: (value) => 'keyword' },
            hideInTable: true,
        },
        {
            title: <FormattedMessage id="pages.searchTable.titleOption" />,
            dataIndex: 'option',
            valueType: 'option',
            render: (_, record) => [
                <a
                    key={record.identifier}
                    onClick={async () => {
                        setCurrentRow(record);
                        setFeatureFormVisible(true);
                    }}
                >
                    <FormattedMessage id="pages.table.edit" />
                </a>,
                <Popconfirm
                    key={record.identifier}
                    title={<FormattedMessage id="pages.table.deleteConfirm" />}
                    onConfirm={async () => {
                        setCurrentRow(record);
                        await handleRemove([record]);
                    }}
                >
                    <a href="#">
                        <FormattedMessage id="pages.table.delete" />
                    </a>
                </Popconfirm>,
            ],
        },
    ];

    return (
        <PageContainer
            onBack={e => { history.goBack(); }}
            title={intl.formatMessage({ id: 'menu.device.manager.product.list' })}
        >
            <ProTable<TableColumnType>
                columns={columns}
                rowKey={e => {
                    if (e?.identifier) {
                        return `${e.featureType}-${e.identifier.toString()}`;
                    }
                    return '';
                }}
                search={false}
                actionRef={tableActionRef}
                pagination={false}
                dateFormatter="string"
                headerTitle={<Space>
                    <Button
                        type="primary"
                        key="createFeature"
                        onClick={() => {
                            setCurrentRow(undefined)
                            setFeatureFormVisible(true);
                        }}
                    >
                        <PlusOutlined />
                        <FormattedMessage id="pages.searchTable.new" />
                    </Button>
                    <Button
                        type="primary"
                        key="tslModel"
                        onClick={() => {
                            setTslFormVisible(true);
                        }}
                    >
                        <FormattedMessage id="pages.table.product.thing.tslModel" />
                    </Button>
                    <Button
                        type="primary"
                        key="publish"
                        onClick={async () => {
                            if (props.location.state?.id) {
                                const hide = message.loading(intl.formatMessage({ id: 'pages.table.processing' }));
                                const result = await putProduct({ id: props.location.state.id }, { ...props.location.state, features: productFeatures });
                                if (result?.id) {
                                    hide();
                                    message.success(intl.formatMessage({ id: 'pages.table.successful' }));
                                }
                            }
                        }}
                    >
                        <FormattedMessage id="pages.table.product.thing.publish" />
                    </Button>
                </Space>}
                options={{
                    search: false,
                    fullScreen: true,
                }}
                rowSelection={{
                    onChange: (_, selectedRows) => {
                        setSelectedRows(selectedRows);
                    },
                }}

                dataSource={featuresToTableColumns(productFeatures)}
            />
            {selectedRows?.length > 0 && (
                <FooterToolbar
                    extra={
                        <div>
                            <FormattedMessage id="pages.searchTable.chosen" defaultMessage="已选择" />{' '}
                            <a style={{ fontWeight: 600 }}>{selectedRows.length}</a>{' '}
                            <FormattedMessage id="pages.searchTable.item" defaultMessage="项" />
                        </div>
                    }
                >
                    <Button type="primary"
                        onClick={async () => {
                            await handleRemove(selectedRows);
                            setSelectedRows([]);
                            tableActionRef.current?.reloadAndRest?.();
                        }}
                    >
                        <FormattedMessage id="pages.searchTable.batchDeletion" />
                    </Button>
                </FooterToolbar>
            )}
            <Drawer
                width={400}
                visible={showDetail}
                onClose={() => {
                    setCurrentRow(undefined);
                    setShowDetail(false);
                }}
                closable={false}
            >
                {currentRow?.identifier && (
                    <ProDescriptions<TableColumnType>
                        column={1}
                        title={currentRow?.featureName}
                        request={async () => {
                            return { data: currentRow || {}, };
                        }}
                        params={{
                            id: currentRow?.identifier,
                        }}
                        columns={columns as ProDescriptionsItemProps<TableColumnType>[]}
                    />
                )}
            </Drawer>
            <FeatureForm
                initialValues={currentRow}
                onSubmit={async (formData: Record<string, any>) => {
                    if (formData.featureType == 'property') {
                        let propertiesList: API.PropertyFeature[] = [];
                        if (productFeatures?.properties) {
                            propertiesList = [...productFeatures.properties]
                        }
                        const existed = propertiesList.find(e => e.identifier == formData.identifier);
                        if (existed) {
                            propertiesList = propertiesList.filter(e => e.identifier != formData.identifier);
                        }
                        propertiesList = [formData, ...propertiesList];
                        const features = { ...productFeatures, properties: propertiesList }
                        setProductFeatures(features);
                    }
                    else if (formData.featureType == 'service') {
                        let serviceList: API.ServiceFeature[] = [];
                        if (productFeatures?.services) {
                            serviceList = [...productFeatures.services]
                        }
                        const existed = serviceList.find(e => e.identifier == formData.identifier);
                        if (existed) {
                            serviceList = serviceList.filter(e => e.identifier != formData.identifier);
                        }
                        serviceList = [formData, ...serviceList];
                        const features = { ...productFeatures, services: serviceList }
                        setProductFeatures(features);
                    }
                    else if (formData.featureType == 'event') {
                        let eventList: API.EventFeature[] = [];
                        if (productFeatures?.events) {
                            eventList = [...productFeatures.events]
                        }
                        const existed = eventList.find(e => e.identifier == formData.identifier);
                        if (existed) {
                            eventList = eventList.filter(e => e.identifier != formData.identifier);
                        }
                        eventList = [formData, ...eventList];
                        const features = { ...productFeatures, events: eventList }
                        setProductFeatures(features);
                    }
                    return true;
                }}
                onCancel={() => {
                    setCurrentRow(undefined);
                    setFeatureFormVisible(false);
                }}
                visible={featureFormVisible}
            />
            <ModalForm<API.CreateProductCommand>
                autoComplete="off"
                visible={tslFormVisible}
                onVisibleChange={setTslFormVisible}
                title={intl.formatMessage({
                    id: 'pages.table.product.thing.tslModel',
                })}
                onFinish={async (value) => {
                    setTslFormVisible(false);
                    return true;
                }}
                submitter={{
                    render: (props, doms) => {
                        return (
                            <Space style={{
                                width: '100%',
                                display: 'flex',
                                justifyContent: 'flex-end',
                            }}>
                                {doms.reverse()}
                            </Space>
                        )
                    },
                }}
                style={{ overflow: 'scroll' }}
            >
                <pre>
                    {productFeatures ? JSON.stringify(productFeatures, null, 2) : <Empty image={Empty.PRESENTED_IMAGE_SIMPLE} />}
                </pre>
            </ModalForm>
        </PageContainer>
    );
};