using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbstractShopView
{
    public partial class FormCreateZakaz : Form
    {
        public FormCreateZakaz()
        {
            InitializeComponent();
        }

        private void FormCreateZakaz_Load(object sender, EventArgs e)
        {
            try
            {
                var responseC = APIClient.GetRequest("api/Zakazchik/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<ZakazchikViewModel> list = APIClient.GetElement<List<ZakazchikViewModel>>(responseC);
                    if (list != null)
                    {
                        comboBoxZakazchik.DisplayMember = "ZakazchikFIO";
                        comboBoxZakazchik.ValueMember = "Id";
                        comboBoxZakazchik.DataSource = list;
                        comboBoxZakazchik.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
                var responseP = APIClient.GetRequest("api/Dvigateli/GetList");
                if (responseP.Result.IsSuccessStatusCode)
                {
                    List<DvigateliViewModel> list = APIClient.GetElement<List<DvigateliViewModel>>(responseP);
                    if (list != null)
                    {
                        comboBoxProduct.DisplayMember = "DvigateliName";
                        comboBoxProduct.ValueMember = "Id";
                        comboBoxProduct.DataSource = list;
                        comboBoxProduct.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseP));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalcSum()
        {
            if (comboBoxProduct.SelectedValue != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = Convert.ToInt32(comboBoxProduct.SelectedValue);
                    var responseP = APIClient.GetRequest("api/Dvigateli/Get/" + id);
                    if (responseP.Result.IsSuccessStatusCode)
                    {
                        DvigateliViewModel Dvigateli = APIClient.GetElement<DvigateliViewModel>(responseP);
                        int count = Convert.ToInt32(textBoxCount.Text);
                        textBoxSum.Text = (count * (int)Dvigateli.Price).ToString();
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(responseP));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxZakazchik.SelectedValue == null)
            {
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxProduct.SelectedValue == null)
            {
                MessageBox.Show("Выберите изделие", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Main/CreateZakaz", new ZakazBindingModel
                {
                    ZakazchikId = Convert.ToInt32(comboBoxZakazchik.SelectedValue),
                    DvigateliId = Convert.ToInt32(comboBoxProduct.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToInt32(textBoxSum.Text)
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}