using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO.Pipes;
using System.IO;

namespace TagManager
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args?.Length > 0)
            {
                if (args[0] == "add")
                {
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Directory").OpenSubKey("Background").OpenSubKey("shell", true).CreateSubKey("TagManager");
                    key.SetValue("", "Изменить теги всех файлов в папке");
                    Microsoft.Win32.RegistryKey commandkey = key.CreateSubKey("command");
                    commandkey.SetValue("", Application.ExecutablePath + " %V");
                    key.SetValue("Icon", Application.UserAppDataPath + @"/contextIcon.ico");


                    key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("jpegfile").OpenSubKey("shell", true).CreateSubKey("TagManager");
                    key.SetValue("", "Изменить теги");
                    commandkey = key.CreateSubKey("command");
                    commandkey.SetValue("", Application.ExecutablePath + " %1");
                    key.SetValue("Icon", Application.UserAppDataPath + @"/contextIcon.ico");


                    Application.Exit();
                }
                else if (args[0] == "remove")
                {
                    Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Directory").OpenSubKey("Background").OpenSubKey("shell", true).DeleteSubKeyTree("TagManager");
                    Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("jpegfile").OpenSubKey("shell", true).DeleteSubKeyTree("TagManager");

                    Application.Exit();
                }
                else
                {
                    Run(args);
                }
            }
            else
            {
                Run(args);
            }
        }
        public static Singleton singleton;
        static void Run(string[] args)
        {
            singleton = new Singleton(new Guid("0f1fad5b-d9cb-462f-a115-70267728350e"), args);
            if (singleton.isFirst)
            {
                if (args == null || args.Length == 0) { args = new string[1] { "" }; }
                RunInterface(args[0]);
            }
            else
            {
                Application.Exit();
            }
        }
        static void RunInterface(string arg)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(arg));
        }
    }
    class Singleton:IDisposable
    {
        Guid identify = Guid.Empty;
        Mutex mutex;
        SynchronizationContext synsContext;
        public bool isFirst { get; private set; }
        public Singleton(Guid identify, string[] args)
        {
            this.identify = identify;
            bool isFirst;
            mutex = new Mutex(true, identify.ToString(), out isFirst);
            this.isFirst = isFirst;
            this.synsContext = SynchronizationContext.Current;
            if (isFirst)
            {
                ListenAsyns();
            }
            else
            {
                NotifyFirst(args,false);
            }
        }
        private void Listen()
        {
            try
            {
                var server = new NamedPipeServerStream(identify.ToString());
                var reader = new StreamReader(server);
                server.WaitForConnection();
                var args = new List<string>();
                while (server.IsConnected)
                {
                    args.Add(reader.ReadLine());
                }
                //synsContext.Post(o => this.OnOtherInstanceCreated(args.ToArray()), null);
                OtherInstanceCreated(this, args.ToArray());
                reader.Close();
                server.Close();

                Listen();
            }
            catch(IOException)
            {
                Listen();
            }
        }
        private void ListenAsyns()
        {
            Task.Factory.StartNew(Listen, TaskCreationOptions.LongRunning);
        }
        private void NotifyFirst(string[] args, bool ignoreException)
        {
            Thread.Sleep(1000);
            try
            {
                var client = new NamedPipeClientStream(identify.ToString());
                var writer = new StreamWriter(client);
                client.Connect(2000);
                if(args?.Length > 0)
                {
                    foreach(var a in args) { writer.WriteLine(a); }
                }
                writer.Close();
                client.Close();
            }
            catch(IOException)
            {
                
            }
            catch(TimeoutException)
            {

            }
        }

        protected virtual void OnOtherInstanceCreated(string[] arguments)
        {
            EventHandler<string[]> handler = OtherInstanceCreated;

            if (handler != null)
            {
                handler(this, arguments);
            }
        }

        public void Dispose()
        {
            if (mutex != null && isFirst)
            {
                mutex.WaitOne();
                mutex.ReleaseMutex();
                mutex = null;
            }
        }

        public event EventHandler<string[]> OtherInstanceCreated;

    }
}
