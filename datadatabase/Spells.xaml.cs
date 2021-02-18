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
using System.Text.RegularExpressions;

namespace datadatabase
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Spells : Page
    {
        private Frame MainFrame;
        private string CurUser;
        private Role role;
        private OperationScenarios operation;
        private AttrScenarios attr;
        private const string boxItem = "System.Windows.Controls.ComboBoxItem: ";
        private OracleTransaction Transaction = null;
        private const string select_all_spells = "select h.name Name, s.id Id, st.type_name TypeName," +
            @" REGEXP_REPLACE(regexp_substr(s.DESCRIPTION, '{''name'': [''""][A-Za-z ''!-/)/(]+[''""],'), '{''name'': |''|,','') AS Name, " +
            @" REGEXP_REPLACE(s.DESCRIPTION, '{''name'': [''""][A-Za-z ''!-/)/(]+[''""],|}', '') AS Description, " +
            "s.manacost Manacost, s.cooldown as Cooldown " +
                "from heroes h, hero_spells hs, spells s, spell_type st " +
                "where s.id = hs.spell_id and h.id = hs.hero_id and s.spell_type = st.id ";
        public Spells()
        {
            InitializeComponent();
        }
        public Spells(string user, in Frame main, Role r) : this()
        {
            CurUser = user;
            MainFrame = main;
            role = r;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Transaction?.Rollback();
            
            var oracle = OraConnect.oracle;
            if (oracle.State.ToString() == "Closed")
            {
                oracle.Open();
            }
            var comm = oracle.CreateCommand();
            switch (operation)
            {
                case OperationScenarios.Select:
                    SelectCase(comm);
                    break;
                case OperationScenarios.Delete:
                    Transaction = DeleteCase(comm);
                    break;
                case OperationScenarios.Update:
                    if(role == Role.user)
                    {
                        MessageBox.Show($"{CurUser}, you dont have access to this operation.");
                        goto default;
                    }
                    var needed_id = UpdateCase(comm);
                    if (needed_id == int.MinValue)
                        goto default;
                    Transaction = oracle.BeginTransaction();
                    comm.Transaction = Transaction;
                    MainFrame.Content = new InsertUpdateSpells(CurUser, in MainFrame, operation, comm, needed_id, this);
                    break;
                case OperationScenarios.Insert:
                    if (role == Role.user)
                    {
                        MessageBox.Show($"{CurUser}, you dont have access to this operation.");
                        goto default;
                    }
                    comm.CommandText = "select max(id) from spells";
                    var read = comm.ExecuteReader();
                    read.Read();
                    var new_id = read.GetInt32(0);
                    Transaction = oracle.BeginTransaction();
                    comm.Transaction = Transaction;
                    MainFrame.Content = new InsertUpdateSpells(CurUser, in MainFrame, operation, comm, new_id, this);
                    break;
                default:
                    SelectCase(comm);
                    break;
            }
            
            if (Transaction == null)
                oracle.Close();
        }

        private void SelectCase(OracleCommand comm)
        //create standalone function for delete and stop messing around;
        {

            Action<System.Collections.IList, OracleCommand> RowSelectedScenario = (System.Collections.IList row, OracleCommand comm1) =>
            {
                string result = string.Empty;
                bool first_iter = true;
                foreach (DataRowView i in row)
                {
                    if (first_iter)
                    {
                        first_iter = false;
                        result += i["id"].ToString();
                    }
                    else
                        result += "," + i["id"].ToString();
                }
                comm1.CommandText = select_all_spells + $"and s.id in ({result})";

            };
            if (TextQuery.Text.ToUpper() == "ALL")
            {
                comm.CommandText = select_all_spells;
            }
            else if (TextQuery.Text != "")
            {
                var search = TextQuery.Text;
                search = Regex.Replace(search, @"'", "''");
                for (; Regex.IsMatch(search, @"--|[/][*]|union|;|from");)
                    search = Regex.Replace(search, @"--|[/][*]|union|;|from", "");
                switch (attr)
                {
                    case AttrScenarios.Id:
                        if (!Regex.IsMatch(search, @"[^0-9]+"))
                            comm.CommandText = select_all_spells + $"and s.id = {search}";
                        else
                        {
                            MessageBox.Show("Invalid input");
                            MyLogger.Log.Warn(SelectBadInput(CurUser, attr, search));
                            comm.CommandText = select_all_spells;
                        }
                        break;
                    case AttrScenarios.Name:
                        if (!Regex.IsMatch(search, @"[^a-zA-Z !-/)/(]+"))
                            comm.CommandText = select_all_spells + "and s.description like '{''name'': " + $"''{search}''%'";
                        else
                        {
                            MessageBox.Show("Invalid input");
                            MyLogger.Log.Warn(SelectBadInput(CurUser, attr, search));
                            comm.CommandText = select_all_spells;
                        }
                        break;
                    case AttrScenarios.Hero:
                        if (!Regex.IsMatch(search, @"[^a-zA-Z]+"))
                            comm.CommandText = select_all_spells + $"and h.name = '{search}'";
                        else
                        {
                            MessageBox.Show("Invalid input");
                            MyLogger.Log.Warn(SelectBadInput(CurUser, attr, search));
                            comm.CommandText = select_all_spells;
                        }
                        break;

                    default:
                        comm.CommandText = select_all_spells;
                        MyLogger.Log.Warn($"User: {CurUser} passed no arguments while searching for spells");
                        break;
                }

            }
            else if (DataGrid.SelectedItems.Count != 0)
            {
                RowSelectedScenario(DataGrid.SelectedItems, comm);
            }
            if (DataGrid.SelectedItems.Count == 0 && TextQuery.Text == "")
            {
                comm.CommandText = select_all_spells;
            }
            if (DataGrid.SelectedItems.Count != 0 && TextQuery.Text != "")
            {
                RowSelectedScenario(DataGrid.SelectedItems, comm);
            }
            _fillDataGrid(comm.ExecuteReader());

        }

        private OracleTransaction DeleteCase(OracleCommand comm)
        {
            Func<System.Collections.IList, OracleCommand, List<int>> RowSelectedScenario = (System.Collections.IList row, OracleCommand comm1) =>
            {
                var result = new List<int>();
                foreach (DataRowView i in row)
                {
                    result.Add(int.Parse(i["id"].ToString()));
                }
                return result;
            };

            var transaction = Transaction ?? OraConnect.oracle.BeginTransaction();
            if (Transaction == null)
            {
                MyLogger.Log.Info($"User: {CurUser} has begone transaction");
            }
            comm.Transaction = transaction;
            bool no_input_errors = true;
            if (TextQuery.Text != "")
            {
                var search = TextQuery.Text;
                search = Regex.Replace(search, @"'", "''");
                for (; Regex.IsMatch(search, @"--|[/][*]|union|[(]|[)]|from");)
                    search = Regex.Replace(search, @"--|[/][*]|union|[(]|[)]|from", "");
                switch (attr)
                { // add log to every case
                    case AttrScenarios.Id:
                        if (!Regex.IsMatch(search, @"[^0-9]+"))
                        {
                            _deleteSpellsFromDependentTables(comm, int.Parse(search));
                            comm.CommandText = $"delete from spells where id = {search}";
                        }
                        else
                        {
                            MessageBox.Show("Invalid input");
                            MyLogger.Log.Warn(DeleteBadInput(CurUser, attr, search));
                            no_input_errors = false;
                            comm.CommandText = select_all_spells;
                        }
                        break;
                    case AttrScenarios.Name:
                        if (!Regex.IsMatch(search, @"[^a-zA-Z !-/)/(]+"))
                        {
                            comm.CommandText = "select id from spells where description like '{''name'': " + $"''{search}''%'";
                            var read = comm.ExecuteReader();
                            if (read.Read())
                            {
                                var id = read.GetInt32(0);
                                _deleteSpellsFromDependentTables(comm, id);
                                comm.CommandText = "delete from spells where description like '{''name'': " + $"''{search}''%'";
                            }
                            else
                            {
                                MyLogger.Log.Warn(DeleteBadInput(CurUser, attr, search));
                                comm.CommandText = select_all_spells;
                                no_input_errors = false;
                            }

                        }
                        else
                        {
                            MessageBox.Show("Invalid input");
                            MyLogger.Log.Warn(DeleteBadInput(CurUser, attr, search));
                            no_input_errors = false;
                            comm.CommandText = select_all_spells;
                        }
                        break;
                    case AttrScenarios.Hero:
                        if (!Regex.IsMatch(search, @"[^a-zA-Z]+"))
                        {
                            comm.CommandText = $"select distinct hs.spell_id from hero_spells hs, heroes h where h.name = '{search}' and h.id = hs.hero_id";
                            var read = comm.ExecuteReader();
                            if (read.Read())
                            {
                                var id = new List<int>();
                                id.Add(read.GetInt32(0));
                                while (read.Read())
                                {
                                    id.Add(read.GetInt32(0));
                                }
                                bool first_iter = true;
                                string result = string.Empty;
                                foreach (var i in id)
                                {
                                    if (first_iter)
                                    {
                                        first_iter = false;
                                        result += i;
                                    }
                                    _deleteSpellsFromDependentTables(comm, i);
                                    result += "," + i;
                                }
                                comm.CommandText = $"delete from spells where id in({result})";                               
                            }
                            else
                            {
                                MyLogger.Log.Warn(DeleteBadInput(CurUser,attr,search));
                                comm.CommandText = select_all_spells;
                                no_input_errors = false;
                                
                            }

                        }
                        else
                        {
                            MessageBox.Show("Invalid input");
                            MyLogger.Log.Warn(DeleteBadInput(CurUser, attr, search));
                            no_input_errors = false;
                            comm.CommandText = select_all_spells;
                        }
                        break;
                    default:
                        comm.CommandText = select_all_spells;
                        MyLogger.Log.Warn($"User: {CurUser} passed no arguments while deleting spells");
                        no_input_errors = false;
                        break;
                }

            }
            else if (DataGrid.SelectedItems.Count != 0)
            {
                var list = RowSelectedScenario(DataGrid.SelectedItems, comm);
                bool first_iter = true;
                string result = string.Empty;
                foreach (var i in list)
                {
                    if (first_iter)
                    {
                        first_iter = false;
                        result += i;
                    }
                    _deleteSpellsFromDependentTables(comm, i);
                    result += "," + i;
                }

                comm.CommandText = $"delete from spells where id in({result})";
            }
            else if (DataGrid.SelectedItems.Count != 0 && TextQuery.Text != "")
            {
                var list = RowSelectedScenario(DataGrid.SelectedItems, comm);
                bool first_iter = true;
                string result = string.Empty;
                foreach (var i in list)
                {
                    if (first_iter)
                    {
                        first_iter = false;
                        result += i;
                    }
                    _deleteSpellsFromDependentTables(comm, i);
                    result += "," + i;
                }

                comm.CommandText = $"delete from spells where id in({result})";
            }
            if (no_input_errors)
            {
                var rowsAffected = comm.ExecuteNonQuery();
                MyLogger.Log.Info($"User: {CurUser} has deleted some spells. {rowsAffected} rows affected");
            }
            else
            {
                _fillDataGrid(comm.ExecuteReader());
            }
            //Log here
            return transaction;
        }

        private int UpdateCase(OracleCommand comm)
        {
            bool invalid_input = false;
            int needed_id = int.MinValue;
            if (TextQuery.Text.ToUpper() == "ALL")
            {
                MessageBox.Show("Invalid input");
                MyLogger.Log.Warn($"User: {CurUser}. Invalid input occured while inserting hero by none, got: '{TextQuery.Text}'  ");
            }
            else if (TextQuery.Text != "")
            {

                var search = TextQuery.Text;
                search = Regex.Replace(search, @"'", "''");
                for (; Regex.IsMatch(search, @"--|[/][*]|union|[(]|[)]|from");)
                    search = Regex.Replace(search, @"--|[/][*]|union|[(]|[)]|from", "");
                switch (attr)
                {
                    case AttrScenarios.Id:
                        if (!Regex.IsMatch(search, @"[^0-9]+"))
                        {
                            comm.CommandText = $"select id from spells where id = {search}";
                            var read = comm.ExecuteReader();
                            if (read.Read())
                            {
                                needed_id = read.GetInt32(0);
                            }
                            else
                            {
                                MessageBox.Show("Invalid input");
                                MyLogger.Log.Warn(UpdateBadInput(CurUser, attr, search));
                                comm.CommandText = select_all_spells;
                                invalid_input = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid input");
                            MyLogger.Log.Warn(UpdateBadInput(CurUser, attr, search));
                            comm.CommandText = select_all_spells;
                        }
                        break;
                    case AttrScenarios.Name:
                        if (!Regex.IsMatch(search, @"[^a-zA-Z !-/)/(]+"))
                        {
                            comm.CommandText = "select id from spells where description like '{''name'': " + $"''{search}''%'";
                            var read = comm.ExecuteReader();
                            if (read.Read())
                            {
                                needed_id = read.GetInt32(0);
                            }
                            else
                            {
                                MessageBox.Show("Invalid input");
                                MyLogger.Log.Warn(UpdateBadInput(CurUser, attr, search));
                                comm.CommandText = select_all_spells;
                                invalid_input = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid input");
                            MyLogger.Log.Warn(UpdateBadInput(CurUser, attr, search));
                            comm.CommandText = select_all_spells;
                        }
                        break;
                    default:
                        comm.CommandText = select_all_spells;
                        MyLogger.Log.Warn($"User: {CurUser} passed no arguments while update new hero");
                        break;
                }

            }
            else if (DataGrid.SelectedItem != null)
            {
                var row = DataGrid.SelectedItem as DataRowView;
                needed_id = int.Parse(row["id"].ToString());
            }
            else if (DataGrid.SelectedItems.Count == 0 && TextQuery.Text == "")
            {
                comm.CommandText = select_all_spells;
            }
            /*if (DataGrid.SelectedItems.Count != 0 && TextQuery.Text != "")
            {
                var row = DataGrid.SelectedItem as DataRowView;
                needed_id = int.Parse(row["id"].ToString());
            }*/
            if (invalid_input)
            {
                return int.MinValue;
            }
            else
            {
                return needed_id;
            }

        }

        private void _deleteSpellsFromDependentTables(OracleCommand comm, int id)
        {
            comm.CommandText = $"delete from hero_spells where spell_id = {id}";
            var rows = comm.ExecuteNonQuery();
            MyLogger.Log.Info($"User: {CurUser} has deleted some rows from hero_spells where id = {id}. {rows} rows were affected");
            comm.CommandText = $"delete from spells where id = {id}";
            comm.ExecuteNonQuery();
            MyLogger.Log.Info($"User: {CurUser} has deleted some rows from spells where id = {id}. {rows} rows were affected");
        }

        private void _fillDataGrid(OracleDataReader read)
        {
            DataTable dt = new DataTable();
            dt.Load(read);
            DataGrid.ItemsSource = dt.DefaultView;
        }

        private string SelectBadInput(string user, AttrScenarios attr, string bad)
        {
            return $"User: {user}. Invalid input occured while searching for spells by {Enum.GetName(attr.GetType(), attr)}, got: '{bad}'  ";
        }
        private string DeleteBadInput(string user, AttrScenarios attr, string bad)
        {
            return $"User: {user}. Invalid input occured while deleting spells by {Enum.GetName(attr.GetType(), attr)}, got: '{bad}'  ";
        }
        private string UpdateBadInput(string user, AttrScenarios attr, string bad)
        {
            return $"User: {user}. Invalid input occured while updating spell by {Enum.GetName(attr.GetType(), attr)}, got: '{bad}'  ";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Heroes(CurUser,in MainFrame, role);
        }

        private void AttrCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = sender as ComboBox;
            switch (box.SelectedItem.ToString()) // box.Text would have done it better, feels braindead 
            {
                case boxItem + "Id":
                    attr = AttrScenarios.Id;
                    break;
                case boxItem + "Name":
                    attr = AttrScenarios.Name;
                    break;
                case boxItem + "Hero":
                    attr = AttrScenarios.Hero;
                    break;
            }
            DependentBox.Content = Enum.GetName(attr.GetType(), attr) + " :";
        }

        private void OperationCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = sender as ComboBox;
            switch (box.SelectedItem.ToString()) // "System.Windows.Controls.ComboBoxItem: "
            {
                case boxItem + "Select":
                    operation = OperationScenarios.Select;
                    Enable_DisableCombo(true);
                    break;
                case boxItem + "Delete":
                    operation = OperationScenarios.Delete;
                    Enable_DisableCombo(true);
                    break;
                case boxItem + "Update":
                    operation = OperationScenarios.Update;
                    Enable_DisableCombo(false);
                    AttrCombo.SelectedIndex = AttrCombo.SelectedIndex >= 1 ? 0 : AttrCombo.SelectedIndex;
                    break;
                case boxItem + "Insert":
                    operation = OperationScenarios.Insert;
                    Enable_DisableCombo(false);
                    AttrCombo.SelectedIndex = AttrCombo.SelectedIndex >= 1 ? 0 : AttrCombo.SelectedIndex;
                    break;
            }

        }
        private void Enable_DisableCombo(bool enable)
        {
            hero.IsEnabled = enable;
        }

        private void Rollback_Click(object sender, RoutedEventArgs e)
        {
            Transaction?.Rollback();
            if (Transaction != null)
            {
                MyLogger.Log.Info($"User: {CurUser} has reverted reacent transaction");
            }
            Transaction?.Dispose();
            Transaction = null;
            try
            {
                OraConnect.oracle.Close();
            }
            catch (Exception except)
            {
                MyLogger.Log.Error(except);
            }

        }

        private void Commit_Click(object sender, RoutedEventArgs e)
        {
            Transaction?.Commit();
            if (Transaction != null)
            {
                MyLogger.Log.Info($"User: {CurUser} has saved reacent transaction");
            }
            Transaction?.Dispose();
            Transaction = null;
            try
            {
                OraConnect.oracle.Close();
            }
            catch (Exception except)
            {
                MyLogger.Log.Error(except);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Transaction?.Rollback();
            MainFrame.Content = new LoginPage(in MainFrame);
        }
    }
}
