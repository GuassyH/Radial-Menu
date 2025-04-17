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
    [HideInInspector] public Color color;


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


        meshRenderer.sharedMaterial = new Material(material);
        meshRenderer.sharedMaterial.color = color;

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

        List<Vector3>  col_top = new List<Vector3>();
        List<Vector3>  col_bottom = new List<Vector3>();
        List<Vector3> col_points = new List<Vector3>();

        for (int i = 0; i < resolution; i++)
        {
            float newAngle = angle - (angleStride / 2f) + (angleStride * i / (resolution - 1f));

            // Top Vertex
            vertices[i] = new _vertex();
            vertices[i].position = new Vector3(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad), 0) * radius;
            
            // Bottom Vertex
            vertices[resolution + i] = new _vertex();
            vertices[resolution + i].position = new Vector3(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad), 0) * (radius - thickness);         
        }


        for (int i = 0; i < vertices.Length; i++){
            if(i < resolution){ col_top.Add(vertices[i].position);  }
            else{   col_bottom.Add(vertices[i].position);   }
        }

        col_top.Reverse();

        for (int i = 0; i < col_top.Count; i++) {   col_points.Add(col_top[i]);  }
        for (int i = 0; i < col_bottom.Count; i++)  {   col_points.Add(col_bottom[i]);   }

        col_positions = col_points.ToArray();

    

        for (int i = 0; i < vertices.Length; i++)
        {
            positions[i] = vertices[i].position;
            
            GameObject vert_obj = new GameObject();
            vert_obj.name = "Vert " + i;
            vert_obj.transform.SetParent(this.transform);
            vert_obj.transform.localPosition = positions[i];
        }

        mesh.vertices = positions;

    }



    void CalculateIndices(){
        indices = new List<int>();
        indices.Clear();

        for (int i = 0; i < (resolution - 1); i++)
        {
            indices.Add(i);
            indices.Add(i + resolution);
            indices.Add(i + 1);
            
            indices.Add(i + 1);
            indices.Add(i + resolution);
            indices.Add(i + resolution + 1);
    
        }
    }

}
