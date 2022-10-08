using ZeroFramework.DeviceCenter.Domain.Aggregates.MonitoringAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.Application.Services.Monitoring
{
    /// <summary>
    /// [HJ525—2009]《水污染物名称代码》http://www.mee.gov.cn/ywgz/fgbz/bz/bzwb/other/xxbz/201001/W020111114571767696171.pdf
    /// [HJ524-2009]《大气污染物名称代码》http://www.mee.gov.cn/ywgz/fgbz/bz/bzwb/other/xxbz/201001/W020111114570763622395.pdf
    /// [HJ212-2017]《污染物在线监控（监测）系统数据传输标准》http://www.mee.gov.cn/ywgz/fgbz/bz/bzwb/other/qt/201706/W020170608577218811635.pdf
    /// </summary>
    public class MonitoringFactorDataSeedProvider : IDataSeedProvider
    {
        private readonly IRepository<MonitoringFactor> _monitoringFactorRepository;

        public MonitoringFactorDataSeedProvider(IRepository<MonitoringFactor> monitoringFactorRepository)
        {
            _monitoringFactorRepository = monitoringFactorRepository;
        }

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            if (await _monitoringFactorRepository.GetCountAsync() <= 0)
            {
                var monitoringFactors = new List<MonitoringFactor>
                {
                    new MonitoringFactor { FactorCode = "w01001", ChineseName = "pH值", EnglishName = "pH", Unit = "NTU", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "w01002", ChineseName = "色度", EnglishName = "Color", Unit = "PCU", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "w01009", ChineseName = "溶解氧", EnglishName = "Dissolved Oxygen", Unit = "mg/L", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "w01010", ChineseName = "水温", EnglishName = "Temperature", Unit = "°C", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "w01014", ChineseName = "电导率", EnglishName = "Conductivity", Unit = "us/cm", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "w01016", ChineseName = "叶绿素", EnglishName = "Chlorophyll", Unit = "mg/L", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "w21001", ChineseName = "总氮", EnglishName = "Nitrogen", Unit = "mg/L", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "w21003", ChineseName = "氨氮", EnglishName = "Ammonnia-Nitrogen", Unit = "mg/L", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "w01018", ChineseName = "化学需氧量", EnglishName = "COD", Unit = "mg/L", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "w01020", ChineseName = "总有机碳", EnglishName = "TOC", Unit = "mg/L", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "w01006", ChineseName = "溶解性总固体", EnglishName = "Total Dissolved Solids", Unit = "ppm", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "w21011", ChineseName = "总磷", EnglishName = "Phosphorus", Unit = "mg/L", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "w21024", ChineseName = "余氯", EnglishName = "Residual Chlorine", Unit = "mg/L", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "w21025", ChineseName = "游离氯", EnglishName = "Free Chlorine", Unit = "mg/L", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "w21026", ChineseName = "二氧化硫", EnglishName = "Sulfur Dioxide", Unit = "mg/L", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "w01004", ChineseName = "透明度", EnglishName = "Transparency Clarity", Unit = "FTU", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "w01012", ChineseName = "悬浮物", EnglishName = "Suspended Solids", Unit = "mg/L", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "w34001", ChineseName = "二氧化氯", EnglishName = "Chlorine Dioxide", Unit = "mg/L", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "w34011", ChineseName = "臭氧", EnglishName = "Ozone", Unit = "mg/L", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "a01001", ChineseName = "温度", EnglishName = "Temperature", Unit = "°C", Decimals = 1 },
                    new MonitoringFactor { FactorCode = "a01002", ChineseName = "湿度", EnglishName = "Humidity", Unit = "%", Decimals = 1 },
                    new MonitoringFactor { FactorCode = "a01006", ChineseName = "气压", EnglishName = "Pressure", Unit = "MPa", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "a31001", ChineseName = "甲醛", EnglishName = "Formaldehyde", Unit = "mg/m³", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "a34004", ChineseName = "细微颗粒物", EnglishName = "PM2.5", Unit = "ug/m³", Decimals = 2 },
                    new MonitoringFactor { FactorCode = "a21005", ChineseName = "一氧化碳", EnglishName = "Carbon Monoxide", Unit = "mg/m³", Decimals = 2 },
                };

                monitoringFactors.ForEach(async item => await _monitoringFactorRepository.InsertAsync(item));

                await _monitoringFactorRepository.UnitOfWork.SaveChangesAsync();
            }
        }
    }
}
