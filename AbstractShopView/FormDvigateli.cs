﻿using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbstractShopView
{
    public partial class FormDvigateli : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        private List<DvigateliDetaliViewModel> DvigateliDetalis;

        public FormDvigateli()
        {
            InitializeComponent();
        }

        private void FormProduct_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Dvigateli/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var Dvigateli = APIClient.GetElement<DvigateliViewModel>(response);
                        textBoxName.Text = Dvigateli.DvigateliName;
                        textBoxPrice.Text = Dvigateli.Price.ToString();
                        DvigateliDetalis = Dvigateli.DvigateliDetalis;
                        LoadData();
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
            else
            {
                DvigateliDetalis = new List<DvigateliDetaliViewModel>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (DvigateliDetalis != null)
                {
                    dataGridView.DataSource = null;
                    dataGridView.DataSource = DvigateliDetalis;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Visible = false;
                    dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormDvigateliComponent();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                    {
                        form.Model.DvigateliId = id.Value;
                    }
                    DvigateliDetalis.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = new FormDvigateliComponent();
                form.Model = DvigateliDetalis[dataGridView.SelectedRows[0].Cells[0].RowIndex];
                if (form.ShowDialog() == DialogResult.OK)
                {
                    DvigateliDetalis[dataGridView.SelectedRows[0].Cells[0].RowIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        DvigateliDetalis.RemoveAt(dataGridView.SelectedRows[0].Cells[0].RowIndex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (DvigateliDetalis == null || DvigateliDetalis.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<DvigateliDetaliBindingModel> DvigateliDetaliBM = new List<DvigateliDetaliBindingModel>();
                for (int i = 0; i < DvigateliDetalis.Count; ++i)
                {
                    DvigateliDetaliBM.Add(new DvigateliDetaliBindingModel
                    {
                        Id = DvigateliDetalis[i].Id,
                        DvigateliId = DvigateliDetalis[i].DvigateliId,
                        DetaliId = DvigateliDetalis[i].DetaliId,
                        Count = DvigateliDetalis[i].Count
                    });
                }
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Dvigateli/UpdElement", new DvigateliBindingModel
                    {
                        Id = id.Value,
                        DvigateliName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        DvigateliDetalis = DvigateliDetaliBM
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Dvigateli/AddElement", new DvigateliBindingModel
                    {
                        DvigateliName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        DvigateliDetalis = DvigateliDetaliBM
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