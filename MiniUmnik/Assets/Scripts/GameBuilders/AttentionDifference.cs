using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttentionDifference : MonoBehaviour
{
    private bool IsOpened { get; set; }
    public event Action OnOpened;
    private Material Material { get; set; }

    public void Open()
    {
        if (!IsOpened)
        {
            SetColorAlpha(1);
            IsOpened = true;
            OnOpened.Invoke();
        }
    }

    private void OnMouseDown()
    {
        Open();
    }
    // Start is called before the first frame update
    void Start()
    {
        Material = this.GetComponent<MeshRenderer>().material;
        SetColorAlpha(0);
    }
    private void SetColorAlpha(float a)
    {

        Material.SetColor("_Color", new Color(Material.color.r, Material.color.g, Material.color.b, a));
    }
    // Update is called once per frame
    void Update()
    {

    }
}
