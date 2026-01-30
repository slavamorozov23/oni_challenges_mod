using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000B69 RID: 2921
[SerializationConfig(MemberSerialization.OptIn)]
public class CargoLander : GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>
{
	// Token: 0x06005685 RID: 22149 RVA: 0x001F8064 File Offset: 0x001F6264
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.init;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.InitializeOperationalFlag(RocketModule.landedFlag, false).Enter(delegate(CargoLander.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).EventHandler(GameHashes.OnStorageChange, delegate(CargoLander.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		});
		this.init.ParamTransition<bool>(this.isLanded, this.grounded, GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.IsTrue).GoTo(this.stored);
		this.stored.TagTransition(GameTags.Stored, this.landing, true).EventHandler(GameHashes.JettisonedLander, delegate(CargoLander.StatesInstance smi)
		{
			smi.OnJettisoned();
		});
		this.landing.PlayAnim("landing", KAnim.PlayMode.Loop).Enter(delegate(CargoLander.StatesInstance smi)
		{
			smi.ShowLandingPreview(true);
		}).Exit(delegate(CargoLander.StatesInstance smi)
		{
			smi.ShowLandingPreview(false);
		}).Enter(delegate(CargoLander.StatesInstance smi)
		{
			smi.ResetAnimPosition();
		}).Update(delegate(CargoLander.StatesInstance smi, float dt)
		{
			smi.LandingUpdate(dt);
		}, UpdateRate.SIM_EVERY_TICK, false).Transition(this.land, (CargoLander.StatesInstance smi) => smi.flightAnimOffset <= 0f, UpdateRate.SIM_200ms);
		this.land.PlayAnim("grounded_pre").OnAnimQueueComplete(this.grounded);
		this.grounded.DefaultState(this.grounded.loaded).ToggleOperationalFlag(RocketModule.landedFlag).Enter(delegate(CargoLander.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).Enter(delegate(CargoLander.StatesInstance smi)
		{
			smi.sm.isLanded.Set(true, smi, false);
		});
		this.grounded.loaded.PlayAnim("grounded").ParamTransition<bool>(this.hasCargo, this.grounded.empty, GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.IsFalse).OnSignal(this.emptyCargo, this.grounded.emptying).Enter(delegate(CargoLander.StatesInstance smi)
		{
			smi.DoLand();
		});
		this.grounded.emptying.PlayAnim("deploying").TriggerOnEnter(GameHashes.JettisonCargo, null).OnAnimQueueComplete(this.grounded.empty);
		this.grounded.empty.PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.grounded.loaded, GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.IsTrue);
	}

	// Token: 0x04003A5B RID: 14939
	public StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.BoolParameter hasCargo;

	// Token: 0x04003A5C RID: 14940
	public StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.Signal emptyCargo;

	// Token: 0x04003A5D RID: 14941
	public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State init;

	// Token: 0x04003A5E RID: 14942
	public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State stored;

	// Token: 0x04003A5F RID: 14943
	public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State landing;

	// Token: 0x04003A60 RID: 14944
	public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State land;

	// Token: 0x04003A61 RID: 14945
	public CargoLander.CrashedStates grounded;

	// Token: 0x04003A62 RID: 14946
	public StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.BoolParameter isLanded = new StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.BoolParameter(false);

	// Token: 0x02001CDC RID: 7388
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04008970 RID: 35184
		public Tag previewTag;

		// Token: 0x04008971 RID: 35185
		public bool deployOnLanding = true;
	}

	// Token: 0x02001CDD RID: 7389
	public class CrashedStates : GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State
	{
		// Token: 0x04008972 RID: 35186
		public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State loaded;

		// Token: 0x04008973 RID: 35187
		public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State emptying;

		// Token: 0x04008974 RID: 35188
		public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State empty;
	}

	// Token: 0x02001CDE RID: 7390
	public class StatesInstance : GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.GameInstance
	{
		// Token: 0x0600AEF5 RID: 44789 RVA: 0x003D4EFC File Offset: 0x003D30FC
		public StatesInstance(IStateMachineTarget master, CargoLander.Def def) : base(master, def)
		{
		}

		// Token: 0x0600AEF6 RID: 44790 RVA: 0x003D4F48 File Offset: 0x003D3148
		public void ResetAnimPosition()
		{
			base.GetComponent<KBatchedAnimController>().Offset = Vector3.up * this.flightAnimOffset;
		}

		// Token: 0x0600AEF7 RID: 44791 RVA: 0x003D4F65 File Offset: 0x003D3165
		public void OnJettisoned()
		{
			this.flightAnimOffset = 50f;
		}

		// Token: 0x0600AEF8 RID: 44792 RVA: 0x003D4F74 File Offset: 0x003D3174
		public void ShowLandingPreview(bool show)
		{
			if (show)
			{
				this.landingPreview = Util.KInstantiate(Assets.GetPrefab(base.def.previewTag), base.transform.GetPosition(), Quaternion.identity, base.gameObject, null, true, 0);
				this.landingPreview.SetActive(true);
				return;
			}
			this.landingPreview.DeleteObject();
			this.landingPreview = null;
		}

		// Token: 0x0600AEF9 RID: 44793 RVA: 0x003D4FD8 File Offset: 0x003D31D8
		public void LandingUpdate(float dt)
		{
			this.flightAnimOffset = Mathf.Max(this.flightAnimOffset - dt * this.topSpeed, 0f);
			this.ResetAnimPosition();
			int num = Grid.PosToCell(base.gameObject.transform.GetPosition() + new Vector3(0f, this.flightAnimOffset, 0f));
			if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == base.gameObject.GetMyWorldId())
			{
				SimMessages.EmitMass(num, ElementLoader.GetElementIndex(this.exhaustElement), dt * this.exhaustEmitRate, this.exhaustTemperature, 0, 0, -1);
			}
		}

		// Token: 0x0600AEFA RID: 44794 RVA: 0x003D5078 File Offset: 0x003D3278
		public void DoLand()
		{
			base.smi.master.GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
			OccupyArea component = base.smi.GetComponent<OccupyArea>();
			if (component != null)
			{
				component.ApplyToCells = true;
			}
			if (base.def.deployOnLanding && this.CheckIfLoaded())
			{
				base.sm.emptyCargo.Trigger(this);
			}
			base.smi.master.gameObject.Trigger(1591811118, this);
		}

		// Token: 0x0600AEFB RID: 44795 RVA: 0x003D50FC File Offset: 0x003D32FC
		public bool CheckIfLoaded()
		{
			bool flag = false;
			MinionStorage component = base.GetComponent<MinionStorage>();
			if (component != null)
			{
				flag |= (component.GetStoredMinionInfo().Count > 0);
			}
			Storage component2 = base.GetComponent<Storage>();
			if (component2 != null && !component2.IsEmpty())
			{
				flag = true;
			}
			if (flag != base.sm.hasCargo.Get(this))
			{
				base.sm.hasCargo.Set(flag, this, false);
			}
			return flag;
		}

		// Token: 0x04008975 RID: 35189
		[Serialize]
		public float flightAnimOffset = 50f;

		// Token: 0x04008976 RID: 35190
		public float exhaustEmitRate = 2f;

		// Token: 0x04008977 RID: 35191
		public float exhaustTemperature = 1000f;

		// Token: 0x04008978 RID: 35192
		public SimHashes exhaustElement = SimHashes.CarbonDioxide;

		// Token: 0x04008979 RID: 35193
		public float topSpeed = 5f;

		// Token: 0x0400897A RID: 35194
		private GameObject landingPreview;
	}
}
