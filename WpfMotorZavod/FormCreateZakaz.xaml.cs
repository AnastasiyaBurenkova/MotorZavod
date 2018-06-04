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
    /// Логика взаимодействия для FormCreateZakaz.xaml
    /// </summary>
    public partial class FormCreateZakaz : Window
    {
        public FormCreateZakaz()
        {
            InitializeComponent();
            Loaded += FormCreateZakaz_Load;
            comboBoxDvigateli.SelectionChanged += comboBoxDvigateli_SelectedIndexChanged;
            comboBoxDvigateli.SelectionChanged += new SelectionChangedEventHandler(comboBoxDvigateli_SelectedIndexChanged);
        }

        private void FormCreateZakaz_Load(object sender, EventArgs e)
        {
            try
            {
                var responseC = APIClient.GetRequest("api/Zakazchik/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<ZakazchikViewModel> list = APIClient.GetElement<List<ZakazchikViewModel>>(responseC);
                    if (list != null)
                    {
                        comboBoxClient.DisplayMemberPath = "ZakazchikFIO";
                        comboBoxClient.SelectedValuePath = "Id";
                        comboBoxClient.ItemsSource = list;
                        comboBoxDvigateli.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
                var responseP = APIClient.GetRequest("api/Dvigateli/GetList");
                if (responseP.Result.IsSuccessStatusCode)
                {
                    List<DvigateliViewModel> list = APIClient.GetElement<List<DvigateliViewModel>>(responseP);
                    if (list != null)
                    {
                        comboBoxDvigateli.DisplayMemberPath = "DvigateliName";
                        comboBoxDvigateli.SelectedValuePath = "Id";
                        comboBoxDvigateli.ItemsSource = list;
                        comboBoxDvigateli.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseP));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalcSum()
        {
            if (comboBoxDvigateli.SelectedItem != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = ((DvigateliViewModel)comboBoxDvigateli.SelectedItem).Id;
                    var responseP = APIClient.GetRequest("api/Dvigateli/Get/" + id);
                    if (responseP.Result.IsSuccessStatusCode)
                    {
                        DvigateliViewModel Dvigateli = APIClient.GetElement<DvigateliViewModel>(responseP);
                        int count = Convert.ToInt32(textBoxCount.Text);
                        textBoxSum.Text = (count * (int)Dvigateli.Price).ToString();
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(responseP));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void textBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void comboBoxDvigateli_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxClient.SelectedItem == null)
            {
                MessageBox.Show("Выберите получателя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxDvigateli.SelectedItem == null)
            {
                MessageBox.Show("Выберите мебель", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Main/CreateZakaz", new ZakazBindingModel
                {
                    ZakazchikId = Convert.ToInt32(comboBoxClient.SelectedValue),
                    DvigateliId = Convert.ToInt32(comboBoxDvigateli.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToInt32(textBoxSum.Text)
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
