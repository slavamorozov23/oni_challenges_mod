using System;
using System.Linq;
using KSerialization;
using UnityEngine;

// Token: 0x02000431 RID: 1073
public class SpecialCargoBayClusterReceptacle : SingleEntityReceptacle, IBaggedStateAnimationInstructions
{
	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06001626 RID: 5670 RVA: 0x0007E34C File Offset: 0x0007C54C
	public bool IsRocketOnGround
	{
		get
		{
			return base.gameObject.HasTag(GameTags.RocketOnGround);
		}
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x06001627 RID: 5671 RVA: 0x0007E35E File Offset: 0x0007C55E
	public bool IsRocketInSpace
	{
		get
		{
			return base.gameObject.HasTag(GameTags.RocketInSpace);
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x06001628 RID: 5672 RVA: 0x0007E370 File Offset: 0x0007C570
	private bool isDoorOpen
	{
		get
		{
			return this.capsule.sm.IsDoorOpen.Get(this.capsule);
		}
	}

	// Token: 0x06001629 RID: 5673 RVA: 0x0007E38D File Offset: 0x0007C58D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreType = Db.Get().ChoreTypes.CreatureFetch;
	}

	// Token: 0x0600162A RID: 5674 RVA: 0x0007E3AC File Offset: 0x0007C5AC
	protected override void OnSpawn()
	{
		this.capsule = base.gameObject.GetSMI<SpecialCargoBayCluster.Instance>();
		this.SetupLootSymbolObject();
		base.OnSpawn();
		this.SetTrappedCritterAnimations(base.Occupant);
		base.Subscribe(-1697596308, new Action<object>(this.OnCritterStorageChanged));
		base.Subscribe<SpecialCargoBayClusterReceptacle>(-887025858, SpecialCargoBayClusterReceptacle.OnRocketLandedDelegate);
		base.Subscribe<SpecialCargoBayClusterReceptacle>(-1447108533, SpecialCargoBayClusterReceptacle.OnCargoBayRelocatedDelegate);
		base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
	}

	// Token: 0x0600162B RID: 5675 RVA: 0x0007E434 File Offset: 0x0007C634
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject != null)
		{
			SpecialCargoBayClusterReceptacle component = gameObject.GetComponent<SpecialCargoBayClusterReceptacle>();
			if (component != null)
			{
				Tag tag = (component.Occupant != null) ? component.Occupant.PrefabID() : component.requestedEntityTag;
				if (base.Occupant != null && base.Occupant.PrefabID() != tag)
				{
					this.ClearOccupant();
				}
				if (tag != this.requestedEntityTag && this.fetchChore != null)
				{
					base.CancelActiveRequest();
				}
				if (tag != Tag.Invalid)
				{
					this.CreateOrder(tag, component.requestedEntityAdditionalFilterTag);
				}
			}
		}
	}

	// Token: 0x0600162C RID: 5676 RVA: 0x0007E4E3 File Offset: 0x0007C6E3
	public override void CreateOrder(Tag entityTag, Tag additionalFilterTag)
	{
		base.CreateOrder(entityTag, additionalFilterTag);
		if (this.fetchChore != null)
		{
			this.fetchChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		}
	}

