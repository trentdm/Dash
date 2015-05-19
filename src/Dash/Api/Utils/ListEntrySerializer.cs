using System;
using System.Reflection;
using Google.GData.Spreadsheets;

namespace Dash.Api.Utils
{
    public static class ListEntrySerializer
    {
        public static T Deserialize<T>(this ListEntry row) where T : new()
        {
            var result = new T();

            foreach (var prop in typeof(T).GetProperties())
            {
                var index = row.Elements.IndexOf(new ListEntry.Custom { LocalName = prop.Name.ToLower() });

                if (index > -1)
                {
                    var val = GetValue(prop, row.Elements[index].Value);
                    prop.SetValue(result, val);
                }
            }

            return result;
        }

        private static object GetValue(PropertyInfo prop, string val)
        {
            if (typeof(string).IsAssignableFrom(prop.PropertyType))
                return val;
            else if (typeof(DateTime).IsAssignableFrom(prop.PropertyType))
                return DateTime.Parse(val);
            else if (typeof(int).IsAssignableFrom(prop.PropertyType))
                return int.Parse(val);
            else if (typeof(double).IsAssignableFrom(prop.PropertyType))
                return double.Parse(val);
            else
                return val;
        }
    }
}
