using CoctailMakerApp.Data.Context;
using CoctailMakerApp.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;

namespace CoctailMakerApp.Data.Services
{
    public partial class MainService
    {
        public SystemStatus Status { get; protected set; } = new SystemStatus();

        protected async Task MainLoopAsync()
        {
            var ledPin = 1;

            GpioController gpio = null;
            SerialPort uart = null;
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                // see https://docs.microsoft.com/en-us/dotnet/iot/tutorials/blink-led
                gpio = new GpioController();
                if (!gpio.IsPinOpen(ledPin))
                {
                    gpio.OpenPin(ledPin, PinMode.Output);
                }

                // see https://docs.microsoft.com/de-de/dotnet/api/system.io.ports.serialport?view=dotnet-plat-ext-5.0
                uart = new SerialPort();
            }

            var token = cancellationTokenSource.Token;
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(1000);
                Status.Led1State = !Status.Led1State;
                Status.SystemTime = DateTime.Now;
                _ = UpdateSystemStatus(Status);

                
                gpio?.Write(ledPin, Status.Led1State ? PinValue.High : PinValue.Low);
            }
        }
    }
}
