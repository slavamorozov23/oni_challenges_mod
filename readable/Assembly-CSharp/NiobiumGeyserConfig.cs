using System;
using STRINGS;
using UnityEngine;

// Token: 0x020001AF RID: 431
public class NiobiumGeyserConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600086D RID: 2157 RVA: 0x00038E82 File Offset: 0x00037082
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x00038E89 File Offset: 0x00037089
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x00038E8C File Offset: 0x0003708C
	public GameObject CreatePrefab()
	{
		GeyserConfigurator.GeyserType geyserType = new GeyserConfigurator.GeyserType("molten_niobium", SimHashes.MoltenNiobium, GeyserConfigurator.GeyserShape.Molten, 3500f, 800f, 1600f, 150f, null, null, 6000f, 12000f, 0.005f, 0.01f, 15000f, 135000f, 0.4f, 0.8f, 372.15f);
		GameObject gameObject = GeyserGenericConfig.CreateGeyser("NiobiumGeyser", "geyser_molten_niobium_kanim", 3, 3, CREATURES.SPECIES.GEYSER.MOLTEN_NIOBIUM.NAME, CREATURES.SPECIES.GEYSER.MOLTEN_NIOBIUM.DESC, geyserType.idHash, geyserType.geyserTemperature, DlcManager.EXPANSION1, null);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent, false);
		return gameObject;
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x00038F35 File Offset: 0x00037135
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x00038F37 File Offset: 0x00037137
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400065A RID: 1626
	public const string ID = "NiobiumGeyser";
}
