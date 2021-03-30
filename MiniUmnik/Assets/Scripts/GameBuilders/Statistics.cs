using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct Statistics
{
    public int Level { get; set; }
    public string Data { get; set; }
    public bool IsCompleted { get; set; }
    public Statistics(int level, bool isCompleted, string data)
    {
        Data = data;
        Level = level;
        IsCompleted = isCompleted;
    }
}

