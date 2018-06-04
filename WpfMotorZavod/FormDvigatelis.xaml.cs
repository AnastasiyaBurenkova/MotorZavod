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
    /// Логика взаимодействия для FormDvigatelis.xaml
    /// </summary>
    public partial class FormDvigatelis : Window
    {

        public FormDvigatelis()
        {
            InitializeComponent();
            Loaded += FormDvigatelis_Load;
        }

        private void FormDvigatelis_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<DvigateliViewModel> list = Task.Run(() => APIClient.GetRequestData<List<DvigateliViewModel>>("api/Dvigateli/GetList")).Result;
                if (list != null)
                {
                    dataGridViewDvigatelis.ItemsSource = list;
                    dataGridViewDvigatelis.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewDvigatelis.Columns[1].Width = DataGridLength.Auto;
                    dataGridViewDvigatelis.Columns[3].Visibility = Visibility.Hidden;
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

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormDvigateli();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewDvigatelis.SelectedItem != null)
            {
                var form = new FormDvigateli();
                form.Id = ((DvigateliViewModel)dataGridViewDvigatelis.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewDvigatelis.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    int id = ((DvigateliViewModel)dataGridViewDvigatelis.SelectedItem).Id;

                    Task task = Task.Run(() => APIClient.PostRequestData("api/Dvigateli/DelElement", new ZakazchikBindingModel { Id = id }));

                    task.ContinueWith((prevTask) => MessageBox.Show("Запись удалена. Обновите список", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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
                }
            }
        }
        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}