using Godot;
using System;

public partial class Terrain : MeshInstance3D
{
	[Export]
	private int mapSize = 11;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Randomize();
		var st = new SurfaceTool();

		st.Begin(Mesh.PrimitiveType.Triangles);

		// Vertices
		for (int z = 0; z < mapSize; z++) {
			for (int x = 0; x < mapSize; x++)  {
				float y = GD.Randf();
				st.SetColor(new Color(y, y, y));
				st.AddVertex(new Vector3(x - ((mapSize - 1) / 2.0f), y, z - ((mapSize - 1) / 2.0f)));
			}
		}
		// Clockwise-wound indices
		for (int z = 0; z < mapSize - 1; z++) {
			for (int x = 0; x < mapSize - 1; x++) {
				st.AddIndex(mapSize * z + x);
				st.AddIndex(mapSize * z + x + 1);
				st.AddIndex(mapSize * (z + 1) + x);

				st.AddIndex(mapSize * z + x + 1);
				st.AddIndex(mapSize * (z + 1) + x + 1);
				st.AddIndex(mapSize * (z + 1) + x);
			}
		}

		st.GenerateNormals();

		// Commit to a mesh.
		var mesh = st.Commit((ArrayMesh)GetMesh());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
