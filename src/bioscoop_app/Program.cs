using System;
using Chromely.Core;

namespace bioscoop_app
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create Chromely app
            AppBuilder
                .Create()
                .UseApp<ChromelyUi>()
                .Build()
                .Run(args);
        }
    }
}
