using System;

// Token: 0x020000E2 RID: 226
public class PlayAnimsStates : GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>
{
	// Token: 0x06000418 RID: 1048 RVA: 0x0002225C File Offset: 0x0002045C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.animating;
		GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.State root = this.root;
		string name = "Unused";
		string tooltip = "Unused";
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, (string str, PlayAnimsStates.Instance smi) => smi.def.statusItemName, (string str, PlayAnimsStates.Instance smi) => smi.def.statusItemTooltip, main);
		this.animating.Enter("PlayAnims", delegate(PlayAnimsStates.Instance smi)
		{
			smi.PlayAnims();
		}).OnAnimQueueComplete(this.done).EventHandler(GameHashes.TagsChanged, delegate(PlayAnimsStates.Instance smi, object obj)
		{
			smi.HandleTagsChanged(obj);
		});
		this.done.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete((PlayAnimsStates.Instance smi) => smi.def.tag, false);
	}

	// Token: 0x04000314 RID: 788
	public GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.State animating;

	// Token: 0x04000315 RID: 789
	public GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.State done;

	// Token: 0x020010FB RID: 4347
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06008368 RID: 33640 RVA: 0x00343612 File Offset: 0x00341812
		public Def(Tag tag, bool loop, string anim, string status_item_name, string status_item_tooltip) : this(tag, loop, new string[]
		{
			anim
		}, status_item_name, status_item_tooltip)
		{
		}

		// Token: 0x06008369 RID: 33641 RVA: 0x0034362A File Offset: 0x0034182A
		public Def(Tag tag, bool loop, string[] anims, string status_item_name, string status_item_tooltip)
		{
			this.tag = tag;
			this.loop = loop;
			this.anims = anims;
			this.statusItemName = status_item_name;
			this.statusItemTooltip = status_item_tooltip;
		}

		// Token: 0x0600836A RID: 33642 RVA: 0x00343657 File Offset: 0x00341857
		public override string ToString()
		{
			return this.tag.ToString() + "(PlayAnimsStates)";
		}

		// Token: 0x040063AF RID: 25519
		public Tag tag;

		// Token: 0x040063B0 RID: 25520
		public string[] anims;

		// Token: 0x040063B1 RID: 25521
		public bool loop;

		// Token: 0x040063B2 RID: 25522
		public string statusItemName;

		// Token: 0x040063B3 RID: 25523
		public string statusItemTooltip;
	}

	// Token: 0x020010FC RID: 4348
	public new class Instance : GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.GameInstance
	{
		// Token: 0x0600836B RID: 33643 RVA: 0x00343674 File Offset: 0x00341874
		public Instance(Chore<PlayAnimsStates.Instance> chore, PlayAnimsStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, def.tag);
		}

		// Token: 0x0600836C RID: 33644 RVA: 0x0034369C File Offset: 0x0034189C
		public void PlayAnims()
		{
			if (base.def.anims == null || base.def.anims.Length == 0)
			{
				return;
			}
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			for (int i = 0; i < base.def.anims.Length; i++)
			{
				KAnim.PlayMode mode = KAnim.PlayMode.Once;
				if (base.def.loop && i == base.def.anims.Length - 1)
				{
					mode = KAnim.PlayMode.Loop;
				}
				if (i == 0)
				{
					component.Play(base.def.anims[i], mode, 1f, 0f);
				}
				else
				{
					component.Queue(base.def.anims[i], mode, 1f, 0f);
				}
			}
		}

		// Token: 0x0600836D RID: 33645 RVA: 0x00343755 File Offset: 0x00341955
		public void HandleTagsChanged(object obj)
		{
			if (!base.smi.HasTag(base.smi.def.tag))
			{
				base.smi.GoTo(null);
			}
		}
	}
}
