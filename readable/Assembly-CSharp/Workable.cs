using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000674 RID: 1652
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Workable")]
public class Workable : KMonoBehaviour, ISaveLoadable, IApproachable
{
	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x06002831 RID: 10289 RVA: 0x000E7DA5 File Offset: 0x000E5FA5
	// (set) Token: 0x06002832 RID: 10290 RVA: 0x000E7DAD File Offset: 0x000E5FAD
	public WorkerBase worker { get; protected set; }

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x06002833 RID: 10291 RVA: 0x000E7DB6 File Offset: 0x000E5FB6
	// (set) Token: 0x06002834 RID: 10292 RVA: 0x000E7DBE File Offset: 0x000E5FBE
	public float WorkTimeRemaining
	{
		get
		{
			return this.workTimeRemaining;
		}
		set
		{
			this.workTimeRemaining = value;
		}
	}

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x06002835 RID: 10293 RVA: 0x000E7DC7 File Offset: 0x000E5FC7
	// (set) Token: 0x06002836 RID: 10294 RVA: 0x000E7DCF File Offset: 0x000E5FCF
	public bool preferUnreservedCell { get; set; }

	// Token: 0x06002837 RID: 10295 RVA: 0x000E7DD8 File Offset: 0x000E5FD8
	public virtual float GetWorkTime()
	{
		return this.workTime;
	}

	// Token: 0x06002838 RID: 10296 RVA: 0x000E7DE0 File Offset: 0x000E5FE0
	public WorkerBase GetWorker()
	{
		return this.worker;
	}

	// Token: 0x06002839 RID: 10297 RVA: 0x000E7DE8 File Offset: 0x000E5FE8
	public virtual float GetPercentComplete()
	{
		if (this.workTimeRemaining > this.workTime)
		{
			return -1f;
		}
		return 1f - this.workTimeRemaining / this.workTime;
	}

	// Token: 0x0600283A RID: 10298 RVA: 0x000E7E11 File Offset: 0x000E6011
	public void ConfigureMultitoolContext(HashedString context, Tag hitEffectTag)
	{
		this.multitoolContext = context;
		this.multitoolHitEffectTag = hitEffectTag;
	}

