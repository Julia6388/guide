using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace проводник
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string str;
        private void FillNodes(TreeNode dirNode)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(dirNode.FullPath);            
                foreach (DirectoryInfo dirItem in dir.GetDirectories())
                {
                    //добавляем узел для каждой папки
                    TreeNode newNode = new TreeNode(dirItem.Name);
                    dirNode.Nodes.Add(newNode);
                    newNode.Nodes.Add("*");
                }
                foreach (FileInfo dirItem in dir.GetFiles())
                {
                    // Добавляем узел для каждого файла
                    TreeNode newNode = new TreeNode(dirItem.Name);
                    dirNode.Nodes.Add(newNode);
                    newNode.Nodes.Add("*");
                }
            }
            catch (IOException)
            {
                if (str != null && str.Equals("")) // существует ли адрес и проверка на пустой путь
                    Process.Start(str);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] MyLogicalDrives = System.IO.Directory.GetLogicalDrives();            
            foreach (string disk in MyLogicalDrives) // динамическое нахождение дисков
            {
                TreeNode rootNode = new TreeNode(@disk);
                treeView1.Nodes.Add(rootNode);
                FillNodes(rootNode);
            }
        }
        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            //Если найден узел со *, то удаляем его
            // и получаем список подпапок
            if ( e.Node.Nodes[0].Text == "*")
            {
                e.Node.Nodes.Clear();
                str = e.Node.FullPath;
                FillNodes(e.Node);
            }
        }
        string path = "";
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // При нажатие на файл или папку в списке запускаем или открываем его
            if (path != null && !path.Equals(""))
                Process.Start(@listBox1.Items[listBox1.SelectedIndex].ToString());
        }
        private void treeView1_Click(object sender, EventArgs e)
        {
            //Перекидываем на список все файлы и папки
            try
            {
                listBox1.Items.Clear();
                path = treeView1.SelectedNode.FullPath;
                string[] astrFiles = System.IO.Directory.GetFiles(@path);
                listBox1.Items.Add("Всего файлов:  " + astrFiles.Length);

                foreach (string file in astrFiles)
                    listBox1.Items.Add(file);
            }
            catch { }
        }
    }
}
