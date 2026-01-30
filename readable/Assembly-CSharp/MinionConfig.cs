using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000339 RID: 825
public class MinionConfig : IEntityConfig
{
	// Token: 0x0600110D RID: 4365 RVA: 0x00065688 File Offset: 0x00063888
	public static string[] GetAttributes()
	{
		return BaseMinionConfig.BaseMinionAttributes().Append(new string[]
		{
			Db.Get().Attributes.FoodExpectation.Id,
			Db.Get().Attributes.ToiletEfficiency.Id
		});
	}

	// Token: 0x0600110E RID: 4366 RVA: 0x000656C8 File Offset: 0x000638C8
	public static string[] GetAmounts()
	{
		return BaseMinionConfig.BaseMinionAmounts().Append(new string[]
		{
			Db.Get().Amounts.Bladder.Id,
			Db.Get().Amounts.Stamina.Id,
			Db.Get().Amounts.Calories.Id
		});
	}

	// Token: 0x0600110F RID: 4367 RVA: 0x0006572C File Offset: 0x0006392C
	public static AttributeModifier[] GetTraits()
	{
		return BaseMinionConfig.BaseMinionTraits(MinionConfig.MODEL).Append(new AttributeModifier[]
		{
			new AttributeModifier(Db.Get().Attributes.FoodExpectation.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.FOOD_QUALITY_EXPECTATION, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.MAX_CALORIES, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.CALORIES_BURNED_PER_SECOND, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Amounts.Stamina.deltaAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.STAMINA_USED_PER_SECOND, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Amounts.Bladder.deltaAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.BLADDER_INCREASE_PER_SECOND, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Attributes.ToiletEfficiency.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.TOILET_EFFICIENCY, MinionConfig.NAME, false, false, true)
		});
	}

	// Token: 0x06001110 RID: 4368 RVA: 0x000658B2 File Offset: 0x00063AB2
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseMinionConfig.BaseMinion(MinionConfig.MODEL, MinionConfig.GetAttributes(), MinionConfig.GetAmounts(), MinionConfig.GetTraits());
		gameObject.AddOrGet<CodexEntryRedirector>().CodexID = "DUPLICANTS";
		return gameObject;
	}

	// Token: 0x06001111 RID: 4369 RVA: 0x000658E0 File Offset: 0x00063AE0
	public void OnPrefabInit(GameObject go)
	{
		BaseMinionConfig.BasePrefabInit(go, MinionConfig.MODEL);
		DUPLICANTSTATS statsFor = DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL);
		Db.Get().Amounts.Bladder.Lookup(go).value = UnityEngine.Random.Range(0f, 10f);
		AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(go);
		amountInstance.value = (statsFor.BaseStats.HUNGRY_THRESHOLD + statsFor.BaseStats.SATISFIED_THRESHOLD) * 0.5f * amountInstance.GetMax();
		AmountInstance amountInstance2 = Db.Get().Amounts.Stamina.Lookup(go);
		amountInstance2.value = amountInstance2.GetMax();
	}

	// Token: 0x06001112 RID: 4370 RVA: 0x0006598C File Offset: 0x00063B8C
	public void OnSpawn(GameObject go)
	{
		Sensors component = go.GetComponent<Sensors>();
		component.Add(new ToiletSensor(component));
		BaseMinionConfig.BaseOnSpawn(go, MinionConfig.MODEL, this.RATIONAL_AI_STATE_MACHINES);
		go.GetComponent<OxygenBreather>().AddGasProvider(new GasBreatherFromWorldProvider());
		go.Trigger(1589886948, go);
	}

	// Token: 0x06001113 RID: 4371 RVA: 0x000659CC File Offset: 0x00063BCC
	public MinionConfig()
	{
		Func<RationalAi.Instance, StateMachine.Instance>[] array = BaseMinionConfig.BaseRationalAiStateMachines();
		Func<RationalAi.Instance, StateMachine.Instance>[] array2 = new Func<RationalAi.Instance, StateMachine.Instance>[9];
		array2[0] = ((RationalAi.Instance smi) => new BreathMonitor.Instance(smi.master));
		array2[1] = ((RationalAi.Instance smi) => new SteppedInMonitor.Instance(smi.master));
		array2[2] = ((RationalAi.Instance smi) => new Dreamer.Instance(smi.master));
		array2[3] = ((RationalAi.Instance smi) => new StaminaMonitor.Instance(smi.master));
		array2[4] = ((RationalAi.Instance smi) => new RationMonitor.Instance(smi.master));
		array2[5] = ((RationalAi.Instance smi) => new CalorieMonitor.Instance(smi.master));
		array2[6] = ((RationalAi.Instance smi) => new BladderMonitor.Instance(smi.master));
		array2[7] = ((RationalAi.Instance smi) => new HygieneMonitor.Instance(smi.master));
		array2[8] = ((RationalAi.Instance smi) => new TiredMonitor.Instance(smi.master));
		this.RATIONAL_AI_STATE_MACHINES = array.Append(array2);
		base..ctor();
	}

	// Token: 0x04000AEB RID: 2795
	public static Tag MODEL = GameTags.Minions.Models.Standard;

	// Token: 0x04000AEC RID: 2796
	public static string NAME = DUPLICANTS.MODEL.STANDARD.NAME;

	// Token: 0x04000AED RID: 2797
	public static string ID = MinionConfig.MODEL.ToString();

	// Token: 0x04000AEE RID: 2798
	public Func<RationalAi.Instance, StateMachine.Instance>[] RATIONAL_AI_STATE_MACHINES;
}
