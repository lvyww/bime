using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace bime
{

    static internal class Config
    {
        static public Dictionary<string, string> dicts = new Dictionary<string, string>();
        static public string Path = "config.txt";

        static public void SetDefault(params string[] args)
        {
            for (int i = 0; i + 1 < args.Length; i += 2)
            {
                dicts[args[i]] = args[i + 1];
            }

        }


        static private Timer WriteTimer = null;


        static private void WriteNow(object obj)
        {

            if (Path == "")
                return;

            try
            {
                StreamWriter sw = new StreamWriter(Path);

                foreach (var c in dicts)
                {
                    sw.WriteLineAsync(c.Key + "\t" + c.Value);
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

        static public void WriteConfig(int Delay = 0)
        {

            if (Path == "")
                return;

            try
            {
                if (Delay == 0)
                {
                    if (WriteTimer != null)
                    {
                        WriteTimer.Dispose();
                        WriteTimer = null;
                    }

                    StreamWriter sw = new StreamWriter(Path);

                    foreach (var c in dicts)
                    {
                        sw.WriteLineAsync(c.Key + "\t" + c.Value);
                    }


                    sw.Close();
                }
                else if (Delay > 0)
                {
                    if (WriteTimer == null)
                    {
                        WriteTimer = new Timer(WriteNow, null, Delay, Timeout.Infinite);
                    }
                    else
                    {
                        WriteTimer.Dispose();
                        WriteTimer = new Timer(WriteNow, null, Delay, Timeout.Infinite);
                        //    WriteTimer.Change(Delay, Timeout.Infinite);

                    }
                }
            }
            catch (Exception)
            {


            }
            finally { }


        }

        static public void ReadConfig()
        {
            //     char[] sp = { '\r', ' ', '\t' };

            if (!File.Exists(Path))
            {
                WriteConfig();
                return;
            }

            char[] sp1 = { '\n' };

            string[] lines = File.ReadAllText(Path).Split(sp1, StringSplitOptions.RemoveEmptyEntries);


            foreach (string line in lines)
            {
                if (line.Substring(0, 1) == "#")
                    continue;
                string line_p = line.Replace("\r", "").Replace("\n", "");

                string[] sp = { "\t", " ", "," };



                foreach (string s in sp)
                {
                    if (line_p.Contains(s))
                    {
                        int pos = line_p.IndexOf(s);
                        if (pos >= 1 && pos <= line_p.Length - 2)
                        {
                            string key = line_p.Substring(0, pos);
                            string value = line_p.Substring(pos + 1);
                            dicts[key] = value;
                            break;
                        }
                    }
                }



            }


            WriteConfig();



        }

        static public bool GetBool(string key)
        {
            if (dicts.ContainsKey(key) && dicts[key] == "是")
                return true;
            else
                return false;
        }
        static public string GetString(string key)
        {
            if (dicts.ContainsKey(key))
                return dicts[key];
            else
                return "";
        }

        static public int GetInt(string key)
        {
            if (dicts.ContainsKey(key) && Int32.TryParse(dicts[key], out int num))
                return num;
            else
                return 0;
        }


        static public double GetDouble(string key)
        {
            if (dicts.ContainsKey(key) && Double.TryParse(dicts[key], out double num))
                return num;
            else
                return 0;
        }

        static public void Set(string key, bool value)
        {
            if (value)
                dicts[key] = "是";
            else
                dicts[key] = "否";

            WriteConfig(3000);
        }
        static public void Set(string key, int value)
        {
            dicts[key] = value.ToString();
            WriteConfig(3000);
        }

        static public void Set(string key, string value)
        {
            dicts[key] = value;
            WriteConfig(3000);
        }

        static public void Set(string key, double value, int fraction = -1)
        {
            string f = "F" + fraction.ToString();
            if (fraction > 0)
                dicts[key] = value.ToString(f);
            else
                dicts[key] = value.ToString();

            WriteConfig(3000);

        }
    }

}
