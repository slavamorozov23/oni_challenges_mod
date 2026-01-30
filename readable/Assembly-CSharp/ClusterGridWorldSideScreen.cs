using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E27 RID: 3623
public class ClusterGridWorldSideScreen : SideScreenContent
{
	// Token: 0x060072FE RID: 29438 RVA: 0x002BE538 File Offset: 0x002BC738
	protected override void OnSpawn()
	{
		this.viewButton.onClick += this.OnClickView;
	}

	// Token: 0x060072FF RID: 29439 RVA: 0x002BE551 File Offset: 0x002BC751
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<AsteroidGridEntity>() != null;
	}

	// Token: 0x06007300 RID: 29440 RVA: 0x002BE560 File Offset: 0x002BC760
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetEntity = target.GetComponent<AsteroidGridEntity>();
		this.icon.sprite = Def.GetUISprite(this.targetEntity, "ui", false).first;
		WorldContainer component = this.targetEntity.GetComponent<WorldContainer>();
		bool flag = component != null && component.IsDiscovered;
		this.viewButton.isInteractable = flag;
		if (!flag)
		{
			this.viewButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERWORLDSIDESCREEN.VIEW_WORLD_DISABLE_TOOLTIP);
			return;
		}
		this.viewButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERWORLDSIDESCREEN.VIEW_WORLD_TOOLTIP);
	}

	// Token: 0x06007301 RID: 29441 RVA: 0x002BE604 File Offset: 0x002BC804
	private void OnClickView()
	{
		WorldContainer component = this.targetEntity.GetComponent<WorldContainer>();
		if (!component.IsDupeVisited)
		{
			component.LookAtSurface();
		}
		ClusterManager.Instance.SetActiveWorld(component.id);
		ManagementMenu.Instance.CloseAll();
	}

	// Token: 0x04004F7B RID: 20347
	public Image icon;

	// Token: 0x04004F7C RID: 20348
	public KButton viewButton;

	// Token: 0x04004F7D RID: 20349
	private AsteroidGridEntity targetEntity;
}
