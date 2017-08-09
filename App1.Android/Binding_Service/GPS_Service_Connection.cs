using Android.Util;
using Android.OS;
using Android.Content;

namespace App1.Droid
{
	public class GPS_Service_Connection : Java.Lang.Object, IServiceConnection
	{
		static readonly string TAG = typeof(GPS_Service_Connection).FullName;

		MainActivity mainActivity;
		public GPS_Service_Connection(MainActivity activity)
		{
			IsConnected = false;
			Binder = null;
			mainActivity = activity;
		}

		public bool IsConnected { get; private set; }
		public GPS_Service_Binder Binder { get; private set; }

		public void OnServiceConnected(ComponentName name, IBinder service)
		{
			Binder = service as GPS_Service_Binder;
			IsConnected = this.Binder != null;

            string message = "onServiceConnected - ";
			Log.Debug(TAG, $"OnServiceConnected {name.ClassName}");

			if (IsConnected)
			{
                message = message + " bound to service " + name.ClassName;
			}
			else
			{
				message = message + " not bound to service " + name.ClassName;
			}

            Log.Info(TAG, message);

		}

		public void OnServiceDisconnected(ComponentName name)
		{
			Log.Debug(TAG, $"OnServiceDisconnected {name.ClassName}");
			IsConnected = false;
			Binder = null;
		}
	}

}
