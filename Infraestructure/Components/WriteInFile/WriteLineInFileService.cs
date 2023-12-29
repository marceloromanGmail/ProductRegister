using Application.Components;

namespace Infraestructure.Components.WriteInFile
{
    public class WriteLineInFileService : IWriteLineInFileService
    {
        private readonly string _pathFile;
        public WriteLineInFileService(string pathFile)
        {
            _pathFile = pathFile;
        }
        public async Task WriteLineAsync(string text)
        {
            await File.AppendAllLinesAsync(_pathFile, new[] { text });
        }
    }
}
