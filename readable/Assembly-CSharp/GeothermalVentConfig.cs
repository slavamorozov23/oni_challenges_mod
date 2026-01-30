using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000240 RID: 576
public class GeothermalVentConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000BA9 RID: 2985 RVA: 0x00046B55 File Offset: 0x00044D55
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000BAA RID: 2986 RVA: 0x00046B5C File Offset: 0x00044D5C
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x00046B60 File Offset: 0x00044D60
	public virtual GameObject CreatePrefab()
	{
		string id = "GeothermalVentEntity";
		string name = STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.EFFECT + "\n\n" + STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.PENALTY.TIER4;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_geospout_kanim"), "off", Grid.SceneLayer.BuildingBack, 3, 4, tier, tier2, SimHashes.Unobtanium, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddComponent<GeothermalVent>();
		gameObject.AddComponent<GeothermalPlantComponent>();
		gameObject.AddComponent<Operational>();
		gameObject.AddComponent<UserNameable>();
		Storage storage = gameObject.AddComponent<Storage>();
		storage.showCapacityAsMainStatus = false;
		storage.showCapacityStatusItem = false;
		storage.showDescriptor = false;
		return gameObject;
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x00046C34 File Offset: 0x00044E34
	public void OnPrefabInit(GameObject inst)
	{
		LogicPorts logicPorts = inst.AddOrGet<LogicPorts>();
		logicPorts.inputPortInfo = new LogicPorts.Port[0];
		logicPorts.outputPortInfo = new LogicPorts.Port[]
		{
			LogicPorts.Port.OutputPort("GEOTHERMAL_VENT_STATUS_PORT", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.LOGIC_PORT_INACTIVE, false, false)
		};
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x00046C9B File Offset: 0x00044E9B
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400081D RID: 2077
	public const string ID = "GeothermalVentEntity";

	// Token: 0x0400081E RID: 2078
	public const string OUTPUT_LOGIC_PORT_ID = "GEOTHERMAL_VENT_STATUS_PORT";

	// Token: 0x0400081F RID: 2079
	private const string ANIM_FILE = "gravitas_geospout_kanim";

	// Token: 0x04000820 RID: 2080
	public const string OFFLINE_ANIM = "off";

	// Token: 0x04000821 RID: 2081
	public const string QUEST_ENTOMBED_ANIM = "pooped";

	// Token: 0x04000822 RID: 2082
	public const string IDLE_ANIM = "on";

	// Token: 0x04000823 RID: 2083
	public const string OBSTRUCTED_ANIM = "over_pressure";

	// Token: 0x04000824 RID: 2084
	public const int EMISSION_RANGE = 1;

	// Token: 0x04000825 RID: 2085
	public const float EMISSION_INTERVAL_SEC = 0.2f;

	// Token: 0x04000826 RID: 2086
	public const float EMISSION_MAX_PRESSURE_KG = 120f;

	// Token: 0x04000827 RID: 2087
	public const float EMISSION_MAX_RATE_PER_TICK = 3f;

	// Token: 0x04000828 RID: 2088
	public static string TOGGLE_ANIMATION = "working_loop";

	// Token: 0x04000829 RID: 2089
	public static HashedString TOGGLE_ANIM_OVERRIDE = "anim_interacts_geospout_kanim";

	// Token: 0x0400082A RID: 2090
	public const float TOGGLE_CHORE_DURATION_SECONDS = 10f;

	// Token: 0x0400082B RID: 2091
	public static MathUtil.MinMax INITIAL_DEBRIS_VELOCIOTY = new MathUtil.MinMax(1f, 5f);

	// Token: 0x0400082C RID: 2092
	public static MathUtil.MinMax INITIAL_DEBRIS_ANGLE = new MathUtil.MinMax(200f, 340f);

	// Token: 0x0400082D RID: 2093
	public static MathUtil.MinMax DEBRIS_MASS_KG = new MathUtil.MinMax(30f, 34f);

	// Token: 0x0400082E RID: 2094
	public const string BAROMETER_ANIM = "meter";

	// Token: 0x0400082F RID: 2095
	public const string BAROMETER_TARGET = "meter_target";

	// Token: 0x04000830 RID: 2096
	public static string[] BAROMETER_SYMBOLS = new string[]
	{
		"meter_target"
	};

	// Token: 0x04000831 RID: 2097
	public const string CONNECTED_ANIM = "meter_connected";

	// Token: 0x04000832 RID: 2098
	public const string CONNECTED_TARGET = "meter_connected_target";

	// Token: 0x04000833 RID: 2099
	public static string[] CONNECTED_SYMBOLS = new string[]
	{
		"meter_connected_target"
	};

	// Token: 0x04000834 RID: 2100
	public const float CONNECTED_PROGRESS = 1f;

	// Token: 0x04000835 RID: 2101
	public const float DISCONNECTED_PROGRESS = 0f;
}
