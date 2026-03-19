using System.Collections.Generic;
using UnityEngine;

public enum QuadTreeNodeType
{
    ROOT = 0,
    LEAF
}

public class QuadTree
{
    public Vector3 Center = Vector3.zero;
    public Vector3 Extent = Vector3.one;
    public int Capacity = 20;

    private List<Transform> Objects = new List<Transform>();
    private bool Divided = false;
    private QuadTreeNodeType NodeType = QuadTreeNodeType.LEAF;
    private QuadTree NorthWest, NorthEast, SouthWest, SouthEast = null;

    public QuadTree() { }

    public void Insert(Transform Obj)
    {
        // 1st Case - not filled yet >> is object contained inside node region (T >> Push, F >> Get Away)

        bool Contained = Contains(Obj);
        if (!Contained)
            return;

        if (NodeType != QuadTreeNodeType.ROOT)
        {
            if (Objects.Count < Capacity)
            {
                Objects.Add(Obj);
                return;
            }
        }

        // 2nd Case - filled >> divide >> insert new object >> redistribution (LEAF NODE >> ROOT NODE)
        if (!Divided)
        {
            Subdevide();
            Objects.Add(Obj);
            Redistribute();
        }
        else
        {
            AddToSubDivision(Obj);
        }
    }

    private void Subdevide()
    {
        var HalfExtent = new Vector3(Extent.x * 0.5f, Extent.y, Extent.z * 0.5f);

        NorthEast = new QuadTree();
        NorthEast.Center = new Vector3(Center.x + Extent.x * 0.5f, Center.y, Center.z + Extent.z * 0.5f);
        NorthEast.Extent = HalfExtent;
        NorthEast.Capacity = Capacity;

        NorthWest = new QuadTree();
        NorthWest.Center = new Vector3(Center.x - Extent.x * 0.5f, Center.y, Center.z + Extent.z * 0.5f);
        NorthWest.Extent = HalfExtent;
        NorthWest.Capacity = Capacity;

        SouthEast = new QuadTree();
        SouthEast.Center = new Vector3(Center.x + Extent.x * 0.5f, Center.y, Center.z - Extent.z * 0.5f);
        SouthEast.Extent = HalfExtent;
        SouthEast.Capacity = Capacity;

        SouthWest = new QuadTree();
        SouthWest.Center = new Vector3(Center.x - Extent.x * 0.5f, Center.y, Center.z - Extent.z * 0.5f);
        SouthWest.Extent = HalfExtent;
        SouthWest.Capacity = Capacity;

        Divided = true;

        NodeType = QuadTreeNodeType.ROOT;
    }

    private void Redistribute()
    {
        for (int i = Objects.Count - 1; i >= 0; i--)
        {
            var obj = Objects[i];

            if (NorthEast.Contains(obj))
            {
                NorthEast.Insert(obj);
                Objects.Remove(obj);
                continue;
            }

            if (NorthWest.Contains(obj))
            {
                NorthWest.Insert(obj);
                Objects.Remove(obj);
                continue;
            }

            if (SouthEast.Contains(obj))
            {
                SouthEast.Insert(obj);
                Objects.Remove(obj);
                continue;
            }

            if (SouthWest.Contains(obj))
            {
                SouthWest.Insert(obj);
                Objects.Remove(obj);
                continue;
            }
        }
    }

    private void AddToSubDivision(Transform obj)
    {

        if (NorthEast.Contains(obj))
        {
            NorthEast.Insert(obj);
            return;
        }

        if (NorthWest.Contains(obj))
        {
            NorthWest.Insert(obj);
            return;
        }

        if (SouthEast.Contains(obj))
        {
            SouthEast.Insert(obj);
            return;
        }

        if (SouthWest.Contains(obj))
        {
            SouthWest.Insert(obj);
            return;
        }
    }

    public void Query(Vector3 RegionCenter, Vector3 RegionExtent, List<Transform> ObjectList)
    {
        // 1st - not contained region (Get Away)

        if (!IntersectsAABB(RegionCenter, RegionExtent))
        {
            return;
        }

        // 2nd contained
        if (NodeType == QuadTreeNodeType.LEAF)
        {
            // get all objects from this node
            foreach (var obj in Objects)
            {
                ObjectList.Add(obj);
            }
        }
        else
        {
            NorthEast.Query(RegionCenter, RegionExtent, ObjectList);
            NorthWest.Query(RegionCenter, RegionExtent, ObjectList);
            SouthEast.Query(RegionCenter, RegionExtent, ObjectList);
            SouthWest.Query(RegionCenter, RegionExtent, ObjectList);
        }
    }

    private bool Contains(Transform obj)
    {
        Vector3 pos = obj.position;

        return (pos.x >= Center.x - Extent.x &&
                pos.x <= Center.x + Extent.x &&
                pos.z >= Center.z - Extent.z &&
                pos.z <= Center.z + Extent.z);
    }

    public bool IntersectsAABB(Vector3 otherCenter, Vector3 otherExtent)
    {
        float dx = Mathf.Abs(Center.x - otherCenter.x);
        float dz = Mathf.Abs(Center.z - otherCenter.z);

        return (dx <= (Extent.x + otherExtent.x)) &&
               (dz <= (Extent.z + otherExtent.z));
    }


    public void DebugOnEditor()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Center, new Vector3(Extent.x * 2.0f, Extent.y, Extent.z * 2.0f));

        if (Divided)
        {
            NorthEast.DebugOnEditor();
            NorthWest.DebugOnEditor();
            SouthEast.DebugOnEditor();
            SouthWest.DebugOnEditor();
        }
    }
}
