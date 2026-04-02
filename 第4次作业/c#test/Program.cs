using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace FileMergerApp
{
    // 启动入口
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm()); 
        }
    }

    // 界面类
    public class MainForm : Form
    {
        // 定义控件变量
        private TextBox? txtPath1;
        private TextBox? txtPath2;
        private Button? btnSelectFile1;
        private Button? btnSelectFile2;
        private Button? btnMerge;

        // 【关键点】构造函数：名字必须和类名 MainForm 完全一模一样，且没有返回值类型
        public MainForm()
        {
            InitializeManualUI();
        }

        private void InitializeManualUI()
        {
            this.Text = "文本合并工具";
            this.Size = new Size(500, 350);
            this.StartPosition = FormStartPosition.CenterScreen;

            txtPath1 = new TextBox { Location = new Point(20, 30), Size = new Size(320, 25), ReadOnly = true };
            btnSelectFile1 = new Button { Text = "选择文件1", Location = new Point(360, 28), Size = new Size(100, 30) };
            btnSelectFile1.Click += (s, e) => { txtPath1.Text = SelectTextFile(); };

            txtPath2 = new TextBox { Location = new Point(20, 80), Size = new Size(320, 25), ReadOnly = true };
            btnSelectFile2 = new Button { Text = "选择文件2", Location = new Point(360, 78), Size = new Size(100, 30) };
            btnSelectFile2.Click += (s, e) => { txtPath2.Text = SelectTextFile(); };

            btnMerge = new Button { Text = "执行合并", Location = new Point(180, 160), Size = new Size(120, 50), BackColor = Color.LightBlue };
            btnMerge.Click += BtnMerge_Click;

            this.Controls.Add(txtPath1);
            this.Controls.Add(btnSelectFile1);
            this.Controls.Add(txtPath2);
            this.Controls.Add(btnSelectFile2);
            this.Controls.Add(btnMerge);
        }

        private void BtnMerge_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPath1?.Text) || string.IsNullOrEmpty(txtPath2?.Text))
            {
                MessageBox.Show("请先选择两个文件！");
                return;
            }

            try
            {
                string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                if (!Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);

                string content = File.ReadAllText(txtPath1!.Text) + Environment.NewLine + 
                               "--- 分割线 ---" + Environment.NewLine + 
                               File.ReadAllText(txtPath2!.Text);

                string fullPath = Path.Combine(dataFolder, $"Merged_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
                File.WriteAllText(fullPath, content);
                MessageBox.Show($"成功！文件已保存至：\n{fullPath}");
            }
            catch (Exception ex) { MessageBox.Show("错误: " + ex.Message); }
        }

        private string SelectTextFile()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "文本文件|*.txt";
                return ofd.ShowDialog() == DialogResult.OK ? ofd.FileName : string.Empty;
            }
        }
    }
}