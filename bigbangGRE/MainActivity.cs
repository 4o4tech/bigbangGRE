using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;


using Android.App;
using Android.Widget;
using Android.OS;
using Android.Util;
using Newtonsoft.Json;
using System.Web;

namespace bigbangGRE
{
    [Activity(Label = "bigbangGRE", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            Button submit = FindViewById<Button>(Resource.Id.submit_button);
            EditText inputText = FindViewById<EditText>(Resource.Id.InputText);
            TextView outputText = FindViewById<TextView>(Resource.Id.OutputText);


            submit.Click += async delegate
            {
                string str = inputText.Text.ToString();

                string outputStr = "";

                //outputText.Text += str;

                outputStr = await MakeRequest(str);

                outputText.Text += outputStr;

                // Fetch the weather information asynchronously, 
                // parse the results, then update the screen:
                //JsonValue json = await FetchSentenceAsync(url);

            };
        }



        /*
        // Gets info data from the passed URL.
        private async Task<JsonValue> FetchSentenceAsync(string url)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                {
                    // Use this stream to build a JSON document object:
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                    // Return the JSON document:
                    return jsonDoc;
                }
            }
        }


        // Parse the weather data, then write temperature, humidity, 
        // conditions, and location to the screen.
        private void ParseAndDisplay(JsonValue json)
        {
            
            // Extract the array of name/value results for the field name "weatherObservation". 
            JsonValue analyzerId = json["analyzerId"];
            JsonValue result = json["result"];

        }

        */


         public async Task<string> MakeRequest(string inputStr){
                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);

                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "f434b0a9708e47b3bf185bdbff3bd39c");

                var uri = "https://api.projectoxford.ai/linguistics/v1.0/analyze?" + queryString;

                HttpResponseMessage response;

            string sendJson = toJson(inputStr);

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(sendJson);

            using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await client.PostAsync(uri, content);
        
                }



            string deValue = null ;


            if (response.StatusCode.ToString() == "OK")
            {
                var responseResult = response.Content.ReadAsStringAsync().Result;
                deValue = HttpUtility.UrlDecode(responseResult.ToString());

                return deValue;
            }
            return "No Value";
        }

        public string toJson(string inputStr)
        {

            /*

                   "language" : "en",
                   "analyzerIds" : ["4fa79af1-f22c-408d-98bb-b7d7aeef7f04", "22a6b758-420f-4745-8a3c-46835a67c0d2"],
                   "text" : "Hi, Macus! How are you today? Can you help me solve some GRE question?" 

            */


            toJson jsonStr = new toJson();
            jsonStr.language = "en";
            jsonStr.analyzerIds = new string[]{"4fa79af1-f22c-408d-98bb-b7d7aeef7f04", "22a6b758-420f-4745-8a3c-46835a67c0d2"};
            jsonStr.text = inputStr;

          
            string json = JsonConvert.SerializeObject(jsonStr);

         //   toJson deserializedStr = JsonConvert.DeserializeObject<toJson>(json);

            return json;
        }


    }


    public class toJson
    {

        public string language { get; set; }

        public string[] analyzerIds{ get; set; }

        public string text { get; set; }

    }
}

