using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CourseProject
{
    /// <summary>
    /// Логика взаимодействия для SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
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

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow signup = new LoginWindow();
            signup.Show();
            Close();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (username.Text == "" || passwordInput.Text.Length == 0)
            {
                MessageBox.Show("Type data in every input!");
            }
            else
            {
                bool IsHere = false;
                using (Entities ent = new Entities())
                {
                    foreach (var item in ent.GETACCOUNTS())
                    {
                        if (item.LOGIN == username.Text)
                            IsHere = true;
                    }
                    if (!IsHere)
                    {
                        ent.ADD_NEW_ACCOUNT(username.Text, passwordInput.Text);
                        MessageBox.Show("CLient has been successfully added!");
                    }
                    else
                    {
                        MessageBox.Show("This Client is already signed in or this login is taken");
                    }
                }
            }
        }
    }
}
