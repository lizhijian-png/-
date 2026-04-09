using System;
using System.Drawing;
using System.Windows.Forms;

namespace CalculatorApp
{
    public class CalculatorForm : Form
    {
        private TextBox txtDisplay;
        private double resultValue = 0;
        private string operationPerformed = "";
        private bool isOperationPerformed = false;
        private string firstOperandStr = "";

        public CalculatorForm()
        {
            // 设置窗体属性
            this.Text = "C# 计算器 (VS Code 版)";
            this.Size = new Size(320, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // 1. 初始化文本框
            txtDisplay = new TextBox
            {
                Location = new Point(10, 10),
                Size = new Size(285, 50),
                Font = new Font("Arial", 18, FontStyle.Bold),
                Text = "0",
                TextAlign = HorizontalAlignment.Right,
                ReadOnly = true
            };
            this.Controls.Add(txtDisplay);

            // 2. 定义按钮布局
            string[] buttons = {
                "7", "8", "9", "/",
                "4", "5", "6", "*",
                "1", "2", "3", "-",
                "C", "0", "=", "+"
            };

            int x = 10, y = 70;
            for (int i = 0; i < buttons.Length; i++)
            {
                Button btn = new Button
                {
                    Text = buttons[i],
                    Size = new Size(65, 65),
                    Location = new Point(x, y),
                    Font = new Font("Arial", 12, FontStyle.Bold)
                };

                // 绑定事件
                if ("0123456789".Contains(btn.Text))
                    btn.Click += Button_Click;
                else if (btn.Text == "C")
                    btn.Click += Clear_Click;
                else if (btn.Text == "=")
                    btn.Click += Equal_Click;
                else
                    btn.Click += Operator_Click;

                this.Controls.Add(btn);

                // 计算下一个按钮的位置
                x += 75;
                if ((i + 1) % 4 == 0)
                {
                    x = 10;
                    y += 75;
                }
            }
        }

        // 数字点击
        private void Button_Click(object sender, EventArgs e)
        {
            if (txtDisplay.Text == "0" || isOperationPerformed)
                txtDisplay.Clear();

            isOperationPerformed = false;
            Button btn = (Button)sender;
            txtDisplay.Text += btn.Text;
        }

        // 运算符点击
        private void Operator_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            operationPerformed = btn.Text;
            resultValue = double.Parse(txtDisplay.Text);
            firstOperandStr = txtDisplay.Text;
            isOperationPerformed = true;
        }

        // 清空
        private void Clear_Click(object sender, EventArgs e)
        {
            txtDisplay.Text = "0";
            resultValue = 0;
            operationPerformed = "";
        }

        // 等于
        private void Equal_Click(object sender, EventArgs e)
        {
            double secondValue = double.Parse(txtDisplay.Text);
            string secondOperandStr = txtDisplay.Text;
            double finalResult = 0;

            switch (operationPerformed)
            {
                case "+": finalResult = resultValue + secondValue; break;
                case "-": finalResult = resultValue - secondValue; break;
                case "*": finalResult = resultValue * secondValue; break;
                case "/": 
                    if (secondValue != 0) finalResult = resultValue / secondValue;
                    else { txtDisplay.Text = "错误"; return; }
                    break;
                default: return;
            }

            // 显示格式: 18+5=23
            txtDisplay.Text = $"{firstOperandStr}{operationPerformed}{secondOperandStr}={finalResult}";
            isOperationPerformed = true; // 结果显示后，按数字会直接清空开启新运算
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CalculatorForm());
        }
    }
}