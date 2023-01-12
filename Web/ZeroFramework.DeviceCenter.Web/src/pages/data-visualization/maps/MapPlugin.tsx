import { PlusOutlined, MinusOutlined, AimOutlined, BulbOutlined } from "@ant-design/icons";
import { Button, Space } from "antd";
import { useEffect } from "react";

export default (props: any) => {

    const map = props.__map__;

    useEffect(() => {
        map.plugin(['AMap.Geolocation'], () => {

            const geolocation = new window.AMap.Geolocation({
                // 是否使用高精度定位，默认：true
                enableHighAccuracy: true,
                // 设置定位超时时间，默认：无穷大
                timeout: 10000,
                // 定位按钮的停靠位置的偏移量，默认：Pixel(10, 20)
                buttonOffset: new window.AMap.Pixel(10, 20),
                //  定位成功后调整地图视野范围使定位位置及精度范围视野内可见，默认：false
                zoomToAccuracy: true,
                //  定位按钮的排放位置,  RB表示右下
                buttonPosition: 'RB'
            })

            geolocation.getCurrentPosition(function (status: any, result: any) {
                if (status == 'complete') {
                    onComplete(result)
                } else {
                    onError(result)
                }
            });

            // eslint-disable-next-line @typescript-eslint/no-unused-vars
            function onComplete(data: any) {

            }

            // eslint-disable-next-line @typescript-eslint/no-unused-vars
            function onError(data: any) {

            }
        });
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return <Space style={{
        position: 'absolute',
        top: '10px',
        right: '10px',
        background: '#fff',
        padding: '10px'
    }}>
        <Button size='small' icon={<BulbOutlined />} onClick={() => {
            if (map.getMapStyle() == 'amap://styles/blue') {
                map.setMapStyle('amap://styles/macaron');
            } else {
                map.setMapStyle('amap://styles/blue');
            }
        }} />
        <Button size='small' icon={<AimOutlined />} onClick={() => { map.setZoomAndCenter(5, [107.308563, 36.93777]); }} />
        <Button size='small' icon={<PlusOutlined />} onClick={() => { map.zoomIn(); }} />
        <Button size='small' icon={<MinusOutlined />} onClick={() => { map.zoomOut(); }} />
    </Space>;
}