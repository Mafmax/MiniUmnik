using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDesigner : MonoBehaviour
{
    protected int currentLevel;
    protected PlatformController platform;
    protected List<Answer> answers = new List<Answer>();
    protected bool answerRecieved = false;
    protected Transform answerAnchor;
    protected AnswerPic answerPicExample;
    protected AnswerText answerTextExample;
    protected Text question;
    private float TimeLeft { get; set; }
    private Coroutine Timer { get; set; }
    public event Action<float> OnTimerTick;
    private IEnumerator TimerCoroutine(float time)
    {
        TimeLeft = 0.0f;
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            TimeLeft += 0.2f;
            OnTimerTick?.Invoke(time - TimeLeft);
            if (TimeLeft >= time)
            {
                SendStatistics(false, "Время истекло");
                break;
            }
        }
    }
    protected void StartTimer(float time)
    {
        Timer = StartCoroutine(nameof(TimerCoroutine), time);
    }
    protected void SendStatistics(bool completed, string data)
    {
        data = completed ? data + $"(За время {TimeLeft})" : data;
        StatisticsMaker.AddStatistics(new Statistics(currentLevel + 1, completed, data));
        answerRecieved = true;
        if (Timer != null)
        {

        StopCoroutine(Timer);
        }
    }
    protected virtual void Awake()
    {
        answerPicExample = Resources.Load<AnswerPic>("Prefabs/AnswerPic");
        answerTextExample = Resources.Load<AnswerText>("Prefabs/AnswerText");
        platform = FindObjectOfType<PlatformController>();
        answerAnchor = GameObject.Find("Answers").transform;
        question = UIController.GetMenu<GameMenu>().question;

    }
    public virtual IEnumerator CreateLevel(int level)
    {
        Debug.Log($"Старт уровня (заглушка)");
        yield return new WaitForSeconds(4);

        yield break;
    }
    private void MixAnswers()
    {
        var temp = new List<Answer>();
        while (answers.Count > 0)
        {
            var rnd = UnityEngine.Random.Range(0, answers.Count);
            temp.Add(answers[rnd]);
            answers.RemoveAt(rnd);
        }
        answers = temp;
    }
    protected void PlaceAnswers()
    {
        MixAnswers();
        int count = answers.Count;
        for (int i = 0; i < count; i++)
        {
            float PI = 3.14159f;
            var angle = -PI / 2 + PI / count * (i + 0.5f);
            var r = 6.0f;
            var offset = new Vector3(-r * Mathf.Sin(angle), 0.5f, r * Mathf.Cos(angle));
            answers[i].transform.position = answerAnchor.position + offset;
            answers[i].transform.forward = -new Vector3(offset.x, 0, offset.z);
        }

    }
    public virtual IEnumerator RemoveLevel()
    {

        Debug.Log($"Удаление уровня (заглушка)");
        yield break;
    }
    public virtual IEnumerator WaitAnswer()
    {
            yield return new WaitWhile(()=>!answerRecieved);
    }
}
