﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Hämmer Electronics">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   The main program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SparkplugNet.Examples
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Serilog;

    using SparkplugNet.Core.Application;
    using SparkplugNet.Core.Device;
    using SparkplugNet.Core.Node;

    using VersionAData = VersionA.Data;
    using VersionBData = VersionB.Data;

    /// <summary>
    /// The main program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The cancellation token source.
        /// </summary>
        private static readonly CancellationTokenSource CancellationTokenSource = new ();

        /// <summary>
        /// The version A metrics.
        /// </summary>
        private static readonly List<VersionAData.KuraMetric> VersionAMetrics = new ()
        {
            new ()
            {
                Name = "Test", Type = VersionAData.DataType.Double, DoubleValue = 1.20
            },
            new ()
            {
                Name = "Test2", Type = VersionAData.DataType.Bool, BoolValue = true
            }
        };

        /// <summary>
        /// The version A metrics.
        /// </summary>
        private static readonly List<VersionBData.Metric> VersionBMetrics = new ()
        {
            new VersionBData.Metric
            {
                Name = "Test", DataType = (uint)VersionBData.DataType.Double, DoubleValue = 1.20
            },
            new VersionBData.Metric
            {
                Name = "Test2",
                DataType = (uint)VersionBData.DataType.Boolean, BooleanValue = true
            }
        };

        /// <summary>
        /// The main method.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        public static async Task Main()
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .CreateLogger();

                await RunVersionA();
                // await RunVersionB();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Runs a version A simulation.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        private static async Task RunVersionA()
        {
            Console.WriteLine("Starting application...");
            var applicationMetrics = new List<VersionAData.KuraMetric>(VersionAMetrics);
            var application = new VersionA.SparkplugApplication(applicationMetrics, Log.Logger);
            var applicationOptions = new SparkplugApplicationOptions("localhost", 1883, "application1", "user", "password", false, "scada1", TimeSpan.FromSeconds(30), true, null, null, CancellationTokenSource.Token);
            await application.Start(applicationOptions);
            Console.WriteLine("Application started...");

            Console.WriteLine("Starting node...");
            var nodeMetrics = new List<VersionAData.KuraMetric>(VersionAMetrics);
            var node = new VersionA.SparkplugNode(nodeMetrics, Log.Logger);
            var nodeOptions = new SparkplugNodeOptions("localhost", 1883, "node 1", "user", "password", false, "scada1", "group1", "node1", TimeSpan.FromSeconds(30), null, null, CancellationTokenSource.Token);
            await node.Start(nodeOptions);
            Console.WriteLine("Node started...");

            Console.WriteLine("Starting device...");
            var deviceMetrics = new List<VersionAData.KuraMetric>(VersionAMetrics);
            var device = new VersionA.SparkplugDevice(deviceMetrics, Log.Logger);
            var deviceOptions = new SparkplugDeviceOptions("localhost", 1883, "device 1", "user", "password", false, "scada1", "group1", "node1", "device1", TimeSpan.FromSeconds(30), null, null, CancellationTokenSource.Token);
            await device.Start(deviceOptions);
            Console.WriteLine("Device started...");
        }

        /// <summary>
        /// Runs a version B simulation.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        private static async Task RunVersionB()
        {
            Console.WriteLine("Starting application...");
            var applicationMetrics = new List<VersionBData.Metric>(VersionBMetrics);
            var application = new VersionB.SparkplugApplication(applicationMetrics, Log.Logger);
            var applicationOptions = new SparkplugApplicationOptions("localhost", 1883, "application1", "user", "password", false, "scada1", TimeSpan.FromSeconds(30), true, null, null, CancellationTokenSource.Token);
            await application.Start(applicationOptions);
            Console.WriteLine("Application started...");

            Console.WriteLine("Starting node...");
            var nodeMetrics = new List<VersionBData.Metric>(VersionBMetrics);
            var node = new VersionB.SparkplugNode(nodeMetrics, Log.Logger);
            var nodeOptions = new SparkplugNodeOptions("localhost", 1883, "node 1", "user", "password", false, "scada1", "group1", "node1", TimeSpan.FromSeconds(30), null, null, CancellationTokenSource.Token);
            await node.Start(nodeOptions);
            Console.WriteLine("Node started...");

            Console.WriteLine("Starting device...");
            var deviceMetrics = new List<VersionBData.Metric>(VersionBMetrics);
            var device = new VersionB.SparkplugDevice(deviceMetrics, Log.Logger);
            var deviceOptions = new SparkplugDeviceOptions("localhost", 1883, "device 1", "user", "password", false, "scada1", "group1", "node1", "device1", TimeSpan.FromSeconds(30), null, null, CancellationTokenSource.Token);
            await device.Start(deviceOptions);
            Console.WriteLine("Device started...");
        }
    }
}
