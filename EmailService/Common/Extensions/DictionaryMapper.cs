using System.Reflection;

namespace EmailService.Common.Extensions
{
    public static class DictionaryMapper
    {
        // Generic version
        public static T MapToModel<T>(IDictionary<string, string> data) where T : new()
        {
            var model = new T();
            MapProperties(model, data);
            return model;
        }

        // Dynamic version for Type object
        public static object MapToModel(IDictionary<string, string> data, Type type)
        {
            var model = Activator.CreateInstance(type)!;
            MapProperties(model, data);
            return model;
        }

        private static void MapProperties(object model, IDictionary<string, string> data)
        {
            var props = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                if (!prop.CanWrite) continue;
                if (data.TryGetValue(prop.Name, out var value))
                {
                    try
                    {
                        object? converted = Convert.ChangeType(value, prop.PropertyType);
                        prop.SetValue(model, converted);
                    }
                    catch
                    {
                        // optionally log conversion errors
                    }
                }
            }
        }
    }
}
