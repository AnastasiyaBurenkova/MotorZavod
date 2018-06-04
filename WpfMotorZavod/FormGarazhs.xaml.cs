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
    /// Логика взаимодействия для FormGarazhs.xaml
    /// </summary>
    public partial class FormGarazhs : Window
    {

        public FormGarazhs()
        {
            InitializeComponent();
            Loaded += FormGarazhs_Load;
        }

        private void FormGarazhs_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Garazh/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<GarazhViewModel> list = APIClient.GetElement<List<GarazhViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewGarazhs.ItemsSource = list;
                        dataGridViewGarazhs.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewGarazhs.Columns[1].Width = DataGridLength.Auto;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormGarazh();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewGarazhs.SelectedItem != null)
            {
                var form = new FormGarazh();
                form.Id = ((GarazhViewModel)dataGridViewGarazhs.SelectedItem).Id;
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
                        var response = APIClient.PostRequest("api/Garazh/DelElement", new ZakazchikBindingModel { Id = id });
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