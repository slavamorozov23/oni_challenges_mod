using System;
using UnityEngine;

// Token: 0x02000C1E RID: 3102
public class WaterTrapGuide : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x06005D4A RID: 23882 RVA: 0x0021C57D File Offset: 0x0021A77D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.parentController = this.parent.GetComponent<KBatchedAnimController>();
		this.guideController = base.GetComponent<KBatchedAnimController>();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

	// Token: 0x06005D4B RID: 23883 RVA: 0x0021C5AE File Offset: 0x0021A7AE
	private void RefreshTint()
	{
		this.guideController.TintColour = this.parentController.TintColour;
	}

	// Token: 0x06005D4C RID: 23884 RVA: 0x0021C5C6 File Offset: 0x0021A7C6
	public void RefreshPosition()
	{
		if (this.guideController != null && this.guideController.IsMoving)
		{
			this.guideController.SetDirty();
		}
	}

	// Token: 0x06005D4D RID: 23885 RVA: 0x0021C5F0 File Offset: 0x0021A7F0
	private void RefreshDepthAvailable()
	{
		int depthAvailable = WaterTrapGuide.GetDepthAvailable(Grid.PosToCell(this), this.parent);
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
				WaterTrapGuide.OccupyArea(this.parent, depthAvailable);
			}
			this.previousDepthAvailable = depthAvailable;
		}
	}

	// Token: 0x06005D4E RID: 23886 RVA: 0x0021C674 File Offset: 0x0021A874
	public void RenderEveryTick(float dt)
	{
		this.RefreshPosition();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

	// Token: 0x06005D4F RID: 23887 RVA: 0x0021C688 File Offset: 0x0021A888
	public static void OccupyArea(GameObject go, int depth_available)
	{
		int cell = Grid.PosToCell(go.transform.GetPosition());
		for (int i = 1; i <= 4; i++)
		{
			int key = Grid.OffsetCell(cell, 0, -i);
			if (i <= depth_available)
			{
				Grid.ObjectLayers[1][key] = go;
			}
			else if (Grid.ObjectLayers[1].ContainsKey(key) && Grid.ObjectLayers[1][key] == go)
			{
				Grid.ObjectLayers[1][key] = null;
			}
		}
	}

	// Token: 0x06005D50 RID: 23888 RVA: 0x0021C708 File Offset: 0x0021A908
	public static int GetDepthAvailable(int root_cell, GameObject pump)
	{
		int num = 4;
		int result = 0;
		for (int i = 1; i <= num; i++)
		{
			int num2 = Grid.OffsetCell(root_cell, 0, -i);
			if (!Grid.IsValidCell(num2) || Grid.Solid[num2] || (Grid.ObjectLayers[1].ContainsKey(num2) && !(Grid.ObjectLayers[1][num2] == null) && !(Grid.ObjectLayers[1][num2] == pump)))
			{
				break;
			}
			result = i;
		}
		return result;
	}

	// Token: 0x04003E2D RID: 15917
	private int previousDepthAvailable = -1;

	// Token: 0x04003E2E RID: 15918
	public GameObject parent;

	// Token: 0x04003E2F RID: 15919
	public bool occupyTiles;

	// Token: 0x04003E30 RID: 15920
	private KBatchedAnimController parentController;

	// Token: 0x04003E31 RID: 15921
	private KBatchedAnimController guideController;
}
