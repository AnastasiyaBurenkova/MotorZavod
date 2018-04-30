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
    /// Логика взаимодействия для TakeZakazInWork.xaml
    /// </summary>
    public partial class FormTakeZakazInWork : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ID { set { id = value; } }

        private readonly IRabochiService serviceRabochi;

        private readonly IMainService serviceMain;

        private int? id;

        public FormTakeZakazInWork(IRabochiService serviceI, IMainService serviceM)
        {
            InitializeComponent();
            Loaded += FormTakeZakazInWork_Load;
            this.serviceRabochi = serviceI;
            this.serviceMain = serviceM;
        }

        private void FormTakeZakazInWork_Load(object sender, EventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                List<RabochiViewModel> listRabochi = serviceRabochi.GetList();
                if (listRabochi != null)
                {
                    comboBoxRabochi.DisplayMemberPath = "RabochiFIO";
                    comboBoxRabochi.SelectedValuePath = "Id";
                    comboBoxRabochi.ItemsSource = listRabochi;
                    comboBoxRabochi.SelectedItem = null;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxRabochi.SelectedItem == null)
            {
                MessageBox.Show("Выберите исполнителя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceMain.TakeZakazInWork(new ZakazBindingModel
                {
                    Id = id.Value,
                    RabochiId = ((RabochiViewModel)comboBoxRabochi.SelectedItem).Id,
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
