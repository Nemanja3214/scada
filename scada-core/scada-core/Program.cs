﻿
using scada_core_6.ApiClient;
using scada_core.SimulationDriver;
using scada_core.TagProcessing;

namespace scada_core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ApiClient apiClient = new ApiClient();
            TagProcessor tagProcessingService = new TagProcessor(apiClient);
            tagProcessingService.InitializeTagThreads();
            
            SimulationDriver.SimulationDriver sd = new SimulationDriver.SimulationDriver(apiClient);
            Task simulation = Task.Run(() => sd.Simulate());

            RealTimeDriver rtd = new RealTimeDriver(apiClient);
            Task realTime = Task.Run(() => rtd.Simulate());

            simulation.Wait();
            realTime.Wait();
        }
    }
}