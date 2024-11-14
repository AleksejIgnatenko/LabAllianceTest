namespace LabAllianceTest.Abstractions
{
    internal interface IFileStorage
    {
        Task<string> ReadFromFileAsync();
        Task WriteToFileAsync(string content);
    }
}