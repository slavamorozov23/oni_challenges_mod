using System;
using UnityEngine;

// Token: 0x020006D6 RID: 1750
public class ArtifactModule : SingleEntityReceptacle, IRenderEveryTick, IHexCellCollector
{
	// Token: 0x06002AD4 RID: 10964 RVA: 0x000FADC0 File Offset: 0x000F8FC0
	protected override void OnSpawn()
	{
		this.craft = this.module.CraftInterface.GetComponent<Clustercraft>();
		if (this.craft.Status == Clustercraft.CraftStatus.InFlight && base.occupyingObject != null)
		{
			base.occupyingObject.SetActive(false);
		}
		base.OnSpawn();
		base.Subscribe(705820818, new Action<object>(this.OnEnterSpace));
		base.Subscribe(-1165815793, new Action<object>(this.OnExitSpace));
	}

	// Token: 0x06002AD5 RID: 10965 RVA: 0x000FAE41 File Offset: 0x000F9041
	public void RenderEveryTick(float dt)
	{
		this.ArtifactTrackModulePosition();
	}

	// Token: 0x06002AD6 RID: 10966 RVA: 0x000FAE4C File Offset: 0x000F904C
	private void ArtifactTrackModulePosition()
	{
		this.occupyingObjectRelativePosition = this.animController.Offset + Vector3.up * 0.5f + new Vector3(0f, 0f, -1f);
		if (base.occupyingObject != null)
		{
			this.PositionOccupyingObject();
		}
	}

	// Token: 0x06002AD7 RID: 10967 RVA: 0x000FAEAB File Offset: 0x000F90AB
	private void OnEnterSpace(object data)
	{
		if (base.occupyingObject != null)
		{
			base.occupyingObject.SetActive(false);
		}
	}

	// Token: 0x06002AD8 RID: 10968 RVA: 0x000FAEC7 File Offset: 0x000F90C7
	private void OnExitSpace(object data)
	{
		if (base.occupyingObject != null)
		{
			base.occupyingObject.SetActive(true);
		}
	}

	// Token: 0x06002AD9 RID: 10969 RVA: 0x000FAEE3 File Offset: 0x000F90E3
	public bool CheckIsCollecting()
	{
		return false;
	}

	// Token: 0x06002ADA RID: 10970 RVA: 0x000FAEE6 File Offset: 0x000F90E6
	public string GetProperName()
	{
		return base.GetComponent<RocketModuleCluster>().GetProperName();
	}

	// Token: 0x06002ADB RID: 10971 RVA: 0x000FAEF3 File Offset: 0x000F90F3
	public Sprite GetUISprite()
	{
		return Def.GetUISprite(base.gameObject.GetComponent<KPrefabID>().PrefabID(), "ui", false).first;
	}

	// Token: 0x06002ADC RID: 10972 RVA: 0x000FAF1A File Offset: 0x000F911A
	public float GetCapacity()
	{
		return 1f;
	}

	// Token: 0x06002ADD RID: 10973 RVA: 0x000FAF21 File Offset: 0x000F9121
	public float GetMassStored()
	{
		return (float)this.storage.items.Count;
	}

	// Token: 0x06002ADE RID: 10974 RVA: 0x000FAF34 File Offset: 0x000F9134
	public float TimeInState()
	{
		return 0f;
	}

	// Token: 0x06002ADF RID: 10975 RVA: 0x000FAF3B File Offset: 0x000F913B
	public string GetCapacityBarText()
	{
		return string.Format("{0} / {1}", this.GetMassStored(), this.GetCapacity());
	}

	// Token: 0x0400198D RID: 6541
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x0400198E RID: 6542
	[MyCmpReq]
	private RocketModuleCluster module;

	// Token: 0x0400198F RID: 6543
	private Clustercraft craft;
}
