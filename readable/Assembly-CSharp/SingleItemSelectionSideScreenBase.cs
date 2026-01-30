using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E7C RID: 3708
public abstract class SingleItemSelectionSideScreenBase : SideScreenContent
{
	// Token: 0x060075FC RID: 30204 RVA: 0x002D0B72 File Offset: 0x002CED72
	private static bool TagContainsSearchWord(Tag tag, string search)
	{
		return string.IsNullOrEmpty(search) || tag.ProperNameStripLink().ToUpper().Contains(search.ToUpper());
	}

	// Token: 0x1700082D RID: 2093
	// (get) Token: 0x060075FE RID: 30206 RVA: 0x002D0B9D File Offset: 0x002CED9D
	// (set) Token: 0x060075FD RID: 30205 RVA: 0x002D0B94 File Offset: 0x002CED94
	private protected SingleItemSelectionRow CurrentSelectedItem { protected get; private set; }

	// Token: 0x060075FF RID: 30207 RVA: 0x002D0BA8 File Offset: 0x002CEDA8
	protected override void OnPrefabInit()
	{
		if (this.searchbar != null)
		{
			this.searchbar.EditingStateChanged = new Action<bool>(this.OnSearchbarEditStateChanged);
			this.searchbar.ValueChanged = new Action<string>(this.OnSearchBarValueChanged);
			this.activateOnSpawn = true;
		}
		base.OnPrefabInit();
	}

	// Token: 0x06007600 RID: 30208 RVA: 0x002D0C00 File Offset: 0x002CEE00
	protected virtual void OnSearchbarEditStateChanged(bool isEditing)
	{
		base.isEditing = isEditing;
	}

	// Token: 0x06007601 RID: 30209 RVA: 0x002D0C0C File Offset: 0x002CEE0C
	protected virtual void OnSearchBarValueChanged(string value)
	{
		foreach (Tag tag in this.categories.Keys)
		{
			SingleItemSelectionSideScreenBase.Category category = this.categories[tag];
			bool flag = SingleItemSelectionSideScreenBase.TagContainsSearchWord(tag, value);
			int num = category.FilterItemsBySearch(flag ? null : value);
			category.SetUnfoldedState((num > 0) ? SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded : SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Folded);
			category.SetVisibilityState(flag || num > 0);
		}
	}

	// Token: 0x06007602 RID: 30210 RVA: 0x002D0C9C File Offset: 0x002CEE9C
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return base.GetSortKey();
	}

	// Token: 0x06007603 RID: 30211 RVA: 0x002D0CB2 File Offset: 0x002CEEB2
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

	// Token: 0x06007604 RID: 30212 RVA: 0x002D0CCC File Offset: 0x002CEECC
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

	// Token: 0x06007605 RID: 30213 RVA: 0x002D0CE8 File Offset: 0x002CEEE8
	public virtual void SetData(Dictionary<Tag, HashSet<Tag>> data)
	{
		this.ProhibitAllCategories();
		foreach (Tag tag in data.Keys)
		{
			ICollection<Tag> items = data[tag];
			this.CreateCategoryWithItems(tag, items);
		}
		this.SortAll();
		if (this.searchbar != null && !string.IsNullOrEmpty(this.searchbar.CurrentSearchValue))
		{
			this.searchbar.ClearSearch();
		}
	}

	// Token: 0x06007606 RID: 30214 RVA: 0x002D0D7C File Offset: 0x002CEF7C
	public virtual SingleItemSelectionSideScreenBase.Category CreateCategoryWithItems(Tag categoryTag, ICollection<Tag> items)
	{
		SingleItemSelectionSideScreenBase.Category orCreateEmptyCategory = this.GetOrCreateEmptyCategory(categoryTag);
		if (!orCreateEmptyCategory.InitializeItemList(items.Count))
		{
			orCreateEmptyCategory.RemoveAllItems();
		}
		foreach (Tag itemTag in items)
		{
			SingleItemSelectionRow orCreateItemRow = this.GetOrCreateItemRow(itemTag);
			orCreateEmptyCategory.AddItem(orCreateItemRow);
		}
		return orCreateEmptyCategory;
	}

	// Token: 0x06007607 RID: 30215 RVA: 0x002D0DEC File Offset: 0x002CEFEC
	public virtual SingleItemSelectionSideScreenBase.Category GetOrCreateEmptyCategory(Tag categoryTag)
	{
		this.original_CategoryRow.gameObject.SetActive(false);
		SingleItemSelectionSideScreenBase.Category category = null;
		if (!this.categories.TryGetValue(categoryTag, out category))
		{
			HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.original_CategoryRow.gameObject, this.original_CategoryRow.transform.parent.gameObject, false);
			hierarchyReferences.gameObject.SetActive(true);
			category = new SingleItemSelectionSideScreenBase.Category(hierarchyReferences, categoryTag);
			category.ItemRemoved = new Action<SingleItemSelectionRow>(this.RecycleItemRow);
			SingleItemSelectionSideScreenBase.Category category2 = category;
			category2.ToggleClicked = (Action<SingleItemSelectionSideScreenBase.Category>)Delegate.Combine(category2.ToggleClicked, new Action<SingleItemSelectionSideScreenBase.Category>(this.CategoryToggleClicked));
			this.categories.Add(categoryTag, category);
		}
		else
		{
			category.SetProihibedState(false);
			category.SetVisibilityState(true);
		}
		return category;
	}

	// Token: 0x06007608 RID: 30216 RVA: 0x002D0EA8 File Offset: 0x002CF0A8
	public virtual SingleItemSelectionRow GetOrCreateItemRow(Tag itemTag)
	{
		this.original_ItemRow.gameObject.SetActive(false);
		SingleItemSelectionRow singleItemSelectionRow = null;
		if (!this.pooledRows.TryGetValue(itemTag, out singleItemSelectionRow))
		{
			singleItemSelectionRow = Util.KInstantiateUI<SingleItemSelectionRow>(this.original_ItemRow.gameObject, this.original_ItemRow.transform.parent.gameObject, false);
			UnityEngine.Object @object = singleItemSelectionRow;
			string str = "Item-";
			Tag tag = itemTag;
			@object.name = str + tag.ToString();
		}
		else
		{
			this.pooledRows.Remove(itemTag);
		}
		singleItemSelectionRow.gameObject.SetActive(true);
		singleItemSelectionRow.SetTag(itemTag);
		singleItemSelectionRow.Clicked = new Action<SingleItemSelectionRow>(this.ItemRowClicked);
		singleItemSelectionRow.SetVisibleState(true);
		return singleItemSelectionRow;
	}

	// Token: 0x06007609 RID: 30217 RVA: 0x002D0F5C File Offset: 0x002CF15C
	public SingleItemSelectionSideScreenBase.Category GetCategoryWithItem(Tag itemTag, bool includeNotVisibleCategories = false)
	{
		foreach (SingleItemSelectionSideScreenBase.Category category in this.categories.Values)
		{
			if ((includeNotVisibleCategories || category.IsVisible) && category.GetItem(itemTag) != null)
			{
				return category;
			}
		}
		return null;
	}

	// Token: 0x0600760A RID: 30218 RVA: 0x002D0FD0 File Offset: 0x002CF1D0
	public virtual void SetSelectedItem(SingleItemSelectionRow itemRow)
	{
		if (this.CurrentSelectedItem != null)
		{
			this.CurrentSelectedItem.SetSelected(false);
		}
		this.CurrentSelectedItem = itemRow;
		if (itemRow != null)
		{
			itemRow.SetSelected(true);
		}
	}

	// Token: 0x0600760B RID: 30219 RVA: 0x002D1004 File Offset: 0x002CF204
	public virtual bool SetSelectedItem(Tag itemTag)
	{
		foreach (Tag key in this.categories.Keys)
		{
			SingleItemSelectionSideScreenBase.Category category = this.categories[key];
			if (category.IsVisible)
			{
				SingleItemSelectionRow item = category.GetItem(itemTag);
				if (item != null)
				{
					this.SetSelectedItem(item);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600760C RID: 30220 RVA: 0x002D108C File Offset: 0x002CF28C
	public virtual void ItemRowClicked(SingleItemSelectionRow rowClicked)
	{
		this.SetSelectedItem(rowClicked);
	}

	// Token: 0x0600760D RID: 30221 RVA: 0x002D1095 File Offset: 0x002CF295
	public virtual void CategoryToggleClicked(SingleItemSelectionSideScreenBase.Category categoryClicked)
	{
		categoryClicked.ToggleUnfoldedState();
	}

	// Token: 0x0600760E RID: 30222 RVA: 0x002D10A0 File Offset: 0x002CF2A0
	private void RecycleItemRow(SingleItemSelectionRow row)
	{
		if (this.pooledRows.ContainsKey(row.tag))
		{
			global::Debug.LogError(string.Format("Recycling an item row with tag {0} that was already in the recycle pool", row.tag));
		}
		if (this.CurrentSelectedItem == row)
		{
			this.SetSelectedItem(null);
		}
		row.Clicked = null;
		row.SetSelected(false);
		row.transform.SetParent(this.original_ItemRow.transform.parent.parent);
		row.gameObject.SetActive(false);
		this.pooledRows.Add(row.tag, row);
	}

	// Token: 0x0600760F RID: 30223 RVA: 0x002D113C File Offset: 0x002CF33C
	private void ProhibitAllCategories()
	{
		foreach (SingleItemSelectionSideScreenBase.Category category in this.categories.Values)
		{
			category.SetProihibedState(true);
		}
	}

	// Token: 0x06007610 RID: 30224 RVA: 0x002D1194 File Offset: 0x002CF394
	public virtual void SortAll()
	{
		foreach (SingleItemSelectionSideScreenBase.Category category in this.categories.Values)
		{
			if (category.IsVisible)
			{
				category.Sort();
				category.SendToLastSibiling();
			}
		}
	}

	// Token: 0x040051AD RID: 20909
	[Space]
	[Header("Settings")]
	[SerializeField]
	private SearchBar searchbar;

	// Token: 0x040051AE RID: 20910
	[SerializeField]
	protected HierarchyReferences original_CategoryRow;

	// Token: 0x040051AF RID: 20911
	[SerializeField]
	protected SingleItemSelectionRow original_ItemRow;

	// Token: 0x040051B0 RID: 20912
	protected SortedDictionary<Tag, SingleItemSelectionSideScreenBase.Category> categories = new SortedDictionary<Tag, SingleItemSelectionSideScreenBase.Category>(SingleItemSelectionSideScreenBase.categoryComparer);

	// Token: 0x040051B1 RID: 20913
	private Dictionary<Tag, SingleItemSelectionRow> pooledRows = new Dictionary<Tag, SingleItemSelectionRow>();

	// Token: 0x040051B2 RID: 20914
	private static TagNameComparer categoryComparer = new TagNameComparer(GameTags.Void);

	// Token: 0x040051B3 RID: 20915
	private static SingleItemSelectionSideScreenBase.ItemComparer itemRowComparer = new SingleItemSelectionSideScreenBase.ItemComparer(GameTags.Void);

	// Token: 0x020020E9 RID: 8425
	public class ItemComparer : IComparer<SingleItemSelectionRow>
	{
		// Token: 0x0600BAAD RID: 47789 RVA: 0x003FB9B7 File Offset: 0x003F9BB7
		public ItemComparer()
		{
		}

		// Token: 0x0600BAAE RID: 47790 RVA: 0x003FB9BF File Offset: 0x003F9BBF
		public ItemComparer(Tag firstTag)
		{
			this.firstTag = firstTag;
		}

		// Token: 0x0600BAAF RID: 47791 RVA: 0x003FB9D0 File Offset: 0x003F9BD0
		public int Compare(SingleItemSelectionRow x, SingleItemSelectionRow y)
		{
			if (x == y)
			{
				return 0;
			}
			if (this.firstTag.IsValid)
			{
				if (x.tag == this.firstTag && y.tag != this.firstTag)
				{
					return 1;
				}
				if (x.tag != this.firstTag && y.tag == this.firstTag)
				{
					return -1;
				}
			}
			return x.tag.ProperNameStripLink().CompareTo(y.tag.ProperNameStripLink());
		}

		// Token: 0x0400977C RID: 38780
		private Tag firstTag;
	}

	// Token: 0x020020EA RID: 8426
	public class Category
	{
		// Token: 0x0600BAB0 RID: 47792 RVA: 0x003FBA60 File Offset: 0x003F9C60
		public virtual void ToggleUnfoldedState()
		{
			SingleItemSelectionSideScreenBase.Category.UnfoldedStates currentState = (SingleItemSelectionSideScreenBase.Category.UnfoldedStates)this.toggle.CurrentState;
			if (currentState == SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Folded)
			{
				this.SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
				return;
			}
			if (currentState != SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded)
			{
				return;
			}
			this.SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Folded);
		}

		// Token: 0x0600BAB1 RID: 47793 RVA: 0x003FBA90 File Offset: 0x003F9C90
		public virtual void SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates new_state)
		{
			this.toggle.ChangeState((int)new_state);
			this.entries.gameObject.SetActive(new_state == SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
		}

		// Token: 0x0600BAB2 RID: 47794 RVA: 0x003FBAB2 File Offset: 0x003F9CB2
		public virtual void SetTitle(string text)
		{
			this.title.text = text;
		}

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x0600BAB4 RID: 47796 RVA: 0x003FBAC9 File Offset: 0x003F9CC9
		// (set) Token: 0x0600BAB3 RID: 47795 RVA: 0x003FBAC0 File Offset: 0x003F9CC0
		public Tag CategoryTag { get; protected set; }

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x0600BAB6 RID: 47798 RVA: 0x003FBADA File Offset: 0x003F9CDA
		// (set) Token: 0x0600BAB5 RID: 47797 RVA: 0x003FBAD1 File Offset: 0x003F9CD1
		public bool IsProhibited { get; protected set; }

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x0600BAB7 RID: 47799 RVA: 0x003FBAE2 File Offset: 0x003F9CE2
		public bool IsVisible
		{
			get
			{
				return this.hierarchyReferences != null && this.hierarchyReferences.gameObject.activeSelf;
			}
		}

		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x0600BAB8 RID: 47800 RVA: 0x003FBB04 File Offset: 0x003F9D04
		protected RectTransform entries
		{
			get
			{
				return this.hierarchyReferences.GetReference<RectTransform>("Entries");
			}
		}

		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x0600BAB9 RID: 47801 RVA: 0x003FBB16 File Offset: 0x003F9D16
		protected LocText title
		{
			get
			{
				return this.hierarchyReferences.GetReference<LocText>("Label");
			}
		}

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x0600BABA RID: 47802 RVA: 0x003FBB28 File Offset: 0x003F9D28
		protected MultiToggle toggle
		{
			get
			{
				return this.hierarchyReferences.GetReference<MultiToggle>("Toggle");
			}
		}

		// Token: 0x0600BABB RID: 47803 RVA: 0x003FBB3A File Offset: 0x003F9D3A
		public Category(HierarchyReferences references, Tag categoryTag)
		{
			this.CategoryTag = categoryTag;
			this.hierarchyReferences = references;
			this.toggle.onClick = new System.Action(this.OnToggleClicked);
			this.SetTitle(categoryTag.ProperName());
		}

		// Token: 0x0600BABC RID: 47804 RVA: 0x003FBB74 File Offset: 0x003F9D74
		public virtual void OnToggleClicked()
		{
			Action<SingleItemSelectionSideScreenBase.Category> toggleClicked = this.ToggleClicked;
			if (toggleClicked == null)
			{
				return;
			}
			toggleClicked(this);
		}

		// Token: 0x0600BABD RID: 47805 RVA: 0x003FBB88 File Offset: 0x003F9D88
		public virtual void AddItems(SingleItemSelectionRow[] _items)
		{
			if (this.items == null)
			{
				this.items = new List<SingleItemSelectionRow>(_items);
				return;
			}
			for (int i = 0; i < _items.Length; i++)
			{
				if (!this.items.Contains(_items[i]))
				{
					_items[i].transform.SetParent(this.entries, false);
					this.items.Add(_items[i]);
				}
			}
		}

		// Token: 0x0600BABE RID: 47806 RVA: 0x003FBBEA File Offset: 0x003F9DEA
		public virtual void AddItem(SingleItemSelectionRow item)
		{
			if (this.items == null)
			{
				this.items = new List<SingleItemSelectionRow>();
			}
			item.transform.SetParent(this.entries, false);
			this.items.Add(item);
		}

		// Token: 0x0600BABF RID: 47807 RVA: 0x003FBC1D File Offset: 0x003F9E1D
		public virtual bool InitializeItemList(int size)
		{
			if (this.items == null)
			{
				this.items = new List<SingleItemSelectionRow>(size);
				return true;
			}
			return false;
		}

		// Token: 0x0600BAC0 RID: 47808 RVA: 0x003FBC36 File Offset: 0x003F9E36
		public virtual void SetVisibilityState(bool isVisible)
		{
			this.hierarchyReferences.gameObject.SetActive(isVisible && !this.IsProhibited);
		}

		// Token: 0x0600BAC1 RID: 47809 RVA: 0x003FBC58 File Offset: 0x003F9E58
		public virtual void RemoveAllItems()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				SingleItemSelectionRow obj = this.items[i];
				Action<SingleItemSelectionRow> itemRemoved = this.ItemRemoved;
				if (itemRemoved != null)
				{
					itemRemoved(obj);
				}
			}
			this.items.Clear();
			this.items = null;
		}

		// Token: 0x0600BAC2 RID: 47810 RVA: 0x003FBCAC File Offset: 0x003F9EAC
		public virtual SingleItemSelectionRow RemoveItem(Tag itemTag)
		{
			if (this.items != null)
			{
				SingleItemSelectionRow singleItemSelectionRow = this.items.Find((SingleItemSelectionRow row) => row.tag == itemTag);
				if (singleItemSelectionRow != null)
				{
					Action<SingleItemSelectionRow> itemRemoved = this.ItemRemoved;
					if (itemRemoved != null)
					{
						itemRemoved(singleItemSelectionRow);
					}
					return singleItemSelectionRow;
				}
			}
			return null;
		}

		// Token: 0x0600BAC3 RID: 47811 RVA: 0x003FBD04 File Offset: 0x003F9F04
		public virtual bool RemoveItem(SingleItemSelectionRow itemRow)
		{
			if (this.items != null && this.items.Remove(itemRow))
			{
				Action<SingleItemSelectionRow> itemRemoved = this.ItemRemoved;
				if (itemRemoved != null)
				{
					itemRemoved(itemRow);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600BAC4 RID: 47812 RVA: 0x003FBD34 File Offset: 0x003F9F34
		public SingleItemSelectionRow GetItem(Tag itemTag)
		{
			if (this.items == null)
			{
				return null;
			}
			return this.items.Find((SingleItemSelectionRow row) => row.tag == itemTag);
		}

		// Token: 0x0600BAC5 RID: 47813 RVA: 0x003FBD70 File Offset: 0x003F9F70
		public int FilterItemsBySearch(string searchValue)
		{
			int num = 0;
			if (this.items != null)
			{
				foreach (SingleItemSelectionRow singleItemSelectionRow in this.items)
				{
					bool flag = SingleItemSelectionSideScreenBase.TagContainsSearchWord(singleItemSelectionRow.tag, searchValue);
					singleItemSelectionRow.SetVisibleState(flag);
					if (flag)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600BAC6 RID: 47814 RVA: 0x003FBDE0 File Offset: 0x003F9FE0
		public void Sort()
		{
			if (this.items != null)
			{
				this.items.Sort(SingleItemSelectionSideScreenBase.itemRowComparer);
				foreach (SingleItemSelectionRow singleItemSelectionRow in this.items)
				{
					singleItemSelectionRow.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x0600BAC7 RID: 47815 RVA: 0x003FBE50 File Offset: 0x003FA050
		public void SendToLastSibiling()
		{
			this.hierarchyReferences.transform.SetAsLastSibling();
		}

		// Token: 0x0600BAC8 RID: 47816 RVA: 0x003FBE62 File Offset: 0x003FA062
		public void SetProihibedState(bool isPohibited)
		{
			this.IsProhibited = isPohibited;
			if (this.IsVisible && isPohibited)
			{
				this.SetVisibilityState(false);
			}
		}

		// Token: 0x0400977D RID: 38781
		public Action<SingleItemSelectionRow> ItemRemoved;

		// Token: 0x0400977E RID: 38782
		public Action<SingleItemSelectionSideScreenBase.Category> ToggleClicked;

		// Token: 0x04009781 RID: 38785
		protected HierarchyReferences hierarchyReferences;

		// Token: 0x04009782 RID: 38786
		protected List<SingleItemSelectionRow> items;

		// Token: 0x02002A84 RID: 10884
		public enum UnfoldedStates
		{
			// Token: 0x0400BB93 RID: 48019
			Folded,
			// Token: 0x0400BB94 RID: 48020
			Unfolded
		}
	}
}
