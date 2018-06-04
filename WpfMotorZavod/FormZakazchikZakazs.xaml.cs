using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
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
    /// Логика взаимодействия для FormZakazchikZakazs.xaml
    /// </summary>
    public partial class FormZakazchikZakazs : Window
    {

        public FormZakazchikZakazs()
        {
            InitializeComponent();
        }

        private void buttonMake_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate >= dateTimePickerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                reportViewer.LocalReport.ReportEmbeddedResource = "WpfMotorZavod.ReportZakazchikZakazs.rdlc";
                ReportParameter parameter = new ReportParameter("ReportParameterPeriod",
                                            "c " + Convert.ToDateTime(dateTimePickerFrom.SelectedDate).ToString("dd-MM") +
                                            " по " + Convert.ToDateTime(dateTimePickerTo.SelectedDate).ToString("dd-MM"));
                reportViewer.LocalReport.SetParameters(parameter);


                var response = APIClient.PostRequest("api/Report/GetZakazchikZakazs", new ReportBindingModel
                {
                    DateFrom = dateTimePickerFrom.SelectedDate,
                    DateTo = dateTimePickerTo.SelectedDate
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    var dataSource = APIClient.GetElement<List<ZakazchikZakazsModel>>(response);
                    ReportDataSource source = new ReportDataSource("DataSetZakazs", dataSource);
                    reportViewer.LocalReport.DataSources.Add(source);
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }

                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonToPdf_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate >= dateTimePickerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "pdf|*.pdf"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    var response = APIClient.PostRequest("api/Report/SaveZakazchikZakazs", new ReportBindingModel
                    {
                        FileName = sfd.FileName,
                        DateFrom = dateTimePickerFrom.SelectedDate,
                        DateTo = dateTimePickerTo.SelectedDate
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}