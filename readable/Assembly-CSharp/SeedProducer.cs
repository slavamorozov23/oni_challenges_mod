using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020008BB RID: 2235
[AddComponentMenu("KMonoBehaviour/scripts/SeedProducer")]
public class SeedProducer : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06003D95 RID: 15765 RVA: 0x00157A15 File Offset: 0x00155C15
	public void Configure(string SeedID, SeedProducer.ProductionType productionType, int newSeedsProduced = 1)
	{
		this.seedInfo.seedId = SeedID;
		this.seedInfo.productionType = productionType;
		this.seedInfo.newSeedsProduced = newSeedsProduced;
	}

	// Token: 0x06003D96 RID: 15766 RVA: 0x00157A3B File Offset: 0x00155C3B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SeedProducer>(-216549700, SeedProducer.DropSeedDelegate);
		base.Subscribe<SeedProducer>(1623392196, SeedProducer.DropSeedDelegate);
		base.Subscribe<SeedProducer>(-1072826864, SeedProducer.CropPickedDelegate);
	}

	// Token: 0x06003D97 RID: 15767 RVA: 0x00157A78 File Offset: 0x00155C78
	private GameObject ProduceSeed(string seedId, int units = 1, bool canMutate = true)
	{
		if (seedId != null && units > 0)
		{
			Vector3 position = base.gameObject.transform.GetPosition() + new Vector3(0f, 0.5f, 0f);
			GameObject prefab = Assets.GetPrefab(new Tag(seedId));
			GameObject gameObject = GameUtil.KInstantiate(prefab, position, Grid.SceneLayer.Ore, null, 0);
			MutantPlant component = base.GetComponent<MutantPlant>();
			if (component != null)
			{
				MutantPlant component2 = gameObject.GetComponent<MutantPlant>();
				bool flag = false;
				if (canMutate && component2 != null && component2.IsOriginal)
				{
					flag = this.RollForMutation();
				}
				if (flag)
				{
					component2.Mutate();
				}
				else
				{
					component.CopyMutationsTo(component2);
				}
			}
			PrimaryElement component3 = base.gameObject.GetComponent<PrimaryElement>();
			PrimaryElement component4 = gameObject.GetComponent<PrimaryElement>();
			component4.Temperature = component3.Temperature;
			component4.Units = (float)units;
			base.Trigger(472291861, gameObject);
			gameObject.SetActive(true);
			string text = gameObject.GetProperName();
			if (component != null)
			{
				text = component.GetSubSpeciesInfo().GetNameWithMutations(text, component.IsIdentified, false);
			}
			PopFXManager.Instance.SpawnFX(Def.GetUISprite(prefab, "ui", false).first, PopFXManager.Instance.sprite_Plus, text, gameObject.transform, Vector3.zero, 1.5f, true, false, false);
			return gameObject;
		}
		return null;
	}

	// Token: 0x06003D98 RID: 15768 RVA: 0x00157BC8 File Offset: 0x00155DC8
	public void DropSeed(object data = null)
	{
		if (this.droppedSeedAlready)
		{
			return;
		}
		if (this.seedInfo.newSeedsProduced <= 0)
		{
			return;
		}
		GameObject gameObject = this.ProduceSeed(this.seedInfo.seedId, this.seedInfo.newSeedsProduced, false);
		Uprootable component = base.GetComponent<Uprootable>();
		if (component != null && component.worker != null)
		{
			gameObject.Trigger(580035959, component.worker);
		}
		base.Trigger(-1736624145, gameObject);
		this.droppedSeedAlready = true;
	}

	// Token: 0x06003D99 RID: 15769 RVA: 0x00157C4D File Offset: 0x00155E4D
	public void CropDepleted(object data)
	{
		this.DropSeed(null);
	}

	// Token: 0x06003D9A RID: 15770 RVA: 0x00157C58 File Offset: 0x00155E58
	public void CropPicked(object data)
	{
		if (this.seedInfo.productionType == SeedProducer.ProductionType.Harvest || this.seedInfo.productionType == SeedProducer.ProductionType.HarvestOnly)
		{
			WorkerBase completed_by = base.GetComponent<Harvestable>().completed_by;
			float num = this.seedDropChances;
			if (completed_by != null)
			{
				num += completed_by.GetComponent<AttributeConverters>().Get(Db.Get().AttributeConverters.SeedHarvestChance).Evaluate();
			}
			num *= this.seedDropChanceMultiplier;
			int num2 = (UnityEngine.Random.Range(0f, 1f) <= num) ? 1 : 0;
			if (num2 > 0)
			{
				this.ProduceSeed(this.seedInfo.seedId, num2, true).Trigger(580035959, completed_by);
			}
		}
	}

	// Token: 0x06003D9B RID: 15771 RVA: 0x00157D04 File Offset: 0x00155F04
	public bool RollForMutation()
	{
		AttributeInstance attributeInstance = Db.Get().PlantAttributes.MaxRadiationThreshold.Lookup(this);
		int num = Grid.PosToCell(base.gameObject);
		float num2 = Mathf.Clamp(Grid.IsValidCell(num) ? Grid.Radiation[num] : 0f, 0f, attributeInstance.GetTotalValue()) / attributeInstance.GetTotalValue() * 0.8f;
		return UnityEngine.Random.value < num2;
	}

	// Token: 0x06003D9C RID: 15772 RVA: 0x00157D74 File Offset: 0x00155F74
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Assets.GetPrefab(new Tag(this.seedInfo.seedId)) != null;
		switch (this.seedInfo.productionType)
		{
		case SeedProducer.ProductionType.Hidden:
		case SeedProducer.ProductionType.DigOnly:
		case SeedProducer.ProductionType.Crop:
			return null;
		case SeedProducer.ProductionType.Harvest:
		case SeedProducer.ProductionType.HarvestOnly:
			list.Add(new Descriptor(UI.GAMEOBJECTEFFECTS.SEED_PRODUCTION_HARVEST, UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_PRODUCTION_HARVEST, Descriptor.DescriptorType.Lifecycle, true));
			list.Add(new Descriptor(string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.BONUS_SEEDS, GameUtil.GetFormattedPercent(this.seedDropChances * 100f * this.seedDropChanceMultiplier, GameUtil.TimeSlice.None)), string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.BONUS_SEEDS, GameUtil.GetFormattedPercent(this.seedDropChances * 100f * this.seedDropChanceMultiplier, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
			break;
		case SeedProducer.ProductionType.Fruit:
			list.Add(new Descriptor(UI.GAMEOBJECTEFFECTS.SEED_PRODUCTION_FRUIT, UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_PRODUCTION_DIG_ONLY, Descriptor.DescriptorType.Lifecycle, true));
			break;
		case SeedProducer.ProductionType.Sterile:
			list.Add(new Descriptor(UI.GAMEOBJECTEFFECTS.MUTANT_STERILE, UI.GAMEOBJECTEFFECTS.TOOLTIPS.MUTANT_STERILE, Descriptor.DescriptorType.Effect, false));
			break;
		default:
			DebugUtil.Assert(false, "Seed producer type descriptor not specified");
			return null;
		}
		return list;
	}

	// Token: 0x04002603 RID: 9731
	public SeedProducer.SeedInfo seedInfo;

	// Token: 0x04002604 RID: 9732
	public float seedDropChanceMultiplier = 1f;

	// Token: 0x04002605 RID: 9733
	public float seedDropChances = 0.1f;

	// Token: 0x04002606 RID: 9734
	private bool droppedSeedAlready;

	// Token: 0x04002607 RID: 9735
	private static readonly EventSystem.IntraObjectHandler<SeedProducer> DropSeedDelegate = new EventSystem.IntraObjectHandler<SeedProducer>(delegate(SeedProducer component, object data)
	{
		if (component.seedInfo.productionType != SeedProducer.ProductionType.HarvestOnly)
		{
			component.DropSeed(data);
		}
	});

	// Token: 0x04002608 RID: 9736
	private static readonly EventSystem.IntraObjectHandler<SeedProducer> CropPickedDelegate = new EventSystem.IntraObjectHandler<SeedProducer>(delegate(SeedProducer component, object data)
	{
		component.CropPicked(data);
	});

	// Token: 0x020018CC RID: 6348
	[Serializable]
	public struct SeedInfo
	{
		// Token: 0x04007C01 RID: 31745
		public string seedId;

		// Token: 0x04007C02 RID: 31746
		public SeedProducer.ProductionType productionType;

		// Token: 0x04007C03 RID: 31747
		public int newSeedsProduced;
	}

	// Token: 0x020018CD RID: 6349
	public enum ProductionType
	{
		// Token: 0x04007C05 RID: 31749
		Hidden,
		// Token: 0x04007C06 RID: 31750
		DigOnly,
		// Token: 0x04007C07 RID: 31751
		Harvest,
		// Token: 0x04007C08 RID: 31752
		Fruit,
		// Token: 0x04007C09 RID: 31753
		Sterile,
		// Token: 0x04007C0A RID: 31754
		Crop,
		// Token: 0x04007C0B RID: 31755
		HarvestOnly
	}
}
