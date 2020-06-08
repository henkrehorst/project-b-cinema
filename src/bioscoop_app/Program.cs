using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using bioscoop_app.Model;
using bioscoop_app.Repository;
using bioscoop_app.Service;
using Chromely.Core;
using FluentValidation;
using Newtonsoft.Json;

namespace bioscoop_app
{
    class Program
    {
        static void Main(string[] args)
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
            //setup storages files modules
            StorageService.SetupStorageFiles();

            // Create Chromely app
            AppBuilder
                .Create()
                .UseApp<ChromelyUi>()
                .Build()
                .Run(args);
            
        }
    }
}
