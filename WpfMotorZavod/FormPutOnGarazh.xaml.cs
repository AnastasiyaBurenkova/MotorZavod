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
                List<DetaliViewModel> listC = Task.Run(() => APIClient.GetRequestData<List<DetaliViewModel>>("api/Detali/GetList")).Result;
                if (listC != null)
                {
                    comboBoxDetali.DisplayMemberPath = "DetaliName";
                    comboBoxDetali.SelectedValuePath = "Id";
                    comboBoxDetali.ItemsSource = listC;
                    comboBoxDetali.SelectedItem = null;
                }
                List<GarazhViewModel> listS = Task.Run(() => APIClient.GetRequestData<List<GarazhViewModel>>("api/Garazh/GetList")).Result;
                if (listS != null)
                {
                    comboBoxGarazh.DisplayMemberPath = "GarazhName";
                    comboBoxGarazh.SelectedValuePath = "Id";
                    comboBoxGarazh.ItemsSource = listS;
                    comboBoxGarazh.SelectedItem = null;
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
                int DetaliId = Convert.ToInt32(comboBoxDetali.SelectedValue);
                int GarazhId = Convert.ToInt32(comboBoxGarazh.SelectedValue);
                int count = Convert.ToInt32(textBoxCount.Text);
                Task task = Task.Run(() => APIClient.PostRequestData("api/Main/PutDetaliOnGarazh", new GarazhDetaliBindingModel
                {
                    DetaliId = DetaliId,
                    GarazhId = GarazhId,
                    Count = count
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("База пополнен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
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