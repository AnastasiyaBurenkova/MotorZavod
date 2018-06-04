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
    /// Логика взаимодействия для FormPutOnGarazh.xaml
    /// </summary>
    public partial class FormPutOnGarazh : Window
    {
        public FormPutOnGarazh()
        {
            InitializeComponent();
            Loaded += FormPutOnGarazh_Load;
        }

        private void FormPutOnGarazh_Load(object sender, EventArgs e)
        {
            try
            {
                var responseC = APIClient.GetRequest("api/Detali/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<DetaliViewModel> list = APIClient.GetElement<List<DetaliViewModel>>(responseC);
                    if (list != null)
                    {
                        comboBoxDetali.DisplayMemberPath = "DetaliName";
                        comboBoxDetali.SelectedValuePath = "Id";
                        comboBoxDetali.ItemsSource = list;
                        comboBoxDetali.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
                var responseS = APIClient.GetRequest("api/Garazh/GetList");
                if (responseS.Result.IsSuccessStatusCode)
                {
                    List<GarazhViewModel> list = APIClient.GetElement<List<GarazhViewModel>>(responseS);
                    if (list != null)
                    {
                        comboBoxGarazh.DisplayMemberPath = "GarazhName";
                        comboBoxGarazh.SelectedValuePath = "Id";
                        comboBoxGarazh.ItemsSource = list;
                        comboBoxGarazh.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxDetali.SelectedItem == null)
            {
                MessageBox.Show("Выберите заготовку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxGarazh.SelectedItem == null)
            {
                MessageBox.Show("Выберите базу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Main/PutDetaliOnGarazh", new GarazhDetaliBindingModel
                {
                    DetaliId = Convert.ToInt32(comboBoxDetali.SelectedValue),
                    GarazhId = Convert.ToInt32(comboBoxGarazh.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
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