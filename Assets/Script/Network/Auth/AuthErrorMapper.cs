using PlayFab;

public static class AuthErrorMapper
{
    public static string Map(PlayFabError error)
    {
        switch (error.Error)
        {
            case PlayFabErrorCode.EmailAddressNotAvailable:
                return "Ця пошта вже зареєстрована";
            case PlayFabErrorCode.UsernameNotAvailable:
                return "Цей нікнейм зайнятий";
            case PlayFabErrorCode.InvalidEmailOrPassword:
            case PlayFabErrorCode.AccountNotFound:
                return "Невірна пошта або пароль";
            case PlayFabErrorCode.InvalidEmailAddress:
                return "Некоректна пошта";
            case PlayFabErrorCode.InvalidPassword:
                return "Некоректний пароль";
            case PlayFabErrorCode.InvalidUsername:
                return "Некоректний нікнейм";
            case PlayFabErrorCode.ConnectionError:
                return "Немає з'єднання з сервером";
            default:
                return error.ErrorMessage;
        }
    }
}