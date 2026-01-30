using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000779 RID: 1913
[SerializationConfig(MemberSerialization.OptIn)]
public class HighEnergyParticleRedirector : StateMachineComponent<HighEnergyParticleRedirector.StatesInstance>, IHighEnergyParticleDirection
{
	// Token: 0x17000293 RID: 659
	// (get) Token: 0x060030A7 RID: 12455 RVA: 0x00118FC4 File Offset: 0x001171C4
	// (set) Token: 0x060030A8 RID: 12456 RVA: 0x00118FCC File Offset: 0x001171CC
	public EightDirection Direction
	{
		get
		{
			return this._direction;
		}
		set
		{
			this._direction = value;
			if (this.directionController != null)
			{
				this.directionController.SetRotation((float)(45 * EightDirectionUtil.GetDirectionIndex(this._direction)));
				this.directionController.controller.enabled = false;
				this.directionController.controller.enabled = true;
			}
		}
	}

	// Token: 0x060030A9 RID: 12457 RVA: 0x00119024 File Offset: 0x00117224
	private void OnCopySettings(object data)
	{
		HighEnergyParticleRedirector component = ((GameObject)data).GetComponent<HighEnergyParticleRedirector>();
		if (component != null)
		{
			this.Direction = component.Direction;
		}
	}

