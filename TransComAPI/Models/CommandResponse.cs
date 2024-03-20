namespace TransComAPI.Models
{
    public class CommandResponse
    {
        public bool Success { get; set; }
        public string Response { get; set; }
        public string ErrorMessage { get; set; }
        public string SendTimestamp { get; set; } 
        public string ReceiveTimestamp { get; set; } 
        public double Interval { get; set; }
    }


}
