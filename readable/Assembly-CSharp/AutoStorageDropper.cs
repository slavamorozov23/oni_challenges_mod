using System;
using UnityEngine;

// Token: 0x0200059C RID: 1436
public class AutoStorageDropper : GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>
{
	// Token: 0x06002041 RID: 8257 RVA: 0x000BA088 File Offset: 0x000B8288
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.root.Update(delegate(AutoStorageDropper.Instance smi, float dt)
		{
			smi.UpdateBlockedStatus();
		}, UpdateRate.SIM_200ms, true);
		this.idle.EventTransition(GameHashes.OnStorageChange, this.pre_drop, null).OnSignal(this.checkCanDrop, this.pre_drop, (AutoStorageDropper.Instance smi, StateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.SignalParameter param) => !smi.GetComponent<Storage>().IsEmpty()).ParamTransition<bool>(this.isBlocked, this.blocked, GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.IsTrue);
		this.pre_drop.ScheduleGoTo((AutoStorageDropper.Instance smi) => smi.def.delay, this.dropping);
		this.dropping.Enter(delegate(AutoStorageDropper.Instance smi)
		{
			smi.Drop();
		}).GoTo(this.idle);
		this.blocked.ParamTransition<bool>(this.isBlocked, this.pre_drop, GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.OutputTileBlocked, null);
	}

	// Token: 0x040012C0 RID: 4800
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State idle;

	// Token: 0x040012C1 RID: 4801
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State pre_drop;

	// Token: 0x040012C2 RID: 4802
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State dropping;

	// Token: 0x040012C3 RID: 4803
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State blocked;

	// Token: 0x040012C4 RID: 4804
	private StateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.BoolParameter isBlocked;

	// Token: 0x040012C5 RID: 4805
	public StateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.Signal checkCanDrop;

	// Token: 0x0200141B RID: 5147
	public class DropperFxConfig
	{
		// Token: 0x04006D83 RID: 28035
		public string animFile;

		// Token: 0x04006D84 RID: 28036
		public string animName;

		// Token: 0x04006D85 RID: 28037
		public Grid.SceneLayer layer = Grid.SceneLayer.FXFront;

		// Token: 0x04006D86 RID: 28038
		public bool useElementTint = true;

		// Token: 0x04006D87 RID: 28039
		public bool flipX;

		// Token: 0x04006D88 RID: 28040
		public bool flipY;
	}

	// Token: 0x0200141C RID: 5148
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006D89 RID: 28041
		public CellOffset dropOffset;

		// Token: 0x04006D8A RID: 28042
		public bool asOre;

		// Token: 0x04006D8B RID: 28043
		public SimHashes[] elementFilter;

		// Token: 0x04006D8C RID: 28044
		public bool invertElementFilterInitialValue;

		// Token: 0x04006D8D RID: 28045
		public bool blockedBySubstantialLiquid;

		// Token: 0x04006D8E RID: 28046
		public AutoStorageDropper.DropperFxConfig neutralFx;

		// Token: 0x04006D8F RID: 28047
		public AutoStorageDropper.DropperFxConfig leftFx;

		// Token: 0x04006D90 RID: 28048
		public AutoStorageDropper.DropperFxConfig rightFx;

		// Token: 0x04006D91 RID: 28049
		public AutoStorageDropper.DropperFxConfig upFx;

		// Token: 0x04006D92 RID: 28050
		public AutoStorageDropper.DropperFxConfig downFx;

		// Token: 0x04006D93 RID: 28051
		public Vector3 fxOffset = Vector3.zero;

		// Token: 0x04006D94 RID: 28052
		public float cooldown = 2f;

		// Token: 0x04006D95 RID: 28053
		public float delay;
	}

	// Token: 0x0200141D RID: 5149
	public new class Instance : GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.GameInstance
	{
		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06008E99 RID: 36505 RVA: 0x003693EA File Offset: 0x003675EA
		// (set) Token: 0x06008E9A RID: 36506 RVA: 0x003693F2 File Offset: 0x003675F2
		public bool isInvertElementFilter { get; private set; }

		// Token: 0x06008E9B RID: 36507 RVA: 0x003693FB File Offset: 0x003675FB
		public Instance(IStateMachineTarget master, AutoStorageDropper.Def def) : base(master, def)
		{
			this.isInvertElementFilter = def.invertElementFilterInitialValue;
		}

		// Token: 0x06008E9C RID: 36508 RVA: 0x00369411 File Offset: 0x00367611
		public void SetInvertElementFilter(bool value)
		{
			base.smi.isInvertElementFilter = value;
			base.smi.sm.checkCanDrop.Trigger(base.smi);
		}

		// Token: 0x06008E9D RID: 36509 RVA: 0x0036943C File Offset: 0x0036763C
		public void UpdateBlockedStatus()
		{
			int cell = Grid.PosToCell(base.smi.GetDropPosition());
			bool value = Grid.IsSolidCell(cell) || (base.def.blockedBySubstantialLiquid && Grid.IsSubstantialLiquid(cell, 0.35f));
			base.sm.isBlocked.Set(value, base.smi, false);
		}

		// Token: 0x06008E9E RID: 36510 RVA: 0x0036949C File Offset: 0x0036769C
		private bool IsFilteredElement(SimHashes element)
		{
			for (int num = 0; num != base.def.elementFilter.Length; num++)
			{
				if (base.def.elementFilter[num] == element)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008E9F RID: 36511 RVA: 0x003694D4 File Offset: 0x003676D4
		private bool AllowedToDrop(SimHashes element)
		{
			return base.def.elementFilter == null || base.def.elementFilter.Length == 0 || (!this.isInvertElementFilter && this.IsFilteredElement(element)) || (this.isInvertElementFilter && !this.IsFilteredElement(element));
		}

		// Token: 0x06008EA0 RID: 36512 RVA: 0x00369524 File Offset: 0x00367724
		public void Drop()
		{
			bool flag = false;
			Element element = null;
			for (int i = this.m_storage.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = this.m_storage.items[i];
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (this.AllowedToDrop(component.ElementID))
				{
					if (base.def.asOre)
					{
						this.m_storage.Drop(gameObject, true);
						gameObject.transform.SetPosition(this.GetDropPosition());
						element = component.Element;
						flag = true;
					}
					else
					{
						Dumpable component2 = gameObject.GetComponent<Dumpable>();
						if (!component2.IsNullOrDestroyed())
						{
							component2.Dump(this.GetDropPosition());
							element = component.Element;
							flag = true;
						}
					}
				}
			}
			AutoStorageDropper.DropperFxConfig dropperAnim = this.GetDropperAnim();
			if (flag && dropperAnim != null && GameClock.Instance.GetTime() > this.m_timeSinceLastDrop + base.def.cooldown)
			{
				this.m_timeSinceLastDrop = GameClock.Instance.GetTime();
				Vector3 vector = Grid.CellToPosCCC(Grid.PosToCell(this.GetDropPosition()), dropperAnim.layer);
				vector += ((this.m_rotatable != null) ? this.m_rotatable.GetRotatedOffset(base.def.fxOffset) : base.def.fxOffset);
				KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(dropperAnim.animFile, vector, null, false, dropperAnim.layer, false);
				kbatchedAnimController.destroyOnAnimComplete = false;
				kbatchedAnimController.FlipX = dropperAnim.flipX;
				kbatchedAnimController.FlipY = dropperAnim.flipY;
				if (dropperAnim.useElementTint)
				{
					kbatchedAnimController.TintColour = element.substance.colour;
				}
				kbatchedAnimController.Play(dropperAnim.animName, KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06008EA1 RID: 36513 RVA: 0x003696EC File Offset: 0x003678EC
		public AutoStorageDropper.DropperFxConfig GetDropperAnim()
		{
			CellOffset cellOffset = (this.m_rotatable != null) ? this.m_rotatable.GetRotatedCellOffset(base.def.dropOffset) : base.def.dropOffset;
			if (cellOffset.x < 0)
			{
				return base.def.leftFx;
			}
			if (cellOffset.x > 0)
			{
				return base.def.rightFx;
			}
			if (cellOffset.y < 0)
			{
				return base.def.downFx;
			}
			if (cellOffset.y > 0)
			{
				return base.def.upFx;
			}
			return base.def.neutralFx;
		}

		// Token: 0x06008EA2 RID: 36514 RVA: 0x0036978C File Offset: 0x0036798C
		public Vector3 GetDropPosition()
		{
			if (!(this.m_rotatable != null))
			{
				return base.transform.GetPosition() + base.def.dropOffset.ToVector3();
			}
			return base.transform.GetPosition() + this.m_rotatable.GetRotatedCellOffset(base.def.dropOffset).ToVector3();
		}

		// Token: 0x04006D96 RID: 28054
		[MyCmpGet]
		private Storage m_storage;

		// Token: 0x04006D97 RID: 28055
		[MyCmpGet]
		private Rotatable m_rotatable;

		// Token: 0x04006D99 RID: 28057
		private float m_timeSinceLastDrop;
	}
}
