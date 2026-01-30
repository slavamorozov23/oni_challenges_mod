using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200061A RID: 1562
[SkipSaveFileSerialization]
public class Overheatable : StateMachineComponent<Overheatable.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x060024D9 RID: 9433 RVA: 0x000D3BD5 File Offset: 0x000D1DD5
	public void ResetTemperature()
	{
		base.GetComponent<PrimaryElement>().Temperature = 293.15f;
	}

	// Token: 0x060024DA RID: 9434 RVA: 0x000D3BE8 File Offset: 0x000D1DE8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overheatTemp = this.GetAttributes().Add(Db.Get().BuildingAttributes.OverheatTemperature);
		this.fatalTemp = this.GetAttributes().Add(Db.Get().BuildingAttributes.FatalTemperature);
	}

	// Token: 0x060024DB RID: 9435 RVA: 0x000D3C3C File Offset: 0x000D1E3C
	private void InitializeModifiers()
	{
		if (this.modifiersInitialized)
		{
			return;
		}
		this.modifiersInitialized = true;
		AttributeModifier modifier = new AttributeModifier(this.overheatTemp.Id, this.baseOverheatTemp, UI.TOOLTIPS.BASE_VALUE, false, false, true)
		{
			OverrideTimeSlice = new GameUtil.TimeSlice?(GameUtil.TimeSlice.None)
		};
		AttributeModifier modifier2 = new AttributeModifier(this.fatalTemp.Id, this.baseFatalTemp, UI.TOOLTIPS.BASE_VALUE, false, false, true);
		this.GetAttributes().Add(modifier);
		this.GetAttributes().Add(modifier2);
	}

	// Token: 0x060024DC RID: 9436 RVA: 0x000D3CC8 File Offset: 0x000D1EC8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.InitializeModifiers();
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		if (handle.IsValid() && GameComps.StructureTemperatures.IsEnabled(handle))
		{
			GameComps.StructureTemperatures.Disable(handle);
			GameComps.StructureTemperatures.Enable(handle);
		}
		base.smi.StartSM();
	}

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x060024DD RID: 9437 RVA: 0x000D3D29 File Offset: 0x000D1F29
	public float OverheatTemperature
	{
		get
		{
			this.InitializeModifiers();
			if (this.overheatTemp == null)
			{
				return 10000f;
			}
			return this.overheatTemp.GetTotalValue();
		}
	}

	// Token: 0x060024DE RID: 9438 RVA: 0x000D3D4C File Offset: 0x000D1F4C
	public Notification CreateOverheatedNotification()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		return new Notification(MISC.NOTIFICATIONS.BUILDINGOVERHEATED.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.BUILDINGOVERHEATED.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + component.GetProperName(), false, 0f, null, null, null, true, false, false);
	}

	// Token: 0x060024DF RID: 9439 RVA: 0x000D3DAC File Offset: 0x000D1FAC
	private static string ToolTipResolver(List<Notification> notificationList, object data)
	{
		string text = "";
		for (int i = 0; i < notificationList.Count; i++)
		{
			Notification notification = notificationList[i];
			text += (string)notification.tooltipData;
			if (i < notificationList.Count - 1)
			{
				text += "\n";
			}
		}
		return string.Format(MISC.NOTIFICATIONS.BUILDINGOVERHEATED.TOOLTIP, text);
	}

	// Token: 0x060024E0 RID: 9440 RVA: 0x000D3E14 File Offset: 0x000D2014
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.overheatTemp != null && this.fatalTemp != null)
		{
			string formattedValue = this.overheatTemp.GetFormattedValue();
			string formattedValue2 = this.fatalTemp.GetFormattedValue();
			string text = UI.BUILDINGEFFECTS.TOOLTIPS.OVERHEAT_TEMP;
			text = text + "\n\n" + this.overheatTemp.GetAttributeValueTooltip();
			Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.OVERHEAT_TEMP, formattedValue, formattedValue2), string.Format(text, formattedValue, formattedValue2), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		else if (this.baseOverheatTemp != 0f)
		{
			string formattedTemperature = GameUtil.GetFormattedTemperature(this.baseOverheatTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
			string formattedTemperature2 = GameUtil.GetFormattedTemperature(this.baseFatalTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
			string format = UI.BUILDINGEFFECTS.TOOLTIPS.OVERHEAT_TEMP;
			Descriptor item2 = new Descriptor(string.Format(UI.BUILDINGEFFECTS.OVERHEAT_TEMP, formattedTemperature, formattedTemperature2), string.Format(format, formattedTemperature, formattedTemperature2), Descriptor.DescriptorType.Effect, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x04001590 RID: 5520
	private bool modifiersInitialized;

	// Token: 0x04001591 RID: 5521
	private AttributeInstance overheatTemp;

	// Token: 0x04001592 RID: 5522
	private AttributeInstance fatalTemp;

	// Token: 0x04001593 RID: 5523
	public float baseOverheatTemp;

	// Token: 0x04001594 RID: 5524
	public float baseFatalTemp;

	// Token: 0x020014EE RID: 5358
	public class StatesInstance : GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.GameInstance
	{
		// Token: 0x06009189 RID: 37257 RVA: 0x00371630 File Offset: 0x0036F830
		public StatesInstance(Overheatable smi) : base(smi)
		{
		}

		// Token: 0x0600918A RID: 37258 RVA: 0x0037163C File Offset: 0x0036F83C
		public void TryDoOverheatDamage()
		{
			if (Time.time - this.lastOverheatDamageTime < 7.5f)
			{
				return;
			}
			this.lastOverheatDamageTime += 7.5f;
			base.master.BoxingTrigger<BuildingHP.DamageSourceInfo>(-794517298, new BuildingHP.DamageSourceInfo
			{
				damage = 1,
				source = BUILDINGS.DAMAGESOURCES.BUILDING_OVERHEATED,
				popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.OVERHEAT,
				fullDamageEffectName = "smoke_damage_kanim"
			});
		}

		// Token: 0x04007002 RID: 28674
		public float lastOverheatDamageTime;
	}

	// Token: 0x020014EF RID: 5359
	public class States : GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable>
	{
		// Token: 0x0600918B RID: 37259 RVA: 0x003716C0 File Offset: 0x0036F8C0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.safeTemperature;
			this.root.EventTransition(GameHashes.BuildingBroken, this.invulnerable, null);
			this.invulnerable.EventHandler(GameHashes.BuildingPartiallyRepaired, delegate(Overheatable.StatesInstance smi)
			{
				smi.master.ResetTemperature();
			}).EventTransition(GameHashes.BuildingPartiallyRepaired, this.safeTemperature, null);
			this.safeTemperature.TriggerOnEnter(GameHashes.OptimalTemperatureAchieved, null).EventTransition(GameHashes.BuildingOverheated, this.overheated, null);
			this.overheated.Enter(delegate(Overheatable.StatesInstance smi)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_OverheatingBuildings, true);
			}).EventTransition(GameHashes.BuildingNoLongerOverheated, this.safeTemperature, null).ToggleStatusItem(Db.Get().BuildingStatusItems.Overheated, null).ToggleNotification((Overheatable.StatesInstance smi) => smi.master.CreateOverheatedNotification()).TriggerOnEnter(GameHashes.TooHotWarning, null).Enter("InitOverheatTime", delegate(Overheatable.StatesInstance smi)
			{
				smi.lastOverheatDamageTime = Time.time;
			}).Update("OverheatDamage", delegate(Overheatable.StatesInstance smi, float dt)
			{
				smi.TryDoOverheatDamage();
			}, UpdateRate.SIM_4000ms, false);
		}

		// Token: 0x04007003 RID: 28675
		public GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.State invulnerable;

		// Token: 0x04007004 RID: 28676
		public GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.State safeTemperature;

		// Token: 0x04007005 RID: 28677
		public GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.State overheated;
	}
}
