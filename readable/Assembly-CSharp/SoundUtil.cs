using System;
using UnityEngine;

// Token: 0x02000B5F RID: 2911
public static class SoundUtil
{
	// Token: 0x06005629 RID: 22057 RVA: 0x001F69E4 File Offset: 0x001F4BE4
	public static float GetLiquidDepth(int cell)
	{
		float num = 0f;
		num += Grid.Mass[cell] * (Grid.Element[cell].IsLiquid ? 1f : 0f);
		int num2 = Grid.CellBelow(cell);
		if (Grid.IsValidCell(num2))
		{
			num += Grid.Mass[num2] * (Grid.Element[num2].IsLiquid ? 1f : 0f);
		}
		return Mathf.Min(num / 1000f, 1f);
	}

	// Token: 0x0600562A RID: 22058 RVA: 0x001F6A69 File Offset: 0x001F4C69
	public static float GetLiquidVolume(float mass)
	{
		return Mathf.Min(mass / 100f, 1f);
	}
}
