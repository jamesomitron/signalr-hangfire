﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Abstractions;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace SampleService
{
    public partial class Service1 : ServiceBase
    {
        /// <summary>
        /// Instance of the OWIN server
        /// </summary>
        private IDisposable _appServer;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            StartOptions startOptions = new StartOptions();
            string webAppPort = ConfigurationManager.Instance.AppSettings.AppSetting<string>("Hangfire.WebAppPort", () => "9095");
            startOptions.Urls.Add(string.Format("http://localhost:{0}", webAppPort));
            startOptions.Urls.Add(string.Format("http://127.0.0.1:{0}", webAppPort));
            startOptions.Urls.Add(string.Format("http://{0}:{1}", Environment.MachineName, webAppPort));

            _appServer = WebApp.Start<Startup>(startOptions);
        }

        protected override void OnStop()
        {
            if (_appServer != null) { _appServer.Dispose(); }
        }
    }
}
