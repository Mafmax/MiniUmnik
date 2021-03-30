using System;
using System.Collections.Generic;

public static class StatisticsMaker
{
    public static event Action<Statistics> OnChange;
    private static List<Statistics> Statistics = new List<Statistics>();
    public static void Reset()
    {
        Statistics.Clear();
    }
    public static void AddStatistics(Statistics statistics)
    {
        Statistics.Add(statistics);
        OnChange?.Invoke(statistics);
    }
    public static IEnumerable<Statistics> GetStatistics()
    {
        foreach (var statistics in Statistics)
        {
            yield return statistics;
        }
    } 
}
