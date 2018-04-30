using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbstractShopView
{
    public partial class FormDvigateliComponent : Form
    {
        public DvigateliDetaliViewModel Model { set { model = value; } get { return model; } }

        private DvigateliDetaliViewModel model;

        public FormDvigateliComponent()
        {
            InitializeComponent();
        }

        private void FormProductComponent_Load(object sender, EventArgs e)
        {
            try
            {
                var response = APIClient.GetRequest("api/Detali/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    comboBoxComponent.DisplayMember = "DetaliName";
                    comboBoxComponent.ValueMember = "Id";
                    comboBoxComponent.DataSource = APIClient.GetElement<List<DetaliViewModel>>(response);
                    comboBoxComponent.SelectedItem = null;
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
            if (model != null)
            {
                comboBoxComponent.Enabled = false;
                comboBoxComponent.SelectedValue = model.DetaliId;
                textBoxCount.Text = model.Count.ToString();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxComponent.SelectedValue == null)
            {
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (model == null)
                {
                    model = new DvigateliDetaliViewModel
                    {
                        DetaliId = Convert.ToInt32(comboBoxComponent.SelectedValue),
                        DetaliName = comboBoxComponent.Text,
                        Count = Convert.ToInt32(textBoxCount.Text)
                    };
                }
                else
                {
                    model.Count = Convert.ToInt32(textBoxCount.Text);
                }
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
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