using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private List<Transform[]> cubeLayers = new List<Transform[]>();
    private Transform platform;
    [Header("Экземпляр кубика")]
    public GameObject BlockExample;
    private bool layerFallen = false;
    public bool breaked;
    private void AddCubes(bool[,,] blocks, float offsetTop)
    {
        int len = blocks.GetLength(0);
        float posX = 0, posY = 0, posZ = 0;
        Vector3[,,] result = new Vector3[len, len, len];
        Vector3 parentScale = platform.localScale;
        float size = parentScale.x;
        for (int i = 0; i < len; i++)
        {
            posY = (i + 0.5f) * (size) / len + offsetTop;
            var cubesOnLayer = new Transform[len * len];
            for (int j = 0; j < len; j++)
            {

                posX = (j + 0.5f) * (size) / len;
                for (int s = 0; s < len; s++)
                {
                    posZ = (s + 0.5f) * size / len;
                    //   Debug.Log($"i: {i},j: {j}, s: {s}.");
                    if (blocks[i, j, s])
                    {

                        result[i, j, s] = platform.position - 0.5f * new Vector3(size, 0, size) + new Vector3(posX, posY, posZ);
                        var cube = Instantiate(BlockExample, platform, true);
                        cube.transform.position = result[i, j, s];
                        float coef = (float)size / len;
                        cube.transform.localScale = coef * new Vector3(1.0f / parentScale.x, 1.0f / parentScale.y, 1.0f / parentScale.z);//  new Vector3(5.0f/len, 5.0f / len, 5.0f / len);

                        cubesOnLayer[j * 3 + s] = cube.transform;
                    }
                }
            }
            cubeLayers.Add(cubesOnLayer);
        }



    }
    public IEnumerator AddCubesCoroutine(bool[,,] blocks, float offsetTop, float layerAnimationTime, Color[] colors)
    {
        AddCubes(blocks, offsetTop);
        for (int i = 0; i < cubeLayers.Count; i++)
        {
            for (int j = 0; j < cubeLayers[i].Length; j++)
            {
                if (cubeLayers[i][j] != null)
                {

                    cubeLayers[i][j].GetComponent<MeshRenderer>().materials[0].SetColor("_AlbedoColor", colors[UnityEngine.Random.Range(0, colors.Length)]);
                }
            }
        }
        for (int i = 0; i < cubeLayers.Count; i++)
        {
            var layer = cubeLayers[i];
            if (layer != null && layer.Where(x => x != null).Any())
            {
                layerFallen = true;
                foreach (var cube in layer)
                {
                    if (cube != null)
                    {
                        StartCoroutine(MoveCoroutine(cube, layerAnimationTime, new Vector3(0, -offsetTop, 0)));
                    }
                }

                yield return new WaitWhile(() => layerFallen);
            }
        }
    }
    private IEnumerator MoveCoroutine(Transform obj, float time, Vector3 moveVector)
    {
        int steps = 100;
        Vector3 moveStep = moveVector / steps;
        float timeStep = time / steps;
        var startPos = obj.position;
        int counter = 0;
        while (true)
        {

            counter++;
            if (obj != null)
            {
                obj.position = startPos + moveStep * counter;

            }
            yield return new WaitForSeconds(timeStep);
            if (counter > steps)
            {
                break;
            }
        }
        if (obj != null)
        {

            obj.position = startPos + moveVector;
        }
        layerFallen = false;

    }
    public void DeleteCubes()
    {
        for (int i = 0; i < cubeLayers.Count; i++)
        {
            for (int j = 0; j < cubeLayers[i].Length; j++)
            {
                if (cubeLayers[i][j] != null)
                {
                    Destroy(cubeLayers[i][j].gameObject);
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        platform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (breaked)
        {

            StopAllCoroutines();
            DeleteCubes();
            breaked = false;
        }
    }
}
