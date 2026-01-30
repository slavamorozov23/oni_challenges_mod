using System;
using System.Collections.Generic;
using ProcGen;
using TUNING;

// Token: 0x02000ED7 RID: 3799
public class BestFit
{
	// Token: 0x0600799A RID: 31130 RVA: 0x002EC17C File Offset: 0x002EA37C
	public static Vector2I BestFitWorlds(List<WorldPlacement> worldsToArrange, bool ignoreBestFitY = false)
	{
		List<BestFit.Rect> list = new List<BestFit.Rect>();
		Vector2I vector2I = default(Vector2I);
		List<WorldPlacement> list2 = new List<WorldPlacement>(worldsToArrange);
		list2.Sort((WorldPlacement a, WorldPlacement b) => b.height.CompareTo(a.height));
		int height = list2[0].height;
		foreach (WorldPlacement worldPlacement in list2)
		{
			Vector2I vector2I2 = default(Vector2I);
			while (!BestFit.UnoccupiedSpace(new BestFit.Rect(vector2I2.x, vector2I2.y, worldPlacement.width, worldPlacement.height), list))
			{
				if (ignoreBestFitY)
				{
					vector2I2.x++;
				}
				else if (vector2I2.y + worldPlacement.height >= height + 32)
				{
					vector2I2.y = 0;
					vector2I2.x++;
				}
				else
				{
					vector2I2.y++;
				}
			}
			vector2I.x = Math.Max(worldPlacement.width + vector2I2.x, vector2I.x);
			vector2I.y = Math.Max(worldPlacement.height + vector2I2.y, vector2I.y);
			list.Add(new BestFit.Rect(vector2I2.x, vector2I2.y, worldPlacement.width, worldPlacement.height));
			worldPlacement.SetPosition(vector2I2);
		}
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			vector2I.x += 136;
			vector2I.y = Math.Max(vector2I.y, 136);
		}
		return vector2I;
	}

	// Token: 0x0600799B RID: 31131 RVA: 0x002EC338 File Offset: 0x002EA538
	private static bool UnoccupiedSpace(BestFit.Rect RectA, List<BestFit.Rect> placed)
	{
		foreach (BestFit.Rect rect in placed)
		{
			if (RectA.X1 < rect.X2 && RectA.X2 > rect.X1 && RectA.Y1 < rect.Y2 && RectA.Y2 > rect.Y1)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600799C RID: 31132 RVA: 0x002EC3C8 File Offset: 0x002EA5C8
	public static Vector2I GetGridOffset(IList<WorldContainer> existingWorlds, Vector2I newWorldSize, out Vector2I newWorldOffset)
	{
		List<BestFit.Rect> list = new List<BestFit.Rect>();
		foreach (WorldContainer worldContainer in existingWorlds)
		{
			list.Add(new BestFit.Rect(worldContainer.WorldOffset.x, worldContainer.WorldOffset.y, worldContainer.WorldSize.x, worldContainer.WorldSize.y));
		}
		Vector2I result = new Vector2I(Grid.WidthInCells, 0);
		int widthInCells = Grid.WidthInCells;
		Vector2I vector2I = default(Vector2I);
		while (!BestFit.UnoccupiedSpace(new BestFit.Rect(vector2I.x, vector2I.y, newWorldSize.x, newWorldSize.y), list))
		{
			if (vector2I.x + newWorldSize.x >= widthInCells)
			{
				vector2I.x = 0;
				vector2I.y++;
			}
			else
			{
				vector2I.x++;
			}
		}
		Debug.Assert(vector2I.x + newWorldSize.x <= Grid.WidthInCells, "BestFit is trying to expand the grid width, this is unsupported and will break the SIM.");
		result.y = Math.Max(newWorldSize.y + vector2I.y, Grid.HeightInCells);
		newWorldOffset = vector2I;
		return result;
	}

	// Token: 0x0600799D RID: 31133 RVA: 0x002EC50C File Offset: 0x002EA70C
	public static int CountRocketInteriors(IList<WorldContainer> existingWorlds)
	{
		int num = 0;
		List<BestFit.Rect> list = new List<BestFit.Rect>();
		foreach (WorldContainer worldContainer in existingWorlds)
		{
			list.Add(new BestFit.Rect(worldContainer.WorldOffset.x, worldContainer.WorldOffset.y, worldContainer.WorldSize.x, worldContainer.WorldSize.y));
		}
		Vector2I rocket_INTERIOR_SIZE = ROCKETRY.ROCKET_INTERIOR_SIZE;
		Vector2I vector2I;
		while (BestFit.PlaceWorld(list, rocket_INTERIOR_SIZE, out vector2I))
		{
			num++;
			list.Add(new BestFit.Rect(vector2I.x, vector2I.y, rocket_INTERIOR_SIZE.x, rocket_INTERIOR_SIZE.y));
		}
		return num;
	}

	// Token: 0x0600799E RID: 31134 RVA: 0x002EC5D4 File Offset: 0x002EA7D4
	private static bool PlaceWorld(List<BestFit.Rect> placedWorlds, Vector2I newWorldSize, out Vector2I newWorldOffset)
	{
		Vector2I vector2I = new Vector2I(Grid.WidthInCells, 0);
		int widthInCells = Grid.WidthInCells;
		Vector2I vector2I2 = default(Vector2I);
		while (!BestFit.UnoccupiedSpace(new BestFit.Rect(vector2I2.x, vector2I2.y, newWorldSize.x, newWorldSize.y), placedWorlds))
		{
			if (vector2I2.x + newWorldSize.x >= widthInCells)
			{
				vector2I2.x = 0;
				vector2I2.y++;
			}
			else
			{
				vector2I2.x++;
			}
		}
		vector2I.y = Math.Max(newWorldSize.y + vector2I2.y, Grid.HeightInCells);
		newWorldOffset = vector2I2;
		return vector2I2.x + newWorldSize.x <= Grid.WidthInCells && vector2I2.y + newWorldSize.y <= Grid.HeightInCells;
	}

	// Token: 0x02002138 RID: 8504
	private struct Rect
	{
		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x0600BBB8 RID: 48056 RVA: 0x003FE1EB File Offset: 0x003FC3EB
		public int X1
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x0600BBB9 RID: 48057 RVA: 0x003FE1F3 File Offset: 0x003FC3F3
		public int X2
		{
			get
			{
				return this.x + this.width + 2;
			}
		}

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x0600BBBA RID: 48058 RVA: 0x003FE204 File Offset: 0x003FC404
		public int Y1
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x0600BBBB RID: 48059 RVA: 0x003FE20C File Offset: 0x003FC40C
		public int Y2
		{
			get
			{
				return this.y + this.height + 2;
			}
		}

		// Token: 0x0600BBBC RID: 48060 RVA: 0x003FE21D File Offset: 0x003FC41D
		public Rect(int x, int y, int width, int height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		// Token: 0x040098A7 RID: 39079
		private int x;

		// Token: 0x040098A8 RID: 39080
		private int y;

		// Token: 0x040098A9 RID: 39081
		private int width;

		// Token: 0x040098AA RID: 39082
		private int height;
	}
}
