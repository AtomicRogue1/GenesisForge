using Godot;
using System;

// In this code, 0 is a block and 1 is a free space.

public partial class MapGeneration : Node2D
{
	Random rand = new Random();
	TileMap levelTilemap;

	//Preferred Settings:
	// width=64 height=64 GeneratingPercent=30
	// width=32 height=32 GeneratingPercent=30
	// width=48 height=48 GeneratingPercent=30 (Best as of now)

	[ExportCategory("Level Gen Attributes")]
	[Export]
	float GeneratingPercent;
	[Export]
	int width = 64;
	[Export]
	int height = 64;
	[Export]
	int BoundaryThickness = 3;

	int[,] map;
	int[,] tempMapForComparison;

	public float randomSeed;

	[ExportCategory("For Smoothing Map")]
	int SmoothingParameter = 4;


	public override void _Ready()
	{
		levelTilemap = GetNode<TileMap>("TileMap");
		map = new int[width, height];
	}

	public void GenerateMap()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				randomSeed = rand.Next(0, 100);
				map[i, j] = GeneratingPercent > randomSeed ? 0 : 1; // if(randomNumber>GeneratingPercent) color(0,0,0) else color(1,1,1)
			}
		}
		GD.Print("Map generated.");
	}

	public void MakeBoundary()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if ((i < BoundaryThickness) || (i >= width - BoundaryThickness) || (j < BoundaryThickness) || (j >= height - BoundaryThickness))
				{
					map[i, j] = 0;
				}
			}
		}
		GD.Print("Boundary of map generated.");
	}

	bool compareMaps()
	{
		for (int i = 0; i < map.GetLength(0); i++)
		{
			for (int j = 0; j < map.GetLength(1); j++)
			{
				if (map[i, j] != tempMapForComparison[i, j])
					return false; // Found a mismatch.
			}
		}
		return true;
	}

	// Will test filters
	public void SmoothingFilter()
	{
		tempMapForComparison = map;

		do
		{
			tempMapForComparison = (int[,])map.Clone();
			for (int i = BoundaryThickness; i < width - BoundaryThickness; i++)
			{
				for (int j = BoundaryThickness; j < height - BoundaryThickness; j++)
				{
					if (map[i, j] == 1)
					{
						if (map[i - 1, j - 1] + map[i - 1, j] + map[i - 1, j + 1] + map[i, j - 1] + map[i, j + 1] + map[i + 1, j - 1] + map[i + 1, j] + map[i + 1, j + 1] < SmoothingParameter)
							map[i, j] = 0;
					}
				}
			}
		}
		while (!compareMaps());
	}
	// Experiment Results with Smoothing Filter:
	// Very bad by itself. Generates cave but leaves behind a lot of noise.
	// Very interesting results found at GeneratingPercentage 30%! Cave generated was noisy but looked good after applying median filter.
	// Try this by exhausting use of Smoothing Filter first, then exhaust Median Filter.

	public void MedianFilter()
	{
		tempMapForComparison = map;

		do
		{
			tempMapForComparison = (int[,])map.Clone();
			int[,] newMap = new int[width, height];

			for (int i = 1; i < width - 1; i++)
			{
				for (int j = 1; j < height - 1; j++)
				{
					int[] neighbors = {
				map[i - 1, j - 1], map[i - 1, j], map[i - 1, j + 1],
				map[i, j - 1], map[i, j], map[i, j + 1],
				map[i + 1, j - 1], map[i + 1, j], map[i + 1, j + 1]
			};

					Array.Sort(neighbors);
					newMap[i, j] = neighbors[4]; // Median value
				}
			}

			map = newMap;
		}
		while (!compareMaps());
	}
	// Experiment Results with Median Filter:
	// - Decent Map Generation.
	// - Optimal Generating Percentage was found to be at 47%
	// - Great for removing black dots / noise

	public void PutTilesOutsideLevel()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if ((i < BoundaryThickness) || (i >= width - BoundaryThickness) || (j < BoundaryThickness) || (j >= height - BoundaryThickness))
				{
					levelTilemap.SetCell(0, new Vector2I(i, j), 0, new Vector2I(3, 3), 0);
				}
			}
		}
	}

	public void PutTiles()
	{
		// Putting relevant tiles inside boundary
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if ((i >= BoundaryThickness) || (i < width - BoundaryThickness) || (j >= BoundaryThickness) || (j < height - BoundaryThickness))
				{
					levelTilemap.SetCell(
					0,
					new Vector2I(i, j),
					map[i, j] == 0 ? 0 : -1,
					new Vector2I(3, 1), 0
					);
				}
			}
		}
	}

	public void PutDiagonalTiles()
	{
		int[,] tempMap = map;
		for (int i = BoundaryThickness; i < width - BoundaryThickness; i++)
		{
			for (int j = BoundaryThickness; j < height - BoundaryThickness; j++)
			{
				// For this tile
				// |\
				// |_\
				if (map[i - 1, j] == 0 &&
					map[i - 1, j + 1] == 0 &&
					map[i, j + 1] == 0 &&
					map[i, j - 1] == 1 &&
					map[i + 1, j - 1] == 1 &&
					map[i + 1, j] == 1 &&
					map[i, j] == 1 //This placing of tile will only happen if current tile is free. 
					)
				{
					levelTilemap.SetPattern(0, new Vector2I(i, j), levelTilemap.TileSet.GetPattern(1));
				}

				// For this tile
				// | /
				// |/
				else if (map[i - 1, j - 1] == 0 &&
					map[i, j - 1] == 0 &&
					map[i - 1, j] == 0 &&
					map[i + 1, j] == 1 &&
					map[i + 1, j + 1] == 1 &&
					map[i, j + 1] == 1 &&
					map[i, j] == 1)
				{
					levelTilemap.SetPattern(0, new Vector2I(i, j), levelTilemap.TileSet.GetPattern(3));
				}

				// For this tile
				// \ |
				//  \|
				else if (map[i, j - 1] == 0 &&
					map[i + 1, j - 1] == 0 &&
					map[i + 1, j] == 0 &&
					map[i - 1, j] == 1 &&
					map[i - 1, j + 1] == 1 &&
					map[i, j + 1] == 1 &&
					map[i, j] == 1)
				{
					levelTilemap.SetPattern(0, new Vector2I(i, j), levelTilemap.TileSet.GetPattern(2));
				}

				// For this tile
				//  /|
				// /_|
				else if (map[i + 1, j] == 0 &&
					map[i + 1, j + 1] == 0 &&
					map[i, j + 1] == 0 &&
					map[i - 1, j - 1] == 1 &&
					map[i, j - 1] == 1 &&
					map[i - 1, j] == 1 &&
					map[i, j] == 1)
				{
					levelTilemap.SetPattern(0, new Vector2I(i, j), levelTilemap.TileSet.GetPattern(0));
				}

			}
		}

		map = tempMap;
	}

	public void UpdateBitmapAfterPuttingTile()
	{
		for (int i = BoundaryThickness; i < width - BoundaryThickness; i++)
		{
			for (int j = BoundaryThickness; j < height - BoundaryThickness; j++)
			{
				if (levelTilemap.GetCellSourceId(0, new Vector2I(i, j)) != -1)
					map[i, j] = 0;
			}
		}
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustReleased("ui_accept"))
		{
			GenerateMap();
			MakeBoundary();

			PutTiles();
			PutTilesOutsideLevel();
			PutDiagonalTiles();

			SmoothingFilter();

			PutTiles();
			PutTilesOutsideLevel();
			PutDiagonalTiles();

			MedianFilter();

			PutTiles();
			PutTilesOutsideLevel();
			PutDiagonalTiles();

			if(rand.Next(0, 100)>=50)
			{
				UpdateBitmapAfterPuttingTile();
				MedianFilter();
				UpdateBitmapAfterPuttingTile();
				MedianFilter();
			}

		}
	}



	// 	public override void _Draw()
	// 	{
	// 		for (int i = 0; i < width; i++)
	// 		{
	// 			for (int j = 0; j < height; j++)
	// 			{
	// 				randomSeed = rand.Next(0, 100);
	// 				DrawRect(
	// 					new Rect2(new Vector2(i * 8, j * 8), new Vector2(8, 8)),
	// 					new Color(map[i, j], map[i, j], map[i, j]), // if(randomNumber>GeneratingPercent) color(0,0,0) else color(1,1,1)
	// 					true
	// 					);
	// 			}
	// 		}

	// 	}
}
