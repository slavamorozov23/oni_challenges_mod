using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B45 RID: 2885
public class SetLocker : StateMachineComponent<SetLocker.StatesInstance>, ISidescreenButtonControl
{
	// Token: 0x060054DC RID: 21724 RVA: 0x001EF527 File Offset: 0x001ED727
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060054DD RID: 21725 RVA: 0x001EF52F File Offset: 0x001ED72F
	public void ChooseContents()
	{
		this.contents = this.possible_contents_ids[UnityEngine.Random.Range(0, this.possible_contents_ids.GetLength(0))];
	}

	// Token: 0x060054DE RID: 21726 RVA: 0x001EF550 File Offset: 0x001ED750
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.contents == null)
		{
			this.ChooseContents();
		}
		else
		{
			string[] array = this.contents;
			for (int i = 0; i < array.Length; i++)
			{
				if (Assets.GetPrefab(array[i]) == null)
				{
					this.ChooseContents();
					break;
				}
			}
		}
		if (this.pendingRummage)
		{
			this.ActivateChore(null);
		}
	}

	// Token: 0x060054DF RID: 21727 RVA: 0x001EF5C0 File Offset: 0x001ED7C0
	public void DropContents()
	{
		if (this.contents == null)
		{
			return;
		}
		if (DlcManager.IsExpansion1Active() && this.numDataBanks.Length >= 2)
		{
			int num = UnityEngine.Random.Range(this.numDataBanks[0], this.numDataBanks[1]);
			for (int i = 0; i <= num; i++)
			{
				Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), this.dropOffset.x, this.dropOffset.y, "OrbitalResearchDatabank", Grid.SceneLayer.Front).SetActive(true);
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Assets.GetPrefab("OrbitalResearchDatabank".ToTag()).GetProperName(), base.smi.master.transform, 1.5f, false);
			}
		}
		for (int j = 0; j < this.contents.Length; j++)
		{
			GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), this.dropOffset.x, this.dropOffset.y, this.contents[j], Grid.SceneLayer.Front);
			if (gameObject != null)
			{
				gameObject.SetActive(true);
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Assets.GetPrefab(this.contents[j].ToTag()).GetProperName(), base.smi.master.transform, 1.5f, false);
			}
		}
		base.gameObject.Trigger(-372600542, this);
	}

	// Token: 0x060054E0 RID: 21728 RVA: 0x001EF72F File Offset: 0x001ED92F
	private void OnClickOpen()
	{
		this.ActivateChore(null);
	}

	// Token: 0x060054E1 RID: 21729 RVA: 0x001EF738 File Offset: 0x001ED938
	private void OnClickCancel()
	{
		this.CancelChore(null);
	}

	// Token: 0x060054E2 RID: 21730 RVA: 0x001EF744 File Offset: 0x001ED944
	public void ActivateChore(object param = null)
	{
		if (this.chore != null)
		{
			return;
		}
		Prioritizable.AddRef(base.gameObject);
		base.Trigger(1980521255, null);
		this.pendingRummage = true;
		base.GetComponent<Workable>().SetWorkTime(1.5f);
		this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, delegate(Chore o)
		{
			this.CompleteChore();
		}, delegate(Chore o)
		{
			base.smi.GoTo(base.smi.sm.being_worked);
		}, delegate(Chore o)
		{
			this.OnChoreEnd();
		}, true, null, false, true, Assets.GetAnim(this.overrideAnim), false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x060054E3 RID: 21731 RVA: 0x001EF7E4 File Offset: 0x001ED9E4
	public void CancelChore(object param = null)
	{
		if (this.chore == null)
		{
			return;
		}
		this.pendingRummage = false;
		Prioritizable.RemoveRef(base.gameObject);
		base.Trigger(1980521255, null);
		this.chore.Cancel("User cancelled");
		this.chore = null;
	}

	// Token: 0x060054E4 RID: 21732 RVA: 0x001EF824 File Offset: 0x001EDA24
	private void OnChoreEnd()
	{
		if (this.skipAnim && this.chore != null)
		{
			base.smi.GoTo(base.smi.sm.closed);
		}
	}

	// Token: 0x060054E5 RID: 21733 RVA: 0x001EF854 File Offset: 0x001EDA54
	private void CompleteChore()
	{
		this.used = true;
		if (this.skipAnim)
		{
			this.DropContents();
			base.smi.GoTo(base.smi.sm.off);
		}
		else
		{
			base.smi.GoTo(base.smi.sm.open);
		}
		this.chore = null;
		this.pendingRummage = false;
		Game.Instance.userMenu.Refresh(base.gameObject);
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x170005EE RID: 1518
	// (get) Token: 0x060054E6 RID: 21734 RVA: 0x001EF8DC File Offset: 0x001EDADC
	public string SidescreenButtonText
	{
		get
		{
			if (this.used)
			{
				return UI.USERMENUACTIONS.OPENPOI.ALREADY_RUMMAGED;
			}
			if (this.chore != null)
			{
				return UI.USERMENUACTIONS.OPENPOI.NAME_OFF;
			}
			return UI.USERMENUACTIONS.OPENPOI.NAME;
		}
	}

	// Token: 0x170005EF RID: 1519
	// (get) Token: 0x060054E7 RID: 21735 RVA: 0x001EF90E File Offset: 0x001EDB0E
	public string SidescreenButtonTooltip
	{
		get
		{
			if (this.used)
			{
				return UI.USERMENUACTIONS.OPENPOI.TOOLTIP_ALREADYRUMMAGED;
			}
			if (this.chore != null)
			{
				return UI.USERMENUACTIONS.OPENPOI.TOOLTIP_OFF;
			}
			return UI.USERMENUACTIONS.OPENPOI.TOOLTIP;
		}
	}

	// Token: 0x060054E8 RID: 21736 RVA: 0x001EF940 File Offset: 0x001EDB40
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x060054E9 RID: 21737 RVA: 0x001EF943 File Offset: 0x001EDB43
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x060054EA RID: 21738 RVA: 0x001EF946 File Offset: 0x001EDB46
	public void OnSidescreenButtonPressed()
	{
		if (this.chore == null)
		{
			this.OnClickOpen();
			return;
		}
		this.OnClickCancel();
	}

	// Token: 0x060054EB RID: 21739 RVA: 0x001EF95D File Offset: 0x001EDB5D
	public bool SidescreenButtonInteractable()
	{
		return !this.used;
	}

	// Token: 0x060054EC RID: 21740 RVA: 0x001EF968 File Offset: 0x001EDB68
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x060054ED RID: 21741 RVA: 0x001EF96C File Offset: 0x001EDB6C
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x04003952 RID: 14674
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x04003953 RID: 14675
	public string[][] possible_contents_ids;

	// Token: 0x04003954 RID: 14676
	public string machineSound;

	// Token: 0x04003955 RID: 14677
	public string overrideAnim;

	// Token: 0x04003956 RID: 14678
	public Vector2I dropOffset = Vector2I.zero;

	// Token: 0x04003957 RID: 14679
	public int[] numDataBanks;

	// Token: 0x04003958 RID: 14680
	[Serialize]
	private string[] contents;

	// Token: 0x04003959 RID: 14681
	public bool dropOnDeconstruct;

	// Token: 0x0400395A RID: 14682
	public bool skipAnim;

	// Token: 0x0400395B RID: 14683
	[Serialize]
	private bool pendingRummage;

	// Token: 0x0400395C RID: 14684
	[Serialize]
	private bool used;

	// Token: 0x0400395D RID: 14685
	private Chore chore;

	// Token: 0x02001CA5 RID: 7333
	public class StatesInstance : GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.GameInstance
	{
		// Token: 0x0600AE3A RID: 44602 RVA: 0x003D2BC2 File Offset: 0x003D0DC2
		public StatesInstance(SetLocker master) : base(master)
		{
		}

		// Token: 0x0600AE3B RID: 44603 RVA: 0x003D2BCB File Offset: 0x003D0DCB
		public override void StartSM()
		{
			base.StartSM();
			base.smi.Subscribe(-702296337, delegate(object o)
			{
				if (base.smi.master.dropOnDeconstruct && base.smi.IsInsideState(base.smi.sm.closed))
				{
					base.smi.master.DropContents();
				}
			});
		}
	}

	// Token: 0x02001CA6 RID: 7334
	public class States : GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker>
	{
		// Token: 0x0600AE3D RID: 44605 RVA: 0x003D2C3C File Offset: 0x003D0E3C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.closed;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.closed.PlayAnim("on").Enter(delegate(SetLocker.StatesInstance smi)
			{
				if (smi.master.machineSound != null)
				{
					LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
					if (component != null)
					{
						component.StartSound(GlobalAssets.GetSound(smi.master.machineSound, false));
					}
				}
			});
			this.being_worked.DoNothing();
			this.open.PlayAnim("working_pre").QueueAnim("working_loop", false, null).QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.off).Exit(delegate(SetLocker.StatesInstance smi)
			{
				smi.master.DropContents();
			});
			this.off.PlayAnim("off").Enter(delegate(SetLocker.StatesInstance smi)
			{
				if (smi.master.machineSound != null)
				{
					LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
					if (component != null)
					{
						component.StopSound(GlobalAssets.GetSound(smi.master.machineSound, false));
					}
				}
			});
		}

		// Token: 0x040088B5 RID: 34997
		public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State closed;

		// Token: 0x040088B6 RID: 34998
		public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State being_worked;

		// Token: 0x040088B7 RID: 34999
		public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State open;

		// Token: 0x040088B8 RID: 35000
		public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State off;
	}
}
