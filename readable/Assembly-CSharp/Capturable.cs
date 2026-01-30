using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200088D RID: 2189
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Capturable")]
public class Capturable : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x06003C37 RID: 15415 RVA: 0x00150EFE File Offset: 0x0014F0FE
	public bool IsMarkedForCapture
	{
		get
		{
			return this.markedForCapture;
		}
	}

	// Token: 0x06003C38 RID: 15416 RVA: 0x00150F08 File Offset: 0x0014F108
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Capturables.Add(this);
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.attributeConverter = Db.Get().AttributeConverters.CapturableSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
		this.resetProgressOnStop = true;
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.multitoolContext = "capture";
		this.multitoolHitEffectTag = "fx_capture_splash";
	}

	// Token: 0x06003C39 RID: 15417 RVA: 0x00150FC8 File Offset: 0x0014F1C8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Capturable>(1623392196, Capturable.OnDeathDelegate);
		base.Subscribe<Capturable>(493375141, Capturable.OnRefreshUserMenuDelegate);
		base.Subscribe<Capturable>(-1582839653, Capturable.OnTagsChangedDelegate);
		if (this.markedForCapture)
		{
			Prioritizable.AddRef(base.gameObject);
		}
		this.UpdateStatusItem();
		this.UpdateChore();
		base.SetWorkTime(10f);
	}

	// Token: 0x06003C3A RID: 15418 RVA: 0x00151038 File Offset: 0x0014F238
	protected override void OnCleanUp()
	{
		Components.Capturables.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06003C3B RID: 15419 RVA: 0x0015104C File Offset: 0x0014F24C
	public override Vector3 GetTargetPoint()
	{
		Vector3 result = base.transform.GetPosition();
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component != null)
		{
			result = component.bounds.center;
		}
		result.z = 0f;
		return result;
	}

	// Token: 0x06003C3C RID: 15420 RVA: 0x00151091 File Offset: 0x0014F291
	private void OnDeath(object data)
	{
		this.allowCapture = false;
		this.markedForCapture = false;
		this.UpdateChore();
	}

	// Token: 0x06003C3D RID: 15421 RVA: 0x001510A7 File Offset: 0x0014F2A7
	private void OnTagsChanged(object _)
	{
		this.MarkForCapture(this.markedForCapture);
	}

	// Token: 0x06003C3E RID: 15422 RVA: 0x001510B8 File Offset: 0x0014F2B8
	public void MarkForCapture(bool mark)
	{
		PrioritySetting priority = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);
		this.MarkForCapture(mark, priority, false);
	}

	// Token: 0x06003C3F RID: 15423 RVA: 0x001510D8 File Offset: 0x0014F2D8
	public void MarkForCapture(bool mark, PrioritySetting priority, bool updateMarkedPriority = false)
	{
		mark = (mark && this.IsCapturable());
		if (this.markedForCapture && !mark)
		{
			Prioritizable.RemoveRef(base.gameObject);
		}
		else if (!this.markedForCapture && mark)
		{
			Prioritizable.AddRef(base.gameObject);
			Prioritizable component = base.GetComponent<Prioritizable>();
			if (component)
			{
				component.SetMasterPriority(priority);
			}
		}
		else if (updateMarkedPriority && this.markedForCapture && mark)
		{
			Prioritizable component2 = base.GetComponent<Prioritizable>();
			if (component2)
			{
				component2.SetMasterPriority(priority);
			}
		}
		this.markedForCapture = mark;
		this.UpdateStatusItem();
		this.UpdateChore();
	}

	// Token: 0x06003C40 RID: 15424 RVA: 0x00151174 File Offset: 0x0014F374
	public bool IsCapturable()
	{
		return this.allowCapture && !base.gameObject.HasTag(GameTags.Trapped) && !base.gameObject.HasTag(GameTags.Stored) && !base.gameObject.HasTag(GameTags.Creatures.Bagged);
	}

	// Token: 0x06003C41 RID: 15425 RVA: 0x001511C8 File Offset: 0x0014F3C8
	private void OnRefreshUserMenu(object data)
	{
		if (!this.IsCapturable())
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (!this.markedForCapture) ? new KIconButtonMenu.ButtonInfo("action_capture", UI.USERMENUACTIONS.CAPTURE.NAME, delegate()
		{
			this.MarkForCapture(true);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CAPTURE.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_capture", UI.USERMENUACTIONS.CANCELCAPTURE.NAME, delegate()
		{
			this.MarkForCapture(false);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELCAPTURE.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06003C42 RID: 15426 RVA: 0x0015126C File Offset: 0x0014F46C
	private void UpdateStatusItem()
	{
		this.shouldShowSkillPerkStatusItem = this.markedForCapture;
		base.UpdateStatusItem(null);
		if (this.markedForCapture)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OrderCapture, this);
			return;
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.OrderCapture, false);
	}

	// Token: 0x06003C43 RID: 15427 RVA: 0x001512D0 File Offset: 0x0014F4D0
	private void UpdateChore()
	{
		if (this.markedForCapture && this.chore == null)
		{
			this.chore = new WorkChore<Capturable>(Db.Get().ChoreTypes.Capture, this, null, true, null, new Action<Chore>(this.OnChoreBegins), new Action<Chore>(this.OnChoreEnds), true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			return;
		}
		if (!this.markedForCapture && this.chore != null)
		{
			this.chore.Cancel("not marked for capture");
			this.chore = null;
		}
	}

	// Token: 0x06003C44 RID: 15428 RVA: 0x00151358 File Offset: 0x0014F558
	private void OnChoreBegins(Chore chore)
	{
		IdleStates.Instance smi = base.gameObject.GetSMI<IdleStates.Instance>();
		if (smi != null)
		{
			smi.GoTo(smi.sm.root);
			smi.GetComponent<Navigator>().Stop(false, true);
		}
	}

	// Token: 0x06003C45 RID: 15429 RVA: 0x00151394 File Offset: 0x0014F594
	private void OnChoreEnds(Chore chore)
	{
		IdleStates.Instance smi = base.gameObject.GetSMI<IdleStates.Instance>();
		if (smi != null)
		{
			smi.GoTo(smi.sm.GetDefaultState());
		}
	}

	// Token: 0x06003C46 RID: 15430 RVA: 0x001513C1 File Offset: 0x0014F5C1
	protected override void OnStartWork(WorkerBase worker)
	{
		base.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.StunnedForCapture, false);
	}

	// Token: 0x06003C47 RID: 15431 RVA: 0x001513D4 File Offset: 0x0014F5D4
	protected override void OnStopWork(WorkerBase worker)
	{
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.StunnedForCapture);
	}

	// Token: 0x06003C48 RID: 15432 RVA: 0x001513E8 File Offset: 0x0014F5E8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		int num = this.NaturalBuildingCell();
		if (Grid.Solid[num])
		{
			int num2 = Grid.CellAbove(num);
			if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
			{
				num = num2;
			}
		}
		this.MarkForCapture(false);
		this.baggable.SetWrangled();
		this.baggable.transform.SetPosition(Grid.CellToPosCCC(num, Grid.SceneLayer.Ore));
	}

	// Token: 0x06003C49 RID: 15433 RVA: 0x00151454 File Offset: 0x0014F654
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (this.allowCapture)
		{
			descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.CAPTURE_METHOD_WRANGLE, UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_WRANGLE, Descriptor.DescriptorType.Effect, false));
		}
		return descriptors;
	}

	// Token: 0x04002524 RID: 9508
	[MyCmpAdd]
	private Baggable baggable;

	// Token: 0x04002525 RID: 9509
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x04002526 RID: 9510
	public bool allowCapture = true;

	// Token: 0x04002527 RID: 9511
	[Serialize]
	private bool markedForCapture;

	// Token: 0x04002528 RID: 9512
	private Chore chore;

	// Token: 0x04002529 RID: 9513
	private static readonly EventSystem.IntraObjectHandler<Capturable> OnDeathDelegate = new EventSystem.IntraObjectHandler<Capturable>(delegate(Capturable component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x0400252A RID: 9514
	private static readonly EventSystem.IntraObjectHandler<Capturable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Capturable>(delegate(Capturable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x0400252B RID: 9515
	private static readonly EventSystem.IntraObjectHandler<Capturable> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Capturable>(delegate(Capturable component, object data)
	{
		component.OnTagsChanged(data);
	});
}
