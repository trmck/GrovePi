using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using GrovePi;
using GrovePi.Sensors;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace ButtonToggleRelay
{
    public sealed class StartupTask : IBackgroundTask
    {

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            IButtonSensor button = DeviceFactory.Build.ButtonSensor(Pin.DigitalPin4);
            IRelay relay = DeviceFactory.Build.Relay(Pin.DigitalPin2);
            var g = DeviceFactory.Build.GrovePi();
            System.Diagnostics.Debug.WriteLine(g.GetFirmwareVersion());

            SensorStatus toggleState = SensorStatus.Off;

            while (true)
            {
                try
                {
                    var btnState = button.CurrentState;

                    if (btnState == SensorStatus.On)
                    {
                        toggleState = (toggleState == SensorStatus.Off) ? SensorStatus.On : SensorStatus.Off;
                    }

                    relay.ChangeState(toggleState);
                    System.Threading.Tasks.Task.Delay(100).Wait();
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception occurred: " + ex.Message);
                }
            }
        }
    }
}
