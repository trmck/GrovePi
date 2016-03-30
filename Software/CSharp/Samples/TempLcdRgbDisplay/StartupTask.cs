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

namespace TempLcdRgbDisplay
{
    public sealed class StartupTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            ITemperatureAndHumiditySensor thSesnsor = DeviceFactory.Build.TemperatureAndHumiditySensor(Pin.DigitalPin8, Model.Dht11);
            IRgbLcdDisplay display = DeviceFactory.Build.RgbLcdDisplay();

            double tempC = 0;

            while(true)
            {
                try
                {
                    tempC = thSesnsor.TemperatureInCelsius();
                    double tempF = (tempC * (9 / 5)) + 32;

                    if (tempF < 68)
                        display.SetBacklightRgb(0, 255, 255); // Set the RGB backlight to a light blue-ish color
                    else
                        display.SetBacklightRgb(255, 255, 0); // Set the RGB backlight to a light yellow-ish color

                    display.SetText(String.Format("Temp: {0:0.#} F", tempF));
                    System.Threading.Tasks.Task.Delay(5 * 1000).Wait();
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
}
