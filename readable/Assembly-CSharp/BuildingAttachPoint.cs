using System;
using UnityEngine;

// Token: 0x020006F6 RID: 1782
[AddComponentMenu("KMonoBehaviour/scripts/BuildingAttachPoint")]
public class BuildingAttachPoint : KMonoBehaviour
{
	// Token: 0x06002C1A RID: 11290 RVA: 0x00100E12 File Offset: 0x000FF012
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.BuildingAttachPoints.Add(this);
		this.TryAttachEmptyHardpoints();
	}

	// Token: 0x06002C1B RID: 11291 RVA: 0x00100E2B File Offset: 0x000FF02B
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06002C1C RID: 11292 RVA: 0x00100E34 File Offset: 0x000FF034
	private void TryAttachEmptyHardpoints()
	{
		for (int i = 0; i < this.points.Length; i++)
		{
			if (!(this.points[i].attachedBuilding != null))
			{
				bool flag = false;
				int num = 0;
				while (num < Components.AttachableBuildings.Count && !flag)
				{
					if (Components.AttachableBuildings[num].attachableToTag == this.points[i].attachableType && Grid.OffsetCell(Grid.PosToCell(base.gameObject), this.points[i].position) == Grid.PosToCell(Components.AttachableBuildings[num]))
					{
						this.points[i].attachedBuilding = Components.AttachableBuildings[num];
						flag = true;
					}
					num++;
				}
			}
		}
	}

	// Token: 0x06002C1D RID: 11293 RVA: 0x00100F0C File Offset: 0x000FF10C
	public bool AcceptsAttachment(Tag type, int cell)
	{
		int cell2 = Grid.PosToCell(base.gameObject);
		for (int i = 0; i < this.points.Length; i++)
		{
			if (Grid.OffsetCell(cell2, this.points[i].position) == cell && this.points[i].attachableType == type)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002C1E RID: 11294 RVA: 0x00100F6E File Offset: 0x000FF16E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.BuildingAttachPoints.Remove(this);
	}

	// Token: 0x04001A26 RID: 6694
	public BuildingAttachPoint.HardPoint[] points = new BuildingAttachPoint.HardPoint[0];

	// Token: 0x020015BA RID: 5562
	[Serializable]
	public struct HardPoint
	{
		// Token: 0x0600944D RID: 37965 RVA: 0x00378639 File Offset: 0x00376839
		public HardPoint(CellOffset position, Tag attachableType, AttachableBuilding attachedBuilding)
		{
			this.position = position;
			this.attachableType = attachableType;
			this.attachedBuilding = attachedBuilding;
		}

		// Token: 0x0400727F RID: 29311
		public CellOffset position;

		// Token: 0x04007280 RID: 29312
		public Tag attachableType;

		// Token: 0x04007281 RID: 29313
		public AttachableBuilding attachedBuilding;
	}
}
