using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MemoryLevelDesigner : LevelDesigner
{
    private bool canSelect = true;
    private Cart CartExample;
    private Cart Opened { get; set; }
    private List<Cart> Carts { get; set; }
    public override IEnumerator RemoveLevel()
    {
        for (int i = 0; i < Carts.Count; i++)
        {
            if (Carts[i] != null)
            {

                Carts[i].StopAllCoroutines();
                Destroy(Carts[i].gameObject);
            }
        }
        yield return null;
    }
    protected override void Awake()
    {
        base.Awake();
        CartExample = Resources.Load<Cart>("Prefabs/Cart");
    }
    private void RemovePair(Cart first, Cart second)
    {
        Carts.Remove(first);
        Carts.Remove(second);
        Destroy(first.gameObject);
        Destroy(second.gameObject);
        if (Carts.Count == 0)
        {
            SendStatistics(true, "Отгадано вовремя");
        }

    }
    private IEnumerator SelectLogicCoroutine(Cart selection)
    {
        yield return StartCoroutine(selection.Open());
        if (Opened == null)
        {
            Opened = selection;

        }
        else
        {

            if (Opened.Name.Equals(selection.Name))
            {

                RemovePair(selection, Opened);
            }
            else
            {
                StartCoroutine(selection.Close());
                StartCoroutine(Opened.Close());
                yield return new WaitWhile(() => selection.IsMove || Opened.IsMove);

            }
            Opened = null;
        }
        canSelect = true;
    }

    private void SelectLogic(Cart selection)
    {
        if (!canSelect)
        {
            return;
        }
        canSelect = false;
        StartCoroutine(SelectLogicCoroutine(selection));
    }
    private IEnumerator ConfigureCarts()
    {
        Carts = new List<Cart>();
        Texture[,] topology;
        float time = 0.0f;
        if (currentLevel < 4)
        {
            time = 80;
            topology = new Texture[3, 2];
        }
        else if (currentLevel < 8)
        {
            topology = new Texture[4, 2];
            time = 100;
        }
        else
        {
            time = 140;
            topology = new Texture[4, 3];
        }
        FillTopology(topology);
        var offset = new Vector2(0.1f, 0.1f);
        var size = CartExample.GetSize() + offset;
        float posX;
        float posY;
        Vector2 topologySize = new Vector2(topology.GetLength(0), topology.GetLength(1));
        for (int i = 0; i < topologySize.x; i++)
        {
            posX = (i - topologySize.x / 2) * size.x;
            for (int j = 0; j < topologySize.y; j++)
            {
                posY = (j + 0.5f) * size.y + 1f;
                var cartPos = answerAnchor.position + new Vector3(posX, posY, 0);
                var cart = Instantiate(CartExample);
                cart.Create(topology[i, j]);
                cart.transform.position = cartPos;
                cart.OnChoice += SelectLogic;
                Carts.Add(cart);
            }
        }
        StartTimer(time);
        yield return null;
    }
    private void FillTopology(Texture[,] topology)
    {
        int allcarts = Cart.AllImages.Length;
        int needCarts = topology.Length / 2;
        Texture[] cartsTextures = new Texture[needCarts];

        for (int i = 0; i < needCarts; i++)
        {
            int rnd = 0;
            do
            {
                rnd = UnityEngine.Random.Range(0, Cart.AllImages.Length);
            } while (cartsTextures.Where(x => x != null && x.name.Equals(Cart.AllImages[rnd].name, StringComparison.OrdinalIgnoreCase)).Any());
            cartsTextures[i] = Cart.AllImages[rnd];
        }

        for (int i = 0; i < topology.GetLength(0); i++)
        {
            for (int j = 0; j < topology.GetLength(1); j++)
            {
                Texture cartTexture;
                do
                {
                    cartTexture = cartsTextures[UnityEngine.Random.Range(0, needCarts)];
                } while (FindCount(topology, cartTexture, (x, y) => x != null && y != null && x.name.Equals(y.name, StringComparison.OrdinalIgnoreCase)) > 1);
                topology[i, j] = cartTexture;

            }
        }
    }


    private int FindCount<T>(T[,] array, T match, Func<T, T, bool> matcher)
    {
        int count = 0;
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                if (matcher(match, array[i, j]))
                {
                    count++;
                }
            }
        }
        return count;
    }
    public override IEnumerator CreateLevel(int level)
    {
        currentLevel = level;
        answerRecieved = false;
        yield return StartCoroutine(ConfigureCarts());
    }

}
