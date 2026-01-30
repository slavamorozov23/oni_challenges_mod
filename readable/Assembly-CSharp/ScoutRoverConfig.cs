using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000166 RID: 358
public class ScoutRoverConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060006DE RID: 1758 RVA: 0x00030DC3 File Offset: 0x0002EFC3
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x00030DCA File Offset: 0x0002EFCA
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x00030DD0 File Offset: 0x0002EFD0
	public GameObject CreatePrefab()
	{
		return BaseRoverConfig.BaseRover("ScoutRover", STRINGS.ROBOTS.MODELS.SCOUT.NAME, GameTags.Robots.Models.ScoutRover, STRINGS.ROBOTS.MODELS.SCOUT.DESC, "scout_bot_kanim", 100f, 1f, 2f, TUNING.ROBOTS.SCOUTBOT.CARRY_CAPACITY, TUNING.ROBOTS.SCOUTBOT.DIGGING, TUNING.ROBOTS.SCOUTBOT.CONSTRUCTION, TUNING.ROBOTS.SCOUTBOT.ATHLETICS, TUNING.ROBOTS.SCOUTBOT.HIT_POINTS, TUNING.ROBOTS.SCOUTBOT.BATTERY_CAPACITY, TUNING.ROBOTS.SCOUTBOT.BATTERY_DEPLETION_RATE, Db.Get().Amounts.InternalChemicalBattery, false);
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x00030E47 File Offset: 0x0002F047
	public void OnPrefabInit(GameObject inst)
	{
		BaseRoverConfig.OnPrefabInit(inst, Db.Get().Amounts.InternalChemicalBattery);
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x00030E60 File Offset: 0x0002F060
	public void OnSpawn(GameObject inst)
	{
		BaseRoverConfig.OnSpawn(inst);
		Effects effects = inst.GetComponent<Effects>();
		if (inst.transform.parent == null)
		{
			if (effects.HasEffect("ScoutBotCharging"))
			{
				effects.Remove("ScoutBotCharging");
			}
		}
		else if (!effects.HasEffect("ScoutBotCharging"))
		{
			effects.Add("ScoutBotCharging", false);
		}
		inst.Subscribe(856640610, delegate(object _)
		{
			if (inst.transform.parent == null)
			{
				if (effects.HasEffect("ScoutBotCharging"))
				{
					effects.Remove("ScoutBotCharging");
					return;
				}
			}
			else if (!effects.HasEffect("ScoutBotCharging"))
			{
				effects.Add("ScoutBotCharging", false);
			}
		});
	}

	// Token: 0x0400053A RID: 1338
	public const string ID = "ScoutRover";

	// Token: 0x0400053B RID: 1339
	public const float MASS = 100f;

	// Token: 0x0400053C RID: 1340
	private const float WIDTH = 1f;

	// Token: 0x0400053D RID: 1341
	private const float HEIGHT = 2f;

	// Token: 0x0400053E RID: 1342
	public const int MAXIMUM_TECH_CONSTRUCTION_TIER = 2;
}
