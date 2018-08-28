using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class OceanSimulation : MonoBehaviour
{
    [SerializeField] private int _gridSize = 10;
    [SerializeField] private float _cellSize = 1;
    [SerializeField] private List<PerlinNoiseLayer> _perlinNoiseLayers;

    private Mesh _mesh;

    private void Awake()
    {
        _mesh = PlaneGenerator.Generate(_gridSize, _cellSize);
        _mesh.MarkDynamic();

        GetComponent<MeshFilter>().mesh = _mesh;
    }

    private void Update()
    {
        var vertices = _mesh.vertices;

        FlattenVertices(vertices);

        ExecutePerlinNoiseJobs(vertices);

        _mesh.vertices = vertices;
        //_mesh.SetVertices(vertices.ToList());
        _mesh.RecalculateNormals();
    }

    private void FlattenVertices(Vector3[] vertices)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = 0;
        }
    }

    private void ExecutePerlinNoiseJobs(Vector3[] vertices)
    {
        var jobHandles = new List<JobHandle>();
        var vertexArray = new NativeArray<Vector3>(vertices, Allocator.TempJob);


        for (int i = 0; i < _perlinNoiseLayers.Count; i++)
        {
            var Job = new AddPerlinNoiseJob
            {
                Vertices = vertexArray,
                Layer = _perlinNoiseLayers[i],
                Time = Time.timeSinceLevelLoad
            };
            if (i == 0)
            {
                jobHandles.Add(Job.Schedule(vertices.Length, 250));
            }
            else
            {
                jobHandles.Add(Job.Schedule(vertices.Length, 250, jobHandles[i - 1]));
            }
        }

        jobHandles.Last().Complete();

        vertexArray.CopyTo(vertices);
        vertexArray.Dispose();
    }
}
