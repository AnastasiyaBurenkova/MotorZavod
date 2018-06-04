using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Логика взаимодействия для FormGarazh.xaml
    /// </summary>
    public partial class FormGarazh : Window
    {

        public int Id { set { id = value; } }

        private int? id;

        public FormGarazh()
        {
            InitializeComponent();
            Loaded += FormGarazh_Load;

        }

        private void FormGarazh_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Garazh/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var Garazh = APIClient.GetElement<GarazhViewModel>(response);
                        textBoxName.Text = Garazh.GarazhName;
                        dataGridViewGarazh.ItemsSource = Garazh.GarazhDetalis;
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
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Garazh/UpdElement", new GarazhBindingModel
                    {
                        Id = id.Value,
                        GarazhName = textBoxName.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Garazh/AddElement", new GarazhBindingModel
                    {
                        GarazhName = textBoxName.Text
                    });
                }
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}