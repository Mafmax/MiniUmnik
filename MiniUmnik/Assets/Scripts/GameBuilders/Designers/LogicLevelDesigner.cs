using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LogicLevelDesigner : LevelDesigner
{

    private static int BoxesEndLevel = 10;
    private bool[,,] cubes;
    private Color[] colors = new Color[4];
    private GameObject cube;
    private int size;




    protected sealed override void Awake()
    {
        base.Awake();
        colors[0] = Color.red;
        colors[1] = Color.green;
        colors[2] = Color.blue;
        colors[3] = Color.magenta;
        cube = Resources.Load<GameObject>("Prefabs/Block");

    }
    private void ConstructCubes()
    {
        if (currentLevel < BoxesEndLevel)
        {

            size = 3;
            cubes = new bool[size, size, size];
            switch (currentLevel)
            {
                // верх, право, перед

                case 0:
                    cubes[1, 1, 2] = true;
                    cubes[0, 1, 1] = true;
                    break;
                case 1:

                    cubes[1, 0, 0] = true;
                    cubes[1, 1, 1] = true;
                    cubes[0, 1, 1] = true;
                    cubes[0, 1, 0] = true;
                    break;
                case 2:

                    cubes[1, 0, 0] = true;
                    cubes[1, 1, 1] = true;
                    cubes[0, 1, 1] = true;
                    cubes[2, 1, 2] = true;
                    cubes[1, 0, 0] = true;
                    break;
                case 3:

                    cubes[1, 0, 0] = true;
                    cubes[1, 1, 1] = true;
                    cubes[0, 1, 1] = true;
                    cubes[2, 1, 2] = true;
                    cubes[1, 0, 0] = true;
                    cubes[2, 2, 2] = true;
                    break;
                case 4:

                    cubes[1, 0, 0] = true;
                    cubes[1, 1, 1] = true;
                    cubes[0, 1, 1] = true;
                    cubes[2, 1, 2] = true;
                    cubes[2, 0, 0] = true;
                    break;
                case 5:

                    cubes[1, 0, 0] = true;
                    cubes[1, 1, 1] = true;
                    cubes[0, 1, 1] = true;
                    cubes[2, 1, 2] = true;
                    cubes[1, 1, 0] = true;
                    cubes[1, 0, 2] = true;
                    cubes[1, 2, 0] = true;
                    break;
                case 6:

                    cubes[1, 0, 0] = true;
                    cubes[1, 1, 1] = true;
                    cubes[0, 1, 1] = true;
                    cubes[2, 1, 2] = true;
                    cubes[1, 1, 0] = true;
                    cubes[1, 0, 2] = true;
                    cubes[1, 2, 0] = true;
                    cubes[1, 2, 2] = true;
                    break;
                case 7:

                    cubes[1, 0, 0] = true;
                    cubes[1, 1, 1] = true;
                    cubes[0, 1, 1] = true;
                    cubes[2, 1, 2] = true;
                    cubes[1, 1, 0] = true;
                    cubes[1, 0, 2] = true;
                    cubes[1, 2, 0] = true;
                    cubes[1, 2, 2] = true;
                    cubes[1, 2, 1] = true;
                    break;
                case 8:

                    cubes[1, 0, 0] = true;
                    cubes[1, 1, 1] = true;
                    cubes[0, 1, 1] = true;
                    cubes[2, 1, 2] = true;
                    cubes[1, 1, 0] = true;
                    cubes[1, 0, 2] = true;
                    cubes[1, 2, 0] = true;
                    cubes[1, 2, 2] = true;
                    cubes[0, 0, 0] = true;
                    break;
                case 9:

                    cubes[1, 0, 0] = true;
                    cubes[1, 1, 1] = true;
                    cubes[0, 1, 1] = true;
                    cubes[2, 1, 2] = true;
                    cubes[1, 1, 0] = true;
                    cubes[1, 0, 2] = true;
                    cubes[1, 2, 0] = true;
                    cubes[1, 2, 2] = true;
                    cubes[0, 0, 0] = true;
                    cubes[0, 1, 0] = true;
                    break;
            }

        }

    }

    public override IEnumerator CreateLevel(int level)
    {

        answerRecieved = false;
        currentLevel = level;

        ConstructCubes();
        yield return StartCoroutine(platform.AddCubesCoroutine(cubes, 20, 0.5f, colors));
       
        yield return StartCoroutine(CreateAnswersCoroutine());




    }

    public override IEnumerator RemoveLevel()
    {

        question.text = "";
        platform.breaked = true;
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


    private Dictionary<string, string> ConfigureQuestions()
    {
        Dictionary<string, string> materialQuestionDictionary = new Dictionary<string, string>();
        materialQuestionDictionary.Add("leftScreen", "Выберите вид слева");
        materialQuestionDictionary.Add("rightScreen", "Выберите вид справа");
        materialQuestionDictionary.Add("topScreen", "Выберите вид сверху");
        return materialQuestionDictionary;
    }
    private IEnumerator CreateAnswersCoroutine()
    {


        answers.Clear();
        float time = 0.0f;

        if (currentLevel < 4)
        {
            time = 15;
        }
        else if (currentLevel < 8)
        {
            time = 12;
        }
        else
        {
            time = 10;
        }


        List<KeyValuePair<string, string>> materialQuestion = ConfigureQuestions().ToList();
        int qCount = materialQuestion.Count;
        var randomIndex = UnityEngine.Random.Range(0, qCount);
        string rightAnswer = materialQuestion.Where(x => x.Key == materialQuestion[randomIndex].Key).FirstOrDefault().Key;
        question.color = Color.blue;
        question.fontSize = 25;
        question.text = materialQuestion.Where(x => x.Key == rightAnswer).FirstOrDefault().Value;
        while (materialQuestion.Count > 0)
        {

            var answer = Instantiate(answerPicExample);
            int rnd = UnityEngine.Random.Range(0, materialQuestion.Count);
            var material = materialQuestion[rnd].Key;
            if (material == rightAnswer)
            {
                answer.IsCorrect = true;
            }
            answer.Create(OnChoice, material);
            answers.Add(answer);
            materialQuestion.RemoveAt(rnd);
        }

        PlaceAnswers();
        StartTimer(time);
        yield break;
    }


    private void OnChoice(Answer answer)
    {
        bool completed = answer.IsCorrect;
        string data = completed ? $"Решено верно" : "Ошибка";
        SendStatistics(completed, data);
    }
}