	// Token: 0x0600283B RID: 10299 RVA: 0x000E7E24 File Offset: 0x000E6024
	public virtual Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		Workable.AnimInfo result = default(Workable.AnimInfo);
		if (this.overrideAnims != null && this.overrideAnims.Length != 0)
		{
			BuildingFacade buildingFacade = this.GetBuildingFacade();
			bool flag = false;
			if (buildingFacade != null && !buildingFacade.IsOriginal)
			{
				flag = buildingFacade.interactAnims.TryGetValue(base.name, out result.overrideAnims);
			}
			if (!flag)
			{
				result.overrideAnims = this.overrideAnims;
			}
		}
		if (this.multitoolContext.IsValid && this.multitoolHitEffectTag.IsValid)
		{
			result.smi = new MultitoolController.Instance(this, worker, this.multitoolContext, Assets.GetPrefab(this.multitoolHitEffectTag));
		}
		return result;
	}

	// Token: 0x0600283C RID: 10300 RVA: 0x000E7EC7 File Offset: 0x000E60C7
	public virtual HashedString[] GetWorkAnims(WorkerBase worker)
	{
		return this.workAnims;
	}

	// Token: 0x0600283D RID: 10301 RVA: 0x000E7ECF File Offset: 0x000E60CF
	public virtual KAnim.PlayMode GetWorkAnimPlayMode()
	{
		return this.workAnimPlayMode;
	}

	// Token: 0x0600283E RID: 10302 RVA: 0x000E7ED7 File Offset: 0x000E60D7
	public virtual HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
	{
		if (successfully_completed)
		{
			return this.workingPstComplete;
		}
		return this.workingPstFailed;
	}

	// Token: 0x0600283F RID: 10303 RVA: 0x000E7EE9 File Offset: 0x000E60E9
	public virtual Vector3 GetWorkOffset()
	{
		return Vector3.zero;
	}

	// Token: 0x06002840 RID: 10304 RVA: 0x000E7EF0 File Offset: 0x000E60F0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().MiscStatusItems.Using;
		this.workingStatusItem = Db.Get().MiscStatusItems.Operating;
		this.readyForSkillWorkStatusItem = Db.Get().BuildingStatusItems.RequiresSkillPerk;
		this.workTime = this.GetWorkTime();
		this.workTimeRemaining = Mathf.Min(this.workTimeRemaining, this.workTime);
	}

	// Token: 0x06002841 RID: 10305 RVA: 0x000E7F68 File Offset: 0x000E6168
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.shouldShowSkillPerkStatusItem && !string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			if (this.skillsUpdateHandle != -1)
			{
				Game.Instance.Unsubscribe(this.skillsUpdateHandle);
			}
			this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, Workable.UpdateStatusItemDispatcher, this);
		}
		if (this.requireMinionToWork && this.minionUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.minionUpdateHandle);
		}
		this.minionUpdateHandle = Game.Instance.Subscribe(586301400, Workable.UpdateStatusItemDispatcher, this);
		base.GetComponent<KPrefabID>().AddTag(GameTags.HasChores, false);
		if (base.gameObject.HasTag(this.laboratoryEfficiencyBonusTagRequired))
		{
			this.useLaboratoryEfficiencyBonus = true;
			base.Subscribe<Workable>(144050788, Workable.OnUpdateRoomDelegate);
		}
		this.ShowProgressBar(this.alwaysShowProgressBar && this.workTimeRemaining < this.GetWorkTime());
		this.UpdateStatusItem(null);
	}

	// Token: 0x06002842 RID: 10306 RVA: 0x000E8064 File Offset: 0x000E6264
	private void RefreshRoom()
	{
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(base.gameObject));
		if (cavityForCell != null && cavityForCell.room != null)
		{
			this.OnUpdateRoom(cavityForCell.room);
			return;
		}
		this.OnUpdateRoom(null);
	}

	// Token: 0x06002843 RID: 10307 RVA: 0x000E80AC File Offset: 0x000E62AC
	private void OnUpdateRoom(object data)
	{
		if (this.worker == null)
		{
			return;
		}
		Room room = (Room)data;
		if (room != null && room.roomType == Db.Get().RoomTypes.Laboratory)
		{
			this.currentlyInLaboratory = true;
			if (this.laboratoryEfficiencyBonusStatusItemHandle == Guid.Empty)
			{
				this.laboratoryEfficiencyBonusStatusItemHandle = this.worker.OfferStatusItem(Db.Get().DuplicantStatusItems.LaboratoryWorkEfficiencyBonus, this);
				return;
			}
		}
		else
		{
			this.currentlyInLaboratory = false;
			if (this.laboratoryEfficiencyBonusStatusItemHandle != Guid.Empty)
			{
				this.worker.RevokeStatusItem(this.laboratoryEfficiencyBonusStatusItemHandle);
				this.laboratoryEfficiencyBonusStatusItemHandle = Guid.Empty;
			}
		}
	}

	// Token: 0x06002844 RID: 10308 RVA: 0x000E815C File Offset: 0x000E635C
	protected virtual void UpdateStatusItem(object data = null)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (component == null)
		{
			return;
		}
		component.RemoveStatusItem(this.workStatusItemHandle, false);
		if (this.worker == null)
		{
			if (this.requireMinionToWork && Components.LiveMinionIdentities.GetWorldItems(this.GetMyWorldId(), false).Count == 0)
			{
				this.workStatusItemHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.WorkRequiresMinion, null);
				return;
			}
			if (this.shouldShowSkillPerkStatusItem && !string.IsNullOrEmpty(this.requiredSkillPerk))
			{
				if (!MinionResume.AnyMinionHasPerk(this.requiredSkillPerk, this.GetMyWorldId()))
				{
					StatusItem status_item = DlcManager.FeatureClusterSpaceEnabled() ? Db.Get().BuildingStatusItems.ClusterColonyLacksRequiredSkillPerk : Db.Get().BuildingStatusItems.ColonyLacksRequiredSkillPerk;
					this.workStatusItemHandle = component.AddStatusItem(status_item, this.requiredSkillPerk);
					return;
				}
				this.workStatusItemHandle = component.AddStatusItem(this.readyForSkillWorkStatusItem, this.requiredSkillPerk);
				return;
			}
		}
		else if (this.workingStatusItem != null)
		{
			this.workStatusItemHandle = component.AddStatusItem(this.workingStatusItem, this);
		}
	}

	// Token: 0x06002845 RID: 10309 RVA: 0x000E8274 File Offset: 0x000E6474
	protected override void OnLoadLevel()
	{
		this.overrideAnims = null;
		base.OnLoadLevel();
	}

	// Token: 0x06002846 RID: 10310 RVA: 0x000E8283 File Offset: 0x000E6483
	public virtual int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x06002847 RID: 10311 RVA: 0x000E828C File Offset: 0x000E648C
	public void StartWork(WorkerBase worker_to_start)
	{
		global::Debug.Assert(worker_to_start != null, "How did we get a null worker?");
		this.worker = worker_to_start;
		this.UpdateStatusItem(null);
		if (this.showProgressBar)
		{
			this.ShowProgressBar(true);
		}
		if (this.useLaboratoryEfficiencyBonus)
		{
			this.RefreshRoom();
		}
		this.OnStartWork(this.worker);
		if (this.worker != null)
		{
			string conversationTopic = this.GetConversationTopic();
			if (conversationTopic != null)
			{
				this.worker.Trigger(937885943, conversationTopic);
			}
		}
		if (this.OnWorkableEventCB != null)
		{
			this.OnWorkableEventCB(this, Workable.WorkableEvent.WorkStarted);
		}
		this.numberOfUses++;
		if (this.worker != null)
		{
			if (base.gameObject.GetComponent<KSelectable>() != null && base.gameObject.GetComponent<KSelectable>().IsSelected && this.worker.gameObject.GetComponent<LoopingSounds>() != null)
			{
				this.worker.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(true);
			}
			else if (this.worker.gameObject.GetComponent<KSelectable>() != null && this.worker.gameObject.GetComponent<KSelectable>().IsSelected && base.gameObject.GetComponent<LoopingSounds>() != null)
			{
				base.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(true);
			}
		}
		base.gameObject.Trigger(853695848, this);
	}

	// Token: 0x06002848 RID: 10312 RVA: 0x000E83F8 File Offset: 0x000E65F8
	public bool WorkTick(WorkerBase worker, float dt)
	{
		bool flag = false;
		if (dt > 0f)
		{
			this.workTimeRemaining -= dt;
			flag = this.OnWorkTick(worker, dt);
		}
		return flag || this.workTimeRemaining < 0f;
	}

	// Token: 0x06002849 RID: 10313 RVA: 0x000E8438 File Offset: 0x000E6638
	public virtual float GetEfficiencyMultiplier(WorkerBase worker)
	{
		float num = 1f;
		if (this.attributeConverter != null)
		{
			AttributeConverterInstance attributeConverterInstance = worker.GetAttributeConverter(this.attributeConverter.Id);
			if (attributeConverterInstance != null)
			{
				num += attributeConverterInstance.Evaluate();
			}
		}
		if (this.lightEfficiencyBonus)
		{
			int num2 = Grid.PosToCell(worker.gameObject);
			if (Grid.IsValidCell(num2))
			{
				if (Grid.LightIntensity[num2] > DUPLICANTSTATS.STANDARD.Light.NO_LIGHT)
				{
					this.currentlyLit = true;
					num += DUPLICANTSTATS.STANDARD.Light.LIGHT_WORK_EFFICIENCY_BONUS;
					if (this.lightEfficiencyBonusStatusItemHandle == Guid.Empty)
					{
						this.lightEfficiencyBonusStatusItemHandle = worker.OfferStatusItem(Db.Get().DuplicantStatusItems.LightWorkEfficiencyBonus, this);
					}
				}
				else
				{
					this.currentlyLit = false;
					if (this.lightEfficiencyBonusStatusItemHandle != Guid.Empty)
					{
						worker.RevokeStatusItem(this.lightEfficiencyBonusStatusItemHandle);
					}
				}
			}
		}
		if (this.useLaboratoryEfficiencyBonus && this.currentlyInLaboratory)
		{
			num += 0.1f;
		}
		return Mathf.Max(num, this.minimumAttributeMultiplier);
	}

	// Token: 0x0600284A RID: 10314 RVA: 0x000E8541 File Offset: 0x000E6741
	public virtual Klei.AI.Attribute GetWorkAttribute()
	{
		if (this.attributeConverter != null)
		{
			return this.attributeConverter.attribute;
		}
		return null;
	}

	// Token: 0x0600284B RID: 10315 RVA: 0x000E8558 File Offset: 0x000E6758
	public virtual string GetConversationTopic()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (!component.HasTag(GameTags.NotConversationTopic))
		{
			return component.PrefabTag.Name;
		}
		return null;
	}

	// Token: 0x0600284C RID: 10316 RVA: 0x000E8586 File Offset: 0x000E6786
	public float GetAttributeExperienceMultiplier()
	{
		return this.attributeExperienceMultiplier;
	}

	// Token: 0x0600284D RID: 10317 RVA: 0x000E858E File Offset: 0x000E678E
	public string GetSkillExperienceSkillGroup()
	{
		return this.skillExperienceSkillGroup;
	}

	// Token: 0x0600284E RID: 10318 RVA: 0x000E8596 File Offset: 0x000E6796
	public float GetSkillExperienceMultiplier()
	{
		return this.skillExperienceMultiplier;
	}

	// Token: 0x0600284F RID: 10319 RVA: 0x000E859E File Offset: 0x000E679E
	protected virtual bool OnWorkTick(WorkerBase worker, float dt)
	{
		return false;
	}

	// Token: 0x06002850 RID: 10320 RVA: 0x000E85A4 File Offset: 0x000E67A4
	public void StopWork(WorkerBase workerToStop, bool aborted)
	{
		if (this.worker == workerToStop && aborted)
		{
			this.OnAbortWork(workerToStop);
		}
		if (this.shouldTransferDiseaseWithWorker)
		{
			this.TransferDiseaseWithWorker(workerToStop);
		}
		if (this.OnWorkableEventCB != null)
		{
			this.OnWorkableEventCB(this, Workable.WorkableEvent.WorkStopped);
		}
		this.OnStopWork(workerToStop);
		if (this.resetProgressOnStop)
		{
			this.workTimeRemaining = this.GetWorkTime();
		}
		this.ShowProgressBar(this.alwaysShowProgressBar && this.workTimeRemaining < this.GetWorkTime());
		if (this.lightEfficiencyBonusStatusItemHandle != Guid.Empty)
		{
			workerToStop.RevokeStatusItem(this.lightEfficiencyBonusStatusItemHandle);
			this.lightEfficiencyBonusStatusItemHandle = Guid.Empty;
		}
		if (this.laboratoryEfficiencyBonusStatusItemHandle != Guid.Empty)
		{
			this.worker.RevokeStatusItem(this.laboratoryEfficiencyBonusStatusItemHandle);
			this.laboratoryEfficiencyBonusStatusItemHandle = Guid.Empty;
		}
		if (base.gameObject.GetComponent<KSelectable>() != null && !base.gameObject.GetComponent<KSelectable>().IsSelected && base.gameObject.GetComponent<LoopingSounds>() != null)
		{
			base.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(false);
		}
		else if (workerToStop.gameObject.GetComponent<KSelectable>() != null && !workerToStop.gameObject.GetComponent<KSelectable>().IsSelected && workerToStop.gameObject.GetComponent<LoopingSounds>() != null)
		{
			workerToStop.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(false);
		}
		this.worker = null;
		base.gameObject.Trigger(679550494, this);
		this.UpdateStatusItem(null);
	}

	// Token: 0x06002851 RID: 10321 RVA: 0x000E8730 File Offset: 0x000E6930
	public virtual StatusItem GetWorkerStatusItem()
	{
		return this.workerStatusItem;
	}

	// Token: 0x06002852 RID: 10322 RVA: 0x000E8738 File Offset: 0x000E6938
	public void SetWorkerStatusItem(StatusItem item)
	{
		this.workerStatusItem = item;
	}

	// Token: 0x06002853 RID: 10323 RVA: 0x000E8744 File Offset: 0x000E6944
	public void CompleteWork(WorkerBase worker)
	{
		if (this.shouldTransferDiseaseWithWorker)
		{
			this.TransferDiseaseWithWorker(worker);
		}
		this.OnCompleteWork(worker);
		if (this.OnWorkableEventCB != null)
		{
			this.OnWorkableEventCB(this, Workable.WorkableEvent.WorkCompleted);
		}
		this.workTimeRemaining = this.GetWorkTime();
		this.ShowProgressBar(false);
		base.gameObject.Trigger(-2011693419, this);
	}

	// Token: 0x06002854 RID: 10324 RVA: 0x000E87A0 File Offset: 0x000E69A0
	public void SetReportType(ReportManager.ReportType report_type)
	{
		this.reportType = report_type;
	}

	// Token: 0x06002855 RID: 10325 RVA: 0x000E87A9 File Offset: 0x000E69A9
	public ReportManager.ReportType GetReportType()
	{
		return this.reportType;
	}

	// Token: 0x06002856 RID: 10326 RVA: 0x000E87B1 File Offset: 0x000E69B1
	protected virtual void OnStartWork(WorkerBase worker)
	{
	}

	// Token: 0x06002857 RID: 10327 RVA: 0x000E87B3 File Offset: 0x000E69B3
	protected virtual void OnStopWork(WorkerBase worker)
	{
	}

	// Token: 0x06002858 RID: 10328 RVA: 0x000E87B5 File Offset: 0x000E69B5
	protected virtual void OnCompleteWork(WorkerBase worker)
	{
	}

	// Token: 0x06002859 RID: 10329 RVA: 0x000E87B7 File Offset: 0x000E69B7
	protected virtual void OnAbortWork(WorkerBase worker)
	{
	}

	// Token: 0x0600285A RID: 10330 RVA: 0x000E87B9 File Offset: 0x000E69B9
	public virtual void OnPendingCompleteWork(WorkerBase worker)
	{
	}

	// Token: 0x0600285B RID: 10331 RVA: 0x000E87BB File Offset: 0x000E69BB
	public void SetOffsets(CellOffset[] offsets)
	{
		if (this.offsetTracker != null)
		{
			this.offsetTracker.Clear();
		}
		this.offsetTracker = new StandardOffsetTracker(offsets);
	}

	// Token: 0x0600285C RID: 10332 RVA: 0x000E87DC File Offset: 0x000E69DC
	public void SetOffsetTable(CellOffset[][] offset_table)
	{
		if (this.offsetTracker != null)
		{
			this.offsetTracker.Clear();
		}
		this.offsetTracker = new OffsetTableTracker(offset_table, this);
	}

	// Token: 0x0600285D RID: 10333 RVA: 0x000E87FE File Offset: 0x000E69FE
	public virtual CellOffset[] GetOffsets(int cell)
	{
		if (this.offsetTracker == null)
		{
			this.offsetTracker = new StandardOffsetTracker(new CellOffset[1]);
		}
		return this.offsetTracker.GetOffsets(cell);
	}

	// Token: 0x0600285E RID: 10334 RVA: 0x000E8825 File Offset: 0x000E6A25
	public virtual bool ValidateOffsets(int cell)
	{
		if (this.offsetTracker == null)
		{
			this.offsetTracker = new StandardOffsetTracker(new CellOffset[1]);
		}
		return this.offsetTracker.ValidateOffsets(cell);
	}

	// Token: 0x0600285F RID: 10335 RVA: 0x000E884C File Offset: 0x000E6A4C
	public CellOffset[] GetOffsets()
	{
		return this.GetOffsets(Grid.PosToCell(this));
	}

	// Token: 0x06002860 RID: 10336 RVA: 0x000E885A File Offset: 0x000E6A5A
	public void SetWorkTime(float work_time)
	{
		this.workTime = work_time;
		this.workTimeRemaining = work_time;
	}

	// Token: 0x06002861 RID: 10337 RVA: 0x000E886A File Offset: 0x000E6A6A
	public bool ShouldFaceTargetWhenWorking()
	{
		return this.faceTargetWhenWorking;
	}

	// Token: 0x06002862 RID: 10338 RVA: 0x000E8872 File Offset: 0x000E6A72
	public virtual Vector3 GetFacingTarget()
	{
		return base.transform.GetPosition();
	}

	// Token: 0x06002863 RID: 10339 RVA: 0x000E8880 File Offset: 0x000E6A80
	public void ShowProgressBar(bool show)
	{
		if (show)
		{
			if (this.progressBar == null)
			{
				this.progressBar = ProgressBar.CreateProgressBar(base.gameObject, new Func<float>(this.GetPercentComplete));
			}
			this.progressBar.SetVisibility(true);
			return;
		}
		if (this.progressBar != null)
		{
			this.progressBar.gameObject.DeleteObject();
			this.progressBar = null;
		}
	}

	// Token: 0x06002864 RID: 10340 RVA: 0x000E88F0 File Offset: 0x000E6AF0
	protected override void OnCleanUp()
	{
		this.ShowProgressBar(false);
		if (this.offsetTracker != null)
		{
			this.offsetTracker.Clear();
		}
		if (this.skillsUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.skillsUpdateHandle);
		}
		if (this.minionUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.minionUpdateHandle);
		}
		base.OnCleanUp();
		this.OnWorkableEventCB = null;
	}

	// Token: 0x06002865 RID: 10341 RVA: 0x000E8958 File Offset: 0x000E6B58
	public virtual Vector3 GetTargetPoint()
	{
		Vector3 vector = base.transform.GetPosition();
		float y = vector.y + 0.65f;
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component != null)
		{
			vector = component.bounds.center;
		}
		vector.y = y;
		vector.z = 0f;
		return vector;
	}

	// Token: 0x06002866 RID: 10342 RVA: 0x000E89B2 File Offset: 0x000E6BB2
	public int GetNavigationCost(Navigator navigator, int cell)
	{
		return navigator.GetNavigationCost(cell, this.GetOffsets(cell));
	}

	// Token: 0x06002867 RID: 10343 RVA: 0x000E89C2 File Offset: 0x000E6BC2
	public int GetNavigationCost(Navigator navigator)
	{
		return this.GetNavigationCost(navigator, Grid.PosToCell(this));
	}

	// Token: 0x06002868 RID: 10344 RVA: 0x000E89D1 File Offset: 0x000E6BD1
	private void TransferDiseaseWithWorker(WorkerBase worker)
	{
		if (this == null || worker == null)
		{
			return;
		}
		Workable.TransferDiseaseWithWorker(base.gameObject, worker.gameObject);
	}

	// Token: 0x06002869 RID: 10345 RVA: 0x000E89F8 File Offset: 0x000E6BF8
	public static void TransferDiseaseWithWorker(GameObject workable, GameObject worker)
	{
		if (workable == null || worker == null)
		{
			return;
		}
		PrimaryElement component = workable.GetComponent<PrimaryElement>();
		if (component == null)
		{
			return;
		}
		PrimaryElement component2 = worker.GetComponent<PrimaryElement>();
		if (component2 == null)
		{
			return;
		}
		SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
		invalid.idx = component2.DiseaseIdx;
		invalid.count = (int)((float)component2.DiseaseCount * 0.33f);
		SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
		invalid2.idx = component.DiseaseIdx;
		invalid2.count = (int)((float)component.DiseaseCount * 0.33f);
		component2.ModifyDiseaseCount(-invalid.count, "Workable.TransferDiseaseWithWorker");
		component.ModifyDiseaseCount(-invalid2.count, "Workable.TransferDiseaseWithWorker");
		if (invalid.count > 0)
		{
			component.AddDisease(invalid.idx, invalid.count, "Workable.TransferDiseaseWithWorker");
		}
		if (invalid2.count > 0)
		{
			component2.AddDisease(invalid2.idx, invalid2.count, "Workable.TransferDiseaseWithWorker");
		}
	}

	// Token: 0x0600286A RID: 10346 RVA: 0x000E8AF0 File Offset: 0x000E6CF0
	public void SetShouldShowSkillPerkStatusItem(bool shouldItBeShown)
	{
		this.shouldShowSkillPerkStatusItem = shouldItBeShown;
		if (this.skillsUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.skillsUpdateHandle);
			this.skillsUpdateHandle = -1;
		}
		if (this.shouldShowSkillPerkStatusItem && !string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, Workable.UpdateStatusItemDispatcher, this);
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x0600286B RID: 10347 RVA: 0x000E8B5C File Offset: 0x000E6D5C
	public virtual bool InstantlyFinish(WorkerBase worker)
	{
		float num = worker.GetWorkable().WorkTimeRemaining;
		if (!float.IsInfinity(num))
		{
			worker.Work(num);
			return true;
		}
		DebugUtil.DevAssert(false, this.ToString() + " was asked to instantly finish but it has infinite work time! Override InstantlyFinish in your workable!", null);
		return false;
	}

	// Token: 0x0600286C RID: 10348 RVA: 0x000E8BA0 File Offset: 0x000E6DA0
	public virtual List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.trackUses)
		{
			Descriptor item = new Descriptor(string.Format(BUILDING.DETAILS.USE_COUNT, this.numberOfUses), string.Format(BUILDING.DETAILS.USE_COUNT_TOOLTIP, this.numberOfUses), Descriptor.DescriptorType.Detail, false);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x0600286D RID: 10349 RVA: 0x000E8C00 File Offset: 0x000E6E00
	public virtual BuildingFacade GetBuildingFacade()
	{
		return base.GetComponent<BuildingFacade>();
	}

	// Token: 0x0600286E RID: 10350 RVA: 0x000E8C08 File Offset: 0x000E6E08
	public virtual KAnimControllerBase GetAnimController()
	{
		return base.GetComponent<KAnimControllerBase>();
	}

	// Token: 0x0600286F RID: 10351 RVA: 0x000E8C10 File Offset: 0x000E6E10
	[ContextMenu("Refresh Reachability")]
	public void RefreshReachability()
	{
		if (this.offsetTracker != null)
		{
			this.offsetTracker.ForceRefresh();
		}
	}

	// Token: 0x04001798 RID: 6040
	public float workTime;

	// Token: 0x04001799 RID: 6041
	protected bool showProgressBar = true;

	// Token: 0x0400179A RID: 6042
	public bool alwaysShowProgressBar;

	// Token: 0x0400179B RID: 6043
	public bool surpressWorkerForceSync;

	// Token: 0x0400179C RID: 6044
	protected bool lightEfficiencyBonus = true;

	// Token: 0x0400179D RID: 6045
	protected Guid lightEfficiencyBonusStatusItemHandle;

	// Token: 0x0400179E RID: 6046
	public bool currentlyLit;

	// Token: 0x0400179F RID: 6047
	public Tag laboratoryEfficiencyBonusTagRequired = RoomConstraints.ConstraintTags.ScienceBuilding;

	// Token: 0x040017A0 RID: 6048
	private bool useLaboratoryEfficiencyBonus;

	// Token: 0x040017A1 RID: 6049
	protected Guid laboratoryEfficiencyBonusStatusItemHandle;

	// Token: 0x040017A2 RID: 6050
	private bool currentlyInLaboratory;

	// Token: 0x040017A3 RID: 6051
	protected StatusItem workerStatusItem;

	// Token: 0x040017A4 RID: 6052
	protected StatusItem workingStatusItem;

	// Token: 0x040017A5 RID: 6053
	protected Guid workStatusItemHandle;

	// Token: 0x040017A6 RID: 6054
	protected OffsetTracker offsetTracker;

	// Token: 0x040017A7 RID: 6055
	[SerializeField]
	protected string attributeConverterId;

	// Token: 0x040017A8 RID: 6056
	protected AttributeConverter attributeConverter;

	// Token: 0x040017A9 RID: 6057
	protected float minimumAttributeMultiplier = 0.5f;

	// Token: 0x040017AA RID: 6058
	public bool resetProgressOnStop;

	// Token: 0x040017AB RID: 6059
	protected bool shouldTransferDiseaseWithWorker = true;

	// Token: 0x040017AC RID: 6060
	[SerializeField]
	protected float attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;

	// Token: 0x040017AD RID: 6061
	[SerializeField]
	protected string skillExperienceSkillGroup;

	// Token: 0x040017AE RID: 6062
	[SerializeField]
	protected float skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;

	// Token: 0x040017AF RID: 6063
	public bool triggerWorkReactions = true;

	// Token: 0x040017B0 RID: 6064
	public ReportManager.ReportType reportType = ReportManager.ReportType.WorkTime;

	// Token: 0x040017B1 RID: 6065
	[SerializeField]
	[Tooltip("What layer does the dupe switch to when interacting with the building")]
	public Grid.SceneLayer workLayer = Grid.SceneLayer.Move;

	// Token: 0x040017B2 RID: 6066
	[SerializeField]
	[Serialize]
	protected float workTimeRemaining = float.PositiveInfinity;

	// Token: 0x040017B3 RID: 6067
	[SerializeField]
	public KAnimFile[] overrideAnims;

	// Token: 0x040017B4 RID: 6068
	[SerializeField]
	protected HashedString multitoolContext;

	// Token: 0x040017B5 RID: 6069
	[SerializeField]
	protected Tag multitoolHitEffectTag;

	// Token: 0x040017B6 RID: 6070
	[SerializeField]
	[Tooltip("Whether to user the KAnimSynchronizer or not")]
	public bool synchronizeAnims = true;

	// Token: 0x040017B7 RID: 6071
	[SerializeField]
	[Tooltip("Whether to display number of uses in the details panel")]
	public bool trackUses;

	// Token: 0x040017B8 RID: 6072
	[Serialize]
	protected int numberOfUses;

	// Token: 0x040017B9 RID: 6073
	public Action<Workable, Workable.WorkableEvent> OnWorkableEventCB;

	// Token: 0x040017BA RID: 6074
	protected int skillsUpdateHandle = -1;

	// Token: 0x040017BB RID: 6075
	private int minionUpdateHandle = -1;

	// Token: 0x040017BC RID: 6076
	public string requiredSkillPerk;

	// Token: 0x040017BD RID: 6077
	[SerializeField]
	protected bool shouldShowSkillPerkStatusItem = true;

	// Token: 0x040017BE RID: 6078
	[SerializeField]
	public bool requireMinionToWork;

	// Token: 0x040017BF RID: 6079
	protected StatusItem readyForSkillWorkStatusItem;

	// Token: 0x040017C0 RID: 6080
	public HashedString[] workAnims = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x040017C1 RID: 6081
	public HashedString[] workingPstComplete = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x040017C2 RID: 6082
	public HashedString[] workingPstFailed = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x040017C3 RID: 6083
	public KAnim.PlayMode workAnimPlayMode;

	// Token: 0x040017C4 RID: 6084
	public bool faceTargetWhenWorking;

	// Token: 0x040017C5 RID: 6085
	private static readonly EventSystem.IntraObjectHandler<Workable> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<Workable>(delegate(Workable component, object data)
	{
		component.OnUpdateRoom(data);
	});

	// Token: 0x040017C6 RID: 6086
	protected static Action<object, object> UpdateStatusItemDispatcher = delegate(object context, object data)
	{
		Unsafe.As<Workable>(context).UpdateStatusItem(data);
	};

	// Token: 0x040017C7 RID: 6087
	protected ProgressBar progressBar;

	// Token: 0x02001549 RID: 5449
	public enum WorkableEvent
	{
		// Token: 0x04007165 RID: 29029
		WorkStarted,
		// Token: 0x04007166 RID: 29030
		WorkCompleted,
		// Token: 0x04007167 RID: 29031
		WorkStopped
	}

	// Token: 0x0200154A RID: 5450
	public struct AnimInfo
	{
		// Token: 0x04007168 RID: 29032
		public KAnimFile[] overrideAnims;

		// Token: 0x04007169 RID: 29033
		public StateMachine.Instance smi;
	}
}
