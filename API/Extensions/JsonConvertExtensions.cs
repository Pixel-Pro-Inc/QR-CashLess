using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    /// <summary>
    /// This is an extention for turning responses from the database to and fro the object types required
    /// </summary>
    public static class JsonConvertExtensions
    {
        // TODO: create the JsonConverter that we see with all controllers

        /// <summary>
        /// This is to be used of firebaseResponses
        /// 
        /// For example it will be used like : AppUser response = await _firebaseDataContext.GetData(dir).ToObject<AppUser>();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<T> FromJsonToObject<T>(this List<object> source)
       where T : class, new()
        {
            List<T> results = new List<T>();

            for (int i = 0; i < source.Count; i++)
            {
                T item = JsonConvert.DeserializeObject<T>(((JObject)source[i]).ToString());

                results.Add(item);
            }
            return results;
        }

        public static T FromJsonToObject<T>(this object source)
      where T : class, new()
        {
            return JsonConvert.DeserializeObject<T>(((JObject)source).ToString()); 
        }

    }
}
