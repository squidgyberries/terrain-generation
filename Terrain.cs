using Godot;
using System;
using System.Linq;

public partial class Terrain : MeshInstance3D {
	[Export]
	private int mapSize = 11;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		GD.Randomize();
		var st = new SurfaceTool();

		// st.Begin(Mesh.PrimitiveType.Triangles);

		// var vertices = new System.Collections.Generic.List<Vector3>(mapSize * mapSize);
		var vertices = new Vector3[mapSize * mapSize];
		var colors = new Color[mapSize * mapSize];
		// var indices = new System.Collections.Generic.List<int>((mapSize - 1) * (mapSize - 1) * 6);
		var indices = new int[(mapSize - 1) * (mapSize - 1) * 6];

		// Vertices
		for (int z = 0; z < mapSize; z++) {
			for (int x = 0; x < mapSize; x++)  {
				float y = GD.Randf();

				// st.SetColor(new Color(y, y, y));
				// st.AddVertex(new Vector3(x - ((mapSize - 1) / 2.0f), y, z - ((mapSize - 1) / 2.0f)));

				int i = mapSize * z + x;
				vertices[i] = new Vector3(x - ((mapSize - 1) / 2.0f), y, z - ((mapSize - 1) / 2.0f));
				colors[i] = new Color(y, y, y);
			}
		}
		// Clockwise-wound indices
		for (int z = 0; z < mapSize - 1; z++) {
			for (int x = 0; x < mapSize - 1; x++) {
				// st.AddIndex(mapSize * z + x);
				// st.AddIndex(mapSize * z + x + 1);
				// st.AddIndex(mapSize * (z + 1) + x);

				// st.AddIndex(mapSize * z + x + 1);
				// st.AddIndex(mapSize * (z + 1) + x + 1);
				// st.AddIndex(mapSize * (z + 1) + x);

				int i = ((mapSize - 1) * z + x) * 6;
				
				indices[i] = mapSize * z + x;
				indices[i + 1] = mapSize * z + x + 1;
				indices[i + 2] = mapSize * (z + 1) + x;

				indices[i + 3] = mapSize * z + x + 1;
				indices[i + 4] = mapSize * (z + 1) + x + 1;
				indices[i + 5] = mapSize * (z + 1) + x;
			}
		}

		var arrays = new Godot.Collections.Array();
		arrays.Resize((int)Mesh.ArrayType.Max);
		arrays[(int)Mesh.ArrayType.Vertex] = vertices;
		arrays[(int)Mesh.ArrayType.Index] = indices;
		arrays[(int)Mesh.ArrayType.Color] = colors;
		st.CreateFromArrays(arrays);

		st.GenerateNormals();

		// Commit to a mesh.
		// Consider using ArrayMesh directly and calculate normals using custom function
		var mesh = st.Commit((ArrayMesh)GetMesh());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	}
}