	// Token: 0x060030AA RID: 12458 RVA: 0x00119052 File Offset: 0x00117252
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HighEnergyParticleRedirector>(-905833192, HighEnergyParticleRedirector.OnCopySettingsDelegate);
		base.Subscribe<HighEnergyParticleRedirector>(-801688580, HighEnergyParticleRedirector.OnLogicValueChangedDelegate);
	}

	// Token: 0x060030AB RID: 12459 RVA: 0x0011907C File Offset: 0x0011727C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		HighEnergyParticlePort component = base.GetComponent<HighEnergyParticlePort>();
		if (component)
		{
			HighEnergyParticlePort highEnergyParticlePort = component;
			highEnergyParticlePort.onParticleCaptureAllowed = (HighEnergyParticlePort.OnParticleCaptureAllowed)Delegate.Combine(highEnergyParticlePort.onParticleCaptureAllowed, new HighEnergyParticlePort.OnParticleCaptureAllowed(this.OnParticleCaptureAllowed));
		}
		if (HighEnergyParticleRedirector.infoStatusItem_Logic == null)
		{
			HighEnergyParticleRedirector.infoStatusItem_Logic = new StatusItem("HEPRedirectorLogic", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			HighEnergyParticleRedirector.infoStatusItem_Logic.resolveStringCallback = new Func<string, object, string>(HighEnergyParticleRedirector.ResolveInfoStatusItem);
			HighEnergyParticleRedirector.infoStatusItem_Logic.resolveTooltipCallback = new Func<string, object, string>(HighEnergyParticleRedirector.ResolveInfoStatusItemTooltip);
		}
		this.selectable.AddStatusItem(HighEnergyParticleRedirector.infoStatusItem_Logic, this);
		this.directionController = new EightDirectionController(base.GetComponent<KBatchedAnimController>(), "redirector_target", "redirector", EightDirectionController.Offset.Infront);
		this.Direction = this.Direction;
		base.smi.StartSM();
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
	}

	// Token: 0x060030AC RID: 12460 RVA: 0x0011916E File Offset: 0x0011736E
	private bool OnParticleCaptureAllowed(HighEnergyParticle particle)
	{
		return this.AllowIncomingParticles;
	}

	// Token: 0x060030AD RID: 12461 RVA: 0x00119178 File Offset: 0x00117378
	private void LaunchParticle()
	{
		if (base.smi.master.storage.Particles < 0.1f)
		{
			base.smi.master.storage.ConsumeAll();
			return;
		}
		int highEnergyParticleOutputCell = base.GetComponent<Building>().GetHighEnergyParticleOutputCell();
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
		gameObject.SetActive(true);
		if (gameObject != null)
		{
			HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
			component.payload = base.smi.master.storage.ConsumeAll();
			component.payload -= 0.1f;
			component.capturedBy = this.port;
			component.SetDirection(this.Direction);
			this.directionController.PlayAnim("redirector_send", KAnim.PlayMode.Once);
			this.directionController.controller.Queue("redirector", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x060030AE RID: 12462 RVA: 0x00119278 File Offset: 0x00117478
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		HighEnergyParticlePort component = base.GetComponent<HighEnergyParticlePort>();
		if (component != null)
		{
			HighEnergyParticlePort highEnergyParticlePort = component;
			highEnergyParticlePort.onParticleCaptureAllowed = (HighEnergyParticlePort.OnParticleCaptureAllowed)Delegate.Remove(highEnergyParticlePort.onParticleCaptureAllowed, new HighEnergyParticlePort.OnParticleCaptureAllowed(this.OnParticleCaptureAllowed));
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x060030AF RID: 12463 RVA: 0x001192BD File Offset: 0x001174BD
	public bool AllowIncomingParticles
	{
		get
		{
			return !this.hasLogicWire || (this.hasLogicWire && this.isLogicActive);
		}
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x060030B0 RID: 12464 RVA: 0x001192D9 File Offset: 0x001174D9
	public bool HasLogicWire
	{
		get
		{
			return this.hasLogicWire;
		}
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x060030B1 RID: 12465 RVA: 0x001192E1 File Offset: 0x001174E1
	public bool IsLogicActive
	{
		get
		{
			return this.isLogicActive;
		}
	}

	// Token: 0x060030B2 RID: 12466 RVA: 0x001192EC File Offset: 0x001174EC
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(HighEnergyParticleRedirector.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x060030B3 RID: 12467 RVA: 0x0011931C File Offset: 0x0011751C
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == HighEnergyParticleRedirector.PORT_ID)
		{
			this.isLogicActive = (logicValueChanged.newValue > 0);
			this.hasLogicWire = (this.GetNetwork() != null);
		}
	}

	// Token: 0x060030B4 RID: 12468 RVA: 0x00119360 File Offset: 0x00117560
	private static string ResolveInfoStatusItem(string format_str, object data)
	{
		HighEnergyParticleRedirector highEnergyParticleRedirector = (HighEnergyParticleRedirector)data;
		if (!highEnergyParticleRedirector.HasLogicWire)
		{
			return BUILDING.STATUSITEMS.HIGHENERGYPARTICLEREDIRECTOR.NORMAL;
		}
		if (highEnergyParticleRedirector.IsLogicActive)
		{
			return BUILDING.STATUSITEMS.HIGHENERGYPARTICLEREDIRECTOR.LOGIC_CONTROLLED_ACTIVE;
		}
		return BUILDING.STATUSITEMS.HIGHENERGYPARTICLEREDIRECTOR.LOGIC_CONTROLLED_STANDBY;
	}

	// Token: 0x060030B5 RID: 12469 RVA: 0x001193A4 File Offset: 0x001175A4
	private static string ResolveInfoStatusItemTooltip(string format_str, object data)
	{
		HighEnergyParticleRedirector highEnergyParticleRedirector = (HighEnergyParticleRedirector)data;
		if (!highEnergyParticleRedirector.HasLogicWire)
		{
			return BUILDING.STATUSITEMS.HIGHENERGYPARTICLEREDIRECTOR.TOOLTIPS.NORMAL;
		}
		if (highEnergyParticleRedirector.IsLogicActive)
		{
			return BUILDING.STATUSITEMS.HIGHENERGYPARTICLEREDIRECTOR.TOOLTIPS.LOGIC_CONTROLLED_ACTIVE;
		}
		return BUILDING.STATUSITEMS.HIGHENERGYPARTICLEREDIRECTOR.TOOLTIPS.LOGIC_CONTROLLED_STANDBY;
	}

	// Token: 0x04001D0E RID: 7438
	public static readonly HashedString PORT_ID = "HEPRedirector";

	// Token: 0x04001D0F RID: 7439
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001D10 RID: 7440
	[MyCmpReq]
	private HighEnergyParticleStorage storage;

	// Token: 0x04001D11 RID: 7441
	[MyCmpGet]
	private HighEnergyParticlePort port;

	// Token: 0x04001D12 RID: 7442
	public float directorDelay;

	// Token: 0x04001D13 RID: 7443
	public bool directionControllable = true;

	// Token: 0x04001D14 RID: 7444
	[Serialize]
	private EightDirection _direction;

	// Token: 0x04001D15 RID: 7445
	private EightDirectionController directionController;

	// Token: 0x04001D16 RID: 7446
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001D17 RID: 7447
	private static readonly EventSystem.IntraObjectHandler<HighEnergyParticleRedirector> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<HighEnergyParticleRedirector>(delegate(HighEnergyParticleRedirector component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001D18 RID: 7448
	private static readonly EventSystem.IntraObjectHandler<HighEnergyParticleRedirector> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<HighEnergyParticleRedirector>(delegate(HighEnergyParticleRedirector component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001D19 RID: 7449
	private bool hasLogicWire;

	// Token: 0x04001D1A RID: 7450
	private bool isLogicActive;

	// Token: 0x04001D1B RID: 7451
	private static StatusItem infoStatusItem_Logic;

	// Token: 0x02001689 RID: 5769
	public class StatesInstance : GameStateMachine<HighEnergyParticleRedirector.States, HighEnergyParticleRedirector.StatesInstance, HighEnergyParticleRedirector, object>.GameInstance
	{
		// Token: 0x060097A1 RID: 38817 RVA: 0x003856F8 File Offset: 0x003838F8
		public StatesInstance(HighEnergyParticleRedirector smi) : base(smi)
		{
		}
	}

	// Token: 0x0200168A RID: 5770
	public class States : GameStateMachine<HighEnergyParticleRedirector.States, HighEnergyParticleRedirector.StatesInstance, HighEnergyParticleRedirector>
	{
		// Token: 0x060097A2 RID: 38818 RVA: 0x00385704 File Offset: 0x00383904
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false);
			this.ready.PlayAnim("on").TagTransition(GameTags.Operational, this.inoperational, true).EventTransition(GameHashes.OnParticleStorageChanged, this.redirect, null);
			this.redirect.PlayAnim("working_pre").QueueAnim("working_loop", false, null).QueueAnim("working_pst", false, null).ScheduleGoTo((HighEnergyParticleRedirector.StatesInstance smi) => smi.master.directorDelay, this.ready).Exit(delegate(HighEnergyParticleRedirector.StatesInstance smi)
			{
				smi.master.LaunchParticle();
			});
		}

		// Token: 0x04007528 RID: 29992
		public GameStateMachine<HighEnergyParticleRedirector.States, HighEnergyParticleRedirector.StatesInstance, HighEnergyParticleRedirector, object>.State inoperational;

		// Token: 0x04007529 RID: 29993
		public GameStateMachine<HighEnergyParticleRedirector.States, HighEnergyParticleRedirector.StatesInstance, HighEnergyParticleRedirector, object>.State ready;

		// Token: 0x0400752A RID: 29994
		public GameStateMachine<HighEnergyParticleRedirector.States, HighEnergyParticleRedirector.StatesInstance, HighEnergyParticleRedirector, object>.State redirect;

		// Token: 0x0400752B RID: 29995
		public GameStateMachine<HighEnergyParticleRedirector.States, HighEnergyParticleRedirector.StatesInstance, HighEnergyParticleRedirector, object>.State launch;
	}
}
