﻿
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PrismUnoSampleApp.EnglishRestaurant.NetworkServices
{
    // <auto-generated />
    //
    // To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
    //
    //    using QuickType;
    //
    //    var ocrResult = OcrResult.FromJson(jsonString);


    public partial class OcrResult
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("textAngle")]
        public double TextAngle { get; set; }

        [JsonProperty("orientation")]
        public string Orientation { get; set; }

        [JsonProperty("regions")]
        public Region[] Regions { get; set; }
    }

    public partial class Region
    {
        [JsonProperty("boundingBox")]
        public string BoundingBox { get; set; }

        [JsonProperty("lines")]
        public Line[] Lines { get; set; }
    }

    public partial class Line
    {
        [JsonProperty("boundingBox")]
        public string BoundingBox { get; set; }

        [JsonProperty("words")]
        public Word[] Words { get; set; }
    }

    public partial class Word
    {
        [JsonProperty("boundingBox")]
        public string BoundingBox { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public partial class OcrResult
    {
        public static OcrResult FromJson(string json) => JsonConvert.DeserializeObject<OcrResult>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this OcrResult self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
