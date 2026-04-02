namespace GestionApiClient.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public string? ResponseBody { get; }

        public ApiException(string message, int statusCode, string? responseBody = null)
            : base(message)
        {
            StatusCode = statusCode;
            ResponseBody = responseBody;
        }
    }
}