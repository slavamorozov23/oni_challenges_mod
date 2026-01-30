using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020007F8 RID: 2040
[AddComponentMenu("KMonoBehaviour/Workable/SingleEntityReceptacle")]
public class SingleEntityReceptacle : Workable, IRender1000ms
{
	// Token: 0x17000382 RID: 898
	// (get) Token: 0x06003699 RID: 13977 RVA: 0x0013401F File Offset: 0x0013221F
	public FetchChore GetActiveRequest
	{
		get
		{
			return this.fetchChore;
		}
	}

	// Token: 0x17000383 RID: 899
	// (get) Token: 0x0600369A RID: 13978 RVA: 0x00134027 File Offset: 0x00132227
	// (set) Token: 0x0600369B RID: 13979 RVA: 0x0013404E File Offset: 0x0013224E
	protected GameObject occupyingObject
	{
		get
		{
			if (this.occupyObjectRef.Get() != null)
			{
				return this.occupyObjectRef.Get().gameObject;
			}
			return null;
		}
		set
		{
			if (value == null)
			{
				this.occupyObjectRef.Set(null);
				return;
			}
			this.occupyObjectRef.Set(value.GetComponent<KSelectable>());
		}
	}

	// Token: 0x17000384 RID: 900
	// (get) Token: 0x0600369C RID: 13980 RVA: 0x00134077 File Offset: 0x00132277
	public GameObject Occupant
	{
		get
		{
			return this.occupyingObject;
		}
	}

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x0600369D RID: 13981 RVA: 0x0013407F File Offset: 0x0013227F
	public IReadOnlyList<Tag> possibleDepositObjectTags
	{
		get
		{
			return this.possibleDepositTagsList;
		}
	}

	// Token: 0x0600369E RID: 13982 RVA: 0x00134087 File Offset: 0x00132287
	public bool HasDepositTag(Tag tag)
	{
		return this.possibleDepositTagsList.Contains(tag);
	}

	// Token: 0x0600369F RID: 13983 RVA: 0x00134098 File Offset: 0x00132298
	public bool IsValidEntity(GameObject candidate)
	{
		if (!Game.IsCorrectDlcActiveForCurrentSave(candidate.GetComponent<KPrefabID>()))
		{
			return false;
		}
		IReceptacleDirection component = candidate.GetComponent<IReceptacleDirection>();
		bool flag = this.rotatable != null || component == null || component.Direction == this.Direction;
		int num = 0;
		while (flag && num < this.additionalCriteria.Count)
		{
			flag = this.additionalCriteria[num](candidate);
			num++;
		}
		return flag;
	}

	// Token: 0x17000386 RID: 902
	// (get) Token: 0x060036A0 RID: 13984 RVA: 0x0013410B File Offset: 0x0013230B
	public SingleEntityReceptacle.ReceptacleDirection Direction
	{
		get
		{
			return this.direction;
		}
	}

	// Token: 0x060036A1 RID: 13985 RVA: 0x00134114 File Offset: 0x00132314
	protected Vector3 GetOccupyingObjectRelativePosition()
	{
		if (this.Occupant != null && SingleEntityReceptacle.CustomOccupyingObjectRelativePosition.ContainsKey(this.selfPrefabID))
		{
			KPrefabID component = this.Occupant.GetComponent<KPrefabID>();
			foreach (SingleEntityReceptacle.CustomPositionData customPositionData in SingleEntityReceptacle.CustomOccupyingObjectRelativePosition[this.selfPrefabID])
			{
				if (component.HasTag(customPositionData.tag))
				{
					return customPositionData.pos;
				}
			}
		}
		return this.occupyingObjectRelativePosition;
	}

	// Token: 0x060036A2 RID: 13986 RVA: 0x001341B8 File Offset: 0x001323B8
	protected override void OnPrefabInit()
	{
		this.selfPrefabID = base.gameObject.PrefabID();
		base.OnPrefabInit();
	}

