using System;
using System.Linq;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E00 RID: 3584
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleMinionWidget")]
public class ScheduleMinionWidget : KMonoBehaviour
{
	// Token: 0x170007D8 RID: 2008
	// (get) Token: 0x06007189 RID: 29065 RVA: 0x002B5CF4 File Offset: 0x002B3EF4
	// (set) Token: 0x0600718A RID: 29066 RVA: 0x002B5CFC File Offset: 0x002B3EFC
	public Schedulable schedulable { get; private set; }

	// Token: 0x0600718B RID: 29067 RVA: 0x002B5D08 File Offset: 0x002B3F08
	public void ChangeAssignment(Schedule targetSchedule, Schedulable schedulable)
	{
		DebugUtil.LogArgs(new object[]
		{
			"Assigning",
			schedulable,
			"from",
			ScheduleManager.Instance.GetSchedule(schedulable).name,
			"to",
			targetSchedule.name
		});
		ScheduleManager.Instance.GetSchedule(schedulable).Unassign(schedulable);
		targetSchedule.Assign(schedulable);
	}

	// Token: 0x0600718C RID: 29068 RVA: 0x002B5D70 File Offset: 0x002B3F70
	public void Setup(Schedulable schedulable)
	{
		this.schedulable = schedulable;
		IAssignableIdentity component = schedulable.GetComponent<IAssignableIdentity>();
		this.portrait.SetIdentityObject(component, true);
		this.label.text = component.GetProperName();
		MinionIdentity minionIdentity = component as MinionIdentity;
		StoredMinionIdentity storedMinionIdentity = component as StoredMinionIdentity;
		this.RefreshWidgetWorldData();
		if (minionIdentity != null)
		{
			Traits component2 = minionIdentity.GetComponent<Traits>();
			if (component2.HasTrait("NightOwl"))
			{
				this.nightOwlIcon.SetActive(true);
			}
			else if (component2.HasTrait("EarlyBird"))
			{
				this.earlyBirdIcon.SetActive(true);
			}
		}
		else if (storedMinionIdentity != null)
		{
			if (storedMinionIdentity.traitIDs.Contains("NightOwl"))
			{
				this.nightOwlIcon.SetActive(true);
			}
			else if (storedMinionIdentity.traitIDs.Contains("EarlyBird"))
			{
				this.earlyBirdIcon.SetActive(true);
			}
		}
		this.dropDown.Initialize(ScheduleManager.Instance.GetSchedules().Cast<IListableOption>(), new Action<IListableOption, object>(this.OnDropEntryClick), null, new Action<DropDownEntry, object>(this.DropEntryRefreshAction), false, schedulable);
	}

	// Token: 0x0600718D RID: 29069 RVA: 0x002B5E80 File Offset: 0x002B4080
	public void RefreshWidgetWorldData()
	{
		this.worldContainer.SetActive(DlcManager.IsExpansion1Active());
		MinionIdentity minionIdentity = this.schedulable.GetComponent<IAssignableIdentity>() as MinionIdentity;
		if (minionIdentity == null)
		{
			return;
		}
		this.label.text = minionIdentity.GetProperName();
		if (DlcManager.IsExpansion1Active())
		{
			WorldContainer myWorld = minionIdentity.GetMyWorld();
			string text = myWorld.GetComponent<ClusterGridEntity>().Name;
			Image componentInChildren = this.worldContainer.GetComponentInChildren<Image>();
			componentInChildren.sprite = myWorld.GetComponent<ClusterGridEntity>().GetUISprite();
			componentInChildren.SetAlpha((ClusterManager.Instance.activeWorld == myWorld) ? 1f : 0.7f);
			if (ClusterManager.Instance.activeWorld != myWorld)
			{
				text = string.Concat(new string[]
				{
					"<color=",
					Constants.NEUTRAL_COLOR_STR,
					">",
					text,
					"</color>"
				});
			}
			this.worldContainer.GetComponentInChildren<LocText>().SetText(text);
		}
	}

	// Token: 0x0600718E RID: 29070 RVA: 0x002B5F78 File Offset: 0x002B4178
	private void OnDropEntryClick(IListableOption option, object obj)
	{
		Schedule targetSchedule = (Schedule)option;
		this.ChangeAssignment(targetSchedule, this.schedulable);
	}

	// Token: 0x0600718F RID: 29071 RVA: 0x002B5F9C File Offset: 0x002B419C
	private void DropEntryRefreshAction(DropDownEntry entry, object obj)
	{
		Schedule schedule = (Schedule)entry.entryData;
		if (((Schedulable)obj).GetSchedule() == schedule)
		{
			entry.label.text = string.Format(UI.SCHEDULESCREEN.SCHEDULE_DROPDOWN_ASSIGNED, schedule.name);
			entry.button.isInteractable = false;
		}
		else
		{
			entry.label.text = schedule.name;
			entry.button.isInteractable = true;
		}
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("worldContainer").gameObject.SetActive(false);
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("ScheduleIcon").gameObject.SetActive(true);
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("PortraitContainer").gameObject.SetActive(false);
	}

