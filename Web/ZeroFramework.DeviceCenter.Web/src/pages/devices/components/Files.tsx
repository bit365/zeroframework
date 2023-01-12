import { Button, Drawer, message, Popconfirm, Upload, } from 'antd';
import type { ActionType, ProColumns } from '@ant-design/pro-table';
import ProTable from '@ant-design/pro-table';
import { UploadOutlined } from '@ant-design/icons';
import { FooterToolbar } from '@ant-design/pro-layout';
import { useIntl, FormattedMessage } from 'umi';
import { useRef, useState } from 'react';
import ProDescriptions from '@ant-design/pro-descriptions';
import { useEffect } from 'react';

export type TableListItem = {
    key: string,
    fileName: string;
    fileSize: number;
    uploadTime: number;
};

export default () => {
    const [currentRow, setCurrentRow] = useState<TableListItem>();
    const [selectedRows, setSelectedRows] = useState<TableListItem[]>([]);
    const [showDetail, setShowDetail] = useState<boolean>(false);
    const [tableListDataSource, setTableListDataSource] = useState<TableListItem[]>([]);
    const intl = useIntl();
    const tableActionRef = useRef<ActionType>();

    // eslint-disable-next-line @typescript-eslint/no-shadow
    const handleRemove = async (selectedRows: TableListItem[]) => {
        const hide = message.loading(intl.formatMessage({ id: 'pages.table.processing' }));
        if (!selectedRows) return true;
        try {
            let list = [...tableListDataSource];
            await Promise.all(selectedRows.map(async (value) => {
                if (value.key) {
                    list = list.filter(e => e.key != value.key);
                }
            }));
            setTableListDataSource(list);
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

    useEffect(() => {
        tableActionRef.current?.reload()
    }, [tableListDataSource]);


    useEffect(() => {
        const initialValues: TableListItem[] = [];
        for (let i = 0; i < 5; i += 1) {
            initialValues.push({
                key: i.toString(),
                fileName: `servicepack${i}.apk`,
                fileSize: Math.floor(Math.random() * 20000),
                uploadTime: Date.now() - Math.floor(Math.random() * 100000),
            });
        }
        setTableListDataSource(initialValues);
    }, []);

    const columns: ProColumns<TableListItem>[] = [
        {
            title: <FormattedMessage id='pages.devices.view.files.key' />,
            dataIndex: 'key',
            valueType: 'text',
            sorter: false,
            search: false,
            hideInTable: true,
        },
        {
            title: <FormattedMessage id='pages.devices.view.files.fileName' />,
            dataIndex: 'fileName',
            valueType: 'text',
            sorter: true,
            search: false,
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
            title: <FormattedMessage id='pages.devices.view.files.fileSize' />,
            dataIndex: 'fileSize',
            sorter: true,
            valueType: 'digit',
            render: (dom, entity) => `${entity.fileSize.toFixed(2)}KB`,
        },
        {
            title: <FormattedMessage id='pages.devices.view.files.uploadTime' />,
            dataIndex: 'uploadTime',
            valueType: 'dateTime',
            sorter: true,
            search: false,
        },
        {
            title: <FormattedMessage id="pages.searchTable.titleOption" />,
            dataIndex: 'option',
            valueType: 'option',
            render: (_, record) => [
                <a
                    key={record.key}
                    onClick={async () => {
                        setCurrentRow(record);
                    }}
                >
                    <FormattedMessage id="pages.devices.view.files.download" />
                </a>,
                <Popconfirm
                    key={record.fileName}
                    title={<FormattedMessage id="pages.table.deleteConfirm" />}
                    onConfirm={async () => {
                        if (record.fileName) {
                            await handleRemove([record]);
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

    return (
        <>
            <ProTable<TableListItem>
                style={{ marginTop: 10, }}
                columns={columns}
                rowKey={e => e.key}
                actionRef={tableActionRef}
                pagination={{
                    showSizeChanger: true,
                    showQuickJumper: true,
                }}
                // eslint-disable-next-line @typescript-eslint/no-unused-vars
                request={(params, sort, filter) => {
                    return Promise.resolve({
                        data: tableListDataSource,
                        success: true,
                    });
                }}
                dateFormatter="string"
                headerTitle={
                    <Upload name="logo" action="/upload.do" itemRender={() => null} onChange={info => {
                        if (info.file.status == 'done' && info.file.name && info.file.size) {
                            const list = [...tableListDataSource, {
                                key: info.file.uid,
                                fileName: info.file.name,
                                fileSize: info.file.size / 1024,
                                uploadTime: Date.now(),
                            }];
                            setTableListDataSource(list);
                            tableActionRef.current?.reload();
                        }
                    }}>
                        <Button icon={<UploadOutlined />}>
                            <FormattedMessage id='pages.devices.view.files.upload' />
                        </Button>
                    </Upload>}
                options={{
                    search: true,
                }}
                rowSelection={{
                    // eslint-disable-next-line @typescript-eslint/no-shadow
                    onChange: (_, selectedRows) => {
                        setSelectedRows(selectedRows);
                    },
                }}
                search={false}
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
                {currentRow?.key && (
                    <ProDescriptions<API.DeviceGetResponseModel>
                        column={1}
                        title={currentRow?.fileName}
                        request={async () => {
                            return { data: currentRow || {}, };
                        }}
                        params={{
                            id: currentRow?.key,
                        }}
                        columns={columns as TableListItem[]}
                    />
                )}
            </Drawer>
        </>
    );
};