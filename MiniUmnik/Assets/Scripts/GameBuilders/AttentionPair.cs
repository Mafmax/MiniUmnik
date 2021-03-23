using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttentionPair : MonoBehaviour
{
    private Transform Left { get; set; }
    private Tuple<AttentionDifference, AttentionDifference>[] Differences { get; set; }
    private Transform Right { get; set; }
    public event Action OnCompleted;
    public event Action OnAnswered;
    private int currentAnswered;
    public int CurrentAnswered 
    {
        get 
        {
            return currentAnswered;
        }
       private set 
        {
            if (value == Differences.Length)
            {
                currentAnswered = 0;
                OnCompleted?.Invoke();
            }
            else
            {
                currentAnswered = value;
            }
        }
    }
    private void OnAnswer()
    {
        CurrentAnswered = CurrentAnswered+1;
        OnAnswered.Invoke();
        Debug.Log($"Отличий найдено: {currentAnswered}");
    }

    // Start is called before the first frame update
    void Awake()
    {
        var childsTransworms = GetComponentsInChildren<Transform>(true);
        Left = childsTransworms.Where(x => x.name.ToLower() == "Left".ToLower()).FirstOrDefault();
        Right = childsTransworms.Where(x => x.name.ToLower() == "Right".ToLower()).FirstOrDefault();
        var leftAnswers = Left.GetComponentsInChildren<Transform>().Where(x => x.name.Contains("Answer")).ToArray();
        var rightAnswers = Right.GetComponentsInChildren<Transform>().Where(x => x.name.Contains("Answer")).ToArray();
        Differences = new Tuple<AttentionDifference, AttentionDifference>[10];
        for (int i = 0; i < 10; i++)
        {

            Differences[i] = new Tuple<AttentionDifference, AttentionDifference>(leftAnswers[i].gameObject.AddComponent<AttentionDifference>(), rightAnswers[i].gameObject.AddComponent<AttentionDifference>());
            Differences[i].Item1.OnOpened += Differences[i].Item2.Open;
            Differences[i].Item2.OnOpened += Differences[i].Item1.Open;
            Differences[i].Item1.OnOpened += OnAnswer;
        }
        Extrude(0.01f);
    }
    private void Extrude(float distance)
    {
        for (int i = 0; i < Differences.Length; i++)
        {
            Differences[i].Item1.transform.Translate(0, distance, 0);
            Differences[i].Item2.transform.Translate(0, distance, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
