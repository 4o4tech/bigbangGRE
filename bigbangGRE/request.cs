using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Linguistics;
using Microsoft.ProjectOxford.Linguistics.Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;  
using Android.Views;
using Android.Widget;

namespace bigbangGRE
{
    [Activity(Label = "Activity1")]
    public class Activity1 : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        private static readonly LinguisticsClient Client = new LinguisticsClient("e236c06d0f214a84853dc929b1c9984a");

        /// <summary>
        /// Default jsonserializer settings
        /// </summary>
        private static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        static void Main(string[] args)
        {
            // List analyzers
            Analyzer[] supportedAnalyzers = null;
            try
            {
                supportedAnalyzers = ListAnalyzers();
                var analyzersAsJson = JsonConvert.SerializeObject(supportedAnalyzers, Formatting.Indented, jsonSerializerSettings);
                //make the analyze sentence become json formate
                Console.WriteLine("Supported analyzers: " + analyzersAsJson);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Failed to list supported analyzers: " + e.ToString());
                System.Environment.Exit(1);
            }

            // Analyze text with all available analyzers
            var analyzeTextRequest = new AnalyzeTextRequest()
            {
                Language = "en",
                AnalyzerIds = supportedAnalyzers.Select(analyzer => analyzer.Id).ToArray(),
                Text = "Welcome to Microsoft Linguistic Analysis!"
            };

            try
            {
                var analyzeTextResults = AnalyzeText(analyzeTextRequest);
                var resultsAsJson = JsonConvert.SerializeObject(analyzeTextResults, Formatting.Indented, jsonSerializerSettings);

                Console.WriteLine("Analyze text results: " + resultsAsJson);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Failed to list supported analyzers: " + e.ToString());
                System.Environment.Exit(1);
            }

            Console.ReadKey();
        }

        /// <summary>
        /// List analyzers synchronously.
        /// </summary>
        /// <returns>An array of supported analyzers.</returns>
        private static Analyzer[] ListAnalyzers()
        {
            try
            {
                return Client.ListAnalyzersAsync().Result;
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to gather list of analyzers", exception as ClientException);
            }
        }

        /// <summary>
        /// Analyze text synchronously.
        /// </summary>
        /// <param name="request">Analyze text request.</param>
        /// <returns>An array of analyze text result.</returns>
        private static AnalyzeTextResult[] AnalyzeText(AnalyzeTextRequest request)
        {
            try
            {
                return Client.AnalyzeTextAsync(request).Result;
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to analyze text", exception as ClientException);
            }
        }


    }
}