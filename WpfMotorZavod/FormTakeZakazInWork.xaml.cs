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
    /// Логика взаимодействия для FormTakeZakazInWork.xaml
    /// </summary>
    public partial class FormTakeZakazInWork : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IRabochiService serviceR;

        private readonly IMainService serviceG;

        private int? id;

        public FormTakeZakazInWork(IRabochiService serviceR, IMainService serviceG)
        {
            InitializeComponent();
            Loaded += FormTakeZakazInWork_Load;
            this.serviceR = serviceR;
            this.serviceG = serviceG;
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
                List<RabochiViewModel> listR = serviceR.GetList();
                if (listR != null)
                {
                    comboBoxRabochi.DisplayMemberPath = "RabochiFIO";
                    comboBoxRabochi.SelectedValuePath = "Id";
                    comboBoxRabochi.ItemsSource = listR;
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
                MessageBox.Show("Выберите рабочего", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceG.TakeZakazInWork(new ZakazBindingModel
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
