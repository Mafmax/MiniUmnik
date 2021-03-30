using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cart : MonoBehaviour
{
    public static Texture[] allImages;
    public static Texture[] AllImages
    {
        get
        {
            if (allImages == null)
            {
                allImages = Resources.LoadAll<Texture>("Images/Memory").Where(x => Int32.TryParse(x.name, out var plug)).ToArray();
            }
            return allImages;
        }
    }
    private MeshRenderer meshRenderer;
    private MeshRenderer MeshRenderer
    {
        get
        {
            if (meshRenderer == null)
            {
                meshRenderer = GetRenderer();
            }
            return meshRenderer;
        }
        set
        {
            meshRenderer = value;
        }
    }

    public event Action<Cart> OnChoice;
    public string Name { get; set; }
    public bool IsOpen { get; set; }
    public bool IsMove { get; set; }

    // Start is called before the first frame update
    public void Create(string pictureName)
    {
        var picture = AllImages.Where(x => x.name.Equals(pictureName)).FirstOrDefault();
        Create(picture);
    }
    public void Create(Texture picture)
    {
        MeshRenderer.material.mainTexture = picture;

        Name = picture.name;
    }
    public IEnumerator Open(float time = 1.0f)
    {
        IsOpen = true;
        IsMove = true;
        float angleStep = 5f;
        float angle = 180f;
        for (float i = 0; i < angle; i += angleStep)
        {
            yield return new WaitForSeconds(time / angle*angleStep);
            if (this != null)
            {

            this.transform.Rotate(Vector3.up, angleStep);
            }
        }
        IsMove = false;
    }
    public IEnumerator Close(float time = 0.5f)
    {
        IsOpen = false;
        IsMove = true;
        float angleStep = 5f;
        float angle = 180f;
        for (float i = 0; i < angle; i += angleStep)
        {
            yield return new WaitForSeconds(time / angle * angleStep);
            if (this != null)
            {

            this.transform.Rotate(Vector3.up, -angleStep);
            }
        }
        IsMove = false;
    }
 
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<Bullet>(out var plug))
        {
            if (!IsOpen)
            {
                OnChoice?.Invoke(this);
            }
            Destroy(collision.gameObject);
        }
    }

    private MeshRenderer GetRenderer()
    {
        return GetComponentsInChildren<MeshRenderer>().Where(x => x.name.Equals("Front", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    }
    public Vector2 GetSize()
    {
        Vector3 scale = MeshRenderer.transform.localScale;
        return 10.0f * new Vector2(scale.x, scale.z);
    }

}
