using Newtonsoft.Json;
using PrismUnoSampleApp.EnglishRestaurant.Domains;
using PrismUnoSampleApp.EnglishRestaurant.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PrismUnoSampleApp.EnglishRestaurant.NetworkServices
{
    public class PictureTextReader : IPictureTextReader
    {
        private const string OcrApiDefaultParameters = "?language=unk&detectOrientation=true";
        private const string OcrApi = "vision/v2.1/ocr";
        private const string OcpApimSubscriptionKeyHeaderName = "Ocp-Apim-Subscription-Key";
        private readonly HttpClient _client;
        private readonly OcrConfiguration _ocrConfiguration;

        public PictureTextReader(HttpClient client, OcrConfiguration ocrConfiguration)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _ocrConfiguration = ocrConfiguration ?? throw new ArgumentNullException(nameof(ocrConfiguration));
        }
        public async Task<(bool ok, DetectedText[] result)> ReadTextsAsync(byte[] value)
        {
            var content = new ByteArrayContent(value);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            content.Headers.Add(OcpApimSubscriptionKeyHeaderName, _ocrConfiguration.SubscriptionKey);
            var response = await _client.PostAsync($"{CreateOrcApiEndpoint()}{OcrApiDefaultParameters}", content);
            if (!response.IsSuccessStatusCode)
            {
                return (false, null);
            }

            var results = JsonConvert.DeserializeObject<OcrResult>(await response.Content.ReadAsStringAsync());
            var lines = results.Regions.SelectMany(x => x.Lines);
            var detectedTexts = lines.Select(x => string.Join(" ", x.Words.Select(y => y.Text)))
                .Select(x => new DetectedText(x))
                .ToArray();
            return (true, detectedTexts);
        }

        private string CreateOrcApiEndpoint() => 
            $"{_ocrConfiguration.Endpoint}{(_ocrConfiguration.Endpoint.EndsWith("/") ? "" : "/")}{OcrApi}";
    }
}
