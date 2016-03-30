using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using GrovePi;
using GrovePi.Sensors;
using GrovePi.I2CDevices;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace EnvironmentMonitor
{
    public sealed class StartupTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            ILightSensor lightSensor = DeviceFactory.Build.LightSensor(Pin.AnalogPin0);
            ISoundSensor microphone = DeviceFactory.Build.SoundSensor(Pin.AnalogPin1);
            ITemperatureAndHumiditySensor thSesnsor = DeviceFactory.Build.TemperatureAndHumiditySensor(Pin.DigitalPin8, Model.Dht11);
            IUltrasonicRangerSensor rangeSensor = DeviceFactory.Build.UltraSonicSensor(Pin.DigitalPin3);

            IRgbLcdDisplay display = DeviceFactory.Build.RgbLcdDisplay();

            int lightLevel = 0;
            int soundLevel = 0;
            double tempC = 0;
            int distanceC = 0;

            bool toggleDisplay = true;
            int cycles = 0;

            while (true)
            {
                try
                {
                    if((cycles % 2) == 0)
                    {
                        distanceC = rangeSensor.MeasureInCentimeters();
                        tempC = thSesnsor.TemperatureInCelsius();
                    }
                    else
                    {
                        lightLevel = lightSensor.SensorValue();
                        soundLevel = microphone.SensorValue();
                    }
                    
                    double tempF = (tempC * (9 / 5)) + 32;

                    System.Diagnostics.Debug.WriteLine(String.Format("Light: {0}cd, Sound: {1}db, Temp: {2}F, Dist: {3}cm", lightLevel, soundLevel, tempF, distanceC));

                    if((cycles % 5) == 0)
                    {
                        toggleDisplay = !toggleDisplay;
                    }

                    display.SetBacklightRgb(0, 255, 255);
                    if (toggleDisplay)
                    {
                        display.SetText(String.Format("Light: {0:0.#}cd\nSound: {1:0.#}db", lightLevel, soundLevel));
                    }
                    else
                    {
                        display.SetText(String.Format("Temp: {0:0.#}F\nDist: {1}cm", tempF, distanceC));
                    }
                    cycles++;
                    System.Threading.Tasks.Task.Delay(1 * 1000).Wait();
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
}
