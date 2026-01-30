using System;
using TUNING;
using UnityEngine;

// Token: 0x02000767 RID: 1895
public class GeneticAnalysisStationWorkable : Workable
{
	// Token: 0x06003005 RID: 12293 RVA: 0x00114E9C File Offset: 0x0011309C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanIdentifyMutantSeeds.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.AnalyzingGenes;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_genetic_analysisstation_kanim")
		};
		base.SetWorkTime(150f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x06003006 RID: 12294 RVA: 0x00114F5A File Offset: 0x0011315A
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this.storage.FindFirst(GameTags.UnidentifiedSeed));
	}

	// Token: 0x06003007 RID: 12295 RVA: 0x00114F8E File Offset: 0x0011318E
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, false);
	}

	// Token: 0x06003008 RID: 12296 RVA: 0x00114FB3 File Offset: 0x001131B3
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.IdentifyMutant();
	}

	// Token: 0x06003009 RID: 12297 RVA: 0x00114FC4 File Offset: 0x001131C4
	public void IdentifyMutant()
	{
		GameObject gameObject = this.storage.FindFirst(GameTags.UnidentifiedSeed);
		DebugUtil.DevAssertArgs(gameObject != null, new object[]
		{
			"AAACCCCKKK!! GeneticAnalysisStation finished studying a seed but we don't have one in storage??"
		});
		if (gameObject != null)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			Pickupable pickupable;
			if (component.PrimaryElement.Units > 1f)
			{
				pickupable = component.TakeUnit(1f);
			}
			else
			{
				pickupable = this.storage.Drop(gameObject, true).GetComponent<Pickupable>();
			}
			pickupable.transform.SetPosition(base.transform.GetPosition() + this.finishedSeedDropOffset);
			MutantPlant component2 = pickupable.GetComponent<MutantPlant>();
			PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(component2.SubSpeciesID);
			component2.Analyze();
			SaveGame.Instance.ColonyAchievementTracker.LogAnalyzedSeed(component2.SpeciesID);
		}
	}

	// Token: 0x04001C96 RID: 7318
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04001C97 RID: 7319
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04001C98 RID: 7320
	[SerializeField]
	public Vector3 finishedSeedDropOffset;

	// Token: 0x04001C99 RID: 7321
	private Notification notification;

	// Token: 0x04001C9A RID: 7322
	public GeneticAnalysisStation.StatesInstance statesInstance;
}
