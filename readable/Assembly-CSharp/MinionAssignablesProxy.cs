using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x020006E0 RID: 1760
[AddComponentMenu("KMonoBehaviour/scripts/MinionAssignablesProxy")]
public class MinionAssignablesProxy : KMonoBehaviour, IAssignableIdentity
{
	// Token: 0x1700020D RID: 525
	// (get) Token: 0x06002B5E RID: 11102 RVA: 0x000FD4BB File Offset: 0x000FB6BB
	// (set) Token: 0x06002B5F RID: 11103 RVA: 0x000FD4C3 File Offset: 0x000FB6C3
	public IAssignableIdentity target { get; private set; }

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06002B60 RID: 11104 RVA: 0x000FD4CC File Offset: 0x000FB6CC
	public bool IsConfigured
	{
		get
		{
			return this.slotsConfigured;
		}
	}

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06002B61 RID: 11105 RVA: 0x000FD4D4 File Offset: 0x000FB6D4
	public int TargetInstanceID
	{
		get
		{
			return this.target_instance_id;
		}
	}

	// Token: 0x06002B62 RID: 11106 RVA: 0x000FD4DC File Offset: 0x000FB6DC
	public GameObject GetTargetGameObject()
	{
		if (this.target == null && this.target_instance_id != -1)
		{
			this.RestoreTargetFromInstanceID();
		}
		KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)this.target;
		if (kmonoBehaviour != null)
		{
			return kmonoBehaviour.gameObject;
		}
		return null;
	}

	// Token: 0x06002B63 RID: 11107 RVA: 0x000FD520 File Offset: 0x000FB720
	public float GetArrivalTime()
	{
		if (this.GetTargetGameObject().GetComponent<MinionIdentity>() != null)
		{
			return this.GetTargetGameObject().GetComponent<MinionIdentity>().arrivalTime;
		}
		if (this.GetTargetGameObject().GetComponent<StoredMinionIdentity>() != null)
		{
			return this.GetTargetGameObject().GetComponent<StoredMinionIdentity>().arrivalTime;
		}
		global::Debug.LogError("Could not get minion arrival time");
		return -1f;
	}

	// Token: 0x06002B64 RID: 11108 RVA: 0x000FD584 File Offset: 0x000FB784
	public int GetTotalSkillpoints()
	{
		if (this.GetTargetGameObject().GetComponent<MinionIdentity>() != null)
		{
			return this.GetTargetGameObject().GetComponent<MinionResume>().TotalSkillPointsGained;
		}
		if (this.GetTargetGameObject().GetComponent<StoredMinionIdentity>() != null)
		{
			return MinionResume.CalculateTotalSkillPointsGained(this.GetTargetGameObject().GetComponent<StoredMinionIdentity>().TotalExperienceGained);
		}
		global::Debug.LogError("Could not get minion skill points time");
		return -1;
	}

	// Token: 0x06002B65 RID: 11109 RVA: 0x000FD5EC File Offset: 0x000FB7EC
	public Tag GetMinionModel()
	{
		MinionIdentity component = this.GetTargetGameObject().GetComponent<MinionIdentity>();
		if (component != null)
		{
			return component.model;
		}
		StoredMinionIdentity component2 = this.GetTargetGameObject().GetComponent<StoredMinionIdentity>();
		if (component2 != null)
		{
			return component2.model;
		}
		global::Debug.LogError("Could not get minion model");
		return Tag.Invalid;
	}

	// Token: 0x06002B66 RID: 11110 RVA: 0x000FD640 File Offset: 0x000FB840
	public void SetTarget(IAssignableIdentity target, GameObject targetGO)
	{
		global::Debug.Assert(target != null, "target was null");
		if (targetGO == null)
		{
			global::Debug.LogWarningFormat("{0} MinionAssignablesProxy.SetTarget {1}, {2}, {3}. DESTROYING", new object[]
			{
				base.GetInstanceID(),
				this.target_instance_id,
				target,
				targetGO
			});
			Util.KDestroyGameObject(base.gameObject);
		}
		this.target = target;
		this.target_instance_id = targetGO.GetComponent<KPrefabID>().InstanceID;
		base.gameObject.name = "Minion Assignables Proxy : " + targetGO.name;
	}

	// Token: 0x06002B67 RID: 11111 RVA: 0x000FD6D8 File Offset: 0x000FB8D8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.ownables = new List<Ownables>
		{
			base.gameObject.AddOrGet<Ownables>()
		};
		Components.MinionAssignablesProxy.Add(this);
		base.Subscribe<MinionAssignablesProxy>(1502190696, MinionAssignablesProxy.OnQueueDestroyObjectDelegate);
		this.ConfigureAssignableSlots();
	}

	// Token: 0x06002B68 RID: 11112 RVA: 0x000FD729 File Offset: 0x000FB929
	[OnDeserialized]
	private void OnDeserialized()
	{
	}

	// Token: 0x06002B69 RID: 11113 RVA: 0x000FD72C File Offset: 0x000FB92C
	public void ConfigureAssignableSlots()
	{
		if (this.slotsConfigured)
		{
			return;
		}
		Ownables component = base.GetComponent<Ownables>();
		Equipment component2 = base.GetComponent<Equipment>();
		if (component2 != null)
		{
			foreach (AssignableSlot assignableSlot in Db.Get().AssignableSlots.resources)
			{
				if (assignableSlot is OwnableSlot)
				{
					OwnableSlotInstance slot_instance = new OwnableSlotInstance(component, (OwnableSlot)assignableSlot);
					component.Add(slot_instance);
				}
				else if (assignableSlot is EquipmentSlot)
				{
					EquipmentSlotInstance slot_instance2 = new EquipmentSlotInstance(component2, (EquipmentSlot)assignableSlot);
					component2.Add(slot_instance2);
				}
			}
			BionicUpgradesMonitor.CreateAssignableSlots(this);
		}
		this.slotsConfigured = true;
	}

	// Token: 0x06002B6A RID: 11114 RVA: 0x000FD7EC File Offset: 0x000FB9EC
	public void RestoreTargetFromInstanceID()
	{
		if (this.target_instance_id != -1 && this.target == null)
		{
			KPrefabID instance = KPrefabIDTracker.Get().GetInstance(this.target_instance_id);
			if (instance)
			{
				IAssignableIdentity component = instance.GetComponent<IAssignableIdentity>();
				if (component != null)
				{
					this.SetTarget(component, instance.gameObject);
					return;
				}
				global::Debug.LogWarningFormat("RestoreTargetFromInstanceID target ID {0} was found but it wasn't an IAssignableIdentity, destroying proxy object.", new object[]
				{
					this.target_instance_id
				});
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
			else
			{
				global::Debug.LogWarningFormat("RestoreTargetFromInstanceID target ID {0} was not found, destroying proxy object.", new object[]
				{
					this.target_instance_id
				});
				Util.KDestroyGameObject(base.gameObject);
			}
		}
	}

	// Token: 0x06002B6B RID: 11115 RVA: 0x000FD894 File Offset: 0x000FBA94
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RestoreTargetFromInstanceID();
		if (this.target != null)
		{
			base.Subscribe<MinionAssignablesProxy>(-1585839766, MinionAssignablesProxy.OnAssignablesChangedDelegate);
			Game.Instance.assignmentManager.AddToAssignmentGroup("public", this);
		}
	}

	// Token: 0x06002B6C RID: 11116 RVA: 0x000FD8D0 File Offset: 0x000FBAD0
	private void OnQueueDestroyObject(object data)
	{
		Components.MinionAssignablesProxy.Remove(this);
	}

	// Token: 0x06002B6D RID: 11117 RVA: 0x000FD8DD File Offset: 0x000FBADD
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.assignmentManager.RemoveFromAllGroups(this);
		base.GetComponent<Ownables>().UnassignAll();
		base.GetComponent<Equipment>().UnequipAll();
	}

	// Token: 0x06002B6E RID: 11118 RVA: 0x000FD90B File Offset: 0x000FBB0B
	private void OnAssignablesChanged(object data)
	{
		if (!this.target.IsNull())
		{
			((KMonoBehaviour)this.target).Trigger(-1585839766, data);
		}
	}

	// Token: 0x06002B6F RID: 11119 RVA: 0x000FD930 File Offset: 0x000FBB30
	private void CheckTarget()
	{
		if (this.target == null)
		{
			KPrefabID instance = KPrefabIDTracker.Get().GetInstance(this.target_instance_id);
			if (instance != null)
			{
				this.target = instance.GetComponent<IAssignableIdentity>();
				if (this.target != null)
				{
					MinionIdentity minionIdentity = this.target as MinionIdentity;
					if (minionIdentity)
					{
						minionIdentity.ValidateProxy();
						return;
					}
					StoredMinionIdentity storedMinionIdentity = this.target as StoredMinionIdentity;
					if (storedMinionIdentity)
					{
						storedMinionIdentity.ValidateProxy();
					}
				}
			}
		}
	}

	// Token: 0x06002B70 RID: 11120 RVA: 0x000FD9A8 File Offset: 0x000FBBA8
	public List<Ownables> GetOwners()
	{
		this.CheckTarget();
		return this.target.GetOwners();
	}

	// Token: 0x06002B71 RID: 11121 RVA: 0x000FD9BB File Offset: 0x000FBBBB
	public string GetProperName()
	{
		this.CheckTarget();
		return this.target.GetProperName();
	}

	// Token: 0x06002B72 RID: 11122 RVA: 0x000FD9CE File Offset: 0x000FBBCE
	public Ownables GetSoleOwner()
	{
		this.CheckTarget();
		return this.target.GetSoleOwner();
	}

	// Token: 0x06002B73 RID: 11123 RVA: 0x000FD9E1 File Offset: 0x000FBBE1
	public bool HasOwner(Assignables owner)
	{
		this.CheckTarget();
		return this.target.HasOwner(owner);
	}

	// Token: 0x06002B74 RID: 11124 RVA: 0x000FD9F5 File Offset: 0x000FBBF5
	public int NumOwners()
	{
		this.CheckTarget();
		return this.target.NumOwners();
	}

	// Token: 0x06002B75 RID: 11125 RVA: 0x000FDA08 File Offset: 0x000FBC08
	public bool IsNull()
	{
		this.CheckTarget();
		return this.target.IsNull();
	}

	// Token: 0x06002B76 RID: 11126 RVA: 0x000FDA1C File Offset: 0x000FBC1C
	public static Ref<MinionAssignablesProxy> InitAssignableProxy(Ref<MinionAssignablesProxy> assignableProxyRef, IAssignableIdentity source)
	{
		if (assignableProxyRef == null)
		{
			assignableProxyRef = new Ref<MinionAssignablesProxy>();
		}
		GameObject gameObject = ((KMonoBehaviour)source).gameObject;
		MinionAssignablesProxy minionAssignablesProxy = assignableProxyRef.Get();
		if (minionAssignablesProxy == null)
		{
			GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab(MinionAssignablesProxyConfig.ID), Grid.SceneLayer.NoLayer, null, 0);
			minionAssignablesProxy = gameObject2.GetComponent<MinionAssignablesProxy>();
			minionAssignablesProxy.SetTarget(source, gameObject);
			gameObject2.SetActive(true);
			assignableProxyRef.Set(minionAssignablesProxy);
		}
		else
		{
			minionAssignablesProxy.SetTarget(source, gameObject);
		}
		return assignableProxyRef;
	}

	// Token: 0x040019E5 RID: 6629
	public List<Ownables> ownables;

	// Token: 0x040019E7 RID: 6631
	[Serialize]
	private int target_instance_id = -1;

	// Token: 0x040019E8 RID: 6632
	private bool slotsConfigured;

	// Token: 0x040019E9 RID: 6633
	private static readonly EventSystem.IntraObjectHandler<MinionAssignablesProxy> OnAssignablesChangedDelegate = new EventSystem.IntraObjectHandler<MinionAssignablesProxy>(delegate(MinionAssignablesProxy component, object data)
	{
		component.OnAssignablesChanged(data);
	});

	// Token: 0x040019EA RID: 6634
	private static readonly EventSystem.IntraObjectHandler<MinionAssignablesProxy> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<MinionAssignablesProxy>(delegate(MinionAssignablesProxy component, object data)
	{
		component.OnQueueDestroyObject(data);
	});
}
