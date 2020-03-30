using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using bioscoop_app.Service;
using Chromely.Core;
using Newtonsoft.Json;

namespace bioscoop_app
{
    class Program
    {
        static void Main(string[] args)
        {
            //setup storages files modules
            StorageService.SetupStorageFiles();
            MovieRepository movieRepository = new MovieRepository();
            
            // Create Chromely app
            AppBuilder
                .Create()
                .UseApp<ChromelyUi>()
                .Build()
                .Run(args);
            
        }
    }
}
