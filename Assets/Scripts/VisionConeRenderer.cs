using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionConeRenderer : MonoBehaviour
{
    [Header("Shape")]
    public float ViewDistance = 4f;
    [Range(0f, 360f)] public float Fov = 90f;
    public int Segments = 40;

    [Header("Colors")]
    public Color IdleColor = new Color(0f, 1f, 0f, 0.25f);
    public Color AlertColor = new Color(1f, 0f, 0f, 0.25f);

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
        int vertexCount = Segments + 2;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[Segments * 3];

        vertices[0] = Vector3.zero;

        float half = Fov * 0.5f;

        for (int i = 0; i <= Segments; i++)
        {
            float angle = Mathf.Lerp(-half, half, i / (float)Segments);
            float rad = angle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
            vertices[i + 1] = dir * ViewDistance;
        }

        int t = 0;
        for (int i = 0; i < Segments; i++)
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
            meshRenderer.material.color = alert ? AlertColor : IdleColor;
    }
}
