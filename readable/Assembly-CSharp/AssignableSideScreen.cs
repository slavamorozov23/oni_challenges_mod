using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000E16 RID: 3606
public class AssignableSideScreen : SideScreenContent
{
	// Token: 0x170007E5 RID: 2021
	// (get) Token: 0x06007249 RID: 29257 RVA: 0x002BAB43 File Offset: 0x002B8D43
	// (set) Token: 0x0600724A RID: 29258 RVA: 0x002BAB4B File Offset: 0x002B8D4B
	public Assignable targetAssignable { get; private set; }

	// Token: 0x0600724B RID: 29259 RVA: 0x002BAB54 File Offset: 0x002B8D54
	public override string GetTitle()
	{
		if (this.targetAssignable != null)
		{
			return string.Format(base.GetTitle(), this.targetAssignable.GetProperName());
		}
		return base.GetTitle();
	}

	// Token: 0x0600724C RID: 29260 RVA: 0x002BAB84 File Offset: 0x002B8D84
	protected override void OnSpawn()
	{
		base.OnSpawn();
		MultiToggle multiToggle = this.dupeSortingToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SortByName(true);
		}));
		MultiToggle multiToggle2 = this.generalSortingToggle;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			this.SortByAssignment(true);
		}));
		base.Subscribe(Game.Instance.gameObject, 875045922, new Action<object>(this.OnRefreshData));
	}

	// Token: 0x0600724D RID: 29261 RVA: 0x002BAC07 File Offset: 0x002B8E07
	private void OnRefreshData(object obj)
	{
		this.SetTarget(this.targetAssignable.gameObject);
	}

	// Token: 0x0600724E RID: 29262 RVA: 0x002BAC1C File Offset: 0x002B8E1C
	public override void ClearTarget()
	{
		if (this.targetAssignableSubscriptionHandle != -1 && this.targetAssignable != null)
		{
			this.targetAssignable.Unsubscribe(this.targetAssignableSubscriptionHandle);
			this.targetAssignableSubscriptionHandle = -1;
		}
		this.targetAssignable = null;
		Components.LiveMinionIdentities.OnAdd -= this.OnMinionIdentitiesChanged;
		Components.LiveMinionIdentities.OnRemove -= this.OnMinionIdentitiesChanged;
		base.ClearTarget();
	}

	// Token: 0x0600724F RID: 29263 RVA: 0x002BAC91 File Offset: 0x002B8E91
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Assignable>() != null && target.GetComponent<Assignable>().CanBeAssigned && target.GetComponent<AssignmentGroupController>() == null;
	}

	// Token: 0x06007250 RID: 29264 RVA: 0x002BACBC File Offset: 0x002B8EBC
	public override void SetTarget(GameObject target)
	{
		Components.LiveMinionIdentities.OnAdd += this.OnMinionIdentitiesChanged;
		Components.LiveMinionIdentities.OnRemove += this.OnMinionIdentitiesChanged;
		if (this.targetAssignableSubscriptionHandle != -1 && this.targetAssignable != null)
		{
			this.targetAssignable.Unsubscribe(this.targetAssignableSubscriptionHandle);
		}
		this.targetAssignable = target.GetComponent<Assignable>();
		if (this.targetAssignable == null)
		{
			global::Debug.LogError(string.Format("{0} selected has no Assignable component.", target.GetProperName()));
			return;
		}
		if (this.rowPool == null)
		{
			this.rowPool = new UIPool<AssignableSideScreenRow>(this.rowPrefab);
		}
		base.gameObject.SetActive(true);
		this.identityList = new List<MinionAssignablesProxy>(Components.MinionAssignablesProxy.Items);
		this.dupeSortingToggle.ChangeState(0);
		this.generalSortingToggle.ChangeState(0);
		this.activeSortToggle = null;
		this.activeSortFunction = null;
		if (!this.targetAssignable.CanBeAssigned)
		{
			this.HideScreen(true);
		}
		else
		{
			this.HideScreen(false);
		}
		this.targetAssignableSubscriptionHandle = this.targetAssignable.Subscribe(684616645, new Action<object>(this.OnAssigneeChanged));
		this.Refresh(this.identityList);
		this.SortByAssignment(false);
	}

	// Token: 0x06007251 RID: 29265 RVA: 0x002BADFF File Offset: 0x002B8FFF
	private void OnMinionIdentitiesChanged(MinionIdentity change)
	{
		this.identityList = new List<MinionAssignablesProxy>(Components.MinionAssignablesProxy.Items);
		this.Refresh(this.identityList);
	}

	// Token: 0x06007252 RID: 29266 RVA: 0x002BAE24 File Offset: 0x002B9024
	private void OnAssigneeChanged(object data = null)
	{
		foreach (KeyValuePair<IAssignableIdentity, AssignableSideScreenRow> keyValuePair in this.identityRowMap)
		{
			keyValuePair.Value.Refresh(null);
		}
	}

	// Token: 0x06007253 RID: 29267 RVA: 0x002BAE80 File Offset: 0x002B9080
	private void Refresh(List<MinionAssignablesProxy> identities)
	{
		this.ClearContent();
		this.currentOwnerText.text = string.Format(UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.UNASSIGNED, Array.Empty<object>());
		if (this.targetAssignable == null)
		{
			return;
		}
		if (this.targetAssignable.GetComponent<Equippable>() == null && !this.targetAssignable.HasTag(GameTags.NotRoomAssignable))
		{
			Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(this.targetAssignable.gameObject);
			if (roomOfGameObject != null)
			{
				RoomType roomType = roomOfGameObject.roomType;
				if (roomType.primary_constraint != null && !roomType.primary_constraint.building_criteria(this.targetAssignable.GetComponent<KPrefabID>()))
				{
					AssignableSideScreenRow freeElement = this.rowPool.GetFreeElement(this.rowGroup, true);
					freeElement.sideScreen = this;
					this.identityRowMap.Add(roomOfGameObject, freeElement);
					freeElement.SetContent(roomOfGameObject, new Action<IAssignableIdentity>(this.OnRowClicked), this);
					return;
				}
			}
		}
		if (this.targetAssignable.canBePublic)
		{
			AssignableSideScreenRow freeElement2 = this.rowPool.GetFreeElement(this.rowGroup, true);
			freeElement2.sideScreen = this;
			freeElement2.transform.SetAsFirstSibling();
			this.identityRowMap.Add(Game.Instance.assignmentManager.assignment_groups["public"], freeElement2);
			freeElement2.SetContent(Game.Instance.assignmentManager.assignment_groups["public"], new Action<IAssignableIdentity>(this.OnRowClicked), this);
		}
		foreach (MinionAssignablesProxy minionAssignablesProxy in identities)
		{
			AssignableSideScreenRow freeElement3 = this.rowPool.GetFreeElement(this.rowGroup, true);
			freeElement3.sideScreen = this;
			this.identityRowMap.Add(minionAssignablesProxy, freeElement3);
			freeElement3.SetContent(minionAssignablesProxy, new Action<IAssignableIdentity>(this.OnRowClicked), this);
		}
		this.ExecuteSort(this.activeSortFunction);
	}

	// Token: 0x06007254 RID: 29268 RVA: 0x002BB080 File Offset: 0x002B9280
	private void SortByName(bool reselect)
	{
		this.SelectSortToggle(this.dupeSortingToggle, reselect);
		this.ExecuteSort((IAssignableIdentity i1, IAssignableIdentity i2) => i1.GetProperName().CompareTo(i2.GetProperName()) * (this.sortReversed ? -1 : 1));
	}

	// Token: 0x06007255 RID: 29269 RVA: 0x002BB0A4 File Offset: 0x002B92A4
	private void SortByAssignment(bool reselect)
	{
		this.SelectSortToggle(this.generalSortingToggle, reselect);
		Comparison<IAssignableIdentity> sortFunction = delegate(IAssignableIdentity i1, IAssignableIdentity i2)
		{
			int num = this.targetAssignable.CanAssignTo(i1).CompareTo(this.targetAssignable.CanAssignTo(i2));
			if (num != 0)
			{
				return num * -1;
			}
			num = this.identityRowMap[i1].currentState.CompareTo(this.identityRowMap[i2].currentState);
			if (num != 0)
			{
				return num * (this.sortReversed ? -1 : 1);
			}
			return i1.GetProperName().CompareTo(i2.GetProperName());
		};
		this.ExecuteSort(sortFunction);
	}

	// Token: 0x06007256 RID: 29270 RVA: 0x002BB0D4 File Offset: 0x002B92D4
	private void SelectSortToggle(MultiToggle toggle, bool reselect)
	{
		this.dupeSortingToggle.ChangeState(0);
		this.generalSortingToggle.ChangeState(0);
		if (toggle != null)
		{
			if (reselect && this.activeSortToggle == toggle)
			{
				this.sortReversed = !this.sortReversed;
			}
			this.activeSortToggle = toggle;
		}
		this.activeSortToggle.ChangeState(this.sortReversed ? 2 : 1);
	}

	// Token: 0x06007257 RID: 29271 RVA: 0x002BB140 File Offset: 0x002B9340
	private void ExecuteSort(Comparison<IAssignableIdentity> sortFunction)
	{
		if (sortFunction != null)
		{
			List<IAssignableIdentity> list = new List<IAssignableIdentity>(this.identityRowMap.Keys);
			list.Sort(sortFunction);
			for (int i = 0; i < list.Count; i++)
			{
				this.identityRowMap[list[i]].transform.SetSiblingIndex(i);
			}
			this.activeSortFunction = sortFunction;
		}
	}

	// Token: 0x06007258 RID: 29272 RVA: 0x002BB1A0 File Offset: 0x002B93A0
	private void ClearContent()
	{
		if (this.rowPool != null)
		{
			this.rowPool.DestroyAll();
		}
		foreach (KeyValuePair<IAssignableIdentity, AssignableSideScreenRow> keyValuePair in this.identityRowMap)
		{
			keyValuePair.Value.targetIdentity = null;
		}
		this.identityRowMap.Clear();
	}

	// Token: 0x06007259 RID: 29273 RVA: 0x002BB218 File Offset: 0x002B9418
	private void HideScreen(bool hide)
	{
		if (hide)
		{
			base.transform.localScale = Vector3.zero;
			return;
		}
		if (base.transform.localScale != Vector3.one)
		{
			base.transform.localScale = Vector3.one;
		}
	}

	// Token: 0x0600725A RID: 29274 RVA: 0x002BB255 File Offset: 0x002B9455
	private void OnRowClicked(IAssignableIdentity identity)
	{
		if (this.targetAssignable.assignee != identity)
		{
			this.ChangeAssignment(identity);
			return;
		}
		if (this.CanDeselect(identity))
		{
			this.ChangeAssignment(null);
		}
	}

	// Token: 0x0600725B RID: 29275 RVA: 0x002BB27D File Offset: 0x002B947D
	private bool CanDeselect(IAssignableIdentity identity)
	{
		return identity is MinionAssignablesProxy;
	}

	// Token: 0x0600725C RID: 29276 RVA: 0x002BB288 File Offset: 0x002B9488
	private void ChangeAssignment(IAssignableIdentity new_identity)
	{
		this.targetAssignable.Unassign();
		if (!new_identity.IsNullOrDestroyed())
		{
			this.targetAssignable.Assign(new_identity);
		}
	}

	// Token: 0x0600725D RID: 29277 RVA: 0x002BB2A9 File Offset: 0x002B94A9
	private void OnValidStateChanged(bool state)
	{
		if (base.gameObject.activeInHierarchy)
		{
			this.Refresh(this.identityList);
		}
	}

	// Token: 0x04004EF5 RID: 20213
	[SerializeField]
	private AssignableSideScreenRow rowPrefab;

	// Token: 0x04004EF6 RID: 20214
	[SerializeField]
	private GameObject rowGroup;

	// Token: 0x04004EF7 RID: 20215
	[SerializeField]
	private LocText currentOwnerText;

	// Token: 0x04004EF8 RID: 20216
	[SerializeField]
	private MultiToggle dupeSortingToggle;

	// Token: 0x04004EF9 RID: 20217
	[SerializeField]
	private MultiToggle generalSortingToggle;

	// Token: 0x04004EFA RID: 20218
	private MultiToggle activeSortToggle;

	// Token: 0x04004EFB RID: 20219
	private Comparison<IAssignableIdentity> activeSortFunction;

	// Token: 0x04004EFC RID: 20220
	private bool sortReversed;

	// Token: 0x04004EFD RID: 20221
	private int targetAssignableSubscriptionHandle = -1;

	// Token: 0x04004EFF RID: 20223
	private UIPool<AssignableSideScreenRow> rowPool;

	// Token: 0x04004F00 RID: 20224
	private Dictionary<IAssignableIdentity, AssignableSideScreenRow> identityRowMap = new Dictionary<IAssignableIdentity, AssignableSideScreenRow>();

	// Token: 0x04004F01 RID: 20225
	private List<MinionAssignablesProxy> identityList = new List<MinionAssignablesProxy>();
}
