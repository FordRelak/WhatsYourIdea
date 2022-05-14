using Microsoft.Extensions.Options;
using WhatsYourIdea.Applications.Services.Configurations;
using WhatsYourIdea.Applications.Services.Interfaces;

namespace WhatsYourIdea.Applications.Services.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly FileStorageSettings _settings;

        public FileStorageService(IOptions<FileStorageSettings> options)
        {
            _settings = options.Value;
        }

        public async Task<string> CreateIdeaImageFileAsync(Stream stream, string fileExtenshion, CancellationToken cancellationToken)
        {
            var newFileName = Path.GetRandomFileName();
            newFileName = Path.ChangeExtension(newFileName, fileExtenshion);
            var pathToFile = Path.Combine(_settings.IdeaImagesFolderPath, newFileName);

            using(var fs = new FileStream(pathToFile, FileMode.Create))
            {
                var readCount = 0;
                var buffer = new byte[8192];

                while((readCount = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) != 0)
                {
                    await fs.WriteAsync(buffer, 0, readCount, cancellationToken);
                }
            }

            return Path.Combine(Path.PathSeparator + _settings.IdeaImagesFolderName, newFileName);
        }
    }
}