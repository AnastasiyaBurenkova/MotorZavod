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
    /// Логика взаимодействия для FormRabochi.xaml
    /// </summary>
    public partial class FormRabochi : Window
    {

        public int Id { set { id = value; } }

        private int? id;

        public FormRabochi()
        {
            InitializeComponent();
            Loaded += FormRabochi_Load;
        }

        private void FormRabochi_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Rabochi/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var Rabochi = APIClient.GetElement<RabochiViewModel>(response);
                        textBoxFullName.Text = Rabochi.RabochiFIO;
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
            if (string.IsNullOrEmpty(textBoxFullName.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Rabochi/UpdElement", new RabochiBindingModel
                    {
                        Id = id.Value,
                        RabochiFIO = textBoxFullName.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Rabochi/AddElement", new RabochiBindingModel
                    {
                        RabochiFIO = textBoxFullName.Text
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