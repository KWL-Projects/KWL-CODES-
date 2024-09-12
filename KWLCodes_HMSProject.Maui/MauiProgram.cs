using KWLCodes_HMSProject.Maui;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KWLCodes_HMSProject.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
    //need to take this code out or edit, we do not need the initial hello world

    class Programs
    {
        // Simulated user database
        static Dictionary<string, string> userDatabase = new Dictionary<string, string>
        {
            { "user1", HashPassword("password123") },  // username: user1, password: password123
            { "user2", HashPassword("mypassword") }    // username: user2, password: mypassword
        };

        // Method for secure login
        public static bool Login(string username, string password)
        {
            if (userDatabase.TryGetValue(username, out string storedHash))
            {
                string providedHash = HashPassword(password);
                return storedHash == providedHash;
            }
            return false;
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string GenerateToken(string username)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{DateTime.Now}"));
        }

        // Methods for managing assignments
        public static void CreateAssignment()
        {
            // Implement assignment creation logic here
        }

        public static void ViewAssignment()
        {
            // Implement assignment viewing logic here
        }

        public static void ViewSubmissions()
        {
            // Implement submission viewing logic here
        }

        public static void BrowseOwnSubmissions()
        {
            // Implement own submission browsing logic here
        }

        public static void ViewFeedback()
        {
            // Implement feedback viewing logic here
        }

        // Supporting methods for simulated database interactions
        public static bool CreateDatabaseEntry(string subjectInfo, string assignmentName, DateTime dueDate, string assignmentInfo)
        {
            return true; // Simulating success
        }

        public static Assignment RetrieveAssignmentFromDatabase(string assignmentName)
        {
            return null; // Simulate assignment retrieval
        }

        public static List<Submission> RetrieveSubmissionsFromDatabase(string assignmentName)
        {
            return new List<Submission>(); // Simulate submission retrieval
        }

        public static List<Submission> RetrieveSubmissionsForStudentFromDatabase(string studentName)
        {
            return new List<Submission>(); // Simulate student submissions retrieval
        }

        public static List<Feedback> RetrieveFeedbackForStudentFromDatabase(string studentName)
        {
            return new List<Feedback>(); // Simulate feedback retrieval
        }

        public static void LogToFile(string message)
        {
            try
            {
                string logFilePath = "log.txt";
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging message: {ex.Message}");
            }
        }
    }

    // Supporting classes
    class Assignment
    {
        public string SubjectInfo { get; set; }
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public string Info { get; set; }
    }

    class Submission
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Info { get; set; }
        public string AssignmentName { get; set; }
    }

    class Feedback
    {
        public string AssignmentName { get; set; }
        public string FeedbackText { get; set; }
    }
}



