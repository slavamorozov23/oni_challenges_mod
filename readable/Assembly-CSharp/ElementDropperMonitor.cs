using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200089C RID: 2204
public class ElementDropperMonitor : GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>
{
	// Token: 0x06003C9E RID: 15518 RVA: 0x00152FF0 File Offset: 0x001511F0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.EventHandler(GameHashes.DeathAnimComplete, delegate(ElementDropperMonitor.Instance smi)
		{
			smi.DropDeathElement();
		});
		this.satisfied.OnSignal(this.cellChangedSignal, this.readytodrop, (ElementDropperMonitor.Instance smi, StateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.SignalParameter param) => smi.ShouldDropElement());
		this.readytodrop.ToggleBehaviour(GameTags.Creatures.WantsToDropElements, (ElementDropperMonitor.Instance smi) => true, delegate(ElementDropperMonitor.Instance smi)
		{
			smi.GoTo(this.satisfied);
		}).EventHandler(GameHashes.ObjectMovementStateChanged, delegate(ElementDropperMonitor.Instance smi, object d)
		{
			if (Boxed<GameHashes>.Unbox(d) == GameHashes.ObjectMovementWakeUp)
			{
				smi.GoTo(this.satisfied);
			}
		});
	}

	// Token: 0x0400256C RID: 9580
	public GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.State satisfied;

	// Token: 0x0400256D RID: 9581
	public GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.State readytodrop;

	// Token: 0x0400256E RID: 9582
	public StateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.Signal cellChangedSignal;

	// Token: 0x02001880 RID: 6272
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007B13 RID: 31507
		public SimHashes dirtyEmitElement;

		// Token: 0x04007B14 RID: 31508
		public float dirtyProbabilityPercent;

		// Token: 0x04007B15 RID: 31509
		public float dirtyCellToTargetMass;

		// Token: 0x04007B16 RID: 31510
		public float dirtyMassPerDirty;

		// Token: 0x04007B17 RID: 31511
		public float dirtyMassReleaseOnDeath;

		// Token: 0x04007B18 RID: 31512
		public byte emitDiseaseIdx = byte.MaxValue;

		// Token: 0x04007B19 RID: 31513
		public float emitDiseasePerKg;
	}

	// Token: 0x02001881 RID: 6273
	public new class Instance : GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.GameInstance
	{
		// Token: 0x06009F21 RID: 40737 RVA: 0x003A5141 File Offset: 0x003A3341
		public Instance(IStateMachineTarget master, ElementDropperMonitor.Def def) : base(master, def)
		{
			this.cellChangeHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, ElementDropperMonitor.Instance.OnCellChangeDispatcher, this, "ElementDropperMonitor.Instance");
		}

		// Token: 0x06009F22 RID: 40738 RVA: 0x003A516C File Offset: 0x003A336C
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangeHandlerID);
		}

		// Token: 0x06009F23 RID: 40739 RVA: 0x003A5185 File Offset: 0x003A3385
		private void OnCellChange()
		{
			base.sm.cellChangedSignal.Trigger(this);
		}

		// Token: 0x06009F24 RID: 40740 RVA: 0x003A5198 File Offset: 0x003A3398
		public bool ShouldDropElement()
		{
			return this.IsValidDropCell() && UnityEngine.Random.Range(0f, 100f) < base.def.dirtyProbabilityPercent;
		}

		// Token: 0x06009F25 RID: 40741 RVA: 0x003A51C0 File Offset: 0x003A33C0
		public void DropDeathElement()
		{
			this.DropElement(base.def.dirtyMassReleaseOnDeath, base.def.dirtyEmitElement, base.def.emitDiseaseIdx, Mathf.RoundToInt(base.def.dirtyMassReleaseOnDeath * base.def.dirtyMassPerDirty));
		}

		// Token: 0x06009F26 RID: 40742 RVA: 0x003A5210 File Offset: 0x003A3410
		public void DropPeriodicElement()
		{
			this.DropElement(base.def.dirtyMassPerDirty, base.def.dirtyEmitElement, base.def.emitDiseaseIdx, Mathf.RoundToInt(base.def.emitDiseasePerKg * base.def.dirtyMassPerDirty));
		}

		// Token: 0x06009F27 RID: 40743 RVA: 0x003A5260 File Offset: 0x003A3460
		public void DropElement(float mass, SimHashes element_id, byte disease_idx, int disease_count)
		{
			if (mass <= 0f)
			{
				return;
			}
			Element element = ElementLoader.FindElementByHash(element_id);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			if (element.IsGas || element.IsLiquid)
			{
				SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.GetPosition()), element_id, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, temperature, disease_idx, disease_count, true, -1);
			}
			else if (element.IsSolid)
			{
				element.substance.SpawnResource(base.transform.GetPosition() + new Vector3(0f, 0.5f, 0f), mass, temperature, disease_idx, disease_count, false, true, false);
			}
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, element.name, base.gameObject.transform, 1.5f, false);
		}

		// Token: 0x06009F28 RID: 40744 RVA: 0x003A5330 File Offset: 0x003A3530
		public bool IsValidDropCell()
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			return Grid.IsValidCell(num) && Grid.IsGas(num) && Grid.Mass[num] <= 1f;
		}

		// Token: 0x04007B1A RID: 31514
		private ulong cellChangeHandlerID;

		// Token: 0x04007B1B RID: 31515
		private static readonly Action<object> OnCellChangeDispatcher = delegate(object obj)
		{
			Unsafe.As<ElementDropperMonitor.Instance>(obj).OnCellChange();
		};
	}
}
