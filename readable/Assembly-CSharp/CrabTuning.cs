using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000A7 RID: 167
public static class CrabTuning
{
	// Token: 0x06000336 RID: 822 RVA: 0x00017F63 File Offset: 0x00016163
	public static bool IsReadyToMolt(MoltDropperMonitor.Instance smi)
	{
		return CrabTuning.IsValidTimeToDrop(smi) && CrabTuning.IsValidDropCell(smi) && !smi.prefabID.HasTag(GameTags.Creatures.Hungry) && smi.prefabID.HasTag(GameTags.Creatures.Happy);
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00017F99 File Offset: 0x00016199
	public static bool IsValidTimeToDrop(MoltDropperMonitor.Instance smi)
	{
		return !smi.spawnedThisCycle && (smi.timeOfLastDrop <= 0f || GameClock.Instance.GetTime() - smi.timeOfLastDrop > 600f);
	}

	// Token: 0x06000338 RID: 824 RVA: 0x00017FCC File Offset: 0x000161CC
	public static bool IsValidDropCell(MoltDropperMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi.transform.GetPosition());
		return Grid.IsValidCell(num) && Grid.Element[num].id != SimHashes.Ethanol;
	}

	// Token: 0x04000205 RID: 517
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabEgg".ToTag(),
			weight = 0.97f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabWoodEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabFreshWaterEgg".ToTag(),
			weight = 0.01f
		}
	};

	// Token: 0x04000206 RID: 518
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_WOOD = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabWoodEgg".ToTag(),
			weight = 0.65f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabFreshWaterEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000207 RID: 519
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_FRESH = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabWoodEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabFreshWaterEgg".ToTag(),
			weight = 0.65f
		}
	};

	// Token: 0x04000208 RID: 520
	public static float STANDARD_CALORIES_PER_CYCLE = 100000f;

	// Token: 0x04000209 RID: 521
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x0400020A RID: 522
	public static float STANDARD_STOMACH_SIZE = CrabTuning.STANDARD_CALORIES_PER_CYCLE * CrabTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x0400020B RID: 523
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x0400020C RID: 524
	public static float EGG_MASS = 2f;

	// Token: 0x0400020D RID: 525
	public static CellOffset[] DEFEND_OFFSETS = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 1),
		new CellOffset(-1, 1)
	};
}
