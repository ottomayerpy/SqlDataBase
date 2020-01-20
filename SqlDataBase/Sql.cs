using System;
using System.Collections;
using System.Data.Common;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows;

namespace SqlDataBase
{
    // Класс представляющий константы для обращения к базе данных
    internal static class Const
    {
        // Column Names Tables Students And Parents
        public const string Name = "Name";
        public const string Subname = "Subname";
        public const string Patronymic = "Patronymic";
        public const string DateBirth = "DateBirth";
        public const string Location = "Location";
        public const string GroupName = "GroupName";
        public const string Gender = "Gender";
        public const string Additional = "Additional";
        public const string Photo = "Photo";
        public const string PhotoPassport = "PhotoPassport";
        public const string PhotoSchoolCertificate = "PhotoSchoolCertificate";
        public const string PhotoBirthCertificate = "PhotoBirthCertificate";
        // User Types
        public const string Admin = "Admin";
        public const string User = "User";
        // Genders
        public const string Man = "Man";
        public const string Woman = "Woman";
        // Tables
        public const string Groups = "Groups";
        public const string Parents = "Parents";
        public const string Students = "Students";
        public const string Users = "Users";
        // Other
        public const string All = "All";
        public const string DataBase = "DbUsers";
    }

    internal static class Sql
    {
        private static SqlConnection con;
        private static DbDataReader reader;

        private static object GetValue(string value)
        {
            try
            {
                return reader.GetValue(reader.GetOrdinal(value));
            }
            catch
            {
                return null;
            }
        }

