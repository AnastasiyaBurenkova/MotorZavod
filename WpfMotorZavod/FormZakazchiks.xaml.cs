using AbstractShopService.BindingModels;
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
namespace WpfMotorZavod
{
    /// <summary>
    /// Логика взаимодействия для FormZakazchiks.xaml
    /// </summary>
    public partial class FormZakazchiks : Window
    {

        public FormZakazchiks()
        {
            InitializeComponent();
            Loaded += FormZakazchiks_Load;
        }

        private void FormZakazchiks_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Zakazchik/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<ZakazchikViewModel> list = APIClient.GetElement<List<ZakazchikViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewZakazchiks.ItemsSource = list;
                        dataGridViewZakazchiks.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewZakazchiks.Columns[1].Width = DataGridLength.Auto;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormZakazchik();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewZakazchiks.SelectedItem != null)
            {
                var form = new FormZakazchik();
                form.Id = ((ZakazchikViewModel)dataGridViewZakazchiks.SelectedItem).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewZakazchiks.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((ZakazchikViewModel)dataGridViewZakazchiks.SelectedItem).Id;
                    try
                    {
                        var response = APIClient.PostRequest("api/Zakazchik/DelElement", new ZakazchikBindingModel { Id = id });
                        if (!response.Result.IsSuccessStatusCode)
                        {
                            throw new Exception(APIClient.GetError(response));
                        }
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