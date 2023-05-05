using System;
using Newtonsoft.Json;

namespace Israiloff.Cashbox.Component.Serializer.Json
{
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