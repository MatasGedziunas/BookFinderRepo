using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace BookFinderProject.Models
{
    public class ApiResponse
    {
        [JsonProperty("total_results")]
        public int TotalResults { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("results")]
        public List<Book> Results { get; set; }
    }

    [Serializable]
    public class Book
    {
        public List<string> subcategories { get; set; }
        public bool english_language_learner { get; set; }
        public List<string> vocab_words { get; set; }
        public int? page_count { get; set; }
        public string title_search { get; set; }
        public List<string> author_first_names { get; set; }
        public int? max_age { get; set; }
        public string canonical_published_work_id { get; set; }
        public int? copyright { get; set; }
        public string title { get; set; }
        public Measurement measurements { get; set; }
        public List<string> subject_tags { get; set; }
        public List<object> chapter_measurements { get; set; }
        public List<string> recommended_isbns { get; set; }
        public List<PublishedWork> published_works { get; set; }
        public List<string> author_last_names { get; set; }
        public string series_name { get; set; }
        public int? min_age { get; set; }
        public string book_type { get; set; }
        public List<string> awards { get; set; }
        public List<string> authors { get; set; }
        public List<string> category { get; set; }
        public string language { get; set; }
        public string summary { get; set; }
        public string work_id { get; set; }
        public string canonical_isbn { get; set; }
    }

    public class Measurement
    {
        public English english { get; set; }
    }

    public class English
    {
        public double? mlf { get; set; }
        public object syntactic_demand_percentile { get; set; }
        public object semantic_demand_percentile { get; set; }
        public string lexile_code { get; set; }
        public object decoding_demand_percentile { get; set; }
        public bool measurable { get; set; }
        public double? msl { get; set; }
        public int? lexile { get; set; }
        public object structure_demand_percentile { get; set; }
    }

    public class PublishedWork
    {
        public string isbn { get; set; }
        public bool english_language_learner { get; set; }
        public int? copyright { get; set; }
        public string published_work_id { get; set; }
        public int? page_count { get; set; }
        public string binding { get; set; }
        public string cover_art_url { get; set; }
    }


}