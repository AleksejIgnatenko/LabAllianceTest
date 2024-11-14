using System.IO;
using System.Windows;
using LabAllianceTest.Abstractions;

namespace LabAllianceTest.Helpers
{
    internal class FileStorage : IFileStorage
    {
        private readonly string _filePath;

        public FileStorage(string filePath)
        {
            _filePath = filePath;
        }
        public async Task WriteToFileAsync(string content)
        {
            try
            {
                await File.WriteAllTextAsync(_filePath, content);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось записать в файл: " + ex.Message);
            }
        }

        public async Task<string> ReadFromFileAsync()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    return await File.ReadAllTextAsync(_filePath);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось прочитать файл.");
                return string.Empty;
            }
        }
    }
}
