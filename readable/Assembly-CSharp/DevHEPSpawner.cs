using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000741 RID: 1857
[SerializationConfig(MemberSerialization.OptIn)]
public class DevHEPSpawner : StateMachineComponent<DevHEPSpawner.StatesInstance>, IHighEnergyParticleDirection, ISingleSliderControl, ISliderControl
{
	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06002EC7 RID: 11975 RVA: 0x0010E634 File Offset: 0x0010C834
	// (set) Token: 0x06002EC8 RID: 11976 RVA: 0x0010E63C File Offset: 0x0010C83C
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

	// Token: 0x06002EC9 RID: 11977 RVA: 0x0010E694 File Offset: 0x0010C894
	private void OnCopySettings(object data)
	{
		DevHEPSpawner component = ((GameObject)data).GetComponent<DevHEPSpawner>();
		if (component != null)
		{
			this.Direction = component.Direction;
			this.boltAmount = component.boltAmount;
		}
	}

	// Token: 0x06002ECA RID: 11978 RVA: 0x0010E6CE File Offset: 0x0010C8CE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<DevHEPSpawner>(-905833192, DevHEPSpawner.OnCopySettingsDelegate);
	}

	// Token: 0x06002ECB RID: 11979 RVA: 0x0010E6E8 File Offset: 0x0010C8E8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.directionController = new EightDirectionController(base.GetComponent<KBatchedAnimController>(), "redirector_target", "redirect", EightDirectionController.Offset.Infront);
		this.Direction = this.Direction;
		this.particleController = new MeterController(base.GetComponent<KBatchedAnimController>(), "orb_target", "orb_off", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.particleController.gameObject.AddOrGet<LoopingSounds>();
		this.progressMeterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x06002ECC RID: 11980 RVA: 0x0010E788 File Offset: 0x0010C988
	public void LauncherUpdate(float dt)
	{
		if (this.boltAmount <= 0f)
		{
			return;
		}
		this.launcherTimer += dt;
		this.progressMeterController.SetPositionPercent(this.launcherTimer / 5f);
		if (this.launcherTimer > 5f)
		{
			this.launcherTimer -= 5f;
			int highEnergyParticleOutputCell = base.GetComponent<Building>().GetHighEnergyParticleOutputCell();
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
			gameObject.SetActive(true);
			if (gameObject != null)
			{
				HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
				component.payload = this.boltAmount;
				component.SetDirection(this.Direction);
				this.directionController.PlayAnim("redirect_send", KAnim.PlayMode.Once);
				this.directionController.controller.Queue("redirect", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Play("orb_send", KAnim.PlayMode.Once, 1f, 0f);
				this.particleController.meterController.Queue("orb_off", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x06002ECD RID: 11981 RVA: 0x0010E8C7 File Offset: 0x0010CAC7
	public string SliderTitleKey
	{
		get
		{
			return "";
		}
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x06002ECE RID: 11982 RVA: 0x0010E8CE File Offset: 0x0010CACE
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
		}
	}

	// Token: 0x06002ECF RID: 11983 RVA: 0x0010E8DA File Offset: 0x0010CADA
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x06002ED0 RID: 11984 RVA: 0x0010E8DD File Offset: 0x0010CADD
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x06002ED1 RID: 11985 RVA: 0x0010E8E4 File Offset: 0x0010CAE4
	public float GetSliderMax(int index)
	{
		return 500f;
	}

	// Token: 0x06002ED2 RID: 11986 RVA: 0x0010E8EB File Offset: 0x0010CAEB
	public float GetSliderValue(int index)
	{
		return this.boltAmount;
	}

	// Token: 0x06002ED3 RID: 11987 RVA: 0x0010E8F3 File Offset: 0x0010CAF3
	public void SetSliderValue(float value, int index)
	{
		this.boltAmount = value;
	}

	// Token: 0x06002ED4 RID: 11988 RVA: 0x0010E8FC File Offset: 0x0010CAFC
	public string GetSliderTooltipKey(int index)
	{
		return "";
	}

	// Token: 0x06002ED5 RID: 11989 RVA: 0x0010E903 File Offset: 0x0010CB03
	string ISliderControl.GetSliderTooltip(int index)
	{
		return "";
	}

	// Token: 0x04001BB3 RID: 7091
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001BB4 RID: 7092
	[Serialize]
	private EightDirection _direction;

	// Token: 0x04001BB5 RID: 7093
	public float boltAmount;

	// Token: 0x04001BB6 RID: 7094
	private EightDirectionController directionController;

	// Token: 0x04001BB7 RID: 7095
	private float launcherTimer;

	// Token: 0x04001BB8 RID: 7096
	private MeterController particleController;

	// Token: 0x04001BB9 RID: 7097
	private MeterController progressMeterController;

	// Token: 0x04001BBA RID: 7098
	[Serialize]
	public Ref<HighEnergyParticlePort> capturedByRef = new Ref<HighEnergyParticlePort>();

	// Token: 0x04001BBB RID: 7099
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001BBC RID: 7100
	private static readonly EventSystem.IntraObjectHandler<DevHEPSpawner> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<DevHEPSpawner>(delegate(DevHEPSpawner component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0200161D RID: 5661
	public class StatesInstance : GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.GameInstance
	{
		// Token: 0x06009603 RID: 38403 RVA: 0x0037E421 File Offset: 0x0037C621
		public StatesInstance(DevHEPSpawner smi) : base(smi)
		{
		}
	}

	// Token: 0x0200161E RID: 5662
	public class States : GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner>
	{
		// Token: 0x06009604 RID: 38404 RVA: 0x0037E42C File Offset: 0x0037C62C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false);
			this.ready.PlayAnim("on").TagTransition(GameTags.Operational, this.inoperational, true).Update(delegate(DevHEPSpawner.StatesInstance smi, float dt)
			{
				smi.master.LauncherUpdate(dt);
			}, UpdateRate.SIM_EVERY_TICK, false);
		}

		// Token: 0x040073CF RID: 29647
		public StateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.BoolParameter isAbsorbingRadiation;

		// Token: 0x040073D0 RID: 29648
		public GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.State ready;

		// Token: 0x040073D1 RID: 29649
		public GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.State inoperational;
	}
}
