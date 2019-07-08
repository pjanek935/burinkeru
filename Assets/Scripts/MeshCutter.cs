using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCutter
{
    public static bool CurrentlyCutting
    {
        get;
        private set;
    }

    public static Mesh OriginalMesh;

    public static void Cut (GameObject originalGameObject, Vector3 contactPoint, Vector3 direction, Material cutMaterial = null, bool fill = true, bool addRigidBody = false)
    {
        if (CurrentlyCutting)
        {
            return;
        }

        CurrentlyCutting = true;

        Plane plane = new Plane(originalGameObject.transform.InverseTransformDirection (-direction),
            originalGameObject.transform.InverseTransformPoint (contactPoint));
        OriginalMesh = originalGameObject.GetComponent<MeshFilter>().mesh;
        List<Vector3> addedVertices = new List<Vector3>();

        GeneratedMesh leftMesh = new GeneratedMesh();
        GeneratedMesh rightMesh = new GeneratedMesh();

        int[] submeshIndices;
        int triangleIndexA, triangleIndexB, triangleIndexC;

        for (int i = 0; i < OriginalMesh.subMeshCount; i ++)
        {
            submeshIndices = OriginalMesh.GetTriangles(i);

            for (int j = 0; j < submeshIndices.Length; j += 3)
            {
                triangleIndexA = submeshIndices[j];
                triangleIndexB = submeshIndices[j + 1];
                triangleIndexC = submeshIndices[j + 2];

                MeshTriangle currentTriangle = getTriangle(triangleIndexA, triangleIndexB, triangleIndexC, i);

                bool triangleALeftSide = plane.GetSide(OriginalMesh.vertices[triangleIndexA]);
                bool triangleBLeftSide = plane.GetSide(OriginalMesh.vertices[triangleIndexB]);
                bool triangleCLeftSide = plane.GetSide(OriginalMesh.vertices[triangleIndexC]);

                if (triangleALeftSide && triangleBLeftSide && triangleCLeftSide)
                {
                    leftMesh.AddTriangle(currentTriangle);
                }
                else if (! triangleALeftSide && !triangleBLeftSide && !triangleCLeftSide)
                {
                    rightMesh.AddTriangle(currentTriangle);
                }
                else
                {
                    CutTriangle(plane, currentTriangle, triangleALeftSide, triangleBLeftSide, triangleCLeftSide, leftMesh, rightMesh, addedVertices);
                }
            }
        }

        

       

        GameObject leftGO = new GameObject();
        MeshFilter leftMeshFilter = leftGO.AddComponent<MeshFilter>();
        leftMeshFilter.mesh.SetVertices(leftMesh.Vertices);
        leftMeshFilter.mesh.SetNormals(leftMesh.Normals);
        leftMeshFilter.mesh.SetTriangles(leftMesh.GetTriangles (0), 0);
        leftGO.AddComponent<Rigidbody>();
        BoxCollider bx = leftGO.AddComponent<BoxCollider>();
        bx.size = originalGameObject.GetComponent<BoxCollider>().size;
        MeshRenderer mr = leftGO.AddComponent<MeshRenderer>();
        mr.material = originalGameObject.GetComponent<MeshRenderer>().material;
        leftGO.transform.position = originalGameObject.transform.position;
        leftGO.transform.localScale = originalGameObject.transform.localScale;

        GameObject rightGO = new GameObject();
        MeshFilter rightMeshFilter = rightGO.AddComponent<MeshFilter>();
        rightMeshFilter.mesh.SetVertices(rightMesh.Vertices);
        rightMeshFilter.mesh.SetNormals(rightMesh.Normals);
        rightMeshFilter.mesh.SetTriangles(rightMesh.GetTriangles (0), 0);
        mr = rightGO.AddComponent<MeshRenderer>();
        rightGO.AddComponent<Rigidbody>();
        bx = rightGO.AddComponent<BoxCollider>();
        bx.size = originalGameObject.GetComponent<BoxCollider>().size;
        mr.material = originalGameObject.GetComponent<MeshRenderer>().material;
        rightGO.transform.position = originalGameObject.transform.position;
        rightGO.transform.localScale = originalGameObject.transform.localScale;

        GameObject.Destroy(originalGameObject);
    }

    static MeshTriangle getTriangle (int triangleIndexA, int triangleIndexB, int triangleIndexC, int submeshIndex)
    {
        Vector3[] vertices = OriginalMesh.vertices;
        Vector3[] tVertices = new Vector3[] { vertices[triangleIndexA], vertices[triangleIndexB], vertices[triangleIndexC] };
        Vector3[] normals = OriginalMesh.normals;
        Vector3 [] tNormals = new Vector3[] { normals[triangleIndexA], normals[triangleIndexB], normals[triangleIndexC] };
        Vector2[] uvs = OriginalMesh.uv;
        Vector2 [] tUVs = new Vector2[] { uvs[triangleIndexA], uvs[triangleIndexB], uvs[triangleIndexC] };

        return new MeshTriangle(tVertices, tNormals, tUVs, submeshIndex);
    }

    static void CutTriangle(Plane plane, MeshTriangle triangle, bool triangleALeftSide, bool triangleBLeftSide, bool triangleCLeftSide,
        GeneratedMesh leftMesh, GeneratedMesh rightMesh, List<Vector3> addedVertices)
    {
        List<bool> leftSide = new List<bool>();
        leftSide.Add(triangleALeftSide);
        leftSide.Add(triangleBLeftSide);
        leftSide.Add(triangleBLeftSide);

        MeshTriangle leftMeshTriangle = new MeshTriangle(new Vector3[2], new Vector3[2], new Vector2[2], triangle.SubmeshIndex);
        MeshTriangle rightMeshTriangle = new MeshTriangle(new Vector3[2], new Vector3[2], new Vector2[2], triangle.SubmeshIndex);

        bool left = false;
        bool right = false;

        for (int i = 0; i < 3; i++)
        {
            if (leftSide[i])
            {
                if (! left)
                {
                    left = true;

                    leftMeshTriangle.Vertices[0] = triangle.Vertices[i];
                    leftMeshTriangle.Vertices[1] = leftMeshTriangle.Vertices[0];

                    leftMeshTriangle.UVs[0] = triangle.UVs[i];
                    leftMeshTriangle.UVs[1] = leftMeshTriangle.UVs[0];

                    leftMeshTriangle.Normals[0] = triangle.Normals[i];
                    leftMeshTriangle.Normals[1] = leftMeshTriangle.Normals[0];
                }
                else
                {
                    leftMeshTriangle.Vertices[1] = triangle.Vertices[i];
                    leftMeshTriangle.Normals[1] = triangle.Normals[i];
                    leftMeshTriangle.UVs[1] = triangle.UVs[i];
                }
            }
            else
            {
                if (!right)
                {
                    right = true;

                    rightMeshTriangle.Vertices[0] = triangle.Vertices[i];
                    rightMeshTriangle.Vertices[1] = rightMeshTriangle.Vertices[0];

                    rightMeshTriangle.UVs[0] = triangle.UVs[i];
                    rightMeshTriangle.UVs[1] = rightMeshTriangle.UVs[0];

                    rightMeshTriangle.Normals[0] = triangle.Normals[i];
                    rightMeshTriangle.Normals[1] = rightMeshTriangle.Normals[0];
                }
                else
                {
                    rightMeshTriangle.Vertices[1] = triangle.Vertices[i];
                    rightMeshTriangle.Normals[1] = triangle.Normals[i];
                    rightMeshTriangle.UVs[1] = triangle.UVs[i];
                }
            }
        }

        float normalizedDistance;
        float distance;
        plane.Raycast(new Ray(leftMeshTriangle.Vertices[0], (rightMeshTriangle.Vertices[0] - leftMeshTriangle.Vertices[0]).normalized), out distance);

        normalizedDistance = distance / (rightMeshTriangle.Vertices[0] - leftMeshTriangle.Vertices[0]).magnitude;
        Vector3 vertLeft = Vector3.Lerp(leftMeshTriangle.Vertices[0], rightMeshTriangle.Vertices[0], normalizedDistance);
        addedVertices.Add(vertLeft);

        Vector3 normalLeft = Vector3.Lerp(leftMeshTriangle.Normals[0], rightMeshTriangle.Normals[0], normalizedDistance);
        Vector2 uvLeft = Vector2.Lerp(leftMeshTriangle.UVs[0], rightMeshTriangle.UVs[0], normalizedDistance);

        plane.Raycast(new Ray(leftMeshTriangle.Vertices[1], (rightMeshTriangle.Vertices[1] - leftMeshTriangle.Vertices[1]).normalized), out distance);

        normalizedDistance = distance / (rightMeshTriangle.Vertices[1] - leftMeshTriangle.Vertices[1]).magnitude;
        Vector3 vertRight = Vector3.Lerp(leftMeshTriangle.Vertices[1], rightMeshTriangle.Vertices[1], normalizedDistance);
        addedVertices.Add(vertRight);

        Vector3 normalRight = Vector3.Lerp(leftMeshTriangle.Normals[1], rightMeshTriangle.Normals[1], normalizedDistance);
        Vector2 uvRight = Vector2.Lerp(leftMeshTriangle.UVs[1], rightMeshTriangle.UVs[1], normalizedDistance);

        MeshTriangle currentTriangle;
        Vector3[] updatedVertices = new Vector3[] { leftMeshTriangle.Vertices[0], vertLeft, vertRight };
        Vector3[] updatedNormals = new Vector3[] { leftMeshTriangle.Normals[0], normalLeft, normalRight };
        Vector2[] updatedUVs = new Vector2[] { leftMeshTriangle.UVs[0], uvLeft, uvRight };

        currentTriangle = new MeshTriangle(updatedVertices, updatedNormals, updatedUVs, triangle.SubmeshIndex);

        if (updatedVertices[0] != updatedVertices[1] && updatedVertices[0] != updatedVertices[2])
        {
            if (Vector3.Dot(Vector3.Cross(updatedVertices[1] - updatedVertices[0], updatedVertices[2] - updatedVertices[0]), updatedNormals[0]) < 0)
            {
                FlipTriangle(currentTriangle);
            }

            leftMesh.AddTriangle(currentTriangle);
        }

        updatedVertices = new Vector3[] { leftMeshTriangle.Vertices[0], leftMeshTriangle.Vertices[1], vertRight };
        updatedNormals = new Vector3[] { leftMeshTriangle.Normals[0], leftMeshTriangle.Normals[1], normalRight };
        updatedUVs = new Vector2[] { leftMeshTriangle.UVs[0], leftMeshTriangle.UVs[1], uvRight };

        currentTriangle = new MeshTriangle(updatedVertices, updatedNormals, updatedUVs, triangle.SubmeshIndex);

        if (updatedVertices[0] != updatedVertices[1] && updatedVertices[0] != updatedVertices[2])
        {
            if (Vector3.Dot(Vector3.Cross(updatedVertices[1] - updatedVertices[0], updatedVertices[2] - updatedVertices[0]), updatedNormals[0]) < 0)
            {
                FlipTriangle(currentTriangle);
            }

            leftMesh.AddTriangle(currentTriangle);
        }

        updatedVertices = new Vector3[] { rightMeshTriangle.Vertices[0], vertLeft, vertRight };
        updatedNormals = new Vector3[] { rightMeshTriangle.Normals[0], normalLeft, normalRight };
        updatedUVs = new Vector2[] { rightMeshTriangle.UVs[0], uvLeft, uvRight };

        currentTriangle = new MeshTriangle(updatedVertices, updatedNormals, updatedUVs, triangle.SubmeshIndex);

        if (updatedVertices[0] != updatedVertices[1] && updatedVertices[0] != updatedVertices[2])
        {
            if (Vector3.Dot(Vector3.Cross(updatedVertices[1] - updatedVertices[0], updatedVertices[2] - updatedVertices[0]), updatedNormals[0]) < 0)
            {
               FlipTriangle(currentTriangle);
            }

            rightMesh.AddTriangle(currentTriangle);
        }

        updatedVertices = new Vector3[] { rightMeshTriangle.Vertices[0], rightMeshTriangle.Vertices [1], vertRight };
        updatedNormals = new Vector3[] { rightMeshTriangle.Normals[0], rightMeshTriangle.Normals [1], normalRight };
        updatedUVs = new Vector2[] { rightMeshTriangle.UVs[0], rightMeshTriangle.Normals [1], uvRight };

        currentTriangle = new MeshTriangle(updatedVertices, updatedNormals, updatedUVs, triangle.SubmeshIndex);

        if (updatedVertices[0] != updatedVertices[1] && updatedVertices[0] != updatedVertices[2])
        {
            if (Vector3.Dot(Vector3.Cross(updatedVertices[1] - updatedVertices[0], updatedVertices[2] - updatedVertices[0]), updatedNormals[0]) < 0)
            {
                FlipTriangle(currentTriangle);
            }

            rightMesh.AddTriangle(currentTriangle);
        }
    }

    static void FlipTriangle (MeshTriangle triangle)
    {
        Vector3 tmp = triangle.Vertices[0];
        triangle.Vertices[0] = triangle.Vertices[2];
        triangle.Vertices[2] = tmp;

        tmp = triangle.Normals[0];
        triangle.Normals[0] = triangle.Normals[2];
        triangle.Normals[2] = tmp;

        Vector2 uvTmp = triangle.UVs[0];
        triangle.UVs[0] = triangle.UVs[2];
        triangle.UVs[2] = uvTmp;
    }

    //public static void FillCut (List <Vector3> addedVertieces, Plane plane, GeneratedMesh leftMesh, GeneratedMesh rightMesh)
    //{
    //    List<Vector3> vertices = new List<Vector3>();
    //    List<Vector3> polygone = new List<Vector3>();

    //    for (int i = 0; i < addedVertieces.Count; i ++)
    //    {
    //        if (! vertices.Contains (addedVertieces [i]))
    //        {
    //            polygone.Clear();
    //            polygone.Add(addedVertieces [i]);
    //            polygone.Add(addedVertieces[i + 1]);

    //            vertices.Add(addedVertieces [i]);
    //            vertices.Add(addedVertieces [i + 1]);

    //            EvaluatePairs(addedVertieces, vertices, polygone);
    //                fil
    //        }
    //    }
    //}

    //public static void EvaluatePairs (List <Vector3> addedVertices, List <Vector3> vertices, List <Vector3> polygone)
    //{
    //    bool isDone = false;

    //    while (! isDone)
    //    {
    //        isDone = true;

    //        for (int i = 0; i < addedVertices.Count; i += 2)
    //        {
    //            if (addedVertices [i] == polygone [polygone.Count - 1] && ! vertices.Contains (addedVertices [i + 1]))
    //            {
    //                isDone = false;
    //                polygone.Add(addedVertices [i+1]);
    //                vertices.Add(addedVertices [i +1]);
    //            }
    //            else if (addedVertices [i + 1] == polygone [polygone.Count - 1] && ! vertices.Contains (addedVertices [i]))
    //            {
    //                isDone = false;
    //                polygone.Add(addedVertices [i]);
    //                vertices.Add(addedVertices [i]);
    //            }
    //        }
    //    }
    //}

    //public static void Fill (List <Vector3> _vertices, Plane plane, GeneratedMesh leftMesh, GeneratedMesh rightMesh)
    //{
    //    Vector3 centerPosition = Vector3.zero;

    //    for (int i = 0; i < _vertices.Count; i++)
    //    {
    //        centerPosition += _vertices[i];
    //    }

    //    centerPosition /= _vertices.Count;

    //    Vector3 up = new Vector3(plane.normal.x, plane.normal.y, plane.normal.z);

    //    Vector3 left = Vector3.Cross(plane.normal, plane.normal);

    //    Vector3 displacement = Vector3.zero;
    //    Vector2 uv1 = Vector2.zero;
    //    Vector2 uv2 = Vector2.zero;

    //    for (int i = 0; i < _vertices.Count; i++)
    //    {
    //        displacement = _vertices[i] - centerPosition;
    //        uv1 = new Vector2()
    //        {
    //            x = 0.5f + Vector3.Dot(displacement, left),
    //            y = 0.5f + Vector3.Dot(displacement, up)
    //        };

    //        displacement = _vertices[(i + 1) % _vertices.Count] - centerPosition;
    //        uv2 = new Vector2()
    //        {
    //            x = 0.5f + Vector3.Dot(displacement, left),
    //            y = 0.5f + Vector3.Dot(displacement, up)
    //        };

    //        Vector3[] vertices = new Vector3[] { _vertices[i], _vertices[(i + 1) % _vertices.Count], centerPosition };
    //        Vector3[] normals = new Vector3[] { -plane.normal, plane.normal, -plane.normal };
    //        Vector2[] uvs = new Vector2[] { uv1, uv2, new Vector2(0.5f, 0.5f) };

    //        MeshTriangle currentTriangle = new MeshTriangle(vertices, normals, uvs, OriginalMesh.subMeshCount + 1);

    //        if (Vector3.Dot (Vector3.Cross (vertices [1] = vertices [0], vertices [2] - vertices [0]), normals [0]) < 0)
    //        {
    //            Fliptraingle;
    //        }

    //        leftMesh.AddTriangle(currentTriangle);

    //        normals = new Vector3[] { plane.normal, plane.normal, plane.normal, plane.normal };
    //        currentTriangle = new MeshTriangle(vertices, normals, uvs, OriginalMesh.subMeshCount + 1);

    //        if (Vector3.Dot (Vector3.Cross (vertices [1] - vertices [0], vertices [2] - vertices [0]), normals [0]) < 0)
    //        {
    //            FlipTraingle;
    //        }

    //        rightMesh.AddTriangle(currentTriangle);
    //    }
    //}
}
