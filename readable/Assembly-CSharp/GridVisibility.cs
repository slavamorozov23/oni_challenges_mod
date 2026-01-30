using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000980 RID: 2432
[AddComponentMenu("KMonoBehaviour/scripts/GridVisibility")]
public class GridVisibility : KMonoBehaviour
{
	// Token: 0x060045B3 RID: 17843 RVA: 0x00193470 File Offset: 0x00191670
	protected override void OnSpawn()
	{
		this.cellChangeHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, GridVisibility.OnCellChangeDispatcher, this, "GridVisibility.OnSpawn");
		this.OnCellChange();
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		if (myWorld != null && !base.gameObject.HasTag(GameTags.Stored))
		{
			myWorld.SetDiscovered(false);
		}
	}

	// Token: 0x060045B4 RID: 17844 RVA: 0x001934D4 File Offset: 0x001916D4
	private void OnCellChange()
	{
		if (base.gameObject.HasTag(GameTags.Dead))
		{
			return;
		}
		int num = Grid.PosToCell(this);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		if (!Grid.Revealed[num])
		{
			int baseX;
			int baseY;
			Grid.PosToXY(base.transform.GetPosition(), out baseX, out baseY);
			GridVisibility.Reveal(baseX, baseY, this.radius, this.innerRadius);
			Grid.Revealed[num] = true;
		}
		FogOfWarMask.ClearMask(num);
	}

	// Token: 0x060045B5 RID: 17845 RVA: 0x0019354C File Offset: 0x0019174C
	public static void Reveal(int baseX, int baseY, int radius, float innerRadius)
	{
		int num = (int)Grid.WorldIdx[baseY * Grid.WidthInCells + baseX];
		for (int i = -radius; i <= radius; i++)
		{
			for (int j = -radius; j <= radius; j++)
			{
				int num2 = baseY + i;
				int num3 = baseX + j;
				if (num2 >= 0 && Grid.HeightInCells - 1 >= num2 && num3 >= 0 && Grid.WidthInCells - 1 >= num3)
				{
					int num4 = num2 * Grid.WidthInCells + num3;
					if (Grid.Visible[num4] < 255 && num == (int)Grid.WorldIdx[num4])
					{
						Vector2 vector = new Vector2((float)j, (float)i);
						float num5 = Mathf.Lerp(1f, 0f, (vector.magnitude - innerRadius) / ((float)radius - innerRadius));
						Grid.Reveal(num4, (byte)(255f * num5), false);
					}
				}
			}
		}
	}

	// Token: 0x060045B6 RID: 17846 RVA: 0x00193617 File Offset: 0x00191817
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangeHandlerID);
	}

	// Token: 0x04002EF9 RID: 12025
	public int radius = 18;

	// Token: 0x04002EFA RID: 12026
	public float innerRadius = 16.5f;

	// Token: 0x04002EFB RID: 12027
	private ulong cellChangeHandlerID;

	// Token: 0x04002EFC RID: 12028
	private static readonly Action<object> OnCellChangeDispatcher = delegate(object obj)
	{
		Unsafe.As<GridVisibility>(obj).OnCellChange();
	};
}
