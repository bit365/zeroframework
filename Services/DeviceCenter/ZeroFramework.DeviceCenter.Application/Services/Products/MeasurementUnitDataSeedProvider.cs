using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.Application.Services.Products
{
    public class MeasurementUnitDataSeedProvider : IDataSeedProvider
    {
        private readonly IRepository<MeasurementUnit, int> _measurementUnitRepository;

        public MeasurementUnitDataSeedProvider(IRepository<MeasurementUnit, int> measurementUnitRepository)
        {
            _measurementUnitRepository = measurementUnitRepository;
        }

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            if (await _measurementUnitRepository.GetCountAsync() <= 0)
            {
                List<MeasurementUnit> measurementUnits = new()
                {
                    new MeasurementUnit { UnitName = "无", Unit = string.Empty },
                    new MeasurementUnit { UnitName = "PH值", Unit = "pH" },
                    new MeasurementUnit { UnitName = "土壤EC值", Unit = "dS/m" },
                    new MeasurementUnit { UnitName = "太阳总辐射", Unit = "W/㎡" },
                    new MeasurementUnit { UnitName = "降雨量", Unit = "mm/hour" },
                    new MeasurementUnit { UnitName = "乏", Unit = "var" },
                    new MeasurementUnit { UnitName = "厘泊", Unit = "cP" },
                    new MeasurementUnit { UnitName = "饱和度", Unit = "aw" },
                    new MeasurementUnit { UnitName = "个", Unit = "pcs" },
                    new MeasurementUnit { UnitName = "厘斯", Unit = "cst" },
                    new MeasurementUnit { UnitName = "巴", Unit = "bar" },
                    new MeasurementUnit { UnitName = "纳克每升", Unit = "ppt" },
                    new MeasurementUnit { UnitName = "微克每升", Unit = "ppb" },
                    new MeasurementUnit { UnitName = "微西每厘米", Unit = "uS/cm" },
                    new MeasurementUnit { UnitName = "牛顿每库仑", Unit = "N/C" },
                    new MeasurementUnit { UnitName = "伏特每米", Unit = "V/m" },
                    new MeasurementUnit { UnitName = "滴速", Unit = "ml/min" },
                    new MeasurementUnit { UnitName = "血压", Unit = "mmHg" },
                    new MeasurementUnit { UnitName = "血糖", Unit = "mmol/L" },
                    new MeasurementUnit { UnitName = "毫米每秒", Unit = "mm/s" },
                    new MeasurementUnit { UnitName = "转每分钟", Unit = "turn/m" },
                    new MeasurementUnit { UnitName = "次", Unit = "count" },
                    new MeasurementUnit { UnitName = "档", Unit = "gear" },
                    new MeasurementUnit { UnitName = "步", Unit = "stepCount" },
                    new MeasurementUnit { UnitName = "标准立方米每小时", Unit = "Nm3/h" },
                    new MeasurementUnit { UnitName = "千伏", Unit = "kV" },
                    new MeasurementUnit { UnitName = "千伏安", Unit = "kVA" },
                    new MeasurementUnit { UnitName = "千乏", Unit = "kVar" },
                    new MeasurementUnit { UnitName = "微瓦每平方厘米", Unit = "uw/cm2" },
                    new MeasurementUnit { UnitName = "只", Unit = "只" },
                    new MeasurementUnit { UnitName = "相对湿度", Unit = "%RH" },
                    new MeasurementUnit { UnitName = "立方米每秒", Unit = "m³/s" },
                    new MeasurementUnit { UnitName = "公斤每秒", Unit = "kg/s" },
                    new MeasurementUnit { UnitName = "转每分钟", Unit = "r/min" },
                    new MeasurementUnit { UnitName = "吨每小时", Unit = "t/h" },
                    new MeasurementUnit { UnitName = "千卡每小时", Unit = "KCL/h" },
                    new MeasurementUnit { UnitName = "升每秒", Unit = "L/s" },
                    new MeasurementUnit { UnitName = "兆帕", Unit = "Mpa" },
                    new MeasurementUnit { UnitName = "立方米每小时", Unit = "m³/h" },
                    new MeasurementUnit { UnitName = "千乏时", Unit = "kvarh" },
                    new MeasurementUnit { UnitName = "微克每升", Unit = "μg/L" },
                    new MeasurementUnit { UnitName = "千卡路里", Unit = "kcal" },
                    new MeasurementUnit { UnitName = "吉字节", Unit = "GB" },
                    new MeasurementUnit { UnitName = "兆字节", Unit = "MB" },
                    new MeasurementUnit { UnitName = "千字节", Unit = "KB" },
                    new MeasurementUnit { UnitName = "字节", Unit = "B" },
                    new MeasurementUnit { UnitName = "微克每平方分米每天", Unit = "μg/(d㎡·d)" },
                    new MeasurementUnit { UnitName = "百万分率", Unit = "ppm" },
                    new MeasurementUnit { UnitName = "像素", Unit = "pixel" },
                    new MeasurementUnit { UnitName = "照度", Unit = "Lux" },
                    new MeasurementUnit { UnitName = "重力加速度", Unit = "grav" },
                    new MeasurementUnit { UnitName = "分贝", Unit = "dB" },
                    new MeasurementUnit { UnitName = "百分比", Unit = "%" },
                    new MeasurementUnit { UnitName = "流明", Unit = "lm" },
                    new MeasurementUnit { UnitName = "比特", Unit = "bit" },
                    new MeasurementUnit { UnitName = "克每毫升", Unit = "g/mL" },
                    new MeasurementUnit { UnitName = "克每升", Unit = "g/L" },
                    new MeasurementUnit { UnitName = "毫克每升", Unit = "mg/L" },
                    new MeasurementUnit { UnitName = "微克每立方米", Unit = "μg/m³" },
                    new MeasurementUnit { UnitName = "毫克每立方米", Unit = "mg/m³" },
                    new MeasurementUnit { UnitName = "克每立方米", Unit = "g/m³" },
                    new MeasurementUnit { UnitName = "千克每立方米", Unit = "kg/m³" },
                    new MeasurementUnit { UnitName = "纳法", Unit = "nF" },
                    new MeasurementUnit { UnitName = "皮法", Unit = "pF" },
                    new MeasurementUnit { UnitName = "微法", Unit = "μF" },
                    new MeasurementUnit { UnitName = "法拉", Unit = "F" },
                    new MeasurementUnit { UnitName = "欧姆", Unit = "Ω" },
                    new MeasurementUnit { UnitName = "微安", Unit = "μA" },
                    new MeasurementUnit { UnitName = "毫安", Unit = "mA" },
                    new MeasurementUnit { UnitName = "千安", Unit = "kA" },
                    new MeasurementUnit { UnitName = "安培", Unit = "A" },
                    new MeasurementUnit { UnitName = "毫伏", Unit = "mV" },
                    new MeasurementUnit { UnitName = "伏特", Unit = "V" },
                    new MeasurementUnit { UnitName = "毫秒", Unit = "ms" },
                    new MeasurementUnit { UnitName = "秒", Unit = "s" },
                    new MeasurementUnit { UnitName = "分钟", Unit = "min" },
                    new MeasurementUnit { UnitName = "小时", Unit = "h" },
                    new MeasurementUnit { UnitName = "日", Unit = "day" },
                    new MeasurementUnit { UnitName = "周", Unit = "week" },
                    new MeasurementUnit { UnitName = "月", Unit = "month" },
                    new MeasurementUnit { UnitName = "年", Unit = "year" },
                    new MeasurementUnit { UnitName = "节", Unit = "kn" },
                    new MeasurementUnit { UnitName = "千米每小时", Unit = "km/h" },
                    new MeasurementUnit { UnitName = "米每秒", Unit = "m/s" },
                    new MeasurementUnit { UnitName = "秒", Unit = "″" },
                    new MeasurementUnit { UnitName = "分", Unit = "′" },
                    new MeasurementUnit { UnitName = "度", Unit = "°" },
                    new MeasurementUnit { UnitName = "弧度", Unit = "rad" },
                    new MeasurementUnit { UnitName = "赫兹", Unit = "Hz" },
                    new MeasurementUnit { UnitName = "微瓦", Unit = "μW" },
                    new MeasurementUnit { UnitName = "毫瓦", Unit = "mW" },
                    new MeasurementUnit { UnitName = "千瓦特", Unit = "kW" },
                    new MeasurementUnit { UnitName = "瓦特", Unit = "W" },
                    new MeasurementUnit { UnitName = "卡路里", Unit = "cal" },
                    new MeasurementUnit { UnitName = "千瓦时", Unit = "kW·h" },
                    new MeasurementUnit { UnitName = "瓦时", Unit = "Wh" },
                    new MeasurementUnit { UnitName = "电子伏", Unit = "eV" },
                    new MeasurementUnit { UnitName = "千焦", Unit = "kJ" },
                    new MeasurementUnit { UnitName = "焦耳", Unit = "J" },
                    new MeasurementUnit { UnitName = "华氏度", Unit = "℉" },
                    new MeasurementUnit { UnitName = "开尔文", Unit = "K" },
                    new MeasurementUnit { UnitName = "吨", Unit = "t" },
                    new MeasurementUnit { UnitName = "摄氏度", Unit = "°C" },
                    new MeasurementUnit { UnitName = "毫帕", Unit = "mPa" },
                    new MeasurementUnit { UnitName = "百帕", Unit = "hPa" },
                    new MeasurementUnit { UnitName = "千帕", Unit = "kPa" },
                    new MeasurementUnit { UnitName = "帕斯卡", Unit = "Pa" },
                    new MeasurementUnit { UnitName = "毫克", Unit = "mg" },
                    new MeasurementUnit { UnitName = "克", Unit = "g" },
                    new MeasurementUnit { UnitName = "千克", Unit = "kg" },
                    new MeasurementUnit { UnitName = "牛", Unit = "N" },
                    new MeasurementUnit { UnitName = "毫升", Unit = "mL" },
                    new MeasurementUnit { UnitName = "升", Unit = "L" },
                    new MeasurementUnit { UnitName = "立方毫米", Unit = "mm³" },
                    new MeasurementUnit { UnitName = "立方厘米", Unit = "cm³" },
                    new MeasurementUnit { UnitName = "立方千米", Unit = "km³" },
                    new MeasurementUnit { UnitName = "立方米", Unit = "m³" },
                    new MeasurementUnit { UnitName = "公顷", Unit = "h㎡" },
                    new MeasurementUnit { UnitName = "平方厘米", Unit = "c㎡" },
                    new MeasurementUnit { UnitName = "平方毫米", Unit = "m㎡" },
                    new MeasurementUnit { UnitName = "平方千米", Unit = "k㎡" },
                    new MeasurementUnit { UnitName = "平方米", Unit = "㎡" },
                    new MeasurementUnit { UnitName = "纳米", Unit = "nm" },
                    new MeasurementUnit { UnitName = "微米", Unit = "μm" },
                    new MeasurementUnit { UnitName = "毫米", Unit = "mm" },
                    new MeasurementUnit { UnitName = "厘米", Unit = "cm" },
                    new MeasurementUnit { UnitName = "分米", Unit = "dm" },
                    new MeasurementUnit { UnitName = "千米", Unit = "km" },
                    new MeasurementUnit { UnitName = "米", Unit = "m" },
                };

                foreach (var item in measurementUnits)
                {
                    await _measurementUnitRepository.InsertAsync(item);
                }

                await _measurementUnitRepository.UnitOfWork.SaveChangesAsync();
            }
        }
    }
}
