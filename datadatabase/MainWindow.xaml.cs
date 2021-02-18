using System;
using System.Collections.Generic;
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
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.EntityFramework;
using Oracle.ManagedDataAccess.Types;
using NLog;

namespace datadatabase
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new LoginPage(in MainFrame);
        }       
    }

    public static class OraConnect
    {
        private const string con = "Data Source=(DESCRIPTION="
       + "(ADDRESS=(PROTOCOL=TCP)(PORT=1521))"
       + "(CONNECT_DATA=(SERVICE_NAME=xe)));"
       + "User Id=alex;Password=qwerty;";
        public static readonly OracleConnection oracle;
        static OraConnect()
        {
            oracle = new OracleConnection(con);
        }
    }
    public static class MyLogger
    {
        public static readonly NLog.Logger Log;
        private static NLog.Config.LoggingConfiguration config;
        private static NLog.Targets.FileTarget logfile;
        static MyLogger()
        {
            config = new NLog.Config.LoggingConfiguration();
            logfile = new NLog.Targets.FileTarget("logfile")
            {
                FileName = "MyLog.log",
                Layout = @"${longdate} | ${level} | ${message} | ${exception}"
            };

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;

            Log = NLog.LogManager.GetCurrentClassLogger();
        }
    }
}
