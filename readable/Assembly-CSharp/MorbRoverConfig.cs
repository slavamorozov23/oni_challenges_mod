using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000149 RID: 329
public class MorbRoverConfig : IEntityConfig
{
	// Token: 0x06000642 RID: 1602 RVA: 0x0002E910 File Offset: 0x0002CB10
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseRoverConfig.BaseRover("MorbRover", STRINGS.ROBOTS.MODELS.MORB.NAME, GameTags.Robots.Models.MorbRover, STRINGS.ROBOTS.MODELS.MORB.DESC, "morbRover_kanim", 300f, 1f, 2f, TUNING.ROBOTS.MORBBOT.CARRY_CAPACITY, 1f, 1f, 3f, TUNING.ROBOTS.MORBBOT.HIT_POINTS, 180000f, 30f, Db.Get().Amounts.InternalBioBattery, false);
		gameObject.GetComponent<PrimaryElement>().SetElement(SimHashes.Steel, false);
		gameObject.GetComponent<Deconstructable>().customWorkTime = 10f;
		gameObject.AddOrGet<CodexEntryRedirector>().CodexID = "STORYTRAITMORBROVER";
		return gameObject;
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x0002E9B8 File Offset: 0x0002CBB8
	public void OnPrefabInit(GameObject inst)
	{
		BaseRoverConfig.OnPrefabInit(inst, Db.Get().Amounts.InternalBioBattery);
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x0002E9CF File Offset: 0x0002CBCF
	public void OnSpawn(GameObject inst)
	{
		BaseRoverConfig.OnSpawn(inst);
		inst.Subscribe(1623392196, new Action<object>(this.TriggerDeconstructChoreOnDeath));
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x0002E9F0 File Offset: 0x0002CBF0
	public void TriggerDeconstructChoreOnDeath(object obj)
	{
		if (obj != null)
		{
			Deconstructable component = ((GameObject)obj).GetComponent<Deconstructable>();
			if (!component.IsMarkedForDeconstruction())
			{
				component.QueueDeconstruction(false);
			}
		}
	}

	// Token: 0x040004B9 RID: 1209
	public const string ID = "MorbRover";

	// Token: 0x040004BA RID: 1210
	public const SimHashes MATERIAL = SimHashes.Steel;

	// Token: 0x040004BB RID: 1211
	public const float MASS = 300f;

	// Token: 0x040004BC RID: 1212
	private const float WIDTH = 1f;

	// Token: 0x040004BD RID: 1213
	private const float HEIGHT = 2f;
}
