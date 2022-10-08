import { Card, Tabs } from 'antd';
import { PageContainer } from '@ant-design/pro-layout';
import { useIntl, history } from 'umi';
import { useEffect } from 'react';
import DeviceInfo from './components/DeviceInfo';
import Files from './components/Files';
import Shadow from './components/Shadow';
import Properties from './components/Properties';
import Services from './components/Services';
import Events from './components/Events';
import Topics from './components/Topics';

export default (props: any) => {
    const intl = useIntl();
    const { TabPane } = Tabs;

    function callback(key: any) {
    }

    const fetchStatisticApi = async () => {
    }

    useEffect(() => {
        fetchStatisticApi();
    }, []);

    return (
        <PageContainer
            onBack={() => { history.goBack(); }}
            title={intl.formatMessage({ id: 'menu.device.manager.device.list' })}
        >
            <Card>
                <Tabs defaultActiveKey="1" onChange={callback}>
                    <TabPane tab={intl.formatMessage({ id: 'pages.devices.view.info' })} key="1">
                        <DeviceInfo devideId={props.location.state.id} />
                    </TabPane>
                    <TabPane tab={intl.formatMessage({ id: 'pages.devices.view.properties' })} key="2">
                        <Properties device={props.location.state as API.DeviceGetResponseModel} />
                    </TabPane>
                    <TabPane tab={intl.formatMessage({ id: 'pages.devices.view.services' })} key="3">
                        <Services device={props.location.state as API.DeviceGetResponseModel} />
                    </TabPane>
                    <TabPane tab={intl.formatMessage({ id: 'pages.devices.view.events' })} key="4">
                        <Events device={props.location.state as API.DeviceGetResponseModel} />
                    </TabPane>
                    <TabPane tab={intl.formatMessage({ id: 'pages.devices.view.topics' })} key="5">
                        <Topics device={props.location.state as API.DeviceGetResponseModel} />
                    </TabPane>
                    <TabPane tab={intl.formatMessage({ id: 'pages.devices.view.shadow' })} key="6">
                        <Shadow device={props.location.state as API.DeviceGetResponseModel} />
                    </TabPane>
                    <TabPane tab={intl.formatMessage({ id: 'pages.devices.view.files' })} key="7">
                        <Files />
                    </TabPane>
                </Tabs>
            </Card>
        </PageContainer>
    );
};