using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020004EA RID: 1258
public class StickerBomber : GameStateMachine<StickerBomber, StickerBomber.Instance>
{
	// Token: 0x06001B2B RID: 6955 RVA: 0x0009555C File Offset: 0x0009375C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false).Exit(delegate(StickerBomber.Instance smi)
		{
			smi.nextStickerBomb = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.STICKER_BOMBER.TIME_PER_STICKER_BOMB;
		});
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ToggleStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_StickerBombing, null);
		this.overjoyed.idle.Transition(this.overjoyed.place_stickers, (StickerBomber.Instance smi) => GameClock.Instance.GetTime() >= smi.nextStickerBomb, UpdateRate.SIM_200ms);
		this.overjoyed.place_stickers.Exit(delegate(StickerBomber.Instance smi)
		{
			smi.nextStickerBomb = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.STICKER_BOMBER.TIME_PER_STICKER_BOMB;
		}).ToggleReactable((StickerBomber.Instance smi) => smi.CreateReactable()).OnSignal(this.doneStickerBomb, this.overjoyed.idle);
	}

	// Token: 0x04000FA8 RID: 4008
	public StateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.Signal doneStickerBomb;

	// Token: 0x04000FA9 RID: 4009
	public GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04000FAA RID: 4010
	public StickerBomber.OverjoyedStates overjoyed;

	// Token: 0x0200137C RID: 4988
	public class OverjoyedStates : GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006B51 RID: 27473
		public GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04006B52 RID: 27474
		public GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State place_stickers;
	}

	// Token: 0x0200137D RID: 4989
	public new class Instance : GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008C14 RID: 35860 RVA: 0x003605EF File Offset: 0x0035E7EF
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06008C15 RID: 35861 RVA: 0x003605F8 File Offset: 0x0035E7F8
		public Reactable CreateReactable()
		{
			return new StickerBomber.Instance.StickerBombReactable(base.master.gameObject, base.smi);
		}

		// Token: 0x04006B53 RID: 27475
		[Serialize]
		public float nextStickerBomb;

		// Token: 0x020027FC RID: 10236
		private class StickerBombReactable : Reactable
		{
			// Token: 0x0600CAC6 RID: 51910 RVA: 0x0042BBCC File Offset: 0x00429DCC
			public StickerBombReactable(GameObject gameObject, StickerBomber.Instance stickerBomber) : base(gameObject, "StickerBombReactable", Db.Get().ChoreTypes.Build, 2, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
			{
				this.preventChoreInterruption = true;
				this.stickerBomber = stickerBomber;
			}

			// Token: 0x0600CAC7 RID: 51911 RVA: 0x0042BCAC File Offset: 0x00429EAC
			public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
			{
				if (this.reactor != null)
				{
					return false;
				}
				if (new_reactor == null)
				{
					return false;
				}
				if (this.gameObject != new_reactor)
				{
					return false;
				}
				Navigator component = new_reactor.GetComponent<Navigator>();
				return !(component == null) && component.CurrentNavType != NavType.Tube && component.CurrentNavType != NavType.Ladder && component.CurrentNavType != NavType.Pole;
			}

			// Token: 0x0600CAC8 RID: 51912 RVA: 0x0042BD14 File Offset: 0x00429F14
			protected override void InternalBegin()
			{
				this.stickersToPlace = UnityEngine.Random.Range(4, 6);
				this.STICKER_PLACE_TIMER = this.TIME_PER_STICKER_PLACED;
				this.placementCell = this.FindPlacementCell();
				if (this.placementCell == 0)
				{
					base.End();
					return;
				}
				this.kbac = this.reactor.GetComponent<KBatchedAnimController>();
				this.kbac.AddAnimOverrides(this.animset, 0f);
				this.kbac.Play(this.pre_anim, KAnim.PlayMode.Once, 1f, 0f);
				this.kbac.Queue(this.loop_anim, KAnim.PlayMode.Loop, 1f, 0f);
			}

			// Token: 0x0600CAC9 RID: 51913 RVA: 0x0042BDB4 File Offset: 0x00429FB4
			public override void Update(float dt)
			{
				this.STICKER_PLACE_TIMER -= dt;
				if (this.STICKER_PLACE_TIMER <= 0f)
				{
					this.PlaceSticker();
					this.STICKER_PLACE_TIMER = this.TIME_PER_STICKER_PLACED;
				}
				if (this.stickersPlaced >= this.stickersToPlace)
				{
					this.kbac.Play(this.pst_anim, KAnim.PlayMode.Once, 1f, 0f);
					base.End();
				}
			}

			// Token: 0x0600CACA RID: 51914 RVA: 0x0042BE20 File Offset: 0x0042A020
			protected override void InternalEnd()
			{
				if (this.kbac != null)
				{
					this.kbac.RemoveAnimOverrides(this.animset);
					this.kbac = null;
				}
				this.stickerBomber.sm.doneStickerBomb.Trigger(this.stickerBomber);
				this.stickersPlaced = 0;
			}

			// Token: 0x0600CACB RID: 51915 RVA: 0x0042BE78 File Offset: 0x0042A078
			private int FindPlacementCell()
			{
				int cell = Grid.PosToCell(this.reactor.transform.GetPosition() + Vector3.up);
				HashSetPool<int, PathFinder>.PooledHashSet pooledHashSet = HashSetPool<int, PathFinder>.Allocate();
				ListPool<int, PathFinder>.PooledList pooledList = ListPool<int, PathFinder>.Allocate();
				QueuePool<GameUtil.FloodFillInfo, Comet>.PooledQueue pooledQueue = QueuePool<GameUtil.FloodFillInfo, Comet>.Allocate();
				pooledQueue.Enqueue(new GameUtil.FloodFillInfo
				{
					cell = cell,
					depth = 0
				});
				GameUtil.FloodFillConditional(pooledQueue, this.canPlaceStickerCb, pooledHashSet, pooledList, 2);
				pooledHashSet.Recycle();
				pooledQueue.Recycle();
				int result = (pooledList.Count > 0) ? pooledList.GetRandom<int>() : 0;
				pooledList.Recycle();
				return result;
			}

			// Token: 0x0600CACC RID: 51916 RVA: 0x0042BF08 File Offset: 0x0042A108
			private void PlaceSticker()
			{
				this.stickersPlaced++;
				Vector3 a = Grid.CellToPos(this.placementCell);
				int i = 10;
				while (i > 0)
				{
					i--;
					Vector3 position = a + new Vector3(UnityEngine.Random.Range(-this.tile_random_range, this.tile_random_range), UnityEngine.Random.Range(-this.tile_random_range, this.tile_random_range), -2.5f);
					if (StickerBomb.CanPlaceSticker(StickerBomb.BuildCellOffsets(position)))
					{
						GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("StickerBomb".ToTag()), position, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-this.tile_random_rotation, this.tile_random_rotation)), null, null, true, 0);
						StickerBomb component = gameObject.GetComponent<StickerBomb>();
						string stickerType = this.reactor.GetComponent<MinionIdentity>().stickerType;
						component.SetStickerType(stickerType);
						gameObject.SetActive(true);
						i = 0;
					}
				}
			}

			// Token: 0x0600CACD RID: 51917 RVA: 0x0042BFE3 File Offset: 0x0042A1E3
			protected override void InternalCleanup()
			{
			}

			// Token: 0x0400B13E RID: 45374
			private int stickersToPlace;

			// Token: 0x0400B13F RID: 45375
			private int stickersPlaced;

			// Token: 0x0400B140 RID: 45376
			private int placementCell;

			// Token: 0x0400B141 RID: 45377
			private float tile_random_range = 1f;

			// Token: 0x0400B142 RID: 45378
			private float tile_random_rotation = 90f;

			// Token: 0x0400B143 RID: 45379
			private float TIME_PER_STICKER_PLACED = 0.66f;

			// Token: 0x0400B144 RID: 45380
			private float STICKER_PLACE_TIMER;

			// Token: 0x0400B145 RID: 45381
			private KBatchedAnimController kbac;

			// Token: 0x0400B146 RID: 45382
			private KAnimFile animset = Assets.GetAnim("anim_stickers_kanim");

			// Token: 0x0400B147 RID: 45383
			private HashedString pre_anim = "working_pre";

			// Token: 0x0400B148 RID: 45384
			private HashedString loop_anim = "working_loop";

			// Token: 0x0400B149 RID: 45385
			private HashedString pst_anim = "working_pst";

			// Token: 0x0400B14A RID: 45386
			private StickerBomber.Instance stickerBomber;

			// Token: 0x0400B14B RID: 45387
			private Func<int, bool> canPlaceStickerCb = (int cell) => !Grid.Solid[cell] && (!Grid.IsValidCell(Grid.CellLeft(cell)) || !Grid.Solid[Grid.CellLeft(cell)]) && (!Grid.IsValidCell(Grid.CellRight(cell)) || !Grid.Solid[Grid.CellRight(cell)]) && (!Grid.IsValidCell(Grid.OffsetCell(cell, 0, 1)) || !Grid.Solid[Grid.OffsetCell(cell, 0, 1)]) && (!Grid.IsValidCell(Grid.OffsetCell(cell, 0, -1)) || !Grid.Solid[Grid.OffsetCell(cell, 0, -1)]) && !Grid.IsCellOpenToSpace(cell);
		}
	}
}
