using UnityEngine;

public class QuadTree_Script : MonoBehaviour
{
    public Vector3 Center = Vector3.zero;
    public Vector3 Extent = Vector3.one;
    public int Capacity = 5;

    public static QuadTree Tree = new QuadTree();

    private void Start()
    {
        Tree.Center = Center;
        Tree.Extent = Extent;
        Tree.Capacity = Capacity;

        if (SpawnZone_Script.SpwanObjectsList.Count > 0)
        {
            foreach (Transform obj in SpawnZone_Script.SpwanObjectsList)
            {
                Tree.Insert(obj);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Tree != null)
        {
            Tree.DebugOnEditor();
        }
    }
}
