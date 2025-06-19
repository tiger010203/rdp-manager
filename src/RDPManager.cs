using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RDPManager
{
    // 服务器信息结构
    public struct ServerInfo
    {
        public string IP;
        public string Port;
        public string Username;
        public string Password;
    }

    public class ZeroWriteRDPManager
    {
        // Windows API 导入
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        // 常量定义
        private const int HOTKEY_ID = 1;
        private const int MOD_CONTROL = 0x0002;
        private const int MOD_SHIFT = 0x0004;
        private const int VK_R = 0x52;
        private const int SW_HIDE = 0;
        private const int TIMEOUT_SECONDS = 10;

        // 成员变量
        private List<ServerInfo> servers = new List<ServerInfo>();
        private bool monitoring = false;
        private DateTime lastActivity = DateTime.Now;
        private Thread monitorThread;
        private bool running = true;
        private Form hiddenForm;

        public void Start()
        {
            // 隐藏控制台窗口
            IntPtr consoleWindow = GetConsoleWindow();
            if (consoleWindow != IntPtr.Zero)
            {
                ShowWindow(consoleWindow, SW_HIDE);
            }

            // 创建隐藏窗体用于接收热键消息
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            hiddenForm = new HiddenForm(this);
            
            // 注册全局热键
            RegisterHotKey(hiddenForm.Handle, HOTKEY_ID, MOD_CONTROL | MOD_SHIFT, VK_R);

            // 运行消息循环
            Application.Run(hiddenForm);
        }

        public void ShowInputDialog()
        {
            try
            {
                string domain = Microsoft.VisualBasic.Interaction.InputBox(
                    "请输入服务器信息域名:", "远程桌面管理器", "");

                if (!string.IsNullOrEmpty(domain))
                {
                    Task.Run(async () =>
                    {
                        if (await FetchServersAsync(domain))
                        {
                            Application.Invoke(() => ShowServerList());
                        }
                        else
                        {
                            MessageBox.Show("获取服务器信息失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                // 静默处理异常
            }
        }

        private async Task<bool> FetchServersAsync(string url)
        {
            try
            {
                ClearServers();

                using (var client = new HttpClient())
                {
                    // 禁用缓存
                    client.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
                    {
                        NoCache = true
                    };

                    string content = await client.GetStringAsync(url);
                    return ParseServers(content);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ParseServers(string content)
        {
            try
            {
                string[] lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(':');
                    if (parts.Length >= 4)
                    {
                        ServerInfo server = new ServerInfo
                        {
                            IP = parts[0].Trim(),
                            Port = parts[1].Trim(),
                            Username = parts[2].Trim(),
                            Password = parts[3].Trim()
                        };
                        servers.Add(server);
                    }
                }

                return servers.Count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ShowServerList()
        {
            try
            {
                if (servers.Count == 0) return;

                StringBuilder sb = new StringBuilder("服务器列表:\n\n");
                for (int i = 0; i < servers.Count; i++)
                {
                    sb.AppendLine($"{i + 1}. {servers[i].IP}:{servers[i].Port} - {servers[i].Username}");
                }
                sb.AppendLine($"\n请输入服务器编号 (1-{servers.Count}):");

                string input = Microsoft.VisualBasic.Interaction.InputBox(sb.ToString(), "服务器列表", "");

                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= servers.Count)
                {
                    ConnectToServer(servers[choice - 1]);
                }

                // 开始活动监控
                StartActivityMonitor();
            }
            catch (Exception)
            {
                // 静默处理异常
            }
        }

        private void ConnectToServer(ServerInfo server)
        {
            try
            {
                // 使用纯内存RDP连接
                // 这里实现零写入的RDP连接逻辑
                
                string message = $"正在连接到 {server.IP}:{server.Port}\n用户: {server.Username}";
                MessageBox.Show(message, "RDP连接", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 实际的RDP连接实现
                // 可以使用第三方库如 FreeRDP.NET 等
            }
            catch (Exception)
            {
                // 静默处理异常
            }
        }

        private void StartActivityMonitor()
        {
            monitoring = true;
            lastActivity = DateTime.Now;

            monitorThread?.Join();
            monitorThread = new Thread(ActivityMonitor) { IsBackground = true };
            monitorThread.Start();
        }

        private void ActivityMonitor()
        {
            POINT lastMousePos;
            GetCursorPos(out lastMousePos);

            while (monitoring)
            {
                try
                {
                    // 检查鼠标活动
                    POINT currentMousePos;
                    GetCursorPos(out currentMousePos);

                    if (currentMousePos.X != lastMousePos.X || currentMousePos.Y != lastMousePos.Y)
                    {
                        lastActivity = DateTime.Now;
                        lastMousePos = currentMousePos;
                    }

                    // 检查键盘活动
                    for (int vk = 8; vk < 256; vk++)
                    {
                        if ((GetAsyncKeyState(vk) & 0x8000) != 0)
                        {
                            lastActivity = DateTime.Now;
                            break;
                        }
                    }

                    // 检查超时
                    if ((DateTime.Now - lastActivity).TotalSeconds > TIMEOUT_SECONDS)
                    {
                        ForceCloseAll();
                        break;
                    }

                    Thread.Sleep(100); // 100ms检查间隔
                }
                catch (Exception)
                {
                    // 静默处理异常
                }
            }
        }

        private void ForceCloseAll()
        {
            try
            {
                monitoring = false;
                ClearServers();

                // 关闭所有相关窗口和进程
                // 这里可以添加更多的清理逻辑
            }
            catch (Exception)
            {
                // 静默处理异常
            }
        }

        private void ClearServers()
        {
            try
            {
                // 清空敏感信息
                for (int i = 0; i < servers.Count; i++)
                {
                    ServerInfo server = servers[i];
                    server.IP = null;
                    server.Port = null;
                    server.Username = null;
                    server.Password = null;
                    servers[i] = server;
                }
                servers.Clear();

                // 强制垃圾回收
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch (Exception)
            {
                // 静默处理异常
            }
        }

        public void Cleanup()
        {
            try
            {
                running = false;
                monitoring = false;

                UnregisterHotKey(hiddenForm?.Handle ?? IntPtr.Zero, HOTKEY_ID);

                monitorThread?.Join(1000);
                ClearServers();
            }
            catch (Exception)
            {
                // 静默处理异常
            }
        }
    }

    // 隐藏窗体类
    public class HiddenForm : Form
    {
        private const int WM_HOTKEY = 0x0312;
        private ZeroWriteRDPManager manager;

        public HiddenForm(ZeroWriteRDPManager manager)
        {
            this.manager = manager;
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Visible = false;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == 1)
            {
                manager.ShowInputDialog();
            }
            base.WndProc(ref m);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            manager.Cleanup();
            base.OnFormClosed(e);
        }
    }

    // 程序入口点
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            try
            {
                ZeroWriteRDPManager manager = new ZeroWriteRDPManager();
                manager.Start();
            }
            catch (Exception)
            {
                // 静默处理异常
            }
        }
    }
}

