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
    /// Логика взаимодействия для FormTakeZakazInWork.xaml
    /// </summary>
    public partial class FormTakeZakazInWork : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormTakeZakazInWork()
        {
            InitializeComponent();
            Loaded += FormTakeZakazInWork_Load;
        }

        private void FormTakeZakazInWork_Load(object sender, EventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указана заявка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                var response = APIClient.GetRequest("api/Rabochi/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<RabochiViewModel> list = APIClient.GetElement<List<RabochiViewModel>>(response);
                    if (list != null)
                    {
                        comboBoxRabochi.DisplayMemberPath = "RabochiFIO";
                        comboBoxRabochi.SelectedValuePath = "Id";
                        comboBoxRabochi.ItemsSource = list;
                        comboBoxRabochi.SelectedItem = null;

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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxRabochi.SelectedItem == null)
            {
                MessageBox.Show("Выберите рабочего", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Main/TakeZakazInWork", new ZakazBindingModel
                {
                    Id = id.Value,
                    RabochiId = ((RabochiViewModel)comboBoxRabochi.SelectedItem).Id,
                });
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