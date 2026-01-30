using System;
using UnityEngine;

// Token: 0x020005BA RID: 1466
public class CreatureFallMonitor : GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>
{
	// Token: 0x060021AA RID: 8618 RVA: 0x000C39FC File Offset: 0x000C1BFC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded;
		this.grounded.ToggleBehaviour(GameTags.Creatures.Falling, (CreatureFallMonitor.Instance smi) => smi.ShouldFall(), null);
	}

	// Token: 0x0400139E RID: 5022
	public static float FLOOR_DISTANCE = -0.065f;

	// Token: 0x0400139F RID: 5023
	public GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.State grounded;

	// Token: 0x040013A0 RID: 5024
	public GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.State falling;

	// Token: 0x0200145E RID: 5214
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006E6A RID: 28266
		public bool canSwim;

		// Token: 0x04006E6B RID: 28267
		public bool checkHead = true;
	}

	// Token: 0x0200145F RID: 5215
	public new class Instance : GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.GameInstance
	{
		// Token: 0x06008FA9 RID: 36777 RVA: 0x0036C5F8 File Offset: 0x0036A7F8
		public Instance(IStateMachineTarget master, CreatureFallMonitor.Def def) : base(master, def)
		{
			this.largeCritter = (this.collider.size.y > 1f);
		}

		// Token: 0x06008FAA RID: 36778 RVA: 0x0036C62C File Offset: 0x0036A82C
		public void SnapToGround()
		{
			Vector3 vector = base.smi.transform.GetPosition();
			Vector3 b = this.navigator.NavGrid.GetNavTypeData(this.navigator.CurrentNavType).animControllerOffset;
			vector -= b;
			Vector3 position = Grid.CellToPosCBC(Grid.PosToCell(vector), Grid.SceneLayer.Creatures);
			position.x = vector.x;
			base.smi.transform.SetPosition(position);
			if (this.navigator.IsValidNavType(NavType.Floor))
			{
				this.navigator.SetCurrentNavType(NavType.Floor);
				return;
			}
			if (this.navigator.IsValidNavType(NavType.Hover))
			{
				this.navigator.SetCurrentNavType(NavType.Hover);
			}
		}

		// Token: 0x06008FAB RID: 36779 RVA: 0x0036C6DC File Offset: 0x0036A8DC
		public bool ShouldFall()
		{
			if (this.kprefabId.HasTag(GameTags.Stored))
			{
				return false;
			}
			Vector3 position = base.smi.transform.GetPosition();
			int num = Grid.PosToCell(position);
			if (Grid.IsValidCell(num) && Grid.Solid[num])
			{
				return false;
			}
			if (this.navigator.IsMoving())
			{
				return false;
			}
			if (this.CanSwimAtCurrentLocation())
			{
				return false;
			}
			if (this.navigator.CurrentNavType != NavType.Swim)
			{
				if (this.navigator.NavGrid.NavTable.IsValid(num, this.navigator.CurrentNavType))
				{
					return false;
				}
				if (this.navigator.CurrentNavType == NavType.Ceiling)
				{
					return true;
				}
				if (this.navigator.CurrentNavType == NavType.LeftWall)
				{
					return true;
				}
				if (this.navigator.CurrentNavType == NavType.RightWall)
				{
					return true;
				}
			}
			Vector3 vector = position;
			vector.y += CreatureFallMonitor.FLOOR_DISTANCE;
			int num2 = Grid.PosToCell(vector);
			return !Grid.IsValidCell(num2) || !Grid.Solid[num2];
		}

		// Token: 0x06008FAC RID: 36780 RVA: 0x0036C7E4 File Offset: 0x0036A9E4
		public bool CanSwimAtCurrentLocation()
		{
			if (base.def.canSwim)
			{
				Vector3 position = base.transform.GetPosition();
				float num = 1f;
				if (!base.def.checkHead)
				{
					num = 0.5f;
				}
				else if (this.largeCritter)
				{
					num = 0.25f;
				}
				position.y += this.collider.size.y * num;
				if (Grid.IsSubstantialLiquid(Grid.PosToCell(position), 0.35f))
				{
					if (!GameComps.Gravities.Has(base.gameObject))
					{
						return true;
					}
					if (GameComps.Gravities.GetData(GameComps.Gravities.GetHandle(base.gameObject)).velocity.magnitude < 2f)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04006E6C RID: 28268
		public string anim = "fall";

		// Token: 0x04006E6D RID: 28269
		[MyCmpReq]
		private KPrefabID kprefabId;

		// Token: 0x04006E6E RID: 28270
		[MyCmpReq]
		private Navigator navigator;

		// Token: 0x04006E6F RID: 28271
		[MyCmpReq]
		private KBoxCollider2D collider;

		// Token: 0x04006E70 RID: 28272
		private bool largeCritter;
	}
}
