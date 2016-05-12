using System;
using System.Collections.Generic;

namespace TraceableGalleryApp.Interfaces
{
    public interface IJsonHelper
    {
        T Deserialize<T>(string jsonString, List<Type> converterTypes = null);

        string Serialize<T>(T jsonObject, List<Type> converterTypes = null);
    }
}

