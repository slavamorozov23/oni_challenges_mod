using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02000512 RID: 1298
public class ReactionMonitor : GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>
{
	// Token: 0x06001C12 RID: 7186 RVA: 0x0009B230 File Offset: 0x00099430
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.EventHandler(GameHashes.DestinationReached, delegate(ReactionMonitor.Instance smi)
		{
			smi.ClearLastReaction();
		}).EventHandler(GameHashes.NavigationFailed, delegate(ReactionMonitor.Instance smi)
		{
			smi.ClearLastReaction();
		});
		this.idle.Enter("ClearReactable", delegate(ReactionMonitor.Instance smi)
		{
			this.reactable.Set(null, smi, false);
		}).TagTransition(GameTags.Dead, this.dead, false);
		this.reacting.Enter("Reactable.Begin", delegate(ReactionMonitor.Instance smi)
		{
			this.reactable.Get(smi).Begin(smi.gameObject);
		}).Enter(delegate(ReactionMonitor.Instance smi)
		{
			smi.master.Trigger(-909573545, null);
		}).Enter("Reactable.AddChorePreventionTag", delegate(ReactionMonitor.Instance smi)
		{
			if (this.reactable.Get(smi).preventChoreInterruption)
			{
				smi.GetComponent<KPrefabID>().AddTag(GameTags.PreventChoreInterruption, false);
			}
		}).Update("Reactable.Update", delegate(ReactionMonitor.Instance smi, float dt)
		{
			this.reactable.Get(smi).Update(dt);
		}, UpdateRate.SIM_200ms, false).Exit(delegate(ReactionMonitor.Instance smi)
		{
			smi.master.Trigger(824899998, null);
		}).Exit("Reactable.End", delegate(ReactionMonitor.Instance smi)
		{
			this.reactable.Get(smi).End();
		}).Exit("Reactable.RemoveChorePreventionTag", delegate(ReactionMonitor.Instance smi)
		{
			if (this.reactable.Get(smi).preventChoreInterruption)
			{
				smi.GetComponent<KPrefabID>().RemoveTag(GameTags.PreventChoreInterruption);
			}
		}).EventTransition(GameHashes.NavigationFailed, this.idle, null).TagTransition(GameTags.Dying, this.dead, false).TagTransition(GameTags.Dead, this.dead, false);
		this.dead.DoNothing();
	}

	// Token: 0x06001C13 RID: 7187 RVA: 0x0009B3D1 File Offset: 0x000995D1
	private static bool ShouldReact(ReactionMonitor.Instance smi)
	{
		return smi.ImmediateReactable != null;
	}

	// Token: 0x04001095 RID: 4245
	public GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.State idle;

	// Token: 0x04001096 RID: 4246
	public GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.State reacting;

	// Token: 0x04001097 RID: 4247
	public GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.State dead;

	// Token: 0x04001098 RID: 4248
	public StateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.ObjectParameter<Reactable> reactable;

	// Token: 0x020013A1 RID: 5025
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006C00 RID: 27648
		public ObjectLayer ReactionLayer;
	}

	// Token: 0x020013A2 RID: 5026
	public new class Instance : GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.GameInstance
	{
		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x06008C90 RID: 35984 RVA: 0x0036202F File Offset: 0x0036022F
		// (set) Token: 0x06008C91 RID: 35985 RVA: 0x00362037 File Offset: 0x00360237
		public Reactable ImmediateReactable { get; private set; }

		// Token: 0x06008C92 RID: 35986 RVA: 0x00362040 File Offset: 0x00360240
		public Instance(IStateMachineTarget master, ReactionMonitor.Def def) : base(master, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
			this.lastReactTimes = new Dictionary<HashedString, float>();
			this.oneshotReactables = new List<Reactable>();
		}

		// Token: 0x06008C93 RID: 35987 RVA: 0x00362077 File Offset: 0x00360277
		public bool CanReact(Emote e)
		{
			return this.animController != null && e.IsValidForController(this.animController);
		}

		// Token: 0x06008C94 RID: 35988 RVA: 0x00362098 File Offset: 0x00360298
		public bool TryReact(Reactable reactable, float clockTime, Navigator.ActiveTransition transition = null)
		{
			if (reactable == null)
			{
				return false;
			}
			float num;
			if ((this.lastReactTimes.TryGetValue(reactable.id, out num) && num == this.lastReaction) || clockTime - num < reactable.localCooldown)
			{
				return false;
			}
			if (!reactable.CanBegin(base.gameObject, transition))
			{
				return false;
			}
			this.lastReactTimes[reactable.id] = clockTime;
			base.sm.reactable.Set(reactable, base.smi, false);
			base.smi.GoTo(base.sm.reacting);
			return true;
		}

		// Token: 0x06008C95 RID: 35989 RVA: 0x00362128 File Offset: 0x00360328
		public void PollForReactables(Navigator.ActiveTransition transition)
		{
			if (this.IsReacting())
			{
				return;
			}
			for (int i = this.oneshotReactables.Count - 1; i >= 0; i--)
			{
				Reactable reactable = this.oneshotReactables[i];
				if (reactable.IsExpired())
				{
					reactable.Cleanup();
					this.oneshotReactables.RemoveAt(i);
				}
			}
			Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(base.smi.gameObject));
			ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(int)base.def.ReactionLayer];
			ListPool<ScenePartitionerEntry, ReactionMonitor>.PooledList pooledList = ListPool<ScenePartitionerEntry, ReactionMonitor>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(vector2I.x, vector2I.y, 1, 1, layer, pooledList);
			float num = float.NaN;
			float time = GameClock.Instance.GetTime();
			for (int j = 0; j < pooledList.Count; j++)
			{
				Reactable reactable2 = pooledList[j].obj as Reactable;
				if (this.TryReact(reactable2, time, transition))
				{
					num = time;
					break;
				}
			}
			this.lastReaction = num;
			pooledList.Recycle();
		}

		// Token: 0x06008C96 RID: 35990 RVA: 0x0036222D File Offset: 0x0036042D
		public void ClearLastReaction()
		{
			this.lastReaction = float.NaN;
		}

		// Token: 0x06008C97 RID: 35991 RVA: 0x0036223C File Offset: 0x0036043C
		public void StopReaction()
		{
			for (int i = this.oneshotReactables.Count - 1; i >= 0; i--)
			{
				if (base.sm.reactable.Get(base.smi) == this.oneshotReactables[i])
				{
					this.oneshotReactables[i].Cleanup();
					this.oneshotReactables.RemoveAt(i);
					break;
				}
			}
			base.smi.GoTo(base.sm.idle);
		}

		// Token: 0x06008C98 RID: 35992 RVA: 0x003622BA File Offset: 0x003604BA
		public bool IsReacting()
		{
			return base.smi.IsInsideState(base.sm.reacting);
		}

		// Token: 0x06008C99 RID: 35993 RVA: 0x003622D4 File Offset: 0x003604D4
		public SelfEmoteReactable AddSelfEmoteReactable(GameObject target, HashedString reactionId, Emote emote, bool isOneShot, ChoreType choreType, float globalCooldown = 0f, float localCooldown = 20f, float lifeSpan = float.NegativeInfinity, float maxInitialDelay = 0f, List<Reactable.ReactablePrecondition> emotePreconditions = null)
		{
			if (!this.CanReact(emote))
			{
				return null;
			}
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(target, reactionId, choreType, globalCooldown, localCooldown, lifeSpan, maxInitialDelay);
			selfEmoteReactable.SetEmote(emote);
			int num = 0;
			while (emotePreconditions != null && num < emotePreconditions.Count)
			{
				selfEmoteReactable.AddPrecondition(emotePreconditions[num]);
				num++;
			}
			if (isOneShot)
			{
				this.AddOneshotReactable(selfEmoteReactable);
			}
			return selfEmoteReactable;
		}

		// Token: 0x06008C9A RID: 35994 RVA: 0x00362338 File Offset: 0x00360538
		public SelfEmoteReactable AddSelfEmoteReactable(GameObject target, string reactionId, string emoteAnim, bool isOneShot, ChoreType choreType, float globalCooldown = 0f, float localCooldown = 20f, float maxTriggerTime = float.NegativeInfinity, float maxInitialDelay = 0f, List<Reactable.ReactablePrecondition> emotePreconditions = null)
		{
			Emote emote = new Emote(null, reactionId, new EmoteStep[]
			{
				new EmoteStep
				{
					anim = "react"
				}
			}, emoteAnim);
			return this.AddSelfEmoteReactable(target, reactionId, emote, isOneShot, choreType, globalCooldown, localCooldown, maxTriggerTime, maxInitialDelay, emotePreconditions);
		}

		// Token: 0x06008C9B RID: 35995 RVA: 0x00362388 File Offset: 0x00360588
		public void AddOneshotReactable(SelfEmoteReactable reactable)
		{
			if (reactable == null)
			{
				return;
			}
			this.oneshotReactables.Add(reactable);
		}

		// Token: 0x06008C9C RID: 35996 RVA: 0x0036239C File Offset: 0x0036059C
		public void CancelOneShotReactable(SelfEmoteReactable cancel_target)
		{
			for (int i = this.oneshotReactables.Count - 1; i >= 0; i--)
			{
				Reactable reactable = this.oneshotReactables[i];
				if (cancel_target == reactable)
				{
					reactable.Cleanup();
					this.oneshotReactables.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06008C9D RID: 35997 RVA: 0x003623E8 File Offset: 0x003605E8
		public void CancelOneShotReactables(Emote reactionEmote)
		{
			for (int i = this.oneshotReactables.Count - 1; i >= 0; i--)
			{
				EmoteReactable emoteReactable = this.oneshotReactables[i] as EmoteReactable;
				if (emoteReactable != null && emoteReactable.emote == reactionEmote)
				{
					emoteReactable.Cleanup();
					this.oneshotReactables.RemoveAt(i);
				}
			}
		}

		// Token: 0x04006C02 RID: 27650
		private KBatchedAnimController animController;

		// Token: 0x04006C03 RID: 27651
		private float lastReaction = float.NaN;

		// Token: 0x04006C04 RID: 27652
		private Dictionary<HashedString, float> lastReactTimes;

		// Token: 0x04006C05 RID: 27653
		private List<Reactable> oneshotReactables;
	}
}
