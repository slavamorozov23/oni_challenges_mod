using System;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x02000897 RID: 2199
public class DiggerMonitor : GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>
{
	// Token: 0x06003C86 RID: 15494 RVA: 0x00152790 File Offset: 0x00150990
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		this.loop.EventTransition(GameHashes.BeginMeteorBombardment, (DiggerMonitor.Instance smi) => Game.Instance, this.dig, (DiggerMonitor.Instance smi) => smi.CanTunnel());
		this.dig.ToggleBehaviour(GameTags.Creatures.Tunnel, (DiggerMonitor.Instance smi) => true, null).GoTo(this.loop);
	}

	// Token: 0x0400254F RID: 9551
	public GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.State loop;

	// Token: 0x04002550 RID: 9552
	public GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.State dig;

	// Token: 0x02001875 RID: 6261
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06009EEF RID: 40687 RVA: 0x003A46B1 File Offset: 0x003A28B1
		// (set) Token: 0x06009EF0 RID: 40688 RVA: 0x003A46B9 File Offset: 0x003A28B9
		public int depthToDig { get; set; }
	}

	// Token: 0x02001876 RID: 6262
	public new class Instance : GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.GameInstance
	{
		// Token: 0x06009EF2 RID: 40690 RVA: 0x003A46CC File Offset: 0x003A28CC
		public Instance(IStateMachineTarget master, DiggerMonitor.Def def) : base(master, def)
		{
			global::World instance = global::World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
			this.OnDestinationReachedDelegate = new Action<object>(this.OnDestinationReached);
			master.Subscribe(387220196, this.OnDestinationReachedDelegate);
			master.Subscribe(-766531887, this.OnDestinationReachedDelegate);
		}

		// Token: 0x06009EF3 RID: 40691 RVA: 0x003A4744 File Offset: 0x003A2944
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			global::World instance = global::World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
			base.master.Unsubscribe(387220196, this.OnDestinationReachedDelegate);
			base.master.Unsubscribe(-766531887, this.OnDestinationReachedDelegate);
		}

		// Token: 0x06009EF4 RID: 40692 RVA: 0x003A47A9 File Offset: 0x003A29A9
		private void OnDestinationReached(object data)
		{
			this.CheckInSolid();
		}

		// Token: 0x06009EF5 RID: 40693 RVA: 0x003A47B4 File Offset: 0x003A29B4
		private void CheckInSolid()
		{
			Navigator component = base.gameObject.GetComponent<Navigator>();
			if (component == null)
			{
				return;
			}
			int cell = Grid.PosToCell(base.gameObject);
			if (component.CurrentNavType != NavType.Solid && Grid.IsSolidCell(cell))
			{
				component.SetCurrentNavType(NavType.Solid);
				return;
			}
			if (component.CurrentNavType == NavType.Solid && !Grid.IsSolidCell(cell))
			{
				component.SetCurrentNavType(NavType.Floor);
				base.gameObject.AddTag(GameTags.Creatures.Falling);
			}
		}

		// Token: 0x06009EF6 RID: 40694 RVA: 0x003A4827 File Offset: 0x003A2A27
		private void OnSolidChanged(int cell)
		{
			this.CheckInSolid();
		}

		// Token: 0x06009EF7 RID: 40695 RVA: 0x003A4830 File Offset: 0x003A2A30
		public bool CanTunnel()
		{
			int num = Grid.PosToCell(this);
			if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(num) == SubWorld.ZoneType.Space)
			{
				int num2 = num;
				while (Grid.IsValidCell(num2) && !Grid.Solid[num2])
				{
					num2 = Grid.CellAbove(num2);
				}
				if (!Grid.IsValidCell(num2))
				{
					return this.FoundValidDigCell();
				}
			}
			return false;
		}

		// Token: 0x06009EF8 RID: 40696 RVA: 0x003A4888 File Offset: 0x003A2A88
		private bool FoundValidDigCell()
		{
			int num = base.smi.def.depthToDig;
			int num2 = Grid.PosToCell(base.smi.master.gameObject);
			this.lastDigCell = num2;
			int cell = Grid.CellBelow(num2);
			while (this.IsValidDigCell(cell, null) && num > 0)
			{
				cell = Grid.CellBelow(cell);
				num--;
			}
			if (num > 0)
			{
				cell = GameUtil.FloodFillFind<object>(new Func<int, object, bool>(this.IsValidDigCell), null, num2, base.smi.def.depthToDig, false, true);
			}
			this.lastDigCell = cell;
			return this.lastDigCell != -1;
		}

		// Token: 0x06009EF9 RID: 40697 RVA: 0x003A4924 File Offset: 0x003A2B24
		private bool IsValidDigCell(int cell, object arg = null)
		{
			if (Grid.IsValidCell(cell) && Grid.Solid[cell])
			{
				if (!Grid.HasDoor[cell] && !Grid.Foundation[cell])
				{
					ushort index = Grid.ElementIdx[cell];
					Element element = ElementLoader.elements[(int)index];
					return Grid.Element[cell].hardness < 150 && !element.HasTag(GameTags.RefinedMetal);
				}
				GameObject gameObject = Grid.Objects[cell, 1];
				if (gameObject != null)
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					return Grid.Element[cell].hardness < 150 && !component.Element.HasTag(GameTags.RefinedMetal);
				}
			}
			return false;
		}

		// Token: 0x04007AF4 RID: 31476
		[Serialize]
		public int lastDigCell = -1;

		// Token: 0x04007AF5 RID: 31477
		private Action<object> OnDestinationReachedDelegate;
	}
}
