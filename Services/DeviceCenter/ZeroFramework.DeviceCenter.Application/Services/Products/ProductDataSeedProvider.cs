using System.Dynamic;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.Application.Services.Products
{
    public class ProductDataSeedProvider : IDataSeedProvider
    {
        private readonly IRepository<Product, int> _productRepository;

        public ProductDataSeedProvider(IRepository<Product, int> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            if (await _productRepository.GetCountAsync() <= 0)
            {
                for (int i = 1; i < 18; i++)
                {
                    Guid? tenantId = null;

                    if (i >= 3 && i < 6)
                    {
                        tenantId = Guid.Parse($"5f6f2110-58b6-4cf9-b416-85820ba12c01");
                    }

                    if (i >= 8 && i < 8)
                    {
                        tenantId = Guid.Parse($"5f6f2110-58b6-4cf9-b416-85820ba12c02");
                    }

                    var product = new Product { Name = $"智能监测产品{i.ToString().PadLeft(2, '0')}", TenantId = tenantId, CreationTime = DateTimeOffset.Now };
                    await _productRepository.InsertAsync(product, true);
                }

                static ExpandoObject createExpandoObject(float min, float max, string? unit)
                {
                    dynamic result = new ExpandoObject();
                    result.minValue = min;
                    result.maxValue = max;
                    result.unit = unit;
                    return result;
                }

                var product1 = new Product
                {
                    Name = $"环境空气质量监测产品",
                    NetType = ProductNetType.Cellular,
                    NodeType = ProductNodeType.Gateway,
                    ProtocolType = ProductProtocolType.Modbus,
                    DataFormat = ProductDataFormat.Custom,
                    CreationTime = DateTimeOffset.Now,
                    Features = new ProductFeatures
                    {
                        Properties = new List<PropertyFeature>
                        {
                            new PropertyFeature
                            {
                                Identifier="SO2",
                                Name="二氧化硫",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,100,"μg/m³")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="CO",
                                Name="一氧化碳",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,20,"mg/m³")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="NO2",
                                Name="二氧化氮",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,100,"μg/m³")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="O3",
                                Name="臭氧",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,200,"μg/m³")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="PM25",
                                Name="PM2.5",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,100,"μg/m³")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="PM10",
                                Name="PM10",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,100,"μg/m³")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="TSP",
                                Name="TSP",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,500,"μg/m³")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="NOX",
                                Name="氮氧化物",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,500,"μg/m³")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="PB",
                                Name="铅",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,1,"μg/m³")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="BAP",
                                Name="苯",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,1,"μg/m³")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="CD",
                                Name="镉",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,1,"μg/m³")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="HG",
                                Name="汞",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,1,"μg/m³")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="CR6",
                                Name="六价铬",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,1,"μg/m³")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="F",
                                Name="氟化物",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,10,"μg/m³")
                                }
                            }
                        }
                    }
                };

                await _productRepository.InsertAsync(product1, true);

                var product2 = new Product
                {
                    Name = $"水质监测产品",
                    NetType = ProductNetType.Cellular,
                    NodeType = ProductNodeType.Gateway,
                    ProtocolType = ProductProtocolType.Mqtt,
                    DataFormat = ProductDataFormat.Custom,
                    CreationTime = DateTimeOffset.Now,
                    Features = new ProductFeatures
                    {
                        Properties = new List<PropertyFeature>
                        {
                            new PropertyFeature
                            {
                                Identifier="pH",
                                Name="酸碱度",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,14,null)
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Temperature",
                                Name="水温",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,100,"℃")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Conductivity",
                                Name="电导率",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,500,"ms/cm")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Dissolved",
                                Name="溶解氧",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,20,"mg/L")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Chlorine",
                                Name="余氯",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,2,"mg/L")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Turbidity",
                                Name="浊度",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,100,"NTU")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Suspended",
                                Name="悬浮物",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,10000,"mg/L")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Sludge",
                                Name="污泥浓度",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,25000,"mg/L")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Ozone",
                                Name="臭氧",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,20,"mg/L")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="COD",
                                Name="COD",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,15000,"mg/L")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Nitrogen",
                                Name="氨氮",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,100,"mg/L")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Phosphorus",
                                Name="总磷",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,20,"mg/L")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Chromaticity",
                                Name="色度",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,50,"PCU")
                                }
                            },
                        }
                    }
                };

                await _productRepository.InsertAsync(product2, true);


                var product3 = new Product
                {
                    Name = $"流量液位压力监测产品",
                    NetType = ProductNetType.Cellular,
                    NodeType = ProductNodeType.Gateway,
                    ProtocolType = ProductProtocolType.Mqtt,
                    DataFormat = ProductDataFormat.Custom,
                    CreationTime = DateTimeOffset.Now,
                    Features = new ProductFeatures
                    {
                        Properties = new List<PropertyFeature>
                        {
                            new PropertyFeature
                            {
                                Identifier="Flow",
                                Name="流量",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,1000,"m³/h")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Pressure",
                                Name="压力",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,3,"MPa")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="LiquidLevel",
                                Name="液位",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,40,"m")
                                }
                            }
                        }
                    }
                };

                await _productRepository.InsertAsync(product3, true);

                var product4 = new Product
                {
                    Name = $"气象土壤监测产品",
                    NetType = ProductNetType.Cellular,
                    NodeType = ProductNodeType.Gateway,
                    ProtocolType = ProductProtocolType.Mqtt,
                    DataFormat = ProductDataFormat.Custom,
                    CreationTime = DateTimeOffset.Now,
                    Features = new ProductFeatures
                    {
                        Properties = new List<PropertyFeature>
                        {
                            new PropertyFeature
                            {
                                Identifier="Temperature",
                                Name="环境温度",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,100,"℃")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Humidity",
                                Name="相对湿度",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,100,"%")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="PM25",
                                Name="PM2.5",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,1000,"μg/m³")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="PM10",
                                Name="PM10",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,1000,"μg/m³")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Pressure",
                                Name="大气压力",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,110,"Kpa")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="WindSpeed",
                                Name="风速",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,60,"m/s")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="WindDirection",
                                Name="风向",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,360,"°")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Noise",
                                Name="噪声",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(30,130,"dB")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Rainfall",
                                Name="雨量",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,3,"mm/min")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="SoilTemperature",
                                Name="土壤温度",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,100,"℃")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="SoilHumidity",
                                Name="土壤湿度",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,100,"％")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Evaporation",
                                Name="蒸发量",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,200,"mm")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Salt",
                                Name="土壤盐分",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,20,"ms")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="CO",
                                Name="一氧化碳",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,10,"PPM")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="CO2",
                                Name="二氧化碳",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,5,"PPM")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Radiation",
                                Name="总辐射",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,2000,"W/m2")
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Beam",
                                Name="光照",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,20000,"Lux")
                                }
                            }
                        }
                    }
                };

                await _productRepository.InsertAsync(product4, true);
            }
        }
    }
}
