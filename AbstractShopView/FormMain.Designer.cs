using System.Drawing;

namespace AbstractShopView
{
    partial class FormMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.справочникиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.клиентыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.компонентыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изделияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.складыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сотрудникиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пополнитьСкладToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonPayZakaz = new System.Windows.Forms.Button();
            this.buttonZakazReady = new System.Windows.Forms.Button();
            this.buttonTakeZakazInWork = new System.Windows.Forms.Button();
            this.buttonCreateZakaz = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.buttonRef = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.справочникиToolStripMenuItem,
            this.пополнитьСкладToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1112, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // справочникиToolStripMenuItem
            // 
            this.справочникиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.клиентыToolStripMenuItem,
            this.компонентыToolStripMenuItem,
            this.изделияToolStripMenuItem,
            this.складыToolStripMenuItem,
            this.сотрудникиToolStripMenuItem});
            this.справочникиToolStripMenuItem.Name = "справочникиToolStripMenuItem";
            this.справочникиToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.справочникиToolStripMenuItem.Text = "Справочники";
            // 
            // клиентыToolStripMenuItem
            // 
            this.клиентыToolStripMenuItem.Name = "заказчикиToolStripMenuItem";
            this.клиентыToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.клиентыToolStripMenuItem.Text = "Заказчики";
            this.клиентыToolStripMenuItem.Click += new System.EventHandler(this.клиентыToolStripMenuItem_Click);
            // 
            // компонентыToolStripMenuItem
            // 
            this.компонентыToolStripMenuItem.Name = "деталиToolStripMenuItem";
            this.компонентыToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.компонентыToolStripMenuItem.Text = "Детали";
            this.компонентыToolStripMenuItem.Click += new System.EventHandler(this.компонентыToolStripMenuItem_Click);
            // 
            // изделияToolStripMenuItem
            // 
            this.изделияToolStripMenuItem.Name = "двигателиToolStripMenuItem";
            this.изделияToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.изделияToolStripMenuItem.Text = "Двигатели";
            this.изделияToolStripMenuItem.Click += new System.EventHandler(this.изделияToolStripMenuItem_Click);
            // 
            // складыToolStripMenuItem
            // 
            this.складыToolStripMenuItem.Name = "гаражиToolStripMenuItem";
            this.складыToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.складыToolStripMenuItem.Text = "Гаражы";
            this.складыToolStripMenuItem.Click += new System.EventHandler(this.складыToolStripMenuItem_Click);
            // 
            // сотрудникиToolStripMenuItem
            // 
            this.сотрудникиToolStripMenuItem.Name = "рабочиеToolStripMenuItem";
            this.сотрудникиToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.сотрудникиToolStripMenuItem.Text = "Рабочие";
            this.сотрудникиToolStripMenuItem.Click += new System.EventHandler(this.сотрудникиToolStripMenuItem_Click);
            // 
            // пополнитьСкладToolStripMenuItem
            // 
            this.пополнитьСкладToolStripMenuItem.Name = "пополнитьГаражToolStripMenuItem";
            this.пополнитьСкладToolStripMenuItem.Size = new System.Drawing.Size(115, 20);
            this.пополнитьСкладToolStripMenuItem.Text = "Пополнить гараж";
            this.пополнитьСкладToolStripMenuItem.Click += new System.EventHandler(this.пополнитьСкладToolStripMenuItem_Click);
            // 
            // buttonPayZakaz
            // 
            this.buttonPayZakaz.Location = new System.Drawing.Point(888, 200);
            this.buttonPayZakaz.Name = "buttonPayZakaz";
            this.buttonPayZakaz.Size = new System.Drawing.Size(149, 23);
            this.buttonPayZakaz.TabIndex = 4;
            this.buttonPayZakaz.Text = "Заказ оплачен";
            this.buttonPayZakaz.UseVisualStyleBackColor = true;
            this.buttonPayZakaz.Click += new System.EventHandler(this.buttonPayZakaz_Click);
            // 
            // buttonZakazReady
            // 
            this.buttonZakazReady.Location = new System.Drawing.Point(888, 148);
            this.buttonZakazReady.Name = "buttonZakazReady";
            this.buttonZakazReady.Size = new System.Drawing.Size(149, 23);
            this.buttonZakazReady.TabIndex = 3;
            this.buttonZakazReady.Text = "Заказ готов";
            this.buttonZakazReady.UseVisualStyleBackColor = true;
            this.buttonZakazReady.Click += new System.EventHandler(this.buttonZakazReady_Click);
            // 
            // buttonTakeZakazInWork
            // 
            this.buttonTakeZakazInWork.Location = new System.Drawing.Point(888, 101);
            this.buttonTakeZakazInWork.Name = "buttonTakeZakazInWork";
            this.buttonTakeZakazInWork.Size = new System.Drawing.Size(149, 23);
            this.buttonTakeZakazInWork.TabIndex = 2;
            this.buttonTakeZakazInWork.Text = "Отдать на выполнение";
            this.buttonTakeZakazInWork.UseVisualStyleBackColor = true;
            this.buttonTakeZakazInWork.Click += new System.EventHandler(this.buttonTakeZakazInWork_Click);
            // 
            // buttonCreateZakaz
            // 
            this.buttonCreateZakaz.Location = new System.Drawing.Point(888, 50);
            this.buttonCreateZakaz.Name = "buttonCreateZakaz";
            this.buttonCreateZakaz.Size = new System.Drawing.Size(149, 23);
            this.buttonCreateZakaz.TabIndex = 1;
            this.buttonCreateZakaz.Text = "Создать заказ";
            this.buttonCreateZakaz.UseVisualStyleBackColor = true;
            this.buttonCreateZakaz.Click += new System.EventHandler(this.buttonCreateZakaz_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridView.Location = new System.Drawing.Point(0, 24);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(873, 441);
            this.dataGridView.TabIndex = 0;
            // 
            // buttonRef
            // 
            this.buttonRef.Location = new System.Drawing.Point(888, 251);
            this.buttonRef.Name = "buttonRef";
            this.buttonRef.Size = new System.Drawing.Size(149, 23);
            this.buttonRef.TabIndex = 5;
            this.buttonRef.Text = "Обновить список";
            this.buttonRef.UseVisualStyleBackColor = true;
            this.buttonRef.Click += new System.EventHandler(this.buttonRef_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1112, 465);
            this.Controls.Add(this.buttonRef);
            this.Controls.Add(this.buttonPayZakaz);
            this.Controls.Add(this.buttonZakazReady);
            this.Controls.Add(this.buttonTakeZakazInWork);
            this.Controls.Add(this.buttonCreateZakaz);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "Моторный завод";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem справочникиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem компонентыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изделияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem складыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сотрудникиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пополнитьСкладToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem клиентыToolStripMenuItem;
        private System.Windows.Forms.Button buttonPayZakaz;
        private System.Windows.Forms.Button buttonZakazReady;
        private System.Windows.Forms.Button buttonTakeZakazInWork;
        private System.Windows.Forms.Button buttonCreateZakaz;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button buttonRef;
        private Size ZakazchikSize;
    }
}

