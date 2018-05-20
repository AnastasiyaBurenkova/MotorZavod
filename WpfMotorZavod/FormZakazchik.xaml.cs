using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Unity;
using Unity.Attributes;

namespace WpfMotorZavod
{
    /// <summary>
    /// Логика взаимодействия для FormZakazchik.xaml
    /// </summary>
    public partial class FormZakazchik : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ID { set { id = value; } }

        private readonly IZakazchikService service;

        private int? id;

        public FormZakazchik(IZakazchikService service)
        {
            InitializeComponent();
            Loaded += FormZakazchik_Load;
            this.service = service;
        }

        private void FormZakazchik_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    ZakazchikViewModel view = service.GetElement(id.Value);
                    if (view != null)
                        textBoxFullName.Text = view.ZakazchikFIO;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFullName.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.UpdElement(new ZakazchikBindingModel
                    {
                        Id = id.Value,
                        ZakazchikFIO = textBoxFullName.Text
                    });
                }
                else
                {
                    service.AddElement(new ZakazchikBindingModel
                    {
                        ZakazchikFIO = textBoxFullName.Text
                    });
                }
                MessageBox.Show("Сохранение прошло успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
