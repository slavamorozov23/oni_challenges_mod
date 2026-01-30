using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CAF RID: 3247
public class BuildingChoresPanel : TargetPanel
{
	// Token: 0x06006373 RID: 25459 RVA: 0x002503FC File Offset: 0x0024E5FC
	public override bool IsValidForTarget(GameObject target)
	{
		KPrefabID component = target.GetComponent<KPrefabID>();
		return component != null && component.HasTag(GameTags.HasChores) && !component.HasTag(GameTags.BaseMinion);
	}

	// Token: 0x06006374 RID: 25460 RVA: 0x00250436 File Offset: 0x0024E636
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreGroup = Util.KInstantiateUI<HierarchyReferences>(this.choreGroupPrefab, base.gameObject, false);
		this.choreGroup.gameObject.SetActive(true);
	}

	// Token: 0x06006375 RID: 25461 RVA: 0x00250467 File Offset: 0x0024E667
	private void Update()
	{
		this.Refresh();
	}

	// Token: 0x06006376 RID: 25462 RVA: 0x0025046F File Offset: 0x0024E66F
	protected override void OnSelectTarget(GameObject target)
	{
		base.OnSelectTarget(target);
		this.Refresh();
	}

	// Token: 0x06006377 RID: 25463 RVA: 0x0025047E File Offset: 0x0024E67E
	public override void OnDeselectTarget(GameObject target)
	{
		base.OnDeselectTarget(target);
	}

	// Token: 0x06006378 RID: 25464 RVA: 0x00250487 File Offset: 0x0024E687
	private void Refresh()
	{
		this.RefreshDetails();
	}

	// Token: 0x06006379 RID: 25465 RVA: 0x00250490 File Offset: 0x0024E690
	private void RefreshDetails()
	{
		int myParentWorldId = this.selectedTarget.GetMyParentWorldId();
		List<Chore> list = null;
		GlobalChoreProvider.Instance.choreWorldMap.TryGetValue(myParentWorldId, out list);
		int num = 0;
		while (list != null && num < list.Count)
		{
			Chore chore = list[num];
			if (!chore.isNull && chore.gameObject == this.selectedTarget)
			{
				this.AddChoreEntry(chore);
			}
			num++;
		}
		List<FetchChore> list2 = null;
		GlobalChoreProvider.Instance.fetchMap.TryGetValue(myParentWorldId, out list2);
		int num2 = 0;
		while (list2 != null && num2 < list2.Count)
		{
			FetchChore fetchChore = list2[num2];
			if (!fetchChore.isNull && fetchChore.gameObject == this.selectedTarget)
			{
				this.AddChoreEntry(fetchChore);
			}
			num2++;
		}
		for (int i = this.activeDupeEntries; i < this.dupeEntries.Count; i++)
		{
			this.dupeEntries[i].gameObject.SetActive(false);
		}
		this.activeDupeEntries = 0;
		for (int j = this.activeChoreEntries; j < this.choreEntries.Count; j++)
		{
			this.choreEntries[j].gameObject.SetActive(false);
		}
		this.activeChoreEntries = 0;
	}

	// Token: 0x0600637A RID: 25466 RVA: 0x002505D8 File Offset: 0x0024E7D8
	private void AddChoreEntry(Chore chore)
	{
		HierarchyReferences choreEntry = this.GetChoreEntry(GameUtil.GetChoreName(chore, null), chore.choreType, this.choreGroup.GetReference<RectTransform>("EntriesContainer"));
		FetchChore fetchChore = chore as FetchChore;
		ListPool<Chore.Precondition.Context, BuildingChoresPanel>.PooledList pooledList = ListPool<Chore.Precondition.Context, BuildingChoresPanel>.Allocate();
		List<GameObject> list = new List<GameObject>();
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			list.Add(minionIdentity.gameObject);
		}
		foreach (RobotAi.Instance instance in Components.LiveRobotsIdentities.Items)
		{
			list.Add(instance.gameObject);
		}
		foreach (GameObject gameObject in list)
		{
			pooledList.Clear();
			ChoreConsumer component = gameObject.GetComponent<ChoreConsumer>();
			Chore.Precondition.Context context = default(Chore.Precondition.Context);
			ChoreConsumer.PreconditionSnapshot lastPreconditionSnapshot = component.GetLastPreconditionSnapshot();
			if (lastPreconditionSnapshot.doFailedContextsNeedSorting)
			{
				lastPreconditionSnapshot.failedContexts.Sort();
				lastPreconditionSnapshot.doFailedContextsNeedSorting = false;
			}
			pooledList.AddRange(lastPreconditionSnapshot.failedContexts);
			pooledList.AddRange(lastPreconditionSnapshot.succeededContexts);
			int num = -1;
			int num2 = 0;
			for (int i = pooledList.Count - 1; i >= 0; i--)
			{
				if (!(pooledList[i].chore.driver != null) || !(pooledList[i].chore.driver != component.choreDriver))
				{
					bool flag = pooledList[i].IsPotentialSuccess();
					if (flag)
					{
						num2++;
					}
					FetchAreaChore fetchAreaChore = pooledList[i].chore as FetchAreaChore;
					if (pooledList[i].chore == chore || (fetchChore != null && fetchAreaChore != null && fetchAreaChore.smi.SameDestination(fetchChore)))
					{
						num = (flag ? num2 : int.MaxValue);
						context = pooledList[i];
						break;
					}
				}
			}
			if (num >= 0)
			{
				this.DupeEntryDatas.Add(new BuildingChoresPanel.DupeEntryData
				{
					consumer = component,
					context = context,
					personalPriority = component.GetPersonalPriority(chore.choreType),
					rank = num
				});
			}
		}
		pooledList.Recycle();
		this.DupeEntryDatas.Sort();
		foreach (BuildingChoresPanel.DupeEntryData data in this.DupeEntryDatas)
		{
			this.GetDupeEntry(data, choreEntry.GetReference<RectTransform>("DupeContainer"));
		}
		this.DupeEntryDatas.Clear();
	}

	// Token: 0x0600637B RID: 25467 RVA: 0x002508FC File Offset: 0x0024EAFC
	private HierarchyReferences GetChoreEntry(string label, ChoreType choreType, RectTransform parent)
	{
		HierarchyReferences hierarchyReferences;
		if (this.activeChoreEntries >= this.choreEntries.Count)
		{
			hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.chorePrefab, parent.gameObject, false);
			this.choreEntries.Add(hierarchyReferences);
		}
		else
		{
			hierarchyReferences = this.choreEntries[this.activeChoreEntries];
			hierarchyReferences.transform.SetParent(parent);
			hierarchyReferences.transform.SetAsLastSibling();
		}
		this.activeChoreEntries++;
		hierarchyReferences.GetReference<LocText>("ChoreLabel").text = label;
		hierarchyReferences.GetReference<LocText>("ChoreSubLabel").text = GameUtil.ChoreGroupsForChoreType(choreType);
		Image reference = hierarchyReferences.GetReference<Image>("Icon");
		if (choreType.groups.Length != 0)
		{
			Sprite sprite = Assets.GetSprite(choreType.groups[0].sprite);
			reference.sprite = sprite;
			reference.gameObject.SetActive(true);
			reference.GetComponent<ToolTip>().toolTip = string.Format(UI.DETAILTABS.BUILDING_CHORES.CHORE_TYPE_TOOLTIP, choreType.groups[0].Name);
		}
		else
		{
			reference.gameObject.SetActive(false);
		}
		Image reference2 = hierarchyReferences.GetReference<Image>("Icon2");
		if (choreType.groups.Length > 1)
		{
			Sprite sprite2 = Assets.GetSprite(choreType.groups[1].sprite);
			reference2.sprite = sprite2;
			reference2.gameObject.SetActive(true);
			reference2.GetComponent<ToolTip>().toolTip = string.Format(UI.DETAILTABS.BUILDING_CHORES.CHORE_TYPE_TOOLTIP, choreType.groups[1].Name);
		}
		else
		{
			reference2.gameObject.SetActive(false);
		}
		hierarchyReferences.gameObject.SetActive(true);
		return hierarchyReferences;
	}

	// Token: 0x0600637C RID: 25468 RVA: 0x00250A98 File Offset: 0x0024EC98
	private BuildingChoresPanelDupeRow GetDupeEntry(BuildingChoresPanel.DupeEntryData data, RectTransform parent)
	{
		BuildingChoresPanelDupeRow buildingChoresPanelDupeRow;
		if (this.activeDupeEntries >= this.dupeEntries.Count)
		{
			buildingChoresPanelDupeRow = Util.KInstantiateUI<BuildingChoresPanelDupeRow>(this.dupePrefab.gameObject, parent.gameObject, false);
			this.dupeEntries.Add(buildingChoresPanelDupeRow);
		}
		else
		{
			buildingChoresPanelDupeRow = this.dupeEntries[this.activeDupeEntries];
			buildingChoresPanelDupeRow.transform.SetParent(parent);
			buildingChoresPanelDupeRow.transform.SetAsLastSibling();
		}
		this.activeDupeEntries++;
		buildingChoresPanelDupeRow.Init(data);
		buildingChoresPanelDupeRow.gameObject.SetActive(true);
		return buildingChoresPanelDupeRow;
	}

	// Token: 0x04004396 RID: 17302
	public GameObject choreGroupPrefab;

	// Token: 0x04004397 RID: 17303
	public GameObject chorePrefab;

	// Token: 0x04004398 RID: 17304
	public BuildingChoresPanelDupeRow dupePrefab;

	// Token: 0x04004399 RID: 17305
	private GameObject detailsPanel;

	// Token: 0x0400439A RID: 17306
	private DetailsPanelDrawer drawer;

	// Token: 0x0400439B RID: 17307
	private HierarchyReferences choreGroup;

	// Token: 0x0400439C RID: 17308
	private List<HierarchyReferences> choreEntries = new List<HierarchyReferences>();

	// Token: 0x0400439D RID: 17309
	private int activeChoreEntries;

	// Token: 0x0400439E RID: 17310
	private List<BuildingChoresPanelDupeRow> dupeEntries = new List<BuildingChoresPanelDupeRow>();

	// Token: 0x0400439F RID: 17311
	private int activeDupeEntries;

	// Token: 0x040043A0 RID: 17312
	private List<BuildingChoresPanel.DupeEntryData> DupeEntryDatas = new List<BuildingChoresPanel.DupeEntryData>();

	// Token: 0x02001EDA RID: 7898
	public class DupeEntryData : IComparable<BuildingChoresPanel.DupeEntryData>
	{
		// Token: 0x0600B4D5 RID: 46293 RVA: 0x003EC914 File Offset: 0x003EAB14
		public int CompareTo(BuildingChoresPanel.DupeEntryData other)
		{
			if (this.personalPriority != other.personalPriority)
			{
				return other.personalPriority.CompareTo(this.personalPriority);
			}
			if (this.rank != other.rank)
			{
				return this.rank.CompareTo(other.rank);
			}
			if (this.consumer.GetProperName() != other.consumer.GetProperName())
			{
				return this.consumer.GetProperName().CompareTo(other.consumer.GetProperName());
			}
			return this.consumer.GetInstanceID().CompareTo(other.consumer.GetInstanceID());
		}

		// Token: 0x040090D6 RID: 37078
		public ChoreConsumer consumer;

		// Token: 0x040090D7 RID: 37079
		public Chore.Precondition.Context context;

		// Token: 0x040090D8 RID: 37080
		public int personalPriority;

		// Token: 0x040090D9 RID: 37081
		public int rank;
	}
}
