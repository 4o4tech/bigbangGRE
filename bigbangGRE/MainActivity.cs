using Android.App;
using Android.Widget;
using Android.OS;

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

            Button submit = FindViewById<Button>(Resource.Id.submit_button);


            submit.Click += delegate
            {

            };



        }

        protected string putRequest()
        {




            return null;
        }
    }
}

