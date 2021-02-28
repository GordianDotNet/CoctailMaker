using CoctailMakerApp.Data.Context;
using CoctailMakerApp.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace CoctailMakerApp.Data.Services
{
    public class SystemStatus
    {
        public DateTime SystemTime { get; set; }
    }

    public class MainLoopService : DatabaseServiceBase
    {
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public MainLoopService(ILogger<MainLoopService> logger) : base(logger)
        {
            Start();
        }

        public event Action<SystemStatus> OnSystemStatusChanged;

        public Task UpdateSystemStatus(SystemStatus systemStatus)
        {
            return Task.Run(() => OnSystemStatusChanged?.Invoke(systemStatus));
        }

        public Task Start()
        {
            if (cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource = new CancellationTokenSource();
            }
            return Task.Run(MainLoopAsync, cancellationTokenSource.Token);
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }

        private async Task MainLoopAsync()
        {
            var ledPin = 1;
            var toggle = false;

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
                _ = UpdateSystemStatus(new SystemStatus { SystemTime = DateTime.Now });

                toggle = !toggle;
                gpio?.Write(ledPin, toggle ? PinValue.High : PinValue.Low);
            }
        }
    }
}
