using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SqlDataBase
{
    public partial class LoginWindow
    {
        public LoginWindow()
        {
            InitializeComponent();
            Width = 290;
            GridMain.Margin = new Thickness(-290, 0, 0, 0);
            GridRegistration.Visibility = Visibility.Collapsed;
            GridSettings.Visibility = Visibility.Collapsed;

            ComboBoxServer.Items.Add(Properties.Settings.Default.Server);
            ComboBoxServer.SelectedIndex = 0;
        }

        private async void ServerSearch()
        {
            await Task.Run(() => SStask());
        }

        private void SStask()
        {
            Dispatcher.Invoke(() =>
            {
                ComboBoxServer.Items.Clear();
                foreach (string name in Sql.GetServers()) // Получаем список доступных серверов из метода Sql.GetServers
                {
                    ComboBoxServer.Items.Add(name); // Добавляем сервер в выпадающий список
                }

                ComboBoxServer.SelectedIndex = 0; // Устанавливаем первый сервер как дефолтное значение
            });
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxServer.Text == string.Empty)
            {
                MessageBox.Show("Не указан сервер. Перейдите в настройки и укажите сервер для подключения к базе данных.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (TextBoxLogin.Text == string.Empty)
            {
                MessageBox.Show("Не указан логин. Поле логин не может быть пустым.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (TextBoxPassword.Password == string.Empty)
            {
                MessageBox.Show("Не указан пароль. Поле пароль не может быть пустым.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                // Проверяем логин и пароль в методе Sql.Connect
                if (Sql.Login(TextBoxLogin.Text, TextBoxPassword.Password)) // Если успех, то...
                {
                    Visibility = Visibility.Collapsed; // Переводим окно в режим невидимости
                    // Чистим текстовые поля
                    TextBoxLogin.Text = null;
                    TextBoxPassword.Password = null;
                    new MainWindow().ShowDialog(); // Открываем основное окно, и ждем его закрытия
                    if (SessionUser.Logout) // Если мы нажали кнопку выход
                    {
                        Visibility = Visibility.Visible; // Переводим окно в режим видимости
                        SessionUser.Logout = false;
                    }
                    else // В противном случае закрываем программу
                    {
                        Close();
                    }
                }
            }
        }

        private void ButtonRegistered_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxServer.Text != string.Empty)
            {
                if (TextBoxRegName.Text == string.Empty)
                {
                    MessageBox.Show("Заполните поле \"Имя\".", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (TextBoxRegSubname.Text == string.Empty)
                {
                    MessageBox.Show("Заполните поле \"Фамилия\".", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (TextBoxRegLogin.Text == string.Empty)
                {
                    MessageBox.Show("Заполните поле \"Логин\".", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (PasswordBoxRegPassword.Password == string.Empty)
                {
                    MessageBox.Show("Заполните поле \"Пароль\".", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (PasswordBoxRegPassword.Password.Length < 6)
                {
                    MessageBox.Show("Пароль должен содержать как минимум 6 символов.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (Sql.Registration(TextBoxRegLogin.Text, PasswordBoxRegPassword.Password, TextBoxRegName.Text, TextBoxRegSubname.Text, Const.User))
                {
                    MessageBox.Show("Регистрация прошла успешно.", "Успех");
                    GridMainAnimation(-290, new Grid[] { GridRegistration, GridSettings }); // Вернутся к основной сетке
                }
            }
            else
            {
                MessageBox.Show("Не указан сервер. Перейдите в настройки и укажите сервер для подключения к базе данных.", "Ошибка ввода");
            }
        }

        private void ButtonShowRegistration_Click(object sender, RoutedEventArgs e)
        {
            GridRegistration.Visibility = Visibility.Visible;
            GridMainAnimation(0, new Grid[] { GridLogin }); // Показать сетку регистрации
        }

        private void ButtonShowSettings_Click(object sender, RoutedEventArgs e)
        {
            GridSettings.Visibility = Visibility.Visible;
            GridMainAnimation(-575, new Grid[] { GridLogin }); // Показать сетку настроек
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            GridLogin.Visibility = Visibility.Visible;
            GridMainAnimation(-290, new Grid[] { GridRegistration, GridSettings }); // Вернуться к основной сетке
        }

        // Анимация сетки окна
        private void GridMainAnimation(double point, Grid[] grids)
        {
            ThicknessAnimation animation = new ThicknessAnimation // Новая анимация
            {
                From = new Thickness(GridMain.Margin.Left, GridMain.Margin.Top, GridMain.Margin.Right, GridMain.Margin.Bottom), // Стартовая позиция
                To = new Thickness(point, GridMain.Margin.Top, GridMain.Margin.Right, GridMain.Margin.Bottom), // Конечная позиция
                Duration = TimeSpan.FromSeconds(1), // Время
                SpeedRatio = 7 // Скорость
            };

            animation.Completed += (s, a) => GridsVisibility(grids);

            GridMain.BeginAnimation(MarginProperty, animation); // Запуск
        }

        private void GridsVisibility(Grid[] grids)
        {
            foreach (Grid grid in grids)
            {
                grid.Visibility = Visibility.Collapsed;
            }
        }

        private void ButtonRefreshServerList_Click(object sender, RoutedEventArgs e)
        {
            GridExpectations.Visibility = Visibility.Visible;
            GridExpectationsAnimation(0.9);
            ServerSearch();
            GridExpectationsAnimation(0);
        }

        // Анимация появления серой сетки с сообщением "Ожидайте..."
        // Успевает быть увиденной в том случае, если, поиск ms sql servers занимает какое-то внушительное время
        private void GridExpectationsAnimation(double opacity)
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                To = opacity,
                BeginTime = TimeSpan.FromSeconds(0),
                Duration = TimeSpan.FromSeconds(0.5),
            };

            // Чтобы сетка не перекрывала другие контролы, руиним ее
            if (opacity == 0)
            {
                animation.Completed += (s, a) => GridExpectations.Visibility = Visibility.Collapsed;
            }

            GridExpectations.BeginAnimation(OpacityProperty, animation); // Запуск анимации
        }

        private void ButtonSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            // Сохраняем настройки
            Properties.Settings.Default.Server = ComboBoxServer.Text;
            Properties.Settings.Default.Save();
        }
    }
}
