using System.Collections;
using System.Windows.Media.Imaging;

namespace SqlDataBase
{
    internal class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Subname { get; set; }
        public string Type { get; set; }
    }

    internal static class SessionUser
    {
        public static int Id;
        public static bool Logout = false;
    }

    internal static class Session
    {
        public static int Id;
        public static ArrayList StudentsId = new ArrayList();
        public static ArrayList ParentsId = new ArrayList();
        public static Info CurrentInfo;
        public static Info CreateStudentInfo;
        public static Info CreateDadInfo;
        public static Info CreateMomInfo;
    }

    internal class Info
    {
        public int Id { get; set; } // Parents
        public string Name { get; set; }
        public string Subname { get; set; }
        public string Patronymic { get; set; }
        public string DateBirth { get; set; }
        public string Location { get; set; }
        public string GroupNameOrPlaceWork { get; set; } // Student/Parents
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Additional { get; set; }
        public InfoPhotos Photos = new InfoPhotos();
    }

    public class InfoPhotos
    {
        public BitmapImage Photo = null; // Student/Parents
        public BitmapImage PhotoPassport = null; // Student/Parents
        public BitmapImage PhotoSchoolCertificate = null; // Student
        public BitmapImage PhotoBirthCertificate = null; // Student
        public int NumberSelectedPhoto = 1;
    }

    internal static class InfoPhotosEditing
    {
        public static void Clear(InfoPhotos inf)
        {
            inf.Photo = null;
            inf.PhotoPassport = null;
            inf.PhotoSchoolCertificate = null;
            inf.PhotoBirthCertificate = null;
            inf.NumberSelectedPhoto = 1;
        }

        public static BitmapImage GetBitmapNumber(InfoPhotos inf)
        {
            switch (inf.NumberSelectedPhoto)
            {
                case 1:
                    return inf.Photo;
                case 2:
                    return inf.PhotoPassport;
                case 3:
                    return inf.PhotoSchoolCertificate;
                case 4:
                    return inf.PhotoBirthCertificate;
            }
            return null;
        }

        public static string GetNameSelectedPhoto(InfoPhotos inf)
        {
            switch (inf.NumberSelectedPhoto)
            {
                case 1:
                    return "Фотография";
                case 2:
                    return "Паспорт";
                case 3:
                    return "Аттестат";
                case 4:
                    return "Свидетельство о рождении";
            }
            return null;
        }

        public static BitmapImage Default(InfoPhotos inf)
        {
            inf.NumberSelectedPhoto = 1;
            return inf.Photo;
        }

        public static BitmapImage SelectPhoto(bool next, bool student, InfoPhotos inf)
        {
            if (next)
            {
                if (student)
                {
                    if (inf.NumberSelectedPhoto == 4)
                    {
                        inf.NumberSelectedPhoto = 1;
                    }
                    else
                    {
                        inf.NumberSelectedPhoto += 1;
                    }
                }
                else
                {
                    if (inf.NumberSelectedPhoto == 2)
                    {
                        inf.NumberSelectedPhoto = 1;
                    }
                    else
                    {
                        inf.NumberSelectedPhoto += 1;
                    }
                }
            }
            else
            {
                if (student)
                {
                    if (inf.NumberSelectedPhoto == 1)
                    {
                        inf.NumberSelectedPhoto = 4;
                    }
                    else
                    {
                        inf.NumberSelectedPhoto -= 1;
                    }
                }
                else
                {
                    if (inf.NumberSelectedPhoto == 1)
                    {
                        inf.NumberSelectedPhoto = 2;
                    }
                    else
                    {
                        inf.NumberSelectedPhoto -= 1;
                    }
                }
            }

            switch (inf.NumberSelectedPhoto)
            {
                case 1:
                    return inf.Photo;
                case 2:
                    return inf.PhotoPassport;
                case 3:
                    return inf.PhotoSchoolCertificate;
                case 4:
                    return inf.PhotoBirthCertificate;
            }
            return null;
        }
    }
}
