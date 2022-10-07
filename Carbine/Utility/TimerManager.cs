using System.Collections.Generic;

namespace Carbine.Utility
{
    public class TimerManager
    {
        public static TimerManager Instance
        {
            get
            {
                if (TimerManager.instance == null)
                {
                    TimerManager.instance = new TimerManager();
                }
                return TimerManager.instance;
            }
        }

        public event TimerManager.OnTimerEndHandler OnTimerEnd;

        private TimerManager()
        {
            this.timers = new List<TimerManager.Timer>();
        }

        public int StartTimer(int duration)
        {
            long frame = Engine.Frame;
            TimerManager.Timer item = new TimerManager.Timer
            {
                End = frame + duration,
                Index = ++this.timerCounter
            };
            this.timers.Add(item);
            return this.timerCounter;
        }

        public void Cancel(int timerIndex)
        {
            for (int i = 0; i < this.timers.Count; i++)
            {
                if (this.timers[i].Index == timerIndex)
                {
                    this.timers.RemoveAt(i);
                    return;
                }
            }
        }

        public void Update()
        {
            for (int i = 0; i < this.timers.Count; i++)
            {
                if (this.timers[i].End < Engine.Frame)
                {
                    if (this.OnTimerEnd != null)
                    {
                        this.OnTimerEnd(this.timers[i].Index);
                    }
                    this.timers.RemoveAt(i);
                    i--;
                }
            }
        }

        private static TimerManager instance;

        private List<TimerManager.Timer> timers;

        private int timerCounter;

        private struct Timer
        {
            public long End;

            public int Index;
        }

        public delegate void OnTimerEndHandler(int timerIndex);
    }
}
