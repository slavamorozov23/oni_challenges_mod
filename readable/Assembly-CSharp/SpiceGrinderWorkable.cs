using System;
using System.Linq;
using TUNING;
using UnityEngine;

// Token: 0x02000802 RID: 2050
public class SpiceGrinderWorkable : Workable, IConfigurableConsumer
{
	// Token: 0x06003723 RID: 14115 RVA: 0x0013639C File Offset: 0x0013459C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanSpiceGrinder.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Spicing;
		this.attributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_spice_grinder_kanim")
		};
		base.SetWorkTime(5f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x06003724 RID: 14116 RVA: 0x0013645C File Offset: 0x0013465C
	protected override void OnStartWork(WorkerBase worker)
	{
		if (this.Grinder.CurrentFood != null)
		{
			float num = this.Grinder.CurrentFood.Calories * 0.001f / 1000f;
			base.SetWorkTime(num * 5f);
		}
		else
		{
			global::Debug.LogWarning("SpiceGrider attempted to start spicing with no food");
			base.StopWork(worker, true);
		}
		this.Grinder.UpdateFoodSymbol();
	}

	// Token: 0x06003725 RID: 14117 RVA: 0x001364C5 File Offset: 0x001346C5
	protected override void OnAbortWork(WorkerBase worker)
	{
		if (this.Grinder.CurrentFood == null)
		{
			return;
		}
		this.Grinder.UpdateFoodSymbol();
	}

	// Token: 0x06003726 RID: 14118 RVA: 0x001364E6 File Offset: 0x001346E6
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (this.Grinder.CurrentFood == null)
		{
			return;
		}
		this.Grinder.SpiceFood();
	}

	// Token: 0x06003727 RID: 14119 RVA: 0x00136508 File Offset: 0x00134708
	public IConfigurableConsumerOption[] GetSettingOptions()
	{
		return SpiceGrinder.SettingOptions.Values.ToArray<SpiceGrinder.Option>();
	}

	// Token: 0x06003728 RID: 14120 RVA: 0x00136526 File Offset: 0x00134726
	public IConfigurableConsumerOption GetSelectedOption()
	{
		return this.Grinder.SelectedOption;
	}

	// Token: 0x06003729 RID: 14121 RVA: 0x00136533 File Offset: 0x00134733
	public void SetSelectedOption(IConfigurableConsumerOption option)
	{
		this.Grinder.OnOptionSelected(option as SpiceGrinder.Option);
	}

	// Token: 0x04002181 RID: 8577
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04002182 RID: 8578
	[SerializeField]
	public Vector3 finishedSeedDropOffset;

	// Token: 0x04002183 RID: 8579
	public SpiceGrinder.StatesInstance Grinder;
}
