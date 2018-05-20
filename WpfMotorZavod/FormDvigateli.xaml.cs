using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
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
using Unity;
using Unity.Attributes;

namespace WpfMotorZavod
{
    /// <summary>
    /// Логика взаимодействия для FormDvigateli.xaml
    /// </summary>
    public partial class FormDvigateli : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ID { set { id = value; } }

        private readonly IDvigateliService service;

        private int? id;

        private List<DvigateliDetaliViewModel> productDetalis;

        public FormDvigateli(IDvigateliService service)
        {
            InitializeComponent();
            Loaded += FormDvigateli_Load;
            this.service = service;
        }

        private void FormDvigateli_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    DvigateliViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.DvigateliName;
                        textBoxPrice.Text = view.Price.ToString();
                        productDetalis = view.DvigateliDetalis;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                productDetalis = new List<DvigateliDetaliViewModel>();
        }

        private void LoadData()
        {
            try
            {
                if (productDetalis != null)
                {
                    dataGridViewProduct.ItemsSource = null;
                    dataGridViewProduct.ItemsSource = productDetalis;
                    dataGridViewProduct.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewProduct.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewProduct.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewProduct.Columns[3].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormDvigateliDetali>();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                        form.Model.DvigateliId = id.Value;
                    productDetalis.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewProduct.SelectedItem != null)
            {
                var form = Container.Resolve<FormDvigateliDetali>();
                form.Model = productDetalis[dataGridViewProduct.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    productDetalis[dataGridViewProduct.SelectedIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewProduct.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        productDetalis.RemoveAt(dataGridViewProduct.SelectedIndex);
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
            if (productDetalis == null || productDetalis.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                List<DvigateliDetaliBindingModel> productDetaliBM = new List<DvigateliDetaliBindingModel>();
                for (int i = 0; i < productDetalis.Count; ++i)
                {
                    productDetaliBM.Add(new DvigateliDetaliBindingModel
                    {
                        Id = productDetalis[i].Id,
                        DvigateliId = productDetalis[i].DvigateliId,
                        DetaliId = productDetalis[i].DetaliId,
                        Count = productDetalis[i].Count
                    });
                }
                if (id.HasValue)
                {
                    service.UpdElement(new DvigateliBindingModel
                    {
                        Id = id.Value,
                        DvigateliName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        DvigateliDetalis = productDetaliBM
                    });
                }
                else
                {
                    service.AddElement(new DvigateliBindingModel
                    {
                        DvigateliName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        DvigateliDetalis = productDetaliBM
                    });
                }
                MessageBox.Show("Сохранение прошло успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
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
