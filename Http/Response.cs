namespace Proje1.Http
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string InnerException { get; set; }
        public string SpecialData { get; set; }
        public int FisNo { get; set; }
        public bool Success { get; set; }
    }
}
