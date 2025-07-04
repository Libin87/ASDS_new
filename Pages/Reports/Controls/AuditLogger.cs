using System;
using System.Diagnostics;
using Microsoft.Data.Sqlite;
using System.IO;

namespace ASDS_dev.Pages.Reports.Controls
{
    public class AuditEvent
    {
        public DateTime EventTime { get; set; }
       
        public int EventType { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string EventMessage { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Remarks { get; set; }
        public int RemarksAdded { get; set; }
    }

    public static class AuditLogger
    {
        private static readonly string dbPath = @"D:\ASDS_DB\ASDS.db";
        private static readonly object dbLock = new();

        public static void InitializeDatabase()
        {
            lock (dbLock)
            {
                if (!File.Exists(dbPath))
                {
                    Debug.WriteLine("Database created at: " + dbPath);
                }
                else
                {
                    Debug.WriteLine("Using existing DB at: " + dbPath);
                }

                using var connection = new SqliteConnection($"Data Source={dbPath}");
                try
                {
                    connection.Open();
                    var tableCommand = connection.CreateCommand();
                    tableCommand.CommandText = @"
                        CREATE TABLE IF NOT EXISTS AuditEvents (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            EventTime TEXT,
                           
                            EventType INTEGER,
                            UserId TEXT,
                            UserName TEXT,
                            EventMessage TEXT,
                            OldValue TEXT,
                            NewValue TEXT,
                           
                            Remarks TEXT,
                            RemarksAdded INTEGER
                        );";
                    tableCommand.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    Debug.WriteLine("SQLite Init Error: " + ex.Message);
                    throw;
                }
            }
        }

        public static void LogEvent(AuditEvent audit)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO AuditEvents 
                (EventTime, EventType, UserId, UserName, EventMessage, OldValue, NewValue, Remarks, RemarksAdded)
                VALUES 
                ($eventTime, $eventType, $userId, $userName, $eventMessage, $oldValue, $newValue, $remarks, $remarksAdded);";

            command.Parameters.AddWithValue("$eventTime", audit.EventTime.ToString("yyyy-MM-dd HH:mm:ss"));
           
            command.Parameters.AddWithValue("$eventType", audit.EventType);
            command.Parameters.AddWithValue("$userId", audit.UserId);
            command.Parameters.AddWithValue("$userName", audit.UserName);
            command.Parameters.AddWithValue("$eventMessage", audit.EventMessage);
            command.Parameters.AddWithValue("$oldValue", audit.OldValue ?? "");
            command.Parameters.AddWithValue("$newValue", audit.NewValue ?? "");
            command.Parameters.AddWithValue("$remarks", audit.Remarks ?? "");
            command.Parameters.AddWithValue("$remarksAdded", audit.RemarksAdded);

            command.ExecuteNonQuery();
        }
    }
}
