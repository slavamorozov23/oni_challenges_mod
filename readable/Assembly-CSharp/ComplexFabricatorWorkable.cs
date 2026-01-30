using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020005AC RID: 1452
[AddComponentMenu("KMonoBehaviour/Workable/ComplexFabricatorWorkable")]
public class ComplexFabricatorWorkable : Workable
{
	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06002128 RID: 8488 RVA: 0x000C004D File Offset: 0x000BE24D
	// (set) Token: 0x06002129 RID: 8489 RVA: 0x000C0055 File Offset: 0x000BE255
	public StatusItem WorkerStatusItem
	{
		get
		{
			return this.workerStatusItem;
		}
		set
		{
			this.workerStatusItem = value;
		}
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x0600212A RID: 8490 RVA: 0x000C005E File Offset: 0x000BE25E
	// (set) Token: 0x0600212B RID: 8491 RVA: 0x000C0066 File Offset: 0x000BE266
	public AttributeConverter AttributeConverter
	{
		get
		{
			return this.attributeConverter;
		}
		set
		{
			this.attributeConverter = value;
		}
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x0600212C RID: 8492 RVA: 0x000C006F File Offset: 0x000BE26F
	// (set) Token: 0x0600212D RID: 8493 RVA: 0x000C0077 File Offset: 0x000BE277
	public float AttributeExperienceMultiplier
	{
		get
		{
			return this.attributeExperienceMultiplier;
		}
		set
		{
			this.attributeExperienceMultiplier = value;
		}
	}

	// Token: 0x17000147 RID: 327
	// (set) Token: 0x0600212E RID: 8494 RVA: 0x000C0080 File Offset: 0x000BE280
	public string SkillExperienceSkillGroup
	{
		set
		{
			this.skillExperienceSkillGroup = value;
		}
	}

	// Token: 0x17000148 RID: 328
	// (set) Token: 0x0600212F RID: 8495 RVA: 0x000C0089 File Offset: 0x000BE289
	public float SkillExperienceMultiplier
	{
		set
		{
			this.skillExperienceMultiplier = value;
		}
	}

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06002130 RID: 8496 RVA: 0x000C0092 File Offset: 0x000BE292
	public ComplexRecipe CurrentWorkingOrder
	{
		get
		{
			if (!(this.fabricator != null))
			{
				return null;
			}
			return this.fabricator.CurrentWorkingOrder;
		}
	}

	// Token: 0x06002131 RID: 8497 RVA: 0x000C00B0 File Offset: 0x000BE2B0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
	}

	// Token: 0x06002132 RID: 8498 RVA: 0x000C0120 File Offset: 0x000BE320
	public override string GetConversationTopic()
	{
		string conversationTopic = this.fabricator.GetConversationTopic();
		if (conversationTopic == null)
		{
			return base.GetConversationTopic();
		}
		return conversationTopic;
	}

	// Token: 0x06002133 RID: 8499 RVA: 0x000C0144 File Offset: 0x000BE344
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (!this.operational.IsOperational)
		{
			return;
		}
		if (this.fabricator.CurrentWorkingOrder != null)
		{
			this.InstantiateVisualizer(this.fabricator.CurrentWorkingOrder);
			this.QueueWorkingAnimations();
			return;
		}
		DebugUtil.DevAssertArgs(false, new object[]
		{
			"ComplexFabricatorWorkable.OnStartWork called but CurrentMachineOrder is null",
			base.gameObject
		});
	}

	// Token: 0x06002134 RID: 8500 RVA: 0x000C01A8 File Offset: 0x000BE3A8
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.OnWorkTickActions != null)
		{
			this.OnWorkTickActions(worker, dt);
		}
		this.UpdateOrderProgress(worker, dt);
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06002135 RID: 8501 RVA: 0x000C01CF File Offset: 0x000BE3CF
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (worker != null && this.GetDupeInteract != null)
		{
			worker.GetAnimController().onAnimComplete -= this.PlayNextWorkingAnim;
		}
	}

	// Token: 0x06002136 RID: 8502 RVA: 0x000C0200 File Offset: 0x000BE400
	public override float GetWorkTime()
	{
		ComplexRecipe currentWorkingOrder = this.fabricator.CurrentWorkingOrder;
		if (currentWorkingOrder != null)
		{
			this.workTime = currentWorkingOrder.time;
			return this.workTime;
		}
		return -1f;
	}

	// Token: 0x06002137 RID: 8503 RVA: 0x000C0234 File Offset: 0x000BE434
	public Chore CreateWorkChore(ChoreType choreType, float order_progress)
	{
		Chore result = new WorkChore<ComplexFabricatorWorkable>(choreType, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.workTimeRemaining = this.GetWorkTime() * (1f - order_progress);
		return result;
	}

	// Token: 0x06002138 RID: 8504 RVA: 0x000C026D File Offset: 0x000BE46D
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.fabricator.CompleteWorkingOrder();
		this.DestroyVisualizer();
		base.OnStopWork(worker);
	}

	// Token: 0x06002139 RID: 8505 RVA: 0x000C0290 File Offset: 0x000BE490
	private void InstantiateVisualizer(ComplexRecipe recipe)
	{
		if (this.visualizer != null)
		{
			this.DestroyVisualizer();
		}
		if (this.visualizerLink != null)
		{
			this.visualizerLink.Unregister();
			this.visualizerLink = null;
		}
		if (recipe.FabricationVisualizer == null)
		{
			return;
		}
		this.visualizer = Util.KInstantiate(recipe.FabricationVisualizer, null, null);
		this.visualizer.transform.parent = this.meter.meterController.transform;
		this.visualizer.transform.SetLocalPosition(new Vector3(0f, 0f, 1f));
		this.visualizer.SetActive(true);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		KBatchedAnimController component2 = this.visualizer.GetComponent<KBatchedAnimController>();
		this.visualizerLink = new KAnimLink(component, component2);
	}

	// Token: 0x0600213A RID: 8506 RVA: 0x000C0360 File Offset: 0x000BE560
	private void UpdateOrderProgress(WorkerBase worker, float dt)
	{
		float workTime = this.GetWorkTime();
		float num = Mathf.Clamp01((workTime - base.WorkTimeRemaining) / workTime);
		if (this.fabricator)
		{
			this.fabricator.OrderProgress = num;
		}
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(num);
		}
	}

	// Token: 0x0600213B RID: 8507 RVA: 0x000C03B1 File Offset: 0x000BE5B1
	private void DestroyVisualizer()
	{
		if (this.visualizer != null)
		{
			if (this.visualizerLink != null)
			{
				this.visualizerLink.Unregister();
				this.visualizerLink = null;
			}
			Util.KDestroyGameObject(this.visualizer);
			this.visualizer = null;
		}
	}

	// Token: 0x0600213C RID: 8508 RVA: 0x000C03F0 File Offset: 0x000BE5F0
	public void QueueWorkingAnimations()
	{
		KBatchedAnimController animController = base.worker.GetAnimController();
		if (this.GetDupeInteract != null)
		{
			animController.Queue("working_loop", KAnim.PlayMode.Once, 1f, 0f);
			animController.onAnimComplete += this.PlayNextWorkingAnim;
		}
	}

	// Token: 0x0600213D RID: 8509 RVA: 0x000C0440 File Offset: 0x000BE640
	private void PlayNextWorkingAnim(HashedString anim)
	{
		if (base.worker == null)
		{
			return;
		}
		if (this.GetDupeInteract != null)
		{
			KBatchedAnimController animController = base.worker.GetAnimController();
			if (base.worker.GetState() == WorkerBase.State.Working)
			{
				animController.Play(this.GetDupeInteract(), KAnim.PlayMode.Once);
				return;
			}
			animController.onAnimComplete -= this.PlayNextWorkingAnim;
		}
	}

	// Token: 0x0400134C RID: 4940
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400134D RID: 4941
	[MyCmpReq]
	private ComplexFabricator fabricator;

	// Token: 0x0400134E RID: 4942
	public Action<WorkerBase, float> OnWorkTickActions;

	// Token: 0x0400134F RID: 4943
	public MeterController meter;

	// Token: 0x04001350 RID: 4944
	protected GameObject visualizer;

	// Token: 0x04001351 RID: 4945
	protected KAnimLink visualizerLink;

	// Token: 0x04001352 RID: 4946
	public Func<HashedString[]> GetDupeInteract;
}
