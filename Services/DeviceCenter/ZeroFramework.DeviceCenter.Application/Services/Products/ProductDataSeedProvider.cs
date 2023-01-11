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
                static ExpandoObject createExpandoObject(float min, float max, string? unit, string? externalId = null)
                {
                    dynamic result = new ExpandoObject();
                    result.minValue = min;
                    result.maxValue = max;
                    result.unit = unit;
                    if (externalId is not null)
                    {
                        result.externalId = externalId;
                    }

                    return result;
                }

                var product1 = new Product
                {
                    Name = $"多参数水质分析仪",
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
                                Identifier="Chlorine",
                                Name="余氯",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,10,"mg/L","7"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="ChlorineDioxide",
                                Name="二氧化氯",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,10,"mg/L","8"),
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
                                    Specs=createExpandoObject(0,2,"NTU","9"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Temperature",
                                Name="温度",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,10,"°C","10"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="PH",
                                Name="pH",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,14,null,"17"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="AmmoniaNitrogen",
                                Name="氨氮",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,100,"mg/L","18"),
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
                                    Specs=createExpandoObject(10,1500,"μS/cm","20"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="DissolvedOxygen",
                                Name="溶解氧",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,20,"mg/L","21"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="FlowRate",
                                Name="瞬时流量",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,999,"m³/h","22"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="FlowTotal",
                                Name="累计流量",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,999999,"m³","23"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="压力",
                                Name="Pressure",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,2.5f,"MPa","26"),
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
                                    Specs=createExpandoObject(0,50,"m","27"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="TotalChlorine",
                                Name="总氯",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,2,"mg/L","29"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="FreeChlorine",
                                Name="游离性余氯",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,1,"mg/L","30"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="CombinedChlorine",
                                Name="化合性余氯",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,1,"mg/L","31"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="Chroma",
                                Name="色度",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,50,"PCU","32"),
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
                                    Specs=createExpandoObject(0,15,"mg/L","42"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="ORP",
                                Name="ORP",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,5000,"mv","49"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="InWaterStatus",
                                Name="进水状态",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,1,null,"54"),
                                }
                            },
                        }
                    }
                };

                await _productRepository.InsertAsync(product1, true);


                var product2 = new Product
                {
                    Name = $"李河污水监测站",
                    NetType = ProductNetType.Cellular,
                    NodeType = ProductNodeType.Gateway,
                    ProtocolType = ProductProtocolType.Other,
                    DataFormat = ProductDataFormat.Custom,
                    CreationTime = DateTimeOffset.Now,
                    Features = new ProductFeatures
                    {
                        Properties = new List<PropertyFeature>
                        {
                            new PropertyFeature
                            {
                                Identifier="PH1",
                                Name="缺氧池PH",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,14,null,"wtg011"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="DO1",
                                Name="缺氧池DO",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,10,"mg/L","wtg091"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="MLSS2",
                                Name="沉淀池MLSS",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,10000,"mg/L","wtg097"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="PH2",
                                Name="沉淀池PH",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,14,null,"wtg012"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="DO2",
                                Name="沉淀池DO",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,10,"mg/L","wtg092"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="PH3",
                                Name="好氧池PH",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,14,null,"wtg013"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="DO3",
                                Name="好氧池DO",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,10,"mg/L","wtg093"),
                                }
                            },
                            new PropertyFeature
                            {
                                Identifier="MLSS3",
                                Name="好氧池MLSS",
                                AccessMode= PropertyAccessMode.Read,
                                DataType= new DataType
                                {
                                    Type= DataTypeDefinitions.Float,
                                    Specs=createExpandoObject(0,10000,"mg/L","wtg094"),
                                }
                            },
                        }
                    }
                };

                await _productRepository.InsertAsync(product2, true);
            }
        }
    }
}
