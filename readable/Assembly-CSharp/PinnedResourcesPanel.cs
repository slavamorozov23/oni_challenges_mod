using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DD7 RID: 3543
public class PinnedResourcesPanel : KScreen, IRender1000ms
{
	// Token: 0x06006F0D RID: 28429 RVA: 0x002A1173 File Offset: 0x0029F373
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.rowContainerLayout = this.rowContainer.GetComponent<QuickLayout>();
	}

	// Token: 0x06006F0E RID: 28430 RVA: 0x002A118C File Offset: 0x0029F38C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		PinnedResourcesPanel.Instance = this;
		this.Populate(null);
		Game.Instance.Subscribe(1983128072, new Action<object>(this.Populate));
		MultiToggle component = this.headerButton.GetComponent<MultiToggle>();
		component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
		{
			this.Refresh();
		}));
		MultiToggle component2 = this.seeAllButton.GetComponent<MultiToggle>();
		component2.onClick = (System.Action)Delegate.Combine(component2.onClick, new System.Action(delegate()
		{
			bool flag = AllResourcesScreen.Instance.IsScreenActive();
			AllResourcesScreen.Instance.Show(!flag);
		}));
		this.seeAllLabel = this.seeAllButton.GetComponentInChildren<LocText>();
		MultiToggle component3 = this.clearNewButton.GetComponent<MultiToggle>();
		component3.onClick = (System.Action)Delegate.Combine(component3.onClick, new System.Action(delegate()
		{
			this.ClearAllNew();
		}));
		this.clearAllButton.onClick += delegate()
		{
			this.ClearAllNew();
			this.UnPinAll();
			this.Refresh();
		};
		AllResourcesScreen.Instance.Init();
		this.Refresh();
	}

	// Token: 0x06006F0F RID: 28431 RVA: 0x002A1297 File Offset: 0x0029F497
	protected override void OnForcedCleanUp()
	{
		PinnedResourcesPanel.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06006F10 RID: 28432 RVA: 0x002A12A5 File Offset: 0x0029F4A5
	public void ClearExcessiveNewItems()
	{
		if (DiscoveredResources.Instance.CheckAllDiscoveredAreNew())
		{
			DiscoveredResources.Instance.newDiscoveries.Clear();
		}
	}

	// Token: 0x06006F11 RID: 28433 RVA: 0x002A12C4 File Offset: 0x0029F4C4
	private void ClearAllNew()
	{
		foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair in this.rows)
		{
			if (keyValuePair.Value.gameObject.activeSelf && DiscoveredResources.Instance.newDiscoveries.ContainsKey(keyValuePair.Key))
			{
				DiscoveredResources.Instance.newDiscoveries.Remove(keyValuePair.Key);
			}
		}
	}

	// Token: 0x06006F12 RID: 28434 RVA: 0x002A1354 File Offset: 0x0029F554
	private void UnPinAll()
	{
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair in this.rows)
		{
			worldInventory.pinnedResources.Remove(keyValuePair.Key);
		}
	}

	// Token: 0x06006F13 RID: 28435 RVA: 0x002A13D0 File Offset: 0x0029F5D0
	private PinnedResourcesPanel.PinnedResourceRow CreateRow(Tag tag)
	{
		PinnedResourcesPanel.PinnedResourceRow pinnedResourceRow = new PinnedResourcesPanel.PinnedResourceRow(tag);
		GameObject gameObject = Util.KInstantiateUI(this.linePrefab, this.rowContainer, false);
		pinnedResourceRow.gameObject = gameObject;
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		pinnedResourceRow.icon = component.GetReference<Image>("Icon");
		pinnedResourceRow.nameLabel = component.GetReference<LocText>("NameLabel");
		pinnedResourceRow.valueLabel = component.GetReference<LocText>("ValueLabel");
		pinnedResourceRow.pinToggle = component.GetReference<MultiToggle>("PinToggle");
		pinnedResourceRow.notifyToggle = component.GetReference<MultiToggle>("NotifyToggle");
		pinnedResourceRow.newLabel = component.GetReference<MultiToggle>("NewLabel");
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(tag, "ui", false);
		pinnedResourceRow.icon.sprite = uisprite.first;
		pinnedResourceRow.icon.color = uisprite.second;
		pinnedResourceRow.nameLabel.SetText(tag.ProperNameStripLink());
		MultiToggle component2 = pinnedResourceRow.gameObject.GetComponent<MultiToggle>();
		component2.onClick = (System.Action)Delegate.Combine(component2.onClick, new System.Action(delegate()
		{
			List<Pickupable> list = ClusterManager.Instance.activeWorld.worldInventory.CreatePickupablesList(tag);
			if (list != null && list.Count > 0)
			{
				SelectTool.Instance.SelectAndFocus(list[this.clickIdx % list.Count].transform.position, list[this.clickIdx % list.Count].GetComponent<KSelectable>());
				this.clickIdx++;
				return;
			}
			this.clickIdx = 0;
		}));
		return pinnedResourceRow;
	}

	// Token: 0x06006F14 RID: 28436 RVA: 0x002A1500 File Offset: 0x0029F700
	public void Populate(object data = null)
	{
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		foreach (KeyValuePair<Tag, float> keyValuePair in DiscoveredResources.Instance.newDiscoveries)
		{
			if (!this.rows.ContainsKey(keyValuePair.Key) && this.IsDisplayedTag(keyValuePair.Key))
			{
				this.rows.Add(keyValuePair.Key, this.CreateRow(keyValuePair.Key));
			}
		}
		foreach (Tag tag in worldInventory.pinnedResources)
		{
			if (!this.rows.ContainsKey(tag))
			{
				this.rows.Add(tag, this.CreateRow(tag));
			}
		}
		foreach (Tag tag2 in worldInventory.notifyResources)
		{
			if (!this.rows.ContainsKey(tag2))
			{
				this.rows.Add(tag2, this.CreateRow(tag2));
			}
		}
		foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair2 in this.rows)
		{
			if (false || worldInventory.pinnedResources.Contains(keyValuePair2.Key) || worldInventory.notifyResources.Contains(keyValuePair2.Key) || (DiscoveredResources.Instance.newDiscoveries.ContainsKey(keyValuePair2.Key) && worldInventory.GetAmount(keyValuePair2.Key, false) > 0f))
			{
				if (!keyValuePair2.Value.gameObject.activeSelf)
				{
					keyValuePair2.Value.gameObject.SetActive(true);
				}
			}
			else if (keyValuePair2.Value.gameObject.activeSelf)
			{
				keyValuePair2.Value.gameObject.SetActive(false);
			}
		}
		foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair3 in this.rows)
		{
			keyValuePair3.Value.pinToggle.gameObject.SetActive(worldInventory.pinnedResources.Contains(keyValuePair3.Key));
		}
		this.SortRows();
		this.rowContainerLayout.ForceUpdate();
	}

	// Token: 0x06006F15 RID: 28437 RVA: 0x002A17D8 File Offset: 0x0029F9D8
	private void SortRows()
	{
		List<PinnedResourcesPanel.PinnedResourceRow> list = new List<PinnedResourcesPanel.PinnedResourceRow>();
		foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair in this.rows)
		{
			list.Add(keyValuePair.Value);
		}
		list.Sort((PinnedResourcesPanel.PinnedResourceRow a, PinnedResourcesPanel.PinnedResourceRow b) => a.SortableNameWithoutLink.CompareTo(b.SortableNameWithoutLink));
		foreach (PinnedResourcesPanel.PinnedResourceRow pinnedResourceRow in list)
		{
			this.rows[pinnedResourceRow.Tag].gameObject.transform.SetAsLastSibling();
		}
		this.clearNewButton.transform.SetAsLastSibling();
		this.seeAllButton.transform.SetAsLastSibling();
	}

	// Token: 0x06006F16 RID: 28438 RVA: 0x002A18D4 File Offset: 0x0029FAD4
	private bool IsDisplayedTag(Tag tag)
	{
		foreach (TagSet tagSet in AllResourcesScreen.Instance.allowDisplayCategories)
		{
			foreach (KeyValuePair<Tag, HashSet<Tag>> keyValuePair in DiscoveredResources.Instance.GetDiscoveredResourcesFromTagSet(tagSet))
			{
				if (keyValuePair.Value.Contains(tag))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06006F17 RID: 28439 RVA: 0x002A197C File Offset: 0x0029FB7C
	private void SyncRows()
	{
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		bool flag = false;
		foreach (Tag key in worldInventory.pinnedResources)
		{
			if (!this.rows.ContainsKey(key))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			foreach (KeyValuePair<Tag, float> keyValuePair in DiscoveredResources.Instance.newDiscoveries)
			{
				if (!this.rows.ContainsKey(keyValuePair.Key) && this.IsDisplayedTag(keyValuePair.Key))
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			foreach (Tag key2 in worldInventory.notifyResources)
			{
				if (!this.rows.ContainsKey(key2))
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair2 in this.rows)
			{
				if ((worldInventory.pinnedResources.Contains(keyValuePair2.Key) || worldInventory.notifyResources.Contains(keyValuePair2.Key) || (DiscoveredResources.Instance.newDiscoveries.ContainsKey(keyValuePair2.Key) && worldInventory.GetAmount(keyValuePair2.Key, false) > 0f)) != keyValuePair2.Value.gameObject.activeSelf)
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			this.Populate(null);
		}
	}

	// Token: 0x06006F18 RID: 28440 RVA: 0x002A1B78 File Offset: 0x0029FD78
	public void Refresh()
	{
		this.SyncRows();
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		bool flag = false;
		foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair in this.rows)
		{
			if (keyValuePair.Value.gameObject.activeSelf)
			{
				this.RefreshLine(keyValuePair.Key, worldInventory, false);
				flag = (flag || DiscoveredResources.Instance.newDiscoveries.ContainsKey(keyValuePair.Key));
			}
		}
		this.clearNewButton.gameObject.SetActive(flag);
		this.seeAllLabel.SetText(string.Format(UI.RESOURCESCREEN.SEE_ALL, AllResourcesScreen.Instance.UniqueResourceRowCount()));
	}

	// Token: 0x06006F19 RID: 28441 RVA: 0x002A1C60 File Offset: 0x0029FE60
	private void RefreshLine(Tag tag, WorldInventory inventory, bool initialConfig = false)
	{
		Tag tag2 = tag;
		if (!AllResourcesScreen.Instance.units.ContainsKey(tag))
		{
			return;
		}
		if (!inventory.HasValidCount)
		{
			this.rows[tag].valueLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
		}
		else
		{
			switch (AllResourcesScreen.Instance.units[tag])
			{
			case GameUtil.MeasureUnit.mass:
			{
				float amount = inventory.GetAmount(tag2, false);
				if (this.rows[tag].CheckAmountChanged(amount, true))
				{
					this.rows[tag].valueLabel.SetText(GameUtil.GetFormattedMass(amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				}
				break;
			}
			case GameUtil.MeasureUnit.kcal:
			{
				float num = WorldResourceAmountTracker<RationTracker>.Get().CountAmountForItemWithID(tag.Name, ClusterManager.Instance.activeWorld.worldInventory, true);
				if (this.rows[tag].CheckAmountChanged(num, true))
				{
					this.rows[tag].valueLabel.SetText(GameUtil.GetFormattedCalories(num, GameUtil.TimeSlice.None, true));
				}
				break;
			}
			case GameUtil.MeasureUnit.quantity:
			{
				float amount2 = inventory.GetAmount(tag2, false);
				if (this.rows[tag].CheckAmountChanged(amount2, true))
				{
					this.rows[tag].valueLabel.SetText(GameUtil.GetFormattedUnits(amount2, GameUtil.TimeSlice.None, true, ""));
				}
				break;
			}
			}
		}
		this.rows[tag].pinToggle.onClick = delegate()
		{
			inventory.pinnedResources.Remove(tag);
			this.SyncRows();
		};
		this.rows[tag].notifyToggle.onClick = delegate()
		{
			inventory.notifyResources.Remove(tag);
			this.SyncRows();
		};
		this.rows[tag].newLabel.gameObject.SetActive(DiscoveredResources.Instance.newDiscoveries.ContainsKey(tag));
		this.rows[tag].newLabel.onClick = delegate()
		{
			AllResourcesScreen.Instance.Show(!AllResourcesScreen.Instance.gameObject.activeSelf);
		};
	}

	// Token: 0x06006F1A RID: 28442 RVA: 0x002A1EDF File Offset: 0x002A00DF
	public void Render1000ms(float dt)
	{
		if (this.headerButton != null && this.headerButton.CurrentState == 0)
		{
			return;
		}
		this.Refresh();
	}

	// Token: 0x04004BEA RID: 19434
	public GameObject linePrefab;

	// Token: 0x04004BEB RID: 19435
	public GameObject rowContainer;

	// Token: 0x04004BEC RID: 19436
	public MultiToggle headerButton;

	// Token: 0x04004BED RID: 19437
	public MultiToggle clearNewButton;

	// Token: 0x04004BEE RID: 19438
	public KButton clearAllButton;

	// Token: 0x04004BEF RID: 19439
	public MultiToggle seeAllButton;

	// Token: 0x04004BF0 RID: 19440
	private LocText seeAllLabel;

	// Token: 0x04004BF1 RID: 19441
	private QuickLayout rowContainerLayout;

	// Token: 0x04004BF2 RID: 19442
	private Dictionary<Tag, PinnedResourcesPanel.PinnedResourceRow> rows = new Dictionary<Tag, PinnedResourcesPanel.PinnedResourceRow>();

	// Token: 0x04004BF3 RID: 19443
	public static PinnedResourcesPanel Instance;

	// Token: 0x04004BF4 RID: 19444
	private int clickIdx;

	// Token: 0x0200203F RID: 8255
	public class PinnedResourceRow
	{
		// Token: 0x0600B8A5 RID: 47269 RVA: 0x003F65EA File Offset: 0x003F47EA
		public PinnedResourceRow(Tag tag)
		{
			this.Tag = tag;
			this.SortableNameWithoutLink = tag.ProperNameStripLink();
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x0600B8A6 RID: 47270 RVA: 0x003F6610 File Offset: 0x003F4810
		// (set) Token: 0x0600B8A7 RID: 47271 RVA: 0x003F6618 File Offset: 0x003F4818
		public Tag Tag { get; private set; }

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x0600B8A8 RID: 47272 RVA: 0x003F6621 File Offset: 0x003F4821
		// (set) Token: 0x0600B8A9 RID: 47273 RVA: 0x003F6629 File Offset: 0x003F4829
		public string SortableNameWithoutLink { get; private set; }

		// Token: 0x0600B8AA RID: 47274 RVA: 0x003F6632 File Offset: 0x003F4832
		public bool CheckAmountChanged(float newResourceAmount, bool updateIfTrue)
		{
			bool flag = newResourceAmount != this.oldResourceAmount;
			if (flag && updateIfTrue)
			{
				this.oldResourceAmount = newResourceAmount;
			}
			return flag;
		}

		// Token: 0x0400955E RID: 38238
		public GameObject gameObject;

		// Token: 0x0400955F RID: 38239
		public Image icon;

		// Token: 0x04009560 RID: 38240
		public LocText nameLabel;

		// Token: 0x04009561 RID: 38241
		public LocText valueLabel;

		// Token: 0x04009562 RID: 38242
		public MultiToggle pinToggle;

		// Token: 0x04009563 RID: 38243
		public MultiToggle notifyToggle;

		// Token: 0x04009564 RID: 38244
		public MultiToggle newLabel;

		// Token: 0x04009565 RID: 38245
		private float oldResourceAmount = -1f;
	}
}
