using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020005B7 RID: 1463
public class Diet
{
	// Token: 0x1700014C RID: 332
	// (get) Token: 0x0600218C RID: 8588 RVA: 0x000C2ACC File Offset: 0x000C0CCC
	// (set) Token: 0x0600218D RID: 8589 RVA: 0x000C2AD4 File Offset: 0x000C0CD4
	public Diet.Info[] infos { get; private set; }

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x0600218E RID: 8590 RVA: 0x000C2ADD File Offset: 0x000C0CDD
	// (set) Token: 0x0600218F RID: 8591 RVA: 0x000C2AE5 File Offset: 0x000C0CE5
	public Diet.Info[] solidEdiblesInfo { get; private set; }

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06002190 RID: 8592 RVA: 0x000C2AEE File Offset: 0x000C0CEE
	// (set) Token: 0x06002191 RID: 8593 RVA: 0x000C2AF6 File Offset: 0x000C0CF6
	public Diet.Info[] directlyEatenPlantInfos { get; private set; }

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x06002192 RID: 8594 RVA: 0x000C2AFF File Offset: 0x000C0CFF
	// (set) Token: 0x06002193 RID: 8595 RVA: 0x000C2B07 File Offset: 0x000C0D07
	public Diet.Info[] preyInfos { get; private set; }

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06002194 RID: 8596 RVA: 0x000C2B10 File Offset: 0x000C0D10
	public bool CanEatAnySolid
	{
		get
		{
			return this.solidEdiblesInfo != null && this.solidEdiblesInfo.Length != 0;
		}
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06002195 RID: 8597 RVA: 0x000C2B26 File Offset: 0x000C0D26
	public bool CanEatAnyPlantDirectly
	{
		get
		{
			return this.directlyEatenPlantInfos != null && this.directlyEatenPlantInfos.Length != 0;
		}
	}

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06002196 RID: 8598 RVA: 0x000C2B3C File Offset: 0x000C0D3C
	public bool CanEatPreyCritter
	{
		get
		{
			return this.preyInfos != null && this.preyInfos.Length != 0;
		}
	}

	// Token: 0x06002197 RID: 8599 RVA: 0x000C2B54 File Offset: 0x000C0D54
	public bool IsConsumedTagAbleToBeEatenDirectly(Tag tag)
	{
		if (this.directlyEatenPlantInfos == null && this.preyInfos == null)
		{
			return false;
		}
		for (int i = 0; i < this.directlyEatenPlantInfos.Length; i++)
		{
			if (this.directlyEatenPlantInfos[i].consumedTags.Contains(tag))
			{
				return true;
			}
		}
		for (int j = 0; j < this.preyInfos.Length; j++)
		{
			if (this.preyInfos[j].consumedTags.Contains(tag))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002198 RID: 8600 RVA: 0x000C2BC8 File Offset: 0x000C0DC8
	private void UpdateSecondaryInfoArrays()
	{
		Diet.Info[] directlyEatenPlantInfos;
		if (this.infos != null)
		{
			directlyEatenPlantInfos = (from i in this.infos
			where i.foodType == Diet.Info.FoodType.EatPlantDirectly || i.foodType == Diet.Info.FoodType.EatPlantStorage
			select i).ToArray<Diet.Info>();
		}
		else
		{
			directlyEatenPlantInfos = null;
		}
		this.directlyEatenPlantInfos = directlyEatenPlantInfos;
		Diet.Info[] solidEdiblesInfo;
		if (this.infos != null)
		{
			solidEdiblesInfo = (from i in this.infos
			where i.foodType == Diet.Info.FoodType.EatSolid
			select i).ToArray<Diet.Info>();
		}
		else
		{
			solidEdiblesInfo = null;
		}
		this.solidEdiblesInfo = solidEdiblesInfo;
		Diet.Info[] preyInfos;
		if (this.infos != null)
		{
			preyInfos = (from i in this.infos
			where i.foodType == Diet.Info.FoodType.EatPrey || i.foodType == Diet.Info.FoodType.EatButcheredPrey
			select i).ToArray<Diet.Info>();
		}
		else
		{
			preyInfos = null;
		}
		this.preyInfos = preyInfos;
	}

	// Token: 0x06002199 RID: 8601 RVA: 0x000C2C98 File Offset: 0x000C0E98
	public Diet(params Diet.Info[] infos)
	{
		this.infos = infos;
		this.consumedTags = new List<KeyValuePair<Tag, float>>();
		this.producedTags = new List<KeyValuePair<Tag, float>>();
		for (int i = 0; i < infos.Length; i++)
		{
			Diet.Info info = infos[i];
			using (HashSet<Tag>.Enumerator enumerator = info.consumedTags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Tag tag = enumerator.Current;
					if (-1 == this.consumedTags.FindIndex((KeyValuePair<Tag, float> e) => e.Key == tag))
					{
						this.consumedTags.Add(new KeyValuePair<Tag, float>(tag, info.caloriesPerKg));
					}
					if (this.consumedTagToInfo.ContainsKey(tag))
					{
						string str = "Duplicate diet entry: ";
						Tag tag2 = tag;
						global::Debug.LogError(str + tag2.ToString());
					}
					this.consumedTagToInfo[tag] = info;
				}
			}
			if (info.producedElement != Tag.Invalid && -1 == this.producedTags.FindIndex((KeyValuePair<Tag, float> e) => e.Key == info.producedElement))
			{
				this.producedTags.Add(new KeyValuePair<Tag, float>(info.producedElement, info.producedConversionRate));
			}
		}
		this.UpdateSecondaryInfoArrays();
	}

	// Token: 0x0600219A RID: 8602 RVA: 0x000C2E3C File Offset: 0x000C103C
	public Diet(Diet diet)
	{
		this.infos = new Diet.Info[diet.infos.Length];
		for (int i = 0; i < diet.infos.Length; i++)
		{
			this.infos[i] = new Diet.Info(diet.infos[i]);
		}
		this.consumedTags = new List<KeyValuePair<Tag, float>>();
		this.producedTags = new List<KeyValuePair<Tag, float>>();
		Diet.Info[] infos = this.infos;
		for (int j = 0; j < infos.Length; j++)
		{
			Diet.Info info = infos[j];
			using (HashSet<Tag>.Enumerator enumerator = info.consumedTags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Tag tag = enumerator.Current;
					if (-1 == this.consumedTags.FindIndex((KeyValuePair<Tag, float> e) => e.Key == tag))
					{
						this.consumedTags.Add(new KeyValuePair<Tag, float>(tag, info.caloriesPerKg));
					}
					if (this.consumedTagToInfo.ContainsKey(tag))
					{
						string str = "Duplicate diet entry: ";
						Tag tag2 = tag;
						global::Debug.LogError(str + tag2.ToString());
					}
					this.consumedTagToInfo[tag] = info;
				}
			}
			if (info.producedElement != Tag.Invalid && -1 == this.producedTags.FindIndex((KeyValuePair<Tag, float> e) => e.Key == info.producedElement))
			{
				this.producedTags.Add(new KeyValuePair<Tag, float>(info.producedElement, info.producedConversionRate));
			}
		}
		this.UpdateSecondaryInfoArrays();
	}

	// Token: 0x0600219B RID: 8603 RVA: 0x000C3018 File Offset: 0x000C1218
	public Diet.Info GetDietInfo(Tag tag)
	{
		Diet.Info result = null;
		this.consumedTagToInfo.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x0600219C RID: 8604 RVA: 0x000C3038 File Offset: 0x000C1238
	public float AvailableCaloriesInPrey(Tag tag)
	{
		Diet.Info dietInfo = this.GetDietInfo(tag);
		if (dietInfo == null)
		{
			return 0f;
		}
		GameObject prefab = Assets.GetPrefab(tag);
		if (dietInfo.foodType == Diet.Info.FoodType.EatPrey)
		{
			return prefab.GetComponent<PrimaryElement>().Mass * dietInfo.caloriesPerKg;
		}
		Butcherable component = prefab.GetComponent<Butcherable>();
		float num = 0f;
		if (component == null)
		{
			return 0f;
		}
		foreach (KeyValuePair<string, float> keyValuePair in component.drops)
		{
			Diet.Info dietInfo2 = this.GetDietInfo(new Tag(keyValuePair.Key));
			if (dietInfo2 != null)
			{
				num += keyValuePair.Value * dietInfo2.caloriesPerKg;
			}
		}
		return num;
	}

	// Token: 0x0600219D RID: 8605 RVA: 0x000C3104 File Offset: 0x000C1304
	public void FilterDLC()
	{
		foreach (Diet.Info info in this.infos)
		{
			List<Tag> list = new List<Tag>();
			foreach (Tag tag in info.consumedTags)
			{
				GameObject prefab = Assets.GetPrefab(tag);
				if (prefab == null || !Game.IsCorrectDlcActiveForCurrentSave(prefab.GetComponent<KPrefabID>()))
				{
					list.Add(tag);
				}
			}
			using (List<Tag>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Tag invalid_tag = enumerator2.Current;
					info.consumedTags.Remove(invalid_tag);
					this.consumedTags.RemoveAll((KeyValuePair<Tag, float> t) => t.Key == invalid_tag);
					this.consumedTagToInfo.Remove(invalid_tag);
				}
			}
			if (info.producedElement != Tag.Invalid)
			{
				GameObject prefab2 = Assets.GetPrefab(info.producedElement);
				if (prefab2 == null || !Game.IsCorrectDlcActiveForCurrentSave(prefab2.GetComponent<KPrefabID>()))
				{
					info.consumedTags.Clear();
				}
			}
		}
		this.infos = (from i in this.infos
		where i.consumedTags.Count > 0
		select i).ToArray<Diet.Info>();
		this.UpdateSecondaryInfoArrays();
	}

	// Token: 0x04001395 RID: 5013
	public List<KeyValuePair<Tag, float>> consumedTags;

	// Token: 0x04001396 RID: 5014
	public List<KeyValuePair<Tag, float>> producedTags;

	// Token: 0x04001397 RID: 5015
	private Dictionary<Tag, Diet.Info> consumedTagToInfo = new Dictionary<Tag, Diet.Info>();

	// Token: 0x0200144F RID: 5199
	public class Info
	{
		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06008F56 RID: 36694 RVA: 0x0036B1A1 File Offset: 0x003693A1
		// (set) Token: 0x06008F57 RID: 36695 RVA: 0x0036B1A9 File Offset: 0x003693A9
		public HashSet<Tag> consumedTags { get; private set; }

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06008F58 RID: 36696 RVA: 0x0036B1B2 File Offset: 0x003693B2
		// (set) Token: 0x06008F59 RID: 36697 RVA: 0x0036B1BA File Offset: 0x003693BA
		public Tag producedElement { get; private set; }

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06008F5A RID: 36698 RVA: 0x0036B1C3 File Offset: 0x003693C3
		// (set) Token: 0x06008F5B RID: 36699 RVA: 0x0036B1CB File Offset: 0x003693CB
		public float caloriesPerKg { get; private set; }

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x06008F5C RID: 36700 RVA: 0x0036B1D4 File Offset: 0x003693D4
		// (set) Token: 0x06008F5D RID: 36701 RVA: 0x0036B1DC File Offset: 0x003693DC
		public float producedConversionRate { get; private set; }

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06008F5E RID: 36702 RVA: 0x0036B1E5 File Offset: 0x003693E5
		// (set) Token: 0x06008F5F RID: 36703 RVA: 0x0036B1ED File Offset: 0x003693ED
		public byte diseaseIdx { get; private set; }

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06008F60 RID: 36704 RVA: 0x0036B1F6 File Offset: 0x003693F6
		// (set) Token: 0x06008F61 RID: 36705 RVA: 0x0036B1FE File Offset: 0x003693FE
		public float diseasePerKgProduced { get; private set; }

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06008F62 RID: 36706 RVA: 0x0036B207 File Offset: 0x00369407
		// (set) Token: 0x06008F63 RID: 36707 RVA: 0x0036B20F File Offset: 0x0036940F
		public bool emmitDiseaseOnCell { get; private set; }

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06008F64 RID: 36708 RVA: 0x0036B218 File Offset: 0x00369418
		// (set) Token: 0x06008F65 RID: 36709 RVA: 0x0036B220 File Offset: 0x00369420
		public bool produceSolidTile { get; private set; }

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x06008F66 RID: 36710 RVA: 0x0036B229 File Offset: 0x00369429
		// (set) Token: 0x06008F67 RID: 36711 RVA: 0x0036B231 File Offset: 0x00369431
		public Diet.Info.FoodType foodType { get; private set; }

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x06008F68 RID: 36712 RVA: 0x0036B23A File Offset: 0x0036943A
		// (set) Token: 0x06008F69 RID: 36713 RVA: 0x0036B242 File Offset: 0x00369442
		public string[] eatAnims { get; set; }

		// Token: 0x06008F6A RID: 36714 RVA: 0x0036B24C File Offset: 0x0036944C
		public Info(HashSet<Tag> consumed_tags, Tag produced_element, float calories_per_kg, float produced_conversion_rate = 1f, string disease_id = null, float disease_per_kg_produced = 0f, bool produce_solid_tile = false, Diet.Info.FoodType food_type = Diet.Info.FoodType.EatSolid, bool emmit_disease_on_cell = false, string[] eat_anims = null)
		{
			this.consumedTags = consumed_tags;
			this.producedElement = produced_element;
			this.caloriesPerKg = calories_per_kg;
			this.producedConversionRate = produced_conversion_rate;
			if (!string.IsNullOrEmpty(disease_id))
			{
				this.diseaseIdx = Db.Get().Diseases.GetIndex(disease_id);
			}
			else
			{
				this.diseaseIdx = byte.MaxValue;
			}
			this.diseasePerKgProduced = disease_per_kg_produced;
			this.emmitDiseaseOnCell = emmit_disease_on_cell;
			this.produceSolidTile = produce_solid_tile;
			this.foodType = food_type;
			if (eat_anims == null)
			{
				eat_anims = new string[]
				{
					"eat_pre",
					"eat_loop",
					"eat_pst"
				};
			}
			this.eatAnims = eat_anims;
		}

		// Token: 0x06008F6B RID: 36715 RVA: 0x0036B2FC File Offset: 0x003694FC
		public Info(Diet.Info info)
		{
			this.consumedTags = new HashSet<Tag>(info.consumedTags);
			this.producedElement = info.producedElement;
			this.caloriesPerKg = info.caloriesPerKg;
			this.producedConversionRate = info.producedConversionRate;
			this.diseaseIdx = info.diseaseIdx;
			this.diseasePerKgProduced = info.diseasePerKgProduced;
			this.emmitDiseaseOnCell = info.emmitDiseaseOnCell;
			this.produceSolidTile = info.produceSolidTile;
			this.foodType = info.foodType;
			this.eatAnims = info.eatAnims;
		}

		// Token: 0x06008F6C RID: 36716 RVA: 0x0036B38C File Offset: 0x0036958C
		public bool IsMatch(Tag tag)
		{
			return this.consumedTags.Contains(tag);
		}

		// Token: 0x06008F6D RID: 36717 RVA: 0x0036B39C File Offset: 0x0036959C
		public bool IsMatch(HashSet<Tag> tags)
		{
			if (tags.Count < this.consumedTags.Count)
			{
				foreach (Tag item in tags)
				{
					if (this.consumedTags.Contains(item))
					{
						return true;
					}
				}
				return false;
			}
			foreach (Tag item2 in this.consumedTags)
			{
				if (tags.Contains(item2))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008F6E RID: 36718 RVA: 0x0036B458 File Offset: 0x00369658
		public float ConvertCaloriesToConsumptionMass(float calories)
		{
			return calories / this.caloriesPerKg;
		}

		// Token: 0x06008F6F RID: 36719 RVA: 0x0036B462 File Offset: 0x00369662
		public float ConvertConsumptionMassToCalories(float mass)
		{
			return this.caloriesPerKg * mass;
		}

		// Token: 0x06008F70 RID: 36720 RVA: 0x0036B46C File Offset: 0x0036966C
		public float ConvertConsumptionMassToProducedMass(float consumed_mass)
		{
			return consumed_mass * this.producedConversionRate;
		}

		// Token: 0x06008F71 RID: 36721 RVA: 0x0036B476 File Offset: 0x00369676
		public float ConvertProducedMassToConsumptionMass(float produced_mass)
		{
			return produced_mass / this.producedConversionRate;
		}

		// Token: 0x02002896 RID: 10390
		public enum FoodType
		{
			// Token: 0x0400B2E3 RID: 45795
			EatSolid,
			// Token: 0x0400B2E4 RID: 45796
			EatPlantDirectly,
			// Token: 0x0400B2E5 RID: 45797
			EatPlantStorage,
			// Token: 0x0400B2E6 RID: 45798
			EatPrey,
			// Token: 0x0400B2E7 RID: 45799
			EatButcheredPrey
		}
	}
}
