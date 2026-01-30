using System;
using UnityEngine;

// Token: 0x02000E19 RID: 3609
public class AutoPlumberSideScreen : SideScreenContent
{
	// Token: 0x0600726E RID: 29294 RVA: 0x002BBBEC File Offset: 0x002B9DEC
	protected override void OnSpawn()
	{
		this.activateButton.onClick += delegate()
		{
			DevAutoPlumber.AutoPlumbBuilding(this.building);
		};
		this.powerButton.onClick += delegate()
		{
			DevAutoPlumber.DoElectricalPlumbing(this.building);
		};
		this.pipesButton.onClick += delegate()
		{
			DevAutoPlumber.DoLiquidAndGasPlumbing(this.building);
		};
		this.solidsButton.onClick += delegate()
		{
			DevAutoPlumber.SetupSolidOreDelivery(this.building);
		};
		this.minionButton.onClick += delegate()
		{
			this.SpawnMinion();
		};
	}

	// Token: 0x0600726F RID: 29295 RVA: 0x002BBC6C File Offset: 0x002B9E6C
	private void SpawnMinion()
	{
		MinionStartingStats minionStartingStats = new MinionStartingStats(false, null, null, true);
		GameObject prefab = Assets.GetPrefab(BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
		GameObject gameObject = Util.KInstantiate(prefab, null, null);
		gameObject.name = prefab.name;
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		Vector3 position = Grid.CellToPos(Grid.PosToCell(this.building), CellAlignment.Bottom, Grid.SceneLayer.Move);
		gameObject.transform.SetLocalPosition(position);
		gameObject.SetActive(true);
		minionStartingStats.Apply(gameObject);
	}

	// Token: 0x06007270 RID: 29296 RVA: 0x002BBCEB File Offset: 0x002B9EEB
	public override int GetSideScreenSortOrder()
	{
		return -150;
	}

	// Token: 0x06007271 RID: 29297 RVA: 0x002BBCF2 File Offset: 0x002B9EF2
	public override bool IsValidForTarget(GameObject target)
	{
		return DebugHandler.InstantBuildMode && target.GetComponent<Building>() != null;
	}

	// Token: 0x06007272 RID: 29298 RVA: 0x002BBD09 File Offset: 0x002B9F09
	public override void SetTarget(GameObject target)
	{
		this.building = target.GetComponent<Building>();
	}

	// Token: 0x06007273 RID: 29299 RVA: 0x002BBD17 File Offset: 0x002B9F17
	public override void ClearTarget()
	{
	}

	// Token: 0x04004F10 RID: 20240
	public KButton activateButton;

	// Token: 0x04004F11 RID: 20241
	public KButton powerButton;

	// Token: 0x04004F12 RID: 20242
	public KButton pipesButton;

	// Token: 0x04004F13 RID: 20243
	public KButton solidsButton;

	// Token: 0x04004F14 RID: 20244
	public KButton minionButton;

	// Token: 0x04004F15 RID: 20245
	private Building building;
}
