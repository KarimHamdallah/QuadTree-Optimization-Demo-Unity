using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SpawnZone_Script : MonoBehaviour
{
    [Header("SpawnData")]
    [SerializeField] GameObject SpwanObject;
    [SerializeField] int SpawnCount = 10;
    [SerializeField] float SpawnYOffset = 0.0f;
    [SerializeField] float SpawnObjectMinScale = 0.1f;
    [SerializeField] float SpawnObjectMaxScale = 0.5f;

    public static List<Transform> SpwanObjectsList = new List<Transform>();

    private Bounds SpawnZoneBounds;

    void Awake()
    {
        SpawnZoneBounds = GetComponent<MeshRenderer>().bounds;
        SpwanObjects(SpawnCount);
    }

    private void SpwanObjects(int Count)
    {
        for (int i = 0; i < Count; i++)
        {
            float xpos = UnityEngine.Random.Range(-SpawnZoneBounds.extents.x, SpawnZoneBounds.extents.x);
            float zpos = UnityEngine.Random.Range(-SpawnZoneBounds.extents.z, SpawnZoneBounds.extents.z);
            float scale = UnityEngine.Random.Range(SpawnObjectMinScale, SpawnObjectMaxScale);

            var Obj = Instantiate(SpwanObject, new Vector3(xpos, this.transform.position.y + scale * 0.5f + SpawnYOffset, zpos), Quaternion.identity);
            Obj.transform.localScale = new Vector3(scale, scale, scale);
            Obj.transform.parent = this.transform;
            SpwanObjectsList.Add(Obj.transform);
        }
    }
}
