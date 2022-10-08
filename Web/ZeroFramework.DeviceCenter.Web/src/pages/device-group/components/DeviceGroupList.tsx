import type { FormInstance } from 'antd';
import { Button, Drawer, Form, message, Popconfirm, Space, TreeSelect } from 'antd';
import type { ActionType, ProColumns } from '@ant-design/pro-table';
import ProTable from '@ant-design/pro-table';
import { PlusOutlined } from '@ant-design/icons';
import { FooterToolbar } from '@ant-design/pro-layout';
import { useIntl, FormattedMessage, history } from 'umi';
import { deleteDeviceGroup, getDeviceGroup, getDeviceGroups, postDeviceGroup, putDeviceGroup } from '@/services/deviceCenter/DeviceGroups';
import { useEffect, useRef, useState } from 'react';
import type { ProDescriptionsItemProps } from '@ant-design/pro-descriptions';
import ProDescriptions from '@ant-design/pro-descriptions';
import { ModalForm, ProFormText, ProFormTextArea } from '@ant-design/pro-form';
import UpdateForm from './UpdateForm';
import type { DataNode } from 'antd/lib/tree';

export type DeviceGroupListProps = {
    parentGroupId?: number;
};

export default (props: DeviceGroupListProps) => {
    const [createModalVisible, setCreateModalVisible] = useState<boolean>(false);
    const [updateModalVisible, setUpdateModalVisible] = useState<boolean>(false);
    const [currentRow, setCurrentRow] = useState<API.DeviceGroupGetResponseModel>();
    const [selectedRows, setSelectedRows] = useState<API.DeviceGroupGetResponseModel[]>([]);
    const [showDetail, setShowDetail] = useState<boolean>(false);
    const intl = useIntl();
    const tableActionRef = useRef<ActionType>();
    const createFormRef = useRef<FormInstance>();
    const [deviceGroupTreeData, setDeviceGroupTreeData] = useState<DataNode[]>([]);

    useEffect(() => {
        tableActionRef.current?.reloadAndRest?.();
    }, [props.parentGroupId]);

    const handleRemove = async (selectedrows: API.DeviceGroupGetResponseModel[]) => {
        const hide = message.loading(intl.formatMessage({ id: 'pages.table.processing' }));
        if (!selectedrows) return true;
        try {
            await Promise.all(selectedrows.map(async (value) => {
                if (value.id) {
                    await deleteDeviceGroup({ id: value.id });
                }
            }));
            hide();
            message.success(intl.formatMessage({ id: 'pages.table.successful' }));
            tableActionRef.current?.reload();
            return true;
        } catch (error) {
            hide();
            message.error(intl.formatMessage({ id: 'pages.table.failed' }));
            return false;
        }
    };

    const columns: ProColumns<API.DeviceGroupGetResponseModel>[] = [
        {
            title: '序号',
            dataIndex: 'index',
            valueType: 'indexBorder',
            hideInTable: true,
        },
        {
            title: <FormattedMessage id='pages.table.deviceGroup.id' />,
            dataIndex: 'id',
            valueType: 'text',
            search: false,
            hideInTable: true,
            defaultSortOrder: 'descend',
        },
        {
            title: <FormattedMessage id='pages.table.deviceGroup.name' />,
            dataIndex: 'name',
            valueType: 'text',
            sorter: false,
            search: { transform: () => 'keyword' },
            render: (dom, entity) => {
                return (
                    <a
                        onClick={() => {
                            setCurrentRow(entity);
                            setShowDetail(true);
                        }}
                    >
                        {dom}
                    </a>
                );
            },
        },
        {
            title: <FormattedMessage id='pages.table.deviceGroup.remark' />,
            dataIndex: 'remark',
            valueType: 'text',
            sorter: false,
            search: false,
            hideInTable: true,
        },
        {
            title: <FormattedMessage id='pages.table.product.creationTime' />,
            dataIndex: 'creationTime',
            valueType: 'dateTime',
            sorter: true,
            search: { transform: () => 'keyword' },
        },
        {
            title: <FormattedMessage id="pages.searchTable.titleOption" />,
            dataIndex: 'option',
            valueType: 'option',
            render: (_, record) => [
                <a
                    onClick={() => {
                        history.push({
                            pathname: '/device/device-group/view',
                            query: {
                                groupId: `${record.id}`,
                            },
                        });
                    }}
                    key={`view${record.id}`}
                >
                    <FormattedMessage id="pages.table.view" />
                </a>,
                <a
                    key={`edit${record.id}`}
                    onClick={async () => {
                        setUpdateModalVisible(true);
                        setCurrentRow(record);
                    }}
                >
                    <FormattedMessage id="pages.table.edit" />
                </a>,
                <Popconfirm
                    key={record.id}
                    title={<FormattedMessage id="pages.table.deleteConfirm" />}
                    onConfirm={async () => {
                        const hide = message.loading(intl.formatMessage({ id: 'pages.table.processing' }));
                        if (record.id) {
                            await deleteDeviceGroup({ id: record.id });
                            hide();
                            message.success(intl.formatMessage({ id: 'pages.table.successful' }));
                            tableActionRef.current?.reload();
                        }
                    }}
                >
                    <a href="#">
                        <FormattedMessage id="pages.table.delete" />
                    </a>
                </Popconfirm>,
            ],
        },
    ];

    const fetchDeviceGroupListApi = async (parentId?: number) => {
        const result = await getDeviceGroups({ parentId: parentId, pageNumber: 1, pageSize: 100 }, { errorHandler: () => { } });
        if (result) {
            const parentDataNodes: any[] = [];
            result.items?.forEach(e => {
                if (e.name && e.id) {
                    parentDataNodes.push({ id: e.id, title: e.name, key: e.id, pId: parentId || 0, value: e.id, isLeaf: !e.children?.length });
                }
            });
            return parentDataNodes;
        }
        return [];
    }

    useEffect(() => {
        if (createModalVisible) {
            fetchDeviceGroupListApi().then(e => { setDeviceGroupTreeData(e) });
        }
    }, [createModalVisible]);

    return (
        <>
            <ProTable<API.DeviceGroupGetResponseModel>
                columns={columns}
                rowKey='id'
                search={false}
                actionRef={tableActionRef}
                pagination={{
                    showSizeChanger: true,
                    showQuickJumper: true,
                }}
                expandable={{
                    defaultExpandAllRows: false,
                    rowExpandable: () => false,
                    expandedRowRender: () => null,
                }}
                request={async (params, sort) => {
                    const requestParams = Object.assign(params, {
                        sorter: JSON.stringify(sort),
                        pageNumber: params.current,
                        parentId: props.parentGroupId,
                    });
                    const { current, ...parameter } = requestParams
                    const result = await getDeviceGroups(parameter);
                    return {
                        data: result.items,
                        total: result.totalCount,
                    }
                }}
                dateFormatter="string"
                headerTitle={intl.formatMessage({
                    id: 'pages.searchTable.title'
                })}
                toolBarRender={() => [
                    <Button
                        type="primary"
                        key="primary"
                        onClick={() => {
                            setCreateModalVisible(true);
                        }}
                    >
                        <PlusOutlined />
                        <FormattedMessage id="pages.searchTable.new" />
                    </Button>,
                ]}
                options={{
                    search: true,
                }}
                rowSelection={{
                    onChange: (_, selectedrows) => {
                        setSelectedRows(selectedrows);
                    },
                }}
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
                {currentRow?.id && (
                    <ProDescriptions<API.DeviceGroupGetResponseModel>
                        column={1}
                        title={currentRow?.name}
                        request={async () => {
                            if (currentRow.id) {
                                return { data: await getDeviceGroup({ id: currentRow.id }) };
                            }
                            return { data: currentRow || {}, };
                        }}
                        params={{
                            id: currentRow?.id,
                        }}
                        columns={columns.concat({
                            title: <FormattedMessage id='pages.table.deviceGroup.remark' />,
                            dataIndex: 'remark',
                            valueType: 'text',
                        }) as ProDescriptionsItemProps<API.DeviceGroupGetResponseModel>[]}
                    />
                )}
            </Drawer>
            <ModalForm<API.DeviceGroupCreateRequestModel>
                autoComplete="off"
                visible={createModalVisible}
                onVisibleChange={setCreateModalVisible}
                title={intl.formatMessage({
                    id: 'pages.table.deviceGroup.createForm.title',
                })}
                formRef={createFormRef}
                onFinish={async (value: API.DeviceGroupCreateRequestModel) => {
                    const result = await postDeviceGroup(value as API.DeviceGroupCreateRequestModel, { skipErrorHandler: true });
                    if (result) {
                        setCreateModalVisible(false);
                        createFormRef.current?.resetFields();
                        tableActionRef.current?.reload();
                        message.success(intl.formatMessage({ id: 'pages.table.successful' }));
                        return true;
                    }
                    message.error(intl.formatMessage({ id: 'pages.table.failed' }));
                    return false;
                }}
                submitter={{
                    render: (_, doms: JSX.Element[]) => {
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
            >
                <Form.Item name='parentId' label={intl.formatMessage({ id: 'pages.table.deviceGroup.parentGroup' })} required >
                    <TreeSelect
                        treeDataSimpleMode
                        style={{ width: '100%' }}
                        value={undefined}
                        dropdownStyle={{ maxHeight: 400, overflow: 'auto' }}
                        treeData={deviceGroupTreeData}
                        placeholder={intl.formatMessage({ id: 'pages.table.device.selectPlaceholder' })}
                        allowClear
                        onChange={() => {

                        }}
                        loadData={async ({ id }) => {
                            const children = await fetchDeviceGroupListApi(id);
                            const state = [...deviceGroupTreeData].concat(children);
                            const treeData = state.filter((node, i, arr) => arr.findIndex(t => t.key === node.key) === i);
                            setDeviceGroupTreeData(treeData);
                        }}
                    />
                </Form.Item>

                <ProFormText
                    name="name"
                    label={intl.formatMessage({ id: 'pages.table.deviceGroup.name' })}
                    rules={[
                        {
                            type: 'string',
                            required: true,
                            min: 5,
                            max: 15,
                        },
                    ]}
                />
                <ProFormTextArea
                    name="remark"
                    label={intl.formatMessage({ id: 'pages.table.deviceGroup.remark' })}
                    rules={[
                        {
                            type: 'string',
                            required: false,
                            min: 2,
                            max: 100,
                        },
                    ]}
                />
            </ModalForm>
            <UpdateForm
                onSubmit={async (formData) => {
                    if (currentRow?.id) {
                        const result = await putDeviceGroup({ id: currentRow.id }, formData);
                        if (result) {
                            message.success(intl.formatMessage({ id: 'pages.table.successful' }));
                            setUpdateModalVisible(false);
                            setCurrentRow(undefined);
                            tableActionRef.current?.reload();
                            return;
                        }
                        message.error(intl.formatMessage({ id: 'pages.table.failed' }));
                    }
                }}
                onCancel={() => {
                    setUpdateModalVisible(false);
                    setCurrentRow(undefined);
                }}
                visible={updateModalVisible}
                initialValues={currentRow || {}}
            />
        </>
    );
};