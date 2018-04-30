using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbstractShopView
{
    public partial class FormDetali : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormDetali()
        {
            InitializeComponent();
        }

        private void FormDetali_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Detali/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var Detali = APIClient.GetElement<DetaliViewModel>(response);
                        textBoxName.Text = Detali.DetaliName;
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
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Detali/UpdElement", new DetaliBindingModel
                    {
                        Id = id.Value,
                        DetaliName = textBoxName.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Detali/AddElement", new DetaliBindingModel
                    {
                        DetaliName = textBoxName.Text
                    });
                }
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