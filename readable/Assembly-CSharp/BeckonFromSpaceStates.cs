using System;
using UnityEngine;

// Token: 0x020000D9 RID: 217
internal class BeckonFromSpaceStates : GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>
{
	// Token: 0x060003DC RID: 988 RVA: 0x00020A3C File Offset: 0x0001EC3C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.beckoning;
		this.beckoning.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Beckoning, null).Enter(new StateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State.Callback(BeckonFromSpaceStates.ChooseSong)).DefaultState(this.beckoning.pre);
		this.beckoning.pre.PlayAnim(new Func<BeckonFromSpaceStates.Instance, string>(BeckonFromSpaceStates.GetSongAnimPre), KAnim.PlayMode.Once).OnAnimQueueComplete(this.beckoning.loop);
		this.beckoning.loop.PlayAnim(new Func<BeckonFromSpaceStates.Instance, string>(BeckonFromSpaceStates.GetSongAnimLoop), KAnim.PlayMode.Once).Enter(new StateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State.Callback(BeckonFromSpaceStates.MooEchoFX)).OnAnimQueueComplete(this.beckoning.pst);
		this.beckoning.pst.PlayAnim(new Func<BeckonFromSpaceStates.Instance, string>(BeckonFromSpaceStates.GetSongAnimPst), KAnim.PlayMode.Once).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).Enter(new StateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State.Callback(BeckonFromSpaceStates.DoBeckon)).Enter(new StateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State.Callback(BeckonFromSpaceStates.MooCheer)).BehaviourComplete(GameTags.Creatures.WantsToBeckon, false);
	}

	// Token: 0x060003DD RID: 989 RVA: 0x00020B63 File Offset: 0x0001ED63
	public static string GetSongAnimPre(BeckonFromSpaceStates.Instance smi)
	{
		return smi.ChosenSong.singAnimPre;
	}

	// Token: 0x060003DE RID: 990 RVA: 0x00020B70 File Offset: 0x0001ED70
	public static string GetSongAnimLoop(BeckonFromSpaceStates.Instance smi)
	{
		return smi.ChosenSong.singAnimLoop;
	}

	// Token: 0x060003DF RID: 991 RVA: 0x00020B7D File Offset: 0x0001ED7D
	public static string GetSongAnimPst(BeckonFromSpaceStates.Instance smi)
	{
		return smi.ChosenSong.singAnimPst;
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x00020B8A File Offset: 0x0001ED8A
	private static void ChooseSong(BeckonFromSpaceStates.Instance smi)
	{
		smi.ChooseSong();
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x00020B94 File Offset: 0x0001ED94
	private static void MooEchoFX(BeckonFromSpaceStates.Instance smi)
	{
		KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("moo_call_fx_kanim", smi.master.transform.position, null, false, Grid.SceneLayer.Front, false);
		kbatchedAnimController.destroyOnAnimComplete = true;
		kbatchedAnimController.Play("moo_call", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x00020BE4 File Offset: 0x0001EDE4
	private static Util.IterationInstruction mooCheerVisitor(object obj, BeckonFromSpaceStates.Instance smi)
	{
		KPrefabID kprefabID = (obj as Pickupable).KPrefabID;
		if (kprefabID.gameObject == smi.gameObject)
		{
			return Util.IterationInstruction.Continue;
		}
		if (kprefabID.HasTag("Moo") && kprefabID.GetSMI<AnimInterruptMonitor.Instance>() != null)
		{
			kprefabID.GetSMI<AnimInterruptMonitor.Instance>().PlayAnimSequence(smi.def.choirAnims);
		}
		return Util.IterationInstruction.Continue;
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x00020C44 File Offset: 0x0001EE44
	private static void MooCheer(BeckonFromSpaceStates.Instance smi)
	{
		Vector3 position = smi.transform.GetPosition();
		Extents extents = new Extents((int)position.x, (int)position.y, 15);
		GameScenePartitioner.Instance.VisitEntries<BeckonFromSpaceStates.Instance>(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.pickupablesLayer, new Func<object, BeckonFromSpaceStates.Instance, Util.IterationInstruction>(BeckonFromSpaceStates.mooCheerVisitor), smi);
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x00020CB0 File Offset: 0x0001EEB0
	private static void DoBeckon(BeckonFromSpaceStates.Instance smi)
	{
		Db.Get().Amounts.Beckoning.Lookup(smi.gameObject).value = 0f;
		WorldContainer myWorld = smi.GetMyWorld();
		Vector3 position = smi.transform.position;
		float num = (float)(myWorld.Height + myWorld.WorldOffset.y - 1);
		float layerZ = Grid.GetLayerZ(smi.def.sceneLayer);
		float num2 = (num - position.y) * Mathf.Tan(0.2617994f);
		float num3 = position.x + (float)UnityEngine.Random.Range(-5, 5);
		float num4 = num3 - num2;
		float num5 = num3 + num2;
		float num6 = position.x;
		bool customInitialFlip = false;
		if (num4 > (float)myWorld.WorldOffset.x && num4 < (float)(myWorld.WorldOffset.x + myWorld.Width))
		{
			num6 = num4;
			customInitialFlip = false;
		}
		else if (num4 > (float)myWorld.WorldOffset.x && num4 < (float)(myWorld.WorldOffset.x + myWorld.Width))
		{
			num6 = num5;
			customInitialFlip = true;
		}
		DebugUtil.DevAssert(myWorld.ContainsPoint(new Vector2(num6, num)), "Gassy Moo spawned outside world bounds", null);
		Vector3 position2 = new Vector3(num6, num, layerZ);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(smi.ChosenSong.meteorID), position2, Quaternion.identity, null, null, true, 0);
		GassyMooComet component = gameObject.GetComponent<GassyMooComet>();
		if (component != null)
		{
			component.spawnWithOffset = true;
			if (num6 != position.x)
			{
				component.SetCustomInitialFlip(customInitialFlip);
			}
		}
		gameObject.SetActive(true);
	}

	// Token: 0x040002FA RID: 762
	public BeckonFromSpaceStates.BeckoningState beckoning;

	// Token: 0x040002FB RID: 763
	public GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State behaviourcomplete;

	// Token: 0x020010D9 RID: 4313
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006366 RID: 25446
		public Grid.SceneLayer sceneLayer;

		// Token: 0x04006367 RID: 25447
		public HashedString[] choirAnims = new HashedString[]
		{
			"reply_loop"
		};
	}

	// Token: 0x020010DA RID: 4314
	public new class Instance : GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.GameInstance
	{
		// Token: 0x0600832A RID: 33578 RVA: 0x00342F93 File Offset: 0x00341193
		public Instance(Chore<BeckonFromSpaceStates.Instance> chore, BeckonFromSpaceStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToBeckon);
			this.monitor = base.gameObject.GetSMI<BeckoningMonitor.Instance>();
		}

		// Token: 0x0600832B RID: 33579 RVA: 0x00342FC8 File Offset: 0x003411C8
		public void ChooseSong()
		{
			float num = UnityEngine.Random.value;
			BeckoningMonitor.SongChance chosenSong = null;
			foreach (BeckoningMonitor.SongChance songChance in this.monitor.songChances)
			{
				num -= songChance.weight;
				if (num <= 0f)
				{
					chosenSong = songChance;
					break;
				}
			}
			this.ChosenSong = chosenSong;
		}

		// Token: 0x04006368 RID: 25448
		public BeckoningMonitor.SongChance ChosenSong;

		// Token: 0x04006369 RID: 25449
		private BeckoningMonitor.Instance monitor;
	}

	// Token: 0x020010DB RID: 4315
	public class BeckoningState : GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State
	{
		// Token: 0x0400636A RID: 25450
		public GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State pre;

		// Token: 0x0400636B RID: 25451
		public GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State loop;

		// Token: 0x0400636C RID: 25452
		public GameStateMachine<BeckonFromSpaceStates, BeckonFromSpaceStates.Instance, IStateMachineTarget, BeckonFromSpaceStates.Def>.State pst;
	}
}
