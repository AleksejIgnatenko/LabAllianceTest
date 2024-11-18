using LabAllianceTest.Abstractions;
using LabAllianceTest.Contracts;
using LabAllianceTest.Exceptions;
using LabAllianceTest.Helpers;
using LabAllianceTest.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace LabAllianceTest.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IFileStorage _fileStorage;
        private readonly IPasswordHasher _passwordHasher;

        public UserService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7243/api/User/");
            _fileStorage = new JsonFileStorage("tokenResponse.json");
            _passwordHasher = new PasswordHasher();
        }

        // Регистрация пользователя
        public async Task<(string message, int statusCode)> RegistrationUserAsync(UserModel user)
        {
            // Создание запроса
            var userRequest = new UserRequest(user.Login, _passwordHasher.Generate(user.Password));
            var request = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(userRequest), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("registration", request);

            // Проверка ответа по StatusCode
            if (response.IsSuccessStatusCode)
            {
                return ("Регистрация прошла успешно", 200);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var content = await response.Content.ReadAsStringAsync();

                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);

                if (errorResponse?.error != null)
                {
                    throw new UserValidationException(errorResponse.error);
                }
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return ("Пользователь с таким Login уже существует.", 409);
            }

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadAsStringAsync(), (int)response.StatusCode);
        }

        // Вход
        public async Task<(string message, int statusCode)> LoginUserAsync(UserModel user)
        {
            // Создание запроса
            var userRequest = new UserRequest(user.Login, _passwordHasher.Generate(user.Password));
            var request = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(userRequest), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/connect/token", request);

            // Проверка ответа по StatusCode
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content);

                await _fileStorage.WriteToFileJsonAsync(tokenResponse);

                return ("Вход произошел успешно!", 200);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return ("Неверный логин или пароль", 401);
            }


            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadAsStringAsync(), (int)response.StatusCode);
        }

        public async Task<List<UserModel>?> GetAllUsersAsync()
        {
            // Получение токена и добавление его в headers
            var token = await _fileStorage.ReadAccessTokenFromFileJsonAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("");

            // Проверка ответа по StatusCode
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var getAllUsersResponse = JsonSerializer.Deserialize<List<UserResponse>>(content)
                    ?? throw new InvalidOperationException("Не удалось десериализовать ответ пользователей.");

                var users = getAllUsersResponse.Select(u => UserModel.Create(u.id, u.login, u.password, false).user).ToList();

                return users;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new AuthenticationFailedException("Вы не авторизованы.\nЕсли вы ранее были авторизованы, пожалуйста, обновите токен.");
            }

            response.EnsureSuccessStatusCode();

            return null;
        }

        public async Task<(string message, int statusCode)> RefreshToken()
        {
            // Получение refreshToken и создание запроса
            var refreshToken = await _fileStorage.ReadRefreshTokenFromFileJsonAsync();
            var request = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(refreshToken), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/connect/refresh", request);

            // Проверка ответа по StatusCode
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content);

                await _fileStorage.WriteToFileJsonAsync(tokenResponse);

                return ("Токен успешно обновлен!", 200);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var content = await response.Content.ReadAsStringAsync();
                var error = JsonSerializer.Deserialize<TokenErrorResponse>(content);

                return (error.Error, 400);
            }

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadAsStringAsync(), (int)response.StatusCode);
        }
    }
}
