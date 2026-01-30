using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E41 RID: 3649
public class HabitatModuleSideScreen : SideScreenContent
{
	// Token: 0x170007F7 RID: 2039
	// (get) Token: 0x060073B5 RID: 29621 RVA: 0x002C30BF File Offset: 0x002C12BF
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x060073B6 RID: 29622 RVA: 0x002C30CC File Offset: 0x002C12CC
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x060073B7 RID: 29623 RVA: 0x002C30DC File Offset: 0x002C12DC
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x060073B8 RID: 29624 RVA: 0x002C30E3 File Offset: 0x002C12E3
	public override bool IsValidForTarget(GameObject target)
	{
		return false;
	}

	// Token: 0x060073B9 RID: 29625 RVA: 0x002C30E8 File Offset: 0x002C12E8
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		PassengerRocketModule passengerModule = this.GetPassengerModule(this.targetCraft);
		this.RefreshModulePanel(passengerModule);
	}

	// Token: 0x060073BA RID: 29626 RVA: 0x002C311C File Offset: 0x002C131C
	private PassengerRocketModule GetPassengerModule(Clustercraft craft)
	{
		foreach (Ref<RocketModuleCluster> @ref in craft.GetComponent<CraftModuleInterface>().ClusterModules)
		{
			PassengerRocketModule component = @ref.Get().GetComponent<PassengerRocketModule>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x060073BB RID: 29627 RVA: 0x002C3184 File Offset: 0x002C1384
	private void RefreshModulePanel(PassengerRocketModule module)
	{
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("icon").sprite = Def.GetUISprite(module.gameObject, "ui", false).first;
		KButton reference = component.GetReference<KButton>("button");
		reference.ClearOnClick();
		reference.onClick += delegate()
		{
			AudioMixer.instance.Start(module.interiorReverbSnapshot);
			AudioMixer.instance.PauseSpaceVisibleSnapshot(true);
			ClusterManager.Instance.SetActiveWorld(module.GetComponent<ClustercraftExteriorDoor>().GetTargetWorld().id);
			ManagementMenu.Instance.CloseAll();
		};
		component.GetReference<LocText>("label").SetText(module.gameObject.GetProperName());
	}

	// Token: 0x04005007 RID: 20487
	private Clustercraft targetCraft;

	// Token: 0x04005008 RID: 20488
	public GameObject moduleContentContainer;

	// Token: 0x04005009 RID: 20489
	public GameObject modulePanelPrefab;
}
