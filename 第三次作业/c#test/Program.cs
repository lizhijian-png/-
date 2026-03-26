using System;
using System.Threading;

namespace AlarmClockApp
{
    // 1. 自定义事件参数类，用来传递时间信息
    public class ClockEventArgs : EventArgs
    {
        public DateTime CurrentTime { get; }
        public ClockEventArgs(DateTime time) => CurrentTime = time;
    }

    // 2. 闹钟类（事件发布者 Publisher）
    public class AlarmClock
    {
        // 定义事件：Tick (嘀嗒) 和 Alarm (响铃)
        // EventHandler<T> 是 C# 标准的事件委托
        public event EventHandler<ClockEventArgs> Tick;
        public event EventHandler<ClockEventArgs> Alarm;

        private DateTime _targetTime;

        public AlarmClock(DateTime targetTime)
        {
            _targetTime = targetTime;
        }

        // 启动闹钟的方法
        public void Start()
        {
            Console.WriteLine($"闹钟已设定为: {_targetTime:HH:mm:ss}\n");

            while (true)
            {
                DateTime now = DateTime.Now;

                // 触发嘀嗒事件
                OnTick(new ClockEventArgs(now));

                // 检查是否到达预设时间
                if (now.Hour == _targetTime.Hour && 
                    now.Minute == _targetTime.Minute && 
                    now.Second == _targetTime.Second)
                {
                    OnAlarm(new ClockEventArgs(now));
                    break; // 响铃后停止模拟
                }

                Thread.Sleep(1000); // 模拟每秒跳动
            }
        }

        // 触发事件的规范方法 (通常以 On+事件名 命名)
        protected virtual void OnTick(ClockEventArgs e)
        {
            // ?.Invoke 是为了防止没有订阅者时出现空引用异常
            Tick?.Invoke(this, e);
        }

        protected virtual void OnAlarm(ClockEventArgs e)
        {
            Alarm?.Invoke(this, e);
        }
    }

    // 3. 客户端（事件订阅者 Subscriber）
    class Program
    {
        static void Main(string[] args)
        {
            // 设定一个 5 秒后的闹钟
            DateTime alarmTime = DateTime.Now.AddSeconds(5);
            AlarmClock myClock = new AlarmClock(alarmTime);

            // --- 订阅事件 ---
            // 使用 += 符号将方法绑定到事件上
            myClock.Tick += ShowTick;
            myClock.Alarm += ShowAlarm;

            // 也可以使用 Lambda 表达式简写订阅
            myClock.Alarm += (sender, e) => {
                Console.Beep(); // 顺便让电脑响一声
            };

            myClock.Start();
            
            Console.WriteLine("\n按下任意键退出...");
            Console.ReadKey();
        }

        // 响应嘀嗒的方法
        static void ShowTick(object sender, ClockEventArgs e)
        {
            Console.WriteLine($"[嘀嗒]: {e.CurrentTime:HH:mm:ss}");
        }

        // 响应响铃的方法
        static void ShowAlarm(object sender, ClockEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n【！！！叮铃铃！！！】时间到了！现在是：" + e.CurrentTime);
            Console.ResetColor();
        }
    }
}
