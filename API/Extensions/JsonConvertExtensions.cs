using API.Entities;
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
            int count =source.Count;
            List<T> results = new List<T>();
            try
            {
                for (int i = 0; i < source.Count; i++)
                {
                    T item = JsonConvert.DeserializeObject<T>(((JArray)source[i]).ToString());
                    // This adds the deserialized list in the format into the type we are returning
                    results.Add(item);
                }
            }
            catch (JsonSerializationException jsEx)
            {
                throw new FailedToConvertFromJson($" The Extension failed to convert {results[results.Count]} to {typeof(T)}",jsEx);
            }
            catch(ArgumentOutOfRangeException argEx)
            {
                throw new FailedToConvertFromJson($" The Extension failed to convert {results[results.Count]} to {typeof(T)}", argEx);
            }
            catch (InvalidCastException inEx)
            {
                throw new FailedToConvertFromJson($" The Extension failed to convert {results[results.Count]} to {typeof(T)}"+"It might be cause it is expecting a JObject but we are " +
                    "trying to cast it to a JArray, but really you should look a little deeper", inEx);
            }

            return results;
        }


        /// <summary>
        /// This is an overload that takes in the response when it is a single json object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns> A list of type <typeparamref name="T"/></returns>
      // OBSOLETE: This has been marked obsolete cause the complier can't distinguish between a list of objects and an object, it just takes it both as an object, so it fights with lists
      //  [Obsolete]
      //  public static T FromJsonToObject<T>(this object source)
      //where T : class, new()
      //  {
      //      T result = new T();
      //      try
      //      {
      //          result= JsonConvert.DeserializeObject<T>(((JObject)source).ToString());
      //      }
      //      catch (JsonSerializationException jsEx)
      //      {
      //          throw new FailedToConvertFromJson($" The Extension failed to convert {result} to {typeof(T)}", jsEx);
      //      }
      //      catch (InvalidCastException inEx)
      //      {
      //          throw new FailedToConvertFromJson($" The Extension failed to convert {result} to {typeof(T)}", inEx);
      //      }

      //      return result;
      //  }

    }
}
