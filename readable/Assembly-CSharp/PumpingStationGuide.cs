using System;
using UnityEngine;

// Token: 0x02000622 RID: 1570
[AddComponentMenu("KMonoBehaviour/scripts/PumpingStationGuide")]
public class PumpingStationGuide : KMonoBehaviour, IRender200ms
{
	// Token: 0x06002563 RID: 9571 RVA: 0x000D6B94 File Offset: 0x000D4D94
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.parentController = this.parent.GetComponent<KBatchedAnimController>();
		this.guideController = base.GetComponent<KBatchedAnimController>();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

	// Token: 0x06002564 RID: 9572 RVA: 0x000D6BC5 File Offset: 0x000D4DC5
	public void RefreshPosition()
	{
		if (this.guideController != null && this.guideController.IsMoving)
		{
			this.guideController.SetDirty();
		}
	}

	// Token: 0x06002565 RID: 9573 RVA: 0x000D6BED File Offset: 0x000D4DED
	private void RefreshTint()
	{
		this.guideController.TintColour = this.parentController.TintColour;
	}

	// Token: 0x06002566 RID: 9574 RVA: 0x000D6C08 File Offset: 0x000D4E08
	private void RefreshDepthAvailable()
	{
		int depthAvailable = PumpingStationGuide.GetDepthAvailable(Grid.PosToCell(this), this.parent);
		if (depthAvailable != this.previousDepthAvailable)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			if (depthAvailable == 0)
			{
				component.enabled = false;
			}
			else
			{
				component.enabled = true;
				component.Play(new HashedString("place_pipe" + depthAvailable.ToString()), KAnim.PlayMode.Once, 1f, 0f);
			}
			if (this.occupyTiles)
			{
				PumpingStationGuide.OccupyArea(this.parent, depthAvailable);
			}
			this.previousDepthAvailable = depthAvailable;
		}
	}

	// Token: 0x06002567 RID: 9575 RVA: 0x000D6C8C File Offset: 0x000D4E8C
	public void Render200ms(float dt)
	{
		this.RefreshPosition();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

	// Token: 0x06002568 RID: 9576 RVA: 0x000D6CA0 File Offset: 0x000D4EA0
	public static void OccupyArea(GameObject go, int depth_available)
	{
		int cell = Grid.PosToCell(go.transform.GetPosition());
		for (int i = 1; i <= 4; i++)
		{
			int key = Grid.OffsetCell(cell, 0, -i);
			int key2 = Grid.OffsetCell(cell, 1, -i);
			if (i <= depth_available)
			{
				Grid.ObjectLayers[1][key] = go;
				Grid.ObjectLayers[1][key2] = go;
			}
			else
			{
				if (Grid.ObjectLayers[1].ContainsKey(key) && Grid.ObjectLayers[1][key] == go)
				{
					Grid.ObjectLayers[1][key] = null;
				}
				if (Grid.ObjectLayers[1].ContainsKey(key2) && Grid.ObjectLayers[1][key2] == go)
				{
					Grid.ObjectLayers[1][key2] = null;
				}
			}
		}
	}

	// Token: 0x06002569 RID: 9577 RVA: 0x000D6D70 File Offset: 0x000D4F70
	public static int GetDepthAvailable(int root_cell, GameObject pump)
	{
		int num = 4;
		int result = 0;
		for (int i = 1; i <= num; i++)
		{
			int num2 = Grid.OffsetCell(root_cell, 0, -i);
			int num3 = Grid.OffsetCell(root_cell, 1, -i);
			if (!Grid.IsValidCell(num2) || Grid.Solid[num2] || !Grid.IsValidCell(num3) || Grid.Solid[num3] || (Grid.ObjectLayers[1].ContainsKey(num2) && !(Grid.ObjectLayers[1][num2] == null) && !(Grid.ObjectLayers[1][num2] == pump)) || (Grid.ObjectLayers[1].ContainsKey(num3) && !(Grid.ObjectLayers[1][num3] == null) && !(Grid.ObjectLayers[1][num3] == pump)))
			{
				break;
			}
			result = i;
		}
		return result;
	}

	// Token: 0x040015E4 RID: 5604
	private int previousDepthAvailable = -1;

	// Token: 0x040015E5 RID: 5605
	public GameObject parent;

	// Token: 0x040015E6 RID: 5606
	public bool occupyTiles;

	// Token: 0x040015E7 RID: 5607
	private KBatchedAnimController parentController;

	// Token: 0x040015E8 RID: 5608
	private KBatchedAnimController guideController;
}
