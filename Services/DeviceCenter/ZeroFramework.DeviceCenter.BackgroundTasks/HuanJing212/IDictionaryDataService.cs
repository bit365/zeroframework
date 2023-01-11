namespace ZeroFramework.DeviceCenter.BackgroundTasks.HuanJing212
{
    public interface IDictionaryDataService
    {
        Task HandlingAaync(Dictionary<string, string> keyValuePairs);
    }
}
