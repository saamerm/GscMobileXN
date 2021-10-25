using System;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;

namespace MobileClaims.Core.Util
{
    public static class SerializationUtils
    {
        internal static string SerializeWithType(object toBeSerialized)
        {
            return JsonConvert.SerializeObject(toBeSerialized, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
            });
        }

        internal static T TryDeserializeMessageStatus<T>(string toDeserialize)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(toDeserialize, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}