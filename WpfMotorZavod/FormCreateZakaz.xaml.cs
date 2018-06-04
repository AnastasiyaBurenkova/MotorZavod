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
                List<ZakazchikViewModel> listC = Task.Run(() => APIClient.GetRequestData<List<ZakazchikViewModel>>("api/Zakazchik/GetList")).Result;
                if (listC != null)
                {
                    comboBoxClient.DisplayMemberPath = "ZakazchikFIO";
                    comboBoxClient.SelectedValuePath = "Id";
                    comboBoxClient.ItemsSource = listC;
                    comboBoxDvigateli.SelectedItem = null;
                }

                List<DvigateliViewModel> listP = Task.Run(() => APIClient.GetRequestData<List<DvigateliViewModel>>("api/Dvigateli/GetList")).Result;
                if (listP != null)
                {
                    comboBoxDvigateli.DisplayMemberPath = "DvigateliName";
                    comboBoxDvigateli.SelectedValuePath = "Id";
                    comboBoxDvigateli.ItemsSource = listP;
                    comboBoxDvigateli.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
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
                    DvigateliViewModel product = Task.Run(() => APIClient.GetRequestData<DvigateliViewModel>("api/Dvigateli/Get/" + id)).Result;
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = (count * (int)product.Price).ToString();
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
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
            int ZakazchikId = Convert.ToInt32(comboBoxClient.SelectedValue);
            int DvigateliId = Convert.ToInt32(comboBoxDvigateli.SelectedValue);
            int count = Convert.ToInt32(textBoxCount.Text);
            int sum = Convert.ToInt32(textBoxSum.Text);
            Task task = Task.Run(() => APIClient.PostRequestData("api/Main/CreateZakaz", new ZakazBindingModel
            {
                ZakazchikId = ZakazchikId,
                DvigateliId = DvigateliId,
                Count = count,
                Sum = sum
            }));

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}