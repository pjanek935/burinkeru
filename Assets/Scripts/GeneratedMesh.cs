using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedMesh
{
    List<Vector3> vertices = new List<Vector3>();
    List<Vector3> normals = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    List<List<int>> submeshIndices = new List<List<int>>();

    public List <Vector3> Vertices
    {
        get { return vertices; }
    }

    public List <Vector3> Normals
    {
        get { return normals; }
    }

    public List <Vector2> UVs
    {
        get { return uvs; }
    }

    public List <int> GetTriangles (int submeshIndex)
    {
        return submeshIndices[submeshIndex];
    }

    public void AddTriangle (MeshTriangle triangle)
    {
        int currentVerticesCount = vertices.Count;

        vertices.AddRange(triangle.Vertices);
        normals.AddRange(triangle.Normals);
        uvs.AddRange(triangle.UVs);

        if (submeshIndices.Count < triangle.SubmeshIndex + 1)
        {
            for (int i = submeshIndices.Count; i < triangle.SubmeshIndex + 1; i ++)
            {
                submeshIndices.Add(new List<int> ());
            }
        }

        for (int i = 0; i < 3; i ++)
        {
            submeshIndices[triangle.SubmeshIndex].Add(currentVerticesCount + i);
        }
    }
}
