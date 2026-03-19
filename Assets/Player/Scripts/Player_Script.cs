using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Script : MonoBehaviour
{
    [SerializeField] InputActionAsset InputActions;
    [SerializeField] float SpeedForce = 30.0f;
    [SerializeField] Vector3 AABBCenter = Vector3.zero;
    [SerializeField] Vector3 AABBExtent = Vector3.one;
    [SerializeField] bool UseQuadTreeOptimization = false;

    private GameObject NearestObject = null;
    private InputAction MoveAction = null;
    private Rigidbody rg;

    public static event Action<int> OnDistanceCountChanged;
    private int CheckDistanceCount = 0;

    private void Awake()
    {
        MoveAction = InputActions.FindAction("Move");
        rg = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Move
        var InputDirectionVect = new Vector3(MoveAction.ReadValue<Vector2>().x, 0.0f, MoveAction.ReadValue<Vector2>().y);
        InputDirectionVect = Vector3.ClampMagnitude(InputDirectionVect, 1.0f);
        rg.AddForce(InputDirectionVect * SpeedForce);
    }
    void Update()
    {
        AABBCenter = transform.position;

        // highligh Nearset Object

        if (!UseQuadTreeOptimization)
        {
            float MinDistance = CheckMinDistance(SpawnZone_Script.SpwanObjectsList, out GameObject NewNearestObject);
            if (NewNearestObject != null &&
                (NearestObject == null || NearestObject.GetEntityId() != NewNearestObject.GetEntityId()))
            {
                NearestObject?.GetComponent<SpawnObjectHighlight_Script>().DeHighlight();
                NearestObject = NewNearestObject;
                NearestObject?.GetComponent<SpawnObjectHighlight_Script>().Highlight();
            }
        }
        else
        {
            float MinDistance = CheckMinDistanceOptimized(out GameObject NewNearestObject);
            if (NewNearestObject != null &&
                (NearestObject == null || NearestObject.GetEntityId() != NewNearestObject.GetEntityId()))
            {
                NearestObject?.GetComponent<SpawnObjectHighlight_Script>().DeHighlight();
                NearestObject = NewNearestObject;
                NearestObject?.GetComponent<SpawnObjectHighlight_Script>().Highlight();
            }
        }

        OnDistanceCountChanged?.Invoke(CheckDistanceCount);
    }

    private float CheckDistance(Transform Other)
    {
        return Vector3.Distance(this.transform.position, Other.position);
    }

    private float CheckMinDistance(List<Transform> Objects, out GameObject NearestObject)
    {
        CheckDistanceCount = 0;
        NearestObject = null;
        float MinDistance = float.PositiveInfinity;
        foreach (Transform Object in Objects)
        {
            float Dist = CheckDistance(Object);
            if (Dist < MinDistance)
            {
                MinDistance = Dist;
                NearestObject = Object.gameObject;
            }

            CheckDistanceCount++;
        }

        return MinDistance;
    }

    private float CheckMinDistanceOptimized(out GameObject NearestObject)
    {
        CheckDistanceCount = 0;
        NearestObject = null;
        float MinDistance = float.PositiveInfinity;

        List<Transform> Objects = new List<Transform>();
        QuadTree_Script.Tree.Query(AABBCenter, AABBExtent, Objects);

        foreach (Transform Object in Objects)
        {
            float Dist = CheckDistance(Object);
            if (Dist < MinDistance)
            {
                MinDistance = Dist;
                NearestObject = Object.gameObject;
            }

            CheckDistanceCount++;
        }

        return MinDistance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(AABBCenter, new Vector3(AABBExtent.x * 2.0f, AABBExtent.y, AABBExtent.z * 2.0f));
    }

    public void SetUseQuadTreeOptimization(bool value) { UseQuadTreeOptimization = value; }
}
