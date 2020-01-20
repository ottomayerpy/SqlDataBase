using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Word = Microsoft.Office.Interop.Word;

namespace SqlDataBase
{
    public partial class ReportWindow
    {
        private readonly bool IsRB;
        private readonly bool[] IsStudent = { true, true, true, false, false, false, false, false, false, false };
        private readonly bool[] IsDad = { true, true, true, false, false, false, false, false, false, false };
        private readonly bool[] IsMom = { true, true, true, false, false, false, false, false, false, false };
        private readonly Info InfStudent;
        private readonly Info InfDad;
        private readonly Info InfMom;

        public ReportWindow(bool IsReport, params object[] param)
        {
            InitializeComponent();
            if (IsReport)
            {
                Title = "Формировние отчета";
                InfStudent = (Info)param[0];
                InfDad = (Info)param[1];
                InfMom = (Info)param[2];

                if (InfDad == null)
                {
                    RadioButtonDad.IsEnabled = false;
                }

                if (InfMom == null)
                {
                    RadioButtonMom.IsEnabled = false;
                }

                Loaded += (s, a) => // После полной загрузки окна...
                {
                    RadioButtonStudent.Checked += RadioButtonStudent_Checked; // Присваиваем событие
                    RadioButtonStudent_Event();
                };
            }
            else
            {
                Title = "Просмотр фотографий";
                WindowState = WindowState.Maximized;
                ImageShowFull.Source = InfoPhotosEditing.GetBitmapNumber(Session.CurrentInfo.Photos);
                IsRB = (bool)param[0];
                GridShowImage.Visibility = Visibility.Visible;
                GridShowReport.Visibility = Visibility.Hidden;
            }
        }

        private void ButtonImageBack_Click(object sender, RoutedEventArgs e)
        {
            ImageShowFull.Source = InfoPhotosEditing.SelectPhoto(false, IsRB, Session.CurrentInfo.Photos);
        }

        private void ButtonImageNext_Click(object sender, RoutedEventArgs e)
        {
            ImageShowFull.Source = InfoPhotosEditing.SelectPhoto(true, IsRB, Session.CurrentInfo.Photos);
        }

        private void ButtonPrintImage_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if ((bool)printDialog.ShowDialog())
            {
                printDialog.PrintVisual(ImageShowFull, "document image");
            }
        }

        private void AutoGenerateGridsPosition()
        {
            Grid[] Grids = { GridName, GridSubname, GridPatronymic, GridDateBirth, GridLocation, GridGroupNameOrPlaceWork, GridPhone, GridGender, GridAdditional };

            int count = 0;
            Thickness thickness = new Thickness();

            for (int i = 0; i < Grids.Length; i++)
            {
                if (Grids[i].Visibility == Visibility.Visible)
                {
                    switch (count)
                    {
                        case 0:
                            thickness = new Thickness(10, 10, 0, 0);
                            break;
                        case 1:
                            thickness = new Thickness(10, 40, 0, 0);
                            break;
                        case 2:
                            thickness = new Thickness(10, 70, 0, 0);
                            break;
                        case 3:
                            thickness = new Thickness(10, 100, 0, 0);
                            break;
                        case 4:
                            thickness = new Thickness(10, 130, 0, 0);
                            break;
                        case 5:
                            thickness = new Thickness(10, 160, 0, 0);
                            break;
                        case 6:
                            thickness = new Thickness(10, 190, 0, 0);
                            break;
                        case 7:
                            thickness = new Thickness(10, 220, 0, 0);
                            break;
                        case 8:
                            thickness = new Thickness(10, 250, 0, 0);
                            break;
                    }

                    ThicknessAnimation animation = new ThicknessAnimation // Новая анимация
                    {
                        From = new Thickness(Grids[i].Margin.Left, Grids[i].Margin.Top, Grids[i].Margin.Right, Grids[i].Margin.Bottom), // Стартовая позиция
                        To = thickness, // Конечная позиция
                        Duration = TimeSpan.FromSeconds(1), // Время
                        SpeedRatio = 6, // Скорость
                    };

                    Grids[i].BeginAnimation(MarginProperty, animation); // Запуск
                    count++;
                }
            }
        }

