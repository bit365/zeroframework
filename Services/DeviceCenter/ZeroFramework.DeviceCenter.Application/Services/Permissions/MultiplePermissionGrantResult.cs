namespace ZeroFramework.DeviceCenter.Application.Services.Permissions
{
    public class MultiplePermissionGrantResult
    {
        public bool AllGranted => Result.Values.All(x => x == PermissionGrantResult.Granted);

        public bool AllProhibited => Result.Values.All(x => x == PermissionGrantResult.Prohibited);

        public Dictionary<string, PermissionGrantResult> Result { get; }

        public MultiplePermissionGrantResult() => Result = new Dictionary<string, PermissionGrantResult>();

        public MultiplePermissionGrantResult(string[] names, PermissionGrantResult grantResult = PermissionGrantResult.Undefined) : this()
        {
            if (names is null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            Array.ForEach(names, name => Result.Add(name, grantResult));
        }
    }
}
