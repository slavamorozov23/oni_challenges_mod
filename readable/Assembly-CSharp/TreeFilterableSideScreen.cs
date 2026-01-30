using System;
using System.Collections.Generic;
using STRINGS;
using TMPro;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E8C RID: 3724
public class TreeFilterableSideScreen : SideScreenContent
{
	// Token: 0x17000832 RID: 2098
	// (get) Token: 0x060076A6 RID: 30374 RVA: 0x002D42C4 File Offset: 0x002D24C4
	private bool InputFieldEmpty
	{
		get
		{
			return this.inputField.text == "";
		}
	}

	// Token: 0x17000833 RID: 2099
	// (get) Token: 0x060076A7 RID: 30375 RVA: 0x002D42DB File Offset: 0x002D24DB
	public bool IsStorage
	{
		get
		{
			return this.storage != null;
		}
	}

	// Token: 0x060076A8 RID: 30376 RVA: 0x002D42E9 File Offset: 0x002D24E9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Initialize();
	}

	// Token: 0x060076A9 RID: 30377 RVA: 0x002D42F8 File Offset: 0x002D24F8
	private void Initialize()
	{
		if (this.initialized)
		{
			return;
		}
		this.rowPool = new UIPool<TreeFilterableSideScreenRow>(this.rowPrefab);
		this.elementPool = new UIPool<TreeFilterableSideScreenElement>(this.elementPrefab);
		MultiToggle multiToggle = this.allCheckBox;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			TreeFilterableSideScreenRow.State allCheckboxState = this.GetAllCheckboxState();
			if (allCheckboxState > TreeFilterableSideScreenRow.State.Mixed)
			{
				if (allCheckboxState == TreeFilterableSideScreenRow.State.On)
				{
					this.SetAllCheckboxState(TreeFilterableSideScreenRow.State.Off);
					return;
				}
			}
			else
			{
				this.SetAllCheckboxState(TreeFilterableSideScreenRow.State.On);
			}
		}));
		this.onlyAllowTransportItemsCheckBox.onClick = new System.Action(this.OnlyAllowTransportItemsClicked);
		this.onlyAllowSpicedItemsCheckBox.onClick = new System.Action(this.OnlyAllowSpicedItemsClicked);
		this.initialized = true;
	}

	// Token: 0x060076AA RID: 30378 RVA: 0x002D438C File Offset: 0x002D258C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.allCheckBox.transform.parent.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTONTOOLTIP);
		this.onlyAllowTransportItemsCheckBox.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ONLYALLOWTRANSPORTITEMSBUTTONTOOLTIP);
		this.onlyAllowSpicedItemsCheckBox.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ONLYALLOWSPICEDITEMSBUTTONTOOLTIP);
		this.inputField.ActivateInputField();
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER;
		this.InitSearch();
	}

	// Token: 0x060076AB RID: 30379 RVA: 0x002D4440 File Offset: 0x002D2640
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return base.GetSortKey();
	}

	// Token: 0x060076AC RID: 30380 RVA: 0x002D4456 File Offset: 0x002D2656
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x060076AD RID: 30381 RVA: 0x002D4470 File Offset: 0x002D2670
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x060076AE RID: 30382 RVA: 0x002D448A File Offset: 0x002D268A
	public override int GetSideScreenSortOrder()
	{
		return 1;
	}

	// Token: 0x060076AF RID: 30383 RVA: 0x002D4490 File Offset: 0x002D2690
	private void UpdateAllCheckBoxVisualState()
	{
		switch (this.GetAllCheckboxState())
		{
		case TreeFilterableSideScreenRow.State.Off:
			this.allCheckBox.ChangeState(0);
			return;
		case TreeFilterableSideScreenRow.State.Mixed:
			this.allCheckBox.ChangeState(1);
			return;
		case TreeFilterableSideScreenRow.State.On:
			this.allCheckBox.ChangeState(2);
			return;
		default:
			return;
		}
	}

	// Token: 0x060076B0 RID: 30384 RVA: 0x002D44E0 File Offset: 0x002D26E0
	public void Update()
	{
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (keyValuePair.Value.visualDirty)
			{
				this.visualDirty = true;
				break;
			}
		}
		if (this.visualDirty)
		{
			foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair2 in this.tagRowMap)
			{
				keyValuePair2.Value.RefreshRowElements();
				keyValuePair2.Value.UpdateCheckBoxVisualState();
			}
			this.UpdateAllCheckBoxVisualState();
			this.visualDirty = false;
		}
	}

	// Token: 0x060076B1 RID: 30385 RVA: 0x002D45AC File Offset: 0x002D27AC
	private void OnlyAllowTransportItemsClicked()
	{
		this.storage.SetOnlyFetchMarkedItems(!this.storage.GetOnlyFetchMarkedItems());
	}

	// Token: 0x060076B2 RID: 30386 RVA: 0x002D45C7 File Offset: 0x002D27C7
	private void OnlyAllowSpicedItemsClicked()
	{
		FoodStorage component = this.storage.GetComponent<FoodStorage>();
		component.SpicedFoodOnly = !component.SpicedFoodOnly;
	}

	// Token: 0x060076B3 RID: 30387 RVA: 0x002D45E4 File Offset: 0x002D27E4
	private TreeFilterableSideScreenRow.State GetAllCheckboxState()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (keyValuePair.Value.standardCommodity && keyValuePair.Value.gameObject.activeSelf)
			{
				switch (keyValuePair.Value.GetState())
				{
				case TreeFilterableSideScreenRow.State.Off:
					flag2 = true;
					break;
				case TreeFilterableSideScreenRow.State.Mixed:
					flag3 = true;
					break;
				case TreeFilterableSideScreenRow.State.On:
					flag = true;
					break;
				}
			}
		}
		if (flag3)
		{
			return TreeFilterableSideScreenRow.State.Mixed;
		}
		if (flag && !flag2)
		{
			return TreeFilterableSideScreenRow.State.On;
		}
		if (!flag && flag2)
		{
			return TreeFilterableSideScreenRow.State.Off;
		}
		if (flag && flag2)
		{
			return TreeFilterableSideScreenRow.State.Mixed;
		}
		return TreeFilterableSideScreenRow.State.Off;
	}

	// Token: 0x060076B4 RID: 30388 RVA: 0x002D46A4 File Offset: 0x002D28A4
	private void SetAllCheckboxState(TreeFilterableSideScreenRow.State newState)
	{
		switch (newState)
		{
		case TreeFilterableSideScreenRow.State.Off:
			using (Dictionary<Tag, TreeFilterableSideScreenRow>.Enumerator enumerator = this.tagRowMap.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair = enumerator.Current;
					if (keyValuePair.Value.standardCommodity)
					{
						keyValuePair.Value.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.Off);
					}
				}
				goto IL_AB;
			}
			break;
		case TreeFilterableSideScreenRow.State.Mixed:
			goto IL_AB;
		case TreeFilterableSideScreenRow.State.On:
			break;
		default:
			goto IL_AB;
		}
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair2 in this.tagRowMap)
		{
			if (keyValuePair2.Value.standardCommodity)
			{
				keyValuePair2.Value.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.On);
			}
		}
		IL_AB:
		this.visualDirty = true;
	}

	// Token: 0x060076B5 RID: 30389 RVA: 0x002D4780 File Offset: 0x002D2980
	public bool GetElementTagAcceptedState(Tag t)
	{
		return this.targetFilterable.ContainsTag(t);
	}

	// Token: 0x060076B6 RID: 30390 RVA: 0x002D4790 File Offset: 0x002D2990
	public override bool IsValidForTarget(GameObject target)
	{
		TreeFilterable component = target.GetComponent<TreeFilterable>();
		Storage component2 = target.GetComponent<Storage>();
		return component != null && target.GetComponent<FlatTagFilterable>() == null && component.showUserMenu && (component2 == null || component2.showInUI) && target.GetSMI<StorageTile.Instance>() == null;
	}

	// Token: 0x060076B7 RID: 30391 RVA: 0x002D47E6 File Offset: 0x002D29E6
	private void ReconfigureForPreviousTarget()
	{
		global::Debug.Assert(this.target != null, "TreeFilterableSideScreen trying to restore null target.");
		this.SetTarget(this.target);
	}

	// Token: 0x060076B8 RID: 30392 RVA: 0x002D480C File Offset: 0x002D2A0C
	public override void SetTarget(GameObject target)
	{
		bool flag = true;
		if (this.target == target)
		{
			flag = false;
		}
		this.Initialize();
		this.target = target;
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.targetFilterable = target.GetComponent<TreeFilterable>();
		if (this.targetFilterable == null)
		{
			global::Debug.LogError("The target provided does not have a Tree Filterable component");
			return;
		}
		this.contentMask.GetComponent<LayoutElement>().minHeight = (float)((this.targetFilterable.uiHeight == TreeFilterable.UISideScreenHeight.Tall) ? 380 : 256);
		this.storage = this.targetFilterable.GetFilterStorage();
		this.storage.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		this.storage.Subscribe(1163645216, new Action<object>(this.OnOnlySpicedItemsSettingChanged));
		this.OnOnlyFetchMarkedItemsSettingChanged(null);
		this.OnOnlySpicedItemsSettingChanged(null);
		this.allCheckBoxLabel.SetText(this.targetFilterable.allResourceFilterLabelString);
		this.CreateCategories();
		this.CreateSpecialItemRows();
		this.titlebar.SetActive(false);
		if (this.storage.showSideScreenTitleBar)
		{
			this.titlebar.SetActive(true);
			this.titlebar.GetComponentInChildren<LocText>().SetText(this.storage.GetProperName());
		}
		if (flag)
		{
			if (!this.InputFieldEmpty)
			{
				this.ClearSearch();
			}
		}
		else
		{
			this.UpdateSearchFilter();
		}
		this.ToggleSearchConfiguration(!this.InputFieldEmpty);
	}

	// Token: 0x060076B9 RID: 30393 RVA: 0x002D4980 File Offset: 0x002D2B80
	private void OnOnlyFetchMarkedItemsSettingChanged(object _)
	{
		this.onlyAllowTransportItemsCheckBox.ChangeState(this.storage.GetOnlyFetchMarkedItems() ? 1 : 0);
		if (this.storage.allowSettingOnlyFetchMarkedItems)
		{
			this.onlyallowTransportItemsRow.SetActive(true);
			return;
		}
		this.onlyallowTransportItemsRow.SetActive(false);
	}

	// Token: 0x060076BA RID: 30394 RVA: 0x002D49D0 File Offset: 0x002D2BD0
	private void OnOnlySpicedItemsSettingChanged(object _)
	{
		FoodStorage component = this.storage.GetComponent<FoodStorage>();
		if (component != null)
		{
			this.onlyallowSpicedItemsRow.SetActive(true);
			this.onlyAllowSpicedItemsCheckBox.ChangeState(component.SpicedFoodOnly ? 1 : 0);
			return;
		}
		this.onlyallowSpicedItemsRow.SetActive(false);
	}

	// Token: 0x060076BB RID: 30395 RVA: 0x002D4A22 File Offset: 0x002D2C22
	public bool IsTagAllowed(Tag tag)
	{
		return this.targetFilterable.AcceptedTags.Contains(tag);
	}

	// Token: 0x060076BC RID: 30396 RVA: 0x002D4A35 File Offset: 0x002D2C35
	public void AddTag(Tag tag)
	{
		if (this.targetFilterable == null)
		{
			return;
		}
		this.targetFilterable.AddTagToFilter(tag);
	}

	// Token: 0x060076BD RID: 30397 RVA: 0x002D4A52 File Offset: 0x002D2C52
	public void RemoveTag(Tag tag)
	{
		if (this.targetFilterable == null)
		{
			return;
		}
		this.targetFilterable.RemoveTagFromFilter(tag);
	}

	// Token: 0x060076BE RID: 30398 RVA: 0x002D4A70 File Offset: 0x002D2C70
	private List<TreeFilterableSideScreen.TagOrderInfo> GetTagsSortedAlphabetically(ICollection<Tag> tags)
	{
		List<TreeFilterableSideScreen.TagOrderInfo> list = new List<TreeFilterableSideScreen.TagOrderInfo>();
		foreach (Tag tag in tags)
		{
			list.Add(new TreeFilterableSideScreen.TagOrderInfo
			{
				tag = tag,
				strippedName = tag.ProperNameStripLink()
			});
		}
		list.Sort((TreeFilterableSideScreen.TagOrderInfo a, TreeFilterableSideScreen.TagOrderInfo b) => a.strippedName.CompareTo(b.strippedName));
		return list;
	}

	// Token: 0x060076BF RID: 30399 RVA: 0x002D4B04 File Offset: 0x002D2D04
	private TreeFilterableSideScreenRow AddRow(Tag rowTag)
	{
		if (this.tagRowMap.ContainsKey(rowTag))
		{
			return this.tagRowMap[rowTag];
		}
		TreeFilterableSideScreenRow freeElement = this.rowPool.GetFreeElement(this.rowGroup, true);
		freeElement.Parent = this;
		freeElement.standardCommodity = !STORAGEFILTERS.SPECIAL_STORAGE.Contains(rowTag);
		this.tagRowMap.Add(rowTag, freeElement);
		Dictionary<Tag, bool> dictionary = new Dictionary<Tag, bool>();
		foreach (TreeFilterableSideScreen.TagOrderInfo tagOrderInfo in this.GetTagsSortedAlphabetically(DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(rowTag)).FindAll((TreeFilterableSideScreen.TagOrderInfo t) => !this.targetFilterable.ForbiddenTags.Contains(t.tag)))
		{
			dictionary.Add(tagOrderInfo.tag, this.targetFilterable.ContainsTag(tagOrderInfo.tag) || this.targetFilterable.ContainsTag(rowTag));
		}
		freeElement.SetElement(rowTag, this.targetFilterable.ContainsTag(rowTag), dictionary);
		freeElement.transform.SetAsLastSibling();
		return freeElement;
	}

	// Token: 0x060076C0 RID: 30400 RVA: 0x002D4C18 File Offset: 0x002D2E18
	public float GetAmountInStorage(Tag tag)
	{
		if (!this.IsStorage)
		{
			return 0f;
		}
		return this.storage.GetMassAvailable(tag);
	}

	// Token: 0x060076C1 RID: 30401 RVA: 0x002D4C34 File Offset: 0x002D2E34
	private void CreateCategories()
	{
		if (this.storage.storageFilters != null && this.storage.storageFilters.Count >= 1)
		{
			bool flag = this.target.GetComponent<CreatureDeliveryPoint>() != null;
			foreach (TreeFilterableSideScreen.TagOrderInfo tagOrderInfo in this.GetTagsSortedAlphabetically(this.storage.storageFilters))
			{
				Tag tag = tagOrderInfo.tag;
				if (flag || DiscoveredResources.Instance.IsDiscovered(tag))
				{
					this.AddRow(tag);
				}
			}
			this.visualDirty = true;
			return;
		}
		global::Debug.LogError("If you're filtering, your storage filter should have the filters set on it");
	}

	// Token: 0x060076C2 RID: 30402 RVA: 0x002D4CF4 File Offset: 0x002D2EF4
	private void CreateSpecialItemRows()
	{
		this.specialItemsHeader.transform.SetAsLastSibling();
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (!keyValuePair.Value.standardCommodity)
			{
				keyValuePair.Value.transform.transform.SetAsLastSibling();
			}
		}
		this.RefreshSpecialItemsHeader();
	}

	// Token: 0x060076C3 RID: 30403 RVA: 0x002D4D7C File Offset: 0x002D2F7C
	private void RefreshSpecialItemsHeader()
	{
		bool active = false;
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (!keyValuePair.Value.standardCommodity)
			{
				active = true;
				break;
			}
		}
		this.specialItemsHeader.gameObject.SetActive(active);
	}

	// Token: 0x060076C4 RID: 30404 RVA: 0x002D4DF0 File Offset: 0x002D2FF0
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (this.target != null && (this.tagRowMap == null || this.tagRowMap.Count == 0))
		{
			this.ReconfigureForPreviousTarget();
		}
	}

	// Token: 0x060076C5 RID: 30405 RVA: 0x002D4E24 File Offset: 0x002D3024
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.storage != null)
		{
			this.storage.Unsubscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
			this.storage.Unsubscribe(1163645216, new Action<object>(this.OnOnlySpicedItemsSettingChanged));
		}
		this.rowPool.ClearAll();
		this.elementPool.ClearAll();
		this.tagRowMap.Clear();
	}

	// Token: 0x060076C6 RID: 30406 RVA: 0x002D4EA0 File Offset: 0x002D30A0
	private void RecordRowExpandedStatus()
	{
		this.rowExpandedStatusMemory.Clear();
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			this.rowExpandedStatusMemory.Add(keyValuePair.Key, keyValuePair.Value.ArrowExpanded);
		}
	}

	// Token: 0x060076C7 RID: 30407 RVA: 0x002D4F18 File Offset: 0x002D3118
	private void RestoreRowExpandedStatus()
	{
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (this.rowExpandedStatusMemory.ContainsKey(keyValuePair.Key))
			{
				keyValuePair.Value.SetArrowToggleState(this.rowExpandedStatusMemory[keyValuePair.Key]);
			}
		}
	}

	// Token: 0x060076C8 RID: 30408 RVA: 0x002D4F98 File Offset: 0x002D3198
	private void InitSearch()
	{
		KInputTextField kinputTextField = this.inputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
			KScreenManager.Instance.RefreshStack();
			UISounds.PlaySound(UISounds.Sound.ClickHUD);
			this.RecordRowExpandedStatus();
		}));
		this.inputField.onEndEdit.AddListener(delegate(string value)
		{
			base.isEditing = false;
			KScreenManager.Instance.RefreshStack();
		});
		this.inputField.onValueChanged.AddListener(delegate(string value)
		{
			if (this.InputFieldEmpty)
			{
				this.RestoreRowExpandedStatus();
			}
			this.ToggleSearchConfiguration(!this.InputFieldEmpty);
			this.UpdateSearchFilter();
		});
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER;
		this.clearButton.onClick += delegate()
		{
			if (!this.InputFieldEmpty)
			{
				this.ClearSearch();
			}
		};
	}

	// Token: 0x060076C9 RID: 30409 RVA: 0x002D503C File Offset: 0x002D323C
	private void ToggleSearchConfiguration(bool searching)
	{
		this.configurationRowsContainer.gameObject.SetActive(!searching);
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			keyValuePair.Value.ShowToggleBox(!searching);
		}
		if (searching)
		{
			this.specialItemsHeader.gameObject.SetActive(false);
			return;
		}
		this.RefreshSpecialItemsHeader();
	}

	// Token: 0x060076CA RID: 30410 RVA: 0x002D50C8 File Offset: 0x002D32C8
	private void ClearSearch()
	{
		this.inputField.text = "";
		this.RestoreRowExpandedStatus();
		this.ToggleSearchConfiguration(false);
	}

	// Token: 0x17000834 RID: 2100
	// (get) Token: 0x060076CB RID: 30411 RVA: 0x002D50E7 File Offset: 0x002D32E7
	public string CurrentSearchValue
	{
		get
		{
			if (this.inputField.text == null)
			{
				return "";
			}
			return this.inputField.text;
		}
	}

	// Token: 0x060076CC RID: 30412 RVA: 0x002D5108 File Offset: 0x002D3308
	private void UpdateSearchFilter()
	{
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			keyValuePair.Value.FilterAgainstSearch(keyValuePair.Key, this.CurrentSearchValue);
		}
	}

	// Token: 0x04005226 RID: 21030
	[SerializeField]
	private MultiToggle allCheckBox;

	// Token: 0x04005227 RID: 21031
	[SerializeField]
	private LocText allCheckBoxLabel;

	// Token: 0x04005228 RID: 21032
	[SerializeField]
	private GameObject specialItemsHeader;

	// Token: 0x04005229 RID: 21033
	[SerializeField]
	private MultiToggle onlyAllowTransportItemsCheckBox;

	// Token: 0x0400522A RID: 21034
	[SerializeField]
	private GameObject onlyallowTransportItemsRow;

	// Token: 0x0400522B RID: 21035
	[SerializeField]
	private MultiToggle onlyAllowSpicedItemsCheckBox;

	// Token: 0x0400522C RID: 21036
	[SerializeField]
	private GameObject onlyallowSpicedItemsRow;

	// Token: 0x0400522D RID: 21037
	[SerializeField]
	private TreeFilterableSideScreenRow rowPrefab;

	// Token: 0x0400522E RID: 21038
	[SerializeField]
	private GameObject rowGroup;

	// Token: 0x0400522F RID: 21039
	[SerializeField]
	private TreeFilterableSideScreenElement elementPrefab;

	// Token: 0x04005230 RID: 21040
	[SerializeField]
	private GameObject titlebar;

	// Token: 0x04005231 RID: 21041
	[SerializeField]
	private GameObject contentMask;

	// Token: 0x04005232 RID: 21042
	[SerializeField]
	private KInputTextField inputField;

	// Token: 0x04005233 RID: 21043
	[SerializeField]
	private KButton clearButton;

	// Token: 0x04005234 RID: 21044
	[SerializeField]
	private GameObject configurationRowsContainer;

	// Token: 0x04005235 RID: 21045
	private GameObject target;

	// Token: 0x04005236 RID: 21046
	private bool visualDirty;

	// Token: 0x04005237 RID: 21047
	private bool initialized;

	// Token: 0x04005238 RID: 21048
	private KImage onlyAllowTransportItemsImg;

	// Token: 0x04005239 RID: 21049
	public UIPool<TreeFilterableSideScreenElement> elementPool;

	// Token: 0x0400523A RID: 21050
	private UIPool<TreeFilterableSideScreenRow> rowPool;

	// Token: 0x0400523B RID: 21051
	private TreeFilterable targetFilterable;

	// Token: 0x0400523C RID: 21052
	private Dictionary<Tag, TreeFilterableSideScreenRow> tagRowMap = new Dictionary<Tag, TreeFilterableSideScreenRow>();

	// Token: 0x0400523D RID: 21053
	private Dictionary<Tag, bool> rowExpandedStatusMemory = new Dictionary<Tag, bool>();

	// Token: 0x0400523E RID: 21054
	private Storage storage;

	// Token: 0x020020F0 RID: 8432
	private struct TagOrderInfo
	{
		// Token: 0x04009794 RID: 38804
		public Tag tag;

		// Token: 0x04009795 RID: 38805
		public string strippedName;
	}
}
