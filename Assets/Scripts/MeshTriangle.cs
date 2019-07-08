using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTriangle
{
    List<Vector3> vertieces = new List<Vector3>();
    List<Vector3> normlas = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    int submeshIndex;

    public List <Vector3> Vertices
    {
        get { return vertieces; }
        set { vertieces = value; }
    }

    public List <Vector3> Normals
    {
        get { return normlas; }
        set { normlas = value; }
    }

    public List <Vector2> UVs
    {
        get { return uvs; }
        set { uvs = value; }
    }

    public int SubmeshIndex
    {
        get { return submeshIndex; }
    }

    public MeshTriangle (Vector3[] newVertices, Vector3[] newNormals, Vector2[] newUVs, int submeshIndex)
    {
        Clear();

        this.normlas.AddRange(newNormals);
        this.vertieces.AddRange(newVertices);
        this.uvs.AddRange(newUVs);
        this.submeshIndex = submeshIndex;
    }
    
    public void Clear ()
    {
        vertieces.Clear();
        normlas.Clear();
        uvs.Clear();

        submeshIndex = 0;
    }

}
