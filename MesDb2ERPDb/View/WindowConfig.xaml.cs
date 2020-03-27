using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MesDb2ERPDb.View
{
    /// <summary>
    /// Interaction logic for WindowConfig.xaml
    /// </summary>
    public partial class WindowConfig : Window
    {
        Model.SettingClass SettingClass = null;
        public WindowConfig()
        {
            InitializeComponent();
            LoadSettingToUI();
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            SettingClass = new Model.SettingClass();
            SettingClass.OfflineServer = txt_serverOffline.Text.Trim();
            SettingClass.userOffline = txt_userOffline.Text.Trim();
            SettingClass.password = passwordBox.Password;
            SettingClass.usingOfftlineServer = (bool)cb_serveroffline.IsChecked;
            try
            {
                SettingClass.TimmerBackgroudWorker = int.Parse(txt_timmer.Text.Trim());
            }
            catch (Exception ex)
            {
                SystemLog.Output(SystemLog.MSG_TYPE.Err, "convert str to int fail", ex.Message);
                SettingClass.TimmerBackgroudWorker = 30000;
            }
            
            
            SaveObject.Save_data(SaveObject.Pathsave, SettingClass);

        }
        private void LoadSettingToUI()
        {
            if (File.Exists(SaveObject.Pathsave))
            {
                SettingClass = new Model.SettingClass();
                SettingClass = (Model.SettingClass)SaveObject.Load_data(SaveObject.Pathsave);            
                txt_serverOffline.Text = SettingClass.OfflineServer;
                txt_userOffline.Text = SettingClass.userOffline;
                cb_serveroffline.IsChecked = SettingClass.usingOfftlineServer;
                passwordBox.Password = SettingClass.password;
                txt_timmer.Text = SettingClass.TimmerBackgroudWorker.ToString();
            }
        }
    }
}
