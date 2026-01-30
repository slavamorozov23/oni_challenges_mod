using System;
using UnityEngine;

// Token: 0x02000B16 RID: 2838
public class SweepBotTrappedMonitor : GameStateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>
{
	// Token: 0x060052AD RID: 21165 RVA: 0x001E14A4 File Offset: 0x001DF6A4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.notTrapped;
		this.notTrapped.Update(delegate(SweepBotTrappedMonitor.Instance smi, float dt)
		{
			StorageUnloadMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
			Storage storage = smi2.sm.sweepLocker.Get(smi2);
			Navigator component = smi.master.GetComponent<Navigator>();
			if (storage == null)
			{
				smi.GoTo(this.death);
				return;
			}
			if ((smi.master.gameObject.HasTag(GameTags.Robots.Behaviours.RechargeBehaviour) || smi.master.gameObject.HasTag(GameTags.Robots.Behaviours.UnloadBehaviour)) && !component.CanReach(Grid.PosToCell(storage), SweepBotTrappedMonitor.defaultOffsets))
			{
				smi.GoTo(this.trapped);
			}
		}, UpdateRate.SIM_1000ms, false);
		this.trapped.ToggleBehaviour(GameTags.Robots.Behaviours.TrappedBehaviour, (SweepBotTrappedMonitor.Instance data) => true, null).ToggleStatusItem(Db.Get().RobotStatusItems.CantReachStation, null, Db.Get().StatusItemCategories.Main).Update(delegate(SweepBotTrappedMonitor.Instance smi, float dt)
		{
			StorageUnloadMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
			Storage storage = smi2.sm.sweepLocker.Get(smi2);
			Navigator component = smi.master.GetComponent<Navigator>();
			if (storage == null)
			{
				smi.GoTo(this.death);
			}
			else if ((!smi.master.gameObject.HasTag(GameTags.Robots.Behaviours.RechargeBehaviour) && !smi.master.gameObject.HasTag(GameTags.Robots.Behaviours.UnloadBehaviour)) || component.CanReach(Grid.PosToCell(storage), SweepBotTrappedMonitor.defaultOffsets))
			{
				smi.GoTo(this.notTrapped);
			}
			if (storage != null && component.CanReach(Grid.PosToCell(storage), SweepBotTrappedMonitor.defaultOffsets))
			{
				smi.GoTo(this.notTrapped);
				return;
			}
			if (storage == null)
			{
				smi.GoTo(this.death);
			}
		}, UpdateRate.SIM_1000ms, false);
		this.death.Enter(delegate(SweepBotTrappedMonitor.Instance smi)
		{
			smi.master.gameObject.GetComponent<MovingOrnamentReceptacle>().OrderRemoveOccupant();
			smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("death");
		}).OnAnimQueueComplete(this.destroySelf);
		this.destroySelf.Enter(delegate(SweepBotTrappedMonitor.Instance smi)
		{
			Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactDust, smi.master.transform.position, 0f);
			foreach (Storage storage in smi.master.gameObject.GetComponents<Storage>())
			{
				for (int j = storage.items.Count - 1; j >= 0; j--)
				{
					GameObject gameObject = storage.Drop(storage.items[j], true);
					if (gameObject != null)
					{
						if (GameComps.Fallers.Has(gameObject))
						{
							GameComps.Fallers.Remove(gameObject);
						}
						GameComps.Fallers.Add(gameObject, new Vector2((float)UnityEngine.Random.Range(-5, 5), 8f));
					}
				}
			}
			PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
			component.Element.substance.SpawnResource(Grid.CellToPosCCC(Grid.PosToCell(smi.master.gameObject), Grid.SceneLayer.Ore), SweepBotConfig.MASS, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false, false, false);
			Util.KDestroyGameObject(smi.master.gameObject);
		});
	}

	// Token: 0x040037DD RID: 14301
	public static CellOffset[] defaultOffsets = new CellOffset[]
	{
		new CellOffset(0, 0)
	};

	// Token: 0x040037DE RID: 14302
	public GameStateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>.State notTrapped;

	// Token: 0x040037DF RID: 14303
	public GameStateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>.State trapped;

	// Token: 0x040037E0 RID: 14304
	public GameStateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>.State death;

	// Token: 0x040037E1 RID: 14305
	public GameStateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>.State destroySelf;

	// Token: 0x02001C5B RID: 7259
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001C5C RID: 7260
	public new class Instance : GameStateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>.GameInstance
	{
		// Token: 0x0600AD34 RID: 44340 RVA: 0x003CED87 File Offset: 0x003CCF87
		public Instance(IStateMachineTarget master, SweepBotTrappedMonitor.Def def) : base(master, def)
		{
		}
	}
}
