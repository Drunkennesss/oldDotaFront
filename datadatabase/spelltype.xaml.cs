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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace datadatabase
{
    /// <summary>
    /// Логика взаимодействия для spelltype.xaml
    /// </summary>
    public partial class spelltype : Page
    {
        private string CurUser;
        private Frame MainFrame;
        private LoginPage login;
        private int id;
        public spelltype()
        {
            InitializeComponent();
        }
        public spelltype(string user, in Frame f, in LoginPage l) : this()
        {
            CurUser = user;
            MainFrame = f;
            login = l;
            var oracle = OraConnect.oracle;
            oracle.Open();
            var comm = oracle.CreateCommand();
            comm.CommandText = "select max(id) from spell_type";
            var read = comm.ExecuteReader();
            read.Read();
            id = read.GetInt32(0) + 1;
            oracle.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var oracle = OraConnect.oracle;
            oracle.Open();
            var comm = oracle.CreateCommand();
            if (typeName.Text != "")
            {        
                
                comm.CommandText = $"insert into spell_type (id, type_name) values ({id}, '{typeName.Text}')";
                comm.Transaction = oracle.BeginTransaction();
                try
                {
                    comm.ExecuteNonQuery();
                    comm.Transaction.Commit();
                    MyLogger.Log.Info($"User: {CurUser} has inserted new spell type with id ={id}, spell_name = {typeName.Text}");
                }catch(Exception ex)
                {
                    MyLogger.Log.Error(ex);
                    comm.Transaction.Rollback();
                }
            }
            comm.CommandText = "select * from spell_type";
            var read = comm.ExecuteReader();
            var dt = new DataTable();
            dt.Load(read);
            DataGrid.ItemsSource = dt.DefaultView;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = login;
        }
    }
}
