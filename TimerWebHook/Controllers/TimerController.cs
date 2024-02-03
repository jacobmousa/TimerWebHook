using Microsoft.AspNetCore.Mvc;
using TimerWebHook.Logic;
using TimerWebHook.Logic.Model;

namespace TimerWebHook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimerController : ControllerBase
    {
        private static readonly TimerManager timerManager = new TimerManager();

        private readonly DataStorageService _dataStorageService;

        public TimerController(DataStorageService dataStorageService)
        {
            _dataStorageService = dataStorageService;
            timerManager.timers = _dataStorageService.GetDataList();
        }

        [HttpPost]
        [Route("setTimer")]
        public ActionResult<TimerResponse> SetTimer([FromBody]TimerSet data)
        {
            int totalSecond = data.Hours * 60 + data.Minutes * 60 + data.Seconds;
            var timerAdded = timerManager.AddTimer((guid) => TimerCallback(guid), totalSecond,data.WebhookUrl); // 60 seconds interval
            TimerResponse timerResponse = new TimerResponse(timerAdded.Id, GetTimeLeft(timerAdded.executeTime));

            _dataStorageService.AddData(timerAdded);
            return Ok(timerResponse);
        }

        [HttpGet]
        [Route("getTimer/{id}")]
        public ActionResult<TimerResponse> GetTimer(Guid id)
        {

            var timerItem = timerManager.GetTimer(id);
            if (timerItem == null)
            {
                return NotFound();
            }

            TimerResponse timerResponse = new TimerResponse(id, GetTimeLeft(timerItem.executeTime));
            return Ok(timerResponse);
        }

        [HttpGet]
        [Route("list/{status}/{pageSize}/{pageNumber}")]
        public ActionResult<TimerListResponse> List(string status,int pageSize = 100, int pageNumber = 1)
        {
            List<TimerItem> result = timerManager.GetList(status,pageNumber,pageSize);

            TimerListResponse timerResponse = new()
            {
                pageNumber = 1,
                pageSize = 100
            };

            List<TimerListItemResponse> items  = [];
            foreach(var resultItem in result)
            {
                items.Add(new TimerListItemResponse()
                {
                    id = resultItem.Id,
                    webhookUrl = resultItem.WebHookUrl,
                    status = resultItem.Status
                });
            }
            timerResponse.items = items;
            return Ok(timerResponse);
        }

        private static int GetTimeLeft(TimeSpan executeTime)
        {
            int result = (int)(executeTime - DateTime.Now.TimeOfDay).TotalSeconds;
            result = (int)Math.Max(result, 0);
            return result;
        }

        private void TimerCallback(Guid timerId)
        {
            var result = timerManager.timers.Where(a => a.Id == timerId).FirstOrDefault();
            if (result != null)
            {
                result.Status = "Finished";
                result.TimerValue?.Dispose();

                CallExternalApi(result.WebHookUrl);
            }
        }

        private async void CallExternalApi(string Url)
        {
            string apiUrl = Url;
            ExternalApiService apiService = new ExternalApiService();

            try
            {
                string result = await apiService.CallExternalApi(apiUrl);
                Console.WriteLine($"API Response: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Timer callback executed!");
        }
    }
}
