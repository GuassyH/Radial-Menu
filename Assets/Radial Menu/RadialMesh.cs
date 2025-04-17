using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class RadialMesh : MonoBehaviour
{
    public float angleStride;
    public float angle;

    public float thickness;
    public float radius;
    
    public int resolution;

    private Vector3[] col_positions;
    private Vector3[] positions;
    private _vertex[] vertices;

    private List<int> indices;

    MeshFilter meshFilter;
    Mesh mesh;
    MeshRenderer meshRenderer;

    [HideInInspector] public Material material;


    struct _vertex{
        public Vector3 position;
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateMesh();   
    }


    public void CreateMesh(){

        meshRenderer = this.AddComponent<MeshRenderer>(); 
        meshFilter = this.AddComponent<MeshFilter>();

        mesh = new Mesh();
        mesh.name = this.name + " mesh";

        meshFilter.mesh = mesh;


        meshRenderer.sharedMaterial = material;

        CalculateVertices();
        CalculateIndices();
        

        mesh.vertices = positions;
        mesh.triangles = indices.ToArray();


        mesh.RecalculateBounds();
        PolygonCollider2D col = this.AddComponent<PolygonCollider2D>();

        Vector2[] points = new Vector2[positions.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            points[i] = new Vector2(col_positions[i].x, col_positions[i].y);
        }

        col.pathCount = 1;
        col.SetPath(0, points);

    }


    void CalculateVertices(){

        positions = new Vector3[resolution * 2];
        vertices = new _vertex[resolution * 2];

        List<Vector3> top = new List<Vector3>(resolution);
        List<Vector3> bottom = new List<Vector3>(resolution);

        for (int i = 0; i < resolution; i++)
        {
            float newAngle = angle - (angleStride / 2f) + (angleStride * i / (resolution - 1f));

            // Inner Vertex
            vertices[i*2] = new _vertex();
            vertices[i*2].position = new Vector3(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad), 0) * radius;
            
            // Outer Vertex
            vertices[(i*2) + 1] = new _vertex();
            vertices[(i*2) + 1].position = new Vector3(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad), 0) * (radius - thickness);         
        }


        for (int i = 0; i < vertices.Length; i++){
            if(i % 2 == 0){ top.Add(vertices[i].position);  }
            else{   bottom.Add(vertices[i].position);   }
        }

        top.Reverse();

        List<Vector3> linePoses = new List<Vector3>();
        for (int i = 0; i < top.Count; i++) {   linePoses.Add(top[i]);  }
        for (int i = 0; i < bottom.Count; i++)  {   linePoses.Add(bottom[i]);   }
        col_positions = linePoses.ToArray();

        bottom.Reverse();

        linePoses = new List<Vector3>();

        for (int i = 0; i < top.Count; i++) {   linePoses.Add(top[i]);  }
        for (int i = 0; i < bottom.Count; i++)  {   linePoses.Add(bottom[i]);   }
        


        for (int i = 0; i < vertices.Length; i++)
        {
            positions[i] = linePoses[i];
            
            GameObject vert2 = new GameObject();
            vert2.name = "Vert " + i;
            vert2.transform.SetParent(this.transform);
            vert2.transform.localPosition = positions[i];
        }

        mesh.vertices = positions;

    }

    void CalculateIndices(){
        indices = new List<int>();
        indices.Clear();

        for (int i = 0; i < (resolution - 1); i++)
        {
            indices.Add(i);
            indices.Add(i + 1);
            indices.Add(i + resolution);
            
            indices.Add(i + 1);
            indices.Add(i + resolution + 1);
            indices.Add(i + resolution);
    
        }
    }

}
