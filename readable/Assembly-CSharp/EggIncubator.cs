using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x0200074B RID: 1867
[SerializationConfig(MemberSerialization.OptIn)]
public class EggIncubator : SingleEntityReceptacle, ISaveLoadable, ISim1000ms
{
	// Token: 0x06002F2D RID: 12077 RVA: 0x00110704 File Offset: 0x0010E904
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.autoReplaceEntity = true;
		this.choreType = Db.Get().ChoreTypes.RanchingFetch;
		this.statusItemNeed = Db.Get().BuildingStatusItems.NeedEgg;
		this.statusItemNoneAvailable = Db.Get().BuildingStatusItems.NoAvailableEgg;
		this.statusItemAwaitingDelivery = Db.Get().BuildingStatusItems.AwaitingEggDelivery;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
		this.occupyingObjectRelativePosition = new Vector3(0.5f, 1f, -1f);
		this.synchronizeAnims = false;
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("egg_target", false);
		this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		base.Subscribe<EggIncubator>(-905833192, EggIncubator.OnCopySettingsDelegate);
	}

	// Token: 0x06002F2E RID: 12078 RVA: 0x001107E8 File Offset: 0x0010E9E8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (base.occupyingObject)
		{
			if (base.occupyingObject.HasTag(GameTags.Creature))
			{
				this.storage.allowItemRemoval = true;
			}
			this.storage.RenotifyAll();
			this.PositionOccupyingObject();
		}
		base.Subscribe<EggIncubator>(-592767678, EggIncubator.OnOperationalChangedDelegate);
		base.Subscribe<EggIncubator>(-731304873, EggIncubator.OnOccupantChangedDelegate);
		base.Subscribe<EggIncubator>(-1697596308, EggIncubator.OnStorageChangeDelegate);
		this.smi = new EggIncubatorStates.Instance(this);
		this.smi.StartSM();
	}

	// Token: 0x06002F2F RID: 12079 RVA: 0x00110884 File Offset: 0x0010EA84
	private void OnCopySettings(object data)
	{
		EggIncubator component = ((GameObject)data).GetComponent<EggIncubator>();
		if (component != null)
		{
			this.autoReplaceEntity = component.autoReplaceEntity;
			if (base.occupyingObject == null)
			{
				if (!(this.requestedEntityTag == component.requestedEntityTag) || !(this.requestedEntityAdditionalFilterTag == component.requestedEntityAdditionalFilterTag))
				{
					base.CancelActiveRequest();
				}
				if (this.fetchChore == null)
				{
					Tag requestedEntityTag = component.requestedEntityTag;
					this.CreateOrder(requestedEntityTag, component.requestedEntityAdditionalFilterTag);
				}
			}
			if (base.occupyingObject != null)
			{
				Prioritizable component2 = base.GetComponent<Prioritizable>();
				if (component2 != null)
				{
					Prioritizable component3 = base.occupyingObject.GetComponent<Prioritizable>();
					if (component3 != null)
					{
						component3.SetMasterPriority(component2.GetMasterPriority());
					}
				}
			}
		}
	}

	// Token: 0x06002F30 RID: 12080 RVA: 0x0011094D File Offset: 0x0010EB4D
	protected override void OnCleanUp()
	{
		this.smi.StopSM("cleanup");
		base.OnCleanUp();
	}

	// Token: 0x06002F31 RID: 12081 RVA: 0x00110968 File Offset: 0x0010EB68
	protected override void SubscribeToOccupant()
	{
		base.SubscribeToOccupant();
		if (base.occupyingObject != null)
		{
			this.tracker = base.occupyingObject.AddComponent<KBatchedAnimTracker>();
			this.tracker.symbol = "egg_target";
			this.tracker.forceAlwaysVisible = true;
		}
		this.UpdateProgress();
	}

	// Token: 0x06002F32 RID: 12082 RVA: 0x001109C1 File Offset: 0x0010EBC1
	protected override void UnsubscribeFromOccupant()
	{
		base.UnsubscribeFromOccupant();
		UnityEngine.Object.Destroy(this.tracker);
		this.tracker = null;
		this.UpdateProgress();
	}

	// Token: 0x06002F33 RID: 12083 RVA: 0x001109E4 File Offset: 0x0010EBE4
	private new void OnOperationalChanged(object _ = null)
	{
		if (!base.occupyingObject)
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x06002F34 RID: 12084 RVA: 0x00110A16 File Offset: 0x0010EC16
	private void OnOccupantChanged(object _ = null)
	{
		if (!base.occupyingObject)
		{
			this.storage.allowItemRemoval = false;
		}
	}

	// Token: 0x06002F35 RID: 12085 RVA: 0x00110A31 File Offset: 0x0010EC31
	private void OnStorageChange(object _ = null)
	{
		if (base.occupyingObject && !this.storage.items.Contains(base.occupyingObject))
		{
			this.UnsubscribeFromOccupant();
			this.ClearOccupant();
		}
	}

	// Token: 0x06002F36 RID: 12086 RVA: 0x00110A64 File Offset: 0x0010EC64
	protected override void ClearOccupant()
	{
		bool flag = false;
		if (base.occupyingObject != null)
		{
			flag = !base.occupyingObject.HasTag(GameTags.Egg);
		}
		base.ClearOccupant();
		if (this.autoReplaceEntity && flag && this.requestedEntityTag.IsValid)
		{
			this.CreateOrder(this.requestedEntityTag, Tag.Invalid);
		}
	}

	// Token: 0x06002F37 RID: 12087 RVA: 0x00110AC4 File Offset: 0x0010ECC4
	protected override void PositionOccupyingObject()
	{
		base.PositionOccupyingObject();
		base.occupyingObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
		KSelectable component = base.occupyingObject.GetComponent<KSelectable>();
		if (component != null)
		{
			component.IsSelectable = true;
		}
	}

	// Token: 0x06002F38 RID: 12088 RVA: 0x00110B08 File Offset: 0x0010ED08
	public override void OrderRemoveOccupant()
	{
		UnityEngine.Object.Destroy(this.tracker);
		this.tracker = null;
		if (base.occupyingObject != null && base.occupyingObject.HasTag(GameTags.Egg))
		{
			this.requestedEntityTag = Tag.Invalid;
		}
		this.storage.DropAll(false, false, default(Vector3), true, null);
		base.occupyingObject = null;
		this.ClearOccupant();
	}

	// Token: 0x06002F39 RID: 12089 RVA: 0x00110B78 File Offset: 0x0010ED78
	public float GetProgress()
	{
		float result = 0f;
		if (base.occupyingObject)
		{
			AmountInstance amountInstance = base.occupyingObject.GetAmounts().Get(Db.Get().Amounts.Incubation);
			if (amountInstance != null)
			{
				result = amountInstance.value / amountInstance.GetMax();
			}
			else
			{
				result = 1f;
			}
		}
		return result;
	}

	// Token: 0x06002F3A RID: 12090 RVA: 0x00110BD2 File Offset: 0x0010EDD2
	private void UpdateProgress()
	{
		this.meter.SetPositionPercent(this.GetProgress());
	}

	// Token: 0x06002F3B RID: 12091 RVA: 0x00110BE5 File Offset: 0x0010EDE5
	public void Sim1000ms(float dt)
	{
		this.UpdateProgress();
		this.UpdateChore();
	}

	// Token: 0x06002F3C RID: 12092 RVA: 0x00110BF4 File Offset: 0x0010EDF4
	public void StoreBaby(GameObject baby)
	{
		this.UnsubscribeFromOccupant();
		this.storage.DropAll(false, false, default(Vector3), true, null);
		this.storage.allowItemRemoval = true;
		this.storage.Store(baby, false, false, true, false);
		base.occupyingObject = baby;
		this.SubscribeToOccupant();
		base.Trigger(-731304873, base.occupyingObject);
	}

	// Token: 0x06002F3D RID: 12093 RVA: 0x00110C5C File Offset: 0x0010EE5C
	private void UpdateChore()
	{
		if (this.operational.IsOperational && this.EggNeedsAttention())
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<EggIncubatorWorkable>(Db.Get().ChoreTypes.EggSing, this.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				return;
			}
		}
		else if (this.chore != null)
		{
			this.chore.Cancel("now is not the time for song");
			this.chore = null;
		}
	}

	// Token: 0x06002F3E RID: 12094 RVA: 0x00110CD8 File Offset: 0x0010EED8
	private bool EggNeedsAttention()
	{
		if (!base.Occupant)
		{
			return false;
		}
		IncubationMonitor.Instance instance = base.Occupant.GetSMI<IncubationMonitor.Instance>();
		return instance != null && !instance.HasSongBuff();
	}

	// Token: 0x04001BFF RID: 7167
	[MyCmpAdd]
	private EggIncubatorWorkable workable;

	// Token: 0x04001C00 RID: 7168
	[MyCmpAdd]
	private CopyBuildingSettings copySettings;

	// Token: 0x04001C01 RID: 7169
	private Chore chore;

	// Token: 0x04001C02 RID: 7170
	private EggIncubatorStates.Instance smi;

	// Token: 0x04001C03 RID: 7171
	private KBatchedAnimTracker tracker;

	// Token: 0x04001C04 RID: 7172
	private MeterController meter;

	// Token: 0x04001C05 RID: 7173
	private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<EggIncubator>(delegate(EggIncubator component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04001C06 RID: 7174
	private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnOccupantChangedDelegate = new EventSystem.IntraObjectHandler<EggIncubator>(delegate(EggIncubator component, object data)
	{
		component.OnOccupantChanged(data);
	});

	// Token: 0x04001C07 RID: 7175
	private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<EggIncubator>(delegate(EggIncubator component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04001C08 RID: 7176
	private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<EggIncubator>(delegate(EggIncubator component, object data)
	{
		component.OnCopySettings(data);
	});
}
