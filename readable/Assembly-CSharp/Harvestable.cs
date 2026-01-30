using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000987 RID: 2439
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Harvestable")]
public class Harvestable : Workable
{
	// Token: 0x170004FB RID: 1275
	// (get) Token: 0x060045FE RID: 17918 RVA: 0x00194829 File Offset: 0x00192A29
	// (set) Token: 0x060045FF RID: 17919 RVA: 0x00194831 File Offset: 0x00192A31
	public WorkerBase completed_by { get; protected set; }

	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x06004600 RID: 17920 RVA: 0x0019483A File Offset: 0x00192A3A
	public bool CanBeHarvested
	{
		get
		{
			return this.canBeHarvested;
		}
	}

	// Token: 0x06004601 RID: 17921 RVA: 0x00194842 File Offset: 0x00192A42
	protected Harvestable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x06004602 RID: 17922 RVA: 0x0019486C File Offset: 0x00192A6C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Harvesting;
		this.multitoolContext = "harvest";
		this.multitoolHitEffectTag = "fx_harvest_splash";
		this.harvestDesignatable = base.GetComponent<HarvestDesignatable>();
	}

	// Token: 0x06004603 RID: 17923 RVA: 0x001948C0 File Offset: 0x00192AC0
	protected override void OnSpawn()
	{
		base.Subscribe<Harvestable>(2127324410, Harvestable.ForceCancelHarvestDelegate);
		base.SetWorkTime(10f);
		base.Subscribe<Harvestable>(2127324410, Harvestable.OnCancelDelegate);
		this.faceTargetWhenWorking = true;
		Components.Harvestables.Add(this);
		this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
	}

	// Token: 0x06004604 RID: 17924 RVA: 0x00194951 File Offset: 0x00192B51
	public void OnUprooted(object data)
	{
		if (this.canBeHarvested)
		{
			this.Harvest();
		}
	}

	// Token: 0x06004605 RID: 17925 RVA: 0x00194964 File Offset: 0x00192B64
	public void Harvest()
	{
		if (this.harvestDesignatable != null)
		{
			this.harvestDesignatable.MarkedForHarvest = false;
		}
		this.chore = null;
		base.Trigger(1272413801, this);
		KSelectable component = base.GetComponent<KSelectable>();
		component.RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
		component.RemoveStatusItem(Db.Get().MiscStatusItems.Operating, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
		this.completed_by = null;
	}

	// Token: 0x06004606 RID: 17926 RVA: 0x001949F0 File Offset: 0x00192BF0
	public void OnMarkedForHarvest()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.chore == null)
		{
			this.chore = new WorkChore<Harvestable>(Db.Get().ChoreTypes.Harvest, this, null, true, null, null, null, true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			component.AddStatusItem(Db.Get().MiscStatusItems.PendingHarvest, this);
		}
	}

	// Token: 0x06004607 RID: 17927 RVA: 0x00194A50 File Offset: 0x00192C50
	public void SetCanBeHarvested(bool state)
	{
		this.canBeHarvested = state;
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.canBeHarvested)
		{
			component.AddStatusItem(this.readyForHarvestStatusItem, null);
			if (this.harvestDesignatable != null)
			{
				if (this.harvestDesignatable.HarvestWhenReady)
				{
					this.harvestDesignatable.MarkForHarvest();
				}
				else if (this.harvestDesignatable.InPlanterBox)
				{
					component.AddStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, this);
				}
			}
			else
			{
				this.OnMarkedForHarvest();
			}
		}
		else
		{
			component.RemoveStatusItem(this.readyForHarvestStatusItem, false);
			component.RemoveStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, false);
		}
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06004608 RID: 17928 RVA: 0x00194B11 File Offset: 0x00192D11
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.completed_by = worker;
		this.Harvest();
	}

	// Token: 0x06004609 RID: 17929 RVA: 0x00194B20 File Offset: 0x00192D20
	protected virtual void OnCancel(object data)
	{
		bool flag = data == null || (data is Boxed<bool> && !((Boxed<bool>)data).value);
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel harvest");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
			if (flag && this.harvestDesignatable != null)
			{
				this.harvestDesignatable.SetHarvestWhenReady(false);
			}
		}
		if (flag && this.harvestDesignatable != null)
		{
			this.harvestDesignatable.MarkedForHarvest = false;
		}
	}

	// Token: 0x0600460A RID: 17930 RVA: 0x00194BC2 File Offset: 0x00192DC2
	public bool HasChore()
	{
		return this.chore != null;
	}

	// Token: 0x0600460B RID: 17931 RVA: 0x00194BCF File Offset: 0x00192DCF
	public virtual void ForceCancelHarvest(object data = null)
	{
		this.OnCancel(data);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x0600460C RID: 17932 RVA: 0x00194C09 File Offset: 0x00192E09
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Harvestables.Remove(this);
	}

	// Token: 0x0600460D RID: 17933 RVA: 0x00194C1C File Offset: 0x00192E1C
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
	}

	// Token: 0x04002F21 RID: 12065
	public StatusItem readyForHarvestStatusItem = Db.Get().CreatureStatusItems.ReadyForHarvest;

	// Token: 0x04002F22 RID: 12066
	public HarvestDesignatable harvestDesignatable;

	// Token: 0x04002F23 RID: 12067
	[Serialize]
	protected bool canBeHarvested;

	// Token: 0x04002F25 RID: 12069
	protected Chore chore;

	// Token: 0x04002F26 RID: 12070
	private static readonly EventSystem.IntraObjectHandler<Harvestable> ForceCancelHarvestDelegate = new EventSystem.IntraObjectHandler<Harvestable>(delegate(Harvestable component, object data)
	{
		component.ForceCancelHarvest(data);
	});

	// Token: 0x04002F27 RID: 12071
	private static readonly EventSystem.IntraObjectHandler<Harvestable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Harvestable>(delegate(Harvestable component, object data)
	{
		component.OnCancel(data);
	});
}
