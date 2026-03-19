using UnityEngine;

public class SpawnObjectHighlight_Script : MonoBehaviour
{
    [SerializeField] Material HighlightMat;

    private MeshRenderer Renderer;
    private Material OriginalMat;

    private void Awake()
    {
        Renderer = GetComponent<MeshRenderer>();
        OriginalMat = Renderer.material;
    }

    public void Highlight()
    {
        Renderer.material = HighlightMat;
    }

    public void DeHighlight()
    {
        Renderer.material = OriginalMat;
    }
}
