using System;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000CB1 RID: 3249
public class BuildingGroupScreen : KScreen
{
	// Token: 0x17000717 RID: 1815
	// (get) Token: 0x06006383 RID: 25475 RVA: 0x00250FF6 File Offset: 0x0024F1F6
	public static bool SearchIsEmpty
	{
		get
		{
			return BuildingGroupScreen.Instance == null || BuildingGroupScreen.Instance.inputField.text.IsNullOrWhiteSpace();
		}
	}

	// Token: 0x17000718 RID: 1816
	// (get) Token: 0x06006384 RID: 25476 RVA: 0x0025101B File Offset: 0x0024F21B
	public static bool IsEditing
	{
		get
		{
			return !(BuildingGroupScreen.Instance == null) && BuildingGroupScreen.Instance.isEditing;
		}
	}

	// Token: 0x06006385 RID: 25477 RVA: 0x00251036 File Offset: 0x0024F236
	protected override void OnPrefabInit()
	{
		BuildingGroupScreen.Instance = this;
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06006386 RID: 25478 RVA: 0x0025104C File Offset: 0x0024F24C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KInputTextField kinputTextField = this.inputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
			UISounds.PlaySound(UISounds.Sound.ClickHUD);
			this.ConfigurePlanScreenForSearch();
		}));
		this.inputField.onEndEdit.AddListener(delegate(string value)
		{
			base.isEditing = false;
		});
		this.inputField.OnValueChangesPaused = delegate()
		{
			PlanScreen.Instance.RefreshCategoryPanelTitle();
			PlanScreen.Instance.RefreshSearch();
		};
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.BUILDMENU.SEARCH_TEXT_PLACEHOLDER;
		this.clearButton.onClick += this.ClearSearch;
	}

	// Token: 0x06006387 RID: 25479 RVA: 0x00251102 File Offset: 0x0024F302
	protected override void OnActivate()
	{
		base.OnActivate();
		base.ConsumeMouseScroll = true;
		this.BindTooltip();
		KInputManager.InputChange.AddListener(new UnityAction(this.BindTooltip));
	}

	// Token: 0x06006388 RID: 25480 RVA: 0x0025112D File Offset: 0x0024F32D
	protected override void OnDeactivate()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.BindTooltip));
		base.OnDeactivate();
	}

	// Token: 0x06006389 RID: 25481 RVA: 0x0025114B File Offset: 0x0024F34B
	private void BindTooltip()
	{
		this.inputField.GetComponent<ToolTip>().toolTip = GameUtil.ReplaceHotkeyString(UI.BUILDMENU.SEARCH_TOOLTIP, global::Action.Find);
	}

	// Token: 0x0600638A RID: 25482 RVA: 0x00251171 File Offset: 0x0024F371
	public void ClearSearch()
	{
		this.inputField.text = "";
		this.inputField.ForceChangeValueRefresh();
	}

	// Token: 0x0600638B RID: 25483 RVA: 0x0025118E File Offset: 0x0024F38E
	private void ConfigurePlanScreenForSearch()
	{
		PlanScreen.Instance.SoftCloseRecipe();
		PlanScreen.Instance.ClearSelection();
		PlanScreen.Instance.ForceRefreshAllBuildingToggles();
		PlanScreen.Instance.ConfigurePanelSize(null);
	}

	// Token: 0x040043A6 RID: 17318
	public static BuildingGroupScreen Instance;

	// Token: 0x040043A7 RID: 17319
	public KInputTextField inputField;

	// Token: 0x040043A8 RID: 17320
	[SerializeField]
	public KButton clearButton;
}
