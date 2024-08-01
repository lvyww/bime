using System;
using System.ServiceModel;
using System.Windows;

namespace bime
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private ServiceHost _serviceHost;
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (States.SwitchedToEn)
                if (Config.GetBool("自动切换系统语言"))
                    ImiHelper.ChangeLanguageEnabeled();

            Config.WriteConfig();
        }


        private static System.Threading.Mutex mutex;
        //系统能够识别有名称的互斥，因此可以使用它禁止应用程序启动两次 
        //第二个参数可以设置为产品的名称:Application.ProductName 
        // 每次启动应用程序，都会验证名称为OnlyRun的互斥是否存在 
        protected override void OnStartup(StartupEventArgs e)
        {


            mutex = new System.Threading.Mutex(true, "BimeOnlyRun");
            if (mutex.WaitOne(0, false))
            {
                base.OnStartup(e);

                _serviceHost = new ServiceHost(typeof(MMService), new Uri("net.pipe://localhost"));
                _serviceHost.AddServiceEndpoint(typeof(IMMService), new NetNamedPipeBinding(), "MMService");
                _serviceHost.Open();

                return;
            }

            var arguments = Environment.GetCommandLineArgs();

            if (arguments.Length <= 1)
            {
                MessageBox.Show("Bime已经在运行！", "提示");
                this.Shutdown();
                return;
            }


            var argument = arguments[1];
            ChannelFactory<IMMService> channelFactory = new ChannelFactory<IMMService>(new NetNamedPipeBinding(), "net.pipe://localhost/MMService");
            var proxy = channelFactory.CreateChannel();


            if (argument.Equals("reload", StringComparison.InvariantCultureIgnoreCase))
            {



                //        MessageBox.Show("重载!");

                proxy.ReloadMB();

            }

            if (argument.Equals("config", StringComparison.InvariantCultureIgnoreCase))
            {



                //        MessageBox.Show("重载!");

                proxy.ShowConfigWin();

            }

            if (argument.Equals("addci", StringComparison.InvariantCultureIgnoreCase))
            {



                //        MessageBox.Show("重载!");

                proxy.AddCi();

            }

            if (argument.Length > 3 && argument.Substring(0, 3) == "mb_")
            {
                string mb = argument.Substring(3);
                proxy.ChangeMB(mb);


            }

            App.Current.Shutdown();
            return;



        }

    }
}
