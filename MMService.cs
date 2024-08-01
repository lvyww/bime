


namespace bime
{
    internal class MMService : IMMService
    {

        private MainWindow GetMainWindow()
        {

            foreach (var s in App.Current.Windows)
            {
                if (s is MainWindow)
                {
                    return (MainWindow)s;
                }

            }

            return null;

        }
        public void ReloadMB()
        {
            GetMainWindow().ReloadMB();
        }

        public void ShowConfigWin()
        {
            GetMainWindow().ShowConfigWin();
        }

        public void AddCi()
        {
            GetMainWindow().ShowWinAddCi();
        }
        public void ChangeMB(string mb)
        {
            GetMainWindow().ChangeMB(mb);

        }
    }
}
