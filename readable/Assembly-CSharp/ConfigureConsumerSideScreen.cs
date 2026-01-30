using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E2E RID: 3630
public class ConfigureConsumerSideScreen : SideScreenContent
{
	// Token: 0x0600733F RID: 29503 RVA: 0x002C0D2B File Offset: 0x002BEF2B
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IConfigurableConsumer>() != null;
	}

	// Token: 0x06007340 RID: 29504 RVA: 0x002C0D36 File Offset: 0x002BEF36
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetProducer = target.GetComponent<IConfigurableConsumer>();
		if (this.settings == null)
		{
			this.settings = this.targetProducer.GetSettingOptions();
		}
		this.PopulateOptions();
	}

	// Token: 0x06007341 RID: 29505 RVA: 0x002C0D6C File Offset: 0x002BEF6C
	private void ClearOldOptions()
	{
		if (this.descriptor != null)
		{
			this.descriptor.gameObject.SetActive(false);
		}
		for (int i = 0; i < this.settingToggles.Count; i++)
		{
			this.settingToggles[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x06007342 RID: 29506 RVA: 0x002C0DC8 File Offset: 0x002BEFC8
	private void PopulateOptions()
	{
		this.ClearOldOptions();
		for (int i = this.settingToggles.Count; i < this.settings.Length; i++)
		{
			IConfigurableConsumerOption setting = this.settings[i];
			HierarchyReferences component = Util.KInstantiateUI(this.consumptionSettingTogglePrefab, this.consumptionSettingToggleContainer.gameObject, true).GetComponent<HierarchyReferences>();
			this.settingToggles.Add(component);
			component.GetReference<LocText>("Label").text = setting.GetName();
			component.GetReference<Image>("Image").sprite = setting.GetIcon();
			MultiToggle reference = component.GetReference<MultiToggle>("Toggle");
			reference.onClick = (System.Action)Delegate.Combine(reference.onClick, new System.Action(delegate()
			{
				this.SelectOption(setting);
			}));
		}
		this.RefreshToggles();
		this.RefreshDetails();
	}

	// Token: 0x06007343 RID: 29507 RVA: 0x002C0EB2 File Offset: 0x002BF0B2
	private void SelectOption(IConfigurableConsumerOption option)
	{
		this.targetProducer.SetSelectedOption(option);
		this.RefreshToggles();
		this.RefreshDetails();
	}

	// Token: 0x06007344 RID: 29508 RVA: 0x002C0ECC File Offset: 0x002BF0CC
	private void RefreshToggles()
	{
		for (int i = 0; i < this.settingToggles.Count; i++)
		{
			MultiToggle reference = this.settingToggles[i].GetReference<MultiToggle>("Toggle");
			reference.ChangeState((this.settings[i] == this.targetProducer.GetSelectedOption()) ? 1 : 0);
			reference.gameObject.SetActive(true);
		}
	}

	// Token: 0x06007345 RID: 29509 RVA: 0x002C0F30 File Offset: 0x002BF130
	private void RefreshDetails()
	{
		if (this.descriptor == null)
		{
			GameObject gameObject = Util.KInstantiateUI(this.settingDescriptorPrefab, this.settingEffectRowsContainer.gameObject, true);
			this.descriptor = gameObject.GetComponent<LocText>();
		}
		IConfigurableConsumerOption selectedOption = this.targetProducer.GetSelectedOption();
		if (selectedOption != null)
		{
			this.descriptor.text = selectedOption.GetDetailedDescription();
			this.selectedOptionNameLabel.text = "<b>" + selectedOption.GetName() + "</b>";
			this.descriptor.gameObject.SetActive(true);
			return;
		}
		this.selectedOptionNameLabel.text = UI.UISIDESCREENS.FABRICATORSIDESCREEN.NORECIPESELECTED;
	}

	// Token: 0x06007346 RID: 29510 RVA: 0x002C0FD6 File Offset: 0x002BF1D6
	public override int GetSideScreenSortOrder()
	{
		return 1;
	}

	// Token: 0x04004FB9 RID: 20409
	[SerializeField]
	private RectTransform consumptionSettingToggleContainer;

	// Token: 0x04004FBA RID: 20410
	[SerializeField]
	private GameObject consumptionSettingTogglePrefab;

	// Token: 0x04004FBB RID: 20411
	[SerializeField]
	private RectTransform settingRequirementRowsContainer;

	// Token: 0x04004FBC RID: 20412
	[SerializeField]
	private RectTransform settingEffectRowsContainer;

	// Token: 0x04004FBD RID: 20413
	[SerializeField]
	private LocText selectedOptionNameLabel;

	// Token: 0x04004FBE RID: 20414
	[SerializeField]
	private GameObject settingDescriptorPrefab;

	// Token: 0x04004FBF RID: 20415
	private IConfigurableConsumer targetProducer;

	// Token: 0x04004FC0 RID: 20416
	private IConfigurableConsumerOption[] settings;

	// Token: 0x04004FC1 RID: 20417
	private LocText descriptor;

	// Token: 0x04004FC2 RID: 20418
	private List<HierarchyReferences> settingToggles = new List<HierarchyReferences>();

	// Token: 0x04004FC3 RID: 20419
	private List<GameObject> requirementRows = new List<GameObject>();
}
