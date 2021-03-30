using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnswerPic : Answer
{
    private Transform screen;
    public  void Create(Action<Answer> answerCallback, Material screenMaterial)
    {
        base.Create(answerCallback);
        var MRenderer = screen.GetChild(0).GetComponent<MeshRenderer>();
        MRenderer.material = screenMaterial;
    }
    public void Create(Action<Answer> answerCallback,string screenMaterialName)
    {
        var material = Resources.Load<Material>("Materials/" + screenMaterialName);
        Create(answerCallback, material);
    }
    protected override void Awake()
    {
        base.Awake();
        screen = GetComponentsInChildren<Transform>().Where(x => x.name == "Screen").FirstOrDefault();

    }
    void Update()
    {
        screen.forward = player.position - answer.position;
    }
}

