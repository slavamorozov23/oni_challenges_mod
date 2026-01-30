using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200073A RID: 1850
public class Deepfryer : ComplexFabricator, IGameObjectEffectDescriptor
{
	// Token: 0x06002E98 RID: 11928 RVA: 0x0010D255 File Offset: 0x0010B455
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreType = Db.Get().ChoreTypes.Cook;
		this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
	}

	// Token: 0x06002E99 RID: 11929 RVA: 0x0010D28C File Offset: 0x0010B48C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.workable.requiredSkillPerk = Db.Get().SkillPerks.CanDeepFry.Id;
		this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Cooking;
		this.workable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_deepfryer_kanim")
		};
		this.workable.AttributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
		this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		ComplexFabricatorWorkable workable = this.workable;
		workable.OnWorkTickActions = (Action<WorkerBase, float>)Delegate.Combine(workable.OnWorkTickActions, new Action<WorkerBase, float>(delegate(WorkerBase worker, float dt)
		{
			global::Debug.Assert(worker != null, "How did we get a null worker?");
			if (this.diseaseCountKillRate > 0)
			{
				PrimaryElement component = base.GetComponent<PrimaryElement>();
				int num = Math.Max(1, (int)((float)this.diseaseCountKillRate * dt));
				component.ModifyDiseaseCount(-num, "Deepfryer");
			}
		}));
		base.GetComponent<ComplexFabricator>().workingStatusItem = Db.Get().BuildingStatusItems.ComplexFabricatorCooking;
	}

	// Token: 0x06002E9A RID: 11930 RVA: 0x0010D398 File Offset: 0x0010B598
	protected override List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
	{
		List<GameObject> list = base.SpawnOrderProduct(recipe);
		foreach (GameObject gameObject in list)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.ModifyDiseaseCount(-component.DiseaseCount, "Deepfryer.CompleteOrder");
		}
		base.GetComponent<Operational>().SetActive(false, false);
		return list;
	}

	// Token: 0x06002E9B RID: 11931 RVA: 0x0010D40C File Offset: 0x0010B60C
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.REMOVES_DISEASE, UI.BUILDINGEFFECTS.TOOLTIPS.REMOVES_DISEASE, Descriptor.DescriptorType.Effect, false));
		return descriptors;
	}

	// Token: 0x04001B9C RID: 7068
	[SerializeField]
	private int diseaseCountKillRate = 100;
}
