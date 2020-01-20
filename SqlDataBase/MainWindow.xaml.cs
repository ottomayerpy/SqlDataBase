using Microsoft.Win32;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace SqlDataBase
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ArrayList items = Sql.GetGroupList(); // Получаем список групп из базы данных

            if (items != null)
            {
                ListViewGroup.Items.Add("Все"); // Добавляем в список групп пункт "Все" чтобы можно было отобразить студентов из всех групп
                foreach (string item in items)
                {
                    ListViewGroup.Items.Add(item); // Добавляем список групп
                }
            }
            else
            {
                MessageBox.Show("Ошибка загрузки групп. Похоже что информация повреждена или отсутствует.", "Ошибка сервера", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            GridDefaultUser.Visibility = Visibility.Hidden; // Скрываем форму анкеты

            // Если делать присваивание событие из конструктора то выползет исключение, потому, что при построении срабатывает событие Checked
            // и начинает выполнять код в котором завязаны манипуляции с другими контролами которые не успели построиться, поэтому ждем полной загрузки окна.
            Loaded += (s, a) => // После полной загрузки окна...
            {
                RadioButtonStudent.Checked += RadioButtonStudent_Checked; // Присваиваем событие
            };
        }

        private void UpdateStudentList()
        {
            ListViewStudent.Items.Clear(); // Чистим список студентов

            ArrayList list = Sql.GetTable($"GroupName = '{ListViewGroup.SelectedItem.ToString()}'"); // Получаем список студентов

            foreach (string item in list)
            {
                ListViewStudent.Items.Add(item); // Добавляем на форму студентов
            }

            GridDefaultUser.Visibility = Visibility.Hidden; // Скрываем форму анкеты
        }

        private void LoadInfo(string GenderParents = null)
        {
            try
            {
                /*TextBoxName.Text = string.Empty;
                TextBoxSubname.Text = string.Empty;
                TextBoxPatronymic.Text = string.Empty;
                DatePickerDateBirth.Text = string.Empty;
                TextBoxLocation.Text = string.Empty;
                TextBoxGroupNameOrPlaceWork.Text = string.Empty;
                TextBoxAdditional.Text = string.Empty;
                TextBoxPhone.Text = string.Empty;
                RadioButtonGenderMan.IsChecked = false;
                RadioButtonGenderWoman.IsChecked = false;
                LoadPhoto(null, true);*/

                if (ButtonChangeAvatar.Visibility == Visibility.Hidden && ButtonDefaultAvatar.Visibility == Visibility.Hidden)
                {
                    ButtonChangeAvatar.Visibility = Visibility.Visible;
                    ButtonDefaultAvatar.Visibility = Visibility.Visible;
                    ButtonProfileDelete.IsEnabled = true;
                    ButtonProfileSave.IsEnabled = true;
                }

                Session.Id = Convert.ToInt32(Session.StudentsId[ListViewStudent.SelectedIndex]); // Получаем айдишник студента
                Info info = null;

                if (GenderParents == null)
                {

                    info = Sql.GetInfo(Session.Id);

                    if ((bool)!RadioButtonStudent.IsChecked)
                    {
                        RadioButtonStudent.IsChecked = true;
                    }
                }
                else
                {
                    info = Sql.GetInfo(Session.Id, GenderParents);

                    if (info == null) // Если инфы о родителе нет...
                    {
                        ButtonChangeAvatar.Visibility = Visibility.Hidden;
                        ButtonDefaultAvatar.Visibility = Visibility.Hidden;
                        ButtonProfileDelete.IsEnabled = false;
                        ButtonProfileSave.IsEnabled = false;
                        return;
                    }
                }

                TextBoxName.Text = info.Name;
                TextBoxSubname.Text = info.Subname;
                TextBoxPatronymic.Text = info.Patronymic;
                DatePickerDateBirth.Text = info.DateBirth;
                TextBoxLocation.Text = info.Location;
                TextBoxGroupNameOrPlaceWork.Text = info.GroupNameOrPlaceWork;
                TextBoxAdditional.Text = info.Additional;
                TextBoxPhone.Text = info.Phone;

                if (info.Gender == Const.Man)
                {
                    RadioButtonGenderMan.IsChecked = true;
                }
                else if (info.Gender == Const.Woman)
                {
                    RadioButtonGenderWoman.IsChecked = true;
                }

                Session.CurrentInfo = info;

                LoadPhoto(Const.Photo, info.Id); // Подгружаем фотографию

                GridDefaultUser.Visibility = Visibility.Visible; // Включаем отображение анкеты
            }
            catch (ArgumentOutOfRangeException)
            {
                /// Except: Temp.StudentsId[ListBoxStudent.SelectedIndex];
                // Исключение при изменении выбора группы если уже загруженна какая либо анкета
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Critical Error - Exeption log", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPhoto(string photo, int castomid = 0) // Метод загрузки фото на форму
        {
            BitmapImage image;

            if (castomid == 0)
            {
                image = Code.LoadImage(Sql.GetAvatar(Session.Id, photo, Const.Students)); // Получаем от сервера массив байт представляющий фото в методе Code.LoadImage преобразуем в BitmapImage
            }
            else
            {
                image = Code.LoadImage(Sql.GetAvatar(castomid, photo, Const.Parents));
            }

            ImageAvatar.ImageSource = image; // Заполняем форму полученным от сервера фото
        }

        private void ButtonLogout_Click(object sender, RoutedEventArgs e) // Событие кнопки выхода
        {
            SessionUser.Logout = true; // Передаем что мы нажали кнопку выхода
            Close(); // Закрываем текущее окно
        }

        private void ButtonChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PNG|*.png|JPG|*.jpg|JPEG|*.jpeg" // Задаем фильт форматов файлов для диалогового окна
            };

            if (openFileDialog.ShowDialog() == true) // Открываем окно выбора изображения, если фото выбранно и нажата кнопка открыть...
            {
                Bitmap image = null;

                try
                {
                    image = new Bitmap(openFileDialog.FileName); // Инициализируем новое изображение из указанного файла

                    if (new FileInfo(openFileDialog.FileName).Length > 5e+6) // Если размер файла превышает 5 мб...
                    {
                        MessageBox.Show("Размер изображения не должен превышать 5 мегабайт", "Ошибка загрузки данных", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (image.Size.Height < 150 || image.Size.Width < 150) // Если высота или ширина меньше 150 пикселей...
                        {
                            MessageBox.Show("Размер изображение не должн быть меньше 150x150 пикселей", "Ошибка загрузки данных", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }

                        string type = string.Empty;
                        BitmapImage img = Code.LoadImage(Code.ImageToByte(image));

                        int photonumber = Session.CurrentInfo.Photos.NumberSelectedPhoto;
                        if (photonumber == 1)
                        {
                            type = Const.Photo;
                            Session.CurrentInfo.Photos.Photo = img;
                        }
                        else if (photonumber == 2)
                        {
                            type = Const.PhotoPassport;
                            Session.CurrentInfo.Photos.PhotoPassport = img;
                        }
                        else if (photonumber == 3)
                        {
                            type = Const.PhotoSchoolCertificate;
                            Session.CurrentInfo.Photos.PhotoSchoolCertificate = img;
                        }
                        else if (photonumber == 4)
                        {
                            type = Const.PhotoBirthCertificate;
                            Session.CurrentInfo.Photos.PhotoBirthCertificate = img;
                        }

                        Sql.UpdateStudentInfo(Session.Id, type, Code.ImageToByte(image)); // Конвертируем изображение в массив байт и отправляем на sql server
                        LoadPhoto(type); // Загружаем фото
                    }
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Данный файл поврежден или не является изображением", "Ошибка загрузки данных", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                finally
                {
                    image.Dispose();
                }
            }
        }

        private void ButtonProfileSave_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxName.Text != Session.CurrentInfo.Name)
            {
                Sql.UpdateStudentInfo(Session.Id, Const.Name, TextBoxName.Text);
            }

            if (TextBoxSubname.Text != Session.CurrentInfo.Subname)
            {
                Sql.UpdateStudentInfo(Session.Id, Const.Subname, TextBoxSubname.Text);
            }

            if (TextBoxPatronymic.Text != Session.CurrentInfo.Patronymic)
            {
                Sql.UpdateStudentInfo(Session.Id, Const.Patronymic, TextBoxPatronymic.Text);
            }

            if (TextBoxLocation.Text != Session.CurrentInfo.Location)
            {
                Sql.UpdateStudentInfo(Session.Id, Const.Location, TextBoxLocation.Text);
            }

            if (TextBoxGroupNameOrPlaceWork.Text != Session.CurrentInfo.GroupNameOrPlaceWork)
            {
                Sql.UpdateStudentInfo(Session.Id, Const.GroupName, TextBoxGroupNameOrPlaceWork.Text);
            }

            if (TextBoxAdditional.Text != Session.CurrentInfo.Additional)
            {
                Sql.UpdateStudentInfo(Session.Id, Const.Additional, TextBoxAdditional.Text);
            }

            if (DatePickerDateBirth.Text != Session.CurrentInfo.DateBirth)
            {
                Sql.UpdateStudentInfo(Session.Id, Const.DateBirth, DatePickerDateBirth.Text);
            }

            if ((bool)RadioButtonGenderMan.IsChecked)
            {
                if (Const.Man != Session.CurrentInfo.Gender)
                {
                    Sql.UpdateStudentInfo(Session.Id, Const.Gender, Const.Man);
                }
            }
            else
            {
                if (Const.Woman != Session.CurrentInfo.Gender)
                {
                    Sql.UpdateStudentInfo(Session.Id, Const.Gender, Const.Woman);
                }
            }
        }

        private void ButtonProfileDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить данную анкету?\nПосле удаление стираются данные о родителях/опекунах", "Удаление",
                MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel) == MessageBoxResult.OK)
            {
                Sql.DeleteStudent();
                UpdateStudentList();
            }
        }

        private void ButtonProfileReport_Click(object sender, RoutedEventArgs e)
        {
            new ReportWindow(true, Sql.GetInfo(Session.Id), Sql.GetInfo(Session.Id, Const.Man), Sql.GetInfo(Session.Id, Const.Woman)).ShowDialog();
        }

        private void ButtonDefaultAvatar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить данную фотографию?", "Удаление",
                MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel) == MessageBoxResult.OK)
            {
                Sql.UpdateStudentInfo(Session.Id, Const.Photo, string.Empty);
                LoadPhoto(Const.Photo);
            }
        }

        private void ButtonOpenFullImage_Click(object sender, RoutedEventArgs e)
        {
            int number = Session.CurrentInfo.Photos.NumberSelectedPhoto;
            new ReportWindow(false, (bool)RadioButtonStudent.IsChecked).ShowDialog();
            Session.CurrentInfo.Photos.NumberSelectedPhoto = number;
        }

        private void RadioButtonStudent_Checked(object sender, RoutedEventArgs e)
        {
            LoadInfo();
            RadioButtonGenderMan.IsEnabled = true;
            RadioButtonGenderWoman.IsEnabled = true;
            LabelGroupAndPlacework.Content = "Группа:";
            InfoPhotosEditing.Default(Session.CurrentInfo.Photos);
            LabelImageAvatar.Content = InfoPhotosEditing.GetNameSelectedPhoto(Session.CurrentInfo.Photos);
        }

        private void RadioButtonDad_Checked(object sender, RoutedEventArgs e)
        {
            LoadInfo(Const.Man);
            RadioButtonGenderMan.IsEnabled = false;
            RadioButtonGenderWoman.IsEnabled = false;
            LabelGroupAndPlacework.Content = "Работа:";
            InfoPhotosEditing.Default(Session.CurrentInfo.Photos);
            LabelImageAvatar.Content = InfoPhotosEditing.GetNameSelectedPhoto(Session.CurrentInfo.Photos);
        }

        private void RadioButtonMom_Checked(object sender, RoutedEventArgs e)
        {
            LoadInfo(Const.Woman);
            RadioButtonGenderMan.IsEnabled = false;
            RadioButtonGenderWoman.IsEnabled = false;
            LabelGroupAndPlacework.Content = "Работа:";
            InfoPhotosEditing.Default(Session.CurrentInfo.Photos);
            LabelImageAvatar.Content = InfoPhotosEditing.GetNameSelectedPhoto(Session.CurrentInfo.Photos);
        }

        private void ListViewGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateStudentList(); // Событие изменения выбора группы
        }

        private void ListViewStudent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadInfo(); // Событие изменения выбора студента
        }

        private void ButtonImageBack_Click(object sender, RoutedEventArgs e)
        {
            ImageAvatar.ImageSource = InfoPhotosEditing.SelectPhoto(false, (bool)RadioButtonStudent.IsChecked, Session.CurrentInfo.Photos);
            LabelImageAvatar.Content = InfoPhotosEditing.GetNameSelectedPhoto(Session.CurrentInfo.Photos);
        }

        private void ButtonImageNext_Click(object sender, RoutedEventArgs e)
        {
            ImageAvatar.ImageSource = InfoPhotosEditing.SelectPhoto(true, (bool)RadioButtonStudent.IsChecked, Session.CurrentInfo.Photos);
            LabelImageAvatar.Content = InfoPhotosEditing.GetNameSelectedPhoto(Session.CurrentInfo.Photos);
        }

        private void GridMainAnimation(double point, bool completed = false)
        {
            ThicknessAnimation animation = new ThicknessAnimation // Новая анимация
            {
                From = new Thickness(GridMain.Margin.Left, GridMain.Margin.Top, GridMain.Margin.Right, GridMain.Margin.Bottom), // Стартовая позиция
                To = new Thickness(point, GridMain.Margin.Top, GridMain.Margin.Right, GridMain.Margin.Bottom), // Конечная позиция
                Duration = TimeSpan.FromSeconds(1), // Время
                SpeedRatio = 4, // Скорость
            };

            if (completed)
            {
                animation.Completed += Animation_Completed;
            }

            GridMain.BeginAnimation(MarginProperty, animation); // Запуск
        }

        private void Animation_Completed(object sender, EventArgs e)
        {
            if (GridMain.Margin == new Thickness(-2000, 0, 0, 0))
            {
                if ((string)LabelCreateStep.Content == "Студент")
                {
                    SetCreateInfo("Student");
                    ButtonCreateBack.IsEnabled = true;
                    LabelCreateStep.Content = "Отец";
                    LabelCreateGroupAndPlacework.Content = "Работа:";

                    LabelCreateGender.Visibility = Visibility.Hidden;
                    RadioButtonCreateGenderMan.Visibility = Visibility.Hidden;
                    RadioButtonCreateGenderWoman.Visibility = Visibility.Hidden;
                    LabelCreateAdditional.Margin = new Thickness(10, 228, 0, 0);
                    TextBoxCreateAdditional.Margin = new Thickness(115, 228, 0, 0);

                    GetCreateInfo("Dad");
                }
                else if ((string)LabelCreateStep.Content == "Отец")
                {
                    SetCreateInfo("Dad");
                    LabelCreateStep.Content = "Мать";
                    ButtonCreateContinue.Content = "Создать";
                    GetCreateInfo("Mom");
                }
                else if ((string)LabelCreateStep.Content == "Мать")
                {
                    SetCreateInfo("Mom");
                    if (Sql.NewStudent())
                    {
                        TabControlMain.SelectedItem = TabItemHome;
                    }

                    ClearCreateTextBoxes();
                    CreatePageDefault();
                    Session.CreateStudentInfo = new Info();
                    Session.CreateDadInfo = new Info();
                    Session.CreateMomInfo = new Info();
                    ButtonCreateContinue.Content = "Продолжить";
                }
            }
            else
            {
                if ((string)LabelCreateStep.Content == "Отец")
                {
                    SetCreateInfo("Dad");
                    CreatePageDefault();
                    GetCreateInfo("Student");
                }
                else if ((string)LabelCreateStep.Content == "Мать")
                {
                    SetCreateInfo("Mom");
                    LabelCreateStep.Content = "Отец";
                    ButtonCreateContinue.Content = "Продолжить";
                    GetCreateInfo("Dad");
                }
            }

            GridMainAnimation(0);
        }

        private void CreatePageDefault()
        {
            LabelCreateStep.Content = "Студент";
            LabelCreateGroupAndPlacework.Content = "Группа:";
            ButtonCreateBack.IsEnabled = false;

            LabelCreateGender.Visibility = Visibility.Visible;
            RadioButtonCreateGenderMan.Visibility = Visibility.Visible;
            RadioButtonCreateGenderWoman.Visibility = Visibility.Visible;
            LabelCreateAdditional.Margin = new Thickness(10, 258, 0, 0);
            TextBoxCreateAdditional.Margin = new Thickness(115, 258, 0, 0);
        }

        private void SetCreateInfo(string info)
        {
            Info inf = new Info
            {
                Name = TextBoxCreateName.Text,
                Subname = TextBoxCreateSubname.Text,
                Patronymic = TextBoxCreatePatronymic.Text,
                DateBirth = DatePickerCreateDateBirth.Text,
                Location = TextBoxCreateLocation.Text,
                GroupNameOrPlaceWork = TextBoxCreateGroupNameOrPlaceWork.Text,
                Phone = TextBoxCreatePhone.Text,
                Additional = TextBoxCreateAdditional.Text
            };
            if ((bool)RadioButtonCreateGenderMan.IsChecked)
            {
                inf.Gender = Const.Man;
            }
            else
            {
                inf.Gender = Const.Woman;
            }

            if (info == "Student")
            {
                Session.CreateStudentInfo = inf;
            }
            else if (info == "Dad")
            {
                Session.CreateDadInfo = inf;
            }
            else
            {
                Session.CreateMomInfo = inf;
            }
        }

        private void GetCreateInfo(string info)
        {
            Info inf;
            if (info == "Student")
            {
                inf = Session.CreateStudentInfo;
            }
            else if (info == "Dad")
            {
                inf = Session.CreateDadInfo;
            }
            else
            {
                inf = Session.CreateMomInfo;
            }

            if (inf != null)
            {
                TextBoxCreateName.Text = inf.Name;
                TextBoxCreateSubname.Text = inf.Subname;
                TextBoxCreatePatronymic.Text = inf.Patronymic;
                DatePickerCreateDateBirth.Text = inf.DateBirth;
                TextBoxCreateLocation.Text = inf.Location;
                TextBoxCreateGroupNameOrPlaceWork.Text = inf.GroupNameOrPlaceWork;
                TextBoxCreatePhone.Text = inf.Phone;
                if (inf.Gender == Const.Man)
                {
                    RadioButtonCreateGenderMan.IsChecked = true;
                }
                else
                {
                    RadioButtonCreateGenderWoman.IsChecked = true;
                }

                TextBoxCreateAdditional.Text = inf.Additional;
            }
            else
            {
                ClearCreateTextBoxes();
            }
        }

        private void ClearCreateTextBoxes()
        {
            TextBoxCreateName.Text = TextBoxCreateSubname.Text = TextBoxCreatePatronymic.Text = DatePickerCreateDateBirth.Text = TextBoxCreateLocation.Text = TextBoxCreateGroupNameOrPlaceWork.Text = TextBoxCreatePhone.Text = TextBoxCreateAdditional.Text = string.Empty;
        }

        private void ButtonCreateContinue_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxCreateName.Text == string.Empty)
            {
                MessageBox.Show("Не указано имя.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (TextBoxCreateSubname.Text == string.Empty)
            {
                MessageBox.Show("Не указана фамилия.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (DatePickerCreateDateBirth.Text == string.Empty)
            {
                MessageBox.Show("Не указана дата рождения.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (TextBoxCreateLocation.Text == string.Empty)
            {
                MessageBox.Show("Не указан город.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (TextBoxCreateGroupNameOrPlaceWork.Text == string.Empty)
            {
                if ((string)LabelCreateStep.Content == "Студент")
                {
                    MessageBox.Show("Не указана группа.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    MessageBox.Show("Не указано место работы.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else if (TextBoxCreatePhone.Text == string.Empty)
            {
                MessageBox.Show("Не указан телефон.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if ((bool)!RadioButtonCreateGenderMan.IsChecked && (bool)!RadioButtonCreateGenderWoman.IsChecked && (string)LabelCreateStep.Content == "Студент")
            {
                MessageBox.Show("Не указан пол.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                GridMainAnimation(-2000, true);
            }
        }

        private void ButtonCreateBack_Click(object sender, RoutedEventArgs e)
        {
            GridMainAnimation(2000, true);
        }

        private void ButtonCreateNewStudent_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxName.Text == string.Empty)
            {
                MessageBox.Show("Не заполнено поле \"Имя\".", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (TextBoxSubname.Text == string.Empty)
            {
                MessageBox.Show("Не заполнено поле \"Фамилия\".", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (DatePickerDateBirth.Text == string.Empty)
            {
                MessageBox.Show("Не заполнено поле \"Дата рождения\".", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (TextBoxLocation.Text == string.Empty)
            {
                MessageBox.Show("Не заполнено поле \"Город\".", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (TextBoxGroupNameOrPlaceWork.Text == string.Empty)
            {
                if ((string)LabelGroupAndPlacework.Content == "Группа:")
                {
                    MessageBox.Show("Не заполнено поле \"Группа\".", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    MessageBox.Show("Не заполнено поле \"Работа\".", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else if ((bool)!RadioButtonGenderMan.IsChecked && (bool)!RadioButtonGenderWoman.IsChecked && (bool)RadioButtonStudent.IsChecked)
            {
                MessageBox.Show("Не заполнено поле \"Пол\".", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if ((bool)RadioButtonStudent.IsChecked)
                {
                    string gender = string.Empty;
                    if ((bool)RadioButtonGenderMan.IsChecked)
                    {
                        gender = Const.Man;
                    }
                    else if ((bool)RadioButtonGenderWoman.IsChecked)
                    {
                        gender = Const.Woman;
                    }

                    string con = $"INSERT INTO Students (Name, Subname, Patronymic, DateBirth, Location, GroupName, Gender, Additional) VALUES " +
                        $"('{TextBoxName.Text}', '{TextBoxSubname.Text}', '{TextBoxPatronymic.Text}', '{Code.DateFormatToSqlServer(DatePickerDateBirth.Text)}', '{TextBoxLocation.Text}', '{TextBoxGroupNameOrPlaceWork.Text}', '{gender}', '{TextBoxAdditional.Text}')";

                    Sql.UpdateStudentInfo(0, Const.All, con);
                }
                else
                {
                    string gender = string.Empty;
                    if ((bool)RadioButtonDad.IsChecked)
                    {
                        gender = Const.Man;
                    }
                    else if ((bool)RadioButtonMom.IsChecked)
                    {
                        gender = Const.Woman;
                    }

                    string con = $"INSERT INTO Parents (Name, Subname, Patronymic, DateBirth, Location, Gender, PlaceWork, Additional, StudentId) VALUES " +
                            $"('{TextBoxName.Text}', '{TextBoxSubname.Text}', '{TextBoxPatronymic.Text}', '{Code.DateFormatToSqlServer(DatePickerDateBirth.Text)}', '{TextBoxLocation.Text}', '{gender}', '{TextBoxGroupNameOrPlaceWork.Text}', '{TextBoxAdditional.Text}', '{Session.Id}')";

                    Sql.UpdateStudentInfo(0, Const.All, con);
                }
                UpdateStudentList();
            }
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0)) // Ввод только цифр
            {
                e.Handled = true;
            }
        }

        private void ButtonSetSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxSetLogin.Text == string.Empty)
            {
                MessageBox.Show("Логин не может быть пустым. Повторите попытку.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (TextBoxSetName.Text == string.Empty)
            {
                MessageBox.Show("Имя не может быть пустим. Повторите попытку.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (TextBoxSetSubname.Text == string.Empty)
            {
                MessageBox.Show("Фамилия не может быть пустой. Повторите попытку.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (PasswordBoxSetPassword.Password != string.Empty && PasswordBoxSetPassword.Password.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать как минимум 6 символов", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (PasswordBoxSetPassword.Password != string.Empty && PasswordBoxSetPassword.Password != PasswordBoxSetConfirmPassword.Password)
            {
                MessageBox.Show("Не правильное подтверждение пароля. Повторите попытку.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                PasswordBoxSetConfirmPassword.Password = string.Empty;
            }
            else
            {
                PasswordRequestWindow window = new PasswordRequestWindow();
                window.ShowDialog();

                if (window.Status())
                {
                    User user = Sql.GetUser();
                    user.Login = TextBoxSetLogin.Text;
                    user.Password = PasswordBoxSetPassword.Password;
                    user.Name = TextBoxSetName.Text;
                    user.Subname = TextBoxSetSubname.Text;
                    if (Sql.UpdateUser(user))
                    {
                        MessageBox.Show("Настройки успешно применены", "Успех");
                    }
                }
            }
        }

        private void TabControlMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabControlMain.SelectedIndex == 2) // При открытии вкладки настроек, загружаем их
            {
                User user = Sql.GetUser();
                TextBoxSetLogin.Text = user.Login;
                TextBoxSetName.Text = user.Name;
                TextBoxSetSubname.Text = user.Subname;
            }
        }
    }
}
