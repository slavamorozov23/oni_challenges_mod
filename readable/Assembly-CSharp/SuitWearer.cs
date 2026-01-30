using System;
using System.Collections.Generic;

// Token: 0x02000A4F RID: 2639
public class SuitWearer : GameStateMachine<SuitWearer, SuitWearer.Instance>
{
	// Token: 0x06004CDB RID: 19675 RVA: 0x001BF3F4 File Offset: 0x001BD5F4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.PathAdvanced, delegate(SuitWearer.Instance smi, object data)
		{
			smi.OnPathAdvanced(data);
		}).EventHandler(GameHashes.NavigationFailed, delegate(SuitWearer.Instance smi, object data)
		{
			smi.UnreserveSuits();
		}).EventHandler(GameHashes.Died, delegate(SuitWearer.Instance smi, object data)
		{
			smi.UnreserveSuits();
		}).DoNothing();
		this.suit.DoNothing();
		this.nosuit.DoNothing();
	}

	// Token: 0x04003335 RID: 13109
	public GameStateMachine<SuitWearer, SuitWearer.Instance, IStateMachineTarget, object>.State suit;

	// Token: 0x04003336 RID: 13110
	public GameStateMachine<SuitWearer, SuitWearer.Instance, IStateMachineTarget, object>.State nosuit;

	// Token: 0x02001B51 RID: 6993
	public new class Instance : GameStateMachine<SuitWearer, SuitWearer.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A95C RID: 43356 RVA: 0x003C10FC File Offset: 0x003BF2FC
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.navigator = master.GetComponent<Navigator>();
			this.navigator.SetFlags(PathFinder.PotentialPath.Flags.PerformSuitChecks);
			this.prefabInstanceID = this.navigator.GetComponent<KPrefabID>().InstanceID;
		}

		// Token: 0x0600A95D RID: 43357 RVA: 0x003C1154 File Offset: 0x003BF354
		public void OnPathAdvanced(object data)
		{
			if (this.navigator.CurrentNavType == NavType.Hover && (this.navigator.flags & PathFinder.PotentialPath.Flags.HasJetPack) <= PathFinder.PotentialPath.Flags.None)
			{
				this.navigator.SetCurrentNavType(NavType.Floor);
			}
			this.UnreserveSuits();
			this.ReserveSuits();
		}

		// Token: 0x0600A95E RID: 43358 RVA: 0x003C1190 File Offset: 0x003BF390
		public void ReserveSuits()
		{
			PathFinder.Path path = this.navigator.path;
			if (path.nodes == null)
			{
				return;
			}
			bool flag = (this.navigator.flags & PathFinder.PotentialPath.Flags.HasAtmoSuit) > PathFinder.PotentialPath.Flags.None;
			bool flag2 = (this.navigator.flags & PathFinder.PotentialPath.Flags.HasJetPack) > PathFinder.PotentialPath.Flags.None;
			bool flag3 = (this.navigator.flags & PathFinder.PotentialPath.Flags.HasOxygenMask) > PathFinder.PotentialPath.Flags.None;
			bool flag4 = (this.navigator.flags & PathFinder.PotentialPath.Flags.HasLeadSuit) > PathFinder.PotentialPath.Flags.None;
			for (int i = 0; i < path.nodes.Count - 1; i++)
			{
				int cell = path.nodes[i].cell;
				Grid.SuitMarker.Flags flags = (Grid.SuitMarker.Flags)0;
				PathFinder.PotentialPath.Flags flags2 = PathFinder.PotentialPath.Flags.None;
				if (Grid.TryGetSuitMarkerFlags(cell, out flags, out flags2))
				{
					bool flag5 = (flags2 & PathFinder.PotentialPath.Flags.HasAtmoSuit) > PathFinder.PotentialPath.Flags.None;
					bool flag6 = (flags2 & PathFinder.PotentialPath.Flags.HasJetPack) > PathFinder.PotentialPath.Flags.None;
					bool flag7 = (flags2 & PathFinder.PotentialPath.Flags.HasOxygenMask) > PathFinder.PotentialPath.Flags.None;
					bool flag8 = (flags2 & PathFinder.PotentialPath.Flags.HasLeadSuit) > PathFinder.PotentialPath.Flags.None;
					bool flag9 = flag2 || flag || flag3 || flag4;
					bool flag10 = flag5 == flag && flag6 == flag2 && flag7 == flag3 && flag8 == flag4;
					bool flag11 = SuitMarker.DoesTraversalDirectionRequireSuit(cell, path.nodes[i + 1].cell, flags);
					if (flag11 && !flag9)
					{
						if (Grid.ReserveSuit(cell, this.prefabInstanceID, true))
						{
							this.suitReservations.Add(cell);
							if (flag5)
							{
								flag = true;
							}
							if (flag6)
							{
								flag2 = true;
							}
							if (flag7)
							{
								flag3 = true;
							}
							if (flag8)
							{
								flag4 = true;
							}
						}
					}
					else if (!flag11 && flag10 && Grid.HasEmptyLocker(cell, this.prefabInstanceID) && Grid.ReserveEmptyLocker(cell, this.prefabInstanceID, true))
					{
						this.emptyLockerReservations.Add(cell);
						if (flag5)
						{
							flag = false;
						}
						if (flag6)
						{
							flag2 = false;
						}
						if (flag7)
						{
							flag3 = false;
						}
						if (flag8)
						{
							flag4 = false;
						}
					}
				}
			}
		}

		// Token: 0x0600A95F RID: 43359 RVA: 0x003C133C File Offset: 0x003BF53C
		public void UnreserveSuits()
		{
			foreach (int num in this.suitReservations)
			{
				if (Grid.HasSuitMarker[num])
				{
					Grid.ReserveSuit(num, this.prefabInstanceID, false);
				}
			}
			this.suitReservations.Clear();
			foreach (int num2 in this.emptyLockerReservations)
			{
				if (Grid.HasSuitMarker[num2])
				{
					Grid.ReserveEmptyLocker(num2, this.prefabInstanceID, false);
				}
			}
			this.emptyLockerReservations.Clear();
		}

		// Token: 0x0600A960 RID: 43360 RVA: 0x003C1410 File Offset: 0x003BF610
		protected override void OnCleanUp()
		{
			this.UnreserveSuits();
		}

		// Token: 0x0400847A RID: 33914
		private List<int> suitReservations = new List<int>();

		// Token: 0x0400847B RID: 33915
		private List<int> emptyLockerReservations = new List<int>();

		// Token: 0x0400847C RID: 33916
		private Navigator navigator;

		// Token: 0x0400847D RID: 33917
		private int prefabInstanceID;
	}
}
