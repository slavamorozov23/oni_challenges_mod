using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020008C4 RID: 2244
[AddComponentMenu("KMonoBehaviour/scripts/WiltCondition")]
public class WiltCondition : KMonoBehaviour
{
	// Token: 0x06003DE2 RID: 15842 RVA: 0x00159075 File Offset: 0x00157275
	public bool IsWilting()
	{
		return this.wilting;
	}

	// Token: 0x06003DE3 RID: 15843 RVA: 0x00159080 File Offset: 0x00157280
	public List<WiltCondition.Condition> CurrentWiltSources()
	{
		List<WiltCondition.Condition> list = new List<WiltCondition.Condition>();
		foreach (KeyValuePair<int, bool> keyValuePair in this.WiltConditions)
		{
			if (!keyValuePair.Value)
			{
				list.Add((WiltCondition.Condition)keyValuePair.Key);
			}
		}
		return list;
	}

	// Token: 0x06003DE4 RID: 15844 RVA: 0x001590EC File Offset: 0x001572EC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.WiltConditions.Add(0, true);
		this.WiltConditions.Add(1, true);
		this.WiltConditions.Add(2, true);
		this.WiltConditions.Add(3, true);
		this.WiltConditions.Add(4, true);
		this.WiltConditions.Add(5, true);
		this.WiltConditions.Add(6, true);
		this.WiltConditions.Add(7, true);
		this.WiltConditions.Add(9, true);
		this.WiltConditions.Add(10, true);
		this.WiltConditions.Add(11, true);
		this.WiltConditions.Add(12, true);
		this.WiltConditions.Add(13, true);
		base.Subscribe<WiltCondition>(-107174716, WiltCondition.SetTemperatureFalseDelegate);
		base.Subscribe<WiltCondition>(-1758196852, WiltCondition.SetTemperatureFalseDelegate);
		base.Subscribe<WiltCondition>(-1234705021, WiltCondition.SetTemperatureFalseDelegate);
		base.Subscribe<WiltCondition>(-55477301, WiltCondition.SetTemperatureFalseDelegate);
		base.Subscribe<WiltCondition>(115888613, WiltCondition.SetTemperatureTrueDelegate);
		base.Subscribe<WiltCondition>(-593125877, WiltCondition.SetPressureFalseDelegate);
		base.Subscribe<WiltCondition>(-1175525437, WiltCondition.SetPressureFalseDelegate);
		base.Subscribe<WiltCondition>(-907106982, WiltCondition.SetPressureTrueDelegate);
		base.Subscribe<WiltCondition>(103243573, WiltCondition.SetPressureFalseDelegate);
		base.Subscribe<WiltCondition>(646131325, WiltCondition.SetPressureFalseDelegate);
		base.Subscribe<WiltCondition>(221594799, WiltCondition.SetAtmosphereElementFalseDelegate);
		base.Subscribe<WiltCondition>(777259436, WiltCondition.SetAtmosphereElementTrueDelegate);
		base.Subscribe<WiltCondition>(1949704522, WiltCondition.SetDrowningFalseDelegate);
		base.Subscribe<WiltCondition>(99949694, WiltCondition.SetDrowningTrueDelegate);
		base.Subscribe<WiltCondition>(-2057657673, WiltCondition.SetDryingOutFalseDelegate);
		base.Subscribe<WiltCondition>(1555379996, WiltCondition.SetDryingOutTrueDelegate);
		base.Subscribe<WiltCondition>(-370379773, WiltCondition.SetIrrigationFalseDelegate);
		base.Subscribe<WiltCondition>(207387507, WiltCondition.SetIrrigationTrueDelegate);
		base.Subscribe<WiltCondition>(-1073674739, WiltCondition.SetFertilizedFalseDelegate);
		base.Subscribe<WiltCondition>(-1396791468, WiltCondition.SetFertilizedTrueDelegate);
		base.Subscribe<WiltCondition>(1113102781, WiltCondition.SetIlluminationComfortTrueDelegate);
		base.Subscribe<WiltCondition>(1387626797, WiltCondition.SetIlluminationComfortFalseDelegate);
		base.Subscribe<WiltCondition>(1628751838, WiltCondition.SetReceptacleTrueDelegate);
		base.Subscribe<WiltCondition>(960378201, WiltCondition.SetReceptacleFalseDelegate);
		base.Subscribe<WiltCondition>(-1089732772, WiltCondition.SetEntombedDelegate);
		base.Subscribe<WiltCondition>(912965142, WiltCondition.SetRootHealthDelegate);
		base.Subscribe<WiltCondition>(874353739, WiltCondition.SetRadiationComfortTrueDelegate);
		base.Subscribe<WiltCondition>(1788072223, WiltCondition.SetRadiationComfortFalseDelegate);
		base.Subscribe<WiltCondition>(-200207042, WiltCondition.SetPollinatedDelegate);
	}

	// Token: 0x06003DE5 RID: 15845 RVA: 0x0015939C File Offset: 0x0015759C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.CheckShouldWilt();
		if (this.wilting)
		{
			this.DoWilt();
			if (!this.goingToWilt)
			{
				this.goingToWilt = true;
				this.Recover();
				return;
			}
		}
		else
		{
			this.DoRecover();
			if (this.goingToWilt)
			{
				this.goingToWilt = false;
				this.Wilt();
			}
		}
	}

	// Token: 0x06003DE6 RID: 15846 RVA: 0x001593F4 File Offset: 0x001575F4
	protected override void OnCleanUp()
	{
		this.wiltSchedulerHandler.ClearScheduler();
		this.recoverSchedulerHandler.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x06003DE7 RID: 15847 RVA: 0x00159414 File Offset: 0x00157614
	public bool IsConditionSatisifed(WiltCondition.Condition condition)
	{
		bool flag;
		return this.WiltConditions.TryGetValue((int)condition, out flag) && flag;
	}

	// Token: 0x06003DE8 RID: 15848 RVA: 0x00159431 File Offset: 0x00157631
	private void SetCondition(WiltCondition.Condition condition, bool satisfiedState)
	{
		if (!this.WiltConditions.ContainsKey((int)condition))
		{
			return;
		}
		this.WiltConditions[(int)condition] = satisfiedState;
		this.CheckShouldWilt();
	}

	// Token: 0x06003DE9 RID: 15849 RVA: 0x00159458 File Offset: 0x00157658
	private void CheckShouldWilt()
	{
		bool flag = false;
		foreach (KeyValuePair<int, bool> keyValuePair in this.WiltConditions)
		{
			if (!keyValuePair.Value)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			if (!this.goingToWilt)
			{
				this.Wilt();
				return;
			}
		}
		else if (this.goingToWilt)
		{
			this.Recover();
		}
	}

	// Token: 0x06003DEA RID: 15850 RVA: 0x001594D4 File Offset: 0x001576D4
	private void Wilt()
	{
		if (!this.goingToWilt)
		{
			this.goingToWilt = true;
			this.recoverSchedulerHandler.ClearScheduler();
			if (!this.wiltSchedulerHandler.IsValid)
			{
				this.wiltSchedulerHandler = GameScheduler.Instance.Schedule("Wilt", this.WiltDelay, new Action<object>(WiltCondition.DoWiltCallback), this, null);
			}
		}
	}

	// Token: 0x06003DEB RID: 15851 RVA: 0x00159534 File Offset: 0x00157734
	private void Recover()
	{
		if (this.goingToWilt)
		{
			this.goingToWilt = false;
			this.wiltSchedulerHandler.ClearScheduler();
			if (!this.recoverSchedulerHandler.IsValid)
			{
				this.recoverSchedulerHandler = GameScheduler.Instance.Schedule("Recover", this.RecoveryDelay, new Action<object>(WiltCondition.DoRecoverCallback), this, null);
			}
		}
	}

	// Token: 0x06003DEC RID: 15852 RVA: 0x00159591 File Offset: 0x00157791
	private static void DoWiltCallback(object data)
	{
		((WiltCondition)data).DoWilt();
	}

	// Token: 0x06003DED RID: 15853 RVA: 0x001595A0 File Offset: 0x001577A0
	private void DoWilt()
	{
		this.wiltSchedulerHandler.ClearScheduler();
		KSelectable component = base.GetComponent<KSelectable>();
		component.GetComponent<KPrefabID>().AddTag(GameTags.Wilting, false);
		if (!this.wilting)
		{
			this.wilting = true;
			base.Trigger(-724860998, null);
		}
		if (this.rm != null)
		{
			if (this.rm.Replanted)
			{
				component.AddStatusItem(Db.Get().CreatureStatusItems.WiltingDomestic, base.GetComponent<ReceptacleMonitor>());
				return;
			}
			component.AddStatusItem(Db.Get().CreatureStatusItems.Wilting, base.GetComponent<ReceptacleMonitor>());
			return;
		}
		else
		{
			ReceptacleMonitor.StatesInstance smi = component.GetSMI<ReceptacleMonitor.StatesInstance>();
			if (smi != null && !smi.IsInsideState(smi.sm.wild))
			{
				component.AddStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowingDomestic, this);
				return;
			}
			component.AddStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowing, this);
			return;
		}
	}

	// Token: 0x06003DEE RID: 15854 RVA: 0x0015968C File Offset: 0x0015788C
	public string WiltCausesString()
	{
		string text = "";
		List<IWiltCause> allSMI = this.GetAllSMI<IWiltCause>();
		allSMI.AddRange(base.GetComponents<IWiltCause>());
		foreach (IWiltCause wiltCause in allSMI)
		{
			foreach (WiltCondition.Condition key in wiltCause.Conditions)
			{
				if (this.WiltConditions.ContainsKey((int)key) && !this.WiltConditions[(int)key])
				{
					text += "\n";
					text += wiltCause.WiltStateString;
					break;
				}
			}
		}
		return text;
	}

	// Token: 0x06003DEF RID: 15855 RVA: 0x00159744 File Offset: 0x00157944
	private static void DoRecoverCallback(object data)
	{
		((WiltCondition)data).DoRecover();
	}

	// Token: 0x06003DF0 RID: 15856 RVA: 0x00159754 File Offset: 0x00157954
	private void DoRecover()
	{
		this.recoverSchedulerHandler.ClearScheduler();
		KSelectable component = base.GetComponent<KSelectable>();
		this.wilting = false;
		component.RemoveStatusItem(Db.Get().CreatureStatusItems.WiltingDomestic, false);
		component.RemoveStatusItem(Db.Get().CreatureStatusItems.Wilting, false);
		component.RemoveStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowing, false);
		component.RemoveStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowingDomestic, false);
		component.GetComponent<KPrefabID>().RemoveTag(GameTags.Wilting);
		base.Trigger(712767498, null);
	}

	// Token: 0x04002630 RID: 9776
	[MyCmpGet]
	private ReceptacleMonitor rm;

	// Token: 0x04002631 RID: 9777
	[Serialize]
	private bool goingToWilt;

	// Token: 0x04002632 RID: 9778
	[Serialize]
	private bool wilting;

	// Token: 0x04002633 RID: 9779
	private Dictionary<int, bool> WiltConditions = new Dictionary<int, bool>();

	// Token: 0x04002634 RID: 9780
	public float WiltDelay = 1f;

	// Token: 0x04002635 RID: 9781
	public float RecoveryDelay = 1f;

	// Token: 0x04002636 RID: 9782
	private SchedulerHandle wiltSchedulerHandler;

	// Token: 0x04002637 RID: 9783
	private SchedulerHandle recoverSchedulerHandler;

	// Token: 0x04002638 RID: 9784
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetTemperatureFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Temperature, false);
	});

	// Token: 0x04002639 RID: 9785
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetTemperatureTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Temperature, true);
	});

	// Token: 0x0400263A RID: 9786
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetPressureFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Pressure, false);
	});

	// Token: 0x0400263B RID: 9787
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetPressureTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Pressure, true);
	});

	// Token: 0x0400263C RID: 9788
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetAtmosphereElementFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.AtmosphereElement, false);
	});

	// Token: 0x0400263D RID: 9789
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetAtmosphereElementTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.AtmosphereElement, true);
	});

	// Token: 0x0400263E RID: 9790
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDrowningFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Drowning, false);
	});

	// Token: 0x0400263F RID: 9791
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDrowningTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Drowning, true);
	});

	// Token: 0x04002640 RID: 9792
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDryingOutFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.DryingOut, false);
	});

	// Token: 0x04002641 RID: 9793
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDryingOutTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.DryingOut, true);
	});

	// Token: 0x04002642 RID: 9794
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIrrigationFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Irrigation, false);
	});

	// Token: 0x04002643 RID: 9795
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIrrigationTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Irrigation, true);
	});

	// Token: 0x04002644 RID: 9796
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetFertilizedFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Fertilized, false);
	});

	// Token: 0x04002645 RID: 9797
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetFertilizedTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Fertilized, true);
	});

	// Token: 0x04002646 RID: 9798
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIlluminationComfortFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.IlluminationComfort, false);
	});

	// Token: 0x04002647 RID: 9799
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIlluminationComfortTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.IlluminationComfort, true);
	});

	// Token: 0x04002648 RID: 9800
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetReceptacleFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Receptacle, false);
	});

	// Token: 0x04002649 RID: 9801
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetReceptacleTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Receptacle, true);
	});

	// Token: 0x0400264A RID: 9802
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetEntombedDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Entombed, !Boxed<bool>.Unbox(data));
	});

	// Token: 0x0400264B RID: 9803
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetRootHealthDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.UnhealthyRoot, Boxed<bool>.Unbox(data));
	});

	// Token: 0x0400264C RID: 9804
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetRadiationComfortFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Radiation, false);
	});

	// Token: 0x0400264D RID: 9805
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetRadiationComfortTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Radiation, true);
	});

	// Token: 0x0400264E RID: 9806
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetPollinatedDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Pollination, Boxed<bool>.Unbox(data));
	});

	// Token: 0x020018DF RID: 6367
	public enum Condition
	{
		// Token: 0x04007C45 RID: 31813
		Temperature,
		// Token: 0x04007C46 RID: 31814
		Pressure,
		// Token: 0x04007C47 RID: 31815
		AtmosphereElement,
		// Token: 0x04007C48 RID: 31816
		Drowning,
		// Token: 0x04007C49 RID: 31817
		Fertilized,
		// Token: 0x04007C4A RID: 31818
		DryingOut,
		// Token: 0x04007C4B RID: 31819
		Irrigation,
		// Token: 0x04007C4C RID: 31820
		IlluminationComfort,
		// Token: 0x04007C4D RID: 31821
		Darkness,
		// Token: 0x04007C4E RID: 31822
		Receptacle,
		// Token: 0x04007C4F RID: 31823
		Entombed,
		// Token: 0x04007C50 RID: 31824
		UnhealthyRoot,
		// Token: 0x04007C51 RID: 31825
		Radiation,
		// Token: 0x04007C52 RID: 31826
		Pollination,
		// Token: 0x04007C53 RID: 31827
		Count
	}
}
