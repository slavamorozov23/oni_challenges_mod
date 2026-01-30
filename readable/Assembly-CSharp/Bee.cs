using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200059F RID: 1439
public class Bee : KMonoBehaviour
{
	// Token: 0x06002060 RID: 8288 RVA: 0x000BA6A4 File Offset: 0x000B88A4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Bee>(-739654666, Bee.OnAttackDelegate);
		base.Subscribe<Bee>(-1283701846, Bee.OnSleepDelegate);
		base.Subscribe<Bee>(-2090444759, Bee.OnWakeUpDelegate);
		base.Subscribe<Bee>(1623392196, Bee.OnDeathDelegate);
		base.Subscribe<Bee>(49018834, Bee.OnSatisfiedDelegate);
		base.Subscribe<Bee>(-647798969, Bee.OnUnhappyDelegate);
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("tag", false);
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("snapto_tag", false);
		this.StopSleep();
	}

	// Token: 0x06002061 RID: 8289 RVA: 0x000BA750 File Offset: 0x000B8950
	private void OnDeath(object data)
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		Storage component2 = base.GetComponent<Storage>();
		byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.id);
		component2.AddOre(SimHashes.NuclearWaste, BeeTuning.WASTE_DROPPED_ON_DEATH, component.Temperature, index, BeeTuning.GERMS_DROPPED_ON_DEATH, false, true);
		component2.DropAll(base.transform.position, true, true, default(Vector3), true, null);
	}

	// Token: 0x06002062 RID: 8290 RVA: 0x000BA7CA File Offset: 0x000B89CA
	private void StartSleep()
	{
		this.RemoveRadiationMod(this.awakeRadiationModKey);
		base.GetComponent<ElementConsumer>().EnableConsumption(true);
	}

	// Token: 0x06002063 RID: 8291 RVA: 0x000BA7E4 File Offset: 0x000B89E4
	private void StopSleep()
	{
		this.AddRadiationModifier(this.awakeRadiationModKey, this.awakeRadiationMod);
		base.GetComponent<ElementConsumer>().EnableConsumption(false);
	}

	// Token: 0x06002064 RID: 8292 RVA: 0x000BA804 File Offset: 0x000B8A04
	private void AddRadiationModifier(HashedString name, float mod)
	{
		this.radiationModifiers.Add(name, mod);
		this.RefreshRadiationOutput();
	}

	// Token: 0x06002065 RID: 8293 RVA: 0x000BA819 File Offset: 0x000B8A19
	private void RemoveRadiationMod(HashedString name)
	{
		this.radiationModifiers.Remove(name);
		this.RefreshRadiationOutput();
	}

	// Token: 0x06002066 RID: 8294 RVA: 0x000BA830 File Offset: 0x000B8A30
	public void RefreshRadiationOutput()
	{
		float num = this.radiationOutputAmount;
		foreach (KeyValuePair<HashedString, float> keyValuePair in this.radiationModifiers)
		{
			num *= keyValuePair.Value;
		}
		RadiationEmitter component = base.GetComponent<RadiationEmitter>();
		component.SetEmitting(true);
		component.emitRads = num;
		component.Refresh();
	}

	// Token: 0x06002067 RID: 8295 RVA: 0x000BA8A8 File Offset: 0x000B8AA8
	private void OnAttack(object data)
	{
		if (((Boxed<Tag>)data).value == GameTags.Creatures.Attack)
		{
			base.GetComponent<Health>().Damage(base.GetComponent<Health>().hitPoints);
		}
	}

	// Token: 0x06002068 RID: 8296 RVA: 0x000BA8D8 File Offset: 0x000B8AD8
	public KPrefabID FindHiveInRoom()
	{
		List<BeeHive.StatesInstance> list = new List<BeeHive.StatesInstance>();
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		foreach (BeeHive.StatesInstance statesInstance in Components.BeeHives.Items)
		{
			if (Game.Instance.roomProber.GetRoomOfGameObject(statesInstance.gameObject) == roomOfGameObject)
			{
				list.Add(statesInstance);
			}
		}
		int num = int.MaxValue;
		KPrefabID result = null;
		foreach (BeeHive.StatesInstance statesInstance2 in list)
		{
			int navigationCost = base.gameObject.GetComponent<Navigator>().GetNavigationCost(Grid.PosToCell(statesInstance2.transform.GetLocalPosition()));
			if (navigationCost < num)
			{
				num = navigationCost;
				result = statesInstance2.GetComponent<KPrefabID>();
			}
		}
		return result;
	}

	// Token: 0x040012D7 RID: 4823
	public float radiationOutputAmount;

	// Token: 0x040012D8 RID: 4824
	private Dictionary<HashedString, float> radiationModifiers = new Dictionary<HashedString, float>();

	// Token: 0x040012D9 RID: 4825
	private float unhappyRadiationMod = 0.1f;

	// Token: 0x040012DA RID: 4826
	private float awakeRadiationMod;

	// Token: 0x040012DB RID: 4827
	private HashedString unhappyRadiationModKey = "UNHAPPY";

	// Token: 0x040012DC RID: 4828
	private HashedString awakeRadiationModKey = "AWAKE";

	// Token: 0x040012DD RID: 4829
	private static readonly EventSystem.IntraObjectHandler<Bee> OnAttackDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.OnAttack(data);
	});

	// Token: 0x040012DE RID: 4830
	private static readonly EventSystem.IntraObjectHandler<Bee> OnSleepDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.StartSleep();
	});

	// Token: 0x040012DF RID: 4831
	private static readonly EventSystem.IntraObjectHandler<Bee> OnWakeUpDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.StopSleep();
	});

	// Token: 0x040012E0 RID: 4832
	private static readonly EventSystem.IntraObjectHandler<Bee> OnDeathDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x040012E1 RID: 4833
	private static readonly EventSystem.IntraObjectHandler<Bee> OnUnhappyDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.AddRadiationModifier(component.unhappyRadiationModKey, component.unhappyRadiationMod);
	});

	// Token: 0x040012E2 RID: 4834
	private static readonly EventSystem.IntraObjectHandler<Bee> OnSatisfiedDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.RemoveRadiationMod(component.unhappyRadiationModKey);
	});
}
