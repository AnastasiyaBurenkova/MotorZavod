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
    /// Логика взаимодействия для FormCreateZakaz.xaml
    /// </summary>
    public partial class FormCreateZakaz : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IZakazchikService serviceP;

        private readonly IDvigateliService serviceM;

        private readonly IMainService serviceG;


        public FormCreateZakaz(IZakazchikService serviceP, IDvigateliService serviceM, IMainService serviceG)
        {
            InitializeComponent();
            Loaded += FormCreateZakaz_Load;
            comboBoxDvigateli.SelectionChanged += comboBoxDvigateli_SelectedIndexChanged;

            comboBoxDvigateli.SelectionChanged += new SelectionChangedEventHandler(comboBoxDvigateli_SelectedIndexChanged);
            this.serviceP = serviceP;
            this.serviceM = serviceM;
            this.serviceG = serviceG;
        }

        private void FormCreateZakaz_Load(object sender, EventArgs e)
        {
            try
            {
                List<ZakazchikViewModel> listP = serviceP.GetList();
                if (listP != null)
                {
                    comboBoxClient.DisplayMemberPath = "ZakazchikFIO";
                    comboBoxClient.SelectedValuePath = "Id";
                    comboBoxClient.ItemsSource = listP;
                    comboBoxDvigateli.SelectedItem = null;
                }
                List<DvigateliViewModel> listM = serviceM.GetList();
                if (listM != null)
                {
                    comboBoxDvigateli.DisplayMemberPath = "DvigateliName";
                    comboBoxDvigateli.SelectedValuePath = "Id";
                    comboBoxDvigateli.ItemsSource = listM;
                    comboBoxDvigateli.SelectedItem = null;
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
                    DvigateliViewModel product = serviceM.GetElement(id);
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
                serviceG.CreateZakaz(new ZakazBindingModel
                {
                    ZakazchikId = ((ZakazchikViewModel)comboBoxClient.SelectedItem).Id,
                    DvigateliId = ((DvigateliViewModel)comboBoxDvigateli.SelectedItem).Id,
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
