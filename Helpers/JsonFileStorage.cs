using System.IO;
using System.Text.Json;
using System.Windows;
using LabAllianceTest.Abstractions;
using LabAllianceTest.Contracts;

namespace LabAllianceTest.Helpers
{
    internal class JsonFileStorage : IFileStorage
    {
        private readonly string _filePath;

        public JsonFileStorage(string filePath)
        {
            _filePath = filePath;
        }

        public async Task WriteToFileJsonAsync(TokenResponse token)
        {
            try
            {
                // Сериализация объекта TokenResponse в строку JSON
                var json = JsonSerializer.Serialize(token, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                // Запись JSON в файл
                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось записать в файл: " + ex.Message);
            }
        }

        public async Task<string> ReadAccessTokenFromFileJsonAsync()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    var jsonString = await File.ReadAllTextAsync(_filePath);
                    var jsonDocument = JsonDocument.Parse(jsonString);
                    if (jsonDocument.RootElement.TryGetProperty("access_token", out var accessToken))
                    {
                        return accessToken.GetString();
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось прочитать файл: " + ex.Message);
                return string.Empty;
            }
        }

        public async Task<string> ReadRefreshTokenFromFileJsonAsync()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    var jsonString = await File.ReadAllTextAsync(_filePath);
                    var jsonDocument = JsonDocument.Parse(jsonString);
                    if (jsonDocument.RootElement.TryGetProperty("refresh_token", out var refreshToken))
                    {
                        return refreshToken.GetString();
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось прочитать файл: " + ex.Message);
                return string.Empty;
            }
        }
    }
}