	// Token: 0x0600162D RID: 5677 RVA: 0x0007E50C File Offset: 0x0007C70C
	public void SetupLootSymbolObject()
	{
		Vector3 storePositionForDrops = this.capsule.GetStorePositionForDrops();
		storePositionForDrops.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
		GameObject gameObject = new GameObject();
		gameObject.name = "lootSymbol";
		gameObject.transform.SetParent(base.transform, true);
		gameObject.SetActive(false);
		gameObject.transform.SetPosition(storePositionForDrops);
		KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddOrGet<KBatchedAnimTracker>();
		kbatchedAnimTracker.symbol = "loot";
		kbatchedAnimTracker.forceAlwaysAlive = true;
		kbatchedAnimTracker.matchParentOffset = true;
		this.lootKBAC = gameObject.AddComponent<KBatchedAnimController>();
		this.lootKBAC.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("mushbar_kanim")
		};
		this.lootKBAC.initialAnim = "object";
		this.buildingAnimCtr.SetSymbolVisiblity("loot", false);
	}

	// Token: 0x0600162E RID: 5678 RVA: 0x0007E5E4 File Offset: 0x0007C7E4
	protected override void ClearOccupant()
	{
		this.LastCritterDead = null;
		if (base.occupyingObject != null)
		{
			this.UnsubscribeFromOccupant();
		}
		this.originWorldID = -1;
		base.occupyingObject = null;
		base.UpdateActive();
		base.UpdateStatusItem();
		if (!this.isDoorOpen)
		{
			if (this.IsRocketOnGround)
			{
				this.SetLootSymbolImage(Tag.Invalid);
				this.capsule.OpenDoor();
			}
		}
		else
		{
			this.capsule.DropInventory();
		}
		base.Trigger(-731304873, base.occupyingObject);
	}

	// Token: 0x0600162F RID: 5679 RVA: 0x0007E66A File Offset: 0x0007C86A
	private void OnCritterStorageChanged(object obj)
	{
		if (obj != null && this.storage.MassStored() == 0f && base.Occupant != null && base.Occupant == (GameObject)obj)
		{
			this.ClearOccupant();
		}
	}

	// Token: 0x06001630 RID: 5680 RVA: 0x0007E6A8 File Offset: 0x0007C8A8
	protected override void SubscribeToOccupant()
	{
		base.SubscribeToOccupant();
		base.Subscribe(base.Occupant, -1582839653, new Action<object>(this.OnTrappedCritterTagsChanged));
		base.Subscribe(base.Occupant, 395373363, new Action<object>(this.OnCreatureInStorageDied));
		base.Subscribe(base.Occupant, 663420073, new Action<object>(this.OnBabyInStorageGrows));
		this.SetupCritterTracker();
		for (int i = 0; i < SpecialCargoBayClusterReceptacle.tagsForCritter.Length; i++)
		{
			Tag tag = SpecialCargoBayClusterReceptacle.tagsForCritter[i];
			base.Occupant.AddTag(tag);
		}
		base.Occupant.GetComponent<Health>().UpdateHealthBar();
	}

	// Token: 0x06001631 RID: 5681 RVA: 0x0007E758 File Offset: 0x0007C958
	protected override void UnsubscribeFromOccupant()
	{
		base.UnsubscribeFromOccupant();
		base.Unsubscribe(base.Occupant, -1582839653, new Action<object>(this.OnTrappedCritterTagsChanged));
		base.Unsubscribe(base.Occupant, 395373363, new Action<object>(this.OnCreatureInStorageDied));
		base.Unsubscribe(base.Occupant, 663420073, new Action<object>(this.OnBabyInStorageGrows));
		this.RemoveCritterTracker();
		if (base.Occupant != null)
		{
			for (int i = 0; i < SpecialCargoBayClusterReceptacle.tagsForCritter.Length; i++)
			{
				Tag tag = SpecialCargoBayClusterReceptacle.tagsForCritter[i];
				base.occupyingObject.RemoveTag(tag);
			}
			base.occupyingObject.GetComponent<Health>().UpdateHealthBar();
		}
	}

	// Token: 0x06001632 RID: 5682 RVA: 0x0007E810 File Offset: 0x0007CA10
	public void SetLootSymbolImage(Tag productTag)
	{
		bool flag = productTag != Tag.Invalid;
		this.lootKBAC.gameObject.SetActive(flag);
		if (flag)
		{
			GameObject prefab = Assets.GetPrefab(productTag.ToString());
			this.lootKBAC.SwapAnims(prefab.GetComponent<KBatchedAnimController>().AnimFiles);
			this.lootKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
		}
	}

	// Token: 0x06001633 RID: 5683 RVA: 0x0007E88B File Offset: 0x0007CA8B
	private void SetupCritterTracker()
	{
		if (base.Occupant != null)
		{
			KBatchedAnimTracker kbatchedAnimTracker = base.Occupant.AddOrGet<KBatchedAnimTracker>();
			kbatchedAnimTracker.symbol = "critter";
			kbatchedAnimTracker.forceAlwaysAlive = true;
			kbatchedAnimTracker.matchParentOffset = true;
		}
	}

	// Token: 0x06001634 RID: 5684 RVA: 0x0007E8C4 File Offset: 0x0007CAC4
	private void RemoveCritterTracker()
	{
		if (base.Occupant != null)
		{
			KBatchedAnimTracker component = base.Occupant.GetComponent<KBatchedAnimTracker>();
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
		}
	}

	// Token: 0x06001635 RID: 5685 RVA: 0x0007E8FA File Offset: 0x0007CAFA
	protected override void ConfigureOccupyingObject(GameObject source)
	{
		this.originWorldID = source.GetMyWorldId();
		source.GetComponent<Baggable>().SetWrangled();
		this.SetTrappedCritterAnimations(source);
	}

	// Token: 0x06001636 RID: 5686 RVA: 0x0007E91C File Offset: 0x0007CB1C
	private void OnBabyInStorageGrows(object obj)
	{
		int num = this.originWorldID;
		this.UnsubscribeFromOccupant();
		GameObject gameObject = (GameObject)obj;
		this.storage.Store(gameObject, false, false, true, false);
		base.occupyingObject = gameObject;
		this.ConfigureOccupyingObject(gameObject);
		this.originWorldID = num;
		this.PositionOccupyingObject();
		this.SubscribeToOccupant();
		base.UpdateStatusItem();
	}

	// Token: 0x06001637 RID: 5687 RVA: 0x0007E978 File Offset: 0x0007CB78
	private void OnTrappedCritterTagsChanged(object obj)
	{
		if (base.Occupant != null && base.Occupant.HasTag(GameTags.Creatures.Die) && this.LastCritterDead != base.Occupant)
		{
			this.capsule.PlayDeathCloud();
			this.LastCritterDead = base.Occupant;
			this.RemoveCritterTracker();
			base.Occupant.GetComponent<KBatchedAnimController>().SetVisiblity(false);
			Butcherable component = base.Occupant.GetComponent<Butcherable>();
			if (component != null && component.drops != null && component.drops.Count > 0)
			{
				this.SetLootSymbolImage(component.drops.Keys.ToList<string>()[0].ToTag());
			}
			else
			{
				this.SetLootSymbolImage(Tag.Invalid);
			}
			if (this.IsRocketInSpace)
			{
				DeathStates.Instance smi = base.Occupant.GetSMI<DeathStates.Instance>();
				smi.GoTo(smi.sm.pst);
			}
		}
	}

	// Token: 0x06001638 RID: 5688 RVA: 0x0007EA6C File Offset: 0x0007CC6C
	private void OnCreatureInStorageDied(object drops_obj)
	{
		GameObject[] array = drops_obj as GameObject[];
		if (array != null)
		{
			foreach (GameObject go in array)
			{
				this.sideProductStorage.Store(go, false, false, true, false);
			}
		}
	}

	// Token: 0x06001639 RID: 5689 RVA: 0x0007EAA6 File Offset: 0x0007CCA6
	private void SetTrappedCritterAnimations(GameObject critter)
	{
		if (critter != null)
		{
			KBatchedAnimController component = critter.GetComponent<KBatchedAnimController>();
			component.FlipX = false;
			component.Play("rocket_biological", KAnim.PlayMode.Loop, 1f, 0f);
			component.enabled = false;
			component.enabled = true;
		}
	}

	// Token: 0x0600163A RID: 5690 RVA: 0x0007EAE6 File Offset: 0x0007CCE6
	protected override void PositionOccupyingObject()
	{
		if (base.Occupant != null)
		{
			base.Occupant.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
			this.SetupCritterTracker();
		}
	}

	// Token: 0x0600163B RID: 5691 RVA: 0x0007EB10 File Offset: 0x0007CD10
	protected override void UpdateStatusItem(KSelectable selectable)
	{
		bool flag = base.Occupant != null;
		if (selectable != null)
		{
			if (flag)
			{
				selectable.AddStatusItem(Db.Get().BuildingStatusItems.SpecialCargoBayClusterCritterStored, this);
			}
			else
			{
				selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.SpecialCargoBayClusterCritterStored, false);
			}
		}
		base.UpdateStatusItem(selectable);
	}

	// Token: 0x0600163C RID: 5692 RVA: 0x0007EB6D File Offset: 0x0007CD6D
	private void OnCargoBayRelocated(object data)
	{
		if (base.Occupant != null)
		{
			KBatchedAnimController component = base.Occupant.GetComponent<KBatchedAnimController>();
			component.enabled = false;
			component.enabled = true;
		}
	}

	// Token: 0x0600163D RID: 5693 RVA: 0x0007EB98 File Offset: 0x0007CD98
	private void OnRocketLanded(object data)
	{
		if (base.Occupant != null)
		{
			ClusterManager.Instance.MigrateCritter(base.Occupant, base.gameObject.GetMyWorldId(), this.originWorldID);
			this.originWorldID = base.Occupant.GetMyWorldId();
		}
		if (base.Occupant == null && !this.isDoorOpen)
		{
			this.SetLootSymbolImage(Tag.Invalid);
			if (this.sideProductStorage.MassStored() > 0f)
			{
				this.capsule.OpenDoor();
			}
		}
	}

	// Token: 0x0600163E RID: 5694 RVA: 0x0007EC23 File Offset: 0x0007CE23
	public string GetBaggedAnimationName()
	{
		return "rocket_biological";
	}

	// Token: 0x04000D2C RID: 3372
	public const string TRAPPED_CRITTER_ANIM_NAME = "rocket_biological";

	// Token: 0x04000D2D RID: 3373
	[MyCmpReq]
	private SymbolOverrideController symbolOverrideComponent;

	// Token: 0x04000D2E RID: 3374
	[MyCmpGet]
	private KBatchedAnimController buildingAnimCtr;

	// Token: 0x04000D2F RID: 3375
	private KBatchedAnimController lootKBAC;

	// Token: 0x04000D30 RID: 3376
	public Storage sideProductStorage;

	// Token: 0x04000D31 RID: 3377
	private SpecialCargoBayCluster.Instance capsule;

	// Token: 0x04000D32 RID: 3378
	private GameObject LastCritterDead;

	// Token: 0x04000D33 RID: 3379
	[Serialize]
	private int originWorldID;

	// Token: 0x04000D34 RID: 3380
	private static Tag[] tagsForCritter = new Tag[]
	{
		GameTags.Creatures.TrappedInCargoBay,
		GameTags.Creatures.PausedHunger,
		GameTags.Creatures.PausedReproduction,
		GameTags.Creatures.PreventGrowAnimation,
		GameTags.HideHealthBar,
		GameTags.PreventDeadAnimation
	};

	// Token: 0x04000D35 RID: 3381
	private static readonly EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle>(delegate(SpecialCargoBayClusterReceptacle component, object data)
	{
		component.OnRocketLanded(data);
	});

	// Token: 0x04000D36 RID: 3382
	private static readonly EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle> OnCargoBayRelocatedDelegate = new EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle>(delegate(SpecialCargoBayClusterReceptacle component, object data)
	{
		component.OnCargoBayRelocated(data);
	});
}
