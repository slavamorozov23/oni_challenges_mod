using System;
using System.Collections.Generic;
using System.Diagnostics;
using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020004D2 RID: 1234
[AddComponentMenu("KMonoBehaviour/scripts/ChoreConsumer")]
public class ChoreConsumer : KMonoBehaviour, IPersonalPriorityManager
{
	// Token: 0x06001A5A RID: 6746 RVA: 0x00091736 File Offset: 0x0008F936
	public ChoreConsumer.PreconditionSnapshot GetLastPreconditionSnapshot()
	{
		return this.preconditionSnapshot;
	}

	// Token: 0x06001A5B RID: 6747 RVA: 0x0009173E File Offset: 0x0008F93E
	public List<Chore.Precondition.Context> GetSuceededPreconditionContexts()
	{
		return this.lastSuccessfulPreconditionSnapshot.succeededContexts;
	}

	// Token: 0x06001A5C RID: 6748 RVA: 0x0009174B File Offset: 0x0008F94B
	public List<Chore.Precondition.Context> GetFailedPreconditionContexts()
	{
		return this.lastSuccessfulPreconditionSnapshot.failedContexts;
	}

	// Token: 0x06001A5D RID: 6749 RVA: 0x00091758 File Offset: 0x0008F958
	public ChoreConsumer.PreconditionSnapshot GetLastSuccessfulPreconditionSnapshot()
	{
		return this.lastSuccessfulPreconditionSnapshot;
	}

