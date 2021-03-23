using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Answer : MonoBehaviour
{
    public bool IsCorrect = false;

    private event Action<Answer> OnChoice;

    private Transform screen;
    private Transform answer;

    private Transform player;

    public void Create(Action<Answer> answerCallback, Material screenMaterial)
    {

        OnChoice += answerCallback;
        var MRenderer = screen.GetChild(0).GetComponent<MeshRenderer>();
        MRenderer.material = screenMaterial;
    }
    public void Create(Action<Answer> answerCallback, string screenMaterialName)
    {
        var material = Resources.Load<Material>("Materials/" + screenMaterialName);
        Create(answerCallback, material);
    }
    // Start is called before the first frame update
    void Awake()
    {
        screen = GetComponentsInChildren<Transform>().Where(x => x.name == "Screen").FirstOrDefault();
        player = FindObjectOfType<PlayerController>().transform;
        answer = transform;
    }
    // Update is called once per frame
    void Update()
    {
        screen.forward = player.position - answer.position;
    }
    private void OnMouseDown()
    {
        SendAnswer();
    }
    private void SendAnswer()
    {
        OnChoice.Invoke(this);
        
    }
}
