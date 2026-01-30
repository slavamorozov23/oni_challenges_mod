using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000AA7 RID: 2727
public class PollinationMonitor : GameStateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>
{
	// Token: 0x06004F0C RID: 20236 RVA: 0x001CB063 File Offset: 0x001C9263
	public static bool IsPollinationEffect(Effect effect)
	{
		return Array.IndexOf<HashedString>(PollinationMonitor.PollinationEffects, effect.IdHash) != -1;
	}

	// Token: 0x06004F0D RID: 20237 RVA: 0x001CB07C File Offset: 0x001C927C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.initialize;
		this.initialize.Enter(delegate(PollinationMonitor.StatesInstance smi)
		{
			if (smi.effects == null)
			{
				smi.GoTo(this.not_pollinated);
				return;
			}
			bool flag = false;
			foreach (HashedString effect_id in PollinationMonitor.PollinationEffects)
			{
				if (smi.effects.HasEffect(effect_id))
				{
					flag = true;
					break;
				}
			}
			smi.GoTo(flag ? this.pollinated : this.not_pollinated);
		});
		this.not_pollinated.Enter(delegate(PollinationMonitor.StatesInstance smi)
		{
			smi.BoxingTrigger(-200207042, false);
		}).EventHandler(GameHashes.EffectAdded, delegate(PollinationMonitor.StatesInstance smi, object data)
		{
			if (PollinationMonitor.IsPollinationEffect(data as Effect))
			{
				smi.GoTo(this.pollinated);
			}
		});
		this.pollinated.Enter(delegate(PollinationMonitor.StatesInstance smi)
		{
			smi.BoxingTrigger(-200207042, true);
		}).EventHandler(GameHashes.EffectRemoved, delegate(PollinationMonitor.StatesInstance smi, object data)
		{
			if (!PollinationMonitor.IsPollinationEffect(data as Effect))
			{
				return;
			}
			if (smi.effects == null)
			{
				smi.GoTo(this.not_pollinated);
				return;
			}
			bool flag = false;
			foreach (HashedString effect_id in PollinationMonitor.PollinationEffects)
			{
				if (smi.effects.HasEffect(effect_id))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				smi.GoTo(this.not_pollinated);
			}
		});
	}

	// Token: 0x040034CD RID: 13517
	public static readonly string INITIALLY_POLLINATED_EFFECT = "InitiallyPollinated";

	// Token: 0x040034CE RID: 13518
	public static readonly HashedString[] PollinationEffects = new HashedString[]
	{
		PollinationMonitor.INITIALLY_POLLINATED_EFFECT,
		"DivergentCropTended",
		"DivergentCropTendedWorm",
		"ButterflyPollinated"
	};

	// Token: 0x040034CF RID: 13519
	public GameStateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>.State initialize;

	// Token: 0x040034D0 RID: 13520
	public GameStateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>.State not_pollinated;

	// Token: 0x040034D1 RID: 13521
	public GameStateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>.State pollinated;

	// Token: 0x040034D2 RID: 13522
	private readonly StateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>.BoolParameter spawn_pollinated = new StateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>.BoolParameter(false);

	// Token: 0x02001BD3 RID: 7123
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x0600AB4F RID: 43855 RVA: 0x003C82BF File Offset: 0x003C64BF
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_POLLINATION, UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_POLLINATION, Descriptor.DescriptorType.Requirement, false)
			};
		}
	}

	// Token: 0x02001BD4 RID: 7124
	public class StatesInstance : GameStateMachine<PollinationMonitor, PollinationMonitor.StatesInstance, IStateMachineTarget, PollinationMonitor.Def>.GameInstance, IWiltCause
	{
		// Token: 0x0600AB51 RID: 43857 RVA: 0x003C82EF File Offset: 0x003C64EF
		public StatesInstance(IStateMachineTarget master, PollinationMonitor.Def def) : base(master, def)
		{
			this.effects = base.GetComponent<Effects>();
			base.Subscribe(1119167081, delegate(object _)
			{
				base.sm.spawn_pollinated.Set(true, this, false);
			});
		}

		// Token: 0x0600AB52 RID: 43858 RVA: 0x003C8320 File Offset: 0x003C6520
		public override void StartSM()
		{
			base.StartSM();
			if (base.sm.spawn_pollinated.Get(this))
			{
				base.sm.spawn_pollinated.Set(false, this, false);
				if (this.effects != null)
				{
					this.effects.Add(PollinationMonitor.INITIALLY_POLLINATED_EFFECT, true).timeRemaining *= UnityEngine.Random.Range(0.75f, 1f);
				}
			}
		}

		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x0600AB53 RID: 43859 RVA: 0x003C8394 File Offset: 0x003C6594
		public WiltCondition.Condition[] Conditions
		{
			get
			{
				return new WiltCondition.Condition[]
				{
					WiltCondition.Condition.Pollination
				};
			}
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x0600AB54 RID: 43860 RVA: 0x003C83A1 File Offset: 0x003C65A1
		public string WiltStateString
		{
			get
			{
				if (!base.IsInsideState(base.sm.not_pollinated))
				{
					return "";
				}
				return Db.Get().CreatureStatusItems.NotPollinated.GetName(this);
			}
		}

		// Token: 0x040085CE RID: 34254
		public Effects effects;
	}
}
