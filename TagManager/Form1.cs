using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections;

namespace TagManager
{
    public partial class Form1 : Form
    {
        public readonly List<string> supportableTypes = new List<string>() { ".jpg", ".jpeg" };
        const bool blockTagFileLoad = false;
        static FileList fullFileList = new FileList();

        static FileList fileList = new FileList();
        static List<Tag_Class> tagList = new List<Tag_Class>();
        static Dictionary<string, List<Tag_Class>> tagDictionary = new Dictionary<string, List<Tag_Class>>();
        public string argument;
        public Form1(string arg)
        {
            argument = arg;
            InitializeComponent();
            treeViewEventsAdd(tree);
            treeViewEventsAdd(tempTree);
            OnDragDropComplite += DragDropComplited;
        }
        const string tagDataFileName = "tagDataFile.tmd";
        const int fileLimit = 600;
        static string DataPath;
        static Tag_Class root;

        private void Form1_Load(object sender, EventArgs e)
        {
            Program.singleton.OtherInstanceCreated += OnOtherInstanceCreated;
            tree.AfterNodeCheck += Tree_AfterNodeCheck;
            tempTree.AfterNodeCheck += Tree_AfterNodeCheck;
            LoadSettings();
            listView1.BringToFront();
            DataPath = Application.UserAppDataPath + @"\";
            if (!File.Exists(DataPath + "contextIcon.ico"))
            {
                FileStream stream = new FileStream(DataPath + "contextIcon.ico", FileMode.Create, FileAccess.Write);
                Icon.Save(stream);
            }
            LoadTagData();
            if (argument.Length > 0)
            {
                AddEditableFileBegin(argument);
            }
        }

        private void Tree_AfterNodeCheck(object sender, TriStatesTreeViewControl.CheckedState e)
        {
            if (appState == AppState.ImageSelect)
            {
                List<string> tags = GetTagsByState(tree.Nodes, TriStatesTreeViewControl.CheckedState.Checked);
                tags.AddRange(GetTagsByState(tempTree.Nodes, TriStatesTreeViewControl.CheckedState.Checked));
                ShowImagesWithTags(tags);
            }
        }

        delegate void StringArrayDelegate(string e);
        private void OnOtherInstanceCreated(object sender, string[] e)
        {
            StringArrayDelegate sad = new StringArrayDelegate(AddEditableFileBegin);
            foreach (var s in e) {
                this.Invoke(sad, s);
            }
        }

        private void LoadSettings()
        {
            if (Properties.Settings.Default.WindowSize != new Size()) Size = Properties.Settings.Default.WindowSize;
            if (Properties.Settings.Default.WindowPos != new Point()) Location = Properties.Settings.Default.WindowPos;
            if (Properties.Settings.Default.SplitterDistance > 0) splitContainer1.SplitterDistance = Properties.Settings.Default.SplitterDistance;
            if (Properties.Settings.Default.Sorting) { listView1.ListViewItemSorter = new ImageComparer(); } else { сортироватьПоДатеToolStripMenuItem.Checked = false; }

            доступИзПроводникаToolStripMenuItem1.Checked = IsRegister();
            запоминатьРазмерОкнаToolStripMenuItem1.Checked = Properties.Settings.Default.SaveWindowSize;
            запоминатьПоложениеОкнаToolStripMenuItem1.Checked = Properties.Settings.Default.SaveWindowPos;
        }

        void SaveTagData()
        {
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(DataPath + tagDataFileName, FileMode.Create);
            bf.Serialize(stream, root);
            stream.Close();
        }
        void CheckRegister()
        {
            if (IsRegister() == false && Properties.Settings.Default.ExplorerContextButton == true)
            {
                if (MessageBox.Show("Добавить доступ к приложению через контекстное меню проводника?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    AddRemoveContextButton(true);
                }
                else
                {
                    Properties.Settings.Default.ExplorerContextButton = false;
                    доступИзПроводникаToolStripMenuItem1.Checked = false;
                }
            }
        }
        bool IsRegister()
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey("jpegfile").OpenSubKey("shell");
            return key.GetSubKeyNames().Contains("TagManager");
        }
        void AddRemoveContextButton(bool add)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.UseShellExecute = true;
            processInfo.FileName = Application.ExecutablePath;
            processInfo.Arguments = add ? "add" : "remove";
            processInfo.Verb = "runas";
            try
            {
                Process.Start(processInfo);
                Properties.Settings.Default.ExplorerContextButton = add;
            }
            catch
            {
                MessageBox.Show("Невозможно изменить контекстное меню проводника.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TagManager.Properties.Settings.Default.ExplorerContextButton = false;
                доступИзПроводникаToolStripMenuItem1.Checked = false;
            }
        }

