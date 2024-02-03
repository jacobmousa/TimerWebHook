namespace TimerWebHook.Logic.Model
{
    public class TimerResponse
    {
        public TimerResponse(Guid id, int timeLeft)
        {
            Id = id;
            TimeLeft = timeLeft;
        }

        public Guid Id { get; set; }
        public int TimeLeft { get; set; }
    }
}