	// Token: 0x06007190 RID: 29072 RVA: 0x002B6070 File Offset: 0x002B4270
	public void SetupBlank(Schedule schedule)
	{
		this.label.text = UI.SCHEDULESCREEN.SCHEDULE_DROPDOWN_BLANK;
		this.dropDown.Initialize(Components.LiveMinionIdentities.Items.Cast<IListableOption>(), new Action<IListableOption, object>(this.OnBlankDropEntryClick), new Func<IListableOption, IListableOption, object, int>(this.BlankDropEntrySort), new Action<DropDownEntry, object>(this.BlankDropEntryRefreshAction), false, schedule);
		Components.LiveMinionIdentities.OnAdd += this.OnLivingMinionsChanged;
		Components.LiveMinionIdentities.OnRemove += this.OnLivingMinionsChanged;
	}

	// Token: 0x06007191 RID: 29073 RVA: 0x002B60FE File Offset: 0x002B42FE
	private void OnLivingMinionsChanged(MinionIdentity minion)
	{
		this.dropDown.ChangeContent(Components.LiveMinionIdentities.Items.Cast<IListableOption>());
	}

	// Token: 0x06007192 RID: 29074 RVA: 0x002B611C File Offset: 0x002B431C
	private void OnBlankDropEntryClick(IListableOption option, object obj)
	{
		Schedule targetSchedule = (Schedule)obj;
		MinionIdentity minionIdentity = (MinionIdentity)option;
		if (minionIdentity == null || minionIdentity.HasTag(GameTags.Dead))
		{
			return;
		}
		this.ChangeAssignment(targetSchedule, minionIdentity.GetComponent<Schedulable>());
	}

	// Token: 0x06007193 RID: 29075 RVA: 0x002B615C File Offset: 0x002B435C
	private void BlankDropEntryRefreshAction(DropDownEntry entry, object obj)
	{
		Schedule schedule = (Schedule)obj;
		MinionIdentity minionIdentity = (MinionIdentity)entry.entryData;
		WorldContainer myWorld = minionIdentity.GetMyWorld();
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("worldContainer").gameObject.SetActive(DlcManager.IsExpansion1Active());
		Image reference = entry.gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("worldIcon");
		reference.sprite = myWorld.GetComponent<ClusterGridEntity>().GetUISprite();
		reference.SetAlpha((ClusterManager.Instance.activeWorld == myWorld) ? 1f : 0.7f);
		string text = myWorld.GetComponent<ClusterGridEntity>().Name;
		if (ClusterManager.Instance.activeWorld != myWorld)
		{
			text = string.Concat(new string[]
			{
				"<color=",
				Constants.NEUTRAL_COLOR_STR,
				">",
				text,
				"</color>"
			});
		}
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("worldLabel").SetText(text);
		if (schedule.IsAssigned(minionIdentity.GetComponent<Schedulable>()))
		{
			entry.label.text = string.Format(UI.SCHEDULESCREEN.SCHEDULE_DROPDOWN_ASSIGNED, minionIdentity.GetProperName());
			entry.button.isInteractable = false;
		}
		else
		{
			entry.label.text = minionIdentity.GetProperName();
			entry.button.isInteractable = true;
		}
		Traits component = minionIdentity.GetComponent<Traits>();
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("NightOwlIcon").gameObject.SetActive(component.HasTrait("NightOwl"));
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("EarlyBirdIcon").gameObject.SetActive(component.HasTrait("EarlyBird"));
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("ScheduleIcon").gameObject.SetActive(false);
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("PortraitContainer").gameObject.SetActive(true);
	}

	// Token: 0x06007194 RID: 29076 RVA: 0x002B6350 File Offset: 0x002B4550
	private int BlankDropEntrySort(IListableOption a, IListableOption b, object obj)
	{
		Schedule schedule = (Schedule)obj;
		MinionIdentity minionIdentity = (MinionIdentity)a;
		MinionIdentity minionIdentity2 = (MinionIdentity)b;
		bool flag = schedule.IsAssigned(minionIdentity.GetComponent<Schedulable>());
		bool flag2 = schedule.IsAssigned(minionIdentity2.GetComponent<Schedulable>());
		if (flag && !flag2)
		{
			return -1;
		}
		if (!flag && flag2)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06007195 RID: 29077 RVA: 0x002B639D File Offset: 0x002B459D
	protected override void OnCleanUp()
	{
		Components.LiveMinionIdentities.OnAdd -= this.OnLivingMinionsChanged;
		Components.LiveMinionIdentities.OnRemove -= this.OnLivingMinionsChanged;
	}

	// Token: 0x04004E60 RID: 20064
	[SerializeField]
	private CrewPortrait portrait;

	// Token: 0x04004E61 RID: 20065
	[SerializeField]
	private DropDown dropDown;

	// Token: 0x04004E62 RID: 20066
	[SerializeField]
	private LocText label;

	// Token: 0x04004E63 RID: 20067
	[SerializeField]
	private GameObject nightOwlIcon;

	// Token: 0x04004E64 RID: 20068
	[SerializeField]
	private GameObject earlyBirdIcon;

	// Token: 0x04004E65 RID: 20069
	[SerializeField]
	private GameObject worldContainer;
}
