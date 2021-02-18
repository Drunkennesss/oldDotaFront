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
using Oracle.ManagedDataAccess.EntityFramework;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Text.RegularExpressions;
using NLog;

//"select a.name, b.description from heroes a, spells b, hero_spells c where a.id = c.hero_id and b.id = c.spell_id order by a.name"
namespace datadatabase
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Heroes : Page
    {
        private Frame MainFrame;
        private string CurUser;
        private Role role;
        private OperationScenarios operation;
        private AttrScenarios attr;
        private const string boxItem = "System.Windows.Controls.ComboBoxItem: ";
        private OracleTransaction Transaction = null;
        private const string select_all_heroes = "select * from heroes";

        public Heroes()
        {
            InitializeComponent();            
            //oracle = new OracleConnection(con);
        }
        public Heroes(string user, in Frame f, Role r) : this()
        {
            CurUser = user;
            MainFrame = f;
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
                    var needed_id = UpdateCase(comm);
                    if (needed_id == int.MinValue)
                        goto default;
                    Transaction = oracle.BeginTransaction();
                    comm.Transaction = Transaction;
                    MainFrame.Content = new InsertUpdateHeroes(CurUser, in MainFrame, operation, comm, needed_id, this);
                    break;
                case OperationScenarios.Insert:
                    int needed_id2;
                    comm.CommandText = "select max(id) from heroes";
                    var read = comm.ExecuteReader();
                    read.Read();
                    needed_id2 = read.GetInt32(0) + 1;
                    Transaction = oracle.BeginTransaction();
                    comm.Transaction = Transaction;
                    MainFrame.Content = new InsertUpdateHeroes(CurUser, in MainFrame, operation, comm, needed_id2, this);
                    break;
                default:
                    SelectCase(comm);
                    break;
            }
            if(Transaction == null)
                oracle.Close();
        }

        private void _fillDataGrid(OracleDataReader read)
        {
            DataTable dt = new DataTable();
            dt.Load(read);
            DataGrid.ItemsSource = dt.DefaultView;
        }
        
        private void SelectCase(OracleCommand comm) 
            //create standalone function for delete and stop messing around;
        {
            
            Action<System.Collections.IList, OracleCommand> RowSelectedScenario = (System.Collections.IList row, OracleCommand comm1) =>
              {
                  string result = string.Empty;
                  bool first_iter = true;
                  foreach(DataRowView i in row)
                  {
                      if (first_iter)
                      {
                          first_iter = false;
                          result += i["id"].ToString();
                      }
                      else
                          result += "," + i["id"].ToString();
                  }
                  comm1.CommandText = _select_heroes_with_lvl() + $" where id in ({result})";
                  
              };
            if (TextQuery.Text.ToUpper() == "ALL")
            {
                comm.CommandText = _select_heroes_with_lvl();
            }
            else if (TextQuery.Text != "")
            {
                var search = TextQuery.Text;
                search = Regex.Replace(search, @"'", "''");
                for (; Regex.IsMatch(search, @"--|[/][*]|union|[(]|[)]|from"); )
                    search = Regex.Replace(search, @"--|[/][*]|union|[(]|[)]|from", "");
                switch (attr)
                {
                    case AttrScenarios.Id:
                        if (!Regex.IsMatch(search, @"[^0-9]+"))
                            comm.CommandText = _select_heroes_with_lvl() + $" where id = {search}";
                        else
                        {
                            MessageBox.Show("Invalid input");
                            MyLogger.Log.Warn(SelectBadInput(CurUser, attr, search));
                            comm.CommandText = _select_heroes_with_lvl();
                        }
                        break;
                    case AttrScenarios.Name:
                        if (!Regex.IsMatch(search, @"[^a-zA-Z]+"))
                            comm.CommandText = _select_heroes_with_lvl() + $" where name = '{search}'";
                        else
                        {
                            MessageBox.Show("Invalid input");
                            MyLogger.Log.Warn(SelectBadInput(CurUser, attr, search));
                            comm.CommandText = _select_heroes_with_lvl();
                        }
                        break;
                    case AttrScenarios.MainAttr:
                        if (Regex.IsMatch(search, @"str|int|agi"))
                        {
                            var search2 = Regex.Match(search, @"str|int|agi").Value;
                            comm.CommandText = _select_heroes_with_lvl() + $" where main_stat = '{search2}'";
                        }
                        else
                        {
                            MessageBox.Show("Invalid input");
                            MyLogger.Log.Warn(SelectBadInput(CurUser, attr, search));
                            comm.CommandText = _select_heroes_with_lvl();
                        }
                        break;
                    case AttrScenarios.AttackType:
                        if (Regex.IsMatch(search, @"melee|range"))
                        {
                            var search2 = Regex.Match(search, @"melee|range").Value;
                            comm.CommandText =_select_heroes_with_lvl() + $" where attack_type = '{search2}'";
                        }
                        else
                        {
                            MessageBox.Show("Invalid input");
                            MyLogger.Log.Warn(SelectBadInput(CurUser, attr, search));
                            comm.CommandText = _select_heroes_with_lvl();
                        }
                        break;
                    default:
                        comm.CommandText = _select_heroes_with_lvl();
                        MyLogger.Log.Warn($"User: {CurUser} passed no arguments while searching for heroes");
                        break;
                }

            }
            else if (DataGrid.SelectedItems.Count != 0)
            {
                RowSelectedScenario(DataGrid.SelectedItems, comm);
            }
            else if(DataGrid.SelectedItems.Count == 0 && TextQuery.Text == "")
            {
                comm.CommandText = _select_heroes_with_lvl();
            }
            /*if (DataGrid.SelectedItems.Count != 0 && TextQuery.Text != "")
            {
                RowSelectedScenario(DataGrid.SelectedItems, comm);
            }*/
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
                            _deleteHeroesFromDependentTables(comm, int.Parse(search));
                            comm.CommandText = $"delete from heroes where id = {search}";
                        }
                        else
                        {
                            MessageBox.Show("Invalid input");
                            MyLogger.Log.Warn(DeleteBadInput(CurUser, attr, search));
                            no_input_errors = false;
                            comm.CommandText = _select_heroes_with_lvl();
                        }
                        break;
                    case AttrScenarios.Name:
                        if (!Regex.IsMatch(search, @"[^a-zA-Z]+"))
                        {
                            comm.CommandText = $"select id from heroes where name = '{search}'";
                            var read = comm.ExecuteReader();
                            if (read.Read())
                            {
                                var id = read.GetInt32(0);
                                _deleteHeroesFromDependentTables(comm, id);
                                comm.CommandText = $"delete from heroes where name = '{search}'";
                            }
                            else
                            {
                                MyLogger.Log.Warn(DeleteBadInput(CurUser,attr,search));
                                comm.CommandText = _select_heroes_with_lvl();
                                no_input_errors = false;
                            }
                            
                        }
                        else
                        {
                            MessageBox.Show("Invalid input");
                            MyLogger.Log.Warn(DeleteBadInput(CurUser, attr, search));
                            no_input_errors = false;
                            comm.CommandText = _select_heroes_with_lvl();
                        }
                        break;
                    case AttrScenarios.MainAttr:
                        if (Regex.IsMatch(search, @"str|int|agi"))
                        {
                            var search2 = Regex.Match(search, @"str|int|agi").Value;
                            comm.CommandText = $"select id from heroes where main_stat = '{search2}'";
                            var read = comm.ExecuteReader();
                            var id_list = new List<int>();
                            while (read.Read())
                            {
                                id_list.Add(read.GetInt32(0));
                            }
                            foreach (var item in id_list)
                            {
                                _deleteHeroesFromDependentTables(comm, item);
                            }
                            comm.CommandText = $"delete from heroes where main_stat = '{search2}'";
                        }
                        else
                        {
                            MessageBox.Show("Invalid input");
                            MyLogger.Log.Warn(DeleteBadInput(CurUser, attr, search));
                            no_input_errors = false;
                            comm.CommandText = _select_heroes_with_lvl();
                        }
                        break;
                    case AttrScenarios.AttackType:
                        if (Regex.IsMatch(search, @"melee|range"))
                        {
                            var search2 = Regex.Match(search, @"melee|range").Value;
                            comm.CommandText = $"select id from heroes where attack_type = '{search2}'";
                            var read = comm.ExecuteReader();
                            var id_list = new List<int>();
                            while (read.Read())
                            {
                                id_list.Add(read.GetInt32(0));
                            }
                            foreach (var item in id_list)
                            {
                                _deleteHeroesFromDependentTables(comm, item);
                            }
                            comm.CommandText = $"delete from heroes where attack_type = '{search2}'";
                        }
                        else
                        {
                            MessageBox.Show("Invalid input");
                            MyLogger.Log.Warn(DeleteBadInput(CurUser, attr, search));
                            no_input_errors = false;
                            comm.CommandText = _select_heroes_with_lvl();
                        }
                        break;
                    default:
                        comm.CommandText =_select_heroes_with_lvl();
                        MyLogger.Log.Warn($"User: {CurUser} passed no arguments while deleting heroes");
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
                    _deleteHeroesFromDependentTables(comm, i);
                    result += "," + i;
                }

                comm.CommandText = $"delete from heroes where id in({result})";
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
                    _deleteHeroesFromDependentTables(comm, i);
                    result += "," + i;
                }

                comm.CommandText = $"delete from heroes where id in({result})";
            }
            if (no_input_errors)
            {
                var rowsAffected = comm.ExecuteNonQuery();
                MyLogger.Log.Info($"User: {CurUser} has deleted some heroes. {rowsAffected} rows were affected");
            }
            else
            {
                _fillDataGrid(comm.ExecuteReader());
            }
            //Log here
            return transaction;
        }

        private void _deleteHeroesFromDependentTables(OracleCommand comm,int id)
        {
            comm.CommandText = $"delete from hero_talents where hero_id = {id}";
            /*log this*/
            
            var hero_tallents_rows = comm.ExecuteNonQuery();
            MyLogger.Log.Info($"User: {CurUser} has deleted talent with id: {id}. {hero_tallents_rows} rows were affected");
            comm.CommandText = $"select spell_id from hero_spells where hero_id = {id}";
            var reader = comm.ExecuteReader();
            var spells_id = new List<int>();
            while (reader.Read())
            {
               spells_id.Add(reader.GetInt32(0));
            }
            comm.CommandText = $"delete from hero_spells where hero_id = {id}";
            var hero_spells_rows = comm.ExecuteNonQuery();//log this
            MyLogger.Log.Info($"User: {CurUser} has deleted hero_spells with id: {id}. {hero_spells_rows} rows were affected");
            bool first_iter = true;
            string spells_id_to_delete = string.Empty;
            foreach (var i in spells_id)
            {
                if (first_iter)
                {
                    first_iter = false;
                    spells_id_to_delete += i.ToString();
                }
                else
                    spells_id_to_delete += "," + i.ToString();
            }
            if (spells_id_to_delete != string.Empty)
            {
                comm.CommandText = $"delete from spells where id in ({spells_id_to_delete})";
                var final = comm.ExecuteNonQuery();//log this
                MyLogger.Log.Info($"User: {CurUser} has deleted spells with id in ({spells_id_to_delete}). {final} rows were affected");
            }            

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
                            comm.CommandText = $"select id from heroes where id = {search}";
                            var read = comm.ExecuteReader();
                            if (read.Read())
                            {
                                needed_id = read.GetInt32(0);
                            }
                            else
                            {
                                MessageBox.Show("Invalid input");
                                MyLogger.Log.Warn(UpdateBadInput(CurUser, attr, search));
                                comm.CommandText = _select_heroes_with_lvl();
                                invalid_input = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid input");
                            MyLogger.Log.Warn(UpdateBadInput(CurUser, attr, search));
                            comm.CommandText = _select_heroes_with_lvl();
                        }
                        break;
                    case AttrScenarios.Name:
                        if (!Regex.IsMatch(search, @"[^a-zA-Z]+"))
                        {
                            comm.CommandText = $"select id from heroes where name = '{search}'";
                            var read = comm.ExecuteReader();
                            if (read.Read())
                            {
                                needed_id = read.GetInt32(0);
                            }
                            else
                            {
                                MessageBox.Show("Invalid input");
                                MyLogger.Log.Warn(UpdateBadInput(CurUser, attr, search));
                                comm.CommandText = _select_heroes_with_lvl();
                                invalid_input = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid input");
                            MyLogger.Log.Warn(UpdateBadInput(CurUser, attr, search));
                            comm.CommandText = _select_heroes_with_lvl();
                        }
                        break;
                    default:
                        comm.CommandText = _select_heroes_with_lvl();
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
                comm.CommandText = _select_heroes_with_lvl();
            }
            if (DataGrid.SelectedItems.Count != 0 && TextQuery.Text != "")
            {
                var row = DataGrid.SelectedItem as DataRowView;
                needed_id = int.Parse(row["id"].ToString());
            }
            if (invalid_input)
            {
                return int.MinValue;
            }
            else
            {
                return needed_id;
            }
            
        }

        private string SelectBadInput(string user,AttrScenarios attr , string bad)
        {
            return $"User: {user}. Invalid input occured while searching for heroes by {Enum.GetName(attr.GetType(),attr)}, got: '{bad}'  ";
        }
        private string DeleteBadInput(string user, AttrScenarios attr, string bad)
        {
            return $"User: {user}. Invalid input occured while deleting heroes by {Enum.GetName(attr.GetType(), attr)}, got: '{bad}'  ";
        }
        private string UpdateBadInput(string user, AttrScenarios attr, string bad)
        {
            return $"User: {user}. Invalid input occured while updating hero by {Enum.GetName(attr.GetType(), attr)}, got: '{bad}'  ";
        }

        private void OperationCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = sender as ComboBox;
            switch (box.SelectedItem.ToString()) // "System.Windows.Controls.ComboBoxItem: "
            {
                case boxItem + "Select":
                    operation = OperationScenarios.Select;
                    Enable_DisableCombo(true);
                    Enable_DisableLevel(true);
                    break;
                case boxItem + "Delete":
                    operation = OperationScenarios.Delete;
                    Enable_DisableCombo(true);
                    Enable_DisableLevel(false);
                    break;
                case boxItem + "Update":
                    operation = OperationScenarios.Update;
                    Enable_DisableCombo(false);
                    Enable_DisableLevel(false);
                    AttrCombo.SelectedIndex = AttrCombo.SelectedIndex >= 2 ? 0 : AttrCombo.SelectedIndex;
                    break;
                case boxItem + "Insert":
                    operation = OperationScenarios.Insert;
                    Enable_DisableCombo(false);
                    Enable_DisableLevel(false);
                    AttrCombo.SelectedIndex = AttrCombo.SelectedIndex >=1 ? 0 : AttrCombo.SelectedIndex;
                    break;
            }
            
        }
        private void Enable_DisableCombo(bool enable)
        {
             _combo_attr.IsEnabled = enable;
             _combo_attack.IsEnabled = enable;            
        }
        private void Enable_DisableLevel(bool enable)
        {
            Level.IsEnabled = enable;
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
                case boxItem + "Main Attr":
                    attr = AttrScenarios.MainAttr;
                    break;
                case boxItem + "Attack Type":
                    attr = AttrScenarios.AttackType;
                    break;
            }
            DependentLabel.Content = Enum.GetName(attr.GetType(), attr) + " :";
        }       

        private void RollBack_Click(object sender, RoutedEventArgs e)
        {
            Transaction?.Rollback();
            if(Transaction != null)
            {
                MyLogger.Log.Info($"User: {CurUser} has reverted reacent transaction");
            }
            Transaction?.Dispose();
            Transaction = null;
            try
            {
                OraConnect.oracle.Close();
            }
            catch(Exception except)
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Spells(CurUser, MainFrame, role);
        }

        private string _select_heroes_with_lvl()
        {
            if (Level.Value == 0 || !Level.IsEnabled)
                return select_all_heroes;
            var lvl = (int)Level.Value - 1;
            return $"SELECT id, name, main_stat, attack_type, attack_range, agil + {lvl}*agil_incr as agil," +
                $" intel + {lvl}*int_incr as intel, stren + {lvl}*stren_incr as stren, agil_incr, int_incr, stren_incr," +
                $" (case when main_stat = 'agi' then damage_base + agil + {lvl}*agil_incr when main_stat = 'str' " +
                $"then damage_base + stren + {lvl}*stren_incr when main_stat = 'int' then damage_base + intel + {lvl}*int_incr else damage_base end)" +
                $" as damage, hp_base + stren*20 + {lvl}*stren_incr*20 as hp, mana_base + intel*12 + {lvl}*int_incr*12 as mana," +
                $" armour_base + agil*0.16 + {lvl}*agil_incr*0.16 as armor, attack_speed_base + agil + {lvl}*agil_incr as  attack_speed," +
                $" movespeed_base + movespeed_base*agil*0.05/100 + movespeed_base*{lvl}*agil_incr*0.05/100 as movespeed," +
                $" spellresist_base + spellresist_base*stren*0.08/100 + spellresist_base*{lvl}*stren_incr*0.08/100 as spellresist," +
                $" hp_regen_base + stren*0.1 + {lvl}*stren_incr*0.1 as hp_regen, mana_regen_base + 0.05*intel + {lvl}*int_incr*0.05 as mana_regen," +
                $" view_range, projectile_speed FROM heroes";    
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Transaction?.Rollback();
            MainFrame.Content = new LoginPage(in MainFrame);
               
        }
    }
    public enum OperationScenarios {Select = 1, Delete, Update, Insert }
    internal enum AttrScenarios {Name = 1, Id, MainAttr, AttackType, Hero }
    

}
