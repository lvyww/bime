using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace bime
{
    /// <summary>
    /// WinConfig.xaml 的交互逻辑
    /// </summary>
    public partial class WinConfig : Window
    {

        class Setting
        {
            public TextBlock Key;
            public UIElement Value;

            public Setting(TextBlock Key, UIElement Value)
            {
                this.Key = Key;
                this.Value = Value;
            }

            public string GetKey()
            {
                return this.Key.Text;
            }
            public string GetValue()
            {
                if (Value.GetType() == typeof(TextBox))
                {
                    var tb = (TextBox)Value;
                    return tb.Text;
                }
                else if (Value.GetType() == typeof(CheckBox))
                {
                    var tb = (CheckBox)Value;
                    if (tb.IsChecked == true)
                        return "是";
                    else
                        return "否";
                }
                else if (Value.GetType() == typeof(ComboBox))
                {
                    var tb = (ComboBox)Value;
                    if (tb.SelectedIndex < 0)
                        return "";
                    else
                    {
                        ComboBoxItem cb = (ComboBoxItem)tb.SelectedItem;
                        return cb.Content.ToString();
                    }

                }
                else if (Value.GetType() == typeof(StackPanel))
                {
                    var tb = (StackPanel)Value;

                    foreach (RadioButton r in tb.Children)
                    {
                        if (r.IsChecked == true)
                            return r.Content.ToString();

                    }

                }

                return "";
            }
        }

        List<Setting> settingList = new List<Setting>();
        public WinConfig()
        {
            InitializeComponent();
        }

        private void InitFontFamily()
        {

            DirectoryInfo dr = new DirectoryInfo("字体");
            if (!dr.Exists)
                dr.Create();

            CultureInfo cn = CultureInfo.GetCultureInfo("zh-CN");
            CultureInfo en = CultureInfo.GetCultureInfo("en-US");

            foreach (var f in dr.GetFiles("*.ttf"))
            {
                var fullname = f.FullName;

                GlyphTypeface gf = new GlyphTypeface(new Uri(fullname));
                var s = gf.FamilyNames;
                //       var b =  gf.FontUri.ToString();

                string fontname = "";


                if (s.ContainsKey(cn))
                    fontname = s[cn];
                else if (s.ContainsKey(en))
                    fontname = s[en];


                if (fontname != "")
                {




                    ComboBoxItem cbi = new ComboBoxItem();



                    string currentPath = AppDomain.CurrentDomain.BaseDirectory;
                    Uri uri = new Uri(currentPath + "字体\\");
                    FontFamily fm = new FontFamily(uri, "./#" + fontname);
                    cbi.FontFamily = fm;
                    cbi.FontSize = Config.GetDouble("字体大小");
                    cbi.Content = "#" + fontname;

                    CbFonts.Items.Add(cbi);
                }



            }





            foreach (FontFamily fontfamily in Fonts.SystemFontFamilies)
            {
                LanguageSpecificStringDictionary lsd = fontfamily.FamilyNames;
                if (lsd.ContainsKey(XmlLanguage.GetLanguage("zh-cn")))
                {
                    string fontname = null;
                    if (lsd.TryGetValue(XmlLanguage.GetLanguage("zh-cn"), out fontname))
                    {
                        ComboBoxItem cbi = new ComboBoxItem();
                        cbi.FontFamily = new FontFamily(fontname);
                        cbi.FontSize = Config.GetDouble("字体大小");
                        cbi.Content = fontname;

                        CbFonts.Items.Add(cbi);
                    }
                }
                else
                {
                    string fontname = null;
                    if (lsd.TryGetValue(XmlLanguage.GetLanguage("en-us"), out fontname))
                    {
                        ComboBoxItem cbi = new ComboBoxItem();
                        cbi.FontFamily = new FontFamily(fontname);
                        cbi.FontSize = Config.GetDouble("字体大小");
                        cbi.Content = fontname;
                        CbFonts.Items.Add(cbi);

                        //CbFonts.Items.Add(fontname);
                    }
                }
            }

            bool selected = false;
            for (int i = 0; i < CbFonts.Items.Count; i++)
            {

                ComboBoxItem cbi = CbFonts.Items[i] as ComboBoxItem;
                if (Config.GetString("字体") == cbi.Content.ToString())
                {
                    CbFonts.SelectedIndex = i;
                    selected = true;
                }

            }
            if (!selected)
            {
                CbFonts.SelectedIndex = 0;
            }

        }

        private void InitRadioButton()
        {
            foreach (RadioButton r in SpPageKey.Children)
            {
                if (r.Content.ToString() == Config.GetString("翻页键"))
                    r.IsChecked = true;


            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] SkipedConfigItems = { "字体", "当前码表", "翻页键", "四码唯一自动上屏", "Ctrl+I开启录制", "主题", "Ctrl+U开启录制", "右Ctrl或右Alt开启/禁用Bime" };//   "嵌入式候选（娱乐）", "开始菜单使用嵌入式候选", "否" };

            InitFontFamily();
            InitRadioButton();
            settingList.Add(new Setting(TbFonts, CbFonts));
            settingList.Add(new Setting(TbPageKey, SpPageKey));

            int RowCounter = GridMain.RowDefinitions.Count;
            var mg = new Thickness(10, 3, 10, 3);
            foreach (var item in Config.dicts)
            {

                if (SkipedConfigItems.Contains(item.Key))
                    continue;

                GridMain.RowDefinitions.Add(new RowDefinition());

                TextBlock tbk = new TextBlock();
                tbk.Text = item.Key;


                //       tbk.HorizontalAlignment = HorizontalAlignment.Center;
                tbk.VerticalAlignment = VerticalAlignment.Center;
                tbk.SetValue(Grid.RowProperty, RowCounter);
                tbk.SetValue(Grid.ColumnProperty, 0);
                tbk.Margin = mg;
                tbk.FontSize = 14;

                GridMain.Children.Add(tbk);


                if (item.Value == "是" || item.Value == "否")
                {
                    CheckBox tbv = new CheckBox();
                    tbv.IsChecked = item.Value == "是";
                    tbv.VerticalAlignment = VerticalAlignment.Center;
                    tbv.SetValue(Grid.RowProperty, RowCounter);
                    tbv.SetValue(Grid.ColumnProperty, 1);
                    tbv.Margin = mg;
                    tbv.FontSize = 14;

                    GridMain.Children.Add(tbv);

                    settingList.Add(new Setting(tbk, tbv));
                }
                else
                {
                    TextBox tbv = new TextBox();
                    tbv.Text = item.Value;
                    //       tbv.HorizontalAlignment = HorizontalAlignment.Center;
                    tbv.VerticalAlignment = VerticalAlignment.Center;
                    tbv.SetValue(Grid.RowProperty, RowCounter);
                    tbv.SetValue(Grid.ColumnProperty, 1);
                    tbv.Margin = mg;
                    tbv.FontSize = 14;

                    GridMain.Children.Add(tbv);
                    settingList.Add(new Setting(tbk, tbv));
                }



                RowCounter++;
            }


        }



        private void Cancel_Click(object sender, RoutedEventArgs e)
        {


            this.Close();
        }


        private void Save_Click_old(object sender, RoutedEventArgs e)
        {
            List<string> key = new List<string>();
            List<string> value = new List<string>();
            foreach (var item in GridMain.Children)
            {

                if (item.GetType() == typeof(TextBlock))
                {
                    var tb = (TextBlock)item;
                    key.Add(tb.Text);
                }
                if (item.GetType() == typeof(TextBox))
                {
                    var tb = (TextBox)item;
                    value.Add(tb.Text);
                }
                else if (item.GetType() == typeof(CheckBox))
                {
                    var tb = (CheckBox)item;
                    if (tb.IsChecked == true)
                        value.Add("是");
                    else
                        value.Add("否");
                }


            }

            bool modified = false;
            for (int i = 0; i < key.Count; i++)
            {

                if (value[i] != Config.GetString(key[i]))
                {
                    modified = true;
                    Config.Set(key[i], value[i]);
                }

            }
            if (modified)
            {

                MainWindow.Current?.ReloadCfg();

            }
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            bool modified = false;
            foreach (var s in settingList)
            {
                string newValue = s.GetValue();
                //if (newValue != "" && newValue != Config.GetString(s.GetKey()))
                if (newValue != Config.GetString(s.GetKey()))
                {
                    modified = true;
                    Config.Set(s.GetKey(), newValue);
                }

            }


            if (modified)
            {

                MainWindow.Current?.ReloadCfg();
                Candi.UpdateWindow();
            }
        }

        public static WinConfig Current
        {
            get
            {
                foreach (var s in App.Current.Windows)
                {
                    if (s is WinConfig)
                    {
                        return (WinConfig)s;

                    }

                }

                return null;
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool modified = false;
            foreach (var s in settingList)
            {
                string newValue = s.GetValue();
                if (newValue != "" && newValue != Config.GetString(s.GetKey()))
                {
                    modified = true;
                    //       Config.Set(s.GetKey(), newValue);
                }

            }


            if (modified)
            {
                if (MessageBox.Show("设置已修改，是否保存？",
                                    "保存设置",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    foreach (var s in settingList)
                    {
                        string newValue = s.GetValue();
                        if (newValue != "" && newValue != Config.GetString(s.GetKey()))
                        {
                            //modified = true;
                            Config.Set(s.GetKey(), newValue);
                        }

                    }

                    MainWindow.Current?.ReloadCfg();
                    Candi.UpdateWindow();

                }
            }
        }
    }
}

