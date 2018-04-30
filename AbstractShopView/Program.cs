using AbstractShopService.ImplementationsList;
using AbstractShopService.Interfaces;
using System;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

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
