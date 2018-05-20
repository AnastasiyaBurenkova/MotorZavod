using AbstractShopService.ImplementationsList;
using AbstractShopService.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Lifetime;

namespace WpfMotorZavod
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        public static void Main()
        {
            var container = BuildUnityContainer();

            var application = new App();
            application.Run(container.Resolve<FormMain>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<IZakazchikService, ZakazchikServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IDetaliService, DetaliServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IRabochiService, RabochiServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IDvigateliService, DvigateliServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IGarazhService, GarazhServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainService, MainServiceList>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}
