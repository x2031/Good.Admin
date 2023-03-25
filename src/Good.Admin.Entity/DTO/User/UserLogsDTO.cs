namespace Good.Admin.Entity
{
    public class UserLogsDTO
    {
        public string logContent { get; set; }
        public string logType { get; set; }
        public string opUserName { get; set; }
        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }
    }
}
