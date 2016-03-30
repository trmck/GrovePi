using System;
using Windows.ApplicationModel.Background;
using GrovePi.Sensors;
using GrovePi;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace UltrasonicBuzzer
{
    public sealed class StartupTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            IUltrasonicRangerSensor rangeSensor = DeviceFactory.Build.UltraSonicSensor(Pin.DigitalPin3);
            IBuzzer buzzer = DeviceFactory.Build.Buzzer(Pin.DigitalPin2);

            int distance = 0;

            while (true)
            {
                try
                {
                    distance = rangeSensor.MeasureInCentimeters();
                    System.Diagnostics.Debug.WriteLine(distance + " cm");
                    if (distance < 10)
                    {
                        buzzer.ChangeState(SensorStatus.On);
                        System.Diagnostics.Debug.WriteLine("Buzzing\n");
                    }
                    else
                    {
                        buzzer.ChangeState(SensorStatus.Off);
                    }
                    System.Threading.Tasks.Task.Delay(100).Wait();
                }
                catch (Exception ex)
                {
                    if (buzzer.CurrentState == SensorStatus.On)
                    {
                        buzzer.ChangeState(SensorStatus.Off);
                    }
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}