        bool fileAddblock = false;
        int discount = 0;
        void AddEditableFile(string path)
        {
            if (path == "" || fileAddblock) return;
            if (File.Exists(path) && supportableTypes.Contains(new FileInfo(path).Extension))
            {
                if (fileList.Contains(path)) return;
                fileList.Add(path);
                if (!fullFileList.Contains(path)) fullFileList.Add(path);
                if (fileList.Count - discount > fileLimit)
                {
                    if (MessageBox.Show($"Выбрано {fileList.Count}, продолжить?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        fileAddblock = true;
                        return;
                    }
                    else
                    {
                        //Text = $"{fileList.Count} файла(ов) редактируются" + ((appState == AppState.ImageSelected) ? $"из {fullFileList.Count}" : "");
                        discount += fileLimit;
                    }
                }
                if (appState == AppState.Normal)
                {
                    appState = AppState.HasFile;
                    выбратьИзображениеToolStripMenuItem.Enabled = true;
                }
                //ReadTagsFormFile(path);
                fileList.Add(path);//===
            }
            else if (Directory.Exists(path))
            {
                foreach (var f in new DirectoryInfo(path).EnumerateDirectories())
                {
                    AddEditableFile(f.FullName);
                }
                foreach (var f in new DirectoryInfo(path).EnumerateFiles())
                {
                    AddEditableFile(f.FullName);
                }
            }
            //Text = $"{fileList.Count} файла(ов) редактируются";
            Text = $"{fileList.Count} файла(ов) редактируются" + ((appState == AppState.ImageSelected) ? $" из {fullFileList.Count}" : "");
        }
        void AddEditableFileBegin(string path)
        {
            AddEditableFile(path);
            fileList.ReadTags(tagDictionary, tempTree);
            if (fileList.Count > 0)
            {
                Tag_Class.TagsCheckSet(tagList, fileList.Count);
                foreach (TreeNode n in tempTree.Nodes)
                {
                    n.TempNodeStateSet(fileList.Count);
                }
            }
            else
            {
                Text = "TagManager";
            }
        }
        void AddEditableFileBegin(IEnumerable<string> list)
        {
            foreach (var f in list) { AddEditableFile(f); }
            fileList.ReadTags(tagDictionary, tempTree);
            if (fileList.Count > 0)
            {
                Tag_Class.TagsCheckSet(tagList, fileList.Count);
                foreach (TreeNode n in tempTree.Nodes)
                {
                    n.TempNodeStateSet(fileList.Count);
                }
            }
            else
            {
                Text = "TagManager";
            }
        }
        void LoadTagData()
        {
            if (Properties.Settings.Default.ExplorerContextButton) CheckRegister();

            if (File.Exists(DataPath + tagDataFileName) && !blockTagFileLoad)
            {
                bool loaded = false;
                BinaryFormatter bf = new BinaryFormatter();
                Stream stream = new FileStream(DataPath + tagDataFileName, FileMode.Open);
                try
                {
                    root = bf.Deserialize(stream) as Tag_Class;
                    stream.Close();
                    loaded = true;
                }
                catch
                {
                    stream.Close();
                    if (MessageBox.Show($"Ошибка чтения файла {tagDataFileName}, файл будет перезаписан", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        root = new Tag_Class("root", null);
                        File.Delete(DataPath + tagDataFileName);
                    }
                    else
                    {
                        Application.Exit();
                        return;
                    }
                }

                tagList = new List<Tag_Class>() { root };
                if (loaded)
                {
                    foreach (var t in root.Children)
                    {
                        TagsToTreeLoad(t, tree.Nodes.Add(""));
                    }
                }
            }
            else
            {
                root = new Tag_Class("root", null);
                tagList = new List<Tag_Class>() { root };
            }
        }

        void TagsToTreeLoad(Tag_Class tag, TreeNode node)
        {
            node.ContextMenuStrip = contextNodeStrip;
            node.Text = tag.Text;
            node.Name = tagList.Count.ToString();
            tag.Node = node;
            (node.TreeView as TriStatesTreeViewControl).SetCheckboxes(node);

            tagList.Add(tag);
            DictionaryAdd(tag);
            foreach (var t in tag.Children)
            {
                TagsToTreeLoad(t, node.Nodes.Add(""));
            }
        }
        void DictionaryAdd(Tag_Class tag)
        {
            if (tagDictionary.ContainsKey(tag.Text))
            {
                tagDictionary[tag.Text].Add(tag);
            }
            else
            {
                tagDictionary.Add(tag.Text, new List<Tag_Class>() { tag });
            }
        }
        void DictionatyRemove(Tag_Class tag)
        {
            if (tagDictionary.ContainsKey(tag.Text)) {
                List<Tag_Class> dList = tagDictionary[tag.Text];
                dList.Remove(tag);
                if (dList.Count == 0) tagDictionary.Remove(tag.Text);
            }
        }
        void AddNewNode(TreeNode newNode)
        {
            newNode.ContextMenuStrip = contextNodeStrip;
            newNode.BeginEdit();
            Tag_Class parentTag = tagList[newNode.Parent == null ? 0 : int.Parse(newNode.Parent.Name)];
            Tag_Class newTag = new Tag_Class("", newNode);
            newNode.Name = tagList.Count.ToString();
            (newNode.TreeView as TriStatesTreeViewControl).SetCheckboxes(newNode);
            tagList.Add(newTag);
            parentTag.Children.Add(newTag);
        }
        void DeleteTag(TreeNode node)
        {
            //MessageBox.Show(tagList.Count.ToString() + " " + node.Name);
            Tag_Class parentTag = tagList[node.Parent != null ? int.Parse(node.Parent.Name) : 0];
            Tag_Class tag = tagList[int.Parse(node.Name)];
            parentTag.Children.Remove(tag);
            ListsTagRemove(node);
            node.Remove();
        }
        void ListsTagRemove(TreeNode node)
        {
            foreach (var n in node.Nodes)
            {
                ListsTagRemove(n as TreeNode);
            }

            Tag_Class tag = tagList[int.Parse(node.Name)];
            tagList.RemoveAt(int.Parse(node.Name));
            DictionatyRemove(tag);
        }

        void WriteTags(List<string> tags)
        {

            List<string> mixedTags = GetTagsByState(tree.Nodes, TriStatesTreeViewControl.CheckedState.Mixed);
            mixedTags.AddRange(GetTagsByState(tempTree.Nodes, TriStatesTreeViewControl.CheckedState.Mixed));
            FileStream stream;
            foreach (var f in fileList)
            {
                stream = new FileStream(f, FileMode.Open, FileAccess.ReadWrite);
                var decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.None);
                var metadata = decoder.Frames[0].Metadata as BitmapMetadata;
                HashSet<string> keywords;
                if (!(metadata.ContainsQuery("/app1/ifd/PaddingSchema:Padding") && metadata.ContainsQuery("/app1/ifd/exif/PaddingSchema:Padding") && metadata.ContainsQuery("/xmp/PaddingSchema:Padding"))/*!metadata.ToList().Contains(@"/xmp")*/)
                {
                    stream.Close();
                    FilePaddingUpdate(f);
                    stream = new FileStream(f, FileMode.Open, FileAccess.ReadWrite);
                    decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.None);
                    metadata = decoder.Frames[0].Metadata as BitmapMetadata;
                }

                if (metadata?.Keywords != null)
                {
                    keywords = new HashSet<string>(metadata.Keywords);
                    keywords.RemoveWhere((k) => !tags.Contains(k) && !mixedTags.Contains(k));
                    foreach (var k in tags)
                    {
                        keywords.Add(k);
                    }
                }
                else
                {
                    keywords = new HashSet<string>(tags);
                }

                InPlaceBitmapMetadataWriter writer = decoder.Frames[0].CreateInPlaceBitmapMetadataWriter();
                writer.SetQuery("System.Keywords", keywords.ToArray());
                //writer.Keywords = new System.Collections.ObjectModel.ReadOnlyCollection<string>(keywords.ToList());
                if (!writer.TrySave())
                {
                    stream.Close();
                    FileFullRewrite(f, keywords.ToArray());
                }
                else
                {
                    stream.Close();
                }
            }
        }
        static void FilePaddingUpdate(string MetaInputFileName)
        {
            BitmapCreateOptions createOptions = BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreColorProfile;
            Stream oldStream = new System.IO.FileStream(MetaInputFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            BitmapDecoder oldDecoder = BitmapDecoder.Create(oldStream, createOptions, BitmapCacheOption.None);

            uint paddingAmount = 2048;
            BitmapMetadata metadata = oldDecoder.Frames[0].Metadata.Clone() as BitmapMetadata;
            metadata.SetQuery("/app1/ifd/PaddingSchema:Padding", paddingAmount);
            metadata.SetQuery("/app1/ifd/exif/PaddingSchema:Padding", paddingAmount);

            metadata.SetQuery("/xmp/PaddingSchema:Padding", paddingAmount);

            JpegBitmapEncoder output = new JpegBitmapEncoder();
            output.Frames.Add(BitmapFrame.Create(oldDecoder.Frames[0], null, metadata, null));
            MemoryStream tempStream = new MemoryStream();
            output.Save(tempStream);
            oldStream.Close();
            using (Stream outputFile = File.Open(MetaInputFileName, FileMode.Create, FileAccess.ReadWrite))
            {
                tempStream.WriteTo(outputFile);
                tempStream.Close();
                outputFile.Close();
                //output.Save(outputFile);
            }

            oldStream.Close();
        }
        public void FileFullRewrite(string imageFlePath, string[] keywords)
        {
            string jpegDirectory = Path.GetDirectoryName(imageFlePath);
            string jpegFileName = Path.GetFileNameWithoutExtension(imageFlePath);

            BitmapDecoder decoder = null;
            BitmapFrame bitmapFrame = null;
            BitmapMetadata metadata = null;
            FileInfo originalImage = new FileInfo(imageFlePath);

            if (File.Exists(imageFlePath))
            {
                // load the jpg file with a JpegBitmapDecoder    
                using (Stream jpegStreamIn = File.Open(imageFlePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    decoder = new JpegBitmapDecoder(jpegStreamIn, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                }

                bitmapFrame = decoder.Frames[0];
                metadata = (BitmapMetadata)bitmapFrame.Metadata;

                if (bitmapFrame != null)
                {
                    BitmapMetadata metaData = (BitmapMetadata)bitmapFrame.Metadata.Clone();

                    if (metaData != null)
                    {
                        // modify the metadata   
                        metaData.SetQuery("System.Keywords", keywords);

                        // get an encoder to create a new jpg file with the new metadata.      
                        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmapFrame, bitmapFrame.Thumbnail, metaData, bitmapFrame.ColorContexts));
                        //string jpegNewFileName = Path.Combine(jpegDirectory, "JpegTemp.jpg");

                        // Delete the original
                        originalImage.Delete();

                        // Save the new image 
                        using (Stream jpegStreamOut = File.Open(imageFlePath, FileMode.CreateNew, FileAccess.ReadWrite))
                        {
                            encoder.Save(jpegStreamOut);
                        }
                    }
                }
            }
        }
        void WriteCheckedTags()
        {
            List<string> checkedKeywords = GetTagsByState(tree.Nodes, TriStatesTreeViewControl.CheckedState.Checked);
            checkedKeywords.AddRange(GetTagsByState(tempTree.Nodes, TriStatesTreeViewControl.CheckedState.Checked));
            List<string> mixedTags = GetTagsByState(tree.Nodes, TriStatesTreeViewControl.CheckedState.Mixed);
            mixedTags.AddRange(GetTagsByState(tempTree.Nodes, TriStatesTreeViewControl.CheckedState.Mixed));
            fileList.WriteTags(checkedKeywords, mixedTags);
        }
        void DragDropComplited(TreeNode node, TreeNode oldParent, TreeView oldTree)
        {
            Tag_Class tag;
            if (oldTree == tree)
            {
                tag = tagList[int.Parse(node.Name)];
                Tag_Class oldParentTag = tagList[oldParent != null ? int.Parse(oldParent.Name) : 0];
                oldParentTag.Children.Remove(tag);
            }
            else
            {
                tag = new Tag_Class(node.Text, node);
                node.Name = tagList.Count.ToString();
                tagList.Add(tag);
                //MessageBox.Show(node.Name);
                DictionaryAdd(tag);
                node.ContextMenuStrip = contextNodeStrip;
                //MessageBox.Show("Added");
            }
            Tag_Class newParent = tagList[node.Parent != null ? int.Parse(node.Parent.Name) : 0];
            newParent.Children.Add(tag);
            SaveTagData();
            if (tempTree.Nodes.Count == 0) splitContainer1.Panel2Collapsed = true;
        }
        #region Events
        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tree.SelectedNode.BeginEdit();
        }

