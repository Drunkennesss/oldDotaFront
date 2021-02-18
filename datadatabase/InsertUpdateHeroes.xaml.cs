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
using System.Text.RegularExpressions;

namespace datadatabase
{
    /// <summary>
    /// Логика взаимодействия для InsertUpdateHeroes.xaml
    /// </summary>
    public partial class InsertUpdateHeroes : Page
    {
        private string CurUser;
        private Frame MainFrame;
        private readonly Heroes heroes;
        private readonly OperationScenarios operation;
        private OracleCommand comm;
        private int id;
        private static readonly Regex regex = new Regex(@"[^0-9.]+");
        private static readonly Regex regex2 = new Regex(@"[0-9]+[.]{0,1}[0-9]*");
        private const string boxItem = "System.Windows.Controls.ComboBoxItem: ";
        public InsertUpdateHeroes()
        {
            InitializeComponent();
        }

        public InsertUpdateHeroes(string user, in Frame main, OperationScenarios operation, OracleCommand comm1, int id1, in Heroes heroes1) : this()
        {
            CurUser = user;
            MainFrame = main;
            this.operation = operation;
            comm = comm1;
            id = id1;
            heroes = heroes1;
            switch( operation )
            {
                case OperationScenarios.Update:
                    comm.CommandText = $"select name from heroes where id = {id}";
                    var read = comm.ExecuteReader();
                    read.Read();
                    _info.Content = $"Updating {read.GetString(0)}";
                    break;
                case OperationScenarios.Insert:
                    _combo_delete.IsEnabled = false;
                    break;
            }        
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            while (Regex.IsMatch(Name.Text, @"--|[/][*]|union|[(]|[)]|from"))
                Name.Text = Regex.Replace(Name.Text, @"--|[/][*]|union|[(]|[)]|from", "");
            switch (operation)
            {
                case OperationScenarios.Insert:
                    comm.CommandText = InsertText();
                    try
                    {
                        comm.ExecuteNonQuery();
                        MyLogger.Log.Info($"User: {CurUser} has inserted new hero");
                    }
                    catch (Exception except)
                    {
                        MyLogger.Log.Error(except);
                    }
                    if(AddSpell.Text != "")
                    {
                        
                        try
                        {
                            comm.CommandText = $"insert into hero_spells (hero_id, spell_id) values({id}, {AddSpell.Text})";
                            comm.ExecuteNonQuery();
                            MyLogger.Log.Info($"User: {CurUser} has inserted new spell to hero");
                        }
                        catch(Exception except)
                        {
                            MyLogger.Log.Error(except);
                        }
                    }
                    try { comm.Transaction.Commit(); }
                    catch (Exception except) { MyLogger.Log.Error(except); }
                    break;
                case OperationScenarios.Update:
                    comm.CommandText = UpdateText();
                    try
                    {
                        comm.ExecuteNonQuery();
                        MyLogger.Log.Info($"User: {CurUser} has updated hero with id = {id}");
                    }
                    catch (Exception except)
                    {
                        MyLogger.Log.Error(except);
                    }
                    if (AddSpell.Text != "")
                    {
                        if (_adding_spell)
                        {
                            try
                            {
                                comm.CommandText = $"insert into hero_spells (hero_id, spell_id) values({id}, {AddSpell.Text})";
                                comm.ExecuteNonQuery();
                                MyLogger.Log.Info($"User: {CurUser} has updated hero {id} with new spell {AddSpell.Text}");
                            }
                            catch (Exception except)
                            {
                                MyLogger.Log.Error(except);
                            }
                        }
                        else
                        {
                            try
                            {
                                comm.CommandText = $"delete from hero_spells where hero_id = {id} and spell_id = {AddSpell.Text}";
                                comm.ExecuteNonQuery();
                                MyLogger.Log.Info($"User: {CurUser} has updated hero {id} spell {AddSpell.Text} was deleted");
                            }
                            catch (Exception except)
                            {
                                MyLogger.Log.Error(except);
                            }
                        }
                    }
                    try { comm.Transaction.Commit(); }
                    catch (Exception except) { MyLogger.Log.Error(except); }
                    break;
            }
        }

