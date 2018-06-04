using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AbstractShopService;
using AbstractShopService.ImplementationsBD;
using AbstractShopService.Interfaces;
using Unity;
using Unity.Lifetime;

namespace WpfMotorZavod
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        /*App()
            {
                InitializeComponent();
            }*/

        [STAThread]
        public static void Main()
        {
            var container = BuildUnityContainer();

            var application = new App();
            //application.InitializeComponent();
            application.Run(container.Resolve<FormMain>());
            //App app = new App();
            //app.Run(container.Resolve<FormMain>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<DbContext, AbstractDbContext>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IZakazchikService, ZakazchikServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IDetaliService, DetaliServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IRabochiService, RabochiServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IDvigateliService, DvigateliServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IGarazhService, GarazhServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainService, MainServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IReportService, ReportServiceBD>(new HierarchicalLifetimeManager());
            return currentContainer;
        }
    }
}