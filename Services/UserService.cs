using LabAllianceTest.Abstractions;
using LabAllianceTest.Contracts;
using LabAllianceTest.Exceptions;
using LabAllianceTest.Helpers;
using LabAllianceTest.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace LabAllianceTest.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7243/api/User/");
        }

        public async Task<(string message, int statusCode)> RegistrationUserAsync(UserModel user)
        {
            var userRequest = new UserRequest(user.Login, user.Password);
            var request = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(userRequest), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("registration", request);

            if (response.IsSuccessStatusCode)
            {
                return ("Регистрация прошла успешно", 200);
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var content = await response.Content.ReadAsStringAsync();

                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);

                if (errorResponse?.Error?.Errors != null)
                {
                    throw new UserValidationException(errorResponse.Error.Errors);
                }
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return ("Пользоватлеь с таким Login уже существует.", 409);
            }

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadAsStringAsync(), (int)response.StatusCode);
        }

        public async Task<(string message, int statusCode)> LoginUserAsync(UserModel user)
        {
            var userRequest = new UserRequest(user.Login, user.Password);
            var request = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(userRequest), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("login", request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return (content, 200);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return ("Неверный логин или пароль", 401);
            }

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadAsStringAsync(), (int)response.StatusCode);
        }
    }
}
