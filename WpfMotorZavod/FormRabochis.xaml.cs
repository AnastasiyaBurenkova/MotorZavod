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
    /// Логика взаимодействия для FormRabochis.xaml
    /// </summary>
    public partial class FormRabochis : Window
    {

        public FormRabochis()
        {
            InitializeComponent();
            Loaded += FormRabochis_Load;
        }

        private void FormRabochis_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Rabochi/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<RabochiViewModel> list = APIClient.GetElement<List<RabochiViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewRabochis.ItemsSource = list;
                        dataGridViewRabochis.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewRabochis.Columns[1].Width = DataGridLength.Auto;
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
            var form = new FormRabochi();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewRabochis.SelectedItem != null)
            {
                var form = new FormRabochi();
                form.Id = ((RabochiViewModel)dataGridViewRabochis.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewRabochis.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((RabochiViewModel)dataGridViewRabochis.SelectedItem).Id;
                    try
                    {
                        var response = APIClient.PostRequest("api/Rabochi/DelElement", new ZakazchikBindingModel { Id = id });
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