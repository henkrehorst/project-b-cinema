﻿using System;
using bioscoop_app.Controller;
using Chromely;
using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Network;

namespace bioscoop_app
{
    public class ChromelyUi : ChromelyBasicApp
    {
        public override void Configure(IChromelyContainer container)
        {
            base.Configure(container);
            container.RegisterSingleton(typeof(ChromelyController), Guid.NewGuid().ToString(), typeof(DemoController));
            container.RegisterSingleton(typeof(ChromelyController), Guid.NewGuid().ToString(), typeof(MovieController));
        }
    }
}