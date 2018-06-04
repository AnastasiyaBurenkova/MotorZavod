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
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
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

        public int Id { set { id = value; } }

        private readonly IDvigateliService service;

        private int? id;

        private List<DvigateliDetaliViewModel> DvigateliDetalis;

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
                        DvigateliDetalis = view.DvigateliDetalis;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                DvigateliDetalis = new List<DvigateliDetaliViewModel>();
        }

        private void LoadData()
        {
            try
            {
                if (DvigateliDetalis != null)
                {
                    dataGridViewDvigateli.ItemsSource = null;
                    dataGridViewDvigateli.ItemsSource = DvigateliDetalis;
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
            var form = Container.Resolve<FormDvigateliDetali>();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                        form.Model.DvigateliId = id.Value;
                    DvigateliDetalis.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewDvigateli.SelectedItem != null)
            {
                var form = Container.Resolve<FormDvigateliDetali>();
                form.Model = DvigateliDetalis[dataGridViewDvigateli.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    DvigateliDetalis[dataGridViewDvigateli.SelectedIndex] = form.Model;
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
                        DvigateliDetalis.RemoveAt(dataGridViewDvigateli.SelectedIndex);
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
            if (DvigateliDetalis == null || DvigateliDetalis.Count == 0)
            {
                MessageBox.Show("Заполните заготовки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                List<DvigateliDetaliBindingModel> productComponentBM = new List<DvigateliDetaliBindingModel>();
                for (int i = 0; i < DvigateliDetalis.Count; ++i)
                {
                    productComponentBM.Add(new DvigateliDetaliBindingModel
                    {
                        Id = DvigateliDetalis[i].Id,
                        DvigateliId = DvigateliDetalis[i].DvigateliId,
                        DetaliId = DvigateliDetalis[i].DetaliId,
                        Count = DvigateliDetalis[i].Count
                    });
                }
                if (id.HasValue)
                {
                    service.UpdElement(new DvigateliBindingModel
                    {
                        Id = id.Value,
                        DvigateliName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        DvigateliDetalis = productComponentBM
                    });
                }
                else
                {
                    service.AddElement(new DvigateliBindingModel
                    {
                        DvigateliName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        DvigateliDetalis = productComponentBM
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
