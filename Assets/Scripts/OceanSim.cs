using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class OceanSim : MonoBehaviour
{
    [SerializeField] private int _gridSize = 100;
    [SerializeField] private float _cellSize = 10;
    [SerializeField] private PerlinNoiseLayer _perlinNoiseLayer;
    [SerializeField] private Material _material;

    private Mesh _mesh;
    private Vector3[] _vertices;

    private void Awake()
    {
        _mesh = PlaneGenerator.Generate(_gridSize, _cellSize, "OceanMesh");
        _mesh.MarkDynamic();
        _vertices = _mesh.vertices;
    }

    private void Update()
    {
        var jobHandle = new JobHandle();
        var vertexArray = new NativeArray<Vector3>(_vertices, Allocator.TempJob);

        var Job = new SetPerlinNoiseJob
        {
            Vertices = vertexArray,
            Layer = _perlinNoiseLayer,
            Time = Time.timeSinceLevelLoad
        };

        jobHandle = Job.Schedule(vertexArray.Length, 250);
        jobHandle.Complete();

        vertexArray.CopyTo(_vertices);
        vertexArray.Dispose();

        _mesh.vertices = _vertices;

        _mesh.RecalculateNormals();

        Graphics.DrawMesh(_mesh, Vector3.zero, Quaternion.identity, _material, 0);
    }
}
