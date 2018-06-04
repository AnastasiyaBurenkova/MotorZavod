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
                    var Garazh = Task.Run(() => APIClient.GetRequestData<GarazhViewModel>("api/Garazh/Get/" + id.Value)).Result;
                    textBoxName.Text = Garazh.GarazhName;
                    dataGridViewGarazh.ItemsSource = Garazh.GarazhDetalis;
                    dataGridViewGarazh.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewGarazh.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewGarazh.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewGarazh.Columns[3].Width = DataGridLength.Auto;

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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string name = textBoxName.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Garazh/UpdElement", new GarazhBindingModel
                {
                    Id = id.Value,
                    GarazhName = name
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Garazh/AddElement", new GarazhBindingModel
                {
                    GarazhName = name
                }));
            }

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