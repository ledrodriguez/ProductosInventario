namespace Domain.Common.Pages;

public static class OrderByHelper
{
    public static IDictionary<string, bool> Deconstruct<T>(string orderBy)
    {
        var propertiesInfo = typeof(T).GetProperties().Select(x => x.Name.ToLowerInvariant()).ToHashSet();

        orderBy ??= $"{propertiesInfo.FirstOrDefault()} ASC";

        var dictionary = new Dictionary<string, bool>();

        foreach (var order in orderBy.Split(';'))
        {
            var data = order.Split(' ');
            var propertyName = data.FirstOrDefault()?.Trim().ToLowerInvariant();
            if (!string.IsNullOrWhiteSpace(propertyName) && propertiesInfo.Contains(propertyName))
                dictionary.Add(propertyName, data.LastOrDefault()?.Trim().ToLowerInvariant() switch
                {
                    "asc" => true,
                    "desc" => false,
                    _ => true
                });
        }

        return dictionary;
    }
}