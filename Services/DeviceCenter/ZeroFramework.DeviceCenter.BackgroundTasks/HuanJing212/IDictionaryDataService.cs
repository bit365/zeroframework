using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroFramework.DeviceCenter.BackgroundTasks.HuanJing212
{
    public interface IDictionaryDataService
    {
        Task HandlingAaync(Dictionary<string,string> keyValuePairs);
    }
}
