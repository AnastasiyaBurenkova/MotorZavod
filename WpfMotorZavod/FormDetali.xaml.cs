using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
using AbstractShopService.ViewModels;
using AbstractShopService.BindingModels;

namespace WpfMotorZavod
{
    /// <summary>
    /// Логика взаимодействия для FormDetali.xaml
    /// </summary>
    public partial class FormDetali : Window
    {

        public int Id { set { id = value; } }

        private int? id;

        public FormDetali()
        {
            InitializeComponent();
            Loaded += FormDetali_Load;
        }

        private void FormDetali_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Detali/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var Detali = APIClient.GetElement<DetaliViewModel>(response);
                        textBoxName.Text = Detali.DetaliName;
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
                    response = APIClient.PostRequest("api/Detali/UpdElement", new DetaliBindingModel
                    {
                        Id = id.Value,
                        DetaliName = textBoxName.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Detali/AddElement", new DetaliBindingModel
                    {
                        DetaliName = textBoxName.Text
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