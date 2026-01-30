using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E23 RID: 3619
public class CargoModuleSideScreen : SideScreenContent, ISimEveryTick
{
	// Token: 0x060072C8 RID: 29384 RVA: 0x002BD0E7 File Offset: 0x002BB2E7
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x060072C9 RID: 29385 RVA: 0x002BD0F7 File Offset: 0x002BB2F7
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x060072CA RID: 29386 RVA: 0x002BD0FE File Offset: 0x002BB2FE
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Clustercraft>() != null && this.GetCollectionModules(target.GetComponent<Clustercraft>()).Length != 0;
	}

	// Token: 0x060072CB RID: 29387 RVA: 0x002BD120 File Offset: 0x002BB320
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		this.RefreshModulePanel(this.targetCraft);
	}

	// Token: 0x060072CC RID: 29388 RVA: 0x002BD144 File Offset: 0x002BB344
	private IHexCellCollector[] GetCollectionModules(Clustercraft craft)
	{
		List<IHexCellCollector> list = new List<IHexCellCollector>();
		foreach (Ref<RocketModuleCluster> @ref in craft.ModuleInterface.ClusterModules)
		{
			IHexCellCollector hexCellCollector = @ref.Get().GetComponent<IHexCellCollector>();
			if (hexCellCollector == null)
			{
				hexCellCollector = @ref.Get().GetSMI<IHexCellCollector>();
			}
			if (hexCellCollector != null)
			{
				list.Add(hexCellCollector);
			}
		}
		return list.ToArray();
	}

	// Token: 0x060072CD RID: 29389 RVA: 0x002BD1C0 File Offset: 0x002BB3C0
	private void RefreshModulePanel(Clustercraft module)
	{
		this.ClearModules();
		foreach (IHexCellCollector hexCellCollector in this.GetCollectionModules(module))
		{
			GameObject gameObject = Util.KInstantiateUI(this.modulePanelPrefab, this.moduleContentContainer, true);
			this.modulePanels.Add(hexCellCollector, gameObject);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<Image>("icon").sprite = hexCellCollector.GetUISprite();
			component.GetReference<LocText>("label").SetText(hexCellCollector.GetProperName());
		}
		this.RefreshProgressBars();
		this.scrollRectLayout.preferredHeight = (this.scrollRectLayout.minHeight = Mathf.Min((float)this.modulePanels.Count, 2.5f) * this.modulePanelPrefab.GetComponent<RectTransform>().rect.height);
	}

	// Token: 0x060072CE RID: 29390 RVA: 0x002BD290 File Offset: 0x002BB490
	private void ClearModules()
	{
		foreach (KeyValuePair<IHexCellCollector, GameObject> keyValuePair in this.modulePanels)
		{
			Util.KDestroyGameObject(keyValuePair.Value.gameObject);
		}
		this.modulePanels.Clear();
	}

	// Token: 0x060072CF RID: 29391 RVA: 0x002BD2F8 File Offset: 0x002BB4F8
	private void RefreshProgressBars()
	{
		if (this.targetCraft.IsNullOrDestroyed())
		{
			return;
		}
		if (ClusterMapSelectTool.Instance.GetSelected() == null || !this.IsValidForTarget(ClusterMapSelectTool.Instance.GetSelected().gameObject))
		{
			return;
		}
		foreach (KeyValuePair<IHexCellCollector, GameObject> keyValuePair in this.modulePanels)
		{
			HierarchyReferences component = keyValuePair.Value.GetComponent<HierarchyReferences>();
			GenericUIProgressBar reference = component.GetReference<GenericUIProgressBar>("gatheringProgressBar");
			float num = 4f;
			float num2 = keyValuePair.Key.GetCapacity() - keyValuePair.Key.GetMassStored();
			if (keyValuePair.Key.CheckIsCollecting())
			{
				float num3 = keyValuePair.Key.TimeInState() % num;
				if (num2 > 0f)
				{
					reference.SetFillPercentage(num3 / num);
					reference.label.SetText(UI.UISIDESCREENS.CARGOMODULESIDESCREEN.GATHERING_IN_PROGRESS);
				}
			}
			else if (num2 == 0f)
			{
				reference.SetFillPercentage(0f);
				reference.label.SetText(UI.UISIDESCREENS.CARGOMODULESIDESCREEN.GATHERING_FULL);
			}
			else
			{
				reference.SetFillPercentage(0f);
				reference.label.SetText(UI.UISIDESCREENS.CARGOMODULESIDESCREEN.GATHERING_STOPPED);
			}
			GenericUIProgressBar reference2 = component.GetReference<GenericUIProgressBar>("capacityProgressBar");
			float fillPercentage = keyValuePair.Key.GetMassStored() / keyValuePair.Key.GetCapacity();
			reference2.SetFillPercentage(fillPercentage);
			reference2.label.SetText(keyValuePair.Key.GetCapacityBarText());
		}
	}

	// Token: 0x060072D0 RID: 29392 RVA: 0x002BD4A4 File Offset: 0x002BB6A4
	public void SimEveryTick(float dt)
	{
		this.RefreshProgressBars();
	}

	// Token: 0x04004F55 RID: 20309
	private Clustercraft targetCraft;

	// Token: 0x04004F56 RID: 20310
	private Dictionary<IHexCellCollector, GameObject> modulePanels = new Dictionary<IHexCellCollector, GameObject>();

	// Token: 0x04004F57 RID: 20311
	public GameObject moduleContentContainer;

	// Token: 0x04004F58 RID: 20312
	public GameObject modulePanelPrefab;

	// Token: 0x04004F59 RID: 20313
	[SerializeField]
	private LayoutElement scrollRectLayout;
}
