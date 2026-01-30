using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E42 RID: 3650
public class HarvestModuleSideScreen : SideScreenContent, ISimEveryTick
{
	// Token: 0x170007F8 RID: 2040
	// (get) Token: 0x060073BD RID: 29629 RVA: 0x002C3218 File Offset: 0x002C1418
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x060073BE RID: 29630 RVA: 0x002C3225 File Offset: 0x002C1425
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x060073BF RID: 29631 RVA: 0x002C3235 File Offset: 0x002C1435
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x060073C0 RID: 29632 RVA: 0x002C323C File Offset: 0x002C143C
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Clustercraft>() != null && this.GetResourceHarvestModule(target.GetComponent<Clustercraft>()) != null;
	}

	// Token: 0x060073C1 RID: 29633 RVA: 0x002C3260 File Offset: 0x002C1460
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		ResourceHarvestModule.StatesInstance resourceHarvestModule = this.GetResourceHarvestModule(this.targetCraft);
		this.RefreshModulePanel(resourceHarvestModule);
	}

	// Token: 0x060073C2 RID: 29634 RVA: 0x002C3294 File Offset: 0x002C1494
	private ResourceHarvestModule.StatesInstance GetResourceHarvestModule(Clustercraft craft)
	{
		foreach (Ref<RocketModuleCluster> @ref in craft.GetComponent<CraftModuleInterface>().ClusterModules)
		{
			GameObject gameObject = @ref.Get().gameObject;
			if (gameObject.GetDef<ResourceHarvestModule.Def>() != null)
			{
				return gameObject.GetSMI<ResourceHarvestModule.StatesInstance>();
			}
		}
		return null;
	}

	// Token: 0x060073C3 RID: 29635 RVA: 0x002C3300 File Offset: 0x002C1500
	private void RefreshModulePanel(StateMachine.Instance module)
	{
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("icon").sprite = Def.GetUISprite(module.gameObject, "ui", false).first;
		component.GetReference<LocText>("label").SetText(module.gameObject.GetProperName());
	}

	// Token: 0x060073C4 RID: 29636 RVA: 0x002C3354 File Offset: 0x002C1554
	public void SimEveryTick(float dt)
	{
		if (this.targetCraft.IsNullOrDestroyed())
		{
			return;
		}
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		ResourceHarvestModule.StatesInstance resourceHarvestModule = this.GetResourceHarvestModule(this.targetCraft);
		if (resourceHarvestModule == null)
		{
			return;
		}
		GenericUIProgressBar reference = component.GetReference<GenericUIProgressBar>("progressBar");
		float num = 4f;
		float num2 = resourceHarvestModule.timeinstate % num;
		if (resourceHarvestModule.sm.canHarvest.Get(resourceHarvestModule))
		{
			reference.SetFillPercentage(num2 / num);
			reference.label.SetText(UI.UISIDESCREENS.HARVESTMODULESIDESCREEN.MINING_IN_PROGRESS);
		}
		else
		{
			reference.SetFillPercentage(0f);
			reference.label.SetText(UI.UISIDESCREENS.HARVESTMODULESIDESCREEN.MINING_STOPPED);
		}
		GenericUIProgressBar reference2 = component.GetReference<GenericUIProgressBar>("diamondProgressBar");
		Storage component2 = resourceHarvestModule.GetComponent<Storage>();
		float fillPercentage = component2.MassStored() / component2.Capacity();
		reference2.SetFillPercentage(fillPercentage);
		reference2.label.SetText(ElementLoader.GetElement(SimHashes.Diamond.CreateTag()).name + ": " + GameUtil.GetFormattedMass(component2.MassStored(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x0400500A RID: 20490
	private Clustercraft targetCraft;

	// Token: 0x0400500B RID: 20491
	public GameObject moduleContentContainer;

	// Token: 0x0400500C RID: 20492
	public GameObject modulePanelPrefab;
}
