using Godot;
using System;
using System.Linq;

public partial class Terrain : MeshInstance3D {
	[Export]
	private int mapSize = 11;
	[Export]
	private float initialAmplitude = 20.0f;
	[Export]
	private int octaves = 6;
	[Export]
	private int lacunarity = 2;
	[Export]
	private float persistence = 0.5f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		GD.Randomize();

		var fnl = new FastNoiseLite();
		long fnlSeed = GD.Randi() - 2147483648; // 2^31
		fnl.SetSeed((int)fnlSeed);
		fnl.SetNoiseType(FastNoiseLite.NoiseTypeEnum.Simplex);

		var st = new SurfaceTool();

		// st.Begin(Mesh.PrimitiveType.Triangles);

		var vertices = new Vector3[mapSize * mapSize];
		// var colors = new Color[mapSize * mapSize];
		var indices = new int[(mapSize - 1) * (mapSize - 1) * 6];

		// Vertex generation
		// Initialize x and z coordinates for vertices first
		for (int z = 0; z < mapSize; z++) {
			for (int x = 0; x < mapSize; x++) {
				int i = mapSize * z + x;

				float xCoord = x - ((mapSize - 1) * 0.5f);
				float zCoord = z - ((mapSize - 1) * 0.5f);
				vertices[i] = new Vector3(xCoord, 0.0f, zCoord);
			}
		}
		// Fractal noise
		float amplitude = initialAmplitude;
		for (int octave = 0; octave < octaves; octave++) {
			for (int z = 0; z < mapSize; z++) {
				for (int x = 0; x < mapSize; x++)  {
					int i = mapSize * z + x;

					float xCoord = x - ((mapSize - 1) * 0.5f);
					float zCoord = z - ((mapSize - 1) * 0.5f);

					float xNoise = xCoord * Mathf.Pow(lacunarity, octave);
					float zNoise = zCoord * Mathf.Pow(lacunarity, octave);

					// float y = (fnl.GetNoise2D(xNoise, zNoise) + 0.5f) * 0.5f; // 0 - 1
					float y = fnl.GetNoise2D(xNoise, zNoise) + 0.5f; // 0 - 2
					y *= y;
					float yScaled = y * amplitude;

					vertices[i].Y += yScaled;
					// colors[i] = new Color(y, y, y);
				}
			}
			amplitude *= persistence;
		}
		// Clockwise-wound indices
		for (int z = 0; z < mapSize - 1; z++) {
			for (int x = 0; x < mapSize - 1; x++) {

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
		// arrays[(int)Mesh.ArrayType.Color] = colors;
		st.CreateFromArrays(arrays);

		st.GenerateNormals();

		// Consider using ArrayMesh directly and calculate normals using custom function
		var mesh = st.Commit((ArrayMesh)GetMesh());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	}
}
