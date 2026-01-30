using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020008AA RID: 2218
[SkipSaveFileSerialization]
public class IlluminationVulnerable : StateMachineComponent<IlluminationVulnerable.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, IIlluminationTracker
{
	// Token: 0x17000429 RID: 1065
	// (get) Token: 0x06003D22 RID: 15650 RVA: 0x001553D7 File Offset: 0x001535D7
	public int LightIntensityThreshold
	{
		get
		{
			if (this.minLuxAttributeInstance != null)
			{
				return Mathf.RoundToInt(this.minLuxAttributeInstance.GetTotalValue());
			}
			return Mathf.RoundToInt(base.GetComponent<Modifiers>().GetPreModifiedAttributeValue(Db.Get().PlantAttributes.MinLightLux));
		}
	}

	// Token: 0x06003D23 RID: 15651 RVA: 0x00155411 File Offset: 0x00153611
	public string GetIlluminationUITooltip()
	{
		if ((this.prefersDarkness && this.IsComfortable()) || (!this.prefersDarkness && !this.IsComfortable()))
		{
			return UI.TOOLTIPS.VITALS_CHECKBOX_ILLUMINATION_DARK;
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_ILLUMINATION_LIGHT;
	}

	// Token: 0x06003D24 RID: 15652 RVA: 0x00155448 File Offset: 0x00153648
	public string GetIlluminationUILabel()
	{
		return Db.Get().Amounts.Illumination.Name + "\n    • " + (this.prefersDarkness ? UI.GAMEOBJECTEFFECTS.DARKNESS.ToString() : GameUtil.GetFormattedLux(this.LightIntensityThreshold));
	}

	// Token: 0x06003D25 RID: 15653 RVA: 0x00155487 File Offset: 0x00153687
	public bool ShouldIlluminationUICheckboxBeChecked()
	{
		return this.IsComfortable();
	}

	// Token: 0x1700042A RID: 1066
	// (get) Token: 0x06003D26 RID: 15654 RVA: 0x0015548F File Offset: 0x0015368F
	private OccupyArea occupyArea
	{
		get
		{
			if (this._occupyArea == null)
			{
				this._occupyArea = base.GetComponent<OccupyArea>();
			}
			return this._occupyArea;
		}
	}

	// Token: 0x06003D27 RID: 15655 RVA: 0x001554B4 File Offset: 0x001536B4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.GetAmounts().Add(new AmountInstance(Db.Get().Amounts.Illumination, base.gameObject));
		this.minLuxAttributeInstance = base.gameObject.GetAttributes().Add(Db.Get().PlantAttributes.MinLightLux);
	}

	// Token: 0x06003D28 RID: 15656 RVA: 0x00155517 File Offset: 0x00153717
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06003D29 RID: 15657 RVA: 0x0015552A File Offset: 0x0015372A
	public void SetPrefersDarkness(bool prefersDarkness = false)
	{
		this.prefersDarkness = prefersDarkness;
	}

