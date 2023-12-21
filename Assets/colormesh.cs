using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colormesh : MonoBehaviour
{
    [System.Serializable]
    public struct Bound
    {
        public float top;
        public float right;
        public float bottom;
        public float left;
    }

    [Header("Settings")]
    public Bound bound;
    public int quality = 10;
    public float maxHeight = 0.6f;
    public float minHeight = 0f;

    private Vector3[] vertices;
    private Mesh mesh;
    public GameObject oppocolor;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        float range = (bound.right - bound.left) / quality;
        vertices = new Vector3[(quality + 1) * (quality + 1)];

        // generate vertices
        // top vertices

        for (int i = 0; i <= quality; i++)
        {
            for (int j = 0; j <= quality; j++)
            {
                vertices[i*(quality+1) + j] = new Vector3(bound.left + j * range, minHeight, bound.left + i * range);
                //Debug.Log(vertices[i * (quality + 1) + j]);
            }
                
        }
        // bottom vertices
        //{
        //    vertices[i + quality] = new Vector3(bound.left + (i * range), bound.top - maxHeight, 0);
        //}

        int[] tris = new int[quality * quality * 2 * 3];
        for (int i = 0; i < quality; i++)
        {
            for (int j = 0; j < quality; j++)
            {
                int vidx = (quality + 1) * i + j;
                tris[(i * quality + j) * 6] = vidx;
                tris[(i * quality + j) * 6 + 1] = vidx + quality + 1;
                tris[(i * quality + j) * 6 + 2] = vidx + quality + 2;
                //Debug.Log(tris[(i * quality + j) * 6]);
                //Debug.Log(tris[(i * quality + j) * 6 + 1]);
                //Debug.Log(tris[(i * quality + j) * 6 + 2]);
                tris[(i * quality + j) * 6 + 3] = vidx;
                tris[(i * quality + j) * 6 + 4] = vidx + quality + 2;
                tris[(i * quality + j) * 6 + 5] = vidx + 1;
                //Debug.Log(tris[(i * quality + j) * 6 + 3]);
                //Debug.Log(tris[(i * quality + j) * 6 + 4]);
                //Debug.Log(tris[(i * quality + j) * 6 + 5]);
            }
        }

        // generate mesh

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // set up mesh
        meshFilter.mesh = mesh;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int color(int centerx, int centery, float radius, int color)
    {
        for (int i = centery - (int)radius - 1; i <= centery + (int)radius + 1; i++)
        {
            if (i >= 0 && i < quality)
            {
                for (int j = centerx - (int)radius - 1; j <= centerx + (int)radius + 1; j++)
                {
                    if (j >= 0 && j < quality && (j - centerx) * (j - centerx) + (i - centery) * (i - centery) <= radius * radius)
                    {
                        //if (colorgrid[i][j] != color)
                        //{
                        //oppocolor.GetComponet<colormesh>().remove(centerx, centery, radius);
                        //}
                        //colorgrid[i][j] = color;
                        vertices[i * (quality + 1) + j].y = maxHeight;
                     }
                }
            }

        }
        return 0;
    }

    public void remove(int centerx, int centery, float radius)
    {
        for (int i = centery - (int)radius - 1; i <= centery + (int)radius + 1; i++)
        {
            if (i >= 0 && i < quality)
            {
                for (int j = centerx - (int)radius - 1; j <= centerx + (int)radius + 1; j++)
                {
                    if (j >= 0 && j < quality && (j - centerx) * (j - centerx) + (i - centery) * (i - centery) <= radius * radius)
                    {
                        vertices[i * (quality + 1) + j].y = minHeight;
                    }
                }
            }

        }
    }
}
