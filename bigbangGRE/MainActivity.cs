using System;
using System.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;



using Android.App;
using Android.Widget;
using Android.OS;
using Android.Util;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;

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
        // Gets weather data from the passed URL.
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

     

        static async Task<string> MakeRequest(string inputStr){
                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);

                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "f434b0a9708e47b3bf185bdbff3bd39c");

                var uri = "https://api.projectoxford.ai/linguistics/v1.0/analyze?" + queryString;

                HttpResponseMessage response;

                var reqBody = new Body
                {

                /*
                    
                "language" : "en",
		        "analyzerIds" : ["4fa79af1-f22c-408d-98bb-b7d7aeef7f04", "22a6b758-420f-4745-8a3c-46835a67c0d2"],
		        "text" : "Hi, Macus! How are you today? Can you help me solve some GRE question?" 
                     
                */
                    language = "en",
                    analyzerIds = "",
                    text = inputStr
                };


            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Body));
            MemoryStream msObj = new MemoryStream();
            //将序列化之后的Json格式数据写入流中
            js.WriteObject(msObj, reqBody);
            msObj.Position = 0;
            //从0这个位置开始读取流中的数据
            StreamReader sr = new StreamReader(msObj, Encoding.UTF8);
            string json = sr.ReadToEnd();
            sr.Close();
            msObj.Close();

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(json);

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await client.PostAsync(uri, content);
                
                }

           return  response.ToString();
        }


        public class Body
        {
            
            public string language { get; set; }
          
            public string analyzerIds { get; set; }

            public string text { get; set; }

        }


    }
}

