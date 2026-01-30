using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200063E RID: 1598
[AddComponentMenu("KMonoBehaviour/scripts/Worker")]
public class StandardWorker : WorkerBase
{
	// Token: 0x06002624 RID: 9764 RVA: 0x000DB523 File Offset: 0x000D9723
	public override WorkerBase.State GetState()
	{
		return this.state;
	}

	// Token: 0x06002625 RID: 9765 RVA: 0x000DB52B File Offset: 0x000D972B
	public override WorkerBase.StartWorkInfo GetStartWorkInfo()
	{
		return this.startWorkInfo;
	}

	// Token: 0x06002626 RID: 9766 RVA: 0x000DB533 File Offset: 0x000D9733
	public override Workable GetWorkable()
	{
		if (this.startWorkInfo != null)
		{
			return this.startWorkInfo.workable;
		}
		return null;
	}

	// Token: 0x06002627 RID: 9767 RVA: 0x000DB54A File Offset: 0x000D974A
	public override KBatchedAnimController GetAnimController()
	{
		return base.GetComponent<KBatchedAnimController>();
	}

	// Token: 0x06002628 RID: 9768 RVA: 0x000DB552 File Offset: 0x000D9752
	public override Attributes GetAttributes()
	{
		return base.gameObject.GetAttributes();
	}

	// Token: 0x06002629 RID: 9769 RVA: 0x000DB55F File Offset: 0x000D975F
	public override AttributeConverterInstance GetAttributeConverter(string id)
	{
		return base.GetComponent<AttributeConverters>().GetConverter(id);
	}

	// Token: 0x0600262A RID: 9770 RVA: 0x000DB56D File Offset: 0x000D976D
	public override Guid OfferStatusItem(StatusItem item, object data = null)
	{
		return base.GetComponent<KSelectable>().AddStatusItem(item, data);
	}

	// Token: 0x0600262B RID: 9771 RVA: 0x000DB57C File Offset: 0x000D977C
	public override void RevokeStatusItem(Guid id)
	{
		base.GetComponent<KSelectable>().RemoveStatusItem(id, false);
	}

	// Token: 0x0600262C RID: 9772 RVA: 0x000DB58C File Offset: 0x000D978C
	public override void SetWorkCompleteData(object data)
	{
		this.workCompleteData = data;
	}

	// Token: 0x0600262D RID: 9773 RVA: 0x000DB595 File Offset: 0x000D9795
	public override bool UsesMultiTool()
	{
		return this.usesMultiTool;
	}

	// Token: 0x0600262E RID: 9774 RVA: 0x000DB59D File Offset: 0x000D979D
	public override bool IsFetchDrone()
	{
		return this.isFetchDrone;
	}

	// Token: 0x0600262F RID: 9775 RVA: 0x000DB5A5 File Offset: 0x000D97A5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.state = WorkerBase.State.Idle;
		base.Subscribe<StandardWorker>(1485595942, StandardWorker.OnChoreInterruptDelegate);
	}

	// Token: 0x06002630 RID: 9776 RVA: 0x000DB5C5 File Offset: 0x000D97C5
	private string GetWorkableDebugString()
	{
		if (this.GetWorkable() == null)
		{
			return "Null";
		}
		return this.GetWorkable().name;
	}

	// Token: 0x06002631 RID: 9777 RVA: 0x000DB5E8 File Offset: 0x000D97E8
	public void CompleteWork()
	{
		this.successFullyCompleted = false;
		this.state = WorkerBase.State.Idle;
		Workable workable = this.GetWorkable();
		if (workable != null)
		{
			if (workable.triggerWorkReactions && workable.GetWorkTime() > 30f)
			{
				string conversationTopic = workable.GetConversationTopic();
				if (!conversationTopic.IsNullOrWhiteSpace())
				{
					this.CreateCompletionReactable(conversationTopic);
				}
			}
			this.DetachAnimOverrides();
			workable.CompleteWork(this);
			if (workable.worker != null && !(workable is Constructable) && !(workable is Deconstructable) && !(workable is Repairable) && !(workable is Disinfectable))
			{
				BonusEvent.GameplayEventData gameplayEventData = new BonusEvent.GameplayEventData();
				gameplayEventData.workable = workable;
				gameplayEventData.worker = workable.worker;
				gameplayEventData.building = workable.GetComponent<BuildingComplete>();
				gameplayEventData.eventTrigger = GameHashes.UseBuilding;
				GameplayEventManager.Instance.Trigger(1175726587, gameplayEventData);
			}
		}
		this.InternalStopWork(workable, false);
	}

	// Token: 0x06002632 RID: 9778 RVA: 0x000DB6C4 File Offset: 0x000D98C4
	protected virtual void TryPlayingIdle()
	{
		Navigator component = base.GetComponent<Navigator>();
		if (component != null)
		{
			NavGrid.NavTypeData navTypeData = component.NavGrid.GetNavTypeData(component.CurrentNavType);
			if (navTypeData.idleAnim.IsValid)
			{
				base.GetComponent<KAnimControllerBase>().Play(navTypeData.idleAnim, KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x06002633 RID: 9779 RVA: 0x000DB720 File Offset: 0x000D9920
	public override WorkerBase.WorkResult Work(float dt)
	{
		if (this.state == WorkerBase.State.PendingCompletion)
		{
			bool flag = Time.time - this.workPendingCompletionTime > 10f;
			if (!base.GetComponent<KAnimControllerBase>().IsStopped() && !flag)
			{
				return WorkerBase.WorkResult.InProgress;
			}
			this.TryPlayingIdle();
			if (this.successFullyCompleted)
			{
				this.CompleteWork();
				return WorkerBase.WorkResult.Success;
			}
			this.StopWork();
			return WorkerBase.WorkResult.Failed;
		}
		else
		{
			if (this.state != WorkerBase.State.Completing)
			{
				Workable workable = this.GetWorkable();
				if (workable != null)
				{
					if (this.facing)
					{
						if (workable.ShouldFaceTargetWhenWorking())
						{
							this.facing.Face(workable.GetFacingTarget());
						}
						else
						{
							Rotatable component = workable.GetComponent<Rotatable>();
							bool flag2 = component != null && component.GetOrientation() == Orientation.FlipH;
							Vector3 vector = this.facing.transform.GetPosition();
							vector += (flag2 ? Vector3.left : Vector3.right);
							this.facing.Face(vector);
						}
					}
					if (dt > 0f && Game.Instance.FastWorkersModeActive)
					{
						dt = Mathf.Min(workable.WorkTimeRemaining + 0.01f, 5f);
					}
					Klei.AI.Attribute workAttribute = workable.GetWorkAttribute();
					AttributeLevels component2 = base.GetComponent<AttributeLevels>();
					if (workAttribute != null && workAttribute.IsTrainable && component2 != null)
					{
						float attributeExperienceMultiplier = workable.GetAttributeExperienceMultiplier();
						component2.AddExperience(workAttribute.Id, dt, attributeExperienceMultiplier);
					}
					string skillExperienceSkillGroup = workable.GetSkillExperienceSkillGroup();
					if (this.experienceRecipient != null && skillExperienceSkillGroup != null)
					{
						float skillExperienceMultiplier = workable.GetSkillExperienceMultiplier();
						this.experienceRecipient.AddExperienceWithAptitude(skillExperienceSkillGroup, dt, skillExperienceMultiplier);
					}
					float efficiencyMultiplier = workable.GetEfficiencyMultiplier(this);
					float dt2 = dt * efficiencyMultiplier * 1f;
					if (workable.WorkTick(this, dt2) && this.state == WorkerBase.State.Working)
					{
						this.successFullyCompleted = true;
						this.StartPlayingPostAnim();
						workable.OnPendingCompleteWork(this);
					}
				}
				return WorkerBase.WorkResult.InProgress;
			}
			if (this.successFullyCompleted)
			{
				this.CompleteWork();
				return WorkerBase.WorkResult.Success;
			}
			this.StopWork();
			return WorkerBase.WorkResult.Failed;
		}
	}

	// Token: 0x06002634 RID: 9780 RVA: 0x000DB904 File Offset: 0x000D9B04
	private void StartPlayingPostAnim()
	{
		Workable workable = this.GetWorkable();
		if (workable != null && !workable.alwaysShowProgressBar)
		{
			workable.ShowProgressBar(false);
		}
		base.GetComponent<KPrefabID>().AddTag(GameTags.PreventChoreInterruption, false);
		this.state = WorkerBase.State.PendingCompletion;
		this.workPendingCompletionTime = Time.time;
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		HashedString[] workPstAnims = workable.GetWorkPstAnims(this, this.successFullyCompleted);
		if (this.smi == null)
		{
			if (workPstAnims != null && workPstAnims.Length != 0)
			{
				if (workable != null && workable.synchronizeAnims)
				{
					KAnimControllerBase animController = workable.GetAnimController();
					if (animController != null)
					{
						animController.Play(workPstAnims, KAnim.PlayMode.Once);
					}
				}
				else
				{
					component.Play(workPstAnims, KAnim.PlayMode.Once);
				}
			}
			else
			{
				this.state = WorkerBase.State.Completing;
			}
		}
		base.Trigger(-1142962013, this);
	}

	// Token: 0x06002635 RID: 9781 RVA: 0x000DB9C0 File Offset: 0x000D9BC0
	protected virtual void InternalStopWork(Workable target_workable, bool is_aborted)
	{
		this.state = WorkerBase.State.Idle;
		base.gameObject.RemoveTag(GameTags.PerformingWorkRequest);
		base.GetComponent<KAnimControllerBase>().Offset -= this.workAnimOffset;
		this.workAnimOffset = Vector3.zero;
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.PreventChoreInterruption);
		this.DetachAnimOverrides();
		this.ClearPasserbyReactable();
		AnimEventHandler component = base.GetComponent<AnimEventHandler>();
		if (component)
		{
			component.ClearContext();
		}
		if (this.previousStatusItem.item != null)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, this.previousStatusItem.item, this.previousStatusItem.data);
		}
		else
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
		if (target_workable != null)
		{
			target_workable.Unsubscribe(this.onWorkChoreDisabledHandle);
			target_workable.StopWork(this, is_aborted);
		}
		if (this.smi != null)
		{
			this.smi.StopSM("stopping work");
			this.smi = null;
		}
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
		base.transform.SetPosition(position);
		this.startWorkInfo = null;
	}

	// Token: 0x06002636 RID: 9782 RVA: 0x000DBB03 File Offset: 0x000D9D03
	private void OnChoreInterrupt(object data)
	{
		if (this.state == WorkerBase.State.Working)
		{
			this.successFullyCompleted = false;
			this.StartPlayingPostAnim();
		}
	}

	// Token: 0x06002637 RID: 9783 RVA: 0x000DBB1C File Offset: 0x000D9D1C
	private void OnWorkChoreDisabled(object data)
	{
		string text = data as string;
		ChoreConsumer component = base.GetComponent<ChoreConsumer>();
		if (component != null && component.choreDriver != null)
		{
			Chore currentChore = component.choreDriver.GetCurrentChore();
			if (currentChore != null)
			{
				currentChore.Fail((text != null) ? text : "WorkChoreDisabled");
			}
		}
	}

	// Token: 0x06002638 RID: 9784 RVA: 0x000DBB70 File Offset: 0x000D9D70
	public override void StopWork()
	{
		Workable workable = this.GetWorkable();
		if (this.state == WorkerBase.State.PendingCompletion || this.state == WorkerBase.State.Completing)
		{
			this.state = WorkerBase.State.Idle;
			if (this.successFullyCompleted)
			{
				this.CompleteWork();
				base.Trigger(1705586602, this);
			}
			else
			{
				base.Trigger(-993481695, this);
				this.InternalStopWork(workable, true);
			}
		}
		else if (this.state == WorkerBase.State.Working)
		{
			if (workable != null && workable.synchronizeAnims)
			{
				KAnimControllerBase animController = workable.GetAnimController();
				if (animController != null)
				{
					HashedString[] workPstAnims = workable.GetWorkPstAnims(this, false);
					if (workPstAnims != null && workPstAnims.Length != 0)
					{
						animController.Play(workPstAnims, KAnim.PlayMode.Once);
						animController.SetPositionPercent(1f);
					}
				}
			}
			base.Trigger(-993481695, this);
			this.InternalStopWork(workable, true);
		}
		base.Trigger(2027193395, this);
	}

	// Token: 0x06002639 RID: 9785 RVA: 0x000DBC3C File Offset: 0x000D9E3C
	public override void StartWork(WorkerBase.StartWorkInfo start_work_info)
	{
		this.startWorkInfo = start_work_info;
		Game.Instance.StartedWork();
		Workable workable = this.GetWorkable();
		this.surpressForceSyncOnUpdate = false;
		if (this.state != WorkerBase.State.Idle)
		{
			string text = "";
			if (workable != null)
			{
				text = workable.name;
			}
			global::Debug.LogError(string.Concat(new string[]
			{
				base.name,
				".",
				text,
				".state should be idle but instead it's:",
				this.state.ToString()
			}));
		}
		string name = workable.GetType().Name;
		try
		{
			base.gameObject.AddTag(GameTags.PerformingWorkRequest);
			this.state = WorkerBase.State.Working;
			base.gameObject.Trigger(1568504979, this);
			if (workable != null)
			{
				this.animInfo = workable.GetAnim(this);
				if (this.animInfo.smi != null)
				{
					this.smi = this.animInfo.smi;
					this.smi.StartSM();
				}
				Vector3 position = base.transform.GetPosition();
				position.z = Grid.GetLayerZ(workable.workLayer);
				base.transform.SetPosition(position);
				KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
				if (this.animInfo.smi == null)
				{
					this.AttachOverrideAnims(component);
				}
				this.surpressForceSyncOnUpdate = workable.surpressWorkerForceSync;
				HashedString[] workAnims = workable.GetWorkAnims(this);
				KAnim.PlayMode workAnimPlayMode = workable.GetWorkAnimPlayMode();
				Vector3 workOffset = workable.GetWorkOffset();
				this.workAnimOffset = workOffset;
				component.Offset += workOffset;
				if (this.usesMultiTool && this.animInfo.smi == null && workAnims != null && workAnims.Length != 0 && this.experienceRecipient != null)
				{
					if (workable.synchronizeAnims)
					{
						KAnimControllerBase animController = workable.GetAnimController();
						if (animController != null)
						{
							this.kanimSynchronizer = animController.GetSynchronizer();
							if (this.kanimSynchronizer != null)
							{
								this.kanimSynchronizer.Add(component);
							}
						}
						animController.Play(workAnims, workAnimPlayMode);
					}
					else
					{
						component.Play(workAnims, workAnimPlayMode);
					}
				}
			}
			workable.StartWork(this);
			if (workable == null)
			{
				global::Debug.LogWarning("Stopped work as soon as I started. This is usually a sign that a chore is open when it shouldn't be or that it's preconditions are wrong.");
			}
			else
			{
				this.onWorkChoreDisabledHandle = workable.Subscribe(2108245096, new Action<object>(this.OnWorkChoreDisabled));
				if (workable.triggerWorkReactions && workable.WorkTimeRemaining > 10f)
				{
					this.CreatePasserbyReactable();
				}
				KSelectable component2 = base.GetComponent<KSelectable>();
				this.previousStatusItem = component2.GetStatusItem(Db.Get().StatusItemCategories.Main);
				component2.SetStatusItem(Db.Get().StatusItemCategories.Main, workable.GetWorkerStatusItem(), workable);
			}
		}
		catch (Exception ex)
		{
			string str = "Exception in: Worker.StartWork(" + name + ")";
			DebugUtil.LogErrorArgs(this, new object[]
			{
				str + "\n" + ex.ToString()
			});
			throw;
		}
	}

	// Token: 0x0600263A RID: 9786 RVA: 0x000DBF34 File Offset: 0x000DA134
	private void Update()
	{
		if (this.state == WorkerBase.State.Working && !this.surpressForceSyncOnUpdate)
		{
			this.ForceSyncAnims();
		}
	}

	// Token: 0x0600263B RID: 9787 RVA: 0x000DBF4D File Offset: 0x000DA14D
	private void ForceSyncAnims()
	{
		if (Time.deltaTime > 0f && this.kanimSynchronizer != null)
		{
			this.kanimSynchronizer.SyncTime();
		}
	}

	// Token: 0x0600263C RID: 9788 RVA: 0x000DBF70 File Offset: 0x000DA170
	public override bool InstantlyFinish()
	{
		Workable workable = this.GetWorkable();
		return workable != null && workable.InstantlyFinish(this);
	}

	// Token: 0x0600263D RID: 9789 RVA: 0x000DBF98 File Offset: 0x000DA198
	private void AttachOverrideAnims(KAnimControllerBase worker_controller)
	{
		if (this.animInfo.overrideAnims != null && this.animInfo.overrideAnims.Length != 0)
		{
			for (int i = 0; i < this.animInfo.overrideAnims.Length; i++)
			{
				worker_controller.AddAnimOverrides(this.animInfo.overrideAnims[i], 0f);
			}
		}
	}

	// Token: 0x0600263E RID: 9790 RVA: 0x000DBFF0 File Offset: 0x000DA1F0
	private void DetachAnimOverrides()
	{
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		if (this.kanimSynchronizer != null)
		{
			this.kanimSynchronizer.RemoveWithoutIdleAnim(component);
			this.kanimSynchronizer = null;
		}
		if (this.animInfo.overrideAnims != null)
		{
			for (int i = 0; i < this.animInfo.overrideAnims.Length; i++)
			{
				component.RemoveAnimOverrides(this.animInfo.overrideAnims[i]);
			}
			this.animInfo.overrideAnims = null;
		}
	}

	// Token: 0x0600263F RID: 9791 RVA: 0x000DC064 File Offset: 0x000DA264
	private void CreateCompletionReactable(string topic)
	{
		if (GameClock.Instance.GetTime() / 600f < 1f)
		{
			return;
		}
		EmoteReactable emoteReactable = OneshotReactableLocator.CreateOneshotReactable(base.gameObject, 3f, "WorkCompleteAcknowledgement", Db.Get().ChoreTypes.Emote, 9, 5, 100f);
		Emote clapCheer = Db.Get().Emotes.Minion.ClapCheer;
		emoteReactable.SetEmote(clapCheer);
		emoteReactable.RegisterEmoteStepCallbacks("clapcheer_pre", new Action<GameObject>(this.GetReactionEffect), null).RegisterEmoteStepCallbacks("clapcheer_pst", null, delegate(GameObject r)
		{
			r.Trigger(937885943, topic);
		});
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(topic, "ui", true);
		if (uisprite != null)
		{
			Thought thought = new Thought("Completion_" + topic, null, uisprite.first, "mode_satisfaction", "conversation_short", "bubble_conversation", SpeechMonitor.PREFIX_HAPPY, "", true, 4f);
			emoteReactable.SetThought(thought);
		}
	}

	// Token: 0x06002640 RID: 9792 RVA: 0x000DC17C File Offset: 0x000DA37C
	private void CreatePasserbyReactable()
	{
		if (GameClock.Instance.GetTime() / 600f < 1f)
		{
			return;
		}
		if (this.passerbyReactable == null)
		{
			EmoteReactable emoteReactable = new EmoteReactable(base.gameObject, "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, 5, 5, 30f, 720f * TuningData<DupeGreetingManager.Tuning>.Get().greetingDelayMultiplier, float.PositiveInfinity, 0f);
			Emote thumbsUp = Db.Get().Emotes.Minion.ThumbsUp;
			emoteReactable.SetEmote(thumbsUp).SetThought(Db.Get().Thoughts.Encourage).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor)).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsFacingMe)).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsntPartying));
			emoteReactable.RegisterEmoteStepCallbacks("react", new Action<GameObject>(this.GetReactionEffect), null);
			this.passerbyReactable = emoteReactable;
		}
	}

	// Token: 0x06002641 RID: 9793 RVA: 0x000DC27C File Offset: 0x000DA47C
	private void GetReactionEffect(GameObject reactor)
	{
		Effects component = base.GetComponent<Effects>();
		if (component != null)
		{
			component.Add("WorkEncouraged", true);
		}
	}

	// Token: 0x06002642 RID: 9794 RVA: 0x000DC2A6 File Offset: 0x000DA4A6
	private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
	{
		return transition.end == NavType.Floor;
	}

	// Token: 0x06002643 RID: 9795 RVA: 0x000DC2B4 File Offset: 0x000DA4B4
	private bool ReactorIsFacingMe(GameObject reactor, Navigator.ActiveTransition transition)
	{
		Facing component = reactor.GetComponent<Facing>();
		return base.transform.GetPosition().x < reactor.transform.GetPosition().x == component.GetFacing();
	}

	// Token: 0x06002644 RID: 9796 RVA: 0x000DC2F4 File Offset: 0x000DA4F4
	private bool ReactorIsntPartying(GameObject reactor, Navigator.ActiveTransition transition)
	{
		ChoreConsumer component = reactor.GetComponent<ChoreConsumer>();
		return component.choreDriver.HasChore() && component.choreDriver.GetCurrentChore().choreType != Db.Get().ChoreTypes.Party;
	}

	// Token: 0x06002645 RID: 9797 RVA: 0x000DC33B File Offset: 0x000DA53B
	private void ClearPasserbyReactable()
	{
		if (this.passerbyReactable != null)
		{
			this.passerbyReactable.Cleanup();
			this.passerbyReactable = null;
		}
	}

	// Token: 0x04001682 RID: 5762
	private WorkerBase.State state;

	// Token: 0x04001683 RID: 5763
	private WorkerBase.StartWorkInfo startWorkInfo;

	// Token: 0x04001684 RID: 5764
	private const float EARLIEST_REACT_TIME = 1f;

	// Token: 0x04001685 RID: 5765
	[MyCmpGet]
	private Facing facing;

	// Token: 0x04001686 RID: 5766
	[MyCmpGet]
	private IExperienceRecipient experienceRecipient;

	// Token: 0x04001687 RID: 5767
	private float workPendingCompletionTime;

	// Token: 0x04001688 RID: 5768
	private int onWorkChoreDisabledHandle;

	// Token: 0x04001689 RID: 5769
	public object workCompleteData;

	// Token: 0x0400168A RID: 5770
	private Workable.AnimInfo animInfo;

	// Token: 0x0400168B RID: 5771
	private KAnimSynchronizer kanimSynchronizer;

	// Token: 0x0400168C RID: 5772
	private StatusItemGroup.Entry previousStatusItem;

	// Token: 0x0400168D RID: 5773
	private StateMachine.Instance smi;

	// Token: 0x0400168E RID: 5774
	private bool successFullyCompleted;

	// Token: 0x0400168F RID: 5775
	private bool surpressForceSyncOnUpdate;

	// Token: 0x04001690 RID: 5776
	private Vector3 workAnimOffset = Vector3.zero;

	// Token: 0x04001691 RID: 5777
	public bool usesMultiTool = true;

	// Token: 0x04001692 RID: 5778
	public bool isFetchDrone;

	// Token: 0x04001693 RID: 5779
	private static readonly EventSystem.IntraObjectHandler<StandardWorker> OnChoreInterruptDelegate = new EventSystem.IntraObjectHandler<StandardWorker>(delegate(StandardWorker component, object data)
	{
		component.OnChoreInterrupt(data);
	});

	// Token: 0x04001694 RID: 5780
	private Reactable passerbyReactable;
}
