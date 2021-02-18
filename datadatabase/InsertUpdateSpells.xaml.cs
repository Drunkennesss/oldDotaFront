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
    /// Логика взаимодействия для InsertUpdateSpells.xaml
    /// </summary>
    public partial class InsertUpdateSpells : Page
    {
        private string CurUser;
        private Frame MainFrame;
        private readonly Spells spells;
        private readonly OperationScenarios operation;
        private OracleCommand comm;
        private int id;

        public InsertUpdateSpells()
        {
            InitializeComponent();
        }
        public InsertUpdateSpells(string user, in Frame main, OperationScenarios operation, OracleCommand comm1, int id1, in Spells spells1) : this()
        {
            CurUser = user;
            MainFrame = main;
            this.operation = operation;
            comm = comm1;
            id = id1;
            spells = spells1;
            comm.CommandText = "select max(id) from spell_type";
            var read = comm.ExecuteReader();
            read.Read();
            var max = read.GetInt32(0);
            SpellType.Maximum = max;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = spells;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            while (Regex.IsMatch(NameBox.Text, @"--|[/][*]|union|[(]|[)]|from"))
                NameBox.Text = Regex.Replace(NameBox.Text, @"--|[/][*]|union|[(]|[)]|from", "");
            switch (operation)
            {
                case OperationScenarios.Insert:
                    comm.CommandText = InsertText();
                    try
                    {
                        comm.ExecuteNonQuery();
                        MyLogger.Log.Info($"User: {CurUser} has inserted new spell");
                    }
                    catch (Exception except)
                    {
                        MyLogger.Log.Error(except);
                    }
                    try { comm.Transaction.Commit(); }
                    catch (Exception except) { MyLogger.Log.Error(except); }
                    break;
                case OperationScenarios.Update:
                    comm.CommandText = UpdateText();
                    try
                    {
                        comm.ExecuteNonQuery();
                        MyLogger.Log.Info($"User: {CurUser} has updated spell with id = {id}");
                    }
                    catch (Exception except)
                    {
                        MyLogger.Log.Error(except);
                    }
                    try { comm.Transaction.Commit(); }
                    catch (Exception except) { MyLogger.Log.Error(except); }
                    break;
            }
        }

        private string InsertText()
        {
            var nametxt = "{''name'': " + $"''{NameBox.Text}'',";

            return $"insert into spells (id, spell_type, description, cooldown, manacost) " +
                  $"values ({id},{(int)SpellType.Value} ,'{nametxt + Description.Text}', '{Cooldown.Text}', '{Manacost.Text}')";
        }
        private string UpdateText()
        {
            var result = "update spells set ";
            
            List<string> list = new List<string>();
            if (NameBox.Text != "" && Description.Text != "")
            {
                var nametxt = "{''name'': " + $"''{NameBox.Text}'',";
                list.Add($"description = '{nametxt + Description.Text}'");
            }
            if (Cooldown.Text != "") { list.Add($"cooldown = '{Cooldown.Text}'"); }
            if (Manacost.Text != "") { list.Add($"manacost = '{Manacost.Text}'"); }
           
            
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                    result += list[i];
                else
                    result += ", " + list[i];
            }

            return result + $" where id = {id}";
        }
    }


}
