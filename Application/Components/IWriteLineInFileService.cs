namespace Application.Components
{
    public interface IWriteLineInFileService
    {
        Task WriteLineAsync(string text);
    }
}
