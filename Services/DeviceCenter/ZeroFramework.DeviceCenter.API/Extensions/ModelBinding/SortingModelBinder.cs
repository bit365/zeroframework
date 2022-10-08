using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.API.Extensions.ModelBinding
{
    public class SortingModelBinder : IModelBinder
    {
        /// <summary>
        /// Custom Model Binding in ASP.NET Core
        /// </summary>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            bindingContext = bindingContext ?? throw new ArgumentNullException(nameof(bindingContext));

            string modelName = bindingContext.ModelName;

            // Try to fetch the value of the argument by name
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            string? value = valueProviderResult.FirstValue;

            // Check if the argument value is null or empty
            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            var sorter = JsonSerializer.Deserialize<IDictionary<string, string>>(value);

            var sortingDirectionMap = new Dictionary<string, SortingOrder>
            {
                { "ascend", SortingOrder.Ascending },
                { "descend", SortingOrder.Descending }
            };

            if (sorter is not null)
            {
                var effectSorter = sorter.Where(item => sortingDirectionMap.ContainsKey(item.Value));
                var sorting = effectSorter.Select(item => new SortingDescriptor
                {
                    PropertyName = item.Key,
                    SortDirection = sortingDirectionMap[item.Value]
                });

                bindingContext.Result = ModelBindingResult.Success(sorting);
            }

            return Task.CompletedTask;
        }
    }
}