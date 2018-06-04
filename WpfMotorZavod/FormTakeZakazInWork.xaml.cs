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
                List<RabochiViewModel> list = Task.Run(() => APIClient.GetRequestData<List<RabochiViewModel>>("api/Rabochi/GetList")).Result;
                if (list != null)
                {
                    comboBoxRabochi.DisplayMemberPath = "RabochiFIO";
                    comboBoxRabochi.SelectedValuePath = "Id";
                    comboBoxRabochi.ItemsSource = list;
                    comboBoxRabochi.SelectedItem = null;
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxRabochi.SelectedItem == null)
            {
                MessageBox.Show("Выберите рабочего", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                int RabochiId = Convert.ToInt32(comboBoxRabochi.SelectedValue);
                Task task = Task.Run(() => APIClient.PostRequestData("api/Main/TakeZakazInWork", new ZakazBindingModel
                {
                    Id = id.Value,
                    RabochiId = RabochiId
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Заявка передана в работу. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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