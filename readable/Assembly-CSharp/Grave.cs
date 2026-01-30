using System;
using System.Collections.Generic;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000772 RID: 1906
public class Grave : StateMachineComponent<Grave.StatesInstance>
{
	// Token: 0x0600307B RID: 12411 RVA: 0x00117DE6 File Offset: 0x00115FE6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Grave>(-1697596308, Grave.OnStorageChangedDelegate);
		this.epitaphIdx = UnityEngine.Random.Range(0, int.MaxValue);
	}

	// Token: 0x0600307C RID: 12412 RVA: 0x00117E10 File Offset: 0x00116010
	protected override void OnSpawn()
	{
		base.GetComponent<Storage>().SetOffsets(Grave.DELIVERY_OFFSETS);
		Storage component = base.GetComponent<Storage>();
		Storage storage = component;
		storage.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(storage.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkEvent));
		KAnimFile anim = Assets.GetAnim("anim_bury_dupe_kanim");
		int num = 0;
		KAnim.Anim anim2;
		for (;;)
		{
			anim2 = anim.GetData().GetAnim(num);
			if (anim2 == null)
			{
				goto IL_8F;
			}
			if (anim2.name == "working_pre")
			{
				break;
			}
			num++;
		}
		float workTime = (float)(anim2.numFrames - 3) / anim2.frameRate;
		component.SetWorkTime(workTime);
		IL_8F:
		base.OnSpawn();
		base.smi.StartSM();
		Components.Graves.Add(this);
	}

	// Token: 0x0600307D RID: 12413 RVA: 0x00117EC8 File Offset: 0x001160C8
	protected override void OnCleanUp()
	{
		Components.Graves.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x0600307E RID: 12414 RVA: 0x00117EDC File Offset: 0x001160DC
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject != null)
		{
			this.graveName = gameObject.name;
			MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
			if (component != null)
			{
				Personality personality = Db.Get().Personalities.TryGet(component.personalityResourceId);
				KAnimFile anim = Assets.GetAnim("gravestone_kanim");
				if (personality != null && anim.GetData().GetAnim(personality.graveStone) != null)
				{
					this.graveAnim = personality.graveStone;
				}
			}
			Util.KDestroyGameObject(gameObject);
		}
	}

	// Token: 0x0600307F RID: 12415 RVA: 0x00117F63 File Offset: 0x00116163
	private void OnWorkEvent(Workable workable, Workable.WorkableEvent evt)
	{
	}

	// Token: 0x04001CDB RID: 7387
	[Serialize]
	public string graveName;

	// Token: 0x04001CDC RID: 7388
	[Serialize]
	public string graveAnim = "closed";

	// Token: 0x04001CDD RID: 7389
	[Serialize]
	public int epitaphIdx;

	// Token: 0x04001CDE RID: 7390
	[Serialize]
	public float burialTime = -1f;

	// Token: 0x04001CDF RID: 7391
	private static readonly CellOffset[] DELIVERY_OFFSETS = new CellOffset[1];

	// Token: 0x04001CE0 RID: 7392
	private static readonly EventSystem.IntraObjectHandler<Grave> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Grave>(delegate(Grave component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x02001673 RID: 5747
	public class StatesInstance : GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.GameInstance
	{
		// Token: 0x06009737 RID: 38711 RVA: 0x0038395D File Offset: 0x00381B5D
		public StatesInstance(Grave master) : base(master)
		{
		}

		// Token: 0x06009738 RID: 38712 RVA: 0x00383968 File Offset: 0x00381B68
		public void CreateFetchTask()
		{
			this.chore = new FetchChore(Db.Get().ChoreTypes.FetchCritical, base.GetComponent<Storage>(), DUPLICANTSTATS.STANDARD.BaseStats.DEFAULT_MASS, new HashSet<Tag>
			{
				GameTags.BaseMinion
			}, FetchChore.MatchCriteria.MatchTags, GameTags.Corpse, null, null, true, null, null, null, Operational.State.Operational, 0);
			this.chore.allowMultifetch = false;
		}

		// Token: 0x06009739 RID: 38713 RVA: 0x003839CF File Offset: 0x00381BCF
		public void CancelFetchTask()
		{
			this.chore.Cancel("Exit State");
			this.chore = null;
		}

		// Token: 0x040074E6 RID: 29926
		private FetchChore chore;
	}

	// Token: 0x02001674 RID: 5748
	public class States : GameStateMachine<Grave.States, Grave.StatesInstance, Grave>
	{
		// Token: 0x0600973A RID: 38714 RVA: 0x003839E8 File Offset: 0x00381BE8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.empty.PlayAnim("open").Enter("CreateFetchTask", delegate(Grave.StatesInstance smi)
			{
				smi.CreateFetchTask();
			}).Exit("CancelFetchTask", delegate(Grave.StatesInstance smi)
			{
				smi.CancelFetchTask();
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GraveEmpty, null).EventTransition(GameHashes.OnStorageChange, this.full, null);
			this.full.PlayAnim((Grave.StatesInstance smi) => smi.master.graveAnim, KAnim.PlayMode.Once).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Grave, null).Enter(delegate(Grave.StatesInstance smi)
			{
				if (smi.master.burialTime < 0f)
				{
					smi.master.burialTime = GameClock.Instance.GetTime();
				}
			});
		}

		// Token: 0x040074E7 RID: 29927
		public GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State empty;

		// Token: 0x040074E8 RID: 29928
		public GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State full;
	}
}
