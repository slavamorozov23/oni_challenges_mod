using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000620 RID: 1568
[AddComponentMenu("KMonoBehaviour/scripts/Prioritizable")]
public class Prioritizable : KMonoBehaviour
{
	// Token: 0x0600254B RID: 9547 RVA: 0x000D60FB File Offset: 0x000D42FB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Prioritizable>(-905833192, Prioritizable.OnCopySettingsDelegate);
	}

	// Token: 0x0600254C RID: 9548 RVA: 0x000D6114 File Offset: 0x000D4314
	private void OnCopySettings(object data)
	{
		Prioritizable component = ((GameObject)data).GetComponent<Prioritizable>();
		if (component != null)
		{
			this.SetMasterPriority(component.GetMasterPriority());
		}
	}

	// Token: 0x0600254D RID: 9549 RVA: 0x000D6144 File Offset: 0x000D4344
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.masterPriority != -2147483648)
		{
			this.masterPrioritySetting = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);
			this.masterPriority = int.MinValue;
		}
		PrioritySetting prioritySetting;
		if (SaveLoader.Instance.GameInfo.IsVersionExactly(7, 2) && Prioritizable.conversions.TryGetValue(this.masterPrioritySetting, out prioritySetting))
		{
			this.masterPrioritySetting = prioritySetting;
		}
	}

	// Token: 0x0600254E RID: 9550 RVA: 0x000D61A8 File Offset: 0x000D43A8
	protected override void OnSpawn()
	{
		if (this.onPriorityChanged != null)
		{
			this.onPriorityChanged(this.masterPrioritySetting);
		}
		this.RefreshHighPriorityNotification();
		this.RefreshTopPriorityOnWorld();
		Vector3 position = base.transform.GetPosition();
		Extents extents = new Extents((int)position.x, (int)position.y, 1, 1);
		this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(base.name, this, extents, GameScenePartitioner.Instance.prioritizableObjects, null);
		Components.Prioritizables.Add(this);
	}

	// Token: 0x0600254F RID: 9551 RVA: 0x000D622B File Offset: 0x000D442B
	public PrioritySetting GetMasterPriority()
	{
		return this.masterPrioritySetting;
	}

	// Token: 0x06002550 RID: 9552 RVA: 0x000D6234 File Offset: 0x000D4434
	public void SetMasterPriority(PrioritySetting priority)
	{
		if (!priority.Equals(this.masterPrioritySetting))
		{
			this.masterPrioritySetting = priority;
			if (this.onPriorityChanged != null)
			{
				this.onPriorityChanged(this.masterPrioritySetting);
			}
			this.RefreshTopPriorityOnWorld();
			this.RefreshHighPriorityNotification();
		}
	}

	// Token: 0x06002551 RID: 9553 RVA: 0x000D6287 File Offset: 0x000D4487
	private void RefreshTopPriorityOnWorld()
	{
		this.SetTopPriorityOnWorld(this.IsTopPriority());
	}

	// Token: 0x06002552 RID: 9554 RVA: 0x000D6298 File Offset: 0x000D4498
	private void SetTopPriorityOnWorld(bool state)
	{
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		if (Game.Instance == null || myWorld == null)
		{
			return;
		}
		if (state)
		{
			myWorld.AddTopPriorityPrioritizable(this);
			return;
		}
		myWorld.RemoveTopPriorityPrioritizable(this);
	}

	// Token: 0x06002553 RID: 9555 RVA: 0x000D62DA File Offset: 0x000D44DA
	public void AddRef()
	{
		this.refCount++;
		this.RefreshTopPriorityOnWorld();
		this.RefreshHighPriorityNotification();
	}

	// Token: 0x06002554 RID: 9556 RVA: 0x000D62F6 File Offset: 0x000D44F6
	public void RemoveRef()
	{
		this.refCount--;
		if (this.IsTopPriority() || this.refCount == 0)
		{
			this.SetTopPriorityOnWorld(false);
		}
		this.RefreshHighPriorityNotification();
	}

	// Token: 0x06002555 RID: 9557 RVA: 0x000D6323 File Offset: 0x000D4523
	public bool IsPrioritizable()
	{
		return this.refCount > 0;
	}

	// Token: 0x06002556 RID: 9558 RVA: 0x000D632E File Offset: 0x000D452E
	public bool IsTopPriority()
	{
		return this.masterPrioritySetting.priority_class == PriorityScreen.PriorityClass.topPriority && this.IsPrioritizable();
	}

	// Token: 0x06002557 RID: 9559 RVA: 0x000D6348 File Offset: 0x000D4548
	protected override void OnCleanUp()
	{
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		if (myWorld != null)
		{
			myWorld.RemoveTopPriorityPrioritizable(this);
		}
		else
		{
			global::Debug.LogWarning("World has been destroyed before prioritizable " + base.name);
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				worldContainer.RemoveTopPriorityPrioritizable(this);
			}
		}
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
		Components.Prioritizables.Remove(this);
	}

	// Token: 0x06002558 RID: 9560 RVA: 0x000D63F4 File Offset: 0x000D45F4
	public static void AddRef(GameObject go)
	{
		Prioritizable component = go.GetComponent<Prioritizable>();
		if (component != null)
		{
			component.AddRef();
		}
	}

	// Token: 0x06002559 RID: 9561 RVA: 0x000D6418 File Offset: 0x000D4618
	public static void RemoveRef(GameObject go)
	{
		Prioritizable component = go.GetComponent<Prioritizable>();
		if (component != null)
		{
			component.RemoveRef();
		}
	}

	// Token: 0x0600255A RID: 9562 RVA: 0x000D643C File Offset: 0x000D463C
	private void RefreshHighPriorityNotification()
	{
		bool flag = this.masterPrioritySetting.priority_class == PriorityScreen.PriorityClass.topPriority && this.IsPrioritizable();
		if (flag && this.highPriorityStatusItem == Guid.Empty)
		{
			this.highPriorityStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.EmergencyPriority, null);
			return;
		}
		if (!flag && this.highPriorityStatusItem != Guid.Empty)
		{
			this.highPriorityStatusItem = base.GetComponent<KSelectable>().RemoveStatusItem(this.highPriorityStatusItem, false);
		}
	}

	// Token: 0x040015D0 RID: 5584
	[SerializeField]
	[Serialize]
	private int masterPriority = int.MinValue;

	// Token: 0x040015D1 RID: 5585
	[SerializeField]
	[Serialize]
	private PrioritySetting masterPrioritySetting = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);

	// Token: 0x040015D2 RID: 5586
	public Action<PrioritySetting> onPriorityChanged;

	// Token: 0x040015D3 RID: 5587
	public bool showIcon = true;

	// Token: 0x040015D4 RID: 5588
	public Vector2 iconOffset;

	// Token: 0x040015D5 RID: 5589
	public float iconScale = 1f;

	// Token: 0x040015D6 RID: 5590
	[SerializeField]
	private int refCount;

	// Token: 0x040015D7 RID: 5591
	private static readonly EventSystem.IntraObjectHandler<Prioritizable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Prioritizable>(delegate(Prioritizable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040015D8 RID: 5592
	private static Dictionary<PrioritySetting, PrioritySetting> conversions = new Dictionary<PrioritySetting, PrioritySetting>
	{
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 1),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 4)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 2),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 5)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 3),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 6)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 4),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 7)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 5),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 8)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 1),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 6)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 2),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 7)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 3),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 8)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 4),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 9)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 5),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 9)
		}
	};

	// Token: 0x040015D9 RID: 5593
	private HandleVector<int>.Handle scenePartitionerEntry;

	// Token: 0x040015DA RID: 5594
	private Guid highPriorityStatusItem;
}
