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
using System.Data;

namespace datadatabase
{
    /// <summary>
    /// Логика взаимодействия для talents.xaml
    /// </summary>
    public partial class talents : Page
    {
        private string CurUser;
        private Frame MainFrame;
        private LoginPage login;

        public talents()
        {
            InitializeComponent();
        }
        public talents(string user, in Frame main, in LoginPage l) : this()
        {
            CurUser = user;
            MainFrame = main;
            login = l;
            OraConnect.oracle.Open();
            var comm = OraConnect.oracle.CreateCommand();
            comm.CommandText = "select max(id) from heroes";
            var read = comm.ExecuteReader();
            read.Read();
            var max = read.GetInt32(0);
            Hero_id.Maximum = max;
            OraConnect.oracle.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var oracle = OraConnect.oracle;
            oracle.Open();
            var comm = oracle.CreateCommand();
            comm.CommandText = $"select hero_id from hero_talents where hero_id = {(int)Hero_id.Value} and hero_level = {Level.Text}";
            var read = comm.ExecuteReader();
            if (read.Read())
            {
                comm.CommandText = UpdateText();
                comm.Transaction = oracle.BeginTransaction();
                try
                {
                    var r = comm.ExecuteNonQuery();
                    comm.Transaction.Commit();
                    MyLogger.Log.Info($"User: {CurUser} has updated talent with hero_id = {(int)Hero_id.Value} and hero_level = {Level.Text}. {r} rows affected");
                }catch(Exception ex)
                {
                    MyLogger.Log.Error(ex);
                    comm.Transaction.Rollback();
                }
            }
            else
            {
                comm.CommandText = InsertText();
                comm.Transaction = oracle.BeginTransaction();
                try
                {
                    var r = comm.ExecuteNonQuery();
                    comm.Transaction.Commit();
                    MyLogger.Log.Info($"User: {CurUser} has inserted talent with hero_id = {(int)Hero_id.Value}, hero_level = {Level.Text}. {r} rows affected");
                }
                catch (Exception ex)
                {
                    MyLogger.Log.Error(ex);
                    comm.Transaction.Rollback();
                }
            }

            comm.CommandText = "select h.name, t.hero_level, t.left_talent, t.right_talent from heroes h, hero_talents t where t.hero_id = h.id";
            var visual = comm.ExecuteReader();
            var dt = new DataTable();
            dt.Load(visual);
            DataGrid.ItemsSource = dt.DefaultView;
            oracle.Close();
        }

        private string UpdateText()
        {
            var result = "update hero_talents set ";

            List<string> list = new List<string>();
            
            if (Left.Text != "") { list.Add($"left_talent = '{Left.Text}'"); }
            if (Right.Text != "") { list.Add($"right_talent = '{Right.Text}'"); }


            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                    result += list[i];
                else
                    result += ", " + list[i];
            }

            return result + $" hero_id = {(int)Hero_id.Value} and hero_level = {Level.Text}";
        }
        
        private string InsertText()
        {
            var result = "insert into hero_talents(hero_level, hero_id, left_talent, right_talent) " +
                $"values({Level.Text}, {(int)Hero_id.Value}, '{Left.Text}', '{Right.Text}')";
                       
            return result;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = login;
        }
    }
}
