using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LogicLevelDesigner : LevelDesigner
{

    private static int BoxesEndLevel = 3;
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

                case 0:
                    cubes[1, 1, 2] = true;
                    cubes[0, 1, 1] = true;
                    break;
                case 1:

                    cubes[1, 0, 0] = true;
                    cubes[1, 1, 1] = true;
                    cubes[0, 1, 1] = true;
                    break;
                case 2:

                    cubes[1, 0, 0] = true;
                    cubes[1, 1, 1] = true;
                    cubes[0, 1, 1] = true;
                    cubes[2, 1, 2] = true;
                    break;
            }

        }

    }

    public override IEnumerator CreateLevel(int level)
    {
       
        answerRecieved = false;
        currentLevel = level;
        if (currentLevel < 3)
        {

            ConstructCubes();
            yield return StartCoroutine(platform.AddCubesCoroutine(cubes, 20, 0.5f, colors));
            yield return new WaitForSeconds(2);
            yield return StartCoroutine(CreateAnswersCoroutine());
           
        }

        Debug.Log("уровень начался");
        yield break;
    }

    public override IEnumerator RemoveLevel()
    {
        yield return new WaitForSeconds(2);
        question.text = "";
        platform.DeleteCubes();
        for (int i = 0; i < answers.Count; i++)
        {
            Destroy(answers[i].gameObject);
        }
        yield break;
    }

    public override IEnumerator WaitAnswer()
    {
        while (!answerRecieved)
        {
            yield return null;
        }
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

        if (currentLevel < 3)
        {

            List<KeyValuePair<string, string>> materialQuestion = ConfigureQuestions().ToList();
            int qCount = materialQuestion.Count;
            var randomIndex = UnityEngine.Random.Range(0, qCount);
            string rightAnswer = materialQuestion.Where(x => x.Key == materialQuestion[randomIndex].Key).FirstOrDefault().Key;
            question.color = Color.blue;
            question.fontSize = 25;
            question.text = materialQuestion.Where(x => x.Key == rightAnswer).FirstOrDefault().Value;
            while (materialQuestion.Count > 0)
            {

                var answer = Instantiate(answerExample);
                int rnd = UnityEngine.Random.Range(0, materialQuestion.Count);
                var material = materialQuestion[rnd].Key;
                if (material == rightAnswer)
                {
                    answer.IsCorrect = true;
                }
                answer.Create(OnChoice, material);
                float PI = 3.14159f;
                var angle = -PI / 2 + PI / qCount * (qCount - materialQuestion.Count + 0.5f);
                var r = 6.0f;
                var offset = new Vector3(-r * Mathf.Sin(angle), 0.5f, r * Mathf.Cos(angle));
                answer.transform.position = answerAnchor.position + offset;
                answer.transform.forward = -new Vector3(offset.x, 0, offset.z);
                answers.Add(answer);
                materialQuestion.RemoveAt(rnd);
            }


        }


        yield break;
    }

    private void OnChoice(Answer answer)
    {
        answerRecieved = true;
            question.fontSize= 50;
        if (answer.IsCorrect)
        {
            question.color = Color.green;
            question.text = "ПРАВИЛЬНО :)";
        }
        else
        {
            question.color = Color.red;
            question.text = "НЕ ПРАВИЛЬНО :(";

        }
        Debug.Log($"Ответ правильный: {answer.IsCorrect}");
    }
}