        private void tree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Label))
            {
                if (e.Node.Nodes.Count == 0)
                {
                    DeleteTag(e.Node);
                }
                else
                {
                    e.CancelEdit = true;
                }
            }
            else
            {
                Tag_Class tClass = tagList[int.Parse(e.Node.Name)];
                DictionatyRemove(tClass);
                tClass.Text = e.Label;
                DictionaryAdd(tClass);
            }

            SaveTagData();
            if (appState != AppState.ImageSelect)
            {
                UseThisFiles(fileList.ToArray());
            }
        }

        private void tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right) tree.SelectedNode = e.Node;
        }


        private void добавитьПодтегToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (tree.SelectedNode == null) return;
            //MessageBox.Show(e.ToString());
            TreeNode newNode = tree.SelectedNode.Nodes.Add("");
            tree.SelectedNode.Expand();
            AddNewNode(newNode);
        }

        private void удалитьТегToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tree.SelectedNode == null) return;
            if (MessageBox.Show($"Удалить тег {tree.SelectedNode.Text} и {tree.SelectedNode.GetNodeCount(true).ToString()} его подтегов", "Удаление тега", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DeleteTag(tree.SelectedNode);
                SaveTagData();
                UseThisFiles(fileList.ToArray());
            }
        }


        private void добавитьТегToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode newNode = tree.Nodes.Add("");
            AddNewNode(newNode);
        }


        private void доступИзПроводникаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (sender as ToolStripMenuItem);
            menuItem.Checked = !menuItem.Checked;
            Properties.Settings.Default.ExplorerContextButton = menuItem.Checked;
            //CheckRegister(menuItem.Checked,false);
            AddRemoveContextButton(menuItem.Checked);
        }

        private void tree_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            AddEditableFileBegin(s);
        }

        private void tree_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void очиститьСписокФайловToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseFiles();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ImageLoadThread != null) ImageLoadThread.Abort();
            Program.singleton.Dispose();
            Properties.Settings.Default.SaveWindowPos = запоминатьПоложениеОкнаToolStripMenuItem1.Checked;
            Properties.Settings.Default.SaveWindowSize = запоминатьРазмерОкнаToolStripMenuItem1.Checked;
            if (WindowState == FormWindowState.Normal)
            {
                if (Properties.Settings.Default.SaveWindowSize)
                {
                    Properties.Settings.Default.WindowSize = !splitContainer2.Panel2Collapsed ? new Size(Size.Width - splitContainer2.Panel2MinSize, Size.Height) : Size;
                    Properties.Settings.Default.SplitterDistance = splitContainer1.SplitterDistance;
                }
                if (Properties.Settings.Default.SaveWindowPos)
                {
                    Properties.Settings.Default.WindowPos = Location;
                }
            }

            Properties.Settings.Default.Save();
        }

        private void tempTagList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //AddRemoveTagToFiles((sender as CheckedListBox).Items[e.Index] as string, e.NewValue == CheckState.Checked);
        }
        private void запоминатьПоложениеОкнаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (sender as ToolStripMenuItem);
            menuItem.Checked = !menuItem.Checked;
            Properties.Settings.Default.SaveWindowPos = menuItem.Checked;
        }
        private void запоминатьРазмерОкнаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (sender as ToolStripMenuItem);
            menuItem.Checked = !menuItem.Checked;
            Properties.Settings.Default.SaveWindowSize = menuItem.Checked;
        }
        public enum AppState
        {
            Normal, HasFile, ImageSelect, ImageSelected, ImageLoad
        }
        private AppState appState_value = AppState.Normal;
        public AppState appState {
            get { return appState_value; }
            set
            {
                AppState previousState = appState_value;
                appState_value = value;
                if (value != AppState.ImageLoad && ImageLoadThread != null && ImageLoadThread.IsAlive) ImageLoadThread.Abort();

                switch (value)
                {
                    case AppState.ImageLoad:
                    statusStrip1.Visible = true;
                    tree.Enabled = false;
                    tempTree.Enabled = false;
                    ListViewVisible = true;
                    tree.ChildNodesStateSet(tree.TopNode, TriStatesTreeViewControl.CheckedState.UnChecked);
                    tempTree.ChildNodesStateSet(tempTree.TopNode, TriStatesTreeViewControl.CheckedState.UnChecked);
                    выбратьИзображениеToolStripMenuItem.Text = "Отмена";
                    редактироватьТегиToolStripMenuItem.Enabled = false;

                    break;
                    case AppState.Normal:
                    statusStrip1.Visible = false;
                    tree.Enabled = true;
                    tempTree.Enabled = true;
                    выбратьИзображениеToolStripMenuItem.Text = "Выбрать Изображение";
                    ListViewVisible = false;
                    tree.CheckBoxes = false;
                    splitContainer1.Panel2Collapsed = true;
                    Text = "TagManager";
                    выбратьИзображениеToolStripMenuItem.Enabled = false;
                    break;

                    case AppState.HasFile:
                    statusStrip1.Visible = false;
                    tree.Enabled = true;
                    tempTree.Enabled = true;
                    выбратьИзображениеToolStripMenuItem.Text = "Выбрать Изображение";
                    ListViewVisible = false;
                    выбратьИзображениеToolStripMenuItem.Enabled = true;
                    tree.CheckBoxes = true;
                    break;

                    case AppState.ImageSelect:
                    statusStrip1.Visible = false;
                    tree.Enabled = true;
                    tempTree.Enabled = true;

                    ListViewVisible = true;
                    tree.ChildNodesStateSet(tree.TopNode, TriStatesTreeViewControl.CheckedState.UnChecked);
                    tempTree.ChildNodesStateSet(tempTree.TopNode, TriStatesTreeViewControl.CheckedState.UnChecked);
                    выбратьИзображениеToolStripMenuItem.Text = "Отмена";
                    редактироватьТегиToolStripMenuItem.Enabled = true;
                    break;

                    case AppState.ImageSelected:
                    statusStrip1.Visible = false;
                    tree.Enabled = true;
                    tempTree.Enabled = true;

                    ListViewVisible = true;
                    выбратьИзображениеToolStripMenuItem.Text = "Отмена";
                    tree.CheckBoxes = true;
                    break;
                }
            }
        }
        private void выбратьИзображениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (appState)
            {
                case AppState.ImageSelect:
                UseThisFiles(fullFileList.ToArray());
                appState = AppState.HasFile;
                break;
                case AppState.HasFile:
                appState = AppState.ImageLoad;
                break;
                case AppState.ImageSelected:
                UseThisFiles(fullFileList.ToArray());
                appState = AppState.ImageSelect;
                break;
                default:
                AddEditableFileBegin(fileList);
                appState = AppState.HasFile;
                break;
            }
        }
        #endregion
        #region DragDrop
        public event Action<TreeNode, TreeNode, TreeView> OnDragDropComplite;
        private void treeViewEventsAdd(TreeView t)
        {
            t.ItemDrag += new ItemDragEventHandler(this.treeView_ItemDrag);
            t.DragEnter += new DragEventHandler(this.treeView_DragEnter);
            t.DragDrop += new DragEventHandler(this.treeView_DragDrop);
        }
        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }
        private void treeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void treeView_DragDrop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                TreeNode NewNode = null;
                TreeNode CompliteNode = null;
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
                NewNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                TreeView oldTree = NewNode.TreeView;
                TreeNode oldParent = NewNode.Parent;
                if (DestinationNode == null)
                {
                    if ((TreeView)sender == NewNode.TreeView)
                    {
                        CompliteNode = (TreeNode)NewNode.Clone();
                        ((TreeView)sender).Nodes.Add(CompliteNode);
                    }
                    else
                    {
                        CompliteNode = ((TreeView)sender).Nodes.Add(NewNode.Name, NewNode.Text);
                        //CompliteNode.Checked = NewNode.Checked;
                        CompliteNode.CheckSet((TriStatesTreeViewControl.CheckedState)NewNode.Tag);
                    }
                    NewNode.Remove();
                }
                else if (!IsParentNode(NewNode, DestinationNode) && NewNode != DestinationNode)
                {
                    if (DestinationNode.TreeView == NewNode.TreeView)
                    {
                        CompliteNode = (TreeNode)NewNode.Clone();
                        DestinationNode.Nodes.Add(CompliteNode);
                    }
                    else
                    {
                        CompliteNode = DestinationNode.Nodes.Add(NewNode.Name, NewNode.Text);
                        //CompliteNode.Checked = NewNode.Checked;
                        CompliteNode.CheckSet((TriStatesTreeViewControl.CheckedState)NewNode.Tag);
                    }

                    DestinationNode.Expand();
                    NewNode.Remove();
                }
                if (CompliteNode != null && OnDragDropComplite != null) OnDragDropComplite(CompliteNode, oldParent, oldTree);
            }
            else if (e.Data.GetDataPresent("System.Windows.Forms.ListViewItem", false))
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    List<string> newFiles = new List<string>();
                    foreach (var i in listView1.SelectedItems)
                    {
                        newFiles.Add((i as ListViewItem).ImageKey);
                    }
                    appState = AppState.ImageSelected;
                    UseThisFiles(newFiles.ToArray());
                }
            }
            else
            {
                this.OnDragDrop(e);
            }
        }
        private bool IsParentNode(TreeNode parent, TreeNode child)
        {
            if (child.Parent == null)
            {
                return false;
            }
            else if (child.Parent == parent)
            {
                return true;
            }
            else
            {
                return IsParentNode(parent, child.Parent);
            }
        }




        #endregion
        Thread ImageLoadThread;
        private bool ListViewVisible
        {
            set
            {
                if (value == splitContainer2.Panel2Collapsed)
                {
                    if (value)
                    {
                        bool loadreqest = TagManager.Properties.Settings.Default.LoadDelayRequest;
                        string input = "0,01";
                        if (loadreqest)
                        {
                            ShowInputDialog(ref input, "Задержка загрузки", false);
                            input = input.Replace('.', ',');
                        }
                        double delay = 0.01;
                        if (!loadreqest || double.TryParse(input, out delay))
                        {

                            splitContainer2.Panel2Collapsed = false;
                            Size = new Size(Size.Width + splitContainer2.Panel2MinSize, Size.Height);
                            //statusStrip1.Visible = true;
                            fileList.OnProgressChanged += UpdateProgressBar;
                            fileList.OnProcessComplited += OnImagesLoadedComplited;

                            ImageLoadThread = new Thread(new ThreadStart(new ThreadMethod3<ImageList, ListView, double>(fileList.AsynsLoadImages, imageList1, listView1, delay).StartMethod));
                            ImageLoadThread.Start();
                            //fileList.LoadImages(imageList1, listView1);
                        }
                        else
                        {
                            ListViewVisible = true;
                        }
                    }
                    else
                    {
                        Size = new Size(Size.Width - splitContainer2.Panel2MinSize, Size.Height);
                        splitContainer2.Panel2Collapsed = true;
                    }
                }
            }
        }

        void OnImagesLoadedComplited(bool success)
        {
            fileList.OnProcessComplited -= OnImagesLoadedComplited;
            fileList.OnProgressChanged -= UpdateProgressBar;
            Invoke(new Action(() =>
            {
                //statusStrip1.Visible = false;
                appState = AppState.ImageSelect;
            }
            ));
        }
        void UpdateProgressBar(double percent)
        {
            Invoke(new Action(() =>
            {
                toolStripProgressBar1.Value = (int)Math.Round(percent * 100);
            }
            ));
        }

        List<string> GetTagsByState(TreeNodeCollection nodes, TriStatesTreeViewControl.CheckedState state)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].StateImageIndex == (int)state)
                {
                    string c = nodes[i].Text;
                    list.Add(c);
                }
                list.AddRange(GetTagsByState(nodes[i].Nodes, state));
            }
            return list;
        }

        private void редактироватьТегиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Не выбран ни один элемент.");
                return;
            }
            List<string> newFiles = new List<string>();
            foreach (var i in listView1.SelectedItems)
            {
                newFiles.Add((i as ListViewItem).Tag as string);
            }
            appState = AppState.ImageSelected;
            UseThisFiles(newFiles.ToArray());
        }
        public void UseThisFiles(string[] files)
        {
            fileList.Clear();
            Tag_Class.ClearTagsFileCount(tagList);
            tempTree.Nodes.Clear();

            if (files.Length > 0)
            {
                tree.ChildNodesStateSet(tree.TopNode, TriStatesTreeViewControl.CheckedState.UnChecked);
            }

            AddEditableFileBegin(files);

            if (files.Length > 0)
            {
                Tag_Class.TagsCheckSet(tagList, fileList.Count);
            }
            else
            {
                tree.CheckBoxes = false;
            }
        }

        private void сохранитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            WriteCheckedTags();
        }

        private void сохранитьИЗакрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WriteCheckedTags();
            CloseFiles();
        }

        void CloseFiles()
        {
            fileList.Clear();
            fullFileList.Clear();
            imageList1.Images.Clear();
            listView1.Items.Clear();
            Tag_Class.ClearTagsFileCount(tagList);
            tempTree.Nodes.Clear();
            appState = AppState.Normal;
        }

        void RemoveImagesWithoutTag(string tag)
        {
            List<string> keys = new List<string>();
            foreach (ListViewItem i in listView1.Items)
            {
                if (!((string[])i.Tag).Contains(tag))
                {
                    keys.Add(i.Name);
                }
            }
            keys.ForEach(k => listView1.Items.RemoveByKey(k));
        }
        void ShowImagesWithTags(List<string> tags)
        {
            statusStrip1.Visible = true;
            Enabled = false;


            List<string> keys = new List<string>();

            int counter = 0, maxCount;
            maxCount = imageList1.Images.Keys.Count;

            foreach (var i in imageList1.Images.Keys)
            {
                var imageTags = FileList.GetTags(i);
                bool add = true;
                foreach (var t in tags)
                {
                    if (!imageTags.Contains(t))
                    {
                        add = false;
                        break;
                    }
                }
                if (add) keys.Add(i);

                counter++;
                UpdateProgressBar(counter / (double)maxCount);
            }

            maxCount = listView1.Items.Count + keys.Count;
            counter = 0;

            for (int i = listView1.Items.Count - 1; i >= 0; i--)
            {
                if (!keys.Contains(listView1.Items[i].Name))
                {
                    listView1.Items.RemoveAt(i);
                }
                counter++;
                UpdateProgressBar(counter / (double)maxCount);
            }
            foreach (var k in keys)
            {
                if (!listView1.Items.ContainsKey(k))
                {
                    listView1.Items.Add(k, "", k).Tag = k;
                }
                counter++;
                UpdateProgressBar(counter / (double)maxCount);
            }

            statusStrip1.Visible = false;
            Enabled = true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            (tree as TriStatesTreeViewControl).ChildNodesStateSet(tree.TopNode, TriStatesTreeViewControl.CheckedState.Mixed);
        }
        public static ImageForm imageForm;
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Не выбран ни один элемент.");
                return;
            }
            if (imageForm == null || imageForm.closed) imageForm = new ImageForm(this);
            //imageForm.ImagesSet(listView1.SelectedItems)
            List<string> list = new List<string>();
            foreach (ListViewItem i in listView1.SelectedItems)
            {
                list.Add(i.ImageKey);
                //Process.Start((i as ListViewItem).Tag as string);
            }

            imageForm.Show(list);
        }

        private void tree_NodeMouseClick_1(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ((TreeView)sender).SelectedNode = e.Node;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            switch (appState)
            {
                case AppState.HasFile:
                if (e.Control && e.KeyCode != Keys.ControlKey)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.S:
                        WriteCheckedTags();
                        break;
                        case Keys.W:
                        CloseFiles();
                        break;
                    }
                }
                else
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Escape:
                        CloseFiles();
                        break;
                        case Keys.Enter:
                        WriteCheckedTags();
                        CloseFiles();
                        break;
                    }
                }
                break;
                case AppState.ImageSelected:
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                    UseThisFiles(fullFileList.ToArray());
                    appState = AppState.ImageSelect;
                    break;
                    case Keys.Enter:
                    WriteCheckedTags();
                    UseThisFiles(fullFileList.ToArray());
                    appState = AppState.ImageSelect;
                    break;

                }
                break;
                case AppState.ImageSelect:
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                    UseThisFiles(fullFileList.ToArray());
                    appState = AppState.HasFile;
                    break;
                    case Keys.R:
                    if (listView1.SelectedItems.Count > 0)
                    {
                        List<string> newFiles = new List<string>();
                        foreach (var i in listView1.SelectedItems)
                        {
                            newFiles.Add((i as ListViewItem).ImageKey);
                        }
                        appState = AppState.ImageSelected;
                        UseThisFiles(newFiles.ToArray());
                    }
                    break;
                }
                break;
                case AppState.ImageLoad:
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                    //UseThisFiles(fullFileList.ToArray());
                    appState = AppState.HasFile;
                    break;
                }
                break;
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (imageForm == null || imageForm.closed) imageForm = new ImageForm(this);
            List<string> list = new List<string>();
            foreach (ListViewItem i in listView1.SelectedItems)
            {
                list.Add(i.ImageKey);
            }
            imageForm.Show(list);
        }

        private void listView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && listView1.SelectedItems.Count > 0)
            {
                if (imageForm == null || imageForm.closed) imageForm = new ImageForm(this);
                List<string> list = new List<string>();
                foreach (ListViewItem i in listView1.SelectedItems)
                {
                    list.Add(i.ImageKey);
                }

                imageForm.Show(list);
            }
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop((ListViewItem)e.Item, DragDropEffects.Move);
        }

        private static DialogResult ShowInputDialog(ref string input, string name, bool cancerButton)
        {
            System.Drawing.Size size = new System.Drawing.Size(200, 70);
            Form inputBox = new Form();
            inputBox.ControlBox = false;
            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = name;

            System.Windows.Forms.TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new System.Drawing.Point(5, 5);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);
            if (cancerButton)
            {
                Button cancelButton = new Button();
                cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                cancelButton.Name = "cancelButton";
                cancelButton.Size = new System.Drawing.Size(75, 23);
                cancelButton.Text = "&Cancel";
                cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
                inputBox.Controls.Add(cancelButton);
                inputBox.CancelButton = cancelButton;
            }
            inputBox.AcceptButton = okButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }

        private void запрашиватьЗадержкуЗагрузкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            item.Checked = !item.Checked;
            TagManager.Properties.Settings.Default.LoadDelayRequest = item.Checked;
        }

        private void сортироватьПоДатеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool sortingBool = ((ToolStripMenuItem)sender).Checked;
            if (sortingBool != Properties.Settings.Default.Sorting) {
                if (sortingBool)
                {
                    Properties.Settings.Default.Sorting = true;
                    listView1.ListViewItemSorter = new ImageComparer();
                }
                else
                {
                    Properties.Settings.Default.Sorting = false;
                }
            }
        }
    }
    [Serializable]
    public class Tag_Class
    {
        public string Text;
        public List<Tag_Class> Children = new List<Tag_Class>();
        [NonSerialized] public TreeNode Node;
        [NonSerialized] public int filesWithTagCount = 0;
        public Tag_Class(string text, TreeNode node)
        {
            Text = text;
            Node = node;
        }
        public static void TagsCheckSet(List<Tag_Class> tags, int fileCount)
        {
            foreach (var t in tags)
            {
                if (t.Node != null)
                {
                    if (t.filesWithTagCount == 0)
                    {
                        t.Node.CheckSet(TriStatesTreeViewControl.CheckedState.UnChecked);
                    }
                    else if (t.filesWithTagCount == fileCount)
                    {
                        t.Node.CheckSet(TriStatesTreeViewControl.CheckedState.Checked);
                    }
                    else
                    {
                        t.Node.CheckSet(TriStatesTreeViewControl.CheckedState.Mixed);
                    }
                }
            }
        }
        public static void ClearTagsFileCount(List<Tag_Class> list)
        {
            list.ForEach((t) => t.filesWithTagCount = 0);
        }

        //public static void SaveTagData(Tag_Class rootTag, string Folder,string fileName)
        //{
        //    BinaryFormatter bf = new BinaryFormatter();
        //    Stream stream = new FileStream(Folder + fileName, FileMode.Create);
        //    bf.Serialize(stream, rootTag);
        //    stream.Close();
        //}
        //public static void LoadTagData(out Tag_Class rootTag, string Folder, string fileName)
        //{
        //    if (File.Exists(Folder + fileName))
        //    {
        //        bool loaded = false;
        //        BinaryFormatter bf = new BinaryFormatter();
        //        Stream stream = new FileStream(Folder + fileName, FileMode.Open);
        //        try
        //        {
        //            rootTag = bf.Deserialize(stream) as Tag_Class;
        //            stream.Close();
        //            loaded = true;
        //        }
        //        catch
        //        {
        //            stream.Close();
        //            if (MessageBox.Show($"Ошибка чтения файла {fileName}, файл будет перезаписан", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
        //            {
        //                rootTag = new Tag_Class("root", null);
        //                File.Delete(Folder + fileName);
        //            }
        //            else
        //            {
        //                Application.Exit();
        //                rootTag = null;
        //                return;
        //            }
        //        }

        //        //tagList = new List<Tag_Class>() { rootTag };
        //        //if (loaded)
        //        //{
        //        //    foreach (var t in rootTag.Children)
        //        //    {
        //        //        TagsToTreeLoad(t, tree.Nodes.Add(""));
        //        //    }
        //        //}
        //    }
        //    else
        //    {
        //        rootTag = new Tag_Class("root", null);
        //    }
        //}

        //public void TagsToTreeLoad(out List<Tag_Class> tagList,ContextMenuStrip strip)
        //{
        //    tagList = new List<Tag_Class>() { this };



        //}
        //private void TagToTreeLoad(ref List<Tag_Class> tagList,ref Dictionary<string,List<Tag_Class>> dictionary, ContextMenuStrip strip)
        //{
        //    Node.ContextMenuStrip = strip;
        //    Node.Text = Text;
        //    Node.Name = tagList.Count.ToString();
        //    (Node.TreeView as TriStatesTreeViewControl).SetCheckboxes(Node);

        //    tagList.Add(this);
        //    DictionaryAdd(tag);
        //    foreach (var t in Children)
        //    {
        //        t.Node = Node.Nodes.Add("");
        //        t.TagToTreeLoad(ref List < Tag_Class > tagList, ContextMenuStrip strip);
        //    }
        //}
    }
    public class FileList : IList<string>
    {
        public event Action<double> OnProgressChanged;
        public event Action<bool> OnProcessComplited;
        public List<string> fileList = new List<string>();

        public static List<string> GetTags(string path)
        {
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.None);
            var metadata = decoder.Frames[0].Metadata as BitmapMetadata;
            bool hasKeys = metadata?.Keywords != null;
            List<string> res;
            if (hasKeys)
            {
                res = new List<string>(metadata.Keywords);
            }
            else
            {
                res = new List<string>();
            }
            stream.Close();
            stream.Dispose();
            return res;
        }
        public void ReadTags(Dictionary<string, List<Tag_Class>> tagDictionary, TreeView tempTree)
        {
            tempTree.Nodes.Clear();
            tempTree.CheckBoxes = false;
            foreach (var tList in tagDictionary)
            {
                foreach (var t in tList.Value)
                {
                    t.filesWithTagCount = 0;
                }
            }
            fileList.ForEach(f => ReadTags(f, tagDictionary, tempTree));

            tempTree.CheckBoxes = tempTree.Nodes.Count > 0;
            (tempTree.Parent.Parent as SplitContainer).Panel2Collapsed = tempTree.Nodes.Count == 0;
        }
        protected static void ReadTags(string file, Dictionary<string, List<Tag_Class>> tagDictionary, TreeView tempTree)
        {
            List<string> Keywords = GetTags(file);
            foreach (var t in Keywords)
            {
                if (tagDictionary.ContainsKey(t))
                {
                    tagDictionary[t].ForEach((n) =>
                    {
                        n.filesWithTagCount++;
                    }
                    );
                }
                else
                {

                    if (tempTree.Nodes.Find(t, false).Length > 0)
                    {
                        var node = tempTree.Nodes.Find(t, false)[0];
                        node.Tag = (int)node.Tag + 1;
                    }
                    else
                    {
                        var node = tempTree.Nodes.Add(t, t);
                        node.Tag = 1;
                    }
                }
            }
        }

        #region IO
        public void WriteTags(List<string> tags, List<string> mixedTags)
        {
            FileStream stream;
            foreach (var f in fileList)
            {
                stream = new FileStream(f, FileMode.Open, FileAccess.ReadWrite);
                var decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.None);
                var metadata = decoder.Frames[0].Metadata as BitmapMetadata;
                HashSet<string> keywords;

                if (!(metadata.ContainsQuery("/app1/ifd/PaddingSchema:Padding") && metadata.ContainsQuery("/app1/ifd/exif/PaddingSchema:Padding") && metadata.ContainsQuery("/xmp/PaddingSchema:Padding"))/*!metadata.ToList().Contains(@"/xmp")*/)
                {
                    stream.Close();
                    try
                    {
                        FilePaddingUpdate(f);
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось отредактировать файл");
                        continue;
                    }
                    stream = new FileStream(f, FileMode.Open, FileAccess.ReadWrite);
                    decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.None);
                    metadata = decoder.Frames[0].Metadata as BitmapMetadata;
                }

                if (metadata?.Keywords != null)
                {
                    keywords = new HashSet<string>(metadata.Keywords);
                    keywords.RemoveWhere((k) => !tags.Contains(k) && !mixedTags.Contains(k));
                    foreach (var k in tags)
                    {
                        keywords.Add(k);
                    }
                }
                else
                {
                    keywords = new HashSet<string>(tags);
                }

                InPlaceBitmapMetadataWriter writer = decoder.Frames[0].CreateInPlaceBitmapMetadataWriter();
                writer.SetQuery("System.Keywords", keywords.ToArray());
                if (!writer.TrySave())
                {
                    stream.Close();
                    FileFullRewrite(f, keywords.ToArray());
                }
                else
                {
                    stream.Close();
                }
            }
        }
        protected static void FilePaddingUpdate(string MetaInputFileName)
        {
            BitmapCreateOptions createOptions = BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreColorProfile;
            Stream oldStream = new System.IO.FileStream(MetaInputFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            BitmapDecoder oldDecoder = BitmapDecoder.Create(oldStream, createOptions, BitmapCacheOption.None);

            uint paddingAmount = 2048;
            BitmapMetadata metadata = oldDecoder.Frames[0].Metadata.Clone() as BitmapMetadata;
            metadata.SetQuery("/app1/ifd/PaddingSchema:Padding", paddingAmount);
            metadata.SetQuery("/app1/ifd/exif/PaddingSchema:Padding", paddingAmount);

            metadata.SetQuery("/xmp/PaddingSchema:Padding", paddingAmount);

            JpegBitmapEncoder output = new JpegBitmapEncoder();
            output.Frames.Add(BitmapFrame.Create(oldDecoder.Frames[0], null, metadata, null));
            MemoryStream tempStream = new MemoryStream();
            output.Save(tempStream);
            oldStream.Close();
            using (Stream outputFile = File.Open(MetaInputFileName, FileMode.Create, FileAccess.ReadWrite))
            {
                tempStream.WriteTo(outputFile);
                tempStream.Close();
                outputFile.Close();
            }

            oldStream.Close();
        }
        protected static void FileFullRewrite(string imageFilePath, string[] keywords)
        {
            string jpegDirectory = Path.GetDirectoryName(imageFilePath);
            string jpegFileName = Path.GetFileNameWithoutExtension(imageFilePath);

            BitmapDecoder decoder = null;
            BitmapFrame bitmapFrame = null;
            BitmapMetadata metadata = null;
            FileInfo originalImage = new FileInfo(imageFilePath);

            if (File.Exists(imageFilePath))
            {
                // load the jpg file with a JpegBitmapDecoder    
                using (Stream jpegStreamIn = File.Open(imageFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    decoder = new JpegBitmapDecoder(jpegStreamIn, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                }

                bitmapFrame = decoder.Frames[0];
                metadata = (BitmapMetadata)bitmapFrame.Metadata;

                if (bitmapFrame != null)
                {
                    BitmapMetadata metaData = (BitmapMetadata)bitmapFrame.Metadata.Clone();

                    if (metaData != null)
                    {
                        // modify the metadata   
                        metaData.SetQuery("System.Keywords", keywords);

                        // get an encoder to create a new jpg file with the new metadata.      
                        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmapFrame, bitmapFrame.Thumbnail, metaData, bitmapFrame.ColorContexts));
                        //string jpegNewFileName = Path.Combine(jpegDirectory, "JpegTemp.jpg");

                        // Delete the original
                        //originalImage.Delete();
                        originalImage.MoveTo(imageFilePath + "temp");
                        try
                        {
                            // Save the new image 
                            using (Stream jpegStreamOut = File.Open(imageFilePath, FileMode.CreateNew, FileAccess.ReadWrite))
                            {
                                encoder.Save(jpegStreamOut);
                            }
                            File.Delete(imageFilePath + "temp");
                        }
                        catch
                        {
                            File.Move(imageFilePath + "temp", imageFilePath);
                            MessageBox.Show($"Неудалось перезаписать файл {imageFilePath}");
                        }
                    }
                }
            }
        }
        public void LoadImages(ImageList imageList, ListView listView)
        {
            imageList.Images.Clear();
            listView.Items.Clear();
            for (int i = 0; i < fileList.Count; i++)
            {
                string f = fileList[i];
                if (!imageList.Images.ContainsKey(f))
                {
                    Image image = Image.FromFile(f);
                    imageList.Images.Add(f, image);
                    listView.Items.Add(f, "", f).Tag = f;

                    image.Dispose();
                }
                OnProgressChanged?.Invoke((i + 1) / (double)fileList.Count);
            }
        }
        public void AsynsLoadImages(ImageList imageList, ListView listView, double sleepTime)
        {
            listView.Invoke(new Action(() =>
            {
                imageList.Images.Clear();
                listView.Items.Clear();
            }));
            for (int i = 0; i < fileList.Count; i++)
            {
                string f = fileList[i];
                if (!imageList.Images.ContainsKey(f))
                {
                    listView.Invoke(new Action(() =>
                    {
                        Image image = Image.FromFile(f);

                        imageList.Images.Add(f, image);
                        listView.Items.Add(f, "", f).Tag = f;
                        image.Dispose();
                    }));
                }
                OnProgressChanged?.Invoke((i + 1) / (double)fileList.Count);
                Thread.Sleep((int)(sleepTime * 1000));
            }
            OnProcessComplited?.Invoke(true);
        }
        #endregion

        #region standartRealisation
        public string this[int index]
        {
            get
            {
                return ((IList<string>)fileList)[index];
            }

            set
            {
                ((IList<string>)fileList)[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return ((IList<string>)fileList).Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IList<string>)fileList).IsReadOnly;
            }
        }

        public void Add(string item)
        {
            if (!fileList.Contains(item))
            {
                ((IList<string>)fileList).Add(item);
            }
        }

        public void Clear()
        {
            ((IList<string>)fileList).Clear();
        }

        public bool Contains(string item)
        {
            return ((IList<string>)fileList).Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            ((IList<string>)fileList).CopyTo(array, arrayIndex);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IList<string>)fileList).GetEnumerator();
        }

        public int IndexOf(string item)
        {
            return ((IList<string>)fileList).IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            ((IList<string>)fileList).Insert(index, item);
        }

        public bool Remove(string item)
        {
            return ((IList<string>)fileList).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<string>)fileList).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<string>)fileList).GetEnumerator();
        }
        #endregion
    }
    public static class Extantions
    {
        public static void CheckSet(this TreeNode node, TriStatesTreeViewControl.CheckedState state)
        {
            TriStatesTreeViewControl t = node.TreeView as TriStatesTreeViewControl;
            if (t == null)
            {
                node.Checked = state == TriStatesTreeViewControl.CheckedState.Checked;
                node.StateImageIndex = (int)state;
            }
            else
            {
                t.NodeStateSet(node, state);
            }
        }
        public static TriStatesTreeViewControl.CheckedState GetCheckState(this TreeNode node)
        {
            return (TriStatesTreeViewControl.CheckedState)node.SelectedImageIndex;
        }
        public static void TempNodeStateSet(this TreeNode node, int filesCount)
        {
            if ((int)node.Tag == filesCount)
            {
                node.CheckSet(TriStatesTreeViewControl.CheckedState.Checked);
            }
            else if ((int)node.Tag == 0)
            {
                node.CheckSet(TriStatesTreeViewControl.CheckedState.UnChecked);
            }
            else
            {
                node.CheckSet(TriStatesTreeViewControl.CheckedState.Mixed);
            }
        }
    }
    public class ThreadMethod<T>
    {
        public T arg;
        public void StartMethod()
        {
            OriginalMethod.Invoke(arg);
        }
        public ThreadMethod(Action<T> method, T arg)
        {
            OriginalMethod = method;
            this.arg = arg;
        }
        public Action<T> OriginalMethod;
    }
    public class ThreadMethod2<T1, T2>
    {
        public T1 arg1;
        public T2 arg2;
        public void StartMethod()
        {
            OriginalMethod.Invoke(arg1, arg2);
        }
        public ThreadMethod2(Action<T1, T2> method, T1 arg1, T2 arg2)
        {
            OriginalMethod = method;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }
        public Action<T1, T2> OriginalMethod;
    }
    public class ThreadMethod3<T1, T2, T3>
    {
        public T1 arg1;
        public T2 arg2;
        public T3 arg3;
        public void StartMethod()
        {
            OriginalMethod.Invoke(arg1, arg2, arg3);
        }
        public ThreadMethod3(Action<T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3)
        {
            OriginalMethod = method;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
        }
        public Action<T1, T2, T3> OriginalMethod;
    }
    public class ImageComparer : IComparer
    {
        public bool descending;
        public ImageComparer(bool descending = true)
        {
            this.descending = descending;
        }
        public int Compare(object x, object y)
        {
            return Compare(x as ListViewItem, y as ListViewItem);
        }
        private int Compare(ListViewItem x, ListViewItem y)
        {
            DateTime xDate = new FileInfo(x.ImageKey).CreationTime;
            DateTime yDate = new FileInfo(y.ImageKey).CreationTime;
            if(xDate == yDate)
            {
                return 0;
            }
            else if(xDate < yDate)
            {
                return descending ? 1 : -1;
            }
            else
            {
                return descending ? -1 : 1;
            }
        }
    }
}
