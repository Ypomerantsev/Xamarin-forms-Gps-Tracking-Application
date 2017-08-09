using DeviceMotion.Plugin.Abstractions;
using Android.Hardware;
using Android.Content;
using Android.App;
using Android.Runtime;
using System;
using System.Collections.Generic;


namespace App1.Droid.LA
{
    public class LinearAccelerationImplementation: Java.Lang.Object, ISensorEventListener , IDeviceMotion
    {
        private SensorManager sensorManager;
        private Sensor sensorAccelerometer;
        private Sensor sensorLinearAcceleration;
        public static bool zaznalLinearAcceleration = true;
        public static bool zaznalAccelerometer = true;

        public Dictionary<SensorType, bool> sensorStatus = new Dictionary<SensorType, bool>(){
				{ SensorType.LinearAcceleration, false},
				{ SensorType.Gyroscope, false},
				{ SensorType.Accelerometer, false},
                { SensorType.Gravity,false}
			};

        public LinearAccelerationImplementation() : base()
        {
            sensorManager = (SensorManager)Application.Context.GetSystemService(Context.SensorService);
            sensorAccelerometer = sensorManager.GetDefaultSensor(SensorType.Accelerometer);
            sensorLinearAcceleration = sensorManager.GetDefaultSensor(SensorType.LinearAcceleration);
            sensorAccelerometer = sensorManager.GetDefaultSensor(SensorType.Accelerometer);

            if (sensorLinearAcceleration == null)
            {
                zaznalLinearAcceleration = false;
            }
            if (sensorAccelerometer == null)
            {

                zaznalAccelerometer = false;
            }
        }

        /// <summary>
        /// Occurs when sensor value changed.
        /// </summary>
        public event SensorValueChangedEventHandler SensorValueChanged;

        /// <Docs>To be added.</Docs>
        /// <summary>
        /// Called when the accuracy of a sensor has changed.
        /// </summary>
        /// <para tool="javadoc-to-mdoc">Called when the accuracy of a sensor has changed.</para>
        /// <param name="sensor">Sensor.</param>
        /// <param name="accuracy">Accuracy.</param>
        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {

        }

        /// <summary>
        /// Raises the sensor changed event.
        /// </summary>
        /// <param name="e">E.</param>
        public void OnSensorChanged(SensorEvent e)
        {
            if (SensorValueChanged == null)
                return;

            switch (e.Sensor.Type) {
                case SensorType.LinearAcceleration:
                    SensorValueChanged(this, new SensorValueChangedEventArgs() { ValueType = MotionSensorValueType.Vector, SensorType = SensorType.LinearAcceleration, Value = new MotionVector() { X = e.Values[0], Y = e.Values[1], Z = e.Values[2] } });
                    break;

                case SensorType.Accelerometer:
                    SensorValueChanged(this, new SensorValueChangedEventArgs() { ValueType = MotionSensorValueType.Vector, SensorType = SensorType.Accelerometer, Value = new MotionVector() { X = e.Values[0], Y = e.Values[1], Z = e.Values[2] } });
                    break;

        }
            





        }




        /// <summary>
        /// Start the specified sensorType and interval.
        /// </summary>
        /// <param name="sensorType">Sensor type.</param>
        /// <param name="interval">Interval.</param>
        public void StartAll(MotionSensorDelay interval = MotionSensorDelay.Default)
        {


            SensorDelay delay = SensorDelay.Normal;
            switch (interval)
            {
                case MotionSensorDelay.Fastest:
                    delay = SensorDelay.Fastest;
                    break;
                case MotionSensorDelay.Game:
                    delay = SensorDelay.Game;
                    break;
                case MotionSensorDelay.Ui:
                    delay = SensorDelay.Ui;
                    break;

            }

            if (sensorAccelerometer != null)
            {
                sensorManager.RegisterListener(this, sensorAccelerometer, delay);
                sensorStatus[SensorType.Accelerometer] = true;
            }
            else
                Console.WriteLine("Accelerometer not available");

            if (sensorLinearAcceleration != null)
            {
                sensorManager.RegisterListener(this, sensorLinearAcceleration, delay);
                sensorStatus[SensorType.LinearAcceleration] = true;
            }
            else
                Console.WriteLine("LinearAcceleration not available");



        }

        public void Start(SensorType Tip, MotionSensorDelay interval = MotionSensorDelay.Default)
        {


            SensorDelay delay = SensorDelay.Normal;
            switch (interval)
            {
                case MotionSensorDelay.Fastest:
                    delay = SensorDelay.Fastest;
                    break;
                case MotionSensorDelay.Game:
                    delay = SensorDelay.Game;
                    break;
                case MotionSensorDelay.Ui:
                    delay = SensorDelay.Ui;
                    break;

            }

            //TODO IMPLEMENT IF NECESSARY! s switch stavkom

            if (sensorLinearAcceleration != null)
                sensorManager.RegisterListener(this, sensorLinearAcceleration, delay);
            else
                Console.WriteLine("Accelerometer not available");


            sensorStatus[Tip] = true;


        }

        /// <summary>
        /// Stop the specified sensorType.
        /// </summary>
        /// <param name="sensorType">Sensor type.</param>
        public void Stop(SensorType SensorType)
        {

            //TODO IMPLEMENT IF NECESSARY!!!!!!

            //if (sensorLinearAcceleration != null)
            //    sensorManager.UnregisterListener(this, sensorLinearAcceleration);
            //else
            //    Console.WriteLine("Accelerometer not available");

        }

        public void StopAll()
        {

            if (sensorAccelerometer != null)
                sensorManager.UnregisterListener(this, sensorAccelerometer);
            else
                Console.WriteLine("Accelerometer not available");

            if (sensorLinearAcceleration != null)
                sensorManager.UnregisterListener(this, sensorLinearAcceleration);
            else
                Console.WriteLine("LinearAcceleration not available");

            sensorStatus[SensorType.Accelerometer] = false;
            sensorStatus[SensorType.LinearAcceleration] = false;
        }

        public bool IsActive(SensorType sensorType)
        {
            return sensorStatus[sensorType];
        }
    }
}