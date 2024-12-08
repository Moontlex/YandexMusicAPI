using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    // Замените на свои данные
    private const string ClientId = "Ваш ClientId";
    private const string ClientSecret = "Ваш ClientSecret";
    private const string RedirectUri = "https://oauth.yandex.ru/verification_code"; // Для десктоп-приложений
    private const string FileName = "YandexMusicAPI.txt"; // Имя файла для сохранения данных

    static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine("Введите ваш логин:");
            string login = Console.ReadLine();

            Console.WriteLine("Открываем браузер для авторизации...");

            // Формируем URL для авторизации
            string authUrl = $"https://oauth.yandex.ru/authorize?response_type=code&client_id={ClientId}&redirect_uri={RedirectUri}";

            // Открываем браузер для получения авторизационного кода
            Process.Start(new ProcessStartInfo(authUrl) { UseShellExecute = true });

            Console.WriteLine("Введите код подтверждения (code), полученный после авторизации:");
            string authorizationCode = Console.ReadLine();

            // Получаем токен доступа
            string accessToken = await GetAccessTokenAsync(authorizationCode);

            if (!string.IsNullOrEmpty(accessToken))
            {
                Console.WriteLine("Успешная авторизация!");
                Console.WriteLine($"Ваш токен доступа: {accessToken}");

                // Сохраняем данные в файл
                SaveOrUpdateDataInFile(login, accessToken);

                // Пример вызова API Yandex.Music
                await GetYandexMusicAccountInfoAsync(accessToken);
            }
            else
            {
                Console.WriteLine("Ошибка авторизации. Проверьте данные.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("\nПрограмма завершила выполнение. Нажмите Enter для выхода...");
            Console.ReadLine(); // Ожидание нажатия Enter перед выходом
        }
    }

    private static async Task<string> GetAccessTokenAsync(string authorizationCode)
    {
        using HttpClient client = new HttpClient();
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("code", authorizationCode),
            new KeyValuePair<string, string>("client_id", ClientId),
            new KeyValuePair<string, string>("client_secret", ClientSecret)
        });

        HttpResponseMessage response = await client.PostAsync("https://oauth.yandex.ru/token", content);
        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsAsync<dynamic>();
            return responseData.access_token;
        }

        Console.WriteLine($"Ошибка: {response.StatusCode}");
        return null;
    }

    private static async Task GetYandexMusicAccountInfoAsync(string accessToken)
    {
        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth", accessToken);

        HttpResponseMessage response = await client.GetAsync("https://api.music.yandex.net/account/status");
        if (response.IsSuccessStatusCode)
        {
            string accountInfo = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Информация о вашем аккаунте Yandex.Music:");
            Console.WriteLine(accountInfo);
        }
        else
        {
            Console.WriteLine($"Ошибка доступа к Yandex.Music API: {response.StatusCode}");
        }
    }

    private static void SaveOrUpdateDataInFile(string login, string apiKey)
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);

        // Считываем существующие данные, если файл есть
        var data = new List<string>();
        if (File.Exists(filePath))
        {
            data = File.ReadAllLines(filePath).ToList();
        }

        // Проверяем, существует ли запись для данного логина
        bool updated = false;
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].StartsWith($"{login}/"))
            {
                data[i] = $"{login}/{apiKey}"; // Обновляем токен
                updated = true;
                break;
            }
        }

        // Если логин не найден, добавляем новую запись
        if (!updated)
        {
            data.Add($"{login}/{apiKey}");
        }

        // Перезаписываем файл с обновленными данными
        File.WriteAllLines(filePath, data);

        Console.WriteLine($"Данные успешно сохранены или обновлены в файле: {filePath}");
    }
}
