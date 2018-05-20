using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
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
using Unity;
using Unity.Attributes;

namespace WpfMotorZavod
{
    /// <summary>
    /// Логика взаимодействия для FormPutOnGarazh.xaml
    /// </summary>
    public partial class FormPutOnGarazh : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IGarazhService serviceGarazh;

        private readonly IDetaliService serviceDetali;

        private readonly IMainService serviceMain;

        public FormPutOnGarazh(IGarazhService serviceS, IDetaliService serviceC, IMainService serviceM)
        {
            InitializeComponent();
            Loaded += FormPutOnGarazh_Load;
            this.serviceGarazh = serviceS;
            this.serviceDetali = serviceC;
            this.serviceMain = serviceM;
        }

        private void FormPutOnGarazh_Load(object sender, EventArgs e)
        {
            try
            {
                List<DetaliViewModel> listDetali = serviceDetali.GetList();
                if (listDetali != null)
                {
                    comboBoxDetali.DisplayMemberPath = "DetaliName";
                    comboBoxDetali.SelectedValuePath = "Id";
                    comboBoxDetali.ItemsSource = listDetali;
                    comboBoxDetali.SelectedItem = null;
                }
                List<GarazhViewModel> listGarazh = serviceGarazh.GetList();
                if (listGarazh != null)
                {
                    comboBoxStock.DisplayMemberPath = "GarazhName";
                    comboBoxStock.SelectedValuePath = "Id";
                    comboBoxStock.ItemsSource = listGarazh;
                    comboBoxStock.SelectedItem = null;
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
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxStock.SelectedItem == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceMain.PutDetaliOnGarazh(new GarazhDetaliBindingModel
                {
                    DetaliId = Convert.ToInt32(comboBoxDetali.SelectedValue),
                    GarazhId = Convert.ToInt32(comboBoxStock.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                MessageBox.Show("Сохранение прошло успешно", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
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
