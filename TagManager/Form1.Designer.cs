namespace TagManager
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.contextNodeStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.изменитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьПодтегToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьТегToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextTreeStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.добавитьТегToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.редактированиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьИЗакрытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.закончитьРедактированиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.очиститьСписокФайловToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выбратьИзображениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.запоминатьПоложениеОкнаToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.запоминатьРазмерОкнаToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.доступИзПроводникаToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.запрашиватьЗадержкуЗагрузкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tempTagList = new System.Windows.Forms.CheckedListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tree = new TagManager.TriStatesTreeViewControl();
            this.tempTree = new TagManager.TriStatesTreeViewControl();
            this.listView1 = new System.Windows.Forms.ListView();
            this.contextImageStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.редактироватьТегиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отменаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.сортироватьПоДатеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextNodeStrip.SuspendLayout();
            this.contextTreeStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextImageStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextNodeStrip
            // 
            this.contextNodeStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.изменитьToolStripMenuItem,
            this.добавитьПодтегToolStripMenuItem,
            this.удалитьТегToolStripMenuItem});
            this.contextNodeStrip.Name = "contextMenuStrip";
            this.contextNodeStrip.Size = new System.Drawing.Size(166, 70);
            // 
            // изменитьToolStripMenuItem
            // 
            this.изменитьToolStripMenuItem.Name = "изменитьToolStripMenuItem";
            this.изменитьToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.изменитьToolStripMenuItem.Text = "Изменить";
            this.изменитьToolStripMenuItem.Click += new System.EventHandler(this.изменитьToolStripMenuItem_Click);
            // 
            // добавитьПодтегToolStripMenuItem
            // 
            this.добавитьПодтегToolStripMenuItem.Name = "добавитьПодтегToolStripMenuItem";
            this.добавитьПодтегToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.добавитьПодтегToolStripMenuItem.Text = "Добавить подтег";
            this.добавитьПодтегToolStripMenuItem.Click += new System.EventHandler(this.добавитьПодтегToolStripMenuItem_Click);
            // 
            // удалитьТегToolStripMenuItem
            // 
            this.удалитьТегToolStripMenuItem.Name = "удалитьТегToolStripMenuItem";
            this.удалитьТегToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.удалитьТегToolStripMenuItem.Text = "Удалить тег";
            this.удалитьТегToolStripMenuItem.Click += new System.EventHandler(this.удалитьТегToolStripMenuItem_Click);
            // 
            // contextTreeStrip
            // 
            this.contextTreeStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьТегToolStripMenuItem,
            this.редактированиеToolStripMenuItem,
            this.очиститьСписокФайловToolStripMenuItem,
            this.выбратьИзображениеToolStripMenuItem,
            this.настройкиToolStripMenuItem});
            this.contextTreeStrip.Name = "contextTreeStrip";
            this.contextTreeStrip.Size = new System.Drawing.Size(270, 136);
            // 
            // добавитьТегToolStripMenuItem
            // 
            this.добавитьТегToolStripMenuItem.Name = "добавитьТегToolStripMenuItem";
            this.добавитьТегToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.добавитьТегToolStripMenuItem.Text = "Добавить тег";
            this.добавитьТегToolStripMenuItem.Click += new System.EventHandler(this.добавитьТегToolStripMenuItem_Click);
            // 
            // редактированиеToolStripMenuItem
            // 
            this.редактированиеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сохранитьToolStripMenuItem1,
            this.сохранитьИЗакрытьToolStripMenuItem,
            this.закончитьРедактированиеToolStripMenuItem});
            this.редактированиеToolStripMenuItem.Name = "редактированиеToolStripMenuItem";
            this.редактированиеToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.редактированиеToolStripMenuItem.Text = "Файл";
            // 
            // сохранитьToolStripMenuItem1
            // 
            this.сохранитьToolStripMenuItem1.Name = "сохранитьToolStripMenuItem1";
            this.сохранитьToolStripMenuItem1.Size = new System.Drawing.Size(189, 22);
            this.сохранитьToolStripMenuItem1.Text = "Сохранить";
            this.сохранитьToolStripMenuItem1.Click += new System.EventHandler(this.сохранитьToolStripMenuItem1_Click);
            // 
            // сохранитьИЗакрытьToolStripMenuItem
            // 
            this.сохранитьИЗакрытьToolStripMenuItem.Name = "сохранитьИЗакрытьToolStripMenuItem";
            this.сохранитьИЗакрытьToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.сохранитьИЗакрытьToolStripMenuItem.Text = "Сохранить и закрыть";
            this.сохранитьИЗакрытьToolStripMenuItem.Click += new System.EventHandler(this.сохранитьИЗакрытьToolStripMenuItem_Click);
            // 
            // закончитьРедактированиеToolStripMenuItem
            // 
            this.закончитьРедактированиеToolStripMenuItem.Name = "закончитьРедактированиеToolStripMenuItem";
            this.закончитьРедактированиеToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.закончитьРедактированиеToolStripMenuItem.Text = "Закрыть";
            this.закончитьРедактированиеToolStripMenuItem.Click += new System.EventHandler(this.очиститьСписокФайловToolStripMenuItem_Click);
            // 
            // очиститьСписокФайловToolStripMenuItem
            // 
            this.очиститьСписокФайловToolStripMenuItem.Name = "очиститьСписокФайловToolStripMenuItem";
            this.очиститьСписокФайловToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.очиститьСписокФайловToolStripMenuItem.Text = "Закончить редактирование файлов";
            this.очиститьСписокФайловToolStripMenuItem.Click += new System.EventHandler(this.очиститьСписокФайловToolStripMenuItem_Click);
            // 
            // выбратьИзображениеToolStripMenuItem
            // 
            this.выбратьИзображениеToolStripMenuItem.Enabled = false;
            this.выбратьИзображениеToolStripMenuItem.Name = "выбратьИзображениеToolStripMenuItem";
            this.выбратьИзображениеToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.выбратьИзображениеToolStripMenuItem.Text = "Выбрать изображение";
            this.выбратьИзображениеToolStripMenuItem.Click += new System.EventHandler(this.выбратьИзображениеToolStripMenuItem_Click);
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.запоминатьПоложениеОкнаToolStripMenuItem1,
            this.запоминатьРазмерОкнаToolStripMenuItem1,
            this.доступИзПроводникаToolStripMenuItem1,
            this.запрашиватьЗадержкуЗагрузкиToolStripMenuItem,
            this.сортироватьПоДатеToolStripMenuItem});
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            // 
            // запоминатьПоложениеОкнаToolStripMenuItem1
            // 
            this.запоминатьПоложениеОкнаToolStripMenuItem1.Checked = true;
            this.запоминатьПоложениеОкнаToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.запоминатьПоложениеОкнаToolStripMenuItem1.Name = "запоминатьПоложениеОкнаToolStripMenuItem1";
            this.запоминатьПоложениеОкнаToolStripMenuItem1.Size = new System.Drawing.Size(252, 22);
            this.запоминатьПоложениеОкнаToolStripMenuItem1.Text = "Запоминать положение окна";
            this.запоминатьПоложениеОкнаToolStripMenuItem1.Click += new System.EventHandler(this.запоминатьПоложениеОкнаToolStripMenuItem_Click);
            // 
            // запоминатьРазмерОкнаToolStripMenuItem1
            // 
            this.запоминатьРазмерОкнаToolStripMenuItem1.Checked = true;
            this.запоминатьРазмерОкнаToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.запоминатьРазмерОкнаToolStripMenuItem1.Name = "запоминатьРазмерОкнаToolStripMenuItem1";
            this.запоминатьРазмерОкнаToolStripMenuItem1.Size = new System.Drawing.Size(252, 22);
            this.запоминатьРазмерОкнаToolStripMenuItem1.Text = "Запоминать размер окна";
            this.запоминатьРазмерОкнаToolStripMenuItem1.Click += new System.EventHandler(this.запоминатьРазмерОкнаToolStripMenuItem_Click);
            // 
            // доступИзПроводникаToolStripMenuItem1
            // 
            this.доступИзПроводникаToolStripMenuItem1.Checked = true;
            this.доступИзПроводникаToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.доступИзПроводникаToolStripMenuItem1.Name = "доступИзПроводникаToolStripMenuItem1";
            this.доступИзПроводникаToolStripMenuItem1.Size = new System.Drawing.Size(252, 22);
            this.доступИзПроводникаToolStripMenuItem1.Text = "Доступ из проводника";
            this.доступИзПроводникаToolStripMenuItem1.Click += new System.EventHandler(this.доступИзПроводникаToolStripMenuItem_Click);
            // 
            // запрашиватьЗадержкуЗагрузкиToolStripMenuItem
            // 
            this.запрашиватьЗадержкуЗагрузкиToolStripMenuItem.Name = "запрашиватьЗадержкуЗагрузкиToolStripMenuItem";
            this.запрашиватьЗадержкуЗагрузкиToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.запрашиватьЗадержкуЗагрузкиToolStripMenuItem.Text = "Запрашивать задержку загрузки";
            this.запрашиватьЗадержкуЗагрузкиToolStripMenuItem.Click += new System.EventHandler(this.запрашиватьЗадержкуЗагрузкиToolStripMenuItem_Click);
            // 
            // tempTagList
            // 
            this.tempTagList.CheckOnClick = true;
            this.tempTagList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tempTagList.Enabled = false;
            this.tempTagList.FormattingEnabled = true;
            this.tempTagList.Location = new System.Drawing.Point(0, 0);
            this.tempTagList.Name = "tempTagList";
            this.tempTagList.Size = new System.Drawing.Size(150, 46);
            this.tempTagList.TabIndex = 2;
            this.tempTagList.Visible = false;
            this.tempTagList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.tempTagList_ItemCheck);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tempTree);
            this.splitContainer1.Panel2.Controls.Add(this.tempTagList);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(400, 420);
            this.splitContainer1.SplitterDistance = 263;
            this.splitContainer1.TabIndex = 3;
            // 
            // tree
            // 
            this.tree.AllowDrop = true;
            this.tree.ContextMenuStrip = this.contextTreeStrip;
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.LabelEdit = true;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Name = "tree";
            this.tree.Size = new System.Drawing.Size(400, 420);
            this.tree.TabIndex = 2;
            this.tree.TriStateStyleProperty = TagManager.TriStatesTreeViewControl.TriStateStyles.Standard;
            this.tree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tree_AfterLabelEdit);
            this.tree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tree_NodeMouseClick_1);
            // 
            // tempTree
            // 
            this.tempTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tempTree.Location = new System.Drawing.Point(0, 0);
            this.tempTree.Name = "tempTree";
            this.tempTree.Size = new System.Drawing.Size(150, 46);
            this.tempTree.TabIndex = 3;
            this.tempTree.TriStateStyleProperty = TagManager.TriStatesTreeViewControl.TriStateStyles.Standard;
            // 
            // listView1
            // 
            this.listView1.ContextMenuStrip = this.contextImageStrip;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(96, 100);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listView1_ItemDrag);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            this.listView1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.listView1_PreviewKeyDown);
            // 
            // contextImageStrip
            // 
            this.contextImageStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьToolStripMenuItem,
            this.редактироватьТегиToolStripMenuItem,
            this.отменаToolStripMenuItem});
            this.contextImageStrip.Name = "contextImageStrip";
            this.contextImageStrip.Size = new System.Drawing.Size(181, 70);
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.открытьToolStripMenuItem.Text = "Открыть";
            this.открытьToolStripMenuItem.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
            // 
            // редактироватьТегиToolStripMenuItem
            // 
            this.редактироватьТегиToolStripMenuItem.Name = "редактироватьТегиToolStripMenuItem";
            this.редактироватьТегиToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.редактироватьТегиToolStripMenuItem.Text = "Редактировать теги";
            this.редактироватьТегиToolStripMenuItem.Click += new System.EventHandler(this.редактироватьТегиToolStripMenuItem_Click);
            // 
            // отменаToolStripMenuItem
            // 
            this.отменаToolStripMenuItem.Name = "отменаToolStripMenuItem";
            this.отменаToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.отменаToolStripMenuItem.Text = "Отмена";
            this.отменаToolStripMenuItem.Click += new System.EventHandler(this.выбратьИзображениеToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(128, 128);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            this.splitContainer2.Panel1MinSize = 120;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listView1);
            this.splitContainer2.Panel2Collapsed = true;
            this.splitContainer2.Panel2MinSize = 180;
            this.splitContainer2.Size = new System.Drawing.Size(400, 420);
            this.splitContainer2.SplitterDistance = 120;
            this.splitContainer2.TabIndex = 7;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 398);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(400, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.Visible = false;
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(120, 16);
            this.toolStripProgressBar1.Step = 1;
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // сортироватьПоДатеToolStripMenuItem
            // 
            this.сортироватьПоДатеToolStripMenuItem.Checked = true;
            this.сортироватьПоДатеToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.сортироватьПоДатеToolStripMenuItem.Name = "сортироватьПоДатеToolStripMenuItem";
            this.сортироватьПоДатеToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.сортироватьПоДатеToolStripMenuItem.Text = "Сортировать по дате";
            this.сортироватьПоДатеToolStripMenuItem.Click += new System.EventHandler(this.сортироватьПоДатеToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 420);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "TagManager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.tree_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.tree_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.contextNodeStrip.ResumeLayout(false);
            this.contextTreeStrip.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextImageStrip.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextNodeStrip;
        private System.Windows.Forms.ToolStripMenuItem изменитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьПодтегToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьТегToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextTreeStrip;
        private System.Windows.Forms.ToolStripMenuItem добавитьТегToolStripMenuItem;
        private System.Windows.Forms.CheckedListBox tempTagList;
        private System.Windows.Forms.ToolStripMenuItem очиститьСписокФайловToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem запоминатьПоложениеОкнаToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem запоминатьРазмерОкнаToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem доступИзПроводникаToolStripMenuItem1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem выбратьИзображениеToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextImageStrip;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem редактироватьТегиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отменаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem редактированиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem закончитьРедактированиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьИЗакрытьToolStripMenuItem;
        private System.Windows.Forms.ListView listView1;
        private TriStatesTreeViewControl tempTree;
        private TriStatesTreeViewControl tree;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripMenuItem запрашиватьЗадержкуЗагрузкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сортироватьПоДатеToolStripMenuItem;
    }
}