        private string InsertText()
        {
            return $"insert into heroes (id, name, main_stat, attack_type, attack_range, agil, intel, stren," +
                 "agil_incr, int_incr, stren_incr, damage_base, hp_base, mana_base, armour_base," +
                 " attack_speed_base, movespeed_base, spellresist_base, hp_regen_base, mana_regen_base, view_range, projectile_speed)" +
                  $"values ({id}, '{Name.Text}', '{MainStat.Text}', '{AttackType.Text}'," +
                $"{AttackRange.Text}, {Agil.Text}, {Intel.Text}, {Stren.Text}, " +
                $"{AgilIncr.Text}, {IntIncr.Text}, {StrIncr.Text}, {Damage.Text}, " +
                $"{HpBase.Text}, {ManaBase.Text}, {Armor.Text}, {AttackSpeed.Text}, " +
                $"{MoveSpeed.Text}, {SpellResist.Text}, {HpRegen.Text}, {ManaRegen.Text}, {ViewRange.Text}, {ProjSpeed.Text})";
        }
        private string UpdateText()
        {
            var result = "update heroes set ";
            List<string> list = new List<string>();
            if (Name.Text != "") { list.Add($"name = '{Name.Text}'"); }
            if (MainStat.Text != "") { list.Add($"main_stat = '{MainStat.Text}'"); }
            if (AttackType.Text != "") { list.Add($"attack_type = '{AttackType.Text}'"); }
            if (AttackRange.Text != "") { list.Add($"attack_range = {AttackRange.Text}"); }
            if (Agil.Text != "") { list.Add($"agil = {Agil.Text}"); }
            if (Intel.Text != "") { list.Add($"intel = {Intel.Text}"); }
            if (Stren.Text != "") { list.Add($"stren = {Stren.Text}"); }
            if (AgilIncr.Text != "") { list.Add($"agil_incr = {AgilIncr.Text}"); }
            if (IntIncr.Text != "") { list.Add($"int_incr = {IntIncr.Text}"); }
            if (StrIncr.Text != "") { list.Add($"stren_incr = {StrIncr.Text}"); }
            if (Damage.Text != "") { list.Add($"damage_base = {Damage.Text}"); }
            if (HpBase.Text != "") { list.Add($"hp_base = {HpBase.Text}"); }
            if (ManaBase.Text != "") { list.Add($"mana_base = {ManaBase.Text}"); }
            if (Armor.Text != "") { list.Add($"armour_base = {Armor.Text}"); }
            if (AttackSpeed.Text != "") { list.Add($"attack_speed_base = {AttackSpeed.Text}"); }
            if (MoveSpeed.Text != "") { list.Add($"movespeed_base = {MoveSpeed.Text}"); }
            if (SpellResist.Text != "") { list.Add($"spellresist_base = {SpellResist.Text}"); }
            if (HpRegen.Text != "") { list.Add($"hp_regen_base = {HpRegen.Text}"); }
            if (ManaRegen.Text != "") { list.Add($"mana_regen_base = {ManaRegen.Text}"); }
            if (ViewRange.Text != "") { list.Add($"view_range = {ViewRange.Text}"); }
            if (ProjSpeed.Text != "") { list.Add($"projectile_speed = {ProjSpeed.Text}"); }
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0 )
                    result += list[i];
                else
                    result += ", " + list[i];                
            }

            return result + $" where id = {id}";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = heroes;
        }

       
        private static bool IsTextAllowed(string text)
        {
            return !regex.IsMatch(text);
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {            
            e.Handled = !IsTextAllowed(e.Text);            
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbox = sender as ComboBox;
            switch (cbox.SelectedItem.ToString())
            {
                case boxItem + "AddSpellbyId":
                    _adding_spell = true;
                    break;
                case boxItem + "DeleteSpellbyId":
                    _adding_spell = false;
                    break;
            }
        }

        bool _adding_spell = true;
    }
}
