using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace bime
{
    internal static class SendHistory
    {
        private static Stack<string> history = new Stack<string>();
        private static List<string> rec = new List<string>();
        static public bool recording = false;
        static private string FolderPath = "录制";
        const int HourThresh = 6;




        private static string FilePath
        {
            get
            {
                int hour = DateTime.Now.Hour;

                string date = "";
                if (hour < HourThresh)
                    date = DateTime.Now.AddDays(-1).ToString("yyyy_MM_dd");
                else
                    date = DateTime.Now.ToString("yyyy_MM_dd");

                return FolderPath + "/" + date + ".txt";
            }
        }

        public static bool IsRecording
        {
            get { return recording; }
        }

        public static string GetLastCi(int len)
        {
            len = Math.Min(history.Count, len);

            if (len <= 0)
                return "";

 
            string rt = "";
            for (int i = len -1; i>= 0; i--)
                rt += history.ToArray()[i];

            return rt;
        }
        public static void StartStop()
        {
            recording = !recording;

            if (recording)
            {
                rec.Clear();
                if (File.Exists(FilePath))
                {
                    StringInfo si = new StringInfo(File.ReadAllText(FilePath));
                    for (int i = 0; i < si.LengthInTextElements; i++)
                    {
                        rec.Add(si.SubstringByTextElements(i, 1));
                    }
                }
            }
            else
            {
                Save();
            }
        }


        public static void Push(string str)
        {
            history.Push(str);

            if (IsRecording)
            {
                rec.Add(str);
                if (States.IsDebug)
                    Trace.Write(str + "\n");
                Save();
            }

        }

        public static string Pop()
        {
            if (IsRecording)
            {
                if (rec.Count > 0)
                    rec.RemoveAt(rec.Count - 1);
                Save();
            }


            return history.Pop();
        }
        public static void Clear()
        {
            history.Clear();
        }
        public static int Count
        {
            get { return history.Count; }
        }

        public static string First()
        {
            return history.First();
        }

        static SendHistory()
        {
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }
        }


        static private Timer WriteTimer = null;


        static private void SaveNow(object obj)
        {



            try
            {
                StreamWriter sw = new StreamWriter(FilePath);




                foreach (var c in rec)
                {
                    sw.Write(c);
                }


                sw.Close();
                if (WriteTimer != null)
                {
                    WriteTimer.Dispose();
                    WriteTimer = null;
                }
            }
            catch (Exception)
            {


            }
            finally
            {

            }




        }

        static public void Save(int Delay = 3000)
        {


            try
            {
                if (Delay == 0)
                {
                    SaveNow(null);
                }
                else if (Delay > 0)
                {
                    if (WriteTimer == null)
                    {
                        WriteTimer = new Timer(SaveNow, null, Delay, Timeout.Infinite);
                    }
                    else
                    {
                        WriteTimer.Dispose();
                        WriteTimer = new Timer(SaveNow, null, Delay, Timeout.Infinite);


                    }
                }
            }
            catch (Exception)
            {


            }
            finally { }


        }


    }
}
