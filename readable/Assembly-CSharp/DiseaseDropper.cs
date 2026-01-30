using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000898 RID: 2200
public class DiseaseDropper : GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>
{
	// Token: 0x06003C88 RID: 15496 RVA: 0x00152840 File Offset: 0x00150A40
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.stopped;
		this.root.EventHandler(GameHashes.BurstEmitDisease, delegate(DiseaseDropper.Instance smi)
		{
			smi.DropSingleEmit();
		});
		this.working.TagTransition(GameTags.PreventEmittingDisease, this.stopped, false).Update(delegate(DiseaseDropper.Instance smi, float dt)
		{
			smi.DropPeriodic(dt);
		}, UpdateRate.SIM_200ms, false);
		this.stopped.TagTransition(GameTags.PreventEmittingDisease, this.working, true);
	}

	// Token: 0x04002551 RID: 9553
	public GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.State working;

	// Token: 0x04002552 RID: 9554
	public GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.State stopped;

	// Token: 0x04002553 RID: 9555
	public StateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.Signal cellChangedSignal;

	// Token: 0x02001878 RID: 6264
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06009EFF RID: 40703 RVA: 0x003A4A10 File Offset: 0x003A2C10
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			if (this.singleEmitQuantity > 0)
			{
				list.Add(new Descriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_DROPPER_BURST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.diseaseIdx, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.singleEmitQuantity, GameUtil.TimeSlice.None)), UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.DISEASE_DROPPER_BURST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.diseaseIdx, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.singleEmitQuantity, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
			}
			if (this.averageEmitPerSecond > 0)
			{
				list.Add(new Descriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_DROPPER_CONSTANT.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.diseaseIdx, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.averageEmitPerSecond, GameUtil.TimeSlice.PerSecond)), UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.DISEASE_DROPPER_CONSTANT.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.diseaseIdx, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.averageEmitPerSecond, GameUtil.TimeSlice.PerSecond)), Descriptor.DescriptorType.Effect, false));
			}
			return list;
		}

		// Token: 0x04007AFA RID: 31482
		public byte diseaseIdx = byte.MaxValue;

		// Token: 0x04007AFB RID: 31483
		public int singleEmitQuantity;

		// Token: 0x04007AFC RID: 31484
		public int averageEmitPerSecond;

		// Token: 0x04007AFD RID: 31485
		public float emitFrequency = 1f;
	}

	// Token: 0x02001879 RID: 6265
	public new class Instance : GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.GameInstance
	{
		// Token: 0x06009F01 RID: 40705 RVA: 0x003A4B32 File Offset: 0x003A2D32
		public Instance(IStateMachineTarget master, DiseaseDropper.Def def) : base(master, def)
		{
		}

		// Token: 0x06009F02 RID: 40706 RVA: 0x003A4B3C File Offset: 0x003A2D3C
		public bool ShouldDropDisease()
		{
			return true;
		}

		// Token: 0x06009F03 RID: 40707 RVA: 0x003A4B3F File Offset: 0x003A2D3F
		public void DropSingleEmit()
		{
			this.DropDisease(base.def.diseaseIdx, base.def.singleEmitQuantity);
		}

		// Token: 0x06009F04 RID: 40708 RVA: 0x003A4B60 File Offset: 0x003A2D60
		public void DropPeriodic(float dt)
		{
			this.timeSinceLastDrop += dt;
			if (base.def.averageEmitPerSecond > 0 && base.def.emitFrequency > 0f)
			{
				while (this.timeSinceLastDrop > base.def.emitFrequency)
				{
					this.DropDisease(base.def.diseaseIdx, (int)((float)base.def.averageEmitPerSecond * base.def.emitFrequency));
					this.timeSinceLastDrop -= base.def.emitFrequency;
				}
			}
		}

		// Token: 0x06009F05 RID: 40709 RVA: 0x003A4BF4 File Offset: 0x003A2DF4
		public void DropDisease(byte disease_idx, int disease_count)
		{
			if (disease_count <= 0 || disease_idx == 255)
			{
				return;
			}
			int num = Grid.PosToCell(base.transform.GetPosition());
			if (!Grid.IsValidCell(num))
			{
				return;
			}
			SimMessages.ModifyDiseaseOnCell(num, disease_idx, disease_count);
		}

		// Token: 0x06009F06 RID: 40710 RVA: 0x003A4C30 File Offset: 0x003A2E30
		public bool IsValidDropCell()
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			return Grid.IsValidCell(num) && Grid.IsGas(num) && Grid.Mass[num] <= 1f;
		}

		// Token: 0x04007AFE RID: 31486
		private float timeSinceLastDrop;
	}
}
