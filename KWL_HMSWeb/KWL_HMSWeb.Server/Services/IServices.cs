//using KWL_HMSWeb.Server.Data;
using KWL_HMSWeb.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
}
