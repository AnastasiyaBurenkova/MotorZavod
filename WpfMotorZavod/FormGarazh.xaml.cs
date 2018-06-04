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
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using Unity;
using Unity.Attributes;
namespace WpfMotorZavod
{
    /// <summary>
    /// Логика взаимодействия для FormGarazh.xaml
    /// </summary>
    public partial class FormGarazh : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IGarazhService service;

        private int? id;

        public FormGarazh(IGarazhService service)
        {
            InitializeComponent();
            Loaded += FormGarazh_Load;
            this.service = service;
        }

        private void FormGarazh_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    GarazhViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.GarazhName;
                        dataGridViewGarazh.ItemsSource = view.GarazhDetalis;
                        dataGridViewGarazh.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewGarazh.Columns[1].Visibility = Visibility.Hidden;
                        dataGridViewGarazh.Columns[2].Visibility = Visibility.Hidden;
                        dataGridViewGarazh.Columns[3].Width = DataGridLength.Auto;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.UpdElement(new GarazhBindingModel
                    {
                        Id = id.Value,
                        GarazhName = textBoxName.Text
                    });
                }
                else
                {
                    service.AddElement(new GarazhBindingModel
                    {
                        GarazhName = textBoxName.Text
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