        private void ChangeBoolMass(int index, bool boolean)
        {
            if ((bool)RadioButtonStudent.IsChecked)
            {
                IsStudent[index] = boolean;
            }
            else if ((bool)RadioButtonDad.IsChecked)
            {
                IsDad[index] = boolean;
            }
            else
            {
                IsMom[index] = boolean;
            }
        }

        private void ButtonGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Filter = "All files (*.*)|*.*|Microsoft Office Word *.docx|*.docx|Microsoft Office Word *.doc|*.doc",
                FilterIndex = 2,
                AddExtension = true
            };

            if ((bool)saveFileDialog.ShowDialog())
            {
                Word.Application app = new Word.Application(); // Инициализация приложения
                Word.Document doc = app.Documents.Add(Visible: true); // Добавить в приложение новый документ
                string str = null;
                Word.Range docRange = doc.Range();

                for (int i = 0; i < IsStudent.Length; i++)
                {
                    if (IsStudent[i])
                    {
                        switch (i)
                        {
                            case 0:
                                str += $"Имя: {TextBoxName.Text}\n\n";
                                break;
                            case 1:
                                str += $"Фамилия: {TextBoxSubname.Text}\n\n";
                                break;
                            case 2:
                                str += $"Отчество: {TextBoxPatronymic.Text}\n\n";
                                break;
                            case 3:
                                str += $"Дата рождения: {DatePickerDateBirth.Text}\n\n";
                                break;
                            case 4:
                                str += $"Город: {TextBoxLocation.Text}\n\n";
                                break;
                            case 5:
                                if ((bool)RadioButtonStudent.IsChecked)
                                {
                                    str += $"Группа: {TextBoxGroupNameOrPlaceWork.Text}\n\n";
                                }
                                else
                                {
                                    str += $"Работа: {TextBoxGroupNameOrPlaceWork.Text}\n\n";
                                }
                                break;
                            case 6:
                                str += $"Телефон: {TextBoxPhone.Text}\n\n";
                                break;
                            case 7:
                                if ((bool)RadioButtonGenderMan.IsChecked)
                                {
                                    str += "Пол: Мужской\n\n";
                                }
                                else
                                {
                                    str += "Пол: Женский";
                                }
                                break;
                            case 8:
                                str += $"Дополнительно: {TextBoxAdditional.Text}";
                                break;
                            case 9:
                                // Photo
                                break;
                        }
                    }
                }

                doc.Range().Text = str;
                doc.SaveAs2(saveFileDialog.FileName);
                doc.Close(); // Закрыть документ
                app.Quit(); // Закрыть приложение Word
            }
        }

        private void RadioButtonStudent_Checked(object sender, RoutedEventArgs e)
        {
            RadioButtonStudent_Event();
        }

        private void RadioButtonStudent_Event()
        {
            RadioButtonChecked(IsStudent);
            TextBoxName.Text = InfStudent.Name;
            TextBoxSubname.Text = InfStudent.Subname;
            TextBoxPatronymic.Text = InfStudent.Patronymic;
            DatePickerDateBirth.Text = InfStudent.DateBirth;
            TextBoxLocation.Text = InfStudent.Location;
            TextBoxGroupNameOrPlaceWork.Text = InfStudent.GroupNameOrPlaceWork;
            TextBoxPhone.Text = InfStudent.Phone;
            TextBoxAdditional.Text = InfStudent.Additional;

            if (InfStudent.Gender == Const.Man)
            {
                RadioButtonGenderMan.IsChecked = true;
            }
            else
            {
                RadioButtonGenderWoman.IsChecked = true;
            }

            CheckBoxGroupNameOrPlaceWork.Content = "Группа";
            LabelGroupNameOrPlaceWork.Content = "Группа:";
            ImageAvatar.ImageSource = InfStudent.Photos.Photo;
        }

        private void RadioButtonDad_Checked(object sender, RoutedEventArgs e)
        {
            RadioButtonChecked(IsDad);
            TextBoxName.Text = InfDad.Name;
            TextBoxSubname.Text = InfDad.Subname;
            TextBoxPatronymic.Text = InfDad.Patronymic;
            DatePickerDateBirth.Text = InfDad.DateBirth;
            TextBoxLocation.Text = InfDad.Location;
            TextBoxGroupNameOrPlaceWork.Text = InfDad.GroupNameOrPlaceWork;
            TextBoxPhone.Text = InfDad.Phone;
            TextBoxAdditional.Text = InfDad.Additional;

            if (InfDad.Gender == Const.Man)
            {
                RadioButtonGenderMan.IsChecked = true;
            }
            else
            {
                RadioButtonGenderWoman.IsChecked = true;
            }

            CheckBoxGroupNameOrPlaceWork.Content = "Работа";
            LabelGroupNameOrPlaceWork.Content = "Работа:";
            ImageAvatar.ImageSource = InfDad.Photos.Photo;
        }

        private void RadioButtonMom_Checked(object sender, RoutedEventArgs e)
        {
            RadioButtonChecked(IsMom);
            TextBoxName.Text = InfMom.Name;
            TextBoxSubname.Text = InfMom.Subname;
            TextBoxPatronymic.Text = InfMom.Patronymic;
            DatePickerDateBirth.Text = InfMom.DateBirth;
            TextBoxLocation.Text = InfMom.Location;
            TextBoxGroupNameOrPlaceWork.Text = InfMom.GroupNameOrPlaceWork;
            TextBoxPhone.Text = InfMom.Phone;
            TextBoxAdditional.Text = InfMom.Additional;

            if (InfMom.Gender == Const.Man)
            {
                RadioButtonGenderMan.IsChecked = true;
            }
            else
            {
                RadioButtonGenderWoman.IsChecked = true;
            }

            CheckBoxGroupNameOrPlaceWork.Content = "Работа";
            LabelGroupNameOrPlaceWork.Content = "Работа:";
            ImageAvatar.ImageSource = InfMom.Photos.Photo;
        }

        private void RadioButtonChecked(bool[] mass)
        {
            CheckBox[] CheckBoxes = { CheckBoxName, CheckBoxSubname, CheckBoxPatronymic, CheckBoxDateBirth, CheckBoxLocation, CheckBoxGroupNameOrPlaceWork, CheckBoxPhone, CheckBoxGender, CheckBoxAdditional, CheckBoxPhoto };

            GridName.Visibility = GridSubname.Visibility = GridPatronymic.Visibility = GridDateBirth.Visibility = GridLocation.Visibility = GridGroupNameOrPlaceWork.Visibility = GridPhone.Visibility = GridGender.Visibility = GridAdditional.Visibility = EllipseAvatar.Visibility = Visibility.Hidden;

            for (int i = 0; i < mass.Length; i++)
            {
                CheckBoxes[i].IsChecked = mass[i];

                switch (CheckBoxes[i].Name)
                {
                    case "CheckBoxName":
                        CheckBoxName_Event();
                        break;
                    case "CheckBoxSubname":
                        CheckBoxSubname_Event();
                        break;
                    case "CheckBoxPatronymic":
                        CheckBoxPatronymic_Event();
                        break;
                    case "CheckBoxDateBirth":
                        CheckBoxDateBirth_Event();
                        break;
                    case "CheckBoxLocation":
                        CheckBoxLocation_Event();
                        break;
                    case "CheckBoxGroupNameOrPlaceWork":
                        CheckBoxGroupNameOrPlaceWork_Event();
                        break;
                    case "CheckBoxPhone":
                        CheckBoxPhone_Event();
                        break;
                    case "CheckBoxGender":
                        CheckBoxGender_Event();
                        break;
                    case "CheckBoxAdditional":
                        CheckBoxAdditional_Event();
                        break;
                    case "CheckBoxPhoto":
                        CheckBoxPhoto_Event();
                        break;
                }
            }
        }

        private void CheckBoxName_Event()
        {
            if ((bool)CheckBoxName.IsChecked)
            {
                ChangeBoolMass(0, true);
                GridName.Visibility = Visibility.Visible;
            }
            else
            {
                ChangeBoolMass(0, false);
                GridName.Visibility = Visibility.Hidden;
            }
            AutoGenerateGridsPosition();
        }

        private void CheckBoxSubname_Event()
        {
            if ((bool)CheckBoxSubname.IsChecked)
            {
                ChangeBoolMass(1, true);
                GridSubname.Visibility = Visibility.Visible;
            }
            else
            {
                ChangeBoolMass(1, false);
                GridSubname.Visibility = Visibility.Hidden;
            }
            AutoGenerateGridsPosition();
        }

        private void CheckBoxPatronymic_Event()
        {
            if ((bool)CheckBoxPatronymic.IsChecked)
            {
                ChangeBoolMass(2, true);
                GridPatronymic.Visibility = Visibility.Visible;
            }
            else
            {
                ChangeBoolMass(2, false);
                GridPatronymic.Visibility = Visibility.Hidden;
            }
            AutoGenerateGridsPosition();
        }

        private void CheckBoxDateBirth_Event()
        {
            if ((bool)CheckBoxDateBirth.IsChecked)
            {
                ChangeBoolMass(3, true);
                GridDateBirth.Visibility = Visibility.Visible;
            }
            else
            {
                ChangeBoolMass(3, false);
                GridDateBirth.Visibility = Visibility.Hidden;
            }
            AutoGenerateGridsPosition();
        }

        private void CheckBoxLocation_Event()
        {
            if ((bool)CheckBoxLocation.IsChecked)
            {
                ChangeBoolMass(4, true);
                GridLocation.Visibility = Visibility.Visible;
            }
            else
            {
                ChangeBoolMass(4, false);
                GridLocation.Visibility = Visibility.Hidden;
            }
            AutoGenerateGridsPosition();
        }

        private void CheckBoxGroupNameOrPlaceWork_Event()
        {
            if ((bool)CheckBoxGroupNameOrPlaceWork.IsChecked)
            {
                ChangeBoolMass(5, true);
                GridGroupNameOrPlaceWork.Visibility = Visibility.Visible;
            }
            else
            {
                ChangeBoolMass(5, false);
                GridGroupNameOrPlaceWork.Visibility = Visibility.Hidden;
            }
            AutoGenerateGridsPosition();
        }

        private void CheckBoxPhone_Event()
        {
            if ((bool)CheckBoxPhone.IsChecked)
            {
                ChangeBoolMass(6, true);
                GridPhone.Visibility = Visibility.Visible;
            }
            else
            {
                ChangeBoolMass(6, false);
                GridPhone.Visibility = Visibility.Hidden;
            }
            AutoGenerateGridsPosition();
        }

        private void CheckBoxGender_Event()
        {
            if ((bool)CheckBoxGender.IsChecked)
            {
                ChangeBoolMass(7, true);
                GridGender.Visibility = Visibility.Visible;
            }
            else
            {
                ChangeBoolMass(7, false);
                GridGender.Visibility = Visibility.Hidden;
            }
            AutoGenerateGridsPosition();
        }

        private void CheckBoxAdditional_Event()
        {
            if ((bool)CheckBoxAdditional.IsChecked)
            {
                ChangeBoolMass(8, true);
                GridAdditional.Visibility = Visibility.Visible;
            }
            else
            {
                ChangeBoolMass(8, false);
                GridAdditional.Visibility = Visibility.Hidden;
            }
            AutoGenerateGridsPosition();
        }

        private void CheckBoxPhoto_Event()
        {
            if ((bool)CheckBoxPhoto.IsChecked)
            {
                ChangeBoolMass(9, true);
                EllipseAvatar.Visibility = Visibility.Visible;
            }
            else
            {
                ChangeBoolMass(9, false);
                EllipseAvatar.Visibility = Visibility.Hidden;
            }
        }

        private void CheckBoxName_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxName_Event();
        }

        private void CheckBoxSubname_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxSubname_Event();
        }

        private void CheckBoxPatronymic_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxPatronymic_Event();
        }

        private void CheckBoxDateBirth_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxDateBirth_Event();
        }

        private void CheckBoxLocation_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxLocation_Event();
        }

        private void CheckBoxGroupNameOrPlaceWork_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxGroupNameOrPlaceWork_Event();
        }

        private void CheckBoxPhone_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxPhone_Event();
        }

        private void CheckBoxGender_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxGender_Event();
        }

        private void CheckBoxAdditional_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxAdditional_Event();
        }

        private void CheckBoxPhoto_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxPhoto_Event();
        }
    }
}
