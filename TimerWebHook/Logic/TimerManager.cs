using Newtonsoft.Json;
using System.Xml.Linq;
using TimerWebHook.Logic.Model;

namespace TimerWebHook.Logic
{
    public class TimerManager
    {
        public List<TimerItem> timers;

        public TimerItem AddTimer(Action<Guid> callback, int intervalInSeconds,string webhookUrl)
        {
            Guid guid = Guid.NewGuid();
            TimerItem item;
            TimeSpan executeTime = TimeSpan.FromSeconds(intervalInSeconds);
            var timer = new Timer(
                state => callback.Invoke(guid),
                null, executeTime,
                TimeSpan.Zero);

            item = new TimerItem()
            {
                TimerValue = timer,
                Id = guid,
                executeTime = DateTime.Now.TimeOfDay.Add(executeTime),
                WebHookUrl = webhookUrl,
                Status = "Started"
            };
            timers.Add(item);

            return (item);
        }

        public TimerItem GetTimer(Guid id)
        {
            var result = timers.Where(a => a.Id == id);
            return result.FirstOrDefault();
        }

        public List<TimerItem> GetTimers()
        {
            return timers;
        }

        public List<TimerItem> GetList(string status, int pageNumber, int pageSize)
        {
            var resultItems = timers.Where(a => a.Status == status).ToList();
            resultItems = GetPagedItems<TimerItem>(resultItems, pageNumber, pageSize);
          

            return (resultItems);
        }

        List<T> GetPagedItems<T>(List<T> items, int pageNumber, int pageSize)
        {
            return items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }


       }

}
