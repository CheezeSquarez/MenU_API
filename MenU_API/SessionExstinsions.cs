using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace MenU_API
{
    public static class SessionExtensions
    {
        //The first parameter (this) in a static function define an extention method to the class type mentioned after the "this" param!
        //It is a cosmetic method that allow developers to wrte the GetObject method as if it was originally written as part of the class.
        //It does not provide access to any private members in the class! Only replace the alternative of having a static function that gets the object as first param.
        public static T GetObject<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(data);
        }

        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
    }
}
