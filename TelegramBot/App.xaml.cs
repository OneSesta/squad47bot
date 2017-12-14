﻿namespace TelegramBot
{
    using ApiKeyModule;
    using CommonServiceLocator;
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Unity;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using TelegramBot.Common;
    using TelegramBot.Views;
    using Unity;
    using Unity.ServiceLocation;

    public partial class App : PrismApplication
    {

        protected override Window CreateShell()
        {
            InitializeModules();
            return Container.Resolve<MainWindow>();
        }
        
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IBotApiKeyService, ApiKeyService>();
            return;
        }
        
        protected override IModuleCatalog CreateModuleCatalog()
        {
            var catalog = new DirectoryModuleCatalog() { ModulePath = @"./Modules" };
            catalog.Initialize();
            return catalog;
        }
    }
}