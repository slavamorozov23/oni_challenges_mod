using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D02 RID: 3330
public class DetailsScreenMaterialPanel : TargetScreen
{
	// Token: 0x06006707 RID: 26375 RVA: 0x0026D069 File Offset: 0x0026B269
	public override bool IsValidForTarget(GameObject target)
	{
		return true;
	}

	// Token: 0x06006708 RID: 26376 RVA: 0x0026D06C File Offset: 0x0026B26C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.openChangeMaterialPanelButton.onClick += delegate()
		{
			this.OpenMaterialSelectionPanel();
			this.RefreshMaterialSelectionPanel();
			this.RefreshOrderChangeMaterialButton();
		};
	}

	// Token: 0x06006709 RID: 26377 RVA: 0x0026D08C File Offset: 0x0026B28C
	public override void SetTarget(GameObject target)
	{
		if (this.selectedTarget != null)
		{
			this.selectedTarget.Unsubscribe(this.subHandle);
		}
		this.building = null;
		base.SetTarget(target);
		if (target == null)
		{
			return;
		}
		this.materialSelectionPanel.gameObject.SetActive(false);
		this.orderChangeMaterialButton.ClearOnClick();
		this.orderChangeMaterialButton.isInteractable = false;
		this.CloseMaterialSelectionPanel();
		this.building = this.selectedTarget.GetComponent<Building>();
		bool flag = Db.Get().TechItems.IsTechItemComplete(this.building.Def.PrefabID) || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
		this.openChangeMaterialPanelButton.isInteractable = (target.GetComponent<Reconstructable>() != null && target.GetComponent<Reconstructable>().AllowReconstruct && flag);
		this.openChangeMaterialPanelButton.GetComponent<ToolTip>().SetSimpleTooltip(flag ? "" : string.Format(UI.PRODUCTINFO_REQUIRESRESEARCHDESC, Db.Get().TechItems.GetTechFromItemID(this.building.Def.PrefabID).Name));
		this.Refresh(null);
		this.subHandle = target.Subscribe(954267658, new Action<object>(this.RefreshOrderChangeMaterialButton));
		Game.Instance.Subscribe(1980521255, new Action<object>(this.Refresh));
	}

	// Token: 0x0600670A RID: 26378 RVA: 0x0026D1FC File Offset: 0x0026B3FC
	private void OpenMaterialSelectionPanel()
	{
		this.openChangeMaterialPanelButton.gameObject.SetActive(false);
		this.materialSelectionPanel.gameObject.SetActive(true);
		this.RefreshMaterialSelectionPanel();
		if (this.selectedTarget != null && this.building != null)
		{
			this.materialSelectionPanel.SelectSourcesMaterials(this.building);
		}
	}

	// Token: 0x0600670B RID: 26379 RVA: 0x0026D25E File Offset: 0x0026B45E
	private void CloseMaterialSelectionPanel()
	{
		this.currentMaterialDescriptionRow.gameObject.SetActive(true);
		this.openChangeMaterialPanelButton.gameObject.SetActive(true);
		this.materialSelectionPanel.gameObject.SetActive(false);
	}

	// Token: 0x0600670C RID: 26380 RVA: 0x0026D293 File Offset: 0x0026B493
	public override void OnDeselectTarget(GameObject target)
	{
		base.OnDeselectTarget(target);
		this.Refresh(null);
	}

	// Token: 0x0600670D RID: 26381 RVA: 0x0026D2A3 File Offset: 0x0026B4A3
	private void Refresh(object data = null)
	{
		this.RefreshCurrentMaterial();
		this.RefreshMaterialSelectionPanel();
	}

	// Token: 0x0600670E RID: 26382 RVA: 0x0026D2B4 File Offset: 0x0026B4B4
	private void RefreshCurrentMaterial()
	{
		if (this.selectedTarget == null)
		{
			return;
		}
		CellSelectionObject component = this.selectedTarget.GetComponent<CellSelectionObject>();
		Element element = (component == null) ? this.selectedTarget.GetComponent<PrimaryElement>().Element : component.element;
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(element, "ui", false);
		this.currentMaterialIcon.sprite = uisprite.first;
		this.currentMaterialIcon.color = uisprite.second;
		if (component == null)
		{
			this.currentMaterialLabel.SetText(element.name + " x " + GameUtil.GetFormattedMass(this.selectedTarget.GetComponent<PrimaryElement>().Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		}
		else
		{
			this.currentMaterialLabel.SetText(element.name);
		}
		this.currentMaterialDescription.SetText(element.description);
		List<Descriptor> materialDescriptors = GameUtil.GetMaterialDescriptors(element);
		if (materialDescriptors.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.EFFECTS_HEADER, ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.EFFECTS_HEADER, Descriptor.DescriptorType.Effect);
			materialDescriptors.Insert(0, item);
			this.descriptorPanel.gameObject.SetActive(true);
			this.descriptorPanel.SetDescriptors(materialDescriptors);
			return;
		}
		this.descriptorPanel.gameObject.SetActive(false);
	}

	// Token: 0x0600670F RID: 26383 RVA: 0x0026D400 File Offset: 0x0026B600
	private void RefreshMaterialSelectionPanel()
	{
		if (this.selectedTarget == null)
		{
			return;
		}
		this.materialSelectionPanel.ClearSelectActions();
		if (!(this.building == null) && !(this.building is BuildingUnderConstruction))
		{
			this.materialSelectionPanel.ConfigureScreen(this.building.Def.CraftRecipe, (BuildingDef data) => true, (BuildingDef data) => "");
			this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.RefreshOrderChangeMaterialButton));
			Reconstructable component = this.selectedTarget.GetComponent<Reconstructable>();
			if (component != null && component.ReconstructRequested)
			{
				if (!this.materialSelectionPanel.gameObject.activeSelf)
				{
					this.OpenMaterialSelectionPanel();
					this.materialSelectionPanel.RefreshSelectors();
				}
				this.materialSelectionPanel.ForceSelectPrimaryTag(component.PrimarySelectedElementTag);
			}
		}
		this.confirmChangeRow.transform.SetAsLastSibling();
	}

	// Token: 0x06006710 RID: 26384 RVA: 0x0026D519 File Offset: 0x0026B719
	private void RefreshOrderChangeMaterialButton()
	{
		this.RefreshOrderChangeMaterialButton(null);
	}

	// Token: 0x06006711 RID: 26385 RVA: 0x0026D524 File Offset: 0x0026B724
	private void RefreshOrderChangeMaterialButton(object data = null)
	{
		if (this.selectedTarget == null)
		{
			return;
		}
		Reconstructable reconstructable = this.selectedTarget.GetComponent<Reconstructable>();
		bool flag = this.materialSelectionPanel.CurrentSelectedElement != null;
		this.orderChangeMaterialButton.isInteractable = (flag && this.building.GetComponent<PrimaryElement>().Element.tag != this.materialSelectionPanel.CurrentSelectedElement);
		this.orderChangeMaterialButton.ClearOnClick();
		this.orderChangeMaterialButton.onClick += delegate()
		{
			reconstructable.RequestReconstruct(this.materialSelectionPanel.CurrentSelectedElement);
			this.RefreshOrderChangeMaterialButton();
		};
		this.orderChangeMaterialButton.GetComponentInChildren<LocText>().SetText(reconstructable.ReconstructRequested ? UI.USERMENUACTIONS.RECONSTRUCT.CANCEL_RECONSTRUCT : UI.USERMENUACTIONS.RECONSTRUCT.REQUEST_RECONSTRUCT);
		this.orderChangeMaterialButton.GetComponent<ToolTip>().SetSimpleTooltip(reconstructable.ReconstructRequested ? UI.USERMENUACTIONS.RECONSTRUCT.CANCEL_RECONSTRUCT_TOOLTIP : UI.USERMENUACTIONS.RECONSTRUCT.REQUEST_RECONSTRUCT_TOOLTIP);
	}

	// Token: 0x04004677 RID: 18039
	[Header("Current Material")]
	[SerializeField]
	private Image currentMaterialIcon;

	// Token: 0x04004678 RID: 18040
	[SerializeField]
	private RectTransform currentMaterialDescriptionRow;

	// Token: 0x04004679 RID: 18041
	[SerializeField]
	private LocText currentMaterialLabel;

	// Token: 0x0400467A RID: 18042
	[SerializeField]
	private LocText currentMaterialDescription;

	// Token: 0x0400467B RID: 18043
	[SerializeField]
	private DescriptorPanel descriptorPanel;

	// Token: 0x0400467C RID: 18044
	[Header("Change Material")]
	[SerializeField]
	private MaterialSelectionPanel materialSelectionPanel;

	// Token: 0x0400467D RID: 18045
	[SerializeField]
	private RectTransform confirmChangeRow;

	// Token: 0x0400467E RID: 18046
	[SerializeField]
	private KButton orderChangeMaterialButton;

	// Token: 0x0400467F RID: 18047
	[SerializeField]
	private KButton openChangeMaterialPanelButton;

	// Token: 0x04004680 RID: 18048
	private int subHandle = -1;

	// Token: 0x04004681 RID: 18049
	private Building building;
}