	// Token: 0x06001A5E RID: 6750 RVA: 0x00091760 File Offset: 0x0008F960
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (ChoreGroupManager.instance != null)
		{
			foreach (KeyValuePair<Tag, int> keyValuePair in ChoreGroupManager.instance.DefaultChorePermission)
			{
				bool flag = false;
				foreach (HashedString hashedString in this.userDisabledChoreGroups)
				{
					if (hashedString.HashValue == keyValuePair.Key.GetHashCode())
					{
						flag = true;
						break;
					}
				}
				if (!flag && keyValuePair.Value == 0)
				{
					this.userDisabledChoreGroups.Add(new HashedString(keyValuePair.Key.GetHashCode()));
				}
			}
		}
		this.providers.Add(this.choreProvider);
	}

	// Token: 0x06001A5F RID: 6751 RVA: 0x00091870 File Offset: 0x0008FA70
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (this.choreTable != null)
		{
			this.choreTableInstance = new ChoreTable.Instance(this.choreTable, component);
		}
		foreach (ChoreGroup choreGroup in Db.Get().ChoreGroups.resources)
		{
			int personalPriority = this.GetPersonalPriority(choreGroup);
			this.UpdateChoreTypePriorities(choreGroup, personalPriority);
			this.SetPermittedByUser(choreGroup, personalPriority != 0);
		}
		this.consumerState = new ChoreConsumerState(this);
	}

	// Token: 0x06001A60 RID: 6752 RVA: 0x00091914 File Offset: 0x0008FB14
	protected override void OnForcedCleanUp()
	{
		if (this.consumerState != null)
		{
			this.consumerState.navigator = null;
		}
		this.navigator = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06001A61 RID: 6753 RVA: 0x00091937 File Offset: 0x0008FB37
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.choreTableInstance != null)
		{
			this.choreTableInstance.OnCleanUp(base.GetComponent<KPrefabID>());
			this.choreTableInstance = null;
		}
	}

	// Token: 0x06001A62 RID: 6754 RVA: 0x0009195F File Offset: 0x0008FB5F
	public bool IsPermittedByUser(ChoreGroup chore_group)
	{
		return chore_group == null || !this.userDisabledChoreGroups.Contains(chore_group.IdHash);
	}

	// Token: 0x06001A63 RID: 6755 RVA: 0x0009197C File Offset: 0x0008FB7C
	public void SetPermittedByUser(ChoreGroup chore_group, bool is_allowed)
	{
		if (is_allowed)
		{
			if (this.userDisabledChoreGroups.Remove(chore_group.IdHash))
			{
				this.choreRulesChanged.Signal();
				return;
			}
		}
		else if (!this.userDisabledChoreGroups.Contains(chore_group.IdHash))
		{
			this.userDisabledChoreGroups.Add(chore_group.IdHash);
			this.choreRulesChanged.Signal();
		}
	}

	// Token: 0x06001A64 RID: 6756 RVA: 0x000919DA File Offset: 0x0008FBDA
	public bool IsPermittedByTraits(ChoreGroup chore_group)
	{
		return chore_group == null || !this.traitDisabledChoreGroups.Contains(chore_group.IdHash);
	}

	// Token: 0x06001A65 RID: 6757 RVA: 0x000919F8 File Offset: 0x0008FBF8
	public void SetPermittedByTraits(ChoreGroup chore_group, bool is_enabled)
	{
		if (is_enabled)
		{
			if (this.traitDisabledChoreGroups.Remove(chore_group.IdHash))
			{
				this.choreRulesChanged.Signal();
				return;
			}
		}
		else if (!this.traitDisabledChoreGroups.Contains(chore_group.IdHash))
		{
			this.traitDisabledChoreGroups.Add(chore_group.IdHash);
			this.choreRulesChanged.Signal();
		}
	}

	// Token: 0x06001A66 RID: 6758 RVA: 0x00091A58 File Offset: 0x0008FC58
	private bool ChooseChore(ref Chore.Precondition.Context out_context, List<Chore.Precondition.Context> succeeded_contexts)
	{
		if (succeeded_contexts.Count == 0)
		{
			return false;
		}
		Chore currentChore = this.choreDriver.GetCurrentChore();
		if (currentChore == null)
		{
			for (int i = succeeded_contexts.Count - 1; i >= 0; i--)
			{
				Chore.Precondition.Context context = succeeded_contexts[i];
				if (context.IsSuccess())
				{
					out_context = context;
					return true;
				}
			}
		}
		else
		{
			int interruptPriority = Db.Get().ChoreTypes.TopPriority.interruptPriority;
			int num = (currentChore.masterPriority.priority_class == PriorityScreen.PriorityClass.topPriority) ? interruptPriority : currentChore.choreType.interruptPriority;
			for (int j = succeeded_contexts.Count - 1; j >= 0; j--)
			{
				Chore.Precondition.Context context2 = succeeded_contexts[j];
				if (context2.IsSuccess() && ((context2.masterPriority.priority_class == PriorityScreen.PriorityClass.topPriority) ? interruptPriority : context2.interruptPriority) > num && !currentChore.choreType.interruptExclusion.Overlaps(context2.chore.choreType.tags))
				{
					out_context = context2;
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001A67 RID: 6759 RVA: 0x00091B58 File Offset: 0x0008FD58
	public bool FindNextChore(ref Chore.Precondition.Context out_context)
	{
		this.preconditionSnapshot.Clear();
		this.consumerState.Refresh();
		if (this.consumerState.hasSolidTransferArm)
		{
			global::Debug.Assert(this.stationaryReach > 0);
			CellOffset offset = Grid.GetOffset(Grid.PosToCell(this));
			Extents extents = new Extents(offset.x, offset.y, this.stationaryReach);
			GameScenePartitioner.Instance.VisitEntries<ChoreConsumer>(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.fetchChoreLayer, ChoreConsumer.FindNextChoreEvaluateEntryHelper, this);
		}
		else
		{
			for (int i = 0; i < this.providers.Count; i++)
			{
				this.providers[i].CollectChores(this.consumerState, this.preconditionSnapshot.succeededContexts, this.preconditionSnapshot.failedContexts);
			}
		}
		this.preconditionSnapshot.succeededContexts.Sort();
		List<Chore.Precondition.Context> succeededContexts = this.preconditionSnapshot.succeededContexts;
		bool flag = this.ChooseChore(ref out_context, succeededContexts);
		if (flag)
		{
			this.preconditionSnapshot.CopyTo(this.lastSuccessfulPreconditionSnapshot);
		}
		return flag;
	}

	// Token: 0x06001A68 RID: 6760 RVA: 0x00091C6A File Offset: 0x0008FE6A
	public void AddProvider(ChoreProvider provider)
	{
		DebugUtil.Assert(provider != null);
		this.providers.Add(provider);
	}

	// Token: 0x06001A69 RID: 6761 RVA: 0x00091C84 File Offset: 0x0008FE84
	public void RemoveProvider(ChoreProvider provider)
	{
		this.providers.Remove(provider);
	}

	// Token: 0x06001A6A RID: 6762 RVA: 0x00091C93 File Offset: 0x0008FE93
	public void AddUrge(Urge urge)
	{
		DebugUtil.Assert(urge != null);
		this.urges.Add(urge);
		base.Trigger(-736698276, urge);
	}

	// Token: 0x06001A6B RID: 6763 RVA: 0x00091CB6 File Offset: 0x0008FEB6
	public void RemoveUrge(Urge urge)
	{
		this.urges.Remove(urge);
		base.Trigger(231622047, urge);
	}

	// Token: 0x06001A6C RID: 6764 RVA: 0x00091CD1 File Offset: 0x0008FED1
	public bool HasUrge(Urge urge)
	{
		return this.urges.Contains(urge);
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x00091CDF File Offset: 0x0008FEDF
	public List<Urge> GetUrges()
	{
		return this.urges;
	}

	// Token: 0x06001A6E RID: 6766 RVA: 0x00091CE7 File Offset: 0x0008FEE7
	[Conditional("ENABLE_LOGGER")]
	public void Log(string evt, string param)
	{
	}

	// Token: 0x06001A6F RID: 6767 RVA: 0x00091CEC File Offset: 0x0008FEEC
	public bool IsPermittedOrEnabled(ChoreType chore_type, Chore chore)
	{
		if (chore_type.groups.Length == 0)
		{
			return true;
		}
		bool flag = false;
		bool flag2 = true;
		for (int i = 0; i < chore_type.groups.Length; i++)
		{
			ChoreGroup chore_group = chore_type.groups[i];
			if (!this.IsPermittedByTraits(chore_group))
			{
				flag2 = false;
			}
			if (this.IsPermittedByUser(chore_group))
			{
				flag = true;
			}
		}
		return flag && flag2;
	}

	// Token: 0x06001A70 RID: 6768 RVA: 0x00091D3D File Offset: 0x0008FF3D
	public void SetReach(int reach)
	{
		this.stationaryReach = reach;
	}

	// Token: 0x06001A71 RID: 6769 RVA: 0x00091D48 File Offset: 0x0008FF48
	public bool GetNavigationCost(IApproachable approachable, out int cost)
	{
		if (this.navigator)
		{
			cost = this.navigator.GetNavigationCost(approachable);
			if (cost != -1)
			{
				return true;
			}
		}
		else if (this.consumerState.hasSolidTransferArm)
		{
			int cell = approachable.GetCell();
			if (this.consumerState.solidTransferArm.IsCellReachable(cell))
			{
				cost = Grid.GetCellRange(this.NaturalBuildingCell(), cell);
				return true;
			}
		}
		cost = 0;
		return false;
	}

	// Token: 0x06001A72 RID: 6770 RVA: 0x00091DB4 File Offset: 0x0008FFB4
	public bool GetNavigationCost(int cell, out int cost)
	{
		if (this.navigator)
		{
			cost = this.navigator.GetNavigationCost(cell);
			if (cost != -1)
			{
				return true;
			}
		}
		else if (this.consumerState.hasSolidTransferArm && this.consumerState.solidTransferArm.IsCellReachable(cell))
		{
			cost = Grid.GetCellRange(this.NaturalBuildingCell(), cell);
			return true;
		}
		cost = 0;
		return false;
	}

	// Token: 0x06001A73 RID: 6771 RVA: 0x00091E18 File Offset: 0x00090018
	public bool CanReach(IApproachable approachable)
	{
		if (this.navigator)
		{
			return this.navigator.CanReach(approachable);
		}
		if (this.consumerState.hasSolidTransferArm)
		{
			int cell = approachable.GetCell();
			return this.consumerState.solidTransferArm.IsCellReachable(cell);
		}
		return false;
	}

	// Token: 0x06001A74 RID: 6772 RVA: 0x00091E68 File Offset: 0x00090068
	public bool IsWithinReach(IApproachable approachable)
	{
		if (this.navigator)
		{
			return !(this == null) && !(base.gameObject == null) && Grid.IsCellOffsetOf(Grid.PosToCell(this), approachable.GetCell(), approachable.GetOffsets());
		}
		return this.consumerState.hasSolidTransferArm && this.consumerState.solidTransferArm.IsCellReachable(approachable.GetCell());
	}

	// Token: 0x06001A75 RID: 6773 RVA: 0x00091ED8 File Offset: 0x000900D8
	public void ShowHoverTextOnHoveredItem(Chore.Precondition.Context context, KSelectable hover_obj, HoverTextDrawer drawer, SelectToolHoverTextCard hover_text_card)
	{
		if (context.chore.target.isNull || context.chore.target.gameObject != hover_obj.gameObject)
		{
			return;
		}
		drawer.NewLine(26);
		drawer.AddIndent(36);
		drawer.DrawText(context.chore.choreType.Name, hover_text_card.Styles_BodyText.Standard);
		if (!context.IsSuccess())
		{
			Chore.PreconditionInstance preconditionInstance = context.chore.GetPreconditions()[context.failedPreconditionId];
			string text = preconditionInstance.condition.description;
			if (string.IsNullOrEmpty(text))
			{
				text = preconditionInstance.condition.id;
			}
			if (context.chore.driver != null)
			{
				text = text.Replace("{Assignee}", context.chore.driver.GetProperName());
			}
			text = text.Replace("{Selected}", this.GetProperName());
			drawer.DrawText(" (" + text + ")", hover_text_card.Styles_BodyText.Standard);
		}
	}

	// Token: 0x06001A76 RID: 6774 RVA: 0x00091FF0 File Offset: 0x000901F0
	public void ShowHoverTextOnHoveredItem(KSelectable hover_obj, HoverTextDrawer drawer, SelectToolHoverTextCard hover_text_card)
	{
		bool flag = false;
		foreach (Chore.Precondition.Context context in this.preconditionSnapshot.succeededContexts)
		{
			if (context.chore.showAvailabilityInHoverText && !context.chore.target.isNull && !(context.chore.target.gameObject != hover_obj.gameObject))
			{
				if (!flag)
				{
					drawer.NewLine(26);
					drawer.DrawText(DUPLICANTS.CHORES.PRECONDITIONS.HEADER.ToString().Replace("{Selected}", this.GetProperName()), hover_text_card.Styles_BodyText.Standard);
					flag = true;
				}
				this.ShowHoverTextOnHoveredItem(context, hover_obj, drawer, hover_text_card);
			}
		}
		foreach (Chore.Precondition.Context context2 in this.preconditionSnapshot.failedContexts)
		{
			if (context2.chore.showAvailabilityInHoverText && !context2.chore.target.isNull && !(context2.chore.target.gameObject != hover_obj.gameObject))
			{
				if (!flag)
				{
					drawer.NewLine(26);
					drawer.DrawText(DUPLICANTS.CHORES.PRECONDITIONS.HEADER.ToString().Replace("{Selected}", this.GetProperName()), hover_text_card.Styles_BodyText.Standard);
					flag = true;
				}
				this.ShowHoverTextOnHoveredItem(context2, hover_obj, drawer, hover_text_card);
			}
		}
	}

	// Token: 0x06001A77 RID: 6775 RVA: 0x0009218C File Offset: 0x0009038C
	public int GetPersonalPriority(ChoreType chore_type)
	{
		int num;
		if (!this.choreTypePriorities.TryGetValue(chore_type.IdHash, out num))
		{
			num = 3;
		}
		num = Mathf.Clamp(num, 0, 5);
		return num;
	}

	// Token: 0x06001A78 RID: 6776 RVA: 0x000921BC File Offset: 0x000903BC
	public int GetPersonalPriority(ChoreGroup group)
	{
		int value = 3;
		ChoreConsumer.PriorityInfo priorityInfo;
		if (this.choreGroupPriorities.TryGetValue(group.IdHash, out priorityInfo))
		{
			value = priorityInfo.priority;
		}
		return Mathf.Clamp(value, 0, 5);
	}

	// Token: 0x06001A79 RID: 6777 RVA: 0x000921F4 File Offset: 0x000903F4
	public void SetPersonalPriority(ChoreGroup group, int value)
	{
		if (group.choreTypes == null)
		{
			return;
		}
		value = Mathf.Clamp(value, 0, 5);
		ChoreConsumer.PriorityInfo priorityInfo;
		if (!this.choreGroupPriorities.TryGetValue(group.IdHash, out priorityInfo))
		{
			priorityInfo.priority = 3;
		}
		this.choreGroupPriorities[group.IdHash] = new ChoreConsumer.PriorityInfo
		{
			priority = value
		};
		this.UpdateChoreTypePriorities(group, value);
		this.SetPermittedByUser(group, value != 0);
	}

	// Token: 0x06001A7A RID: 6778 RVA: 0x00092266 File Offset: 0x00090466
	public int GetAssociatedSkillLevel(ChoreGroup group)
	{
		return (int)this.GetAttributes().GetValue(group.attribute.Id);
	}

	// Token: 0x06001A7B RID: 6779 RVA: 0x00092280 File Offset: 0x00090480
	private void UpdateChoreTypePriorities(ChoreGroup group, int value)
	{
		ChoreGroups choreGroups = Db.Get().ChoreGroups;
		foreach (ChoreType choreType in group.choreTypes)
		{
			int num = 0;
			foreach (ChoreGroup choreGroup in choreGroups.resources)
			{
				if (choreGroup.choreTypes != null)
				{
					using (List<ChoreType>.Enumerator enumerator3 = choreGroup.choreTypes.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							if (enumerator3.Current.IdHash == choreType.IdHash)
							{
								int personalPriority = this.GetPersonalPriority(choreGroup);
								num = Mathf.Max(num, personalPriority);
							}
						}
					}
				}
			}
			this.choreTypePriorities[choreType.IdHash] = num;
		}
	}

	// Token: 0x06001A7C RID: 6780 RVA: 0x00092398 File Offset: 0x00090598
	public void ResetPersonalPriorities()
	{
	}

	// Token: 0x06001A7D RID: 6781 RVA: 0x0009239C File Offset: 0x0009059C
	public bool RunBehaviourPrecondition(Tag tag)
	{
		ChoreConsumer.BehaviourPrecondition behaviourPrecondition = default(ChoreConsumer.BehaviourPrecondition);
		return this.behaviourPreconditions.TryGetValue(tag, out behaviourPrecondition) && behaviourPrecondition.cb(behaviourPrecondition.arg);
	}

	// Token: 0x06001A7E RID: 6782 RVA: 0x000923D4 File Offset: 0x000905D4
	public void AddBehaviourPrecondition(Tag tag, Func<object, bool> precondition, object arg)
	{
		DebugUtil.Assert(!this.behaviourPreconditions.ContainsKey(tag));
		this.behaviourPreconditions[tag] = new ChoreConsumer.BehaviourPrecondition
		{
			cb = precondition,
			arg = arg
		};
	}

	// Token: 0x06001A7F RID: 6783 RVA: 0x0009241A File Offset: 0x0009061A
	public void RemoveBehaviourPrecondition(Tag tag, Func<object, bool> precondition, object arg)
	{
		this.behaviourPreconditions.Remove(tag);
	}

	// Token: 0x06001A80 RID: 6784 RVA: 0x0009242C File Offset: 0x0009062C
	public bool IsChoreEqualOrAboveCurrentChorePriority<StateMachineType>()
	{
		Chore currentChore = this.choreDriver.GetCurrentChore();
		return currentChore == null || currentChore.choreType.priority <= this.choreTable.GetChorePriority<StateMachineType>(this);
	}

	// Token: 0x06001A81 RID: 6785 RVA: 0x00092468 File Offset: 0x00090668
	public bool IsChoreGroupDisabled(ChoreGroup chore_group)
	{
		bool result = false;
		Traits component = base.gameObject.GetComponent<Traits>();
		if (component != null && component.IsChoreGroupDisabled(chore_group))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06001A82 RID: 6786 RVA: 0x00092498 File Offset: 0x00090698
	public Dictionary<HashedString, ChoreConsumer.PriorityInfo> GetChoreGroupPriorities()
	{
		return this.choreGroupPriorities;
	}

	// Token: 0x06001A83 RID: 6787 RVA: 0x000924A0 File Offset: 0x000906A0
	public void SetChoreGroupPriorities(Dictionary<HashedString, ChoreConsumer.PriorityInfo> priorities)
	{
		this.choreGroupPriorities = priorities;
	}

	// Token: 0x04000F2D RID: 3885
	public const int DEFAULT_PERSONAL_CHORE_PRIORITY = 3;

	// Token: 0x04000F2E RID: 3886
	public const int MIN_PERSONAL_PRIORITY = 0;

	// Token: 0x04000F2F RID: 3887
	public const int MAX_PERSONAL_PRIORITY = 5;

	// Token: 0x04000F30 RID: 3888
	public const int PRIORITY_DISABLED = 0;

	// Token: 0x04000F31 RID: 3889
	public const int PRIORITY_VERYLOW = 1;

	// Token: 0x04000F32 RID: 3890
	public const int PRIORITY_LOW = 2;

	// Token: 0x04000F33 RID: 3891
	public const int PRIORITY_FLAT = 3;

	// Token: 0x04000F34 RID: 3892
	public const int PRIORITY_HIGH = 4;

	// Token: 0x04000F35 RID: 3893
	public const int PRIORITY_VERYHIGH = 5;

	// Token: 0x04000F36 RID: 3894
	[MyCmpAdd]
	private ChoreProvider choreProvider;

	// Token: 0x04000F37 RID: 3895
	[MyCmpAdd]
	public ChoreDriver choreDriver;

	// Token: 0x04000F38 RID: 3896
	[MyCmpGet]
	public Navigator navigator;

	// Token: 0x04000F39 RID: 3897
	[MyCmpAdd]
	private User user;

	// Token: 0x04000F3A RID: 3898
	public bool prioritizeBrainIfNoChore;

	// Token: 0x04000F3B RID: 3899
	public System.Action choreRulesChanged;

	// Token: 0x04000F3C RID: 3900
	private List<ChoreProvider> providers = new List<ChoreProvider>();

	// Token: 0x04000F3D RID: 3901
	private List<Urge> urges = new List<Urge>();

	// Token: 0x04000F3E RID: 3902
	public ChoreTable choreTable;

	// Token: 0x04000F3F RID: 3903
	private ChoreTable.Instance choreTableInstance;

	// Token: 0x04000F40 RID: 3904
	public ChoreConsumerState consumerState;

	// Token: 0x04000F41 RID: 3905
	private Dictionary<Tag, ChoreConsumer.BehaviourPrecondition> behaviourPreconditions = new Dictionary<Tag, ChoreConsumer.BehaviourPrecondition>();

	// Token: 0x04000F42 RID: 3906
	private ChoreConsumer.PreconditionSnapshot preconditionSnapshot = new ChoreConsumer.PreconditionSnapshot();

	// Token: 0x04000F43 RID: 3907
	private ChoreConsumer.PreconditionSnapshot lastSuccessfulPreconditionSnapshot = new ChoreConsumer.PreconditionSnapshot();

	// Token: 0x04000F44 RID: 3908
	[Serialize]
	private Dictionary<HashedString, ChoreConsumer.PriorityInfo> choreGroupPriorities = new Dictionary<HashedString, ChoreConsumer.PriorityInfo>();

	// Token: 0x04000F45 RID: 3909
	private Dictionary<HashedString, int> choreTypePriorities = new Dictionary<HashedString, int>();

	// Token: 0x04000F46 RID: 3910
	private List<HashedString> traitDisabledChoreGroups = new List<HashedString>();

	// Token: 0x04000F47 RID: 3911
	private List<HashedString> userDisabledChoreGroups = new List<HashedString>();

	// Token: 0x04000F48 RID: 3912
	private int stationaryReach = -1;

	// Token: 0x04000F49 RID: 3913
	private static Func<object, ChoreConsumer, Util.IterationInstruction> FindNextChoreEvaluateEntryHelper = delegate(object obj, ChoreConsumer consumer)
	{
		FetchChore fetchChore = obj as FetchChore;
		if (fetchChore == null)
		{
			DebugUtil.Assert(false, "FindNextChore found an entry that wasn't a FetchChore");
			return Util.IterationInstruction.Continue;
		}
		if (fetchChore.target == null)
		{
			DebugUtil.Assert(false, "FindNextChore found an entry with a null target");
			return Util.IterationInstruction.Continue;
		}
		if (fetchChore.isNull)
		{
			global::Debug.LogWarning("FindNextChore found an entry that isNull");
			return Util.IterationInstruction.Continue;
		}
		int cell = Grid.PosToCell(fetchChore.gameObject);
		if (consumer.consumerState.solidTransferArm.IsCellReachable(cell))
		{
			fetchChore.CollectChoresFromGlobalChoreProvider(consumer.consumerState, consumer.preconditionSnapshot.succeededContexts, consumer.preconditionSnapshot.failedContexts, false);
		}
		return Util.IterationInstruction.Continue;
	};

	// Token: 0x02001340 RID: 4928
	private struct BehaviourPrecondition
	{
		// Token: 0x04006ABE RID: 27326
		public Func<object, bool> cb;

		// Token: 0x04006ABF RID: 27327
		public object arg;
	}

	// Token: 0x02001341 RID: 4929
	public class PreconditionSnapshot
	{
		// Token: 0x06008B58 RID: 35672 RVA: 0x0035ED4E File Offset: 0x0035CF4E
		public void CopyTo(ChoreConsumer.PreconditionSnapshot snapshot)
		{
			snapshot.Clear();
			snapshot.succeededContexts.AddRange(this.succeededContexts);
			snapshot.failedContexts.AddRange(this.failedContexts);
			snapshot.doFailedContextsNeedSorting = true;
		}

		// Token: 0x06008B59 RID: 35673 RVA: 0x0035ED7F File Offset: 0x0035CF7F
		public void Clear()
		{
			this.succeededContexts.Clear();
			this.failedContexts.Clear();
			this.doFailedContextsNeedSorting = true;
		}

		// Token: 0x04006AC0 RID: 27328
		public List<Chore.Precondition.Context> succeededContexts = new List<Chore.Precondition.Context>();

		// Token: 0x04006AC1 RID: 27329
		public List<Chore.Precondition.Context> failedContexts = new List<Chore.Precondition.Context>();

		// Token: 0x04006AC2 RID: 27330
		public bool doFailedContextsNeedSorting = true;
	}

	// Token: 0x02001342 RID: 4930
	public struct PriorityInfo
	{
		// Token: 0x04006AC3 RID: 27331
		public int priority;
	}
}
