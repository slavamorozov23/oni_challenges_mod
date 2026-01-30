using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020005D8 RID: 1496
public class EffectImmunityProviderStation<StateMachineInstanceType> : GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def> where StateMachineInstanceType : EffectImmunityProviderStation<StateMachineInstanceType>.BaseInstance
{
	// Token: 0x060022A0 RID: 8864 RVA: 0x000C9808 File Offset: 0x000C7A08
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.inactive;
		this.inactive.EventTransition(GameHashes.ActiveChanged, this.active, (StateMachineInstanceType smi) => smi.GetComponent<Operational>().IsActive);
		this.active.EventTransition(GameHashes.ActiveChanged, this.inactive, (StateMachineInstanceType smi) => !smi.GetComponent<Operational>().IsActive);
	}

	// Token: 0x0400143C RID: 5180
	public GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def>.State inactive;

	// Token: 0x0400143D RID: 5181
	public GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def>.State active;

	// Token: 0x020014B1 RID: 5297
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x060090A0 RID: 37024 RVA: 0x0036EE17 File Offset: 0x0036D017
		public virtual string[] DefaultAnims()
		{
			return new string[]
			{
				"",
				"",
				""
			};
		}

		// Token: 0x060090A1 RID: 37025 RVA: 0x0036EE37 File Offset: 0x0036D037
		public virtual string DefaultAnimFileName()
		{
			return "anim_warmup_kanim";
		}

		// Token: 0x060090A2 RID: 37026 RVA: 0x0036EE3E File Offset: 0x0036D03E
		public string[] GetAnimNames()
		{
			if (this.overrideAnims != null)
			{
				return this.overrideAnims;
			}
			return this.DefaultAnims();
		}

		// Token: 0x060090A3 RID: 37027 RVA: 0x0036EE55 File Offset: 0x0036D055
		public string GetAnimFileName(GameObject entity)
		{
			if (this.overrideFileName != null)
			{
				return this.overrideFileName(entity);
			}
			return this.DefaultAnimFileName();
		}

		// Token: 0x04006F36 RID: 28470
		public Action<GameObject, StateMachineInstanceType> onEffectApplied;

		// Token: 0x04006F37 RID: 28471
		public Func<GameObject, bool> specialRequirements;

		// Token: 0x04006F38 RID: 28472
		public Func<GameObject, string> overrideFileName;

		// Token: 0x04006F39 RID: 28473
		public string[] overrideAnims;

		// Token: 0x04006F3A RID: 28474
		public CellOffset[][] range;
	}

	// Token: 0x020014B2 RID: 5298
	public abstract class BaseInstance : GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def>.GameInstance
	{
		// Token: 0x060090A5 RID: 37029 RVA: 0x0036EE7A File Offset: 0x0036D07A
		public string GetAnimFileName(GameObject entity)
		{
			return base.def.GetAnimFileName(entity);
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x060090A6 RID: 37030 RVA: 0x0036EE88 File Offset: 0x0036D088
		public string PreAnimName
		{
			get
			{
				return base.def.GetAnimNames()[0];
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x060090A7 RID: 37031 RVA: 0x0036EE97 File Offset: 0x0036D097
		public string LoopAnimName
		{
			get
			{
				return base.def.GetAnimNames()[1];
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x060090A8 RID: 37032 RVA: 0x0036EEA6 File Offset: 0x0036D0A6
		public string PstAnimName
		{
			get
			{
				return base.def.GetAnimNames()[2];
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x060090A9 RID: 37033 RVA: 0x0036EEB5 File Offset: 0x0036D0B5
		public bool CanBeUsed
		{
			get
			{
				return this.IsActive && (base.def.specialRequirements == null || base.def.specialRequirements(base.gameObject));
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x060090AA RID: 37034 RVA: 0x0036EEE6 File Offset: 0x0036D0E6
		protected bool IsActive
		{
			get
			{
				return base.IsInsideState(base.sm.active);
			}
		}

		// Token: 0x060090AB RID: 37035 RVA: 0x0036EEF9 File Offset: 0x0036D0F9
		public BaseInstance(IStateMachineTarget master, EffectImmunityProviderStation<StateMachineInstanceType>.Def def) : base(master, def)
		{
		}

		// Token: 0x060090AC RID: 37036 RVA: 0x0036EF04 File Offset: 0x0036D104
		public int GetBestAvailableCell(Navigator dupeLooking, out int _cost)
		{
			_cost = int.MaxValue;
			if (!this.CanBeUsed)
			{
				return Grid.InvalidCell;
			}
			int num = Grid.PosToCell(this);
			int num2 = Grid.InvalidCell;
			if (base.def.range != null)
			{
				for (int i = 0; i < base.def.range.GetLength(0); i++)
				{
					int num3 = int.MaxValue;
					for (int j = 0; j < base.def.range[i].Length; j++)
					{
						int num4 = Grid.OffsetCell(num, base.def.range[i][j]);
						if (dupeLooking.CanReach(num4))
						{
							int navigationCost = dupeLooking.GetNavigationCost(num4);
							if (navigationCost < num3)
							{
								num3 = navigationCost;
								num2 = num4;
							}
						}
					}
					if (num2 != Grid.InvalidCell)
					{
						_cost = num3;
						break;
					}
				}
				return num2;
			}
			if (dupeLooking.CanReach(num))
			{
				_cost = dupeLooking.GetNavigationCost(num);
				return num;
			}
			return Grid.InvalidCell;
		}

		// Token: 0x060090AD RID: 37037 RVA: 0x0036EFE8 File Offset: 0x0036D1E8
		public void ApplyImmunityEffect(GameObject target, bool triggerEvents = true)
		{
			Effects component = target.GetComponent<Effects>();
			if (component == null)
			{
				return;
			}
			this.ApplyImmunityEffect(component);
			if (triggerEvents)
			{
				Action<GameObject, StateMachineInstanceType> onEffectApplied = base.def.onEffectApplied;
				if (onEffectApplied == null)
				{
					return;
				}
				onEffectApplied(component.gameObject, (StateMachineInstanceType)((object)this));
			}
		}

		// Token: 0x060090AE RID: 37038
		protected abstract void ApplyImmunityEffect(Effects target);

		// Token: 0x060090AF RID: 37039 RVA: 0x0036F031 File Offset: 0x0036D231
		public override void StartSM()
		{
			Components.EffectImmunityProviderStations.Add(this);
			base.StartSM();
		}

		// Token: 0x060090B0 RID: 37040 RVA: 0x0036F044 File Offset: 0x0036D244
		protected override void OnCleanUp()
		{
			Components.EffectImmunityProviderStations.Remove(this);
			base.OnCleanUp();
		}
	}
}
