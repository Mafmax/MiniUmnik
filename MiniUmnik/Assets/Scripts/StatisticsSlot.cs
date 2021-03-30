using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsSlot : MonoBehaviour
{
    private Image Result { get; set; }
    private Text Data { get; set; }
    // Start is called before the first frame update
    void Awake()
    {
        Data = GetComponentsInChildren<Text>().Where(x=>x.name.Equals("Data",System.StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        Result = GetComponentInChildren<Image>();

    }
    public void Show(Statistics? statistics)
    {

        if (statistics == null)
        {
            Result.color = Color.gray;
            Data.text = new string('-', 5);
        }
        else
        {
            Result.color = statistics.Value.IsCompleted ? Color.green : Color.red;
            Data.text = statistics.Value.Data;

        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