        public static void Open()
        {
            try
            {
                con = new SqlConnection($"Data Source={Properties.Settings.Default.Server}; Initial Catalog={Const.DataBase}; Integrated Security = true;");
                con.Open();
            }
            catch (SqlException SqlEx)
            {
                MessageBox.Show(SqlEx.Message, "Critical Error - SqlExeption log", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Critical Error - Exeption log", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void Close()
        {
            try
            {
                con.Close();
                con.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Critical Error - Exeption log", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static bool Login(string login, string password)
        {
            try
            {
                Open();

                using (reader = new SqlCommand("SELECT Login, Password, Id FROM Users", con).ExecuteReader()) // Инициализируем чтение данных из сервера
                {
                    while (reader.Read())
                    {
                        if ((string)GetValue("Login") == login && (string)GetValue("Password") == password) // Ищем юзера по логину и паролю
                        {
                            // Записываем его данные
                            SessionUser.Id = (int)GetValue("Id");
                            return true; // Возвращаем ответ, сигнализирующий об успешной авторизации
                        }
                    }

                    MessageBox.Show("Неверный логин или пароль.", "Ошибка авторизации!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Critical Error - Exeption log", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Close();
            }
            return false;
        }

        public static bool Registration(string login, string password, string name, string subname, string type)
        {
            try
            {
                Open();

                if ((string)new SqlCommand($"SELECT Login FROM Users WHERE Login = '{login}'", con).ExecuteScalar() == login)
                {
                    MessageBox.Show("Данный логин уже занят.", "Ошибка регистрации!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    new SqlCommand($"INSERT INTO Users (Login, Password, Name, Subname, Type) VALUES ('{login}', '{password}', '{name}', '{subname}', '{type}')", con).ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Critical Error - Exeption log", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Close();
            }
            return false;
        }

        // Возвращает список групп
        public static ArrayList GetGroupList()
        {
            try
            {
                ArrayList data = new ArrayList();

                Open();

                using (reader = new SqlCommand("SELECT GroupName FROM Groups", con).ExecuteReader()) // Инициализируем чтение данных из сервера
                {
                    while (reader.Read())
                    {
                        data.Add((string)GetValue("GroupName"));
                    }
                }

                return data;
            }
            catch
            {
                return null;
            }
            finally
            {
                Close();
            }
        }

        public static byte[] GetAvatar(int id, string photo, string table)
        {
            try
            {
                Open();
                return Convert.FromBase64String(new SqlCommand($"SELECT {photo} FROM {table} WHERE Id='{id}'", con).ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Critical Error - Exeption log", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Close();
            }
            return null;
        }

        // Возращает список студентов
        public static ArrayList GetTable(string filter = null)
        {
            ArrayList list = new ArrayList();
            ArrayList listid = new ArrayList();

            try
            {
                string strcon = null;

                if (filter == null || filter == "GroupName = 'Все'")
                {
                    strcon = "SELECT Name, Subname, Patronymic, Id FROM Students";
                }
                else
                {
                    strcon = "SELECT Name, Subname, Patronymic, Id FROM Students WHERE " + filter;
                }

                Open();

                using (reader = new SqlCommand(strcon, con).ExecuteReader()) // Инициализируем чтение данных из сервера
                {
                    while (reader.Read())
                    {
                        string name = (string)GetValue("Name");
                        string subname = (string)GetValue("Subname");
                        string patronymic = (string)GetValue("Patronymic");
                        string result = $"{subname} {name} {patronymic}";
                        int id = (int)GetValue("Id");

                        list.Add(result);
                        listid.Add(id);
                    }

                    Session.StudentsId.Clear(); // Чистим список айдишников студентов

                    foreach (int item in listid)
                    {
                        Session.StudentsId.Add(item); // Добавляем список айдишников студентов
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Critical Error - Exeption log", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Close();
            }
            return null;
        }

        public static User GetUser()
        {
            try
            {
                Open();

                using (reader = new SqlCommand($"SELECT Login, Password, Name, Subname, Type FROM Users WHERE Id = '{SessionUser.Id}'", con).ExecuteReader()) // Инициализируем чтение данных из сервера
                {
                    User user = new User();

                    while (reader.Read())
                    {
                        user.Login = (string)GetValue("Login");
                        user.Password = (string)GetValue("Password");
                        user.Name = (string)GetValue("Name");
                        user.Subname = (string)GetValue("Subname");
                        user.Type = (string)GetValue("Type");
                    }

                    return user;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Critical Error - Exeption log", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Close();
            }
            return null;
        }

        public static bool UpdateUser(User user)
        {
            try
            {
                Open();

                if (user.Password != string.Empty)
                {
                    new SqlCommand($"UPDATE Users SET Password='{user.Password}' where Id = {SessionUser.Id}", con).ExecuteNonQuery();
                }

                if (user.Type != string.Empty)
                {
                    new SqlCommand($"UPDATE Users SET Type='{user.Type}' where Id = {SessionUser.Id}", con).ExecuteNonQuery();
                }

                new SqlCommand($"UPDATE Users SET Login='{user.Login}' where Id = {SessionUser.Id}", con).ExecuteNonQuery();
                new SqlCommand($"UPDATE Users SET Name='{user.Name}' where Id = {SessionUser.Id}", con).ExecuteNonQuery();
                new SqlCommand($"UPDATE Users SET Subname='{user.Subname}' where Id = {SessionUser.Id}", con).ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Critical Error - Exeption log", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Close();
            }
            return false;
        }

        public static bool CheckPasswordUser(string pass)
        {
            try
            {
                Open();

                if (pass == (string)new SqlCommand($"SELECT Password FROM Users WHERE Id = '{SessionUser.Id}'", con).ExecuteScalar())
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Critical Error - Exeption log", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Close();
            }
            return false;
        }

        public static Info GetInfo(int id, string gender = null)
        {
            try
            {
                string strcon = null;

                if (gender == null)
                {
                    strcon = $"SELECT Id, Name, Subname, Patronymic, DateBirth, Location, GroupName, Gender, Phone, Additional, Photo, PhotoSchoolCertificate, PhotoPassport, PhotoBirthCertificate FROM Students WHERE Id = '{id}'";
                }
                else
                {
                    strcon = $"SELECT Id, Name, Subname, Patronymic, DateBirth, Location, Gender, PlaceWork, Phone, Additional, Photo, PhotoPassport FROM Parents WHERE StudentId = '{id}' AND Gender = '{gender}'";
                }

                Open();

                using (reader = new SqlCommand(strcon, con).ExecuteReader()) // Инициализируем чтение данных из сервера
                {
                    Info info = new Info();

                    while (reader.Read())
                    {
                        info.Name = (string)GetValue("Name");
                        info.Subname = (string)GetValue("Subname");
                        info.Patronymic = (string)GetValue("Patronymic");
                        info.DateBirth = DateTime.ParseExact(GetValue("DateBirth").ToString().Substring(0, 10), "dd.MM.yyyy", null).ToString();
                        info.Location = (string)GetValue("Location");
                        info.Gender = (string)GetValue("Gender");
                        info.Additional = (string)GetValue("Additional");
                        info.Phone = (string)GetValue("Phone");
                        info.Photos.Photo = Code.LoadImage(Convert.FromBase64String(GetValue("Photo").ToString()));
                        info.Photos.PhotoPassport = Code.LoadImage(Convert.FromBase64String(GetValue("PhotoPassport").ToString()));

                        if (gender == null)
                        {
                            info.GroupNameOrPlaceWork = (string)GetValue("GroupName");
                            info.Photos.PhotoSchoolCertificate = Code.LoadImage(Convert.FromBase64String(GetValue("PhotoSchoolCertificate").ToString()));
                            info.Photos.PhotoBirthCertificate = Code.LoadImage(Convert.FromBase64String(GetValue("PhotoBirthCertificate").ToString()));
                        }
                        else
                        {
                            info.Id = (int)GetValue("Id");
                            info.GroupNameOrPlaceWork = (string)GetValue("PlaceWork");
                        }
                    }

                    if (info.Name != null)
                    {
                        return info;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Critical Error - Exeption log", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Close();
            }

            return null;
        }

        // Изменить параметр пользователя
        public static void UpdateStudentInfo(int id, string sqltype, object info)
        {
            try
            {
                Open();

                switch (sqltype)
                {
                    case Const.DateBirth:
                        new SqlCommand($"UPDATE Students SET DateBirth='{Code.DateFormatToSqlServer((string)info)}' WHERE Id = {id}", con).ExecuteNonQuery();
                        break;
                    case Const.Photo:
                        if (info.ToString() == string.Empty)
                        {
                            new SqlCommand($"UPDATE Students SET Photo='' where Id = {id}", con).ExecuteNonQuery();
                        }
                        else
                        {
                            new SqlCommand($"UPDATE Students SET Photo='{Convert.ToBase64String((byte[])info)}' WHERE Id = {id}", con).ExecuteNonQuery();
                        }
                        break;
                    case Const.PhotoPassport:
                    case Const.PhotoSchoolCertificate:
                    case Const.PhotoBirthCertificate:
                        new SqlCommand($"UPDATE Students SET {sqltype}='{Convert.ToBase64String((byte[])info)}' WHERE Id = {id}", con).ExecuteNonQuery();
                        break;
                    case Const.All:
                        new SqlCommand((string)info, con).ExecuteNonQuery();
                        break;
                    default:
                        new SqlCommand($"UPDATE Students SET {sqltype}='{(string)info}' WHERE Id = {id}", con).ExecuteNonQuery();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Critical Error - Exeption log", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Close();
            }
        }

        public static void DeleteStudent()
        {
            try
            {
                Open();
                new SqlCommand($"DELETE FROM Students WHERE Id = '{Session.Id}'", con).ExecuteNonQuery();
                new SqlCommand($"DELETE FROM Parents WHERE StudentId = '{Session.Id}' AND Gender = '{Const.Man}'", con).ExecuteNonQuery();
                new SqlCommand($"DELETE FROM Parents WHERE StudentId = '{Session.Id}' AND Gender = '{Const.Woman}'", con).ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Critical Error - Exeption log", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Close();
            }
        }

        public static bool NewStudent()
        {
            try
            {
                Open();
                new SqlCommand($"INSERT INTO Students (Name, Subname, Patronymic, DateBirth, Location, GroupName, Gender, Phone, Additional) VALUES ('{Session.CreateStudentInfo.Name}', '{Session.CreateStudentInfo.Subname}', '{Session.CreateStudentInfo.Patronymic}', '{Session.CreateStudentInfo.DateBirth}', '{Session.CreateStudentInfo.Location}', '{Session.CreateStudentInfo.GroupNameOrPlaceWork}', '{Session.CreateStudentInfo.Gender}', '{Session.CreateStudentInfo.Phone}', '{Session.CreateStudentInfo.Additional}')", con).ExecuteNonQuery();
                object id = new SqlCommand($"SELECT @@IDENTITY", con).ExecuteScalar();
                new SqlCommand($"INSERT INTO Parents (Name, Subname, Patronymic, DateBirth, Location, Gender, PlaceWork, Phone, Additional, StudentId) VALUES ('{Session.CreateDadInfo.Name}', '{Session.CreateDadInfo.Subname}', '{Session.CreateDadInfo.Patronymic}', '{Session.CreateDadInfo.DateBirth}', '{Session.CreateDadInfo.Location}', '{Const.Man}', '{Session.CreateDadInfo.GroupNameOrPlaceWork}', '{Session.CreateDadInfo.Phone}', '{Session.CreateDadInfo.Additional}', '{id}')", con).ExecuteNonQuery();
                new SqlCommand($"INSERT INTO Parents (Name, Subname, Patronymic, DateBirth, Location, Gender, PlaceWork, Phone, Additional, StudentId) VALUES ('{Session.CreateMomInfo.Name}', '{Session.CreateMomInfo.Subname}', '{Session.CreateMomInfo.Patronymic}', '{Session.CreateMomInfo.DateBirth}', '{Session.CreateMomInfo.Location}', '{Const.Woman}', '{Session.CreateMomInfo.GroupNameOrPlaceWork}', '{Session.CreateMomInfo.Phone}', '{Session.CreateMomInfo.Additional}', '{id}')", con).ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Critical Error - Exeption log", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Close();
            }
            return false;
        }

        public static ArrayList GetServers()
        {
            return GetServerList.GetSQLServerList();
        }
    }

    internal class GetServerList
    {
        [DllImport("netapi32.dll", EntryPoint = "NetServerEnum")]
        public static extern NERR NetServerEnum([MarshalAs(UnmanagedType.LPWStr)] string ServerName, int Level, out IntPtr BufPtr, int PrefMaxLen, ref int EntriesRead, ref int TotalEntries, uint ServerType, [MarshalAs(UnmanagedType.LPWStr)] string Domain, int ResumeHandle);

        [DllImport("netapi32.dll", EntryPoint = "NetApiBufferFree")]
        public static extern NERR NetApiBufferFree(IntPtr Buffer);

        // Тип сервера
        private const uint SV_TYPE_SQLSERVER = 0x00000004;

        [StructLayout(LayoutKind.Sequential)]
        public struct SERVER_INFO_101
        {
            [MarshalAs(UnmanagedType.U4)] public uint sv101_platform_id;
            [MarshalAs(UnmanagedType.LPWStr)] public string sv101_name;
            [MarshalAs(UnmanagedType.U4)] public uint sv101_version_major;
            [MarshalAs(UnmanagedType.U4)] public uint sv101_version_minor;
            [MarshalAs(UnmanagedType.U4)] public uint sv101_type;
            [MarshalAs(UnmanagedType.LPWStr)] public string sv101_comment;
        }

        // Оперционная система
        public enum PLATFORM_ID : uint
        {
            PLATFORM_ID_DOS = 300,
            PLATFORM_ID_OS2 = 400,
            PLATFORM_ID_NT = 500,
            PLATFORM_ID_OSF = 600,
            PLATFORM_ID_VMS = 700,
        }

        // Список ошибок, возвращаемых NetServerEnum
        public enum NERR
        {
            NERR_Success = 0, // Success
            ERROR_ACCESS_DENIED = 5,
            ERROR_NOT_ENOUGH_MEMORY = 8,
            ERROR_BAD_NETPATH = 53,
            ERROR_NETWORK_BUSY = 54,
            ERROR_INVALID_PARAMETER = 87,
            ERROR_INVALID_LEVEL = 124,
            ERROR_MORE_DATA = 234,
            ERROR_EXTENDED_ERROR = 1208,
            ERROR_NO_NETWORK = 1222,
            ERROR_INVALID_HANDLE_STATE = 1609,
            ERROR_NO_BROWSER_SERVERS_FOUND = 6118,
        }

        // Получить список SQL серверов
        public static ArrayList GetSQLServerList()
        {
            SERVER_INFO_101 si;
            IntPtr pInfo = IntPtr.Zero;
            int etriesread = 0;
            int totalentries = 0;
            ArrayList srvs = new ArrayList();

            try
            {
                NERR err = NetServerEnum(null, 101, out pInfo, -1, ref etriesread, ref totalentries, SV_TYPE_SQLSERVER, null, 0);
                if ((err == NERR.NERR_Success || err == NERR.ERROR_MORE_DATA) && pInfo != IntPtr.Zero)
                {
                    int ptr = pInfo.ToInt32();
                    for (int i = 0; i < etriesread; i++)
                    {
                        si = (SERVER_INFO_101)Marshal.PtrToStructure(new IntPtr(ptr), typeof(SERVER_INFO_101));
                        // Castom
                        if (si.sv101_name == "DESKTOP-KK682Q8")
                        {
                            srvs.Add(si.sv101_name + "\\MSSQLSERVKAB106");
                        }
                        else
                        {
                            srvs.Add(si.sv101_name + "\\SQLEXPRESS"); // Добавляем имя сервера в список
                        }
                        // EndCastom
                        ptr += Marshal.SizeOf(si);
                    }
                }
            }
            catch (Exception) { }
            finally // Освобождаем выделенную память
            {
                if (pInfo != IntPtr.Zero)
                {
                    NetApiBufferFree(pInfo);
                }
            }
            return srvs;
        }
    }
}
