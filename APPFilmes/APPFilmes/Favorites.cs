using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Windows.Storage;
using System.Diagnostics;

namespace APPFilmes
{
    class Favorites
    {
        public static async Task<string> ReadFavorites()
        {
            StorageFolder sfold = ApplicationData.Current.LocalFolder;
            StorageFile local = await sfold.CreateFileAsync("Favorites.json", CreationCollisionOption.OpenIfExists);
            string sf = await FileIO.ReadTextAsync(local);
            Debug.WriteLine(local.Path);
            return sf;
        }
        public static List<FilmesJson> GetFavorites(string data)
        {
            string raw = JObject.Parse(data).SelectToken("Favorites").ToString();
            List<FilmesJson> movieList = JsonConvert.DeserializeObject<List<FilmesJson>>(raw);
            return movieList;
        }
        public static async Task<bool> AddFavorite(FilmesJson favorite)
        {
            if (favorite == null || string.IsNullOrEmpty(favorite.Title))
            {
                return false;
            }
            else
            {
                List<FilmesJson> moviesList = new List<FilmesJson>();
                StorageFolder sfold = ApplicationData.Current.LocalFolder;
                StorageFile local = await sfold.CreateFileAsync("Favorites.json", CreationCollisionOption.OpenIfExists);
                string str = await FileIO.ReadTextAsync(local);
                if (!string.IsNullOrEmpty(str))
                    moviesList = GetFavorites(str);
                moviesList.Add(favorite);
                JsonSerializerSettings conf = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                JArray arr = JArray.Parse(JsonConvert.SerializeObject(moviesList, Formatting.None, conf));
                JObject newFav = new JObject();
                newFav.Add("Favorites", arr);
                await FileIO.WriteTextAsync(local, newFav.ToString());
                return true;
            }
        }
        public static async Task<bool> UnFavorite(FilmesJson favorite)
        {
            if (favorite == null || string.IsNullOrEmpty(favorite.Title))
            {
                return false;
            }
            else
            {
                List<FilmesJson> moviesList = new List<FilmesJson>();
                StorageFolder sfold = ApplicationData.Current.LocalFolder;
                StorageFile local = await sfold.CreateFileAsync("Favorites.json", CreationCollisionOption.OpenIfExists);
                string str = await FileIO.ReadTextAsync(local);
                if (!string.IsNullOrEmpty(str))
                    moviesList = GetFavorites(str);
                favorite = moviesList.FirstOrDefault(p => p.Title == favorite.Title && p.ImdbId == favorite.ImdbId);
                bool resp = false;
                if (favorite != null)
                {
                    resp = moviesList.Remove(favorite);
                    JsonSerializerSettings conf = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    JArray arr = JArray.Parse(JsonConvert.SerializeObject(moviesList, Formatting.None, conf));
                    JObject newFav = new JObject();
                    newFav.Add("Favorites", arr);
                    await FileIO.WriteTextAsync(local, newFav.ToString());
                }
                return resp;
            }
        }
        public static async Task<bool> IsFavorite(FilmesJson favorite)
        {
            if (favorite == null || string.IsNullOrEmpty(favorite.Title))
            {
                return false;
            }
            else
            {
                List<FilmesJson> moviesList = new List<FilmesJson>();
                StorageFolder sfold = ApplicationData.Current.LocalFolder;
                StorageFile local = await sfold.CreateFileAsync("Favorites.json", CreationCollisionOption.OpenIfExists);
                string str = await FileIO.ReadTextAsync(local);
                if (!string.IsNullOrEmpty(str))
                    moviesList = GetFavorites(str);
                bool resp = moviesList.FirstOrDefault(p => p.Title == favorite.Title && p.ImdbId == favorite.ImdbId) != null;
                return resp;
            }
        }
    }
}
