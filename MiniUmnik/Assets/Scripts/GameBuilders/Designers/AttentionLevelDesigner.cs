using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttentionLevelDesigner : LevelDesigner
{
    private Transform[] Pairs { get; set; }
    private AttentionPair CurrentPair { get; set; }
    protected override void Awake()
    {
        base.Awake();
        Pairs = new Transform[10];
        var pairsPrefabs = Resources.LoadAll<GameObject>("Prefabs/AttentionPairs");
        for (int i = 0; i < Pairs.Length; i++)
        {
            Pairs[i] = pairsPrefabs.Where(x => x.name.Contains($"({i + 1})")).Select(x => x.transform).FirstOrDefault();
        }
    }
    public IEnumerator CreatePictures()
    {
        CurrentPair = Instantiate(Pairs[currentLevel]).GetComponent<AttentionPair>();
        CurrentPair.OnCompleted += () => answerRecieved = true;
        CurrentPair.OnAnswered += OnAnswered;
        CurrentPair.transform.position = answerAnchor.position + new Vector3(0,5.5f,5);
        yield return null;
    }
    
    public override IEnumerator CreateLevel(int level)
    {
        answerRecieved = false;
        currentLevel = level;

        yield return StartCoroutine(CreatePictures());
        OnAnswered();
    }
    public override IEnumerator RemoveLevel()
    {
        yield return StartCoroutine(RemovePictures());
    }
    private IEnumerator RemovePictures()
    {
        Destroy(CurrentPair.gameObject);
        yield return null;
    }
    private void OnAnswered()
    {
        var str = $"Найдите все отличия. Осталось: {10-CurrentPair.CurrentAnswered}";
        question.color = Color.green;
        question.text = str;
    }
}
