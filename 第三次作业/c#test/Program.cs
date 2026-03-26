using System;
using System.Threading;

namespace AlarmClockApp
{
    // 事件参数用来传递时间信息
    public class ClockEventArgs : EventArgs
    {
        public DateTime CurrentTime { get; }
        public ClockEventArgs(DateTime time) => CurrentTime = time;
    }

    // 2. 闹钟类（事件发布者 Publisher）
    public class AlarmClock
    {
        // 定义事件：Tick (嘀嗒) 和 Alarm (响铃)
        public event EventHandler<ClockEventArgs> Tick;
        public event EventHandler<ClockEventArgs> Alarm;

        private DateTime _targetTime;

        public AlarmClock(DateTime targetTime)
        {
            _targetTime = targetTime;
        }

        // 启动闹钟
        public void Start()
        {
            Console.WriteLine($"闹钟已设定为: {_targetTime:HH:mm:ss}\n");

            while (true)
            {
                DateTime now = DateTime.Now;

                // 触发嘀嗒
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

        // 触发事件
        protected virtual void OnTick(ClockEventArgs e)
        {
            
            Tick?.Invoke(this, e);
        }

        protected virtual void OnAlarm(ClockEventArgs e)
        {
            Alarm?.Invoke(this, e);
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            // 设定一个 5 秒后的闹钟
            DateTime alarmTime = DateTime.Now.AddSeconds(5);
            AlarmClock myClock = new AlarmClock(alarmTime);

          
           
            myClock.Tick += ShowTick;
            myClock.Alarm += ShowAlarm;

            
            myClock.Alarm += (sender, e) => {
                Console.Beep(); // 顺便让电脑响一声
            };

            myClock.Start();
            
            Console.WriteLine("\n按下任意键退出...");
            Console.ReadKey();
        }

        // 响应嘀
        static void ShowTick(object sender, ClockEventArgs e)
        {
            Console.WriteLine($"[嘀嗒]: {e.CurrentTime:HH:mm:ss}");
        }

        // 响应响铃
        static void ShowAlarm(object sender, ClockEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n【！！！叮铃铃！！！】时间到了！现在是：" + e.CurrentTime);
            Console.ResetColor();
        }
    }
}
