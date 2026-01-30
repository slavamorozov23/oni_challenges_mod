using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000646 RID: 1606
[AddComponentMenu("KMonoBehaviour/Workable/Studyable")]
public class Studyable : Workable, ISidescreenButtonControl
{
	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x06002712 RID: 10002 RVA: 0x000E100A File Offset: 0x000DF20A
	public bool Studied
	{
		get
		{
			return this.studied;
		}
	}

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x06002713 RID: 10003 RVA: 0x000E1012 File Offset: 0x000DF212
	public bool Studying
	{
		get
		{
			return this.chore != null && this.chore.InProgress();
		}
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x06002714 RID: 10004 RVA: 0x000E1029 File Offset: 0x000DF229
	public string SidescreenTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x06002715 RID: 10005 RVA: 0x000E1030 File Offset: 0x000DF230
	public string SidescreenStatusMessage
	{
		get
		{
			if (this.studied)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_STATUS;
			}
			if (this.markedForStudy)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_STATUS;
			}
			return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_STATUS;
		}
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x06002716 RID: 10006 RVA: 0x000E1062 File Offset: 0x000DF262
	public string SidescreenButtonText
	{
		get
		{
			if (this.studied)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_BUTTON;
			}
			if (this.markedForStudy)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_BUTTON;
			}
			return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_BUTTON;
		}
	}

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x06002717 RID: 10007 RVA: 0x000E1094 File Offset: 0x000DF294
	public string SidescreenButtonTooltip
	{
		get
		{
			if (this.studied)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_STATUS;
			}
			if (this.markedForStudy)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_STATUS;
			}
			return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_STATUS;
		}
	}

	// Token: 0x06002718 RID: 10008 RVA: 0x000E10C6 File Offset: 0x000DF2C6
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x06002719 RID: 10009 RVA: 0x000E10C9 File Offset: 0x000DF2C9
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600271A RID: 10010 RVA: 0x000E10D0 File Offset: 0x000DF2D0
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x0600271B RID: 10011 RVA: 0x000E10D3 File Offset: 0x000DF2D3
	public bool SidescreenButtonInteractable()
	{
		return !this.studied;
	}

	// Token: 0x0600271C RID: 10012 RVA: 0x000E10E0 File Offset: 0x000DF2E0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_machine_kanim")
		};
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Studying;
		this.resetProgressOnStop = false;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		base.SetWorkTime(3600f);
	}

	// Token: 0x0600271D RID: 10013 RVA: 0x000E11A8 File Offset: 0x000DF3A8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.studiedIndicator = new MeterController(base.GetComponent<KBatchedAnimController>(), this.meterTrackerSymbol, this.meterAnim, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			this.meterTrackerSymbol
		});
		this.studiedIndicator.meterController.gameObject.AddComponent<LoopingSounds>();
		this.Refresh();
	}

	// Token: 0x0600271E RID: 10014 RVA: 0x000E1206 File Offset: 0x000DF406
	public void CancelChore()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Studyable.CancelChore");
			this.chore = null;
			base.Trigger(1488501379, null);
		}
	}

	// Token: 0x0600271F RID: 10015 RVA: 0x000E1234 File Offset: 0x000DF434
	public void Refresh()
	{
		if (KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.studied)
		{
			this.statusItemGuid = component.ReplaceStatusItem(this.statusItemGuid, Db.Get().MiscStatusItems.Studied, null);
			this.studiedIndicator.gameObject.SetActive(true);
			this.studiedIndicator.meterController.Play(this.meterAnim, KAnim.PlayMode.Loop, 1f, 0f);
			this.requiredSkillPerk = null;
			this.UpdateStatusItem(null);
			return;
		}
		if (this.markedForStudy)
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<Studyable>(Db.Get().ChoreTypes.Research, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
			this.statusItemGuid = component.ReplaceStatusItem(this.statusItemGuid, Db.Get().MiscStatusItems.AwaitingStudy, null);
		}
		else
		{
			this.CancelChore();
			this.statusItemGuid = component.RemoveStatusItem(this.statusItemGuid, false);
		}
		this.studiedIndicator.gameObject.SetActive(false);
	}

	// Token: 0x06002720 RID: 10016 RVA: 0x000E134C File Offset: 0x000DF54C
	private void ToggleStudyChore()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.studied = true;
			if (this.chore != null)
			{
				this.chore.Cancel("debug");
				this.chore = null;
			}
			base.Trigger(-1436775550, null);
		}
		else
		{
			this.markedForStudy = !this.markedForStudy;
		}
		this.Refresh();
	}

	// Token: 0x06002721 RID: 10017 RVA: 0x000E13A9 File Offset: 0x000DF5A9
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.studied = true;
		this.chore = null;
		this.Refresh();
		base.Trigger(-1436775550, null);
		if (DlcManager.IsExpansion1Active())
		{
			this.DropDatabanks();
		}
	}

	// Token: 0x06002722 RID: 10018 RVA: 0x000E13E0 File Offset: 0x000DF5E0
	private void DropDatabanks()
	{
		int num = UnityEngine.Random.Range(7, 13);
		for (int i = 0; i <= num; i++)
		{
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("OrbitalResearchDatabank"), base.transform.position + new Vector3(0f, 1f, 0f), Grid.SceneLayer.Ore, null, 0);
			gameObject.GetComponent<PrimaryElement>().Temperature = 298.15f;
			gameObject.SetActive(true);
		}
	}

	// Token: 0x06002723 RID: 10019 RVA: 0x000E1454 File Offset: 0x000DF654
	public void OnSidescreenButtonPressed()
	{
		this.ToggleStudyChore();
	}

	// Token: 0x06002724 RID: 10020 RVA: 0x000E145C File Offset: 0x000DF65C
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x0400171B RID: 5915
	public string meterTrackerSymbol;

	// Token: 0x0400171C RID: 5916
	public string meterAnim;

	// Token: 0x0400171D RID: 5917
	private Chore chore;

	// Token: 0x0400171E RID: 5918
	private const float STUDY_WORK_TIME = 3600f;

	// Token: 0x0400171F RID: 5919
	[Serialize]
	private bool studied;

	// Token: 0x04001720 RID: 5920
	[Serialize]
	private bool markedForStudy;

	// Token: 0x04001721 RID: 5921
	private Guid statusItemGuid;

	// Token: 0x04001722 RID: 5922
	private Guid additionalStatusItemGuid;

	// Token: 0x04001723 RID: 5923
	public MeterController studiedIndicator;
}
