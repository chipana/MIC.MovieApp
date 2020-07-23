using Newtonsoft.Json;
using System;
using System.Linq;

namespace APPFilmes
{
    public class FilmesJson
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Rated { get; set; }
        public string Release { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Awards { get; set; }
        public string Poster { get; set; }
        public string MetaScore { get; set; }
        [JsonIgnore]
        public double MetaScoreint
        {
            get { return Convert.ToDouble(MetaScore); }
        }
        public string Imdbrating { get; set; }
        [JsonIgnore]
        public float ImdbRatingFloat
        {
            get { return (float)Math.Round(Convert.ToDouble(Imdbrating)); }
        }
        public string ImdbVotes { get; set; }
        [JsonIgnore]
        public int ImdbVotesInt
        {
            get { return Convert.ToInt32(ImdbVotes.Replace(",", string.Empty)); }
        }
        public string ImdbId { get; set; }
        public string type { get; set; }
        [JsonIgnore]
        public string TypeF
        {
            get
            {
                if (!string.IsNullOrEmpty(type))
                    return type.First().ToString().ToUpper() + type.Substring(1);
                else
                    return null;
            }
        }
        public bool response { get; set; }
        [JsonIgnore]
        public string imgtype
        {
            get
            {
                switch (type)
                {
                    case "movie":
                        return "Assets/moviecam.png";
                    case "game":
                        return "Assets/Controller-64.png";
                    case "series":
                        return "Assets/HDTV-64.png";
                    default:
                        return null;
                }
            }
        }
        public FilmesJson()
        {
        }
        public FilmesJson(string Title, string Year, string Rated, string Release, string Runtime, string Genre, string Director, string Writer, string Actors, string Plot, string Language, string Country, string Awards, string Poster, string MetaScore, string Imdbrating, string ImdbVotes, string ImdbId, string type, bool response)
        {
            this.Title = Title;
            this.Year = Year;
            this.Rated = Rated;
            this.Release = Release;
            this.Runtime = Runtime;
            this.Genre = Genre;
            this.Director = Director;
            this.Writer = Writer;
            this.Actors = Actors;
            this.Plot = Plot;
            this.Language = Language;
            this.Country = Country;
            this.Awards = Awards;
            this.Poster = Poster;
            this.MetaScore = MetaScore;
            this.Imdbrating = Imdbrating;
            this.ImdbVotes = ImdbVotes;
            this.ImdbId = ImdbId;
            this.type = type;
            this.response = response;
        }
    }
}