	// Token: 0x06003D2A RID: 15658 RVA: 0x00155533 File Offset: 0x00153733
	protected override void OnCleanUp()
	{
		this.handle.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x06003D2B RID: 15659 RVA: 0x00155546 File Offset: 0x00153746
	public bool IsCellSafe(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		if (this.prefersDarkness)
		{
			return Grid.LightIntensity[cell] == 0;
		}
		return Grid.LightIntensity[cell] >= this.LightIntensityThreshold;
	}

	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x06003D2C RID: 15660 RVA: 0x0015557F File Offset: 0x0015377F
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Darkness,
				WiltCondition.Condition.IlluminationComfort
			};
		}
	}

	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x06003D2D RID: 15661 RVA: 0x00155590 File Offset: 0x00153790
	public string WiltStateString
	{
		get
		{
			if (base.smi.IsInsideState(base.smi.sm.too_bright))
			{
				return Db.Get().CreatureStatusItems.Crop_Too_Bright.GetName(this);
			}
			if (base.smi.IsInsideState(base.smi.sm.too_dark))
			{
				return Db.Get().CreatureStatusItems.Crop_Too_Dark.GetName(this);
			}
			return "";
		}
	}

	// Token: 0x06003D2E RID: 15662 RVA: 0x00155608 File Offset: 0x00153808
	public bool IsComfortable()
	{
		return base.smi.IsInsideState(base.smi.sm.comfortable);
	}

	// Token: 0x06003D2F RID: 15663 RVA: 0x00155628 File Offset: 0x00153828
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		if (this.prefersDarkness)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_DARKNESS, UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_DARKNESS, Descriptor.DescriptorType.Requirement, false)
			};
		}
		return new List<Descriptor>
		{
			new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_LIGHT.Replace("{Lux}", GameUtil.GetFormattedLux(this.LightIntensityThreshold)), UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_LIGHT.Replace("{Lux}", GameUtil.GetFormattedLux(this.LightIntensityThreshold)), Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x040025BA RID: 9658
	private OccupyArea _occupyArea;

	// Token: 0x040025BB RID: 9659
	private SchedulerHandle handle;

	// Token: 0x040025BC RID: 9660
	public bool prefersDarkness;

	// Token: 0x040025BD RID: 9661
	private AttributeInstance minLuxAttributeInstance;

	// Token: 0x020018A0 RID: 6304
	public class StatesInstance : GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.GameInstance
	{
		// Token: 0x06009F99 RID: 40857 RVA: 0x003A6F0E File Offset: 0x003A510E
		public StatesInstance(IlluminationVulnerable master) : base(master)
		{
		}

		// Token: 0x04007B7A RID: 31610
		public bool hasMaturity;
	}

	// Token: 0x020018A1 RID: 6305
	public class States : GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable>
	{
		// Token: 0x06009F9A RID: 40858 RVA: 0x003A6F18 File Offset: 0x003A5118
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.comfortable;
			this.root.Update("Illumination", delegate(IlluminationVulnerable.StatesInstance smi, float dt)
			{
				int num = Grid.PosToCell(smi.master.gameObject);
				if (Grid.IsValidCell(num))
				{
					smi.master.GetAmounts().Get(Db.Get().Amounts.Illumination).SetValue((float)Grid.LightCount[num]);
					return;
				}
				smi.master.GetAmounts().Get(Db.Get().Amounts.Illumination).SetValue(0f);
			}, UpdateRate.SIM_1000ms, false);
			this.comfortable.Update("Illumination.Comfortable", delegate(IlluminationVulnerable.StatesInstance smi, float dt)
			{
				int cell = Grid.PosToCell(smi.master.gameObject);
				if (!smi.master.IsCellSafe(cell))
				{
					GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State state = smi.master.prefersDarkness ? this.too_bright : this.too_dark;
					smi.GoTo(state);
				}
			}, UpdateRate.SIM_1000ms, false).Enter(delegate(IlluminationVulnerable.StatesInstance smi)
			{
				smi.master.Trigger(1113102781, null);
			});
			this.too_dark.TriggerOnEnter(GameHashes.IlluminationDiscomfort, null).Update("Illumination.too_dark", delegate(IlluminationVulnerable.StatesInstance smi, float dt)
			{
				int cell = Grid.PosToCell(smi.master.gameObject);
				if (smi.master.IsCellSafe(cell))
				{
					smi.GoTo(this.comfortable);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.too_bright.TriggerOnEnter(GameHashes.IlluminationDiscomfort, null).Update("Illumination.too_bright", delegate(IlluminationVulnerable.StatesInstance smi, float dt)
			{
				int cell = Grid.PosToCell(smi.master.gameObject);
				if (smi.master.IsCellSafe(cell))
				{
					smi.GoTo(this.comfortable);
				}
			}, UpdateRate.SIM_1000ms, false);
		}

		// Token: 0x04007B7B RID: 31611
		public StateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.BoolParameter illuminated;

		// Token: 0x04007B7C RID: 31612
		public GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State comfortable;

		// Token: 0x04007B7D RID: 31613
		public GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State too_dark;

		// Token: 0x04007B7E RID: 31614
		public GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State too_bright;
	}
}
