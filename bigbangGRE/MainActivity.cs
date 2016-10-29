
using Android.App;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Text;

namespace bigbangGRE
{
    [Activity(Label = "bigbangGRE", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);

            string str = "";
            Button submit = FindViewById<Button>(Resource.Id.submit_button);
            EditText editText = FindViewById<EditText>(Resource.Id.InputText);
            EditText outputText = FindViewById<EditText>(Resource.Id.OutputText);
            str = editText.Text;

            submit.Click += delegate
            {

                postStr();



            };




        }

        protected string postStr()
        {
            string url = "https://www.microsoft.com/cognitive-services/Demo/SPLATDemo/Analyze";
            string postStr = "language=en&analyzerIds=4fa79af1-f22c-408d-98bb-b7d7aeef7f04%2C22a6b758-420f-4745-8a3c-46835a67c0d2%2C08ea174b-bfdb-4e64-987e-602f85da7f72&text=The+Linguistic+Analysis+API+simplifies+using+to+analyze+the+GRE+sentence.%0A++++++++++++++++++++++++&__RequestVerificationToken=xJambwornt4LEHnj1kCWsNqK6hED0e52kupfJs3ohu-z5fCFyOf3obf2YHU0zOg8Odx_LYKUfBd8RDJICZrcrRKELYXjtdGGBYbiueJmiDM1";
            var resposeStr= HttpPost(url, postStr);
            
            return resposeStr;
        }


        private string HttpPost(string Url, string postDataStr)
        {


            // code form http://www.cnblogs.com/xssxss/archive/2012/07/03/2574554.html

            CookieContainer cookie = new CookieContainer();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            request.CookieContainer = cookie;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36";
            request.Referer = "https://www.microsoft.com/cognitive-services/en-us/linguistic-analysis-api";
            

            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }


    }
}

