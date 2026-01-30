using System;
using TUNING;
using UnityEngine;

// Token: 0x0200028B RID: 651
public class LandingBeaconConfig : IBuildingConfig
{
	// Token: 0x06000D36 RID: 3382 RVA: 0x0004E79C File Offset: 0x0004C99C
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000D37 RID: 3383 RVA: 0x0004E7A4 File Offset: 0x0004C9A4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LandingBeacon";
		int width = 1;
		int height = 3;
		string anim = "landing_beacon_kanim";
		int hitpoints = 1000;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.DefaultAnimState = "off";
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 398.15f;
		buildingDef.Floodable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		return buildingDef;
	}

	// Token: 0x06000D38 RID: 3384 RVA: 0x0004E84A File Offset: 0x0004CA4A
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGetDef<LandingBeacon.Def>();
	}

	// Token: 0x06000D39 RID: 3385 RVA: 0x0004E86B File Offset: 0x0004CA6B
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LandingBeaconConfig.AddVisualizer(go);
	}

	// Token: 0x06000D3A RID: 3386 RVA: 0x0004E873 File Offset: 0x0004CA73
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		LandingBeaconConfig.AddVisualizer(go);
	}

	// Token: 0x06000D3B RID: 3387 RVA: 0x0004E87B File Offset: 0x0004CA7B
	public override void DoPostConfigureComplete(GameObject go)
	{
		LandingBeaconConfig.AddVisualizer(go);
	}

	// Token: 0x06000D3C RID: 3388 RVA: 0x0004E883 File Offset: 0x0004CA83
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.RangeMin = 0;
		skyVisibilityVisualizer.RangeMax = 0;
		prefab.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<SkyVisibilityVisualizer>().SkyVisibilityCb = new Func<int, bool>(LandingBeaconConfig.BeaconSkyVisibility);
		};
	}

	// Token: 0x06000D3D RID: 3389 RVA: 0x0004E8C4 File Offset: 0x0004CAC4
	private static bool BeaconSkyVisibility(int cell)
	{
		DebugUtil.DevAssert(ClusterManager.Instance != null, "beacon assumes DLC", null);
		if (Grid.IsValidCell(cell) && Grid.WorldIdx[cell] != 255)
		{
			int num = (int)ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cell]).maximumBounds.y;
			int num2 = cell;
			while (Grid.CellRow(num2) <= num)
			{
				if (!Grid.IsValidCell(num2) || Grid.Solid[num2])
				{
					return false;
				}
				num2 = Grid.CellAbove(num2);
			}
			return true;
		}
		return false;
	}

	// Token: 0x04000909 RID: 2313
	public const string ID = "LandingBeacon";

	// Token: 0x0400090A RID: 2314
	public const int LANDING_ACCURACY = 3;
}
