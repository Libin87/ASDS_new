using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.Data.Sqlite;
using Microsoft.Windows.Storage;

namespace ASDS_dev.Pages.UserManagement
{
    public class User
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserRole { get; set; }
        public string Status { get; set; }
        public string CreatedAt { get; set; }
        public string Pass { get; set; }
    }

    internal class UserDatabase
    {
        private static readonly string dbPath = @"D:\Harshitha\ASDS\ASDS.db";
        private static readonly object dbLock = new object();

        public static void InitializeDatabase()
        {
           
            lock (dbLock)
            {
                // Ensure the file exists or is created
                if (!File.Exists(dbPath))
                {
                    //SqliteConnection.CreateFile(dbPath);
                    Debug.WriteLine("Database created at: " + dbPath);
                }
                else
                {
                    Debug.WriteLine("Using existing DB at: " + dbPath);
                }

                using (var connection = new SqliteConnection($"Data Source={dbPath}"))
                {
                    try
                    {
                        connection.Open();
                        var tableCommand = connection.CreateCommand();
                        tableCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS TBL_Users (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserID TEXT NOT NULL,
                        FirstName TEXT NOT NULL,
                        LastName TEXT NOT NULL,
                        UserRole TEXT NOT NULL,
                        Status TEXT NOT NULL,
                        Password TEXT NOT NULL,
                        CreatedAt TEXT NOT NULL,
                        FailedAttempts INTEGER DEFAULT 0
                    );";
                        tableCommand.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        Debug.WriteLine("SQLite Init Error: " + ex.Message);
                        throw; // rethrow so you can catch it at the top level
                    }
                }
            }
        }

        private static void ExecuteWithRetry(Action action)
        {
            int retries = 3;
            while (retries-- > 0)
            {
                try
                {
                    lock (dbLock) { action(); }
                    break;
                }
                catch (SqliteException ex) when (ex.SqliteErrorCode == 5)
                {
                    Thread.Sleep(100);
                    if (retries == 0) throw;
                }
            }
        }

        public static void AddUser(string userId, string firstName, string lastName, string role, string status, string pass)
        {
            ExecuteWithRetry(() =>
            {
                using (var connection = new SqliteConnection($"Data Source={dbPath}"))
                {
                    connection.Open();
                    var insertCommand = connection.CreateCommand();
                    insertCommand.CommandText = @"
                        INSERT INTO TBL_Users (UserID, FirstName, LastName, UserRole, Status, Password, CreatedAt)
                        VALUES ($id, $first, $last, $role, $status, $pass, $created);";
                    insertCommand.Parameters.AddWithValue("$id", userId);
                    insertCommand.Parameters.AddWithValue("$first", firstName);
                    insertCommand.Parameters.AddWithValue("$last", lastName);
                    insertCommand.Parameters.AddWithValue("$role", role);
                    insertCommand.Parameters.AddWithValue("$status", status);
                    insertCommand.Parameters.AddWithValue("$pass", pass);
                    insertCommand.Parameters.AddWithValue("$created", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    insertCommand.ExecuteNonQuery();
                }
            });
        }

        public static ObservableCollection<User> GetAllUsers()
        {
            var users = new ObservableCollection<User>();

            lock (dbLock)
            {
                
                using (var connection = new SqliteConnection($"Data Source={dbPath}"))
                {
                    Debug.WriteLine("Reading DB from: " + dbPath);
                    connection.Open();
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM TBL_Users";
                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    Debug.WriteLine("User count: " + count);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Debug.WriteLine("No users found in TBL_Users.");
                        }

                        while (reader.Read())
                        {
                            try
                            {
                                users.Add(new User
                                {
                                    UserId = reader["UserID"].ToString(),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    UserRole = reader["UserRole"].ToString(),
                                    Status = reader["Status"].ToString(),
                                    CreatedAt = reader["CreatedAt"]?.ToString() ?? ""
                                });
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Error reading user: " + ex.Message);
                            }
                        }
                    }
                }
            }

            return users;
        }

        public static void DeleteUser(string userId)
        {
            ExecuteWithRetry(() =>
            {
                using (var connection = new SqliteConnection($"Data Source={dbPath}"))
                {
                    connection.Open();
                    var command = new SqliteCommand("DELETE FROM TBL_Users WHERE UserID = @UserID", connection);
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.ExecuteNonQuery();
                }
            });
        }

        public static void UpdateUser(string userId, string firstName, string lastName, string role, string status)
        {
            ExecuteWithRetry(() =>
            {
                using (var connection = new SqliteConnection($"Data Source={dbPath}"))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                        UPDATE TBL_Users
                        SET FirstName = @FirstName,
                            LastName = @LastName,
                            UserRole = @UserRole,
                            Status = @Status
                        WHERE UserID = @UserID";
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@UserRole", role);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.ExecuteNonQuery();
                }
            });
        }

        public static bool RegisterUser(string userId, string firstName, string lastName, string password, string role = "User", string status = "Active")
        {
            bool isRegistered = false;
            ExecuteWithRetry(() =>
            {
                using (var connection = new SqliteConnection($"Data Source={dbPath}"))
                {
                    connection.Open();
                    var checkCommand = connection.CreateCommand();
                    checkCommand.CommandText = "SELECT COUNT(*) FROM TBL_Users WHERE UserID = @UserID";
                    checkCommand.Parameters.AddWithValue("@UserID", userId);
                    var count = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (count == 0)
                    {
                        var insertCommand = connection.CreateCommand();
                        insertCommand.CommandText = @"
                            INSERT INTO TBL_Users (UserID, FirstName, LastName, UserRole, Status, Password, CreatedAt)
                            VALUES (@UserID, @FirstName, @LastName, @UserRole, @Status, @Password, @CreatedAt);";
                        insertCommand.Parameters.AddWithValue("@UserID", userId);
                        insertCommand.Parameters.AddWithValue("@FirstName", firstName);
                        insertCommand.Parameters.AddWithValue("@LastName", lastName);
                        insertCommand.Parameters.AddWithValue("@UserRole", role);
                        insertCommand.Parameters.AddWithValue("@Status", status);
                        insertCommand.Parameters.AddWithValue("@Password", password);
                        insertCommand.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        insertCommand.ExecuteNonQuery();
                        isRegistered = true;
                    }
                }
            });
            return isRegistered;
        }

        public class LoginResult
        {
            public bool IsSuccess { get; set; }
            public bool IsUserNotFound { get; set; }
            public bool IsPasswordIncorrect { get; set; }
            public bool IsSuspended { get; set; }
            public string UserRole { get; set; }
            public string Status { get; set; }
        }

        public static LoginResult ValidateLogin(string userId, string password)
        {
            var result = new LoginResult();

            ExecuteWithRetry(() =>
            {
                using (var connection = new SqliteConnection($"Data Source={dbPath}"))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT Password, UserRole, Status, FailedAttempts FROM TBL_Users WHERE UserID = @UserID";
                    command.Parameters.AddWithValue("@UserID", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            result.IsUserNotFound = true;
                            return;
                        }

                        string storedPassword = reader["Password"].ToString();
                        result.UserRole = reader["UserRole"].ToString();
                        result.Status = reader["Status"].ToString();
                        int failedAttempts = Convert.ToInt32(reader["FailedAttempts"]);

                        if (result.Status.Equals("Suspended", StringComparison.OrdinalIgnoreCase))
                        {
                            result.IsSuspended = true;
                            return;
                        }

                        bool isValid = PasswordHelper.VerifyPassword(password, storedPassword);

                        reader.Close();

                        var updateCmd = connection.CreateCommand();

                        if (!isValid)
                        {
                            failedAttempts++;
                            updateCmd.CommandText = @"
                                UPDATE TBL_Users 
                                SET FailedAttempts = @FailedAttempts, 
                                    Status = CASE WHEN @FailedAttempts >= 3 THEN 'Suspended' ELSE Status END 
                                WHERE UserID = @UserID";
                            updateCmd.Parameters.AddWithValue("@FailedAttempts", failedAttempts);
                            updateCmd.Parameters.AddWithValue("@UserID", userId);
                            updateCmd.ExecuteNonQuery();

                            result.IsPasswordIncorrect = failedAttempts < 3;
                            result.IsSuspended = failedAttempts >= 3;
                        }
                        else
                        {
                            updateCmd.CommandText = "UPDATE TBL_Users SET FailedAttempts = 0 WHERE UserID = @UserID";
                            updateCmd.Parameters.AddWithValue("@UserID", userId);
                            updateCmd.ExecuteNonQuery();
                            result.IsSuccess = true;
                        }
                    }
                }
            });

            return result;
        }

        public static bool UpdateUserPassword(string userId, string newPassword)
        {
            bool updated = false;
            ExecuteWithRetry(() =>
            {
                using (var connection = new SqliteConnection($"Data Source={dbPath}"))
                {
                    connection.Open();
                    var updateCommand = connection.CreateCommand();
                    updateCommand.CommandText = @"UPDATE TBL_Users SET Password = $hashed WHERE UserID = $id;";
                    updateCommand.Parameters.AddWithValue("$hashed", PasswordHelper.HashPassword(newPassword));
                    updateCommand.Parameters.AddWithValue("$id", userId);
                    updated = updateCommand.ExecuteNonQuery() > 0;
                }
            });
            return updated;
        }

        public static bool ValidateUser(string userId, string inputPassword)
        {
            bool isValid = false;
            lock (dbLock)
            {
                using (var connection = new SqliteConnection($"Data Source={dbPath}"))
                {
                    connection.Open();
                    var selectCommand = connection.CreateCommand();
                    selectCommand.CommandText = @"SELECT Password FROM TBL_Users WHERE UserID = $id;";
                    selectCommand.Parameters.AddWithValue("$id", userId);
                    var result = selectCommand.ExecuteScalar();
                    if (result != null)
                    {
                        string storedHashedPassword = result.ToString();
                        isValid = PasswordHelper.VerifyPassword(inputPassword, storedHashedPassword);
                    }
                }
            }
            return isValid;
        }
    }
}
