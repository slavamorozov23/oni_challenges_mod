using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000E88 RID: 3720
public class TemporalTearSideScreen : SideScreenContent
{
	// Token: 0x17000831 RID: 2097
	// (get) Token: 0x06007670 RID: 30320 RVA: 0x002D2CD4 File Offset: 0x002D0ED4
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x06007671 RID: 30321 RVA: 0x002D2CE1 File Offset: 0x002D0EE1
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06007672 RID: 30322 RVA: 0x002D2CF1 File Offset: 0x002D0EF1
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x06007673 RID: 30323 RVA: 0x002D2CF8 File Offset: 0x002D0EF8
	public override bool IsValidForTarget(GameObject target)
	{
		Clustercraft component = target.GetComponent<Clustercraft>();
		TemporalTear temporalTear = ClusterManager.Instance.GetComponent<ClusterPOIManager>().GetTemporalTear();
		return component != null && temporalTear != null && temporalTear.Location == component.Location;
	}

	// Token: 0x06007674 RID: 30324 RVA: 0x002D2D44 File Offset: 0x002D0F44
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		KButton reference = base.GetComponent<HierarchyReferences>().GetReference<KButton>("button");
		reference.ClearOnClick();
		reference.onClick += delegate()
		{
			target.GetComponent<Clustercraft>();
			ClusterManager.Instance.GetComponent<ClusterPOIManager>().GetTemporalTear().ConsumeCraft(this.targetCraft);
		};
		this.RefreshPanel(null);
	}

	// Token: 0x06007675 RID: 30325 RVA: 0x002D2DB0 File Offset: 0x002D0FB0
	private void RefreshPanel(object data = null)
	{
		TemporalTear temporalTear = ClusterManager.Instance.GetComponent<ClusterPOIManager>().GetTemporalTear();
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		bool flag = temporalTear.IsOpen();
		component.GetReference<LocText>("label").SetText(flag ? UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.BUTTON_OPEN : UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.BUTTON_CLOSED);
		component.GetReference<KButton>("button").isInteractable = flag;
	}

	// Token: 0x040051F5 RID: 20981
	private Clustercraft targetCraft;
}