	// Token: 0x060036A3 RID: 13987 RVA: 0x001341D4 File Offset: 0x001323D4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.occupyingObject != null)
		{
			this.PositionOccupyingObject();
			this.SubscribeToOccupant();
		}
		this.UpdateStatusItem();
		if (this.occupyingObject == null && !this.requestedEntityTag.IsValid)
		{
			this.requestedEntityAdditionalFilterTag = null;
		}
		if (this.occupyingObject == null && this.requestedEntityTag.IsValid)
		{
			this.CreateOrder(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag);
		}
		base.Subscribe<SingleEntityReceptacle>(-592767678, SingleEntityReceptacle.OnOperationalChangedDelegate);
		this.TriggerReceptacleOperationalSignal();
	}

	// Token: 0x060036A4 RID: 13988 RVA: 0x00134272 File Offset: 0x00132472
	public void AddDepositTag(Tag t)
	{
		this.possibleDepositTagsList.Add(t);
	}

	// Token: 0x060036A5 RID: 13989 RVA: 0x00134280 File Offset: 0x00132480
	public void AddAdditionalCriteria(Func<GameObject, bool> criteria)
	{
		this.additionalCriteria.Add(criteria);
	}

	// Token: 0x060036A6 RID: 13990 RVA: 0x0013428E File Offset: 0x0013248E
	public void SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection d)
	{
		this.direction = d;
	}

	// Token: 0x060036A7 RID: 13991 RVA: 0x00134297 File Offset: 0x00132497
	public virtual void SetPreview(Tag entityTag, bool solid = false)
	{
	}

	// Token: 0x060036A8 RID: 13992 RVA: 0x00134299 File Offset: 0x00132499
	public virtual void CreateOrder(Tag entityTag, Tag additionalFilterTag)
	{
		this.requestedEntityTag = entityTag;
		this.requestedEntityAdditionalFilterTag = additionalFilterTag;
		this.CreateFetchChore(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag);
		this.SetPreview(entityTag, true);
		this.UpdateStatusItem();
	}

	// Token: 0x060036A9 RID: 13993 RVA: 0x001342C9 File Offset: 0x001324C9
	public virtual void Render1000ms(float dt)
	{
		this.UpdateStatusItem();
	}

	// Token: 0x060036AA RID: 13994 RVA: 0x001342D1 File Offset: 0x001324D1
	protected void UpdateStatusItem()
	{
		this.UpdateStatusItem(base.GetComponent<KSelectable>());
	}

	// Token: 0x060036AB RID: 13995 RVA: 0x001342E0 File Offset: 0x001324E0
	protected virtual void UpdateStatusItem(KSelectable selectable)
	{
		if (this.Occupant != null)
		{
			selectable.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, null, null);
			return;
		}
		if (this.fetchChore == null)
		{
			selectable.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemNeed, null);
			return;
		}
		bool flag = this.fetchChore.fetcher != null;
		WorldContainer myWorld = this.GetMyWorld();
		if (!flag && myWorld != null)
		{
			foreach (Tag tag in this.fetchChore.tags)
			{
				if (myWorld.worldInventory.GetTotalAmount(tag, true) > 0f)
				{
					if (myWorld.worldInventory.GetTotalAmount(this.requestedEntityAdditionalFilterTag, true) > 0f || this.requestedEntityAdditionalFilterTag == Tag.Invalid)
					{
						flag = true;
						break;
					}
					break;
				}
			}
		}
		if (flag)
		{
			selectable.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemAwaitingDelivery, null);
			return;
		}
		selectable.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemNoneAvailable, null);
	}

	// Token: 0x060036AC RID: 13996 RVA: 0x0013442C File Offset: 0x0013262C
	protected void CreateFetchChore(Tag entityTag, Tag additionalRequiredTag)
	{
		if (this.fetchChore == null && entityTag.IsValid && entityTag != GameTags.Empty)
		{
			this.fetchChore = new FetchChore(this.choreType, this.storage, this.GetPrefabFetchMass(entityTag), new HashSet<Tag>
			{
				entityTag
			}, FetchChore.MatchCriteria.MatchID, (additionalRequiredTag.IsValid && additionalRequiredTag != GameTags.Empty) ? additionalRequiredTag : Tag.Invalid, null, null, true, new Action<Chore>(this.OnFetchComplete), delegate(Chore chore)
			{
				this.UpdateStatusItem();
			}, delegate(Chore chore)
			{
				this.UpdateStatusItem();
			}, Operational.State.Functional, 0);
			MaterialNeeds.UpdateNeed(this.requestedEntityTag, 1f, base.gameObject.GetMyWorldId());
			this.UpdateStatusItem();
		}
	}

	// Token: 0x060036AD RID: 13997 RVA: 0x001344F4 File Offset: 0x001326F4
	private float GetPrefabFetchMass(Tag entityTag)
	{
		GameObject prefab = Assets.GetPrefab(entityTag);
		if (prefab != null)
		{
			PrimaryElement component = prefab.GetComponent<PrimaryElement>();
			if (component != null)
			{
				return component.MassPerUnit;
			}
		}
		KCrashReporter.ReportDevNotification(string.Concat(new string[]
		{
			"SingleEntityReceptacle ",
			base.name,
			" is requesting ",
			entityTag.Name,
			" which is not an entity"
		}), Environment.StackTrace, "", false, null);
		return 1f;
	}

	// Token: 0x060036AE RID: 13998 RVA: 0x00134574 File Offset: 0x00132774
	public virtual void OrderRemoveOccupant()
	{
		this.ClearOccupant();
	}

	// Token: 0x060036AF RID: 13999 RVA: 0x0013457C File Offset: 0x0013277C
	protected virtual void ClearOccupant()
	{
		if (this.occupyingObject)
		{
			this.UnsubscribeFromOccupant();
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
		this.occupyingObject = null;
		this.UpdateActive();
		this.UpdateStatusItem();
		base.Trigger(-731304873, this.occupyingObject);
	}

	// Token: 0x060036B0 RID: 14000 RVA: 0x001345D8 File Offset: 0x001327D8
	public void CancelActiveRequest()
	{
		if (this.fetchChore != null)
		{
			MaterialNeeds.UpdateNeed(this.requestedEntityTag, -1f, base.gameObject.GetMyWorldId());
			this.fetchChore.Cancel("User canceled");
			this.fetchChore = null;
		}
		this.requestedEntityTag = Tag.Invalid;
		this.requestedEntityAdditionalFilterTag = Tag.Invalid;
		this.UpdateStatusItem();
		this.SetPreview(Tag.Invalid, false);
	}

	// Token: 0x060036B1 RID: 14001 RVA: 0x00134648 File Offset: 0x00132848
	private void OnOccupantDestroyed(object data)
	{
		this.occupyingObject = null;
		this.ClearOccupant();
		if (this.autoReplaceEntity && this.requestedEntityTag.IsValid && this.requestedEntityTag != GameTags.Empty)
		{
			this.CreateOrder(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag);
		}
	}

	// Token: 0x060036B2 RID: 14002 RVA: 0x0013469B File Offset: 0x0013289B
	protected virtual void SubscribeToOccupant()
	{
		if (this.occupyingObject != null)
		{
			base.Subscribe(this.occupyingObject, 1969584890, new Action<object>(this.OnOccupantDestroyed));
		}
	}

	// Token: 0x060036B3 RID: 14003 RVA: 0x001346C9 File Offset: 0x001328C9
	protected virtual void UnsubscribeFromOccupant()
	{
		if (this.occupyingObject != null)
		{
			base.Unsubscribe(this.occupyingObject, 1969584890, new Action<object>(this.OnOccupantDestroyed));
		}
	}

	// Token: 0x060036B4 RID: 14004 RVA: 0x001346F8 File Offset: 0x001328F8
	private void OnFetchComplete(Chore chore)
	{
		if (this.fetchChore == null)
		{
			global::Debug.LogWarningFormat(base.gameObject, "{0} OnFetchComplete fetchChore null", new object[]
			{
				base.gameObject
			});
			return;
		}
		if (this.fetchChore.fetchTarget == null)
		{
			global::Debug.LogWarningFormat(base.gameObject, "{0} OnFetchComplete fetchChore.fetchTarget null", new object[]
			{
				base.gameObject
			});
			return;
		}
		this.OnDepositObject(this.fetchChore.fetchTarget.gameObject);
	}

	// Token: 0x060036B5 RID: 14005 RVA: 0x00134776 File Offset: 0x00132976
	public void ForceDeposit(GameObject depositedObject)
	{
		if (this.occupyingObject != null)
		{
			this.ClearOccupant();
		}
		this.OnDepositObject(depositedObject);
	}

	// Token: 0x060036B6 RID: 14006 RVA: 0x00134794 File Offset: 0x00132994
	protected virtual void OnDepositObject(GameObject depositedObject)
	{
		this.SetPreview(Tag.Invalid, false);
		MaterialNeeds.UpdateNeed(this.requestedEntityTag, -1f, base.gameObject.GetMyWorldId());
		KBatchedAnimController component = depositedObject.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.GetBatchInstanceData().ClearOverrideTransformMatrix();
		}
		this.occupyingObject = this.SpawnOccupyingObject(depositedObject);
		if (this.occupyingObject != null)
		{
			this.ConfigureOccupyingObject(this.occupyingObject);
			this.occupyingObject.SetActive(true);
			this.PositionOccupyingObject();
			this.SubscribeToOccupant();
		}
		else
		{
			global::Debug.LogWarning(base.gameObject.name + " EntityReceptacle did not spawn occupying entity.");
		}
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("receptacle filled");
			this.fetchChore = null;
		}
		if (!this.autoReplaceEntity)
		{
			this.requestedEntityTag = Tag.Invalid;
			this.requestedEntityAdditionalFilterTag = Tag.Invalid;
		}
		this.UpdateActive();
		this.UpdateStatusItem();
		if (this.destroyEntityOnDeposit)
		{
			Util.KDestroyGameObject(depositedObject);
		}
		base.Trigger(-731304873, this.occupyingObject);
	}

	// Token: 0x060036B7 RID: 14007 RVA: 0x001348A6 File Offset: 0x00132AA6
	protected virtual GameObject SpawnOccupyingObject(GameObject depositedEntity)
	{
		return depositedEntity;
	}

	// Token: 0x060036B8 RID: 14008 RVA: 0x001348A9 File Offset: 0x00132AA9
	protected virtual void ConfigureOccupyingObject(GameObject source)
	{
	}

	// Token: 0x060036B9 RID: 14009 RVA: 0x001348AC File Offset: 0x00132AAC
	protected virtual void PositionOccupyingObject()
	{
		Vector3 vector = this.GetOccupyingObjectRelativePosition();
		if (this.rotatable != null)
		{
			this.occupyingObject.transform.SetPosition(base.gameObject.transform.GetPosition() + this.rotatable.GetRotatedOffset(vector));
		}
		else
		{
			this.occupyingObject.transform.SetPosition(base.gameObject.transform.GetPosition() + vector);
		}
		KBatchedAnimController component = this.occupyingObject.GetComponent<KBatchedAnimController>();
		component.enabled = false;
		component.enabled = true;
	}

	// Token: 0x060036BA RID: 14010 RVA: 0x00134944 File Offset: 0x00132B44
	protected void UpdateActive()
	{
		if (this.Equals(null) || this == null || base.gameObject.Equals(null) || base.gameObject == null)
		{
			return;
		}
		if (this.operational != null)
		{
			this.operational.SetActive(this.operational.IsOperational && this.occupyingObject != null, false);
		}
	}

	// Token: 0x060036BB RID: 14011 RVA: 0x001349B6 File Offset: 0x00132BB6
	protected override void OnCleanUp()
	{
		this.CancelActiveRequest();
		this.UnsubscribeFromOccupant();
		base.OnCleanUp();
	}

	// Token: 0x060036BC RID: 14012 RVA: 0x001349CA File Offset: 0x00132BCA
	protected virtual void OnOperationalChanged(object data)
	{
		this.UpdateActive();
		this.TriggerReceptacleOperationalSignal();
	}

	// Token: 0x060036BD RID: 14013 RVA: 0x001349D8 File Offset: 0x00132BD8
	private void TriggerReceptacleOperationalSignal()
	{
		if (this.operational == null)
		{
			return;
		}
		if (this.occupyingObject)
		{
			this.occupyingObject.Trigger(this.operational.IsOperational ? 1628751838 : 960378201, null);
		}
	}

	// Token: 0x04002128 RID: 8488
	[MyCmpGet]
	public Operational operational;

	// Token: 0x04002129 RID: 8489
	[MyCmpReq]
	protected Storage storage;

	// Token: 0x0400212A RID: 8490
	[MyCmpGet]
	public Rotatable rotatable;

	// Token: 0x0400212B RID: 8491
	protected FetchChore fetchChore;

	// Token: 0x0400212C RID: 8492
	public ChoreType choreType = Db.Get().ChoreTypes.Fetch;

	// Token: 0x0400212D RID: 8493
	[Serialize]
	public bool autoReplaceEntity;

	// Token: 0x0400212E RID: 8494
	[Serialize]
	public Tag requestedEntityTag;

	// Token: 0x0400212F RID: 8495
	[Serialize]
	public Tag requestedEntityAdditionalFilterTag;

	// Token: 0x04002130 RID: 8496
	[Serialize]
	protected Ref<KSelectable> occupyObjectRef = new Ref<KSelectable>();

	// Token: 0x04002131 RID: 8497
	[SerializeField]
	private List<Tag> possibleDepositTagsList = new List<Tag>();

	// Token: 0x04002132 RID: 8498
	[SerializeField]
	private List<Func<GameObject, bool>> additionalCriteria = new List<Func<GameObject, bool>>();

	// Token: 0x04002133 RID: 8499
	[SerializeField]
	protected bool destroyEntityOnDeposit;

	// Token: 0x04002134 RID: 8500
	[SerializeField]
	protected SingleEntityReceptacle.ReceptacleDirection direction;

	// Token: 0x04002135 RID: 8501
	public static Dictionary<Tag, List<SingleEntityReceptacle.CustomPositionData>> CustomOccupyingObjectRelativePosition = new Dictionary<Tag, List<SingleEntityReceptacle.CustomPositionData>>();

	// Token: 0x04002136 RID: 8502
	public Vector3 occupyingObjectRelativePosition = new Vector3(0f, 1f, 3f);

	// Token: 0x04002137 RID: 8503
	private Tag selfPrefabID;

	// Token: 0x04002138 RID: 8504
	protected StatusItem statusItemAwaitingDelivery;

	// Token: 0x04002139 RID: 8505
	protected StatusItem statusItemNeed;

	// Token: 0x0400213A RID: 8506
	protected StatusItem statusItemNoneAvailable;

	// Token: 0x0400213B RID: 8507
	private static readonly EventSystem.IntraObjectHandler<SingleEntityReceptacle> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SingleEntityReceptacle>(delegate(SingleEntityReceptacle component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x0200176F RID: 5999
	public struct CustomPositionData
	{
		// Token: 0x06009B39 RID: 39737 RVA: 0x00394274 File Offset: 0x00392474
		public CustomPositionData(Tag t, Vector3 p)
		{
			this.tag = t;
			this.pos = p;
		}

		// Token: 0x040077BE RID: 30654
		public Tag tag;

		// Token: 0x040077BF RID: 30655
		public Vector3 pos;
	}

	// Token: 0x02001770 RID: 6000
	public enum ReceptacleDirection
	{
		// Token: 0x040077C1 RID: 30657
		Top,
		// Token: 0x040077C2 RID: 30658
		Side,
		// Token: 0x040077C3 RID: 30659
		Bottom
	}
}
