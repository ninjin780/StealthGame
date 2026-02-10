using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionConeRenderer : MonoBehaviour
{
    [Header("Shape")]
    public float viewDistance = 4f;
    [Range(0f, 360f)] public float fov = 90f;
    public int segments = 40;

    [Header("Colors")]
    public Color idleColor = new Color(0f, 1f, 0f, 0.25f);
    public Color alertColor = new Color(1f, 0f, 0f, 0.25f);

    private Mesh mesh;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        mesh = new Mesh { name = "VisionConeMesh" };
        GetComponent<MeshFilter>().mesh = mesh;
        meshRenderer = GetComponent<MeshRenderer>();
        DrawCone();
        SetAlert(false);
    }

    public void DrawCone()
    {
        int vertexCount = segments + 2;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;

        float half = fov * 0.5f;

        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Lerp(-half, half, i / (float)segments);
            float rad = angle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
            vertices[i + 1] = dir * viewDistance;
        }

        int t = 0;
        for (int i = 0; i < segments; i++)
        {
            triangles[t++] = 0;
            triangles[t++] = i + 1;
            triangles[t++] = i + 2;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
    }

    public void SetAlert(bool alert)
    {
        if (meshRenderer != null)
            meshRenderer.material.color = alert ? alertColor : idleColor;
    }
}
