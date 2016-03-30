using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using GrovePi;
using GrovePi.Sensors;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace SoundLed
{
    public sealed class StartupTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // Threshold to turn the led on 400.00 * 5 / 1024 = 1.95v
            const int THRESHOLD_VALUE = 325;

            ISoundSensor microphone = DeviceFactory.Build.SoundSensor(Pin.AnalogPin1);
            ILed led = DeviceFactory.Build.Led(Pin.DigitalPin5);

            double brightness = 0;
            double soundLevel = 0;

            while (true)
            {
                try
                {
                    soundLevel = microphone.SensorValue();
                    System.Diagnostics.Debug.WriteLine("Sound level is " + soundLevel.ToString());

                    if (soundLevel > THRESHOLD_VALUE)
                    {
                        int sMin = THRESHOLD_VALUE;
                        int sMax = THRESHOLD_VALUE * 2;
                        int bMin = 0;
                        int bMax = 255;

                        brightness = Math.Floor(((soundLevel - sMin) * bMax) / (sMax - sMin) + bMin);
                    }
                    else
                    {
                        brightness = 0;
                    }

                    led.AnalogWrite(Convert.ToByte(brightness));
                }
                catch (Exception ex)
                {
                    // NOTE: There are frequent exceptions of the following:
                    // WinRT information: Unexpected number of bytes was transferred. Expected: '. Actual: '.
                    // This appears to be caused by the rapid frequency of writes to the GPIO
                    // These are being swallowed here/

                    // If you want to see the exceptions uncomment the following:
                    // System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
        }
    }
}
