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
    /// Логика взаимодействия для FormDetalis.xaml
    /// </summary>
    public partial class FormDetalis : Window
    {

        public FormDetalis()
        {
            InitializeComponent();
            Loaded += FormDetalis_Load;
        }

        private void FormDetalis_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Detali/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<DetaliViewModel> list = APIClient.GetElement<List<DetaliViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewDetalis.ItemsSource = list;
                        dataGridViewDetalis.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewDetalis.Columns[1].Width = DataGridLength.Auto;
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
            var form = new FormDetali();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewDetalis.SelectedItem != null)
            {
                var form = new FormDetali();
                form.Id = ((DetaliViewModel)dataGridViewDetalis.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewDetalis.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((DetaliViewModel)dataGridViewDetalis.SelectedItem).Id;
                    try
                    {
                        var response = APIClient.PostRequest("api/Detali/DelElement", new ZakazchikBindingModel { Id = id });
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