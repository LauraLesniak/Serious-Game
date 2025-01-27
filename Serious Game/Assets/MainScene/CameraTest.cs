using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TrapezoidMesh : MonoBehaviour
{
    [SerializeField] private Transform corner0Transform;
    [SerializeField] private Transform corner1Transform;
    [SerializeField] private Transform corner2Transform;
    [SerializeField] private Transform corner3Transform;

    private MeshFilter _meshFilter;

    private void OnValidate()
    {
        UpdateMesh();
    }

    private void Update()
    {
        // If you want the mesh to update continuously in Editor while moving corners
        // you can call UpdateMesh() in Update() -- or you could rely on OnValidate() or OnDrawGizmos.
        if (!Application.isPlaying)
        {
            UpdateMesh();
        }
    }

    private void UpdateMesh()
    {
        if (corner0Transform == null || corner1Transform == null
            || corner2Transform == null || corner3Transform == null) return;

        if (_meshFilter == null)
            _meshFilter = GetComponent<MeshFilter>();

        var mesh = new Mesh();

        // Use local positions or world positions, depending on how you want the mesh aligned
        Vector3[] vertices = new Vector3[4]
        {
            transform.InverseTransformPoint(corner0Transform.position),
            transform.InverseTransformPoint(corner1Transform.position),
            transform.InverseTransformPoint(corner2Transform.position),
            transform.InverseTransformPoint(corner3Transform.position)
        };

        int[] triangles = new int[]
        {
            0, 2, 1,
            2, 3, 1
        };

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        _meshFilter.sharedMesh = mesh;
    }
}
