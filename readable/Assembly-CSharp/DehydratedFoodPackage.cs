using System;
using System.Linq;
using FoodRehydrator;
using KSerialization;
using UnityEngine;

// Token: 0x020008D6 RID: 2262
public class DehydratedFoodPackage : Workable, IApproachable
{
	// Token: 0x17000463 RID: 1123
	// (get) Token: 0x06003F0A RID: 16138 RVA: 0x00161F74 File Offset: 0x00160174
	// (set) Token: 0x06003F0B RID: 16139 RVA: 0x00161FA3 File Offset: 0x001601A3
	public GameObject Rehydrator
	{
		get
		{
			Storage storage = base.gameObject.GetComponent<Pickupable>().storage;
			if (storage != null)
			{
				return storage.gameObject;
			}
			return null;
		}
		private set
		{
		}
	}

	// Token: 0x06003F0C RID: 16140 RVA: 0x00161FA5 File Offset: 0x001601A5
	public override BuildingFacade GetBuildingFacade()
	{
		if (!(this.Rehydrator != null))
		{
			return null;
		}
		return this.Rehydrator.GetComponent<BuildingFacade>();
	}

	// Token: 0x06003F0D RID: 16141 RVA: 0x00161FC2 File Offset: 0x001601C2
	public override KAnimControllerBase GetAnimController()
	{
		if (!(this.Rehydrator != null))
		{
			return null;
		}
		return this.Rehydrator.GetComponent<KAnimControllerBase>();
	}

	// Token: 0x06003F0E RID: 16142 RVA: 0x00161FE0 File Offset: 0x001601E0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetOffsets(new CellOffset[]
		{
			default(CellOffset),
			new CellOffset(0, -1)
		});
		if (this.storage.items.Count < 1)
		{
			this.storage.ConsumeAllIgnoringDisease(this.FoodTag);
			int cell = Grid.PosToCell(this);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.FoodTag), Grid.CellToPosCBC(cell, Grid.SceneLayer.Creatures), Grid.SceneLayer.Creatures, null, 0);
			gameObject.SetActive(true);
			gameObject.GetComponent<Edible>().Calories = 1000000f;
			this.storage.Store(gameObject, false, false, true, false);
		}
		base.Subscribe(-1697596308, new Action<object>(this.StorageChangeHandler));
		this.DehydrateItem(this.storage.items.ElementAtOrDefault(0));
	}

	// Token: 0x06003F0F RID: 16143 RVA: 0x001620AC File Offset: 0x001602AC
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (this.Rehydrator != null)
		{
			DehydratedManager component = this.Rehydrator.GetComponent<DehydratedManager>();
			if (component != null)
			{
				component.SetFabricatedFoodSymbol(this.FoodTag);
			}
			this.Rehydrator.GetComponent<AccessabilityManager>().SetActiveWorkable(this);
		}
	}

	// Token: 0x06003F10 RID: 16144 RVA: 0x00162100 File Offset: 0x00160300
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		if (this.storage.items.Count != 1)
		{
			DebugUtil.DevAssert(false, "OnCompleteWork invalid contents of package", null);
			return;
		}
		GameObject gameObject = this.storage.items[0];
		this.storage.Transfer(worker.GetComponent<Storage>(), false, false);
		DebugUtil.DevAssert(this.Rehydrator != null, "OnCompleteWork but no rehydrator", null);
		DehydratedManager component = this.Rehydrator.GetComponent<DehydratedManager>();
		this.Rehydrator.GetComponent<AccessabilityManager>().SetActiveWorkable(null);
		component.ConsumeResourcesForRehydration(base.gameObject, gameObject);
		DehydratedFoodPackage.RehydrateStartWorkItem rehydrateStartWorkItem = (DehydratedFoodPackage.RehydrateStartWorkItem)worker.GetStartWorkInfo();
		if (rehydrateStartWorkItem != null && rehydrateStartWorkItem.setResultCb != null && gameObject != null)
		{
			rehydrateStartWorkItem.setResultCb(gameObject);
		}
	}

	// Token: 0x06003F11 RID: 16145 RVA: 0x001621C4 File Offset: 0x001603C4
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (this.Rehydrator != null)
		{
			this.Rehydrator.GetComponent<AccessabilityManager>().SetActiveWorkable(null);
		}
	}

	// Token: 0x06003F12 RID: 16146 RVA: 0x001621EC File Offset: 0x001603EC
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003F13 RID: 16147 RVA: 0x001621F4 File Offset: 0x001603F4
	private void StorageChangeHandler(object obj)
	{
		GameObject item = (GameObject)obj;
		DebugUtil.DevAssert(!this.storage.items.Contains(item), "Attempting to add item to a dehydrated food package which is not allowed", null);
		this.RehydrateItem(item);
	}

	// Token: 0x06003F14 RID: 16148 RVA: 0x00162230 File Offset: 0x00160430
	public void DehydrateItem(GameObject item)
	{
		DebugUtil.DevAssert(item != null, "Attempting to dehydrate contents of an empty packet", null);
		if (this.storage.items.Count != 1 || item == null)
		{
			DebugUtil.DevAssert(false, "DehydrateItem called, incorrect content", null);
			return;
		}
		item.AddTag(GameTags.Dehydrated);
	}

	// Token: 0x06003F15 RID: 16149 RVA: 0x00162284 File Offset: 0x00160484
	public void RehydrateItem(GameObject item)
	{
		if (this.storage.items.Count != 0)
		{
			DebugUtil.DevAssert(false, "RehydrateItem called, incorrect storage content", null);
			return;
		}
		item.RemoveTag(GameTags.Dehydrated);
		item.AddTag(GameTags.Rehydrated);
		item.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.RehydratedFood, null);
	}

	// Token: 0x06003F16 RID: 16150 RVA: 0x001622E8 File Offset: 0x001604E8
	private void Swap<Type>(ref Type a, ref Type b)
	{
		Type type = a;
		a = b;
		b = type;
	}

	// Token: 0x0400273C RID: 10044
	[Serialize]
	public Tag FoodTag;

	// Token: 0x0400273D RID: 10045
	[MyCmpReq]
	private Storage storage;

	// Token: 0x020018EF RID: 6383
	public class RehydrateStartWorkItem : WorkerBase.StartWorkInfo
	{
		// Token: 0x0600A0CE RID: 41166 RVA: 0x003AA809 File Offset: 0x003A8A09
		public RehydrateStartWorkItem(DehydratedFoodPackage pkg, Action<GameObject> setResultCB) : base(pkg)
		{
			this.package = pkg;
			this.setResultCb = setResultCB;
		}

		// Token: 0x04007C7B RID: 31867
		public DehydratedFoodPackage package;

		// Token: 0x04007C7C RID: 31868
		public Action<GameObject> setResultCb;
	}
}
