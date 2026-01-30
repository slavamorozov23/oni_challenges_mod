using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000763 RID: 1891
public class GammaRayOven : ComplexFabricator, IGameObjectEffectDescriptor
{
	// Token: 0x06002FD5 RID: 12245 RVA: 0x001143C7 File Offset: 0x001125C7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreType = Db.Get().ChoreTypes.Cook;
		this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
	}

	// Token: 0x06002FD6 RID: 12246 RVA: 0x00114400 File Offset: 0x00112600
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Cooking;
		this.workable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_cookstation_kanim")
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
				component.ModifyDiseaseCount(-num, "GammaRayOven");
			}
		}));
		base.GetComponent<Radiator>().emitter.enabled = false;
		base.Subscribe(824508782, new Action<object>(this.UpdateRadiator));
	}

	// Token: 0x06002FD7 RID: 12247 RVA: 0x001144F9 File Offset: 0x001126F9
	private void UpdateRadiator(object data)
	{
		base.GetComponent<Radiator>().emitter.enabled = this.operational.IsActive;
	}

	// Token: 0x06002FD8 RID: 12248 RVA: 0x00114518 File Offset: 0x00112718
	protected override List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
	{
		List<GameObject> list = base.SpawnOrderProduct(recipe);
		foreach (GameObject gameObject in list)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.ModifyDiseaseCount(-component.DiseaseCount, "GammaRayOven.CompleteOrder");
		}
		base.GetComponent<Operational>().SetActive(false, false);
		return list;
	}

	// Token: 0x06002FD9 RID: 12249 RVA: 0x0011458C File Offset: 0x0011278C
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.REMOVES_DISEASE, UI.BUILDINGEFFECTS.TOOLTIPS.REMOVES_DISEASE, Descriptor.DescriptorType.Effect, false));
		return descriptors;
	}

	// Token: 0x04001C81 RID: 7297
	[SerializeField]
	private int diseaseCountKillRate = 100;
}
