using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200071A RID: 1818
public class Checkpoint : StateMachineComponent<Checkpoint.SMInstance>
{
	// Token: 0x1700024F RID: 591
	// (get) Token: 0x06002D61 RID: 11617 RVA: 0x00107176 File Offset: 0x00105376
	private bool RedLightDesiredState
	{
		get
		{
			return this.hasLogicWire && !this.hasInputHigh && this.operational.IsOperational;
		}
	}

	// Token: 0x06002D62 RID: 11618 RVA: 0x00107198 File Offset: 0x00105398
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Checkpoint>(-801688580, Checkpoint.OnLogicValueChangedDelegate);
		base.Subscribe<Checkpoint>(-592767678, Checkpoint.OnOperationalChangedDelegate);
		base.smi.StartSM();
		if (Checkpoint.infoStatusItem_Logic == null)
		{
			Checkpoint.infoStatusItem_Logic = new StatusItem("CheckpointLogic", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			Checkpoint.infoStatusItem_Logic.resolveStringCallback = new Func<string, object, string>(Checkpoint.ResolveInfoStatusItem_Logic);
		}
		this.Refresh(this.redLight);
	}

	// Token: 0x06002D63 RID: 11619 RVA: 0x00107229 File Offset: 0x00105429
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.ClearReactable();
	}

	// Token: 0x06002D64 RID: 11620 RVA: 0x00107237 File Offset: 0x00105437
	public void RefreshLight()
	{
		if (this.redLight != this.RedLightDesiredState)
		{
			this.Refresh(this.RedLightDesiredState);
			this.statusDirty = true;
		}
		if (this.statusDirty)
		{
			this.RefreshStatusItem();
		}
	}

	// Token: 0x06002D65 RID: 11621 RVA: 0x00107268 File Offset: 0x00105468
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(Checkpoint.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x06002D66 RID: 11622 RVA: 0x00107296 File Offset: 0x00105496
	private static string ResolveInfoStatusItem_Logic(string format_str, object data)
	{
		return ((Checkpoint)data).RedLight ? BUILDING.STATUSITEMS.CHECKPOINT.LOGIC_CONTROLLED_CLOSED : BUILDING.STATUSITEMS.CHECKPOINT.LOGIC_CONTROLLED_OPEN;
	}

	// Token: 0x06002D67 RID: 11623 RVA: 0x001072B6 File Offset: 0x001054B6
	private void CreateNewReactable()
	{
		if (this.reactable == null)
		{
			this.reactable = new Checkpoint.CheckpointReactable(this);
		}
	}

	// Token: 0x06002D68 RID: 11624 RVA: 0x001072CC File Offset: 0x001054CC
	private void OrphanReactable()
	{
		this.reactable = null;
	}

	// Token: 0x06002D69 RID: 11625 RVA: 0x001072D5 File Offset: 0x001054D5
	private void ClearReactable()
	{
		if (this.reactable != null)
		{
			this.reactable.Cleanup();
			this.reactable = null;
		}
	}

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x06002D6A RID: 11626 RVA: 0x001072F1 File Offset: 0x001054F1
	public bool RedLight
	{
		get
		{
			return this.redLight;
		}
	}

	// Token: 0x06002D6B RID: 11627 RVA: 0x001072FC File Offset: 0x001054FC
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == Checkpoint.PORT_ID)
		{
			this.hasInputHigh = LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue);
			this.hasLogicWire = (this.GetNetwork() != null);
			this.statusDirty = true;
		}
	}

	// Token: 0x06002D6C RID: 11628 RVA: 0x0010734A File Offset: 0x0010554A
	private void OnOperationalChanged(object _)
	{
		this.statusDirty = true;
	}

	// Token: 0x06002D6D RID: 11629 RVA: 0x00107354 File Offset: 0x00105554
	private void RefreshStatusItem()
	{
		bool on = this.operational.IsOperational && this.hasLogicWire;
		this.selectable.ToggleStatusItem(Checkpoint.infoStatusItem_Logic, on, this);
		this.statusDirty = false;
	}

	// Token: 0x06002D6E RID: 11630 RVA: 0x00107394 File Offset: 0x00105594
	private void Refresh(bool redLightState)
	{
		this.redLight = redLightState;
		this.operational.SetActive(this.operational.IsOperational && this.redLight, false);
		base.smi.sm.redLight.Set(this.redLight, base.smi, false);
		if (this.redLight)
		{
			this.CreateNewReactable();
			return;
		}
		this.ClearReactable();
	}

	// Token: 0x04001AF9 RID: 6905
	[MyCmpReq]
	public Operational operational;

	// Token: 0x04001AFA RID: 6906
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001AFB RID: 6907
	private static StatusItem infoStatusItem_Logic;

	// Token: 0x04001AFC RID: 6908
	private Checkpoint.CheckpointReactable reactable;

	// Token: 0x04001AFD RID: 6909
	public static readonly HashedString PORT_ID = "Checkpoint";

	// Token: 0x04001AFE RID: 6910
	private bool hasLogicWire;

	// Token: 0x04001AFF RID: 6911
	private bool hasInputHigh;

	// Token: 0x04001B00 RID: 6912
	private bool redLight;

	// Token: 0x04001B01 RID: 6913
	private bool statusDirty = true;

	// Token: 0x04001B02 RID: 6914
	private static readonly EventSystem.IntraObjectHandler<Checkpoint> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<Checkpoint>(delegate(Checkpoint component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001B03 RID: 6915
	private static readonly EventSystem.IntraObjectHandler<Checkpoint> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Checkpoint>(delegate(Checkpoint component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x020015E2 RID: 5602
	private class CheckpointReactable : Reactable
	{
		// Token: 0x060094ED RID: 38125 RVA: 0x0037A8E8 File Offset: 0x00378AE8
		public CheckpointReactable(Checkpoint checkpoint) : base(checkpoint.gameObject, "CheckpointReactable", Db.Get().ChoreTypes.Checkpoint, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.checkpoint = checkpoint;
			this.rotated = this.gameObject.GetComponent<Rotatable>().IsRotated;
			this.preventChoreInterruption = false;
		}

		// Token: 0x060094EE RID: 38126 RVA: 0x0037A958 File Offset: 0x00378B58
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.checkpoint == null)
			{
				base.Cleanup();
				return false;
			}
			if (!this.checkpoint.RedLight)
			{
				return false;
			}
			if (this.rotated)
			{
				return transition.x < 0;
			}
			return transition.x > 0;
		}

		// Token: 0x060094EF RID: 38127 RVA: 0x0037A9B8 File Offset: 0x00378BB8
		protected override void InternalBegin()
		{
			this.reactor_navigator = this.reactor.GetComponent<Navigator>();
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"), 1f);
			component.Play("idle_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
			this.checkpoint.OrphanReactable();
			this.checkpoint.CreateNewReactable();
		}

		// Token: 0x060094F0 RID: 38128 RVA: 0x0037AA48 File Offset: 0x00378C48
		public override void Update(float dt)
		{
			if (this.checkpoint == null || !this.checkpoint.RedLight || this.reactor_navigator == null)
			{
				base.Cleanup();
				return;
			}
			this.reactor_navigator.AdvancePath(false);
			if (!this.reactor_navigator.path.IsValid())
			{
				base.Cleanup();
				return;
			}
			NavGrid.Transition nextTransition = this.reactor_navigator.GetNextTransition();
			if (!(this.rotated ? (nextTransition.x < 0) : (nextTransition.x > 0)))
			{
				base.Cleanup();
			}
		}

		// Token: 0x060094F1 RID: 38129 RVA: 0x0037AADA File Offset: 0x00378CDA
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"));
			}
		}

		// Token: 0x060094F2 RID: 38130 RVA: 0x0037AB09 File Offset: 0x00378D09
		protected override void InternalCleanup()
		{
		}

		// Token: 0x040072F7 RID: 29431
		private Checkpoint checkpoint;

		// Token: 0x040072F8 RID: 29432
		private Navigator reactor_navigator;

		// Token: 0x040072F9 RID: 29433
		private bool rotated;
	}

	// Token: 0x020015E3 RID: 5603
	public class SMInstance : GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.GameInstance
	{
		// Token: 0x060094F3 RID: 38131 RVA: 0x0037AB0B File Offset: 0x00378D0B
		public SMInstance(Checkpoint master) : base(master)
		{
		}
	}

	// Token: 0x020015E4 RID: 5604
	public class States : GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint>
	{
		// Token: 0x060094F4 RID: 38132 RVA: 0x0037AB14 File Offset: 0x00378D14
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.go;
			this.root.Update("RefreshLight", delegate(Checkpoint.SMInstance smi, float dt)
			{
				smi.master.RefreshLight();
			}, UpdateRate.SIM_200ms, false);
			this.stop.ParamTransition<bool>(this.redLight, this.go, GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.IsFalse).PlayAnim("red_light");
			this.go.ParamTransition<bool>(this.redLight, this.stop, GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.IsTrue).PlayAnim("green_light");
		}

		// Token: 0x040072FA RID: 29434
		public StateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.BoolParameter redLight;

		// Token: 0x040072FB RID: 29435
		public GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.State stop;

		// Token: 0x040072FC RID: 29436
		public GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.State go;
	}
}
