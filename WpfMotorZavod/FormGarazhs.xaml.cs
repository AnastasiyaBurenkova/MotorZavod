using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для Garazhs.xaml
    /// </summary>
    public partial class FormGarazhs : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IGarazhService service;

        public FormGarazhs(IGarazhService service)
        {
            InitializeComponent();
            Loaded += FormGarazhs_Load;
            this.service = service;
        }

        private void FormGarazhs_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<GarazhViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewGarazhs.ItemsSource = list;
                    dataGridViewGarazhs.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewGarazhs.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormGarazh>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewGarazhs.SelectedItem != null)
            {
                var form = Container.Resolve<FormGarazh>();
                form.ID = ((GarazhViewModel)dataGridViewGarazhs.SelectedItem).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewGarazhs.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((GarazhViewModel)dataGridViewGarazhs.SelectedItem).Id;
                    try
                    {
                        service.DelElement(id);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
