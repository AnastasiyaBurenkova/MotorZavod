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
using Unity;
using Unity.Attributes;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using AbstractShopService.BindingModels;

namespace WpfMotorZavod
{
    /// <summary>
    /// Логика взаимодействия для FormCreateZakaz.xaml
    /// </summary>
    public partial class FormCreateZakaz : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IZakazchikService serviceClient;

        private readonly IDvigateliService serviceProduct;

        private readonly IMainService serviceMain;


        public FormCreateZakaz(IZakazchikService serviceC, IDvigateliService serviceP, IMainService serviceM)
        {
            InitializeComponent();
            Loaded += FormCreateZakaz_Load;
            comboBoxProduct.SelectionChanged += comboBoxProduct_SelectedIndexChanged;

            comboBoxProduct.SelectionChanged += new SelectionChangedEventHandler(comboBoxProduct_SelectedIndexChanged);
            this.serviceClient = serviceC;
            this.serviceProduct = serviceP;
            this.serviceMain = serviceM;
        }

        private void FormCreateZakaz_Load(object sender, EventArgs e)
        {
            try
            {
                List<ZakazchikViewModel> listClient = serviceClient.GetList();
                if (listClient != null)
                {
                    comboBoxClient.DisplayMemberPath = "ZakazchikFIO";
                    comboBoxClient.SelectedValuePath = "Id";
                    comboBoxClient.ItemsSource = listClient;
                    comboBoxProduct.SelectedItem = null;
                }
                List<DvigateliViewModel> listProduct = serviceProduct.GetList();
                if (listProduct != null)
                {
                    comboBoxProduct.DisplayMemberPath = "DvigateliName";
                    comboBoxProduct.SelectedValuePath = "Id";
                    comboBoxProduct.ItemsSource = listProduct;
                    comboBoxProduct.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalcSum()
        {
            if (comboBoxProduct.SelectedItem != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = ((DvigateliViewModel)comboBoxProduct.SelectedItem).Id;
                    DvigateliViewModel product = serviceProduct.GetElement(id);
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = (count * product.Price).ToString();
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

        private void comboBoxProduct_SelectedIndexChanged(object sender, EventArgs e)
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
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxProduct.SelectedItem == null)
            {
                MessageBox.Show("Выберите изделие", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceMain.CreateZakaz(new ZakazBindingModel
                {
                    ZakazchikId = ((ZakazchikViewModel)comboBoxClient.SelectedItem).Id,
                    DvigateliId = ((DvigateliViewModel)comboBoxProduct.SelectedItem).Id,
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToInt32(textBoxSum.Text)
                });
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
