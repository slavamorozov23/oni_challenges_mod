using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000984 RID: 2436
public class HEPBattery : GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>
{
	// Token: 0x060045D1 RID: 17873 RVA: 0x00193D10 File Offset: 0x00191F10
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false).Update(delegate(HEPBattery.Instance smi, float dt)
		{
			smi.DoConsumeParticlesWhileDisabled(dt);
			smi.UpdateDecayStatusItem(false);
		}, UpdateRate.SIM_200ms, false);
		this.operational.Enter("SetActive(true)", delegate(HEPBattery.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit("SetActive(false)", delegate(HEPBattery.Instance smi)
		{
			smi.operational.SetActive(false, false);
		}).PlayAnim("on", KAnim.PlayMode.Loop).TagTransition(GameTags.Operational, this.inoperational, true).Update(new Action<HEPBattery.Instance, float>(this.LauncherUpdate), UpdateRate.SIM_200ms, false);
	}

	// Token: 0x060045D2 RID: 17874 RVA: 0x00193DF8 File Offset: 0x00191FF8
	public void LauncherUpdate(HEPBattery.Instance smi, float dt)
	{
		smi.UpdateDecayStatusItem(true);
		smi.UpdateMeter(null);
		smi.operational.SetActive(smi.particleStorage.Particles > 0f, false);
		smi.launcherTimer += dt;
		if (smi.launcherTimer < smi.def.minLaunchInterval || !smi.AllowSpawnParticles)
		{
			return;
		}
		if (smi.particleStorage.Particles >= smi.particleThreshold)
		{
			smi.launcherTimer = 0f;
			this.Fire(smi);
		}
	}

	// Token: 0x060045D3 RID: 17875 RVA: 0x00193E80 File Offset: 0x00192080
	public void Fire(HEPBattery.Instance smi)
	{
		int highEnergyParticleOutputCell = smi.GetComponent<Building>().GetHighEnergyParticleOutputCell();
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
		gameObject.SetActive(true);
		if (gameObject != null)
		{
			HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
			component.payload = smi.particleStorage.ConsumeAndGet(smi.particleThreshold);
			component.SetDirection(smi.def.direction);
		}
	}

	// Token: 0x04002F09 RID: 12041
	public static readonly HashedString FIRE_PORT_ID = "HEPBatteryFire";

	// Token: 0x04002F0A RID: 12042
	public GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.State inoperational;

	// Token: 0x04002F0B RID: 12043
	public GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.State operational;

	// Token: 0x020019EB RID: 6635
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007F8B RID: 32651
		public float particleDecayRate;

		// Token: 0x04007F8C RID: 32652
		public float minLaunchInterval;

		// Token: 0x04007F8D RID: 32653
		public float minSlider;

		// Token: 0x04007F8E RID: 32654
		public float maxSlider;

		// Token: 0x04007F8F RID: 32655
		public EightDirection direction;
	}

	// Token: 0x020019EC RID: 6636
	public new class Instance : GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.GameInstance, ISingleSliderControl, ISliderControl
	{
		// Token: 0x0600A34D RID: 41805 RVA: 0x003B1690 File Offset: 0x003AF890
		public Instance(IStateMachineTarget master, HEPBattery.Def def) : base(master, def)
		{
			base.Subscribe(-801688580, new Action<object>(this.OnLogicValueChanged));
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
			this.meterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
			this.UpdateMeter(null);
		}

		// Token: 0x0600A34E RID: 41806 RVA: 0x003B171C File Offset: 0x003AF91C
		public void DoConsumeParticlesWhileDisabled(float dt)
		{
			if (this.m_skipFirstUpdate)
			{
				this.m_skipFirstUpdate = false;
				return;
			}
			this.particleStorage.ConsumeAndGet(dt * base.def.particleDecayRate);
			this.UpdateMeter(null);
		}

		// Token: 0x0600A34F RID: 41807 RVA: 0x003B174E File Offset: 0x003AF94E
		public void UpdateMeter(object data = null)
		{
			this.meterController.SetPositionPercent(this.particleStorage.Particles / this.particleStorage.Capacity());
		}

		// Token: 0x0600A350 RID: 41808 RVA: 0x003B1774 File Offset: 0x003AF974
		public void UpdateDecayStatusItem(bool hasPower)
		{
			if (!hasPower)
			{
				if (this.particleStorage.Particles > 0f)
				{
					if (this.statusHandle == Guid.Empty)
					{
						this.statusHandle = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.LosingRadbolts, null);
						return;
					}
				}
				else if (this.statusHandle != Guid.Empty)
				{
					base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
					this.statusHandle = Guid.Empty;
					return;
				}
			}
			else if (this.statusHandle != Guid.Empty)
			{
				base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
				this.statusHandle = Guid.Empty;
			}
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x0600A351 RID: 41809 RVA: 0x003B182E File Offset: 0x003AFA2E
		public bool AllowSpawnParticles
		{
			get
			{
				return this.hasLogicWire && this.isLogicActive;
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x0600A352 RID: 41810 RVA: 0x003B1840 File Offset: 0x003AFA40
		public bool HasLogicWire
		{
			get
			{
				return this.hasLogicWire;
			}
		}

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x0600A353 RID: 41811 RVA: 0x003B1848 File Offset: 0x003AFA48
		public bool IsLogicActive
		{
			get
			{
				return this.isLogicActive;
			}
		}

		// Token: 0x0600A354 RID: 41812 RVA: 0x003B1850 File Offset: 0x003AFA50
		private LogicCircuitNetwork GetNetwork()
		{
			int portCell = base.GetComponent<LogicPorts>().GetPortCell(HEPBattery.FIRE_PORT_ID);
			return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}

		// Token: 0x0600A355 RID: 41813 RVA: 0x003B1880 File Offset: 0x003AFA80
		private void OnLogicValueChanged(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID == HEPBattery.FIRE_PORT_ID)
			{
				this.isLogicActive = (logicValueChanged.newValue > 0);
				this.hasLogicWire = (this.GetNetwork() != null);
			}
		}

		// Token: 0x0600A356 RID: 41814 RVA: 0x003B18C4 File Offset: 0x003AFAC4
		private void OnCopySettings(object data)
		{
			GameObject gameObject = data as GameObject;
			if (gameObject != null)
			{
				HEPBattery.Instance smi = gameObject.GetSMI<HEPBattery.Instance>();
				if (smi != null)
				{
					this.particleThreshold = smi.particleThreshold;
				}
			}
		}

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x0600A357 RID: 41815 RVA: 0x003B18F1 File Offset: 0x003AFAF1
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TITLE";
			}
		}

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x0600A358 RID: 41816 RVA: 0x003B18F8 File Offset: 0x003AFAF8
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
			}
		}

		// Token: 0x0600A359 RID: 41817 RVA: 0x003B1904 File Offset: 0x003AFB04
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x0600A35A RID: 41818 RVA: 0x003B1907 File Offset: 0x003AFB07
		public float GetSliderMin(int index)
		{
			return base.def.minSlider;
		}

		// Token: 0x0600A35B RID: 41819 RVA: 0x003B1914 File Offset: 0x003AFB14
		public float GetSliderMax(int index)
		{
			return base.def.maxSlider;
		}

		// Token: 0x0600A35C RID: 41820 RVA: 0x003B1921 File Offset: 0x003AFB21
		public float GetSliderValue(int index)
		{
			return this.particleThreshold;
		}

		// Token: 0x0600A35D RID: 41821 RVA: 0x003B1929 File Offset: 0x003AFB29
		public void SetSliderValue(float value, int index)
		{
			this.particleThreshold = value;
		}

		// Token: 0x0600A35E RID: 41822 RVA: 0x003B1932 File Offset: 0x003AFB32
		public string GetSliderTooltipKey(int index)
		{
			return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP";
		}

		// Token: 0x0600A35F RID: 41823 RVA: 0x003B1939 File Offset: 0x003AFB39
		string ISliderControl.GetSliderTooltip(int index)
		{
			return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP"), this.particleThreshold);
		}

		// Token: 0x04007F90 RID: 32656
		[MyCmpReq]
		public HighEnergyParticleStorage particleStorage;

		// Token: 0x04007F91 RID: 32657
		[MyCmpGet]
		public Operational operational;

		// Token: 0x04007F92 RID: 32658
		[MyCmpAdd]
		public CopyBuildingSettings copyBuildingSettings;

		// Token: 0x04007F93 RID: 32659
		[Serialize]
		public float launcherTimer;

		// Token: 0x04007F94 RID: 32660
		[Serialize]
		public float particleThreshold = 50f;

		// Token: 0x04007F95 RID: 32661
		public bool ShowWorkingStatus;

		// Token: 0x04007F96 RID: 32662
		private bool m_skipFirstUpdate = true;

		// Token: 0x04007F97 RID: 32663
		private MeterController meterController;

		// Token: 0x04007F98 RID: 32664
		private Guid statusHandle = Guid.Empty;

		// Token: 0x04007F99 RID: 32665
		private bool hasLogicWire;

		// Token: 0x04007F9A RID: 32666
		private bool isLogicActive;
	}
}
