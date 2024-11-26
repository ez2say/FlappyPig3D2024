using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class WireColliderGenerator : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    [SerializeField] private float radius = 0.1f; // Радиус провода
    [SerializeField] private int segments = 8; // Количество сегментов для создания цилиндра

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();

        if (lineRenderer == null || meshFilter == null || meshCollider == null)
        {
            Debug.LogError("LineRenderer, MeshFilter или MeshCollider не найдены на объекте.");
            return;
        }

        UpdateCollider();
    }

    public void UpdateCollider()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[lineRenderer.positionCount * segments];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[(lineRenderer.positionCount - 1) * segments * 6];

        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);

        for (int i = 0; i < positions.Length - 1; i++)
        {
            Vector3 direction = (positions[i + 1] - positions[i]).normalized;
            Vector3 right = Vector3.Cross(direction, Vector3.up).normalized * radius;
            Vector3 up = Vector3.Cross(direction, right).normalized * radius;

            for (int j = 0; j < segments; j++)
            {
                float angle = j * Mathf.PI * 2 / segments;
                Vector3 vertex = positions[i] + right * Mathf.Cos(angle) + up * Mathf.Sin(angle);
                vertices[i * segments + j] = vertex;
                uvs[i * segments + j] = new Vector2(j / (float)segments, i / (float)(positions.Length - 1));
            }
        }

        int triangleIndex = 0;
        for (int i = 0; i < positions.Length - 1; i++)
        {
            for (int j = 0; j < segments; j++)
            {
                int current = i * segments + j;
                int next = i * segments + (j + 1) % segments;
                int nextSegment = (i + 1) * segments + j;
                int nextSegmentNext = (i + 1) * segments + (j + 1) % segments;

                triangles[triangleIndex++] = current;
                triangles[triangleIndex++] = nextSegment;
                triangles[triangleIndex++] = next;

                triangles[triangleIndex++] = next;
                triangles[triangleIndex++] = nextSegment;
                triangles[triangleIndex++] = nextSegmentNext;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }
}