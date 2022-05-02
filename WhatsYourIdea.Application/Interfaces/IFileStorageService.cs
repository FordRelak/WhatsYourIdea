namespace WhatsYourIdea.Applications.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> CreateIdeaImageFileAsync(Stream stream, string fileExtension, CancellationToken cancellationToken);
    }
}