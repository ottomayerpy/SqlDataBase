using System.Windows;
using System.Windows.Input;

namespace SqlDataBase
{
    public partial class PasswordRequestWindow
    {
        private bool Success;

        public bool Status()
        {
            return Success;
        }

        public PasswordRequestWindow()
        {
            InitializeComponent();
        }

        private void ButtonContinue_Click(object sender, RoutedEventArgs e)
        {
            Continue();
        }

        private void PasswordBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Continue();
            }
        }

        private void Continue()
        {
            if (Sql.CheckPasswordUser(PasswordBoxPassword.Password))
            {
                Success = true;
                Close();
            }
            else
            {
                MessageBox.Show("Не правильный пароль. Повторите попытку.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                PasswordBoxPassword.Password = string.Empty;
            }
        }
    }
}
