using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020005DF RID: 1503
public class FallMonitor : GameStateMachine<FallMonitor, FallMonitor.Instance>
{
	// Token: 0x060022D6 RID: 8918 RVA: 0x000CAD70 File Offset: 0x000C8F70
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.standing;
		this.root.TagTransition(GameTags.Stored, this.instorage, false).Update("CheckLanded", delegate(FallMonitor.Instance smi, float dt)
		{
			smi.UpdateFalling();
		}, UpdateRate.SIM_33ms, true);
		this.standing.ParamTransition<bool>(this.isEntombed, this.entombed, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsTrue).ParamTransition<bool>(this.isFalling, this.falling_pre, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsTrue);
		this.falling_pre.Enter("StopNavigator", delegate(FallMonitor.Instance smi)
		{
			smi.GetComponent<Navigator>().Stop(false, true);
		}).Enter("AttemptInitialRecovery", delegate(FallMonitor.Instance smi)
		{
			smi.AttemptInitialRecovery();
		}).GoTo(this.falling).ToggleBrain("falling_pre");
		this.falling.ToggleBrain("falling").PlayAnim("fall_pre").QueueAnim("fall_loop", true, null).ParamTransition<bool>(this.isEntombed, this.entombed, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsTrue).Transition(this.recoverladder, (FallMonitor.Instance smi) => smi.CanRecoverToLadder(), UpdateRate.SIM_33ms).Transition(this.recoverpole, (FallMonitor.Instance smi) => smi.CanRecoverToPole(), UpdateRate.SIM_33ms).ToggleGravity(this.landfloor);
		this.recoverinitialfall.ToggleBrain("recoverinitialfall").Enter("Recover", delegate(FallMonitor.Instance smi)
		{
			smi.Recover();
		}).EventTransition(GameHashes.DestinationReached, this.standing, null).EventTransition(GameHashes.NavigationFailed, this.standing, null).Exit(delegate(FallMonitor.Instance smi)
		{
			smi.RecoverEmote();
		});
		this.landfloor.Enter("Land", delegate(FallMonitor.Instance smi)
		{
			smi.LandFloor();
		}).GoTo(this.standing);
		this.recoverladder.ToggleBrain("recoverladder").PlayAnim("floor_ladder_0_0").Enter("MountLadder", delegate(FallMonitor.Instance smi)
		{
			smi.MountLadder();
		}).OnAnimQueueComplete(this.standing);
		this.recoverpole.ToggleBrain("recoverpole").PlayAnim("floor_pole_0_0").Enter("MountPole", delegate(FallMonitor.Instance smi)
		{
			smi.MountPole();
		}).OnAnimQueueComplete(this.standing);
		this.instorage.TagTransition(GameTags.Stored, this.standing, true);
		this.entombed.DefaultState(this.entombed.recovering);
		this.entombed.recovering.Enter("TryEntombedEscape", delegate(FallMonitor.Instance smi)
		{
			smi.TryEntombedEscape();
		});
		this.entombed.stuck.Enter("StopNavigator", delegate(FallMonitor.Instance smi)
		{
			smi.GetComponent<Navigator>().Stop(false, true);
		}).ToggleChore((FallMonitor.Instance smi) => new EntombedChore(smi.master, smi.entombedAnimOverride), this.standing).ParamTransition<bool>(this.isEntombed, this.standing, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsFalse);
	}

	// Token: 0x0400145C RID: 5212
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State standing;

	// Token: 0x0400145D RID: 5213
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State falling_pre;

	// Token: 0x0400145E RID: 5214
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State falling;

	// Token: 0x0400145F RID: 5215
	public FallMonitor.EntombedStates entombed;

	// Token: 0x04001460 RID: 5216
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverladder;

	// Token: 0x04001461 RID: 5217
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverpole;

	// Token: 0x04001462 RID: 5218
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverinitialfall;

	// Token: 0x04001463 RID: 5219
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State landfloor;

	// Token: 0x04001464 RID: 5220
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State instorage;

	// Token: 0x04001465 RID: 5221
	public StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.BoolParameter isEntombed;

	// Token: 0x04001466 RID: 5222
	public StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.BoolParameter isFalling;

	// Token: 0x020014BB RID: 5307
	public class EntombedStates : GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006F64 RID: 28516
		public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recovering;

		// Token: 0x04006F65 RID: 28517
		public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State stuck;
	}

	// Token: 0x020014BC RID: 5308
	public new class Instance : GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060090C7 RID: 37063 RVA: 0x0036F288 File Offset: 0x0036D488
		public Instance(IStateMachineTarget master, bool shouldPlayEmotes, string entombedAnimOverride = null) : base(master)
		{
			this.navigator = base.GetComponent<Navigator>();
			this.shouldPlayEmotes = shouldPlayEmotes;
			this.entombedAnimOverride = entombedAnimOverride;
			Pathfinding.Instance.FlushNavGridsOnLoad();
			base.Subscribe(915392638, FallMonitor.Instance.OnCellChangedDispatcher, this);
			base.Subscribe(1027377649, FallMonitor.Instance.OnMovementStateChangedDispatcher, this);
			base.Subscribe(387220196, FallMonitor.Instance.OnDestinationReachedDispatcher, this);
		}

		// Token: 0x060090C8 RID: 37064 RVA: 0x0036F378 File Offset: 0x0036D578
		private void OnDestinationReached(object data)
		{
			int item = Grid.PosToCell(base.transform.GetPosition());
			if (!this.safeCells.Contains(item))
			{
				this.safeCells.Add(item);
				if (this.safeCells.Count > this.MAX_CELLS_TRACKED)
				{
					this.safeCells.RemoveAt(0);
				}
			}
		}

		// Token: 0x060090C9 RID: 37065 RVA: 0x0036F3D0 File Offset: 0x0036D5D0
		private void OnMovementStateChanged(object data)
		{
			if (((Boxed<GameHashes>)data).value == GameHashes.ObjectMovementWakeUp)
			{
				int item = Grid.PosToCell(base.transform.GetPosition());
				if (!this.safeCells.Contains(item))
				{
					this.safeCells.Add(item);
					if (this.safeCells.Count > this.MAX_CELLS_TRACKED)
					{
						this.safeCells.RemoveAt(0);
					}
				}
			}
		}

		// Token: 0x060090CA RID: 37066 RVA: 0x0036F43C File Offset: 0x0036D63C
		private void OnCellChanged(object data)
		{
			int value = ((Boxed<int>)data).value;
			if (!this.safeCells.Contains(value))
			{
				this.safeCells.Add(value);
				if (this.safeCells.Count > this.MAX_CELLS_TRACKED)
				{
					this.safeCells.RemoveAt(0);
				}
			}
		}

		// Token: 0x060090CB RID: 37067 RVA: 0x0036F490 File Offset: 0x0036D690
		public void Recover()
		{
			int cell = Grid.PosToCell(this.navigator);
			foreach (NavGrid.Transition transition in this.navigator.NavGrid.transitions)
			{
				if (transition.isEscape && this.navigator.CurrentNavType == transition.start)
				{
					int num = transition.IsValid(cell, this.navigator.NavGrid.NavTable);
					if (Grid.InvalidCell != num)
					{
						Vector2I vector2I = Grid.CellToXY(cell);
						Vector2I vector2I2 = Grid.CellToXY(num);
						this.flipRecoverEmote = (vector2I2.x < vector2I.x);
						this.navigator.BeginTransition(transition);
						return;
					}
				}
			}
		}

		// Token: 0x060090CC RID: 37068 RVA: 0x0036F548 File Offset: 0x0036D748
		public void RecoverEmote()
		{
			if (!this.shouldPlayEmotes)
			{
				return;
			}
			if (UnityEngine.Random.Range(0, 9) == 8)
			{
				new EmoteChore(base.master.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, Db.Get().Emotes.Minion.CloseCall_Fall, KAnim.PlayMode.Once, 1, this.flipRecoverEmote);
			}
		}

		// Token: 0x060090CD RID: 37069 RVA: 0x0036F5A5 File Offset: 0x0036D7A5
		public void LandFloor()
		{
			this.navigator.SetCurrentNavType(NavType.Floor);
			base.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(base.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
		}

		// Token: 0x060090CE RID: 37070 RVA: 0x0036F5D8 File Offset: 0x0036D7D8
		public void AttemptInitialRecovery()
		{
			if (base.gameObject.HasTag(GameTags.Incapacitated))
			{
				return;
			}
			int cell = Grid.PosToCell(this.navigator);
			foreach (NavGrid.Transition transition in this.navigator.NavGrid.transitions)
			{
				if (transition.isEscape && this.navigator.CurrentNavType == transition.start)
				{
					int num = transition.IsValid(cell, this.navigator.NavGrid.NavTable);
					if (Grid.InvalidCell != num)
					{
						base.smi.GoTo(base.smi.sm.recoverinitialfall);
						return;
					}
				}
			}
		}

		// Token: 0x060090CF RID: 37071 RVA: 0x0036F688 File Offset: 0x0036D888
		public bool CanRecoverToLadder()
		{
			int cell = Grid.PosToCell(base.master.transform.GetPosition());
			return this.navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder) && !base.gameObject.HasTag(GameTags.Incapacitated);
		}

		// Token: 0x060090D0 RID: 37072 RVA: 0x0036F6D9 File Offset: 0x0036D8D9
		public void MountLadder()
		{
			this.navigator.SetCurrentNavType(NavType.Ladder);
			base.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(base.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
		}

		// Token: 0x060090D1 RID: 37073 RVA: 0x0036F70C File Offset: 0x0036D90C
		public bool CanRecoverToPole()
		{
			int cell = Grid.PosToCell(base.master.transform.GetPosition());
			return this.navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole) && !base.gameObject.HasTag(GameTags.Incapacitated);
		}

		// Token: 0x060090D2 RID: 37074 RVA: 0x0036F75D File Offset: 0x0036D95D
		public void MountPole()
		{
			this.navigator.SetCurrentNavType(NavType.Pole);
			base.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(base.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
		}

		// Token: 0x060090D3 RID: 37075 RVA: 0x0036F790 File Offset: 0x0036D990
		public void UpdateFalling()
		{
			bool value = false;
			bool flag = false;
			if (!this.navigator.IsMoving() && this.navigator.CurrentNavType != NavType.Tube)
			{
				int num = Grid.PosToCell(base.transform.GetPosition());
				int num2 = Grid.CellAbove(num);
				bool flag2 = Grid.IsValidCell(num);
				bool flag3 = Grid.IsValidCell(num2);
				bool flag4 = this.IsValidNavCell(num);
				flag4 = (flag4 && (!base.gameObject.HasTag(GameTags.Incapacitated) || (this.navigator.CurrentNavType != NavType.Ladder && this.navigator.CurrentNavType != NavType.Pole)));
				flag = ((!flag4 && flag2 && Grid.Solid[num] && !Grid.DupePassable[num]) || (flag3 && Grid.Solid[num2] && !Grid.DupePassable[num2]) || (flag2 && Grid.DupeImpassable[num]) || (flag3 && Grid.DupeImpassable[num2]));
				value = (!flag4 && !flag);
				if ((!flag2 && flag3) || (flag3 && Grid.WorldIdx[num] != Grid.WorldIdx[num2] && Grid.IsWorldValidCell(num2)))
				{
					this.TeleportInWorld(num);
				}
			}
			base.sm.isFalling.Set(value, base.smi, false);
			base.sm.isEntombed.Set(flag, base.smi, false);
		}

		// Token: 0x060090D4 RID: 37076 RVA: 0x0036F910 File Offset: 0x0036DB10
		private void TeleportInWorld(int cell)
		{
			int num = Grid.CellAbove(cell);
			WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]);
			if (world != null)
			{
				int safeCell = world.GetSafeCell();
				global::Debug.Log(string.Format("Teleporting {0} to {1}", this.navigator.name, safeCell));
				this.MoveToCell(safeCell, false);
				return;
			}
			global::Debug.LogError(string.Format("Unable to teleport {0} stuck on {1}", this.navigator.name, cell));
		}

		// Token: 0x060090D5 RID: 37077 RVA: 0x0036F98F File Offset: 0x0036DB8F
		private bool IsValidNavCell(int cell)
		{
			return this.navigator.NavGrid.NavTable.IsValid(cell, this.navigator.CurrentNavType) && !Grid.DupeImpassable[cell];
		}

		// Token: 0x060090D6 RID: 37078 RVA: 0x0036F9C4 File Offset: 0x0036DBC4
		public void TryEntombedEscape()
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			int backCell = base.GetComponent<Facing>().GetBackCell();
			int num2 = Grid.CellAbove(backCell);
			int num3 = Grid.CellBelow(backCell);
			foreach (int num4 in new int[]
			{
				backCell,
				num2,
				num3
			})
			{
				if (this.IsValidNavCell(num4) && !Grid.HasDoor[num4])
				{
					this.MoveToCell(num4, false);
					return;
				}
			}
			int cell = Grid.PosToCell(base.transform.GetPosition());
			foreach (CellOffset offset in this.entombedEscapeOffsets)
			{
				if (Grid.IsCellOffsetValid(cell, offset))
				{
					int num5 = Grid.OffsetCell(cell, offset);
					if (this.IsValidNavCell(num5) && !Grid.HasDoor[num5])
					{
						this.MoveToCell(num5, false);
						return;
					}
				}
			}
			for (int k = this.safeCells.Count - 1; k >= 0; k--)
			{
				int num6 = this.safeCells[k];
				if (num6 != num && this.IsValidNavCell(num6) && !Grid.HasDoor[num6])
				{
					this.MoveToCell(num6, false);
					return;
				}
			}
			foreach (CellOffset offset2 in this.entombedEscapeOffsets)
			{
				if (Grid.IsCellOffsetValid(cell, offset2))
				{
					int num7 = Grid.OffsetCell(cell, offset2);
					int num8 = Grid.CellAbove(num7);
					if (Grid.IsValidCell(num8) && !Grid.Solid[num7] && !Grid.Solid[num8] && !Grid.DupeImpassable[num7] && !Grid.DupeImpassable[num8] && !Grid.HasDoor[num7] && !Grid.HasDoor[num8])
					{
						this.MoveToCell(num7, true);
						return;
					}
				}
			}
			this.GoTo(base.sm.entombed.stuck);
		}

		// Token: 0x060090D7 RID: 37079 RVA: 0x0036FBD8 File Offset: 0x0036DDD8
		private void MoveToCell(int cell, bool forceFloorNav = false)
		{
			base.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
			base.transform.GetComponent<Navigator>().Stop(false, true);
			if (base.gameObject.HasTag(GameTags.Incapacitated) || forceFloorNav)
			{
				base.transform.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
			}
			this.UpdateFalling();
			if (base.sm.isEntombed.Get(base.smi))
			{
				this.GoTo(base.sm.entombed.stuck);
				return;
			}
			this.GoTo(base.sm.standing);
		}

		// Token: 0x04006F66 RID: 28518
		private CellOffset[] entombedEscapeOffsets = new CellOffset[]
		{
			new CellOffset(0, 1),
			new CellOffset(1, 0),
			new CellOffset(-1, 0),
			new CellOffset(1, 1),
			new CellOffset(-1, 1),
			new CellOffset(1, -1),
			new CellOffset(-1, -1)
		};

		// Token: 0x04006F67 RID: 28519
		private Navigator navigator;

		// Token: 0x04006F68 RID: 28520
		private bool shouldPlayEmotes;

		// Token: 0x04006F69 RID: 28521
		public string entombedAnimOverride;

		// Token: 0x04006F6A RID: 28522
		private List<int> safeCells = new List<int>();

		// Token: 0x04006F6B RID: 28523
		private int MAX_CELLS_TRACKED = 3;

		// Token: 0x04006F6C RID: 28524
		private static Action<object, object> OnDestinationReachedDispatcher = delegate(object context, object data)
		{
			Unsafe.As<FallMonitor.Instance>(context).OnDestinationReached(data);
		};

		// Token: 0x04006F6D RID: 28525
		private static Action<object, object> OnMovementStateChangedDispatcher = delegate(object context, object data)
		{
			Unsafe.As<FallMonitor.Instance>(context).OnMovementStateChanged(data);
		};

		// Token: 0x04006F6E RID: 28526
		private static Action<object, object> OnCellChangedDispatcher = delegate(object context, object data)
		{
			Unsafe.As<FallMonitor.Instance>(context).OnCellChanged(data);
		};

		// Token: 0x04006F6F RID: 28527
		private bool flipRecoverEmote;
	}
}
