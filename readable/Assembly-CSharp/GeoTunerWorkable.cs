using System;
using TUNING;

// Token: 0x0200076A RID: 1898
public class GeoTunerWorkable : Workable
{
	// Token: 0x06003025 RID: 12325 RVA: 0x00115F9C File Offset: 0x0011419C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(30f);
		this.requiredSkillPerk = Db.Get().SkillPerks.AllowGeyserTuning.Id;
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.Studying);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_geotuner_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.GeotuningSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.lightEfficiencyBonus = true;
	}
}
