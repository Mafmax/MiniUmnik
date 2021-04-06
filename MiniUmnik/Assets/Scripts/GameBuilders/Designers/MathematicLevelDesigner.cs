using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MathematicLevelDesigner : LevelDesigner
{


    public override IEnumerator CreateLevel(int level)
    {
        answerRecieved = false;
        currentLevel = level;
        yield return StartCoroutine(CreateAnswersCoroutine());
    }


    private IEnumerator CreateAnswersCoroutine()
    {


        answers.Clear();
        var answerQuestion = ConfigureQuestion();
        question.color = Color.blue;
        question.fontSize = 25;
        question.text = answerQuestion.Value;

        int answersCount = 0;
        float time = 0;

        if (currentLevel < 3)
        {
            time = 18;
            answersCount = 8;
        }
        else if (currentLevel < 7)
        {
            time = 19;
            answersCount = 6;

        }
        else if (answersCount <= 10)
        {
            time = 19;
            answersCount = 4;
        }
        foreach (var answ in GetAnswers(answersCount, answerQuestion.Key))
        {
            var answer = Instantiate(answerTextExample);
            if (answ == answerQuestion.Key)
            {
                answer.IsCorrect = true;
            }
            answer.Create(OnChoice, answ);
            answers.Add(answer);
        }

        PlaceAnswers();
        StartTimer(time);
        yield break;
    }

    public override IEnumerator RemoveLevel()
    {
        for (int i = 0; i < answers.Count; i++)
        {
            if (answers[i] != null)
            {
                
            Destroy(answers[i].gameObject);
            }
        }
        StopAllCoroutines();
        yield break;
    }


    private void OnChoice(Answer answer)
    {
        bool completed = answer.IsCorrect;
        string data = completed ? $"Решено верно" : "Ошибка";
        SendStatistics(completed, data);
    }
    private KeyValuePair<string, string> ConfigureQuestion(int numbersCount = 20)
    {
        double x = UnityEngine.Random.Range(0, numbersCount);
        double y = UnityEngine.Random.Range(0, numbersCount);
        int operation = UnityEngine.Random.Range(0, 4);
        StringBuilder question = new StringBuilder();
        string answer = "";
        question.Append("Реши пример: ");
        switch (operation)
        {
            case 0: question.Append($"{x}*{y}="); answer = (x * y).ToString(); break;
            case 1: question.Append($"{x}+{y}="); answer = (x + y).ToString(); break;
            case 2: question.Append($"{x}-{y}="); answer = (x - y).ToString(); break;
            case 3: question.Append($"{x}/{y}="); answer = (x / y).ToString(); break;
        }

        bool correct = false;
        if (Int32.TryParse(answer, out int result))
        {
            if (result >= 0 && result < numbersCount)
            {
                correct = true;
            }
        }

        if (!correct)
        {
            return ConfigureQuestion(numbersCount);
        }
        else
        {
            return new KeyValuePair<string, string>(answer, question.ToString());
        }

    }
    private IEnumerable<string> GetAnswers(int count, string right)
    {
        int rightIndex = UnityEngine.Random.Range(0, count - 1);
        List<string> numbers = new List<string>();
        for (int i = 0; i < 20; i++)
        {
            numbers.Add(i.ToString());
        }
        numbers.Remove(right);

        for (int i = 0; i < count - 1; i++)
        {
            if (i == rightIndex)
            {
                yield return right;
            }
            var rnd = UnityEngine.Random.Range(0, numbers.Count);
            yield return numbers[rnd];
            numbers.RemoveAt(rnd);
        }
    }
}
