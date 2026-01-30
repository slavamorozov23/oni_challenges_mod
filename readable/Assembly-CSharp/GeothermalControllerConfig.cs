using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200023F RID: 575
public class GeothermalControllerConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000B9F RID: 2975 RVA: 0x00046314 File Offset: 0x00044514
	public static List<GeothermalVent.ElementInfo> GetClearingEntombedVentReward()
	{
		return new List<GeothermalVent.ElementInfo>
		{
			new GeothermalVent.ElementInfo
			{
				isSolid = false,
				elementHash = SimHashes.Steam,
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Steam).idx,
				mass = 100f,
				temperature = 1102f,
				diseaseIdx = byte.MaxValue,
				diseaseCount = 0
			},
			new GeothermalVent.ElementInfo
			{
				isSolid = true,
				elementHash = SimHashes.Lead,
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Lead).idx,
				mass = 144f,
				temperature = 502f,
				diseaseIdx = byte.MaxValue,
				diseaseCount = 0
			}
		};
	}

	// Token: 0x06000BA0 RID: 2976 RVA: 0x000463F0 File Offset: 0x000445F0
	public static List<GeothermalControllerConfig.Impurity> GetImpurities()
	{
		return new List<GeothermalControllerConfig.Impurity>
		{
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.IgneousRock).idx,
				mass_kg = 50f,
				required_temp_range = new MathUtil.MinMax(0f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Granite).idx,
				mass_kg = 50f,
				required_temp_range = new MathUtil.MinMax(0f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Obsidian).idx,
				mass_kg = 50f,
				required_temp_range = new MathUtil.MinMax(0f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.SaltWater).idx,
				mass_kg = 320f,
				required_temp_range = new MathUtil.MinMax(0f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.DirtyWater).idx,
				mass_kg = 400f,
				required_temp_range = new MathUtil.MinMax(0f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Rust).idx,
				mass_kg = 125f,
				required_temp_range = new MathUtil.MinMax(330f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenLead).idx,
				mass_kg = 65f,
				required_temp_range = new MathUtil.MinMax(540f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.SulfurGas).idx,
				mass_kg = 30f,
				required_temp_range = new MathUtil.MinMax(700f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.SourGas).idx,
				mass_kg = 200f,
				required_temp_range = new MathUtil.MinMax(800f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.IronOre).idx,
				mass_kg = 50f,
				required_temp_range = new MathUtil.MinMax(850f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenAluminum).idx,
				mass_kg = 100f,
				required_temp_range = new MathUtil.MinMax(1200f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenCopper).idx,
				mass_kg = 100f,
				required_temp_range = new MathUtil.MinMax(1300f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenGold).idx,
				mass_kg = 100f,
				required_temp_range = new MathUtil.MinMax(1400f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Magma).idx,
				mass_kg = 75f,
				required_temp_range = new MathUtil.MinMax(1800f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Hydrogen).idx,
				mass_kg = 50f,
				required_temp_range = new MathUtil.MinMax(1800f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenIron).idx,
				mass_kg = 250f,
				required_temp_range = new MathUtil.MinMax(1900f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Wolframite).idx,
				mass_kg = 275f,
				required_temp_range = new MathUtil.MinMax(2000f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Fullerene).idx,
				mass_kg = 3f,
				required_temp_range = new MathUtil.MinMax(2500f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Niobium).idx,
				mass_kg = 5f,
				required_temp_range = new MathUtil.MinMax(2500f, float.MaxValue)
			}
		};
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x00046947 File Offset: 0x00044B47
	public static float CalculateOutputTemperature(float inputTemperature)
	{
		if (inputTemperature < 1650f)
		{
			return Math.Min(1650f, inputTemperature + 150f);
		}
		return Math.Max(1650f, inputTemperature - 150f);
	}

	// Token: 0x06000BA2 RID: 2978 RVA: 0x00046974 File Offset: 0x00044B74
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x0004697B File Offset: 0x00044B7B
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000BA4 RID: 2980 RVA: 0x00046980 File Offset: 0x00044B80
	GameObject IEntityConfig.CreatePrefab()
	{
		string id = "GeothermalControllerEntity";
		string name = STRINGS.BUILDINGS.PREFABS.GEOTHERMALCONTROLLER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.GEOTHERMALCONTROLLER.EFFECT + "\n\n" + STRINGS.BUILDINGS.PREFABS.GEOTHERMALCONTROLLER.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.PENALTY.TIER4;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_geoplant_kanim"), "off", Grid.SceneLayer.BuildingBack, 7, 8, tier, tier2, SimHashes.Unobtanium, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddComponent<EntityCellVisualizer>();
		gameObject.AddComponent<GeothermalController>();
		gameObject.AddComponent<GeothermalPlantComponent>();
		gameObject.AddComponent<Operational>();
		gameObject.AddComponent<GeothermalController.ReconnectPipes>();
		gameObject.AddComponent<Notifier>();
		Storage storage = gameObject.AddComponent<Storage>();
		storage.showDescriptor = false;
		storage.showInUI = false;
		storage.capacityKg = 12000f;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Seal
		});
		return gameObject;
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x00046A83 File Offset: 0x00044C83
	void IEntityConfig.OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x00046A85 File Offset: 0x00044C85
	void IEntityConfig.OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000801 RID: 2049
	public const string ID = "GeothermalControllerEntity";

	// Token: 0x04000802 RID: 2050
	public const string KEEPSAKE_ID = "keepsake_geothermalplant";

	// Token: 0x04000803 RID: 2051
	public const string COMPLETED_LORE_ENTRY_UNLOCK_ID = "notes_earthquake";

	// Token: 0x04000804 RID: 2052
	private const string ANIM_FILE = "gravitas_geoplant_kanim";

	// Token: 0x04000805 RID: 2053
	public const string OFFLINE_ANIM = "off";

	// Token: 0x04000806 RID: 2054
	public const string ONLINE_ANIM = "on";

	// Token: 0x04000807 RID: 2055
	public const string OBSTRUCTED_ANIM = "on";

	// Token: 0x04000808 RID: 2056
	public const float WORKING_LOOP_DURATION_SECONDS = 16f;

	// Token: 0x04000809 RID: 2057
	public const float HEATPUMP_CAPACITY_KG = 12000f;

	// Token: 0x0400080A RID: 2058
	public const float OUTPUT_TARGET_TEMPERATURE = 1650f;

	// Token: 0x0400080B RID: 2059
	public const float OUTPUT_DELTA_TEMPERATURE = 150f;

	// Token: 0x0400080C RID: 2060
	public const float OUTPUT_PASSTHROUGH_RATIO = 0.92f;

	// Token: 0x0400080D RID: 2061
	public static MathUtil.MinMax OUTPUT_VENT_WEIGHT_RANGE = new MathUtil.MinMax(43f, 57f);

	// Token: 0x0400080E RID: 2062
	public static HashSet<Tag> STEEL_FETCH_TAGS = new HashSet<Tag>
	{
		GameTags.Steel
	};

	// Token: 0x0400080F RID: 2063
	public const float STEEL_FETCH_QUANTITY_KG = 1200f;

	// Token: 0x04000810 RID: 2064
	public const float RECONNECT_PUMP_CHORE_DURATION_SECONDS = 5f;

	// Token: 0x04000811 RID: 2065
	public static HashedString RECONNECT_PUMP_ANIM_OVERRIDE = "anim_use_remote_kanim";

	// Token: 0x04000812 RID: 2066
	public const string BAROMETER_ANIM = "meter";

	// Token: 0x04000813 RID: 2067
	public const string BAROMETER_TARGET = "meter_target";

	// Token: 0x04000814 RID: 2068
	public static string[] BAROMETER_SYMBOLS = new string[]
	{
		"meter_target"
	};

	// Token: 0x04000815 RID: 2069
	public const string THERMOMETER_ANIM = "meter_temp";

	// Token: 0x04000816 RID: 2070
	public const string THERMOMETER_TARGET = "meter_target";

	// Token: 0x04000817 RID: 2071
	public static string[] THERMOMETER_SYMBOLS = new string[]
	{
		"meter_target"
	};

	// Token: 0x04000818 RID: 2072
	public const float THERMOMETER_MIN_TEMP = 50f;

	// Token: 0x04000819 RID: 2073
	public const float THERMOMETER_RANGE = 2450f;

	// Token: 0x0400081A RID: 2074
	public static HashedString[] PRESSURE_ANIM_LOOPS = new HashedString[]
	{
		"pressure_loop",
		"high_pressure_loop",
		"high_pressure_loop2"
	};

	// Token: 0x0400081B RID: 2075
	public static float[] PRESSURE_ANIM_THRESHOLDS = new float[]
	{
		0f,
		0.35f,
		0.85f
	};

	// Token: 0x0400081C RID: 2076
	public const float CLEAR_ENTOMBED_VENT_THRESHOLD_TEMPERATURE = 602f;

	// Token: 0x020011E7 RID: 4583
	public struct Impurity
	{
		// Token: 0x04006632 RID: 26162
		public ushort elementIdx;

		// Token: 0x04006633 RID: 26163
		public float mass_kg;

		// Token: 0x04006634 RID: 26164
		public MathUtil.MinMax required_temp_range;
	}
}
