using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002FC RID: 764
public class ArtifactPOIConfig : IMultiEntityConfig
{
	// Token: 0x06000F8A RID: 3978 RVA: 0x0005B87C File Offset: 0x00059A7C
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (ArtifactPOIConfig.ArtifactPOIParams artifactPOIParams in this.GenerateConfigs())
		{
			list.Add(ArtifactPOIConfig.CreateArtifactPOI(artifactPOIParams.id, artifactPOIParams.anim, Strings.Get(artifactPOIParams.nameStringKey), Strings.Get(artifactPOIParams.descStringKey), artifactPOIParams.poiType.idHash, artifactPOIParams.poiType.initialDatabankCount));
		}
		return list;
	}

	// Token: 0x06000F8B RID: 3979 RVA: 0x0005B91C File Offset: 0x00059B1C
	public static GameObject CreateArtifactPOI(string id, string anim, string name, string desc, HashedString poiType)
	{
		return ArtifactPOIConfig.CreateArtifactPOI(id, anim, name, desc, poiType, 0);
	}

	// Token: 0x06000F8C RID: 3980 RVA: 0x0005B92C File Offset: 0x00059B2C
	public static GameObject CreateArtifactPOI(string id, string anim, string name, string desc, HashedString poiType, int initialDatabankCount)
	{
		GameObject gameObject = EntityTemplates.CreateEntity(id, id, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<ArtifactPOIConfigurator>().presetType = poiType;
		ArtifactPOIClusterGridEntity artifactPOIClusterGridEntity = gameObject.AddOrGet<ArtifactPOIClusterGridEntity>();
		artifactPOIClusterGridEntity.m_name = name;
		artifactPOIClusterGridEntity.m_Anim = anim;
		if (initialDatabankCount > 0)
		{
			gameObject.AddOrGetDef<ClusterGridOneTimeResourceSpawner.Def>().thingsToSpawn = new List<ClusterGridOneTimeResourceSpawner.Data>
			{
				new ClusterGridOneTimeResourceSpawner.Data
				{
					itemID = DatabankHelper.ID,
					mass = 1f * (float)initialDatabankCount
				}
			};
		}
		gameObject.AddOrGetDef<ArtifactPOIStates.Def>();
		gameObject.AddOrGet<InfoDescription>().description = desc;
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextSpaceEntry));
		return gameObject;
	}

	// Token: 0x06000F8D RID: 3981 RVA: 0x0005B9D4 File Offset: 0x00059BD4
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F8E RID: 3982 RVA: 0x0005B9D6 File Offset: 0x00059BD6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000F8F RID: 3983 RVA: 0x0005B9D8 File Offset: 0x00059BD8
	private List<ArtifactPOIConfig.ArtifactPOIParams> GenerateConfigs()
	{
		List<ArtifactPOIConfig.ArtifactPOIParams> list = new List<ArtifactPOIConfig.ArtifactPOIParams>();
		if (!DlcManager.IsExpansion1Active())
		{
			return list;
		}
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_1", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation1", 50, null, false, 30000f, 60000f, DlcManager.EXPANSION1, null)));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_2", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation2", 50, null, false, 30000f, 60000f, DlcManager.EXPANSION1, null)));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_3", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation3", 50, null, false, 30000f, 60000f, DlcManager.EXPANSION1, null)));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_4", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation4", 50, null, false, 30000f, 60000f, DlcManager.EXPANSION1, null)));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_5", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation5", 50, null, false, 30000f, 60000f, DlcManager.EXPANSION1, null)));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_6", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation6", 50, null, false, 30000f, 60000f, DlcManager.EXPANSION1, null)));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_7", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation7", 50, null, false, 30000f, 60000f, DlcManager.EXPANSION1, null)));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_8", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation8", 50, null, false, 30000f, 60000f, DlcManager.EXPANSION1, null)));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("russels_teapot", new ArtifactPOIConfigurator.ArtifactPOIType("RussellsTeapot", "artifact_TeaPot", true, 30000f, 60000f, DlcManager.EXPANSION1, null)));
		list.RemoveAll((ArtifactPOIConfig.ArtifactPOIParams poi) => !DlcManager.IsCorrectDlcSubscribed(poi.poiType));
		return list;
	}

	// Token: 0x04000A2B RID: 2603
	public const int DEFAULT_INITIAL_DATABANK_COUNT = 50;

	// Token: 0x04000A2C RID: 2604
	public const string GravitasSpaceStation1 = "GravitasSpaceStation1";

	// Token: 0x04000A2D RID: 2605
	public const string GravitasSpaceStation2 = "GravitasSpaceStation2";

	// Token: 0x04000A2E RID: 2606
	public const string GravitasSpaceStation3 = "GravitasSpaceStation3";

	// Token: 0x04000A2F RID: 2607
	public const string GravitasSpaceStation4 = "GravitasSpaceStation4";

	// Token: 0x04000A30 RID: 2608
	public const string GravitasSpaceStation5 = "GravitasSpaceStation5";

	// Token: 0x04000A31 RID: 2609
	public const string GravitasSpaceStation6 = "GravitasSpaceStation6";

	// Token: 0x04000A32 RID: 2610
	public const string GravitasSpaceStation7 = "GravitasSpaceStation7";

	// Token: 0x04000A33 RID: 2611
	public const string GravitasSpaceStation8 = "GravitasSpaceStation8";

	// Token: 0x04000A34 RID: 2612
	public const string RussellsTeapot = "RussellsTeapot";

	// Token: 0x0200120B RID: 4619
	public struct ArtifactPOIParams
	{
		// Token: 0x0600869D RID: 34461 RVA: 0x0034A6E8 File Offset: 0x003488E8
		public ArtifactPOIParams(string anim, ArtifactPOIConfigurator.ArtifactPOIType poiType)
		{
			this.id = "ArtifactSpacePOI_" + poiType.id;
			this.anim = anim;
			this.nameStringKey = new StringKey("STRINGS.UI.SPACEDESTINATIONS.ARTIFACT_POI." + poiType.id.ToUpper() + ".NAME");
			this.descStringKey = new StringKey("STRINGS.UI.SPACEDESTINATIONS.ARTIFACT_POI." + poiType.id.ToUpper() + ".DESC");
			this.poiType = poiType;
		}

		// Token: 0x0400668E RID: 26254
		public string id;

		// Token: 0x0400668F RID: 26255
		public string anim;

		// Token: 0x04006690 RID: 26256
		public StringKey nameStringKey;

		// Token: 0x04006691 RID: 26257
		public StringKey descStringKey;

		// Token: 0x04006692 RID: 26258
		public ArtifactPOIConfigurator.ArtifactPOIType poiType;
	}
}
