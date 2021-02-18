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
using System.Text.RegularExpressions;

namespace datadatabase
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private const string admin_pass = "admin";
        private Role role = Role.user;
        private Frame MainFrame;
        public string CurUser { get; private set; }
        private bool focused = false;
        public LoginPage()
        {
            InitializeComponent();
        }
        public LoginPage(in Frame f) : this()
        {
            MainFrame = f;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            //delete this 

            if (LoginBox.Text == "Enter your name" || LoginBox.Text == "" || Regex.IsMatch(LoginBox.Text,@"\s+"))
            {
                MessageBox.Show("Enter your name first");
            }
            else if(Password.Password == "" || Regex.IsMatch(Password.Password, @"\s+"))
            {
                MessageBox.Show("Enter your password first");
            }
            else
            {
                CurUser = LoginBox.Text;
                if(Password.Password == admin_pass)
                {
                    role = Role.admin;
                }
                MyLogger.Log.Info($"User {CurUser} logged in as {Enum.GetName(typeof(Role),role)}");
                //MainWindow main = new MainWindow(CurUser);
                MainFrame.Content = new Heroes(CurUser, in MainFrame, role);
            }

        }
        
        private void LoginBox_MouseEnter(object sender, MouseEventArgs e)
        {
            var tbox = sender as TextBox;
            if (tbox.Text == "Enter your name")
                tbox.Text = "";

        }

        private void LoginBox_MouseLeave(object sender, MouseEventArgs e)
        {
            var tbox = sender as TextBox;
            if (tbox.Text == "" && !focused)
                tbox.Text = "Enter your name";
        }

        private void LoginBox_GotFocus(object sender, RoutedEventArgs e)
        {
            focused = true;
        }

        private void LoginBox_LostFocus(object sender, RoutedEventArgs e)
        {
            focused = false;
            if (LoginBox.Text == "")
                LoginBox.Text = "Enter your name";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "Enter your name" || LoginBox.Text == "" || Regex.IsMatch(LoginBox.Text, @"\s+"))
            {
                MessageBox.Show("Enter your name first");
            }
            else if (Password.Password == "" || Regex.IsMatch(Password.Password, @"\s+"))
            {
                MessageBox.Show("Enter your password first");
            }
            else
            {
                CurUser = LoginBox.Text;
                if (Password.Password == admin_pass)
                {
                    role = Role.admin;
                    MyLogger.Log.Info($"User {CurUser} logged in as {Enum.GetName(typeof(Role), role)}");
                    //MainWindow main = new MainWindow(CurUser);
                    MainFrame.Content = new talents(CurUser, in MainFrame, this);
                }
                else MessageBox.Show($"{CurUser}, you dont have access to this opereation");

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "Enter your name" || LoginBox.Text == "" || Regex.IsMatch(LoginBox.Text, @"\s+"))
            {
                MessageBox.Show("Enter your name first");
            }
            else if (Password.Password == "" || Regex.IsMatch(Password.Password, @"\s+"))
            {
                MessageBox.Show("Enter your password first");
            }
            else
            {
                CurUser = LoginBox.Text;
                if (Password.Password == admin_pass)
                {
                    role = Role.admin;
                    MyLogger.Log.Info($"User {CurUser} logged in as {Enum.GetName(typeof(Role), role)}");
                    //MainWindow main = new MainWindow(CurUser);
                    MainFrame.Content = new spelltype(CurUser, in MainFrame, this);
                }
                else MessageBox.Show($"{CurUser}, you dont have access to this opereation");
 }
        }
    }
    public enum Role { user = 1, admin };
}
