using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace bime
{
    internal static class AutoSpace
    {
        public static Timer tm = null;

        static  public Dictionary<string, int> KeyPairs = new Dictionary<string, int> {

             {"qq", 56},
{"qw", 59},
{"qe", 39},
{"qr", 21},
{"qt", 26},
{"qa", 68},
{"qs", 76},
{"qd", 44},
{"qf", 19},
{"qg", 25},
{"qz", 95},
{"qx", 78},
{"qc", 64},
{"qv", 31},
{"qb", 33},
{"wq", 48},
{"ww", 27},
{"we", 19},
{"wr", 12},
{"wt", 24},
{"wa", 43},
{"ws", 42},
{"wd", 31},
{"wf", 16},
{"wg", 20},
{"wz", 83},
{"wx", 57},
{"wc", 62},
{"wv", 23},
{"wb", 26},
{"eq", 35},
{"ew", 24},
{"ee", 26},
{"er", 12},
{"et", 28},
{"ea", 33},
{"es", 34},
{"ed", 62},
{"ef", 16},
{"eg", 18},
{"ez", 73},
{"ex", 63},
{"ec", 63},
{"ev", 24},
{"eb", 34},
{"rq", 28},
{"rw", 13},
{"re", 16},
{"rr", 30},
{"rt", 52},
{"ra", 22},
{"rs", 22},
{"rd", 32},
{"rf", 58},
{"rg", 64},
{"rz", 60},
{"rx", 67},
{"rc", 71},
{"rv", 64},
{"rb", 60},
{"tq", 25},
{"tw", 16},
{"te", 16},
{"tr", 49},
{"tt", 36},
{"ta", 20},
{"ts", 18},
{"td", 32},
{"tf", 52},
{"tg", 58},
{"tz", 47},
{"tx", 55},
{"tc", 66},
{"tv", 78},
{"tb", 59},
{"aq", 64},
{"aw", 32},
{"ae", 31},
{"ar", 18},
{"at", 19},
{"aa", 34},
{"as", 64},
{"ad", 34},
{"af", 18},
{"ag", 24},
{"az", 96},
{"ax", 72},
{"ac", 61},
{"av", 21},
{"ab", 36},
{"sq", 72},
{"sw", 51},
{"se", 26},
{"sr", 17},
{"st", 25},
{"sa", 67},
{"ss", 53},
{"sd", 20},
{"sf", 12},
{"sg", 19},
{"sz", 86},
{"sx", 82},
{"sc", 62},
{"sv", 20},
{"sb", 29},
{"dq", 55},
{"dw", 28},
{"de", 60},
{"dr", 27},
{"dt", 23},
{"da", 24},
{"ds", 30},
{"dd", 30},
{"df", 12},
{"dg", 22},
{"dz", 43},
{"dx", 50},
{"dc", 57},
{"dv", 31},
{"db", 43},
{"fq", 33},
{"fw", 20},
{"fe", 17},
{"fr", 47},
{"ft", 53},
{"fa", 13},
{"fs", 13},
{"fd", 15},
{"ff", 26},
{"fg", 75},
{"fz", 59},
{"fx", 52},
{"fc", 77},
{"fv", 48},
{"fb", 54},
{"gq", 36},
{"gw", 31},
{"ge", 21},
{"gr", 72},
{"gt", 86},
{"ga", 32},
{"gs", 26},
{"gd", 18},
{"gf", 51},
{"gg", 47},
{"gz", 55},
{"gx", 33},
{"gc", 39},
{"gv", 84},
{"gb", 86},
{"zq", 98},
{"zw", 91},
{"ze", 77},
{"zr", 39},
{"zt", 61},
{"za", 72},
{"zs", 69},
{"zd", 59},
{"zf", 30},
{"zg", 30},
{"zz", 52},
{"zx", 52},
{"zc", 43},
{"zv", 32},
{"zb", 52},
{"xq", 86},
{"xw", 73},
{"xe", 85},
{"xr", 70},
{"xt", 34},
{"xa", 43},
{"xs", 70},
{"xd", 44},
{"xf", 31},
{"xg", 25},
{"xz", 56},
{"xx", 30},
{"xc", 32},
{"xv", 22},
{"xb", 25},
{"cq", 78},
{"cw", 60},
{"ce", 66},
{"cr", 72},
{"ct", 73},
{"ca", 39},
{"cs", 47},
{"cd", 59},
{"cf", 36},
{"cg", 33},
{"cz", 54},
{"cx", 39},
{"cc", 32},
{"cv", 21},
{"cb", 36},
{"vq", 63},
{"vw", 34},
{"ve", 32},
{"vr", 61},
{"vt", 66},
{"va", 25},
{"vs", 35},
{"vd", 27},
{"vf", 47},
{"vg", 65},
{"vz", 28},
{"vx", 28},
{"vc", 25},
{"vv", 26},
{"vb", 39},
{"bq", 57},
{"bw", 35},
{"be", 39},
{"br", 60},
{"bt", 76},
{"ba", 33},
{"bs", 32},
{"bd", 31},
{"bf", 52},
{"bg", 73},
{"bz", 32},
{"bx", 31},
{"bc", 31},
{"bv", 60},
{"bb", 33},
{"yy", 32},
{"yu", 59},
{"yi", 22},
{"yo", 27},
{"yp", 33},
{"yh", 58},
{"yj", 54},
{"yk", 37},
{"yl", 32},
{"yn", 59},
{"ym", 64},
{"uy", 67},
{"uu", 28},
{"ui", 23},
{"uo", 12},
{"up", 21},
{"uh", 64},
{"uj", 51},
{"uk", 41},
{"ul", 29},
{"un", 62},
{"um", 78},
{"iy", 40},
{"iu", 17},
{"ii", 26},
{"io", 27},
{"ip", 29},
{"ih", 17},
{"ij", 12},
{"ik", 40},
{"il", 52},
{"in", 27},
{"im", 34},
{"oy", 29},
{"ou", 15},
{"oi", 34},
{"oo", 38},
{"op", 59},
{"oh", 28},
{"oj", 17},
{"ok", 61},
{"ol", 67},
{"on", 26},
{"om", 29},
{"py", 34},
{"pu", 22},
{"pi", 55},
{"po", 93},
{"pp", 83},
{"ph", 36},
{"pj", 29},
{"pk", 77},
{"pl", 99},
{"pn", 31},
{"pm", 35},
{"hy", 59},
{"hu", 53},
{"hi", 16},
{"ho", 17},
{"hp", 27},
{"hh", 28},
{"hj", 55},
{"hk", 22},
{"hl", 22},
{"hn", 57},
{"hm", 75},
{"jy", 73},
{"ju", 46},
{"ji", 16},
{"jo", 16},
{"jp", 22},
{"jh", 50},
{"jj", 29},
{"jk", 11},
{"jl", 16},
{"jn", 52},
{"jm", 55},
{"ky", 34},
{"ku", 29},
{"ki", 48},
{"ko", 47},
{"kp", 57},
{"kh", 21},
{"kj", 11},
{"kk", 25},
{"kl", 33},
{"kn", 22},
{"km", 23},
{"ly", 43},
{"lu", 25},
{"li", 46},
{"lo", 55},
{"lp", 78},
{"lh", 24},
{"lj", 11},
{"lk", 32},
{"ll", 41},
{"ln", 20},
{"lm", 29},
{"ny", 68},
{"nu", 57},
{"ni", 21},
{"no", 24},
{"np", 36},
{"nh", 65},
{"nj", 43},
{"nk", 26},
{"nl", 23},
{"nn", 32},
{"nm", 73},
{"my", 69},
{"mu", 62},
{"mi", 30},
{"mo", 22},
{"mp", 37},
{"mh", 59},
{"mj", 41},
{"mk", 17},
{"ml", 20},
{"mn", 55},
{"mm", 31},

        };

        public static void ClearTimer()
        {
            if (tm != null)
            {
                tm.Dispose();
                tm = null;
            }

        }

        public static void SetTimer(long time, TimerCallback callback)
        {
            if (tm != null)
            {
                tm.Dispose();
                tm = null;
            }

            tm = new Timer(callback, null, time, Timeout.Infinite);
        }

        
        static AutoSpace()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            const int LR = 30;

            for (int i = 0; i < chars.Length; i++) 
            {
                for (int j = 0; j < chars.Length; j++)
                {
                    string ij = chars.Substring(i,1) + chars.Substring(j, 1);

                    if (!KeyPairs.ContainsKey(ij))
                        KeyPairs.Add(ij, LR);
                }
            }

            ReadKeys();
        }

        


        static class Consts
        {
            public const double Simp1SpaceDang = 20;
            public const double d_huji = 7;
            public const double d_ilegal = 999;



            public const double bad_2s = 1.3;

            public const double r22 = 1; //二简

            public const double c_kuaijian = 0.3;

            public const double r31 = 2; //一简
            public const double r32 = 2; //二简
            public const double r33 = 1; //一简

            public const double r41 = 2; //一简
            public const double r42 = 1.5; //二简
            public const double r43 = 1.1; //一简

            public const double r_key_weight = 30; //key weight

            public const double r_kua1 = 0.32;
            public const double r_kua2 = 0.15;

        }

        class Key
        {
            public string Name;
            public string Finger;
            public string Side;
            public string Col;
            public string Row;
            public double Weight;

            public Key(string name, string finger, string side, string col, string row, double Weight)
            {
                this.Name = name;
                this.Finger = finger;
                this.Side = side;
                this.Col = col;
                this.Row = row;
                this.Weight = Weight;

            }

            public Key(string name, string finger, string side, string col, string row, string weight)
            {
                this.Name = name;
                this.Finger = finger;
                this.Side = side;
                this.Col = col;
                this.Row = row;

                double.TryParse(weight, out double w);
                this.Weight = w;

            }

        }


        static Dictionary<string, Key> Keys = new Dictionary<string, Key>();
        const string StrGlob = ".";

        const string CharLut = "_abcdefghijklmnopqrstuvwxyz" + StrGlob;
   //     static Dictionary<string, double> KeyPairs = new Dictionary<string, double>();

        static void ReadKeys()
        {
            string[] lines =
            {
                "_,0,0,0,100,0", "a,1,1,1,2,23", "b,4,1,5,1,20", "c,3,1,3,1,20", "d,3,1,3,2,10", "e,3,1,3,3,12", "f,4,1,4,2,10", "g,4,1,5,2,14", "h,7,2,6,2,14", "i,8,2,8,3,11", "j,7,2,7,2,9", "k,8,2,8,2,9", "l,9,2,9,2,15", "m,7,2,7,1,18", "n,7,2,6,1,17", "o,9,2,9,3,17", "p,10,2,10,3,27", "q,1,1,1,3,28", "r,4,1,4,3,14", "s,2,1,2,2,15", "t,4,1,5,3,20", "u,7,2,7,3,12", "v,4,1,4,1,16", "w,2,1,2,3,14", "x,2,1,2,1,25", "y,7,2,6,3,22", "z,1,1,1,1,35", ".,1,1,1,1,20"

            };
            foreach (var l in lines)
            {

                string line = l;
                if (line.Length > 1 && line.Substring(0, 1) == "#")
                    continue;

                line = line.Replace("\r", "");
                var ls = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (ls.Length < 6)
                    continue;

                Keys.Add(ls[0], new Key(ls[0], ls[1], ls[2], ls[3], ls[4], ls[5]));
            }
        }


        static void Proc3()
        {
            StreamWriter sw = new StreamWriter("dang3.txt");
            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    for (int k = 0; k < 28; k++)
                    {
                        string code = CharLut.Substring(i, 1) + CharLut.Substring(j, 1) + CharLut.Substring(k, 1);
                        string dang = Amp(CalDang3(code)).ToString("F0");
                        sw.WriteLine($"{code}\t{dang}");
                    }


                }
            }


            sw.Close();


        }

        static void Proc4()
        {
            StreamWriter sw = new StreamWriter("dang4.txt");
            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    for (int k = 0; k < 28; k++)
                    {
                        for (int l = 0; l < 28; l++)
                        {
                            string code = CharLut.Substring(i, 1) + CharLut.Substring(j, 1) + CharLut.Substring(k, 1) + CharLut.Substring(l, 1);
                            string dang = Amp(CalDang4(code)).ToString("F0");
                            sw.WriteLine($"{code}\t{dang}");
                        }

                    }


                }
            }


            sw.Close();


        }


        static int Amp(double val)
        {

            double tmp = (val - 100.0) / 100.0;

            tmp = Math.Min(3, Math.Max(0, tmp));

            return (int)(val * (1 + tmp));

            return (int)val;


            return (int)Math.Round(Math.Pow(val + 100, 1.5) - Math.Pow(100, 1.5), 0);
        }

        static double bad3(double value, string code)
        {



            const double k3 = 2;
            const double f3 = 2;
            const double s3 = 1.5;

            const double base_k3 = 70;
            const double base_f3 = 70;
            const double base_s3 = 35;

            string a = code.Substring(0, 1);
            string b = code.Substring(1, 1);
            string c = code.Substring(2, 1);
            if (Keys[a].Name == Keys[b].Name && Keys[a].Name == Keys[c].Name) //同键
                return value * k3 + base_k3;
            else if (Keys[a].Finger == Keys[b].Finger && Keys[a].Finger == Keys[c].Finger) //同指
                return value * f3 + base_f3;
            else if (Keys[a].Side == Keys[b].Side && Keys[a].Side == Keys[c].Side) //同边
                return value * s3 + base_s3;
            else
                return value;

        }

        static double bad43(double value, string code)
        {

            const double k3 = 2;
            const double f3 = 2;
            const double s3 = 1.5;

            const double base_k3 = 70;
            const double base_f3 = 70;
            const double base_s3 = 35;

            string a = code.Substring(0, 1);
            string b = code.Substring(1, 1);
            string c = code.Substring(2, 1);
            if (Keys[a].Name == Keys[b].Name && Keys[a].Name == Keys[c].Name) //同键
                return value * k3 + base_k3;
            else if (Keys[a].Finger == Keys[b].Finger && Keys[a].Finger == Keys[c].Finger) //同指
                return value * f3 + base_f3;
            else if (Keys[a].Side == Keys[b].Side && Keys[a].Side == Keys[c].Side) //同边
                return value * s3 + base_s3;
            else
                return value;

        }


        static double bad43_kua(double value, string code)
        {

            const double k3 = 1.25;
            const double f3 = 1.25;
            const double s3 = 1.15;

            const double base_k3 = 20;
            const double base_f3 = 20;
            const double base_s3 = 15;

            string a = code.Substring(0, 1);
            string b = code.Substring(1, 1);
            string c = code.Substring(2, 1);
            if (Keys[a].Name == Keys[b].Name && Keys[a].Name == Keys[c].Name) //同键
                return value * k3 + base_k3;
            else if (Keys[a].Finger == Keys[b].Finger && Keys[a].Finger == Keys[c].Finger) //同指
                return value * f3 + base_f3;
            else if (Keys[a].Side == Keys[b].Side && Keys[a].Side == Keys[c].Side) //同边
                return value * s3 + base_s3;
            else
                return value;

        }


        static double bad44(double value, string code)
        {
            const double k4 = 4;
            const double k3f4 = 3.6;
            const double f4 = 3.4;
            const double k3 = 3.2;
            const double f3 = 3.2;
            const double s4 = 3;

            const double base_k4 = 80;
            const double base_k3f4 = 65;
            const double base_f4 = 60;
            const double base_k3 = 55;
            const double base_f3 = 55;
            const double base_s4 = 50;


            string a = code.Substring(0, 1);
            string b = code.Substring(1, 1);
            string c = code.Substring(2, 1);
            string d = code.Substring(3, 1);


            bool isK4 = Keys[a].Name == Keys[b].Name && Keys[a].Name == Keys[c].Name && Keys[a].Name == Keys[d].Name;
            bool isK3 = Keys[a].Name == Keys[b].Name && Keys[a].Name == Keys[c].Name && Keys[a].Name != Keys[d].Name
                || Keys[d].Name == Keys[b].Name && Keys[d].Name == Keys[c].Name && Keys[a].Name != Keys[d].Name;

            bool isF4 = Keys[a].Finger == Keys[b].Finger && Keys[a].Finger == Keys[c].Finger && Keys[a].Finger == Keys[d].Finger;
            bool isF3 = Keys[a].Finger == Keys[b].Finger && Keys[a].Finger == Keys[c].Finger && Keys[a].Finger != Keys[d].Finger
                || Keys[d].Finger == Keys[b].Finger && Keys[d].Finger == Keys[c].Finger && Keys[a].Finger != Keys[d].Finger;

            bool isS4 = Keys[a].Side == Keys[b].Side && Keys[a].Side == Keys[c].Side && Keys[a].Side == Keys[d].Side;

            if (isK4)
                return value * k4 + base_k4;
            else if (isF4 && isK3)
                return value * k3f4 + base_k3f4;
            else if (isF4)
                return value * f4 + base_f4;
            else if (isK3)
                return value * k3 + base_k3;
            else if (isF3)
                return value * f3 + base_f3;
            else if (isS4)
                return value * s4 + base_s4;
            else
                return 1;

        }

        public static double CalDang2(string code)
        {
            return CalDang2_in(code) * Consts.r22;
        }
        public static double CalDang2_in(string code)
        {
            if (code == "j.")
                ;
            string a = code.Substring(0, 1);
            string b = code.Substring(1, 1);
            if (!IsLegal(code))
                return Consts.d_ilegal;

            else if (a == StrGlob)
            {
                double sum = 0;


                for (int i = 0; i < 27; i++)
                {

                    sum += CalDang2(CharLut[i].ToString() + b);
                }
                return sum / 27;
            }
            else if (b == StrGlob)
            {
                if (code == "j.")
                    ;
                double sum = 0;


                for (int i = 0; i < 27; i++)
                {

                    sum += CalDang2(a + CharLut[i].ToString());
                }
                return sum / 27;
            }
            else if (code.Contains("_")) //
                return Consts.d_huji;
            else if (Keys[a].Side != Keys[b].Side)
                return Consts.d_huji;
            else
                return KeyPairs[a + b];




        }


        public static double CalDang3(string code)
        {
            string a = code.Substring(0, 1);
            string b = code.Substring(1, 1);
            string c = code.Substring(2, 1);

            if (!IsLegal(code)) //非法
                return Consts.d_ilegal;
            else if (code.Substring(1, 2) == "__") //1简
                return (Consts.Simp1SpaceDang) * Consts.r31;

            else if (code.Substring(2, 1) == "_")//2简
                return CalDang2(code.Substring(0, 2));
            else if (a == StrGlob)
            {
                double sum = 0;

                for (int i = 0; i < 27; i++)
                {

                    sum += CalDang3(CharLut[i].ToString() + b + c);
                }
                return sum / 27;
            }
            else if (b == StrGlob)
            {
                double sum = 0;


                for (int i = 0; i < 27; i++)
                {

                    sum += CalDang3(a + CharLut[i].ToString() + c);
                }
                return sum / 27;
            }
            else if (c == StrGlob)
            {
                double sum = 0;


                for (int i = 0; i < 27; i++)
                {

                    sum += CalDang3(a + b + CharLut[i].ToString());
                }
                return sum / 27;
            }
            else
            {


                if (Keys[a].Side == Keys[b].Side && Keys[a].Side == Keys[c].Side) //aaa
                {
                    return bad3(KeyPairs[a + b] + KeyPairs[b + c] + KeyPairs[a + c] * Consts.c_kuaijian, code);
                }
                else if (Keys[a].Side == Keys[b].Side && Keys[a].Side != Keys[c].Side) //aab
                {
                    return KeyPairs[a + b] + Consts.d_huji;
                }
                else if (Keys[a].Side != Keys[b].Side && Keys[a].Side != Keys[c].Side) //abb
                {
                    return KeyPairs[b + c] + Consts.d_huji;
                }
                else if (Keys[a].Side != Keys[b].Side && Keys[a].Side == Keys[c].Side) //aba
                {
                    return Consts.d_huji + Consts.d_huji + KeyPairs[a + c] * Consts.c_kuaijian;
                }

            }



            return 0;
        }

        public static double CalDang4(string code)
        {
            string a = code.Substring(0, 1);
            string b = code.Substring(1, 1);
            string c = code.Substring(2, 1);
            string d = code.Substring(3, 1);
            var ka = Keys[a];
            var kb = Keys[b];
            var kc = Keys[c];
            var kd = Keys[d];

            var sa = ka.Side;
            var sb = kb.Side;
            var sc = kc.Side;
            var sd = kd.Side;


            if (!IsLegal(code)) //非法
                return Consts.d_ilegal;
            else if (code.Substring(1, 1) == "_") //1简
                return (Consts.Simp1SpaceDang) * Consts.r41;

            else if (code.Substring(2, 1) == "_")//2简
                return CalDang2(code.Substring(0, 2)) * Consts.r42;
            else if (code.Substring(3, 1) == "_") //3简
                return CalDang3(code.Substring(0, 3)) * Consts.r43;
            else if (a == StrGlob)
            {
                double sum = 0;

                for (int i = 0; i < 27; i++)
                {

                    sum += CalDang4(CharLut[i].ToString() + b + c + d);
                }
                return sum / 27;
            }
            else if (b == StrGlob)
            {
                double sum = 0;


                for (int i = 0; i < 27; i++)
                {

                    sum += CalDang4(a + CharLut[i].ToString() + c + d);
                }
                return sum / 27;
            }
            else if (c == StrGlob)
            {
                double sum = 0;


                for (int i = 0; i < 27; i++)
                {

                    sum += CalDang4(a + b + CharLut[i].ToString() + d);
                }
                return sum / 27;
            }
            else if (d == StrGlob)
            {
                double sum = 0;


                for (int i = 0; i < 27; i++)
                {

                    sum += CalDang4(a + b + c + CharLut[i].ToString());
                }
                return sum / 27;
            }
            else
            {

                double rt = GetKp(a, b) + GetKp(b, c) + GetKp(c, d);
                if (sa == sb && sa == sc && sa == sd) //aaaa
                    rt = bad44(rt, code);
                else if (sa == sb && sa == sc && sa != sd) //aaab
                    rt = bad43(rt, code.Substring(0, 3));
                else if (sa != sb && sa != sc && sa != sd) //abbb
                    rt = bad43(rt, code.Substring(1, 3));
                else if (sa == sb && sa != sc && sa == sd) //aaba
                    rt = bad43_kua(rt + GetKp(b, d) * Consts.r_kua1, a + b + d);
                else if (sa != sb && sa == sc && sa == sd) //abaa
                    rt = bad43_kua(rt + GetKp(a, c) * Consts.r_kua1, a + c + d);
                else if (sa == sb && sa != sc && sa != sd) //aabb
                    ;
                else if (sa != sb && sa != sc && sa == sd) //abba
                    rt += GetKp(a, d) * Consts.r_kua2;
                else if (sa != sb && sa == sc && sa != sd) //abab
                    rt += GetKp(a, c) * Consts.r_kua1 + GetKp(b, d) * Consts.r_kua1;



                return rt;

            }



            return 0;
        }

        static double GetKp(string a, string b)
        {
            if (!Keys.ContainsKey(a) || !Keys.ContainsKey(b))
                return Consts.d_ilegal;
            else if (Keys[a].Side == Keys[b].Side)
                return KeyPairs[a + b];
            else
                return Consts.d_huji;
        }
        static void Proc2()
        {

            StreamWriter sw = new StreamWriter("dang2.txt");
            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    string code = CharLut.Substring(i, 1) + CharLut.Substring(j, 1);
                    string dang = Math.Round(CalDang2(code), 0).ToString("F0");
                    sw.WriteLine($"{code}\t{dang}");

                }
            }


            sw.Close();

        }



        static bool IsLegal(string code)
        {

            int pos = code.IndexOf("_");
            if (pos == -1)
                return true;

            string remain = code.Substring(pos);

            return remain == new string('_', remain.Length);

        }


    }
}
