namespace WhatsYourIdea.Common
{
    public static class StringExtension
    {
        public static string CutController(this string controllerString)
        {
            return controllerString.Replace("Controller", "");
        }
    }
}