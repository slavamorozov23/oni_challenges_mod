using System;
using TUNING;
using UnityEngine;

// Token: 0x0200045C RID: 1116
public class WarpConduitReceiverConfig : IBuildingConfig
{
	// Token: 0x0600174F RID: 5967 RVA: 0x00084878 File Offset: 0x00082A78
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001750 RID: 5968 RVA: 0x00084880 File Offset: 0x00082A80
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WarpConduitReceiver";
		int width = 4;
		int height = 3;
		string anim = "warp_conduit_receiver_kanim";
		int hitpoints = 250;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.DefaultAnimState = "off";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ShowInBuildMenu = false;
		buildingDef.Disinfectable = false;
		buildingDef.Invincible = true;
		buildingDef.Repairable = false;
		return buildingDef;
	}

	// Token: 0x06001751 RID: 5969 RVA: 0x000848FE File Offset: 0x00082AFE
	private void AttachPorts(GameObject go)
	{
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.liquidOutputPort;
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.gasOutputPort;
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.solidOutputPort;
	}

	// Token: 0x06001752 RID: 5970 RVA: 0x00084934 File Offset: 0x00082B34
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		WarpConduitReceiver warpConduitReceiver = go.AddOrGet<WarpConduitReceiver>();
		warpConduitReceiver.liquidPortInfo = this.liquidOutputPort;
		warpConduitReceiver.gasPortInfo = this.gasOutputPort;
		warpConduitReceiver.solidPortInfo = this.solidOutputPort;
		Activatable activatable = go.AddOrGet<Activatable>();
		activatable.synchronizeAnims = true;
		activatable.workAnims = new HashedString[]
		{
			"touchpanel_interact_pre",
			"touchpanel_interact_loop"
		};
		activatable.workingPstComplete = new HashedString[]
		{
			"touchpanel_interact_pst"
		};
		activatable.workingPstFailed = new HashedString[]
		{
			"touchpanel_interact_pst"
		};
		activatable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_warp_conduit_receiver_kanim")
		};
		activatable.SetWorkTime(30f);
		go.AddComponent<ConduitSecondaryOutput>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.Gravitas, false);
	}

	// Token: 0x06001753 RID: 5971 RVA: 0x00084A43 File Offset: 0x00082C43
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<BuildingCellVisualizer>();
		go.GetComponent<Deconstructable>().SetAllowDeconstruction(false);
		go.GetComponent<Activatable>().requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
	}

	// Token: 0x06001754 RID: 5972 RVA: 0x00084A77 File Offset: 0x00082C77
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		go.AddOrGet<BuildingCellVisualizer>();
		this.AttachPorts(go);
	}

	// Token: 0x06001755 RID: 5973 RVA: 0x00084A8F File Offset: 0x00082C8F
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<BuildingCellVisualizer>();
		this.AttachPorts(go);
	}

	// Token: 0x04000DBC RID: 3516
	public const string ID = "WarpConduitReceiver";

	// Token: 0x04000DBD RID: 3517
	private ConduitPortInfo liquidOutputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 1));

	// Token: 0x04000DBE RID: 3518
	private ConduitPortInfo gasOutputPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(-1, 1));

	// Token: 0x04000DBF RID: 3519
	private ConduitPortInfo solidOutputPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(1, 1));
}
