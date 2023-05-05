using System;

namespace Israiloff.Cashbox.Component.Serializer
{
    public interface ISerializer
    {
        string Serialize(object target, bool isPretty = false);

        TResult Deserialize<TResult>(string data);

        object Deserialize(string data, Type type);
    }
}