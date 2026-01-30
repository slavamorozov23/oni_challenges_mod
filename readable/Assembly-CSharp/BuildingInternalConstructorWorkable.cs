using System;
using TUNING;

// Token: 0x020006FF RID: 1791
public class BuildingInternalConstructorWorkable : Workable
{
	// Token: 0x06002C68 RID: 11368 RVA: 0x001028F8 File Offset: 0x00100AF8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.minimumAttributeMultiplier = 0.75f;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.resetProgressOnStop = false;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x06002C69 RID: 11369 RVA: 0x0010299B File Offset: 0x00100B9B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.constructorInstance = this.GetSMI<BuildingInternalConstructor.Instance>();
	}

	// Token: 0x06002C6A RID: 11370 RVA: 0x001029AF File Offset: 0x00100BAF
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.constructorInstance.ConstructionComplete(false);
	}

	// Token: 0x04001A58 RID: 6744
	private BuildingInternalConstructor.Instance constructorInstance;
}
