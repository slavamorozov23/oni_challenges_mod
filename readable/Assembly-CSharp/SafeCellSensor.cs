using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x02000527 RID: 1319
public class SafeCellSensor : Sensor
{
	// Token: 0x06001C72 RID: 7282 RVA: 0x0009C7E8 File Offset: 0x0009A9E8
	private SafeCellQuery.SafeFlags GetIgnoredFlags()
	{
		SafeCellQuery.SafeFlags safeFlags = (SafeCellQuery.SafeFlags)0;
		foreach (string key in this.ignoredFlagsSets.Keys)
		{
			SafeCellQuery.SafeFlags safeFlags2 = this.ignoredFlagsSets[key];
			safeFlags |= safeFlags2;
		}
		return safeFlags;
	}

	// Token: 0x06001C73 RID: 7283 RVA: 0x0009C850 File Offset: 0x0009AA50
	public void AddIgnoredFlagsSet(string setID, SafeCellQuery.SafeFlags flagsToIgnore)
	{
		if (this.ignoredFlagsSets.ContainsKey(setID))
		{
			this.ignoredFlagsSets[setID] = flagsToIgnore;
			return;
		}
		this.ignoredFlagsSets.Add(setID, flagsToIgnore);
	}

	// Token: 0x06001C74 RID: 7284 RVA: 0x0009C87B File Offset: 0x0009AA7B
	public void RemoveIgnoredFlagsSet(string setID)
	{
		if (this.ignoredFlagsSets.ContainsKey(setID))
		{
			this.ignoredFlagsSets.Remove(setID);
		}
	}

	// Token: 0x06001C75 RID: 7285 RVA: 0x0009C898 File Offset: 0x0009AA98
	public SafeCellSensor(Sensors sensors, bool startEnabled = true) : base(sensors, startEnabled)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.brain = base.GetComponent<MinionBrain>();
		this.prefabid = base.GetComponent<KPrefabID>();
		this.traits = base.GetComponent<Traits>();
	}

	// Token: 0x06001C76 RID: 7286 RVA: 0x0009C8F4 File Offset: 0x0009AAF4
	public override void Update()
	{
		if (!this.prefabid.HasTag(GameTags.Idle))
		{
			this.cell = Grid.InvalidCell;
			return;
		}
		bool flag = this.HasSafeCell();
		this.RunSafeCellQuery(false);
		bool flag2 = this.HasSafeCell();
		if (flag2 != flag)
		{
			if (flag2)
			{
				this.sensors.Trigger(982561777, null);
				return;
			}
			this.sensors.Trigger(506919987, null);
		}
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x0009C95E File Offset: 0x0009AB5E
	public void RunSafeCellQuery(bool avoid_light)
	{
		this.cell = this.RunAndGetSafeCellQueryResult(avoid_light);
		if (this.cell == Grid.PosToCell(this.navigator))
		{
			this.cell = Grid.InvalidCell;
		}
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x0009C98C File Offset: 0x0009AB8C
	public int RunAndGetSafeCellQueryResult(bool avoid_light)
	{
		MinionPathFinderAbilities minionPathFinderAbilities = (MinionPathFinderAbilities)this.navigator.GetCurrentAbilities();
		minionPathFinderAbilities.SetIdleNavMaskEnabled(true);
		SafeCellQuery safeCellQuery = PathFinderQueries.safeCellQuery.Reset(this.brain, avoid_light, this.GetIgnoredFlags());
		this.navigator.RunQuery(safeCellQuery);
		minionPathFinderAbilities.SetIdleNavMaskEnabled(false);
		this.cell = safeCellQuery.GetResultCell();
		return this.cell;
	}

	// Token: 0x06001C79 RID: 7289 RVA: 0x0009C9EC File Offset: 0x0009ABEC
	public int GetSensorCell()
	{
		return this.cell;
	}

	// Token: 0x06001C7A RID: 7290 RVA: 0x0009C9F4 File Offset: 0x0009ABF4
	public int GetCellQuery()
	{
		if (this.cell == Grid.InvalidCell)
		{
			this.RunSafeCellQuery(false);
		}
		return this.cell;
	}

	// Token: 0x06001C7B RID: 7291 RVA: 0x0009CA10 File Offset: 0x0009AC10
	public int GetSleepCellQuery()
	{
		if (this.cell == Grid.InvalidCell)
		{
			this.RunSafeCellQuery(!this.traits.HasTrait("NightLight"));
		}
		return this.cell;
	}

	// Token: 0x06001C7C RID: 7292 RVA: 0x0009CA41 File Offset: 0x0009AC41
	public bool HasSafeCell()
	{
		return this.cell != Grid.InvalidCell && this.cell != Grid.PosToCell(this.sensors);
	}

	// Token: 0x040010CB RID: 4299
	private MinionBrain brain;

	// Token: 0x040010CC RID: 4300
	private Navigator navigator;

	// Token: 0x040010CD RID: 4301
	private KPrefabID prefabid;

	// Token: 0x040010CE RID: 4302
	private Traits traits;

	// Token: 0x040010CF RID: 4303
	private int cell = Grid.InvalidCell;

	// Token: 0x040010D0 RID: 4304
	private Dictionary<string, SafeCellQuery.SafeFlags> ignoredFlagsSets = new Dictionary<string, SafeCellQuery.SafeFlags>();
}
