using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class OceanSim : MonoBehaviour
{
    [SerializeField] private int _gridSize = 100;
    [SerializeField] private float _cellSize = 10;
    [SerializeField] private PerlinNoiseLayer _perlinNoiseLayer;

    private Mesh _mesh;

    private void Awake()
    {
        _mesh = new PlaneGenerator().Generate(_gridSize, _cellSize);
        _mesh.MarkDynamic();

        GetComponent<MeshFilter>().mesh = _mesh;
    }

    private void Update()
    {
        var vertices = _mesh.vertices;

        Vector3[] vertices = new Vector3[_mesh.vertices.Length];

        var jobHandle = new JobHandle();
        var vertexArray = new NativeArray<Vector3>(vertices, Allocator.TempJob);

        var Job = new SetPerlinNoiseJob
        {
            Vertices = vertexArray,
            Layer = _perlinNoiseLayer,
            Time = Time.timeSinceLevelLoad
        };

        jobHandle = Job.Schedule(vertexArray.Length, 250);

        jobHandle.Complete();

        vertexArray.CopyTo(vertices);
        vertexArray.Dispose();

        _mesh.vertices = vertices;

        _mesh.RecalculateNormals();
    }
}
