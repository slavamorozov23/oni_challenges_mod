using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AE8 RID: 2792
public static class LightGridManager
{
	// Token: 0x0600513A RID: 20794 RVA: 0x001D6990 File Offset: 0x001D4B90
	public static int ComputeFalloff(float fallOffRate, int cell, int originCell, global::LightShape lightShape, DiscreteShadowCaster.Direction lightDirection)
	{
		int num = originCell;
		if (lightShape == global::LightShape.Quad)
		{
			Vector2I vector2I = Grid.CellToXY(num);
			Vector2I vector2I2 = Grid.CellToXY(cell);
			switch (lightDirection)
			{
			case DiscreteShadowCaster.Direction.North:
			case DiscreteShadowCaster.Direction.South:
			{
				Vector2I vector2I3 = new Vector2I(vector2I2.x, vector2I.y);
				num = Grid.XYToCell(vector2I3.x, vector2I3.y);
				break;
			}
			case DiscreteShadowCaster.Direction.East:
			case DiscreteShadowCaster.Direction.West:
			{
				Vector2I vector2I3 = new Vector2I(vector2I.x, vector2I2.y);
				num = Grid.XYToCell(vector2I3.x, vector2I3.y);
				break;
			}
			}
		}
		return LightGridManager.CalculateFalloff(fallOffRate, cell, num);
	}

	// Token: 0x0600513B RID: 20795 RVA: 0x001D6A1E File Offset: 0x001D4C1E
	private static int CalculateFalloff(float falloffRate, int cell, int origin)
	{
		return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float)Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
	}

	// Token: 0x0600513C RID: 20796 RVA: 0x001D6A3B File Offset: 0x001D4C3B
	public static void Initialise()
	{
		LightGridManager.previewLux = new int[Grid.CellCount];
	}

	// Token: 0x0600513D RID: 20797 RVA: 0x001D6A4C File Offset: 0x001D4C4C
	public static void Shutdown()
	{
		LightGridManager.previewLux = null;
		LightGridManager.previewLightCells.Clear();
	}

	// Token: 0x0600513E RID: 20798 RVA: 0x001D6A60 File Offset: 0x001D4C60
	public static void DestroyPreview()
	{
		foreach (global::Tuple<int, int> tuple in LightGridManager.previewLightCells)
		{
			LightGridManager.previewLux[tuple.first] = 0;
		}
		LightGridManager.previewLightCells.Clear();
	}

	// Token: 0x0600513F RID: 20799 RVA: 0x001D6AC4 File Offset: 0x001D4CC4
	public static void CreatePreview(int origin_cell, float radius, global::LightShape shape, int lux)
	{
		LightGridManager.CreatePreview(origin_cell, radius, shape, lux, 0, DiscreteShadowCaster.Direction.South);
	}

	// Token: 0x06005140 RID: 20800 RVA: 0x001D6AD4 File Offset: 0x001D4CD4
	public static void CreatePreview(int origin_cell, float radius, global::LightShape shape, int lux, int width, DiscreteShadowCaster.Direction direction)
	{
		LightGridManager.previewLightCells.Clear();
		ListPool<int, LightGridManager.LightGridEmitter>.PooledList pooledList = ListPool<int, LightGridManager.LightGridEmitter>.Allocate();
		pooledList.Add(origin_cell);
		DiscreteShadowCaster.GetVisibleCells(origin_cell, pooledList, (int)radius, width, direction, shape, true);
		foreach (int num in pooledList)
		{
			if (Grid.IsValidCell(num))
			{
				int num2 = lux / LightGridManager.ComputeFalloff(0.5f, num, origin_cell, shape, direction);
				LightGridManager.previewLightCells.Add(new global::Tuple<int, int>(num, num2));
				LightGridManager.previewLux[num] = num2;
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x04003631 RID: 13873
	public const float DEFAULT_FALLOFF_RATE = 0.5f;

	// Token: 0x04003632 RID: 13874
	public static List<global::Tuple<int, int>> previewLightCells = new List<global::Tuple<int, int>>();

	// Token: 0x04003633 RID: 13875
	public static int[] previewLux;

	// Token: 0x02001C2A RID: 7210
	public class LightGridEmitter
	{
		// Token: 0x0600ACB5 RID: 44213 RVA: 0x003CD994 File Offset: 0x003CBB94
		public void UpdateLitCells()
		{
			DiscreteShadowCaster.GetVisibleCells(this.state.origin, this.litCells, (int)this.state.radius, this.state.width, this.state.direction, this.state.shape, true);
		}

		// Token: 0x0600ACB6 RID: 44214 RVA: 0x003CD9E8 File Offset: 0x003CBBE8
		public void AddToGrid(bool update_lit_cells)
		{
			DebugUtil.DevAssert(!update_lit_cells || this.litCells.Count == 0, "adding an already added emitter", null);
			if (update_lit_cells)
			{
				this.UpdateLitCells();
			}
			foreach (int num in this.litCells)
			{
				if (Grid.IsValidCell(num))
				{
					int num2 = Mathf.Max(0, Grid.LightCount[num] + this.ComputeLux(num));
					Grid.LightCount[num] = num2;
					LightGridManager.previewLux[num] = num2;
				}
			}
		}

		// Token: 0x0600ACB7 RID: 44215 RVA: 0x003CDA8C File Offset: 0x003CBC8C
		public void RemoveFromGrid()
		{
			foreach (int num in this.litCells)
			{
				if (Grid.IsValidCell(num))
				{
					Grid.LightCount[num] = Mathf.Max(0, Grid.LightCount[num] - this.ComputeLux(num));
					LightGridManager.previewLux[num] = 0;
				}
			}
			this.litCells.Clear();
		}

		// Token: 0x0600ACB8 RID: 44216 RVA: 0x003CDB10 File Offset: 0x003CBD10
		public bool Refresh(LightGridManager.LightGridEmitter.State state, bool force = false)
		{
			if (!force && EqualityComparer<LightGridManager.LightGridEmitter.State>.Default.Equals(this.state, state))
			{
				return false;
			}
			this.RemoveFromGrid();
			this.state = state;
			this.AddToGrid(true);
			return true;
		}

		// Token: 0x0600ACB9 RID: 44217 RVA: 0x003CDB3F File Offset: 0x003CBD3F
		private int ComputeLux(int cell)
		{
			return this.state.intensity / this.ComputeFalloff(cell);
		}

		// Token: 0x0600ACBA RID: 44218 RVA: 0x003CDB54 File Offset: 0x003CBD54
		private int ComputeFalloff(int cell)
		{
			return LightGridManager.ComputeFalloff(this.state.falloffRate, cell, this.state.origin, this.state.shape, this.state.direction);
		}

		// Token: 0x04008713 RID: 34579
		private LightGridManager.LightGridEmitter.State state = LightGridManager.LightGridEmitter.State.DEFAULT;

		// Token: 0x04008714 RID: 34580
		private List<int> litCells = new List<int>();

		// Token: 0x02002A17 RID: 10775
		[Serializable]
		public struct State : IEquatable<LightGridManager.LightGridEmitter.State>
		{
			// Token: 0x0600D382 RID: 54146 RVA: 0x0043A8F4 File Offset: 0x00438AF4
			public bool Equals(LightGridManager.LightGridEmitter.State rhs)
			{
				return this.origin == rhs.origin && this.shape == rhs.shape && this.radius == rhs.radius && this.intensity == rhs.intensity && this.falloffRate == rhs.falloffRate && this.colour == rhs.colour && this.width == rhs.width && this.direction == rhs.direction;
			}

			// Token: 0x0400BA16 RID: 47638
			public int origin;

			// Token: 0x0400BA17 RID: 47639
			public global::LightShape shape;

			// Token: 0x0400BA18 RID: 47640
			public int width;

			// Token: 0x0400BA19 RID: 47641
			public DiscreteShadowCaster.Direction direction;

			// Token: 0x0400BA1A RID: 47642
			public float radius;

			// Token: 0x0400BA1B RID: 47643
			public int intensity;

			// Token: 0x0400BA1C RID: 47644
			public float falloffRate;

			// Token: 0x0400BA1D RID: 47645
			public Color colour;

			// Token: 0x0400BA1E RID: 47646
			public static readonly LightGridManager.LightGridEmitter.State DEFAULT = new LightGridManager.LightGridEmitter.State
			{
				origin = Grid.InvalidCell,
				shape = global::LightShape.Circle,
				radius = 4f,
				intensity = 1,
				falloffRate = 0.5f,
				colour = Color.white,
				direction = DiscreteShadowCaster.Direction.South,
				width = 4
			};
		}
	}
}
