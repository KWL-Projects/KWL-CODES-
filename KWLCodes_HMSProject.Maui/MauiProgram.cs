using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    // Simulated user database
    static Dictionary<string, string> userDatabase = new Dictionary<string, string>
    {
        { "user1", HashPassword("password123") },  // username: user1, password: password123
        { "user2", HashPassword("mypassword") }    // username: user2, password: mypassword
    };

    static void Main(string[] args)
    {
        // Secure login flow
        Console.WriteLine("Please log in.");
        Console.Write("Username: ");
        string username = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();

        if (Login(username, password))
        {
            Console.WriteLine("Login successful!");
            LogToFile($"User '{username}' logged in successfully.");
            string token = GenerateToken(username);
            Console.WriteLine($"Your session token: {token}");
        }
        else
        {
            Console.WriteLine("Login failed. Invalid username or password.");
            LogToFile($"Login attempt failed for user '{username}'.");
            return;
        }

        // Display menu options to the user
        Console.WriteLine("\nChoose an option:");
        Console.WriteLine("1. Create Assignment");
        Console.WriteLine("2. View Assignment");
        Console.WriteLine("3. View Submissions");
        Console.WriteLine("4. Browse Your Own Submissions");
        Console.WriteLine("5. View Feedback on Your Submissions");

        // Read the user's choice
        string choice = Console.ReadLine();

        // Process the user's choice
        if (choice == "1")
        {
            CreateAssignment(); // Handle assignment creation
        }
        else if (choice == "2")
        {
            ViewAssignment(); // Handle viewing an assignment
        }
        else if (choice == "3")
        {
            ViewSubmissions(); // Handle viewing submissions
        }
        else if (choice == "4")
        {
            BrowseOwnSubmissions(); // Handle browsing own submissions
        }
        else if (choice == "5")
        {
            ViewFeedback(); // Handle viewing feedback
        }
        else
        {
            // Inform the user of an invalid choice
            Console.WriteLine("Invalid choice.");
        }
    }

    static bool Login(string username, string password)
    {
        // Check if the username exists in the database
        if (userDatabase.ContainsKey(username))
        {
            // Retrieve the stored hashed password
            string storedHash = userDatabase[username];

            // Hash the provided password
            string providedHash = HashPassword(password);

            // Compare the hashed password with the stored hash
            if (storedHash == providedHash)
            {
                return true;  // Login successful
            }
        }

        return false;  // Login failed
    }

    static string HashPassword(string password)
    {
        // Create a SHA256 hash of the password
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

    static string GenerateToken(string username)
    {
        // Generate a simple token using the username and current time
        string token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{DateTime.Now}"));
        return token;
    }

    static void CreateAssignment()
    {
        // Collect assignment details from the user
        Console.WriteLine("Enter subject information:");
        string subjectInfo = Console.ReadLine();

        Console.WriteLine("Enter assignment name:");
        string assignmentName = Console.ReadLine();

        Console.WriteLine("Enter due date (yyyy-mm-dd):");
        string dueDateInput = Console.ReadLine();

        Console.WriteLine("Enter assignment information:");
        string assignmentInfo = Console.ReadLine();

        // Validate the user input
        if (string.IsNullOrWhiteSpace(subjectInfo) ||
            string.IsNullOrWhiteSpace(assignmentName) ||
            !DateTime.TryParse(dueDateInput, out DateTime dueDate) ||
            string.IsNullOrWhiteSpace(assignmentInfo))
        {
            // Log and display validation failure
            LogToFile("Validation failed: One or more input fields are invalid.");
            Console.WriteLine("Failure: One or more input fields are invalid. Please check your inputs.");
            return;
        }

        // Simulate database entry creation
        bool success = CreateDatabaseEntry(subjectInfo, assignmentName, dueDate, assignmentInfo);

        if (success)
        {
            // Log and display success message
            LogToFile($"Assignment '{assignmentName}' created successfully.");
            Console.WriteLine("Success: Assignment created successfully.");
        }
        else
        {
            // Log and display failure message
            LogToFile("Database entry creation failed.");
            Console.WriteLine("Failure: Could not create assignment. Please try again.");
        }
    }

    static bool CreateDatabaseEntry(string subjectInfo, string assignmentName, DateTime dueDate, string assignmentInfo)
    {
        // Simulate database entry creation
        Console.WriteLine("Simulating database entry creation...");
        return true; // Simulating success
    }

    static void ViewAssignment()
    {
        // Prompt the user to enter the assignment name they want to view
        Console.WriteLine("Enter the assignment name you want to view:");
        string assignmentName = Console.ReadLine();

        // Retrieve the assignment from the simulated database
        var assignment = RetrieveAssignmentFromDatabase(assignmentName);

        if (assignment != null)
        {
            // Log and display the assignment information
            LogToFile($"Assignment '{assignmentName}' retrieved successfully.");
            Console.WriteLine("Success: Assignment information:");
            Console.WriteLine($"Subject: {assignment.SubjectInfo}");
            Console.WriteLine($"Assignment Name: {assignment.Name}");
            Console.WriteLine($"Due Date: {assignment.DueDate:yyyy-MM-dd}");
            Console.WriteLine($"Information: {assignment.Info}");
        }
        else
        {
            // Log and display failure message if assignment not found
            LogToFile($"Assignment '{assignmentName}' not found.");
            Console.WriteLine("Failure: Assignment not found. Please check the name and try again.");
        }
    }

    static Assignment RetrieveAssignmentFromDatabase(string assignmentName)
    {
        // Simulate database retrieval
        if (assignmentName == "Sample Assignment")
        {
            // Return a dummy assignment for demonstration
            return new Assignment
            {
                SubjectInfo = "Sample Subject",
                Name = "Sample Assignment",
                DueDate = new DateTime(2024, 12, 31),
                Info = "Sample information about the assignment."
            };
        }
        return null; // Return null if assignment not found
    }

    static void ViewSubmissions()
    {
        // Prompt the user to enter the assignment name to view submissions
        Console.WriteLine("Enter the assignment name for which you want to view submissions:");
        string assignmentName = Console.ReadLine();

        // Retrieve submissions from the simulated database
        var submissions = RetrieveSubmissionsFromDatabase(assignmentName);

        if (submissions != null && submissions.Count > 0)
        {
            // Log and display the submissions information
            LogToFile($"Submissions for assignment '{assignmentName}' retrieved successfully.");
            Console.WriteLine("Success: Submissions information:");
            foreach (var submission in submissions)
            {
                Console.WriteLine($"Submission ID: {submission.Id}");
                Console.WriteLine($"Student Name: {submission.StudentName}");
                Console.WriteLine($"Submission Date: {submission.SubmissionDate:yyyy-MM-dd}");
                Console.WriteLine($"Submission Info: {submission.Info}");
                Console.WriteLine();
            }
        }
        else
        {
            // Log and display failure message if no submissions found
            LogToFile($"No submissions found for assignment '{assignmentName}'.");
            Console.WriteLine("Failure: No submissions found for the specified assignment. Please check the assignment name and try again.");
        }
    }

    static List<Submission> RetrieveSubmissionsFromDatabase(string assignmentName)
    {
        // Simulate database retrieval
        // Return a list of dummy submissions for demonstration
        if (assignmentName == "Sample Assignment")
        {
            return new List<Submission>
            {
                new Submission
                {
                    Id = 1,
                    StudentName = "John Doe",
                    SubmissionDate = new DateTime(2024, 8, 25),
                    Info = "Submitted via email."
                },
                new Submission
                {
                    Id = 2,
                    StudentName = "Jane Smith",
                    SubmissionDate = new DateTime(2024, 8, 26),
                    Info = "Submitted through the portal."
                }
            };
        }
        return new List<Submission>(); // Return an empty list if no submissions found
    }

    static void BrowseOwnSubmissions()
    {
        // Prompt the user to enter their name to browse their submissions
        Console.WriteLine("Enter your name to browse your submissions:");
        string studentName = Console.ReadLine();

        // Retrieve user's submissions from the simulated database
        var submissions = RetrieveSubmissionsForStudentFromDatabase(studentName);

        if (submissions != null && submissions.Count > 0)
        {
            // Log and display the submissions information
            LogToFile($"Submissions for student '{studentName}' retrieved successfully.");
            Console.WriteLine("Success: Your submissions information:");
            foreach (var submission in submissions)
            {
                Console.WriteLine($"Assignment Name: {submission.AssignmentName}");
                Console.WriteLine($"Submission Date: {submission.SubmissionDate:yyyy-MM-dd}");
                Console.WriteLine($"Submission Info: {submission.Info}");
                Console.WriteLine();
            }
        }
        else
        {
            // Log and display failure message if no submissions found
            LogToFile($"No submissions found for student '{studentName}'.");
            Console.WriteLine("Failure: No submissions found for your name. Please check your name and try again.");
        }
    }

    static List<Submission> RetrieveSubmissionsForStudentFromDatabase(string studentName)
    {
        // Simulate database retrieval
        // Return a list of dummy submissions for demonstration
        if (studentName == "John Doe")
        {
            return new List<Submission>
            {
                new Submission
                {
                    AssignmentName = "Sample Assignment",
                    SubmissionDate = new DateTime(2024, 8, 25),
                    Info = "Submitted via email."
                }
            };
        }
        return new List<Submission>(); // Return an empty list if no submissions found
    }

    static void ViewFeedback()
    {
        // Prompt the user to enter their name to view feedback
        Console.WriteLine("Enter your name to view feedback on your submissions:");
        string studentName = Console.ReadLine();

        // Retrieve feedback from the simulated database
        var feedback = RetrieveFeedbackForStudentFromDatabase(studentName);

        if (feedback != null && feedback.Count > 0)
        {
            // Log and display the feedback information
            LogToFile($"Feedback for student '{studentName}' retrieved successfully.");
            Console.WriteLine("Success: Your feedback:");
            foreach (var fb in feedback)
            {
                Console.WriteLine($"Assignment Name: {fb.AssignmentName}");
                Console.WriteLine($"Feedback: {fb.FeedbackText}");
                Console.WriteLine();
            }
        }
        else
        {
            // Log and display failure message if no feedback found
            LogToFile($"No feedback found for student '{studentName}'.");
            Console.WriteLine("Failure: No feedback found for your name. Please check your name and try again.");
        }
    }

    static List<Feedback> RetrieveFeedbackForStudentFromDatabase(string studentName)
    {
        // Simulate database retrieval
        // Return a list of dummy feedback entries for demonstration
        if (studentName == "John Doe")
        {
            return new List<Feedback>
            {
                new Feedback
                {
                    AssignmentName = "Sample Assignment",
                    FeedbackText = "Good work! Keep it up."
                }
            };
        }
        return new List<Feedback>(); // Return an empty list if no feedback found
    }

    static void LogToFile(string message)
    {
        // Append a log message to the log file
        string logFilePath = "log.txt";
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine($"{DateTime.Now}: {message}");
        }
    }
}

// Supporting classes for assignments, submissions, and feedback
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