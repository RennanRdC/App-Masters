using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour
{
	//Generate a mesh based on width and depth
	public void GenerateMesh(float width, float depth)
	{
		Vector3[] vertices = {
			new Vector3 (0, 0, 0),
			new Vector3 (width, 0, 0),
			new Vector3 (width, 1, 0),
			new Vector3 (0, 1, 0),
			new Vector3 (0, 1, depth),
			new Vector3 (width, 1, depth),
			new Vector3 (width, 0, depth),
			new Vector3 (0, 0, depth),
		};

		int[] triangles = {
			0, 2, 1, //face front
			0, 3, 2,
			2, 3, 4, //face top
			2, 4, 5,
			1, 2, 5, //face right
			1, 5, 6,
			0, 7, 4, //face left
			0, 4, 3,
			5, 4, 7, //face back
			5, 7, 6,
			0, 6, 7, //face bottom
			0, 1, 6
		};

		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.Optimize();
		mesh.RecalculateNormals();
	}
}