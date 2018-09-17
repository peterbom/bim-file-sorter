using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BimFileSorter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("Usage: formatjson.exe <jsonfile>");
                return;
            }

            var filename = args[0];
            var path = Path.Combine(Directory.GetCurrentDirectory(), filename);
            if (!File.Exists(path))
            {
                Console.Error.WriteLine($"File {path} does not exist");
                return;
            }

            var content = File.ReadAllText(path);

            if (!(JsonConvert.DeserializeObject(content) is JObject jObject))
            {
                Console.Error.WriteLine($"Not a valid JSON object: {path}");
                return;
            }

            Sort(jObject);

            File.WriteAllText(path, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        }

        private static void Sort(JObject rootJObject)
        {
            var model = (JObject)rootJObject["model"];

            SortArray(model, "tables", "name");
            foreach (JObject table in rootJObject["model"]["tables"])
            {
                SortArray(table, "columns", "name");
                SortArray(table, "measures", "name");
            }

            SortArray(model, "relationships", "name");
        }

        private static void SortArray(JObject container, string arrayName, string sortPropertyName)
        {
            var array = container[arrayName] as JArray;
            if (array == null)
            {
                // Not an error if the property is missing.
                return;
            }

            var properties = container.Properties().ToList();
            foreach (var property in properties)
            {
                // It's apparently not possible to re-order an array in-place,
                // so we have to remove it and re-add the property with the sorted
                // array. We remove and re-add *every* property so that they remain
                // in the same order.
                property.Remove();
                if (property.Name == arrayName)
                {
                    property.Value = new JArray(array.OrderBy(o => (string)o[sortPropertyName]));
                }

                container.Add(property);
            }
        }
    }
}
