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
    /// Логика взаимодействия для FormDvigatelis.xaml
    /// </summary>
    public partial class FormDvigatelis : Window
    {

        public FormDvigatelis()
        {
            InitializeComponent();
            Loaded += FormDvigatelis_Load;
        }

        private void FormDvigatelis_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Dvigateli/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<DvigateliViewModel> list = APIClient.GetElement<List<DvigateliViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewDvigatelis.ItemsSource = list;
                        dataGridViewDvigatelis.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewDvigatelis.Columns[1].Width = DataGridLength.Auto;
                        dataGridViewDvigatelis.Columns[3].Visibility = Visibility.Hidden;
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
            var form = new FormDvigateli();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewDvigatelis.SelectedItem != null)
            {
                var form = new FormDvigateli();
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
                        var response = APIClient.PostRequest("api/Dvigateli/DelElement", new ZakazchikBindingModel { Id = id });
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