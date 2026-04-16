using System;
using System.Drawing;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebScraperApp
{
    public class MainForm : Form
    {
        private TextBox txtUrl;
        private Button btnFetch;
        private RichTextBox txtOutput;
        private static readonly HttpClient client = new HttpClient();

        public MainForm()
        {
            // --- 界面布局初始化 ---
            this.Text = "极简网页数据提取器";
            this.Size = new Size(600, 500);

            txtUrl = new TextBox { Location = new Point(20, 20), Width = 400, Text = "https://www.example.com" };
            btnFetch = new Button { Location = new Point(430, 18), Text = "提取数据", Width = 120 };
            txtOutput = new RichTextBox { Location = new Point(20, 60), Size = new Size(540, 380), Font = new Font("Consolas", 10) };

            this.Controls.Add(txtUrl);
            this.Controls.Add(btnFetch);
            this.Controls.Add(txtOutput);

            // 绑定点击事件
            btnFetch.Click += async (s, e) => await StartExtraction();
        }

        private async Task StartExtraction()
        {
            string url = txtUrl.Text.Trim();
            if (string.IsNullOrEmpty(url)) return;

            btnFetch.Enabled = false;
            txtOutput.Text = "正在努力爬取中...\r\n";

            try
            {
                // 1. 获取网页内容 (添加 User-Agent 模拟浏览器，防止被反爬)
                client.DefaultRequestHeaders.UserAgent.Clear();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
                
                string html = await client.GetStringAsync(url);

                // 2. 正则表达式定义
                // 手机号：匹配1开头的11位数字 (中国大陆)
                string phonePattern = @"1[3-9]\d{9}";
                // 邮箱：通用匹配规则
                string emailPattern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}";

                var phoneMatches = Regex.Matches(html, phonePattern);
                var emailMatches = Regex.Matches(html, emailPattern);

                // 3. 结果整理与显示
                txtOutput.Clear();
                txtOutput.SelectionFont = new Font(txtOutput.Font, FontStyle.Bold);
                txtOutput.AppendText($"[ 手机号码 ] - 发现 {phoneMatches.Count} 个\r\n");
                foreach (Match m in phoneMatches) txtOutput.AppendText(m.Value + "\r\n");

                txtOutput.AppendText("\r\n--------------------------------\r\n");

                txtOutput.SelectionFont = new Font(txtOutput.Font, FontStyle.Bold);
                txtOutput.AppendText($"[ 电子邮箱 ] - 发现 {emailMatches.Count} 个\r\n");
                foreach (Match m in emailMatches) txtOutput.AppendText(m.Value + "\r\n");
            }
            catch (Exception ex)
            {
                txtOutput.Text = $"出错了！\r\n错误原因: {ex.Message}";
            }
            finally
            {
                btnFetch.Enabled = true;
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }
    }
}