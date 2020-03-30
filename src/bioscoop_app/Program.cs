using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using bioscoop_app.Model;
using bioscoop_app.Service;
using Chromely.Core;
using Newtonsoft.Json;

namespace bioscoop_app
{
    class Program
    {
        static void Main(string[] args)
        {
            StorageService.SetupStorageFiles();
            Console.WriteLine(StorageService.GetDataSourcePath());
            
            var directoryInfo = Directory.GetParent(Environment.CurrentDirectory).Parent;
            if (directoryInfo != null)
            {
                string projectDirectory = directoryInfo.Name;
                Console.WriteLine(projectDirectory);
            }

            Console.WriteLine(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())).Replace(@"\", @"\\"));
            // // Create Chromely app
            // AppBuilder
            //     .Create()
            //     .UseApp<ChromelyUi>()
            //     .Build()
            //     .Run(args);
            
        }
    }
}
