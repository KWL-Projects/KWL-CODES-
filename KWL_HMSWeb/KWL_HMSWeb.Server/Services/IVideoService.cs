namespace KWL_HMSWeb.Services
{
    public interface IVideoService
    {
        Task<string> GetVideoFilePathAsync(int videoId);  // Add methods related to video handling
    }
}
