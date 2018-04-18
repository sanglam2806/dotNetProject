using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT_32_102140091_LamQuangSang
{
    public partial class Form1 : Form
    {

        TreeView trview;
        ImageList imgl;
        Button btopen;
        TextBox tblink;
        ListView listView;
        FolderBrowserDialog folderBrowserDialog;
        ContextMenu menu;
        MenuItem menuViewList, menuViewDetail,
            menuViewLargeIcon, menuViewSmallIcon,
            menuNewFolder, menuNewText, menuNewDoc;

        public Form1()
        {
            //InitializeComponent
            this.Text = "Bai tap 32 by Tim";
            this.Size = new Size(800, 600);

            btopen = new Button();
            btopen.Text = "Open";
            btopen.Location = new Point(10, 10);
            btopen.Click += openLink;
            this.Controls.Add(btopen);

            tblink = new TextBox();
            tblink.Location = new Point(90, 10);
            tblink.Size = new Size(400, 10);
            this.Controls.Add(tblink);

            trview = new TreeView();
            trview.Size = new Size(200, 550);
            trview.Location = new Point(10, 35);
            trview.AfterSelect += TreeSelectedHandler;
            this.Controls.Add(trview);

            listView = new ListView();
            listView.Location = new Point(220, 30);
            listView.Size = new Size(550, 550);
            listView.Columns.Add("name");
            listView.Columns.Add("date create");
            listView.Columns.Add("typeFile");
            listView.Columns[0].Width = 200;
            listView.Columns[1].Width = 150;
            listView.Columns[2].Width = 100;
            listView.View = View.Details;
           // listView.MouseDown += rightClickHandler;
            this.Controls.Add(listView);

            imgl = new ImageList();
            Image folderImage = Image.FromFile(@"img\folder.jpg");
            Image fileImage = Image.FromFile(@"img\file.jpg");
            imgl.Images.Add("folder", folderImage);
            imgl.Images.Add("file", fileImage);
            trview.ImageList = imgl;
            listView.SmallImageList = imgl;
            listView.LargeImageList = imgl;

        }
        private void openLink(Object o, EventArgs e)
        {
            folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                String path = folderBrowserDialog.SelectedPath;
                trview.Nodes.Clear();
                TreeNode node = new TreeNode(path);
                this.trview.Nodes.Add(node);
                Update(node);
                trview.Text = path;
            }
        }

        private void TreeSelectedHandler(Object o, TreeViewEventArgs e)
        {
            tblink.Text = e.Node.FullPath;
            if (trview.SelectedNode.Nodes.Count == 0
                && trview.SelectedNode.SelectedImageKey.Equals("folder"))
                Update(trview.SelectedNode);
        }

        private void Update(TreeNode node)
        {
            listView.Items.Clear();
            AddDir(node);
            AddFile(node);
            node.Expand();
        }

        private void AddDir(TreeNode node)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(node.FullPath);

            try
            {
                foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                {
                    TreeNode objNode = new TreeNode(dir.Name);
                    objNode.ImageKey = "folder";
                    objNode.SelectedImageKey = "folder";
                    node.Nodes.Add(objNode);

                    ListViewItem item = new ListViewItem(dir.Name);
                    item.ImageKey = "folder";
                    item.SubItems.Add(dir.LastWriteTime.ToShortDateString());
                    item.SubItems.Add("folder");
                    listView.Items.Add(item);
                }
            }
            catch { }

        }

        private void AddFile(TreeNode node)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(node.FullPath);

            try
            {
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    TreeNode subNode = new TreeNode(file.Name);
                    subNode.ImageKey = "file";
                    subNode.SelectedImageKey = "file";
                    node.Nodes.Add(subNode);

                    ListViewItem item = new ListViewItem(file.Name);
                    item.ImageKey = "file";
                    item.SubItems.Add(file.LastWriteTime.ToShortDateString());
                    item.SubItems.Add("file");
                    listView.Items.Add(item);
                }
            }
            catch { }
        }
    }
}
