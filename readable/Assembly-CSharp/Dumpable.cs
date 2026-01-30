using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000902 RID: 2306
[AddComponentMenu("KMonoBehaviour/Workable/Dumpable")]
public class Dumpable : Workable
{
	// Token: 0x06004010 RID: 16400 RVA: 0x001687C6 File Offset: 0x001669C6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Dumpable>(493375141, Dumpable.OnRefreshUserMenuDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
	}

	// Token: 0x06004011 RID: 16401 RVA: 0x001687F4 File Offset: 0x001669F4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForDumping)
		{
			this.CreateChore();
		}
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_dumpable_kanim")
		};
		this.workAnims = new HashedString[]
		{
			"working"
		};
		this.synchronizeAnims = false;
		base.SetWorkTime(1f);
	}

	// Token: 0x06004012 RID: 16402 RVA: 0x00168864 File Offset: 0x00166A64
	public void ToggleDumping()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.OnCompleteWork(null);
			return;
		}
		if (this.isMarkedForDumping)
		{
			this.isMarkedForDumping = false;
			this.chore.Cancel("Cancel Dumping!");
			Prioritizable.RemoveRef(base.gameObject);
			this.chore = null;
			base.ShowProgressBar(false);
			return;
		}
		this.isMarkedForDumping = true;
		this.CreateChore();
	}

	// Token: 0x06004013 RID: 16403 RVA: 0x001688C8 File Offset: 0x00166AC8
	private void CreateChore()
	{
		if (this.chore == null)
		{
			Prioritizable.AddRef(base.gameObject);
			this.chore = new WorkChore<Dumpable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}
	}

	// Token: 0x06004014 RID: 16404 RVA: 0x00168914 File Offset: 0x00166B14
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.isMarkedForDumping = false;
		this.chore = null;
		this.Dump();
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x06004015 RID: 16405 RVA: 0x00168935 File Offset: 0x00166B35
	public void Dump()
	{
		this.Dump(base.transform.GetPosition());
	}

	// Token: 0x06004016 RID: 16406 RVA: 0x00168948 File Offset: 0x00166B48
	public void Dump(Vector3 pos)
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		if (component.Mass > 0f)
		{
			if (component.Element.IsLiquid)
			{
				FallingWater.instance.AddParticle(Grid.PosToCell(pos), component.Element.idx, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, true, false, false, false);
			}
			else
			{
				SimMessages.AddRemoveSubstance(Grid.PosToCell(pos), component.ElementID, CellEventLogger.Instance.Dumpable, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, true, -1);
			}
		}
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06004017 RID: 16407 RVA: 0x001689F0 File Offset: 0x00166BF0
	private void OnRefreshUserMenu(object data)
	{
		if (this.HasTag(GameTags.Stored))
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = this.isMarkedForDumping ? new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.DUMP.NAME_OFF, new System.Action(this.ToggleDumping), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DUMP.TOOLTIP_OFF, true) : new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.DUMP.NAME, new System.Action(this.ToggleDumping), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DUMP.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x040027B4 RID: 10164
	private Chore chore;

	// Token: 0x040027B5 RID: 10165
	[Serialize]
	private bool isMarkedForDumping;

	// Token: 0x040027B6 RID: 10166
	private static readonly EventSystem.IntraObjectHandler<Dumpable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Dumpable>(delegate(Dumpable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
