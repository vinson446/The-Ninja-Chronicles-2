using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObj : MonoBehaviour
{
    public float destroyTime = 6f;
    public Vector3 standardOffset;
    public int minX;
    public int maxX;
    public int minY;
    public int maxY;
    public int minZ;
    public int maxZ;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);

        if (gameObject.tag == "DamageText")
        {
            transform.localPosition += standardOffset;
            transform.localPosition += new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
        }
    }
}
