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
    /// Логика взаимодействия для FormDvigateli.xaml
    /// </summary>
    public partial class FormDvigateli : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        private List<DvigateliDetaliViewModel> DetaliDvigatelis;

        public FormDvigateli()
        {
            InitializeComponent();
            Loaded += FormDvigateli_Load;
        }

        private void FormDvigateli_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var Dvigateli = Task.Run(() => APIClient.GetRequestData<DvigateliViewModel>("api/Dvigateli/Get/" + id.Value)).Result;
                    textBoxName.Text = Dvigateli.DvigateliName;
                    textBoxPrice.Text = Dvigateli.Price.ToString();
                    DetaliDvigatelis = Dvigateli.DvigateliDetalis;
                    LoadData();
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
            else
                DetaliDvigatelis = new List<DvigateliDetaliViewModel>();
        }

        private void LoadData()
        {
            try
            {
                if (DetaliDvigatelis != null)
                {
                    dataGridViewDvigateli.ItemsSource = null;
                    dataGridViewDvigateli.ItemsSource = DetaliDvigatelis;
                    dataGridViewDvigateli.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewDvigateli.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewDvigateli.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewDvigateli.Columns[3].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormDvigateliDetali();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                        form.Model.DvigateliId = id.Value;
                    DetaliDvigatelis.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewDvigateli.SelectedItem != null)
            {
                var form = new FormDvigateliDetali();
                form.Model = DetaliDvigatelis[dataGridViewDvigateli.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    DetaliDvigatelis[dataGridViewDvigateli.SelectedIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewDvigateli.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        DetaliDvigatelis.RemoveAt(dataGridViewDvigateli.SelectedIndex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (DetaliDvigatelis == null || DetaliDvigatelis.Count == 0)
            {
                MessageBox.Show("Заполните заготовки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<DvigateliDetaliBindingModel> DetaliDvigateliBM = new List<DvigateliDetaliBindingModel>();
            for (int i = 0; i < DetaliDvigatelis.Count; ++i)
            {
                DetaliDvigateliBM.Add(new DvigateliDetaliBindingModel
                {
                    Id = DetaliDvigatelis[i].Id,
                    DvigateliId = DetaliDvigatelis[i].DvigateliId,
                    DetaliId = DetaliDvigatelis[i].DetaliId,
                    Count = DetaliDvigatelis[i].Count
                });
            }
            string name = textBoxName.Text;
            int price = Convert.ToInt32(textBoxPrice.Text);
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Dvigateli/UpdElement", new DvigateliBindingModel
                {
                    Id = id.Value,
                    DvigateliName = name,
                    Price = price,
                    DvigateliDetalis = DetaliDvigateliBM
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Dvigateli/AddElement", new DvigateliBindingModel
                {
                    DvigateliName = name,
                    Price = price,
                    DvigateliDetalis = DetaliDvigateliBM
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