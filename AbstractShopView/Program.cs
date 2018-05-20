using AbstractShopService.ImplementationsList;
using AbstractShopService.Interfaces;
using System;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;
using AbstractShopService;
using AbstractShopService.ImplementationsBD;
using AbstractShopService.ImplementationsList;
using System.Data.Entity;

namespace AbstractShopView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormMain>());
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
