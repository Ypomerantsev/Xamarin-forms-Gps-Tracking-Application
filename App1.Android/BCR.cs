using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App1.Droid
{
    [Activity(Label = "BroadcastReceiver")]
    public class BCR : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {

            if (intent.Action == Intent.ActionBootCompleted)
            {
                Toast.MakeText(context, "Hello - the app has hit the right place - now to launch the app", ToastLength.Long).Show();
                //context.StartActivity(typeof(MainActivity));
                //Toast.MakeText(context, "Pride do BCR", ToastLength.Long).Show();
                Intent intend = new Intent(context, typeof(Servis));
                intend.SetAction("zazeni");
                context.StartService(intend);
                //Intent intentacc = new Intent(context, typeof(MainActivity));
                //intentacc.PutExtra("BCR","stared_by_BCR");
                //context.StartActivity(intentacc);
            }

        }
    }
}