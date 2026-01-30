using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AF0 RID: 2800
[Serializable]
public class RadiationGridEmitter
{
	// Token: 0x06005162 RID: 20834 RVA: 0x001D804C File Offset: 0x001D624C
	public RadiationGridEmitter(int originCell, int intensity)
	{
		this.originCell = originCell;
		this.intensity = intensity;
	}

	// Token: 0x06005163 RID: 20835 RVA: 0x001D809C File Offset: 0x001D629C
	public void Emit()
	{
		this.scanCells.Clear();
		Vector2 a = Grid.CellToPosCCC(this.originCell, Grid.SceneLayer.Building);
		for (float num = (float)this.direction - (float)this.angle / 2f; num < (float)this.direction + (float)this.angle / 2f; num += (float)(this.angle / this.projectionCount))
		{
			float num2 = UnityEngine.Random.Range((float)(-(float)this.angle / this.projectionCount) / 2f, (float)(this.angle / this.projectionCount) / 2f);
			Vector2 vector = new Vector2(Mathf.Cos((num + num2) * 3.1415927f / 180f), Mathf.Sin((num + num2) * 3.1415927f / 180f));
			int num3 = 3;
			float num4 = (float)(this.intensity / 4);
			Vector2 a2 = vector;
			float num5 = 0f;
			while ((double)num4 > 0.01 && num5 < (float)RadiationGridEmitter.MAX_EMIT_DISTANCE)
			{
				num5 += 1f / (float)num3;
				int num6 = Grid.PosToCell(a + a2 * num5);
				if (!Grid.IsValidCell(num6))
				{
					break;
				}
				if (!this.scanCells.Contains(num6))
				{
					SimMessages.ModifyRadiationOnCell(num6, (float)Mathf.RoundToInt(num4), -1);
					this.scanCells.Add(num6);
				}
				num4 *= Mathf.Max(0f, 1f - Mathf.Pow(Grid.Mass[num6], 1.25f) * Grid.Element[num6].molarMass / 1000000f);
				num4 *= UnityEngine.Random.Range(0.96f, 0.98f);
			}
		}
	}

	// Token: 0x06005164 RID: 20836 RVA: 0x001D824F File Offset: 0x001D644F
	private int CalculateFalloff(float falloffRate, int cell, int origin)
	{
		return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float)Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
	}

	// Token: 0x040036FF RID: 14079
	private static int MAX_EMIT_DISTANCE = 128;

	// Token: 0x04003700 RID: 14080
	public int originCell = -1;

	// Token: 0x04003701 RID: 14081
	public int intensity = 1;

	// Token: 0x04003702 RID: 14082
	public int projectionCount = 20;

	// Token: 0x04003703 RID: 14083
	public int direction;

	// Token: 0x04003704 RID: 14084
	public int angle = 360;

	// Token: 0x04003705 RID: 14085
	public bool enabled;

	// Token: 0x04003706 RID: 14086
	private HashSet<int> scanCells = new HashSet<int>();
}
