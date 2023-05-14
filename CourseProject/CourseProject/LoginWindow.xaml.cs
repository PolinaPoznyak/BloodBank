using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CourseProject
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public List list = new List();

        public LoginWindow()
        {
            InitializeComponent();
        }

        #region Window Events

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Minimise_Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void username_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (username.Text.Equals("Username"))
            {
                username.Clear();
                username.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void username_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (username.Text.Equals(""))
            {
                username.Foreground = new SolidColorBrush(Colors.LightGray);
                username.Text = "Username";
            }
        }

        private void password_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (passwordInput.Text.Equals("Password"))
            {
                passwordInput.Clear();
                passwordInput.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void password_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (passwordInput.Text.Equals(""))
            {
                passwordInput.Foreground = new SolidColorBrush(Colors.LightGray);
                passwordInput.Text = "Password";
            }
        }

        #endregion


        private void signUP_Click(object sender, RoutedEventArgs e)
        {
            SignUpWindow signup = new SignUpWindow();
            signup.Show();
            Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (username.Text == null || passwordInput.Text == null)
            {
                MessageBox.Show("Enter username and password!");
            }
            else
            {
                using (Entities ent = new Entities())
                {
                    var adminPassword = "qwerty";
                    foreach (var adminItem in ent.GETACCOUNTS())
                    {
                        if (adminItem.LOGIN == "admin")
                        {
                            adminPassword = adminItem.PASSWORD;
                        }
                    }
                    if (username.Text.ToLower() == "admin" && passwordInput.Text.ToLower() == adminPassword)
                    {
                        MainWindow admin = new MainWindow();
                        admin.Show();
                        Close();
                    }
                    else
                    {
                        bool Ishere = false;
                        foreach (var clientItem in ent.GETACCOUNTS())
                        {
                            if (username.Text.ToLower() == clientItem.LOGIN && passwordInput.Text.ToLower() == clientItem.PASSWORD)
                            {
                                MainClientWindow user = new MainClientWindow();
                                user.Show();
                                Close();
                                Ishere = true;
                            }
                        }
                        if (!Ishere)
                        {
                            MessageBox.Show("There is no such account. Register or enter correct data!");
                        }
                    }
                }
            }
        }
    }
}
