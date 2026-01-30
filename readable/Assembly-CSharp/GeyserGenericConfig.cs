using System;
using System.Collections.Generic;
using Klei;
using TUNING;
using UnityEngine;

// Token: 0x020001A4 RID: 420
public class GeyserGenericConfig : IMultiEntityConfig
{
	// Token: 0x06000830 RID: 2096 RVA: 0x00037390 File Offset: 0x00035590
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		List<GeyserGenericConfig.GeyserPrefabParams> configs = this.GenerateConfigs();
		foreach (GeyserGenericConfig.GeyserPrefabParams geyserPrefabParams in configs)
		{
			list.Add(GeyserGenericConfig.CreateGeyser(geyserPrefabParams.id, geyserPrefabParams.anim, geyserPrefabParams.width, geyserPrefabParams.height, Strings.Get(geyserPrefabParams.nameStringKey), Strings.Get(geyserPrefabParams.descStringKey), geyserPrefabParams.geyserType.idHash, geyserPrefabParams.geyserType.geyserTemperature, geyserPrefabParams.geyserType.requiredDlcIds, geyserPrefabParams.geyserType.forbiddenDlcIds));
		}
		configs.RemoveAll((GeyserGenericConfig.GeyserPrefabParams x) => !x.isGenericGeyser);
		GameObject gameObject = EntityTemplates.CreateEntity("GeyserGeneric", "Random Geyser Spawner", true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
			int num = 0;
			if (SaveLoader.Instance.clusterDetailSave != null)
			{
				num = SaveLoader.Instance.clusterDetailSave.globalWorldSeed;
			}
			else
			{
				global::Debug.LogWarning("Could not load global world seed for geysers");
			}
			num = num + (int)inst.transform.GetPosition().x + (int)inst.transform.GetPosition().y;
			List<GeyserGenericConfig.GeyserPrefabParams> list2 = configs.FindAll((GeyserGenericConfig.GeyserPrefabParams x) => Game.IsCorrectDlcActiveForCurrentSave(x.geyserType));
			int index = new KRandom(num).Next(0, configs.Count);
			GameUtil.KInstantiate(Assets.GetPrefab(list2[index].id), inst.transform.GetPosition(), Grid.SceneLayer.BuildingBack, null, 0).SetActive(true);
			inst.DeleteObject();
		};
		list.Add(gameObject);
		return list;
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x000374D8 File Offset: 0x000356D8
	public static GameObject CreateGeyser(string id, string anim, int width, int height, string name, string desc, HashedString presetType, float geyserTemperature)
	{
		return GeyserGenericConfig.CreateGeyser(id, anim, width, height, name, desc, presetType, geyserTemperature, null, null);
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x000374F8 File Offset: 0x000356F8
	public static GameObject CreateGeyser(string id, string anim, int width, int height, string name, string desc, HashedString presetType, float geyserTemperature, string[] requiredDlcIds, string[] forbiddenDlcIds)
	{
		float mass = 2000f;
		EffectorValues tier = BUILDINGS.DECOR.BONUS.TIER1;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim(anim), "inactive", Grid.SceneLayer.BuildingBack, width, height, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.GeyserFeature
		}, 293f);
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Katairite, true);
		component.Temperature = geyserTemperature;
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<Uncoverable>();
		gameObject.AddOrGet<CodexEntryRedirector>().CodexID = "GEYSERS";
		gameObject.AddOrGet<Geyser>().outputOffset = new Vector2I(0, 1);
		gameObject.AddOrGet<GeyserConfigurator>().presetType = presetType;
		Studyable studyable = gameObject.AddOrGet<Studyable>();
		studyable.meterTrackerSymbol = "geotracker_target";
		studyable.meterAnim = "tracker";
		gameObject.AddOrGet<LoopingSounds>();
		SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_shake_LP", NOISE_POLLUTION.NOISY.TIER5);
		SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_erupt_LP", NOISE_POLLUTION.NOISY.TIER6);
		KPrefabID component2 = gameObject.GetComponent<KPrefabID>();
		component2.requiredDlcIds = requiredDlcIds;
		component2.forbiddenDlcIds = forbiddenDlcIds;
		return gameObject;
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x0003762C File Offset: 0x0003582C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x0003762E File Offset: 0x0003582E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x00037630 File Offset: 0x00035830
	private List<GeyserGenericConfig.GeyserPrefabParams> GenerateConfigs()
	{
		List<GeyserGenericConfig.GeyserPrefabParams> list = new List<GeyserGenericConfig.GeyserPrefabParams>();
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_steam_kanim", 2, 4, new GeyserConfigurator.GeyserType("steam", SimHashes.Steam, GeyserConfigurator.GeyserShape.Gas, 383.15f, 1000f, 2000f, 5f, null, null, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_steam_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_steam", SimHashes.Steam, GeyserConfigurator.GeyserShape.Gas, 773.15f, 500f, 1000f, 5f, null, null, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_hot_kanim", 4, 2, new GeyserConfigurator.GeyserType("hot_water", SimHashes.Water, GeyserConfigurator.GeyserShape.Liquid, 368.15f, 2000f, 4000f, 500f, null, null, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_slush_kanim", 4, 2, new GeyserConfigurator.GeyserType("slush_water", SimHashes.DirtyWater, GeyserConfigurator.GeyserShape.Liquid, 263.15f, 1000f, 2000f, 500f, null, null, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 263f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_filthy_kanim", 4, 2, new GeyserConfigurator.GeyserType("filthy_water", SimHashes.DirtyWater, GeyserConfigurator.GeyserShape.Liquid, 303.15f, 2000f, 4000f, 500f, null, null, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f).AddDisease(new SimUtil.DiseaseInfo
		{
			idx = Db.Get().Diseases.GetIndex("FoodPoisoning"),
			count = 20000
		}), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_salt_water_cool_slush_kanim", 4, 2, new GeyserConfigurator.GeyserType("slush_salt_water", SimHashes.Brine, GeyserConfigurator.GeyserShape.Liquid, 263.15f, 1000f, 2000f, 500f, null, null, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 263f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_salt_water_kanim", 4, 2, new GeyserConfigurator.GeyserType("salt_water", SimHashes.SaltWater, GeyserConfigurator.GeyserShape.Liquid, 368.15f, 2000f, 4000f, 500f, null, null, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_volcano_small_kanim", 3, 3, new GeyserConfigurator.GeyserType("small_volcano", SimHashes.Magma, GeyserConfigurator.GeyserShape.Molten, 2000f, 400f, 800f, 150f, null, null, 6000f, 12000f, 0.005f, 0.01f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_volcano_big_kanim", 3, 3, new GeyserConfigurator.GeyserType("big_volcano", SimHashes.Magma, GeyserConfigurator.GeyserShape.Molten, 2000f, 800f, 1600f, 150f, null, null, 6000f, 12000f, 0.005f, 0.01f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_co2_kanim", 4, 2, new GeyserConfigurator.GeyserType("liquid_co2", SimHashes.LiquidCarbonDioxide, GeyserConfigurator.GeyserShape.Liquid, 218f, 100f, 200f, 50f, null, null, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 218f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_co2_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_co2", SimHashes.CarbonDioxide, GeyserConfigurator.GeyserShape.Gas, 773.15f, 70f, 140f, 5f, null, null, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_hydrogen_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_hydrogen", SimHashes.Hydrogen, GeyserConfigurator.GeyserShape.Gas, 773.15f, 70f, 140f, 5f, null, null, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_po2_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_po2", SimHashes.ContaminatedOxygen, GeyserConfigurator.GeyserShape.Gas, 773.15f, 70f, 140f, 5f, null, null, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_po2_slimy_kanim", 2, 4, new GeyserConfigurator.GeyserType("slimy_po2", SimHashes.ContaminatedOxygen, GeyserConfigurator.GeyserShape.Gas, 333.15f, 70f, 140f, 5f, null, null, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f).AddDisease(new SimUtil.DiseaseInfo
		{
			idx = Db.Get().Diseases.GetIndex("SlimeLung"),
			count = 5000
		}), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_chlorine_kanim", 2, 4, new GeyserConfigurator.GeyserType("chlorine_gas", SimHashes.ChlorineGas, GeyserConfigurator.GeyserShape.Gas, 333.15f, 70f, 140f, 5f, null, null, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_chlorine_kanim", 2, 4, new GeyserConfigurator.GeyserType("chlorine_gas_cool", SimHashes.ChlorineGas, GeyserConfigurator.GeyserShape.Gas, 278.15f, 70f, 140f, 5f, null, null, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), false));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_methane_kanim", 2, 4, new GeyserConfigurator.GeyserType("methane", SimHashes.Methane, GeyserConfigurator.GeyserShape.Gas, 423.15f, 70f, 140f, 5f, null, null, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_copper_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_copper", SimHashes.MoltenCopper, GeyserConfigurator.GeyserShape.Molten, 2500f, 200f, 400f, 150f, null, null, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_iron_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_iron", SimHashes.MoltenIron, GeyserConfigurator.GeyserShape.Molten, 2800f, 200f, 400f, 150f, null, null, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_gold_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_gold", SimHashes.MoltenGold, GeyserConfigurator.GeyserShape.Molten, 2900f, 200f, 400f, 150f, null, null, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_aluminum_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_aluminum", SimHashes.MoltenAluminum, GeyserConfigurator.GeyserShape.Molten, 2000f, 200f, 400f, 150f, DlcManager.EXPANSION1, null, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_tungsten_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_tungsten", SimHashes.MoltenTungsten, GeyserConfigurator.GeyserShape.Molten, 4000f, 200f, 400f, 150f, DlcManager.EXPANSION1, null, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), false));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_niobium_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_niobium", SimHashes.MoltenNiobium, GeyserConfigurator.GeyserShape.Molten, 3500f, 800f, 1600f, 150f, DlcManager.EXPANSION1, null, 6000f, 12000f, 0.005f, 0.01f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), false));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_cobalt_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_cobalt", SimHashes.MoltenCobalt, GeyserConfigurator.GeyserShape.Molten, 2500f, 200f, 400f, 150f, DlcManager.EXPANSION1, null, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_oil_kanim", 4, 2, new GeyserConfigurator.GeyserType("oil_drip", SimHashes.CrudeOil, GeyserConfigurator.GeyserShape.Liquid, 600f, 1f, 250f, 50f, null, null, 600f, 600f, 1f, 1f, 100f, 500f, 0.4f, 0.8f, 372.15f), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_sulfur_kanim", 4, 2, new GeyserConfigurator.GeyserType("liquid_sulfur", SimHashes.LiquidSulfur, GeyserConfigurator.GeyserShape.Liquid, 438.34998f, 1000f, 2000f, 500f, DlcManager.EXPANSION1, null, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f), true));
		list.RemoveAll((GeyserGenericConfig.GeyserPrefabParams geyser) => !DlcManager.IsCorrectDlcSubscribed(geyser.geyserType));
		return list;
	}

	// Token: 0x0400061C RID: 1564
	public const string ID = "GeyserGeneric";

	// Token: 0x0400061D RID: 1565
	public const string Steam = "steam";

	// Token: 0x0400061E RID: 1566
	public const string HotSteam = "hot_steam";

	// Token: 0x0400061F RID: 1567
	public const string HotWater = "hot_water";

	// Token: 0x04000620 RID: 1568
	public const string SlushWater = "slush_water";

	// Token: 0x04000621 RID: 1569
	public const string FilthyWater = "filthy_water";

	// Token: 0x04000622 RID: 1570
	public const string SlushSaltWater = "slush_salt_water";

	// Token: 0x04000623 RID: 1571
	public const string SaltWater = "salt_water";

	// Token: 0x04000624 RID: 1572
	public const string SmallVolcano = "small_volcano";

	// Token: 0x04000625 RID: 1573
	public const string BigVolcano = "big_volcano";

	// Token: 0x04000626 RID: 1574
	public const string LiquidCO2 = "liquid_co2";

	// Token: 0x04000627 RID: 1575
	public const string HotCO2 = "hot_co2";

	// Token: 0x04000628 RID: 1576
	public const string HotHydrogen = "hot_hydrogen";

	// Token: 0x04000629 RID: 1577
	public const string HotPO2 = "hot_po2";

	// Token: 0x0400062A RID: 1578
	public const string SlimyPO2 = "slimy_po2";

	// Token: 0x0400062B RID: 1579
	public const string ChlorineGas = "chlorine_gas";

	// Token: 0x0400062C RID: 1580
	public const string ChlorineGasCool = "chlorine_gas_cool";

	// Token: 0x0400062D RID: 1581
	public const string Methane = "methane";

	// Token: 0x0400062E RID: 1582
	public const string MoltenCopper = "molten_copper";

	// Token: 0x0400062F RID: 1583
	public const string MoltenIron = "molten_iron";

	// Token: 0x04000630 RID: 1584
	public const string MoltenGold = "molten_gold";

	// Token: 0x04000631 RID: 1585
	public const string MoltenAluminum = "molten_aluminum";

	// Token: 0x04000632 RID: 1586
	public const string MoltenTungsten = "molten_tungsten";

	// Token: 0x04000633 RID: 1587
	public const string MoltenNiobium = "molten_niobium";

	// Token: 0x04000634 RID: 1588
	public const string MoltenCobalt = "molten_cobalt";

	// Token: 0x04000635 RID: 1589
	public const string OilDrip = "oil_drip";

	// Token: 0x04000636 RID: 1590
	public const string LiquidSulfur = "liquid_sulfur";

	// Token: 0x020011B6 RID: 4534
	public struct GeyserPrefabParams
	{
		// Token: 0x06008545 RID: 34117 RVA: 0x00347144 File Offset: 0x00345344
		public GeyserPrefabParams(string anim, int width, int height, GeyserConfigurator.GeyserType geyserType, bool isGenericGeyser)
		{
			this.id = "GeyserGeneric_" + geyserType.id;
			this.anim = anim;
			this.width = width;
			this.height = height;
			this.nameStringKey = new StringKey("STRINGS.CREATURES.SPECIES.GEYSER." + geyserType.id.ToUpper() + ".NAME");
			this.descStringKey = new StringKey("STRINGS.CREATURES.SPECIES.GEYSER." + geyserType.id.ToUpper() + ".DESC");
			this.geyserType = geyserType;
			this.isGenericGeyser = isGenericGeyser;
		}

		// Token: 0x04006569 RID: 25961
		public string id;

		// Token: 0x0400656A RID: 25962
		public string anim;

		// Token: 0x0400656B RID: 25963
		public int width;

		// Token: 0x0400656C RID: 25964
		public int height;

		// Token: 0x0400656D RID: 25965
		public StringKey nameStringKey;

		// Token: 0x0400656E RID: 25966
		public StringKey descStringKey;

		// Token: 0x0400656F RID: 25967
		public GeyserConfigurator.GeyserType geyserType;

		// Token: 0x04006570 RID: 25968
		public bool isGenericGeyser;
	}

	// Token: 0x020011B7 RID: 4535
	private static class TEMPERATURES
	{
		// Token: 0x04006571 RID: 25969
		public const float BELOW_FREEZING = 263.15f;

		// Token: 0x04006572 RID: 25970
		public const float DUPE_NORMAL = 303.15f;

		// Token: 0x04006573 RID: 25971
		public const float DUPE_HOT = 333.15f;

		// Token: 0x04006574 RID: 25972
		public const float BELOW_BOILING = 368.15f;

		// Token: 0x04006575 RID: 25973
		public const float ABOVE_BOILING = 383.15f;

		// Token: 0x04006576 RID: 25974
		public const float HOT1 = 423.15f;

		// Token: 0x04006577 RID: 25975
		public const float HOT2 = 773.15f;

		// Token: 0x04006578 RID: 25976
		public const float MOLTEN_MAGMA = 2000f;
	}

	// Token: 0x020011B8 RID: 4536
	public static class RATES
	{
		// Token: 0x04006579 RID: 25977
		public const float GAS_SMALL_MIN = 40f;

		// Token: 0x0400657A RID: 25978
		public const float GAS_SMALL_MAX = 80f;

		// Token: 0x0400657B RID: 25979
		public const float GAS_NORMAL_MIN = 70f;

		// Token: 0x0400657C RID: 25980
		public const float GAS_NORMAL_MAX = 140f;

		// Token: 0x0400657D RID: 25981
		public const float GAS_BIG_MIN = 100f;

		// Token: 0x0400657E RID: 25982
		public const float GAS_BIG_MAX = 200f;

		// Token: 0x0400657F RID: 25983
		public const float LIQUID_SMALL_MIN = 500f;

		// Token: 0x04006580 RID: 25984
		public const float LIQUID_SMALL_MAX = 1000f;

		// Token: 0x04006581 RID: 25985
		public const float LIQUID_NORMAL_MIN = 1000f;

		// Token: 0x04006582 RID: 25986
		public const float LIQUID_NORMAL_MAX = 2000f;

		// Token: 0x04006583 RID: 25987
		public const float LIQUID_BIG_MIN = 2000f;

		// Token: 0x04006584 RID: 25988
		public const float LIQUID_BIG_MAX = 4000f;

		// Token: 0x04006585 RID: 25989
		public const float MOLTEN_NORMAL_MIN = 200f;

		// Token: 0x04006586 RID: 25990
		public const float MOLTEN_NORMAL_MAX = 400f;

		// Token: 0x04006587 RID: 25991
		public const float MOLTEN_BIG_MIN = 400f;

		// Token: 0x04006588 RID: 25992
		public const float MOLTEN_BIG_MAX = 800f;

		// Token: 0x04006589 RID: 25993
		public const float MOLTEN_HUGE_MIN = 800f;

		// Token: 0x0400658A RID: 25994
		public const float MOLTEN_HUGE_MAX = 1600f;
	}

	// Token: 0x020011B9 RID: 4537
	public static class MAX_PRESSURES
	{
		// Token: 0x0400658B RID: 25995
		public const float GAS = 5f;

		// Token: 0x0400658C RID: 25996
		public const float GAS_HIGH = 15f;

		// Token: 0x0400658D RID: 25997
		public const float MOLTEN = 150f;

		// Token: 0x0400658E RID: 25998
		public const float LIQUID_SMALL = 50f;

		// Token: 0x0400658F RID: 25999
		public const float LIQUID = 500f;
	}

	// Token: 0x020011BA RID: 4538
	public static class ITERATIONS
	{
		// Token: 0x0200277C RID: 10108
		public static class INFREQUENT_MOLTEN
		{
			// Token: 0x0400AF49 RID: 44873
			public const float PCT_MIN = 0.005f;

			// Token: 0x0400AF4A RID: 44874
			public const float PCT_MAX = 0.01f;

			// Token: 0x0400AF4B RID: 44875
			public const float LEN_MIN = 6000f;

			// Token: 0x0400AF4C RID: 44876
			public const float LEN_MAX = 12000f;
		}

		// Token: 0x0200277D RID: 10109
		public static class FREQUENT_MOLTEN
		{
			// Token: 0x0400AF4D RID: 44877
			public const float PCT_MIN = 0.016666668f;

			// Token: 0x0400AF4E RID: 44878
			public const float PCT_MAX = 0.1f;

			// Token: 0x0400AF4F RID: 44879
			public const float LEN_MIN = 480f;

			// Token: 0x0400AF50 RID: 44880
			public const float LEN_MAX = 1080f;
		}
	}
}
