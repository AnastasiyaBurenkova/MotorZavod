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
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using Unity;
using Unity.Attributes;
namespace WpfMotorZavod
{
    /// <summary>
    /// Логика взаимодействия для FormDvigatelis.xaml
    /// </summary>
    public partial class FormDvigatelis : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IDvigateliService service;

        public FormDvigatelis(IDvigateliService service)
        {
            InitializeComponent();
            Loaded += FormDvigatelis_Load;
            this.service = service;
        }

        private void FormDvigatelis_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<DvigateliViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewDvigatelis.ItemsSource = list;
                    dataGridViewDvigatelis.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewDvigatelis.Columns[1].Width = DataGridLength.Auto;
                    dataGridViewDvigatelis.Columns[3].Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormDvigateli>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewDvigatelis.SelectedItem != null)
            {
                var form = Container.Resolve<FormDvigateli>();
                form.Id = ((DvigateliViewModel)dataGridViewDvigatelis.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewDvigatelis.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    int id = ((DvigateliViewModel)dataGridViewDvigatelis.SelectedItem).Id;
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
