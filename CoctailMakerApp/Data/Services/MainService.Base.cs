﻿using CoctailMakerApp.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoctailMakerApp.Data.Services
{
    public partial class MainService
    {
        protected CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        protected DatabaseService DatabaseService { get; }
        protected ILogger<MainService> Logger { get; }        

        public MainService(DatabaseService databaseService, ILogger<MainService> logger)
        {
            DatabaseService = databaseService;
            Logger = logger;
            Start();
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

        public event Action<SystemStatus> OnSystemStatusChanged;
        public Task UpdateSystemStatus(SystemStatus systemStatus)
        {
            return Task.Run(() => OnSystemStatusChanged?.Invoke(systemStatus));
        }
    }
}
