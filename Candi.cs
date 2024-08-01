using System;
using System.Collections.Generic;

namespace bime
{
    static internal class Candi
    {
        public static List<string> All = new List<string>();


        //    public static int PageCount = 1;
        //     public static int CurrentPage = 0;
        private static int currentPage = 0;
        public static string Code = "";

        private static int PageSize
        {
            get
            {
                return Math.Max(Math.Min(Config.GetInt("每页候选个数"), 9), 1);
            }
        }

        public static string ConvertCandi(string text)
        {
            //     string msg = text;
            string[] WeekName = new string[] { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };

            switch (text)
            {
                case "。":
                    if (States.LastNum)
                        return ".";
                    else
                        return text;
                case "{重复上屏}":
                    return States.RepeatBuffer;
                case "{日期}":
                    return System.DateTime.Now.ToString("yyyy年MM月dd日");
                case "{日期.}":
                    return System.DateTime.Now.ToString("yyyy.MM.dd");
                case "{日期-}":
                    return System.DateTime.Now.ToString("yyyy-MM-dd");
                case "{日期/}":
                    return System.DateTime.Now.ToString("yyyy/MM/dd");
                case "{时分秒}":
                    return System.DateTime.Now.ToString("HH:mm:ss");
                case "{时分}":
                    return System.DateTime.Now.ToString("HH:mm");
                case "{星期}":
                    return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
                case "{周}":
                    return WeekName[(int)DateTime.Now.DayOfWeek];
                case "{嵌入模式}":;
                    return "嵌入模式";
                case "{录制}":;
                    return "录制";
                case "{隐藏候选}":
                    return "隐藏候选";



                default:
                    return text;
            }

        }
        public static string ConvertEmb(string text)
        {
            return text.Replace("/", "／");

        }

        public static int TotalCandiCount
        {
            get { return All.Count; }
        }

        public static void NextPage()
        {
            int start = currentPage * PageSize;

            if (start + PageSize >= TotalCandiCount)
                return;

            currentPage++;
        }

        public static void PrevPage()
        {
            int start = currentPage * PageSize;

            if (start - PageSize < 0)
                return;

            currentPage--;
        }

        public static void Update(string code, List<string> candis)
        {
            Code = code;

            if (candis != null)
                All = candis;
            else
                All = new List<string>();

            currentPage = 0;

            UpdateWindow();

        }

        public static void UpdateWindow()
        {
            if (Config.GetBool("嵌入式候选（娱乐）") || (States.IsStartMenu && Config.GetBool("开始菜单使用嵌入式候选"))) //嵌入式
            {
                MainWindow.Current?.UpdateCandidateEmb();
            }
            else
            {
                WinCandidate.Current?.Update();
            }




        }

        public static List<string> Current
        {
            get
            {
                int start = currentPage * PageSize;

                int count = Math.Min(PageSize, TotalCandiCount - start);

                return All.GetRange(start, count);
            }

        }


    }
}
