# Yandex Music OAuth Tool

This project provides a tool for authorization via OAuth 2.0 in Yandex and working with the Yandex.Music API. The program allows you to obtain an access token, use it to work with the API, and saves user information to a local file.

## Main functions
- User authorization via **OAuth 2.0**.
- Receiving an **API token** from the Yandex.Music service.
- Saving data to the file `YandexMusicAPI.txt` in the format:
  ```
  Login/API
  ```
- Updating the token if the login already exists in the file.

## Installation and launch

### 1. If you already have a compiled `.exe` file:
1. Download the executable file from the repository or project build.
2. Make sure you have **.NET Runtime** installed (version 6.0 and higher). You can download it from the official website [Microsoft .NET](https://dotnet.microsoft.com/download).
3. Run the program by double-clicking on the `.exe` file or via the command line:
   ```bash
   path\to\your\program.exe
   ```

### 2. If you want to build the project yourself:
1. Clone the repository:
   ```bash
   cd (path to save folder)
   git clone https://github.com/Moontlex/YandexMusicAPI.git
   ```
2. Open the project in **Visual Studio**.
3. Make sure you have the **.NET 6.0 SDK** desktop environment installed.
4. Build the project using the `Build` button.

## How to use
1. **Launch the program**.
2. Enter your login that you want to associate with the Yandex.Music token.
3. **The program will open a browser** where you can log in to Yandex.
4. After successful authorization, enter the **confirmation code** (get it from the browser).
5. Program:
   - Receive an access token (`access_token`) from Yandex.
   - Saves the login and token to the file `YandexMusicAPI.txt`.
6. If the login already exists in the file, the token will be updated.

## Data saving format
The program creates or updates the `YandexMusicAPI.txt` file in the launch directory. File format:
```
login/API_token
```

### Example:
```
Ivan.Ivanov/y0_AAAAAAA-AAAAAAAAAAAAAAAAAAAAA
```

## Notes
- **Authorization parameters**:
  To use this tool, replace the values ​​in the program code:
  - `ClientId`: your application identifier registered on the Yandex platform.
  - `ClientSecret`: application secret key.
  - `RedirectUri`: URI specified when registering the application (usually `https://oauth.yandex.ru/verification_code`).

- **Secret keys**:
  Never publish your `ClientId` and `ClientSecret` to public repositories. Use environment variables or encryption to protect sensitive data.

## Requirements
- **OS**: Windows
- **.NET Runtime**: versions 6.0 and higher.
- Internet connection to access the Yandex API.

## Contribution
If you would like to contribute:
1. Fork the repository.
2. Make changes.
3. Submit a pull request.

## License
This project is licensed under the MIT License. Read more in [LICENSE](LICENSE).
