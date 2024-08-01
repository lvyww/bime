using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace bime
{


    public class Theme
    {
        public SolidColorBrush Foreground;
        public SolidColorBrush Background;
        public SolidColorBrush Border;
        public string Name;
        public Thickness BorderWidth;

        public CornerRadius Corner = new CornerRadius(5);
        public Theme(string name, string foreground, string background, string border, double borderWidth)
        {
            Name = name;
            Foreground = FromString( foreground);
            Background = FromString( background);
            Border = FromString( border);
            BorderWidth = new Thickness( borderWidth);
            Foreground.Freeze();
            Background.Freeze();
            Border.Freeze();
           

        }


        public Theme(string name, string foreground, string background, string border, double b1, double b2, double b3, double b4)
        {
            Name = name;
            Foreground = FromString(foreground);
            Background = FromString(background);
            Border = FromString(border);
            BorderWidth = new Thickness(b1, b2, b3, b4);
            Foreground.Freeze();
            Background.Freeze();
            Border.Freeze();


        }

        public Theme(string name, string foreground, string background, string border, double b1, double b2, double b3, double b4, double c1, double c2, double c3, double c4)
        {
            Name = name;
            Foreground = FromString(foreground);
            Background = FromString(background);
            Border = FromString(border);
            BorderWidth = new Thickness(b1, b2, b3, b4);
            Corner = new CornerRadius (c1, c2,c3, c4);
            Foreground.Freeze();
            Background.Freeze();
            Border.Freeze();


        }

        public string ForegroundString
        {
            get
            {
                return Foreground.ToString();
            }
        }


        public string BackgroundString
        {
            get
            {
                return Background.ToString();
            }
        }
        public string BorderString
        {
            get
            {
                return Border.ToString();
            }
        }
        public static SolidColorBrush FromString(string str)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + str));

        }
    }
    internal static class Themes
    {
        public static Dictionary <string, Theme> ThemeList = new Dictionary <string, Theme> ();

        public static int CurrentHour = 0;

        static Themes()
        {
            InitThemeList();
        }

        private static void InitThemeList()
        {
            ThemeList.Add("清晨", new Theme("清晨", "FF303030", "FFFDFDFF", "FF56A1DD", 1.25));
            ThemeList.Add("默认", new Theme("默认", "FF000000", "FFFFF8F3", "FF1A7B6B", 1.25));
            ThemeList.Add("星夜", new Theme("星夜", "FFFFDC6A", "FF232B39", "FF3A6B9B", 1.25)); //暗夜
            ThemeList.Add("纸", new Theme("纸", "FF111111", "FFf5f2e8", "FFa8a09d", 1.3)); //起点
            ThemeList.Add("粉", new Theme("粉", "FF000000", "FFFdF9F5", "ffDeacac", 1.25)); //粉
            ThemeList.Add("赛博朋克", new Theme("赛博朋克", "Ff71e4fd", "88001122", "Fff651fc", 1.5,1.5,1.5,1.5,10,0,10,0)); //暗
        }



        public static Theme CurrentTheme()
        {
            if (!ThemeList.ContainsKey(Config.GetString("主题")))
                Config.Set("主题", "默认");

            return ThemeList[Config.GetString("主题")];
        }
    }
}
