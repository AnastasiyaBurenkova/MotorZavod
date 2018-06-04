using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfMotorZavod;

namespace MebelFactoryViewWpf
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

            APIClient.Connect();
            var application = new App();
            //application.InitializeComponent();
            application.Run(new FormMain());
            //App app = new App();
            //app.Run(container.Resolve<FormMain>());
        }


    }
}