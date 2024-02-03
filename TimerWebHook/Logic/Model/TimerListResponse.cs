namespace TimerWebHook.Logic.Model
{
    public class TimerListResponse
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }

        public List<TimerListItemResponse> items { get; set; }

    }

    public class TimerListItemResponse 
    {
        public Guid id  { get; set; }
        public DateTime dateCreated { get; set; }
        public string webhookUrl { get; set; }
        public string status { get; set; }

    }

}
