using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using GrovePi.Sensors;
using GrovePi;
using GrovePi.I2CDevices;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace LightResponsiveLcdRgbDisplay
{
    public sealed class StartupTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            const int THRESHOLD_VALUE = 425;

            ILightSensor lightSensor = DeviceFactory.Build.LightSensor(Pin.AnalogPin0);
            IRgbLcdDisplay display = DeviceFactory.Build.RgbLcdDisplay();

            double lightLevel = 0;

            while (true)
            {
                try {
                    lightLevel = lightSensor.SensorValue();
                    System.Diagnostics.Debug.WriteLine("Light level is " + lightLevel.ToString());

                    if (lightLevel > THRESHOLD_VALUE)
                    {
                        // Set the RGB backlight to a light yellow-ish color
                        display.SetBacklightRgb(255, 255, 0);
                        display.SetText("Night-mode\ndisabled");
                    }
                    else
                    {
                        // Set the RGB backlight to a light blue-ish color
                        display.SetBacklightRgb(0, 255, 255);
                        display.SetText("Night-mode\nenabled");
                    }
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
}
