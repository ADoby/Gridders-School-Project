using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class VertexColor : MonoBehaviour {

    public Color color = Color.white;

	
	// Update is called once per frame
	void Update () {
       // Mesh mesh = GetComponent<MeshFilter>().mesh;
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        Vector3[] vertices = mesh.vertices;
        Color[] colors = new Color[vertices.Length];
        int i = 0;
        while (i < vertices.Length)
        {
           // colors[i] = Color.Lerp(Color.red, Color.green, vertices[i].y);
            colors[i] = color;
            i++;
        }
        mesh.colors = colors;
	}
}
