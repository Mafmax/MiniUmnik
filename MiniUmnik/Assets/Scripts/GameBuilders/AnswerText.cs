using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnswerText : Answer
{
    private Transform text;
    public void Create(Action<Answer> callback, string text)
    {
        base.Create(callback);
        this.text.GetChild(0).GetComponent<TextMesh>().text = text;
    }
    protected override void Awake()
    {
        base.Awake();
        text = GetComponentsInChildren<Transform>().Where(x => x.name == "Text").FirstOrDefault();
    }
    void Update()
    {
        text.forward = player.position - answer.position;
    }
    public override string ToString()
    {
        return text.GetComponent<TextMesh>().text;
    }
}
