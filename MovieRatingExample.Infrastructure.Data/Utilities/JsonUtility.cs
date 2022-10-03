using Newtonsoft.Json;

namespace MovieRatingExample.Infrastructure.Data.Utilities
{
    public static class JsonUtility
    {
        public static T FromJson<T>(this string file)
        {
            return JsonConvert.DeserializeObject<T>(file);
        }

        public static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}
