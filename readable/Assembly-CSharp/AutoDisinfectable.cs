using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200059A RID: 1434
[AddComponentMenu("KMonoBehaviour/Workable/AutoDisinfectable")]
public class AutoDisinfectable : Workable
{
	// Token: 0x0600202F RID: 8239 RVA: 0x000B9C78 File Offset: 0x000B7E78
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Disinfecting;
		this.resetProgressOnStop = true;
		this.multitoolContext = "disinfect";
		this.multitoolHitEffectTag = "fx_disinfect_splash";
	}

	// Token: 0x06002030 RID: 8240 RVA: 0x000B9CE0 File Offset: 0x000B7EE0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<AutoDisinfectable>(493375141, AutoDisinfectable.OnRefreshUserMenuDelegate);
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		base.SetWorkTime(10f);
		this.shouldTransferDiseaseWithWorker = false;
	}

	// Token: 0x06002031 RID: 8241 RVA: 0x000B9D5B File Offset: 0x000B7F5B
	public void CancelChore()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("AutoDisinfectable.CancelChore");
			this.chore = null;
		}
	}

	// Token: 0x06002032 RID: 8242 RVA: 0x000B9D7C File Offset: 0x000B7F7C
	public void RefreshChore()
	{
		if (KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		if (!this.enableAutoDisinfect || !SaveGame.Instance.enableAutoDisinfect)
		{
			if (this.chore != null)
			{
				this.chore.Cancel("Autodisinfect Disabled");
				this.chore = null;
				return;
			}
		}
		else if (this.chore == null || !(this.chore.driver != null))
		{
			int diseaseCount = this.primaryElement.DiseaseCount;
			if (this.chore == null && diseaseCount > SaveGame.Instance.minGermCountForDisinfect)
			{
				this.chore = new WorkChore<AutoDisinfectable>(Db.Get().ChoreTypes.Disinfect, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
				return;
			}
			if (diseaseCount < SaveGame.Instance.minGermCountForDisinfect && this.chore != null)
			{
				this.chore.Cancel("AutoDisinfectable.Update");
				this.chore = null;
			}
		}
	}

	// Token: 0x06002033 RID: 8243 RVA: 0x000B9E5D File Offset: 0x000B805D
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.diseasePerSecond = (float)base.GetComponent<PrimaryElement>().DiseaseCount / 10f;
	}

	// Token: 0x06002034 RID: 8244 RVA: 0x000B9E7E File Offset: 0x000B807E
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.AddDisease(component.DiseaseIdx, -(int)(this.diseasePerSecond * dt + 0.5f), "Disinfectable.OnWorkTick");
		return false;
	}

	// Token: 0x06002035 RID: 8245 RVA: 0x000B9EB0 File Offset: 0x000B80B0
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.AddDisease(component.DiseaseIdx, -component.DiseaseCount, "Disinfectable.OnCompleteWork");
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, this);
		this.chore = null;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06002036 RID: 8246 RVA: 0x000B9F20 File Offset: 0x000B8120
	private void EnableAutoDisinfect()
	{
		this.enableAutoDisinfect = true;
		this.RefreshChore();
	}

	// Token: 0x06002037 RID: 8247 RVA: 0x000B9F2F File Offset: 0x000B812F
	private void DisableAutoDisinfect()
	{
		this.enableAutoDisinfect = false;
		this.RefreshChore();
	}

	// Token: 0x06002038 RID: 8248 RVA: 0x000B9F40 File Offset: 0x000B8140
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button;
		if (!this.enableAutoDisinfect)
		{
			button = new KIconButtonMenu.ButtonInfo("action_disinfect", STRINGS.BUILDINGS.AUTODISINFECTABLE.ENABLE_AUTODISINFECT.NAME, new System.Action(this.EnableAutoDisinfect), global::Action.NumActions, null, null, null, STRINGS.BUILDINGS.AUTODISINFECTABLE.ENABLE_AUTODISINFECT.TOOLTIP, true);
		}
		else
		{
			button = new KIconButtonMenu.ButtonInfo("action_disinfect", STRINGS.BUILDINGS.AUTODISINFECTABLE.DISABLE_AUTODISINFECT.NAME, new System.Action(this.DisableAutoDisinfect), global::Action.NumActions, null, null, null, STRINGS.BUILDINGS.AUTODISINFECTABLE.DISABLE_AUTODISINFECT.TOOLTIP, true);
		}
		Game.Instance.userMenu.AddButton(base.gameObject, button, 10f);
	}

	// Token: 0x040012B8 RID: 4792
	private Chore chore;

	// Token: 0x040012B9 RID: 4793
	private const float MAX_WORK_TIME = 10f;

	// Token: 0x040012BA RID: 4794
	private float diseasePerSecond;

	// Token: 0x040012BB RID: 4795
	[MyCmpGet]
	private PrimaryElement primaryElement;

	// Token: 0x040012BC RID: 4796
	[Serialize]
	private bool enableAutoDisinfect = true;

	// Token: 0x040012BD RID: 4797
	private static readonly EventSystem.IntraObjectHandler<AutoDisinfectable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<AutoDisinfectable>(delegate(AutoDisinfectable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
