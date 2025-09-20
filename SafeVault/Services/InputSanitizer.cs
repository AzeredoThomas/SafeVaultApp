using Ganss.Xss;


public static class InputSanitizer
{
    private static readonly HtmlSanitizer _sanitizer = new HtmlSanitizer();

    public static string Clean(string input)
    {
        return string.IsNullOrWhiteSpace(input) ? input : _sanitizer.Sanitize(input.Trim());
    }
}
