﻿using System;
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
    /// Логика взаимодействия для FormPutOnGarazh.xaml
    /// </summary>
    public partial class FormPutOnGarazh : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IGarazhService serviceB;

        private readonly IDetaliService serviceZ;

        private readonly IMainService serviceG;

        public FormPutOnGarazh(IGarazhService serviceB, IDetaliService serviceZ, IMainService serviceG)
        {
            InitializeComponent();
            Loaded += FormPutOnGarazh_Load;
            this.serviceB = serviceB;
            this.serviceZ = serviceZ;
            this.serviceG = serviceG;
        }

        private void FormPutOnGarazh_Load(object sender, EventArgs e)
        {
            try
            {
                List<DetaliViewModel> listZ = serviceZ.GetList();
                if (listZ != null)
                {
                    comboBoxDetali.DisplayMemberPath = "DetaliName";
                    comboBoxDetali.SelectedValuePath = "Id";
                    comboBoxDetali.ItemsSource = listZ;
                    comboBoxDetali.SelectedItem = null;
                }
                List<GarazhViewModel> listB = serviceB.GetList();
                if (listB != null)
                {
                    comboBoxGarazh.DisplayMemberPath = "GarazhName";
                    comboBoxGarazh.SelectedValuePath = "Id";
                    comboBoxGarazh.ItemsSource = listB;
                    comboBoxGarazh.SelectedItem = null;
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
                MessageBox.Show("Выберите заготовку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxGarazh.SelectedItem == null)
            {
                MessageBox.Show("Выберите базу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceG.PutDetaliOnGarazh(new GarazhDetaliBindingModel
                {
                    DetaliId = Convert.ToInt32(comboBoxDetali.SelectedValue),
                    GarazhId = Convert.ToInt32(comboBoxGarazh.SelectedValue),
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
