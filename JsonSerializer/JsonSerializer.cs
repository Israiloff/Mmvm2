using System;
using Mmvm.Container.Attributes;
using Newtonsoft.Json;

namespace Mmvm.Serializer.Json
{
    [Service(Name = nameof(JsonSerializer))]
    public class JsonSerializer : ISerializer
    {
        #region IJsonConverter impl

        public string Serialize(object target, bool isPretty)
        {
            return JsonConvert.SerializeObject(target, isPretty ? Formatting.Indented : Formatting.None);
        }

        public TResult Deserialize<TResult>(string data)
        {
            return JsonConvert.DeserializeObject<TResult>(data);
        }

        public object Deserialize(string data, Type type)
        {
            return JsonConvert.DeserializeObject(data, type);
        }

        #endregion
    }
}