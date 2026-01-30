using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000833 RID: 2099
[AddComponentMenu("KMonoBehaviour/Workable/Butcherable")]
public class Butcherable : Workable, ISaveLoadable
{
	// Token: 0x0600393A RID: 14650 RVA: 0x0013FA64 File Offset: 0x0013DC64
	public void SetDrops(string[] drops)
	{
		Dictionary<string, float> dictionary = new Dictionary<string, float>();
		for (int i = 0; i < drops.Length; i++)
		{
			if (!dictionary.ContainsKey(drops[i]))
			{
				dictionary.Add(drops[i], 0f);
			}
			Dictionary<string, float> dictionary2 = dictionary;
			string key = drops[i];
			dictionary2[key] += 1f;
		}
		this.SetDrops(dictionary);
	}

	// Token: 0x0600393B RID: 14651 RVA: 0x0013FABF File Offset: 0x0013DCBF
	public void SetDrops(Dictionary<string, float> drops)
	{
		this.drops = drops;
	}

	// Token: 0x0600393C RID: 14652 RVA: 0x0013FAC8 File Offset: 0x0013DCC8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Butcherable>(1272413801, Butcherable.SetReadyToButcherDelegate);
		base.Subscribe<Butcherable>(493375141, Butcherable.OnRefreshUserMenuDelegate);
		this.workTime = 3f;
		this.multitoolContext = "harvest";
		this.multitoolHitEffectTag = "fx_harvest_splash";
	}

	// Token: 0x0600393D RID: 14653 RVA: 0x0013FB28 File Offset: 0x0013DD28
	public void SetReadyToButcher(object param)
	{
		this.readyToButcher = true;
	}

	// Token: 0x0600393E RID: 14654 RVA: 0x0013FB31 File Offset: 0x0013DD31
	public void SetReadyToButcher(bool ready)
	{
		this.readyToButcher = ready;
	}

	// Token: 0x0600393F RID: 14655 RVA: 0x0013FB3C File Offset: 0x0013DD3C
	public void ActivateChore(object param)
	{
		if (this.chore != null)
		{
			return;
		}
		this.chore = new WorkChore<Butcherable>(Db.Get().ChoreTypes.Harvest, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x06003940 RID: 14656 RVA: 0x0013FB85 File Offset: 0x0013DD85
	public void CancelChore(object param)
	{
		if (this.chore == null)
		{
			return;
		}
		this.chore.Cancel("User cancelled");
		this.chore = null;
	}

	// Token: 0x06003941 RID: 14657 RVA: 0x0013FBA7 File Offset: 0x0013DDA7
	private void OnClickCancel()
	{
		this.CancelChore(null);
	}

	// Token: 0x06003942 RID: 14658 RVA: 0x0013FBB0 File Offset: 0x0013DDB0
	private void OnClickButcher()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.OnButcherComplete();
			return;
		}
		this.ActivateChore(null);
	}

	// Token: 0x06003943 RID: 14659 RVA: 0x0013FBC8 File Offset: 0x0013DDC8
	private void OnRefreshUserMenu(object data)
	{
		if (!this.readyToButcher)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore != null) ? new KIconButtonMenu.ButtonInfo("action_harvest", "Cancel Meatify", new System.Action(this.OnClickCancel), global::Action.NumActions, null, null, null, "", true) : new KIconButtonMenu.ButtonInfo("action_harvest", "Meatify", new System.Action(this.OnClickButcher), global::Action.NumActions, null, null, null, "", true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06003944 RID: 14660 RVA: 0x0013FC56 File Offset: 0x0013DE56
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.OnButcherComplete();
	}

	// Token: 0x06003945 RID: 14661 RVA: 0x0013FC60 File Offset: 0x0013DE60
	public GameObject[] CreateDrops(float multiplier = 1f)
	{
		GameObject[] array = new GameObject[this.drops.Count];
		int num = 0;
		float temperature = base.GetComponent<PrimaryElement>().Temperature;
		foreach (KeyValuePair<string, float> keyValuePair in this.drops)
		{
			GameObject gameObject = Scenario.SpawnPrefab(this.GetDropSpawnLocation(), 0, 0, keyValuePair.Key, Grid.SceneLayer.Ore);
			gameObject.SetActive(true);
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.Mass = component.Mass * multiplier * keyValuePair.Value;
			component.Temperature = temperature;
			Edible component2 = gameObject.GetComponent<Edible>();
			if (component2)
			{
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component2.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.BUTCHERED, "{0}", gameObject.GetProperName()), UI.ENDOFDAYREPORT.NOTES.BUTCHERED_CONTEXT);
			}
			array[num] = gameObject;
			num++;
		}
		return array;
	}

	// Token: 0x06003946 RID: 14662 RVA: 0x0013FD68 File Offset: 0x0013DF68
	public void OnButcherComplete()
	{
		if (this.butchered)
		{
			return;
		}
		KSelectable component = base.GetComponent<KSelectable>();
		if (component && component.IsSelected)
		{
			SelectTool.Instance.Select(null, false);
		}
		Pickupable component2 = base.GetComponent<Pickupable>();
		Storage storage = (component2 != null) ? component2.storage : null;
		GameObject[] array = this.CreateDrops(1f);
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (storage != null && storage.storeDropsFromButcherables)
				{
					storage.Store(array[i], false, false, true, false);
				}
			}
		}
		this.chore = null;
		this.butchered = true;
		this.readyToButcher = false;
		Game.Instance.userMenu.Refresh(base.gameObject);
		base.Trigger(395373363, array);
	}

	// Token: 0x06003947 RID: 14663 RVA: 0x0013FE34 File Offset: 0x0013E034
	private int GetDropSpawnLocation()
	{
		int num = Grid.PosToCell(base.gameObject);
		int num2 = Grid.CellAbove(num);
		if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
		{
			return num2;
		}
		return num;
	}

	// Token: 0x040022EC RID: 8940
	[MyCmpGet]
	private KAnimControllerBase controller;

	// Token: 0x040022ED RID: 8941
	[MyCmpGet]
	private Harvestable harvestable;

	// Token: 0x040022EE RID: 8942
	private bool readyToButcher;

	// Token: 0x040022EF RID: 8943
	private bool butchered;

	// Token: 0x040022F0 RID: 8944
	public Dictionary<string, float> drops;

	// Token: 0x040022F1 RID: 8945
	private Chore chore;

	// Token: 0x040022F2 RID: 8946
	private static readonly EventSystem.IntraObjectHandler<Butcherable> SetReadyToButcherDelegate = new EventSystem.IntraObjectHandler<Butcherable>(delegate(Butcherable component, object data)
	{
		component.SetReadyToButcher(data);
	});

	// Token: 0x040022F3 RID: 8947
	private static readonly EventSystem.IntraObjectHandler<Butcherable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Butcherable>(delegate(Butcherable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
