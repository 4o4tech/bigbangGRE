using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Linguistics;
using Microsoft.ProjectOxford.Linguistics.Contract;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


using Android.App;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Text;
using Android.Util;

namespace bigbangGRE
{
    [Activity(Label = "bigbangGRE", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

           
            Button submit = FindViewById<Button>(Resource.Id.submit_button);
            EditText inputText = FindViewById<EditText>(Resource.Id.InputText);
            TextView outputText = FindViewById<TextView>(Resource.Id.OutputText);
            

            submit.Click += delegate
            {
                string str = inputText.Text.ToString();

                string outputStr = running(str);

                outputText.Text += str;

                outputText.Text += outputStr;

            };

            

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


        public string  running(string inputText)
        {
            // List analyzers
            Analyzer[] supportedAnalyzers = null;
            try
            {
                supportedAnalyzers = ListAnalyzers();
                var analyzersAsJson = JsonConvert.SerializeObject(supportedAnalyzers, Formatting.Indented, jsonSerializerSettings);
                //make the analyze sentence become json formate
                Log.Info("The json is: ", analyzersAsJson);
                //Console.WriteLine("Supported analyzers: " + analyzersAsJson);
            }
            catch (Exception e)
            {
                Log.Error("Failed to list supported analyzers",e.ToString());
                //Console.Error.WriteLine("Failed to list supported analyzers: " + e.ToString());
                System.Environment.Exit(1);
            }

            // Analyze text with all available analyzers
            var analyzeTextRequest = new AnalyzeTextRequest()
            {
                Language = "en",
                AnalyzerIds = supportedAnalyzers.Select(analyzer => analyzer.Id).ToArray(),
                Text = inputText
            };

            try
            {
                var analyzeTextResults = AnalyzeText(analyzeTextRequest);
                var resultsAsJson = JsonConvert.SerializeObject(analyzeTextResults, Formatting.Indented, jsonSerializerSettings);


                return resultsAsJson;
                //Console.WriteLine("Analyze text results: " + resultsAsJson);


            }
            catch (Exception e)
            {

                Log.Error("Failed to list supported analyzers:", e.ToString());
                //Console.Error.WriteLine("Failed to list supported analyzers: " + e.ToString());
                return "Sorry, Error";
                System.Environment.Exit(1);
            }

            
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

