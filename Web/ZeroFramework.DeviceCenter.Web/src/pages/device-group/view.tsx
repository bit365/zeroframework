import { Card, Tabs } from 'antd';
import { PageContainer } from '@ant-design/pro-layout';
import { useIntl, history } from 'umi';
import DeviceGroupInfo from './components/DeviceGroupInfo';
import DeviceGroupList from './components/DeviceGroupList';
import DeviceList from './components/DeviceList';
import { useEffect, useState } from 'react';

export default (props: any) => {

    const [tabsActiveKey, setTabsActiveKey] = useState<string>("1");

    const intl = useIntl();
    const { TabPane } = Tabs;

    useEffect(() => {
        setTabsActiveKey("1");
    }, [props.location.query.groupId]);

    return (
        <PageContainer onBack={() => { history.goBack(); }}>
            <Card>
                <Tabs activeKey={tabsActiveKey} onChange={key => setTabsActiveKey(key)}>
                    <TabPane tab={intl.formatMessage({ id: 'pages.deviceGroup.view.info' })} key="1">
                        <DeviceGroupInfo groupId={props.location.query.groupId} />
                    </TabPane>
                    <TabPane tab={intl.formatMessage({ id: 'pages.deviceGroup.view.deviceList' })} key="2">
                        <Card bordered>
                            <DeviceList groupId={props.location.query.groupId} />
                        </Card>
                    </TabPane>
                    <TabPane tab={intl.formatMessage({ id: 'pages.deviceGroup.view.childrenGroup' })} key="3">
                        <Card bordered>
                            <DeviceGroupList parentGroupId={props.location.query.groupId} />
                        </Card>
                    </TabPane>
                </Tabs>
            </Card>
        </PageContainer>
    );
};