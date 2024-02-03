using Newtonsoft.Json;

namespace TimerWebHook.Logic.Model
{
    public class TimerItem
    {
        [JsonIgnore]
        public Timer? TimerValue { get; set; }
        public TimeSpan executeTime { get; set; }
        public Guid Id { get; set; }
        public string WebHookUrl { get; set; }

        public string Status { get; set; }
    }
}
