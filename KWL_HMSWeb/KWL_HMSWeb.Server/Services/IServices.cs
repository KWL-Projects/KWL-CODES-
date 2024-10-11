//using KWL_HMSWeb.Server.Data;
using KWL_HMSWeb.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace KWL_HMSWeb.Services
{
    public interface IServices
    {
        Task<string> GetVideoFilePathAsync(int videoId);  // Add methods related to video handling
    }

    public interface ILogService
    {
        Task LogAsync(string message);
    }

    public class Services : IServices
    {
        private readonly DatabaseContext _context;

        public Services(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<string> GetVideoFilePathAsync(int videoId)
        {
            var submission = await _context.Submission.FirstOrDefaultAsync(s => s.submission_id == videoId);
            return submission?.video_path;
        }
    }

    public class LogService : ILogService
    {
        public async Task LogAsync(string message)
        {
            // Example logging to a file (implement your preferred logging mechanism)
            var logFilePath = "log.txt"; // Adjust this path as necessary
            await File.AppendAllTextAsync(logFilePath, $"{DateTime.UtcNow}: {message}{Environment.NewLine}");
        }
    }

    /*public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int user_id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int user_id);

        // Method to handle collection of Subjects
        Task<IEnumerable<Subject>> GetSubjectsForUserAsync(int user_id);
    }

    public interface ISubjectService
    {
        Task<IEnumerable<Subject>> GetAllSubjectsAsync();
        Task<Subject> GetSubjectByIdAsync(int subject_id);
        Task AddSubjectAsync(Subject subject);
        Task UpdateSubjectAsync(Subject subject);
        Task DeleteSubjectAsync(int subject_id);

        // Method to handle collection of Assignments
        Task<IEnumerable<Assignment>> GetAssignmentsForSubjectAsync(int subject_id);
    }

    public interface IAssignmentService
    {
        Task<IEnumerable<Assignment>> GetAllAssignmentsAsync();
        Task<Assignment> GetAssignmentByIdAsync(int assignmnet_id);
        Task AddAssignmentAsync(Assignment assignment);
        Task UpdateAssignmentAsync(Assignment assignment);
        Task DeleteAssignmentAsync(int assignmnet_id);

        // Method to handle collection of Submissions
        Task<IEnumerable<Submission>> GetSubmissionsForAssignmentAsync(int assignmnet_id);
    }

    public interface ISubmissionService
    {
        Task<IEnumerable<Submission>> GetAllSubmissionsAsync();
        Task<Submission> GetSubmissionByIdAsync(int submission_id);
        Task AddSubmissionAsync(Submission submission);
        Task UpdateSubmissionAsync(Submission submission);
        Task DeleteSubmissionAsync(int submission_id);

        // Method to handle collection of Feedback
        Task<IEnumerable<Feedback>> GetFeedbacksForSubmissionAsync(int submission_id);
    }

    public interface IFeedbackService
    {
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync();
        Task<Feedback> GetFeedbackByIdAsync(int feedback_id);
        Task AddFeedbackAsync(Feedback feedback);
        Task UpdateFeedbackAsync(Feedback feedback);
        Task DeleteFeedbackAsync(int feedback_id);
    }*/

}
