using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TraceableGalleryApp.Interfaces;

namespace TraceableGalleryApp.Utilities
{
    public class JsonHelper : IJsonHelper
    {
        public T Deserialize<T>(string jsonString, List<Type> converterTypes = null)
        {
            var settings = new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
                Error = HandleDeserializationError
            };

            if (converterTypes != null)
                settings.Converters = GetConverters(converterTypes);

            try {
                return JsonConvert.DeserializeObject<T> (jsonString, settings);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine (ex.Message);
                return default(T);
            }
        }

        public string Serialize<T>(T jsonObject,  List<Type> converterTypes = null)
        {
            if (converterTypes != null) 
                return JsonConvert.SerializeObject (jsonObject, Formatting.Indented, new JsonSerializerSettings {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Converters = GetConverters(converterTypes)
                });

            return JsonConvert.SerializeObject (jsonObject);
        }

        static List<JsonConverter> GetConverters(List<Type> types)
        {
            var converters = new List<JsonConverter>();

            foreach (var type in types)
            {
                var converter = Activator.CreateInstance(type) as JsonConverter;
                if (converter != null)
                    converters.Add(converter);
            }

            return converters;
        }

        static void HandleDeserializationError(object sender, ErrorEventArgs errorArgs)
        {
            var currentError = errorArgs.ErrorContext.Error.Message;
            errorArgs.ErrorContext.Handled = true;

            System.Diagnostics.Debug.WriteLine (currentError);
        }
    }
}

