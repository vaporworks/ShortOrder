using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism.Events;
using shortorder.wpf.cookui.Factory;
using shortorder.wpf.cookui.ViewModel;
using shortorder.wpf.cookui.ViewModel.Impl;
using Symbiote.Core;
using Symbiote.Rabbit;

namespace shortorder.wpf.cookui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            Assimilate
                .Initialize()
                .Rabbit(x => x.AddBroker( s => s.Defaults()
                                                .Address( ConfigurationManager.AppSettings["RabbitHost"]) )
                                                .EnrollAsMeshNode( true ))
                .Dependencies( x =>
                                   {
                                       x.For<IEventAggregator>().Use<EventAggregator>().AsSingleton();
                                       x.For<IOrderCollectionViewModel>().Use<OrderCollectionViewModel>();
                                       x.For<IOrderItemViewModel>().Use<OrderItemViewModel>();
                                       x.For<IOrderViewModel>().Use<OrderViewModel>();
                                       x.For<IOrderViewModelFactory>().Use<OrderViewModelFactory>();
                                   } );

            var mainWindow = new MainWindow();
            var viewModel = Assimilate.GetInstanceOf<IOrderCollectionViewModel>();
            mainWindow.DataContext = viewModel;
            viewModel.RequestClose += (sender,args) => mainWindow.Close();
            viewModel.Dispatcher = mainWindow.Dispatcher;
            mainWindow.Show();
        }
    }
}
