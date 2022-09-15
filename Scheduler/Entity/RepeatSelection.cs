using System;
namespace Scheduler.Entity
{
    public enum RepeatSelectionEnum
    {
        None = 0,
        EveryDay = 1,
        EveryWeek = 2,
        EveryMonth = 3,
        EveryYear = 4,
    }

    public enum RepeatEndEnum
    {
        Never = 0,
        After = 1,
        OnDate = 2
    }
}