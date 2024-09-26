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

}
