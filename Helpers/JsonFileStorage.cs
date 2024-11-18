using System.IO;
using System.Text;
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

        // Сохранение токена в файл
        public async Task WriteToFileJsonAsync(TokenResponse token)
        {
            try
            {
                // Сериализация объекта TokenResponse в строку JSON
                var json = JsonSerializer.Serialize(token, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                // Проверка на существование файла
                if (!File.Exists(_filePath))
                {
                    // Создание файла и запись JSON в него
                    using (var stream = File.Create(_filePath))
                    {
                        await stream.WriteAsync(Encoding.UTF8.GetBytes(json));
                    }
                }
                else
                {
                    // Запись JSON в существующий файл
                    await File.WriteAllTextAsync(_filePath, json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось записать в файл: " + ex.Message);
            }
        }

        // Чтение AccessToken из файла
        public async Task<string> ReadAccessTokenFromFileJsonAsync()
        {
            try
            {
                // Проверка наличия файла
                if (File.Exists(_filePath))
                {
                    // Получение AccessToken из файла
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

        // Чтение RefreshToken из файла
        public async Task<string> ReadRefreshTokenFromFileJsonAsync()
        {
            try
            {
                // Проверка наличия файла
                if (File.Exists(_filePath))
                {
                    // Получение RefreshToken из файла
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
