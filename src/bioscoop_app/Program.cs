using System;
using System.IO;
using System.Linq;
using bioscoop_app.Model;
using Chromely.Core;

namespace bioscoop_app
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())).Replace(@"\", @"\\"));
            // Create Chromely app
            AppBuilder
                .Create()
                .UseApp<ChromelyUi>()
                .Build()
                .Run(args);
            
        }
    }
}
