using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    //NOTE: I'm under the impression that extension methods, don't need to be added in AddApplicationServices, because them being static means they are assesable
    /// <summary>
    /// This is an extention class for turning responses from the database to the object types required
    /// </summary>
    public static class JsonConvertExtensions
    {
        // NOTE: For example it will be used like , AppUser response = await _firebaseDataContext.GetData(dir).FromJsonToObject<AppUser>()
        // NOTE: firebase response usually comes out as a list of objects
        /// <summary>
        /// This is to be used of firebaseResponses to convert them to the correct type we want
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns> A list of type <typeparamref name="T"/></returns>
        public static List<T> FromJsonToObject<T>(this List<object> source)
       where T : class, new()
        {
            List<T> results = new List<T>();

            for (int i = 0; i < source.Count; i++)
            {
                T item = JsonConvert.DeserializeObject<T>(((JObject)source[i]).ToString());
                // This adds the deserialized list in the format into the type we are returning
                results.Add(item);
            }
            return results;
        }

        /// <summary>
        /// This is an overload that takes in the response when it is a single json object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns> A list of type <typeparamref name="T"/></returns>
        public static T FromJsonToObject<T>(this object source)
      where T : class, new()
        {
            return JsonConvert.DeserializeObject<T>(((JObject)source).ToString()); 
        }

    }
}
