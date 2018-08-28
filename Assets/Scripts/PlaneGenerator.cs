using UnityEngine;

public static class PlaneGenerator
{
    public static Mesh Generate(int gridSize, float cellSize, string name ="NewMesh")
    {
        int verticesCount = (gridSize + 1) * (gridSize + 1);
        int trianglesCount = gridSize * gridSize * 2;

        var newVertices = new Vector3[verticesCount];
        var newUV = new Vector2[verticesCount];
        var newTriangles = new int[trianglesCount * 3];

        var normilizer = 1f / gridSize;

        for (int x = 0; x <= gridSize; x++)
        {
            for (int z = 0; z <= gridSize; z++)
            {
                int pos = ((gridSize + 1) * x) + z;

                newVertices[pos] = new Vector3(x * cellSize, 0, z * cellSize);
                newUV[pos] = new Vector2(x * normilizer, z * normilizer);
            }
        }

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                int pos = (gridSize * x) + z;

                int pos1 = ((gridSize + 1) * x) + z;

                newTriangles[pos * 6 + 0] = pos1;
                newTriangles[pos * 6 + 1] = pos1 + 1;
                newTriangles[pos * 6 + 2] = pos1 + gridSize + 1;

                newTriangles[pos * 6 + 3] = pos1 + 1;
                newTriangles[pos * 6 + 4] = pos1 + gridSize + 2;
                newTriangles[pos * 6 + 5] = pos1 + gridSize + 1;
            }
        }

        Mesh m = new Mesh
        {
            name = name,
            vertices = newVertices,
            uv = newUV,
            triangles = newTriangles
        };

        m.RecalculateNormals();

        return m;
    }
}
