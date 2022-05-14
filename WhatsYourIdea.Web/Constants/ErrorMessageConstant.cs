namespace WhatsYourIdea.Web.Constants
{
    public static class ErrorMessageConstant
    {
        private static readonly Dictionary<int, string> _errorMessages = new()
        {
            { 400, "У вас недастаточно прав" },
            { 401, "Недостаточно прав" },
            { 404, "Запись не найдена" },
            { 500, "Ошибка сервера" }
        };

        public static string GetMessage(int code)
        {
            return _errorMessages[code];
        }
    }
}