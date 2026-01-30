using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007B4 RID: 1972
public class MassageTable : RelaxationPoint, IGameObjectEffectDescriptor, IActivationRangeTarget
{
	// Token: 0x17000333 RID: 819
	// (get) Token: 0x06003416 RID: 13334 RVA: 0x00127B8B File Offset: 0x00125D8B
	public string ActivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.MASSAGETABLE.ACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x17000334 RID: 820
	// (get) Token: 0x06003417 RID: 13335 RVA: 0x00127B97 File Offset: 0x00125D97
	public string DeactivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.MASSAGETABLE.DEACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x06003418 RID: 13336 RVA: 0x00127BA3 File Offset: 0x00125DA3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<MassageTable>(-905833192, MassageTable.OnCopySettingsDelegate);
	}

	// Token: 0x06003419 RID: 13337 RVA: 0x00127BBC File Offset: 0x00125DBC
	private void OnCopySettings(object data)
	{
		MassageTable component = ((GameObject)data).GetComponent<MassageTable>();
		if (component != null)
		{
			this.ActivateValue = component.ActivateValue;
			this.DeactivateValue = component.DeactivateValue;
		}
	}

	// Token: 0x0600341A RID: 13338 RVA: 0x00127BF8 File Offset: 0x00125DF8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		Effects component = worker.GetComponent<Effects>();
		for (int i = 0; i < MassageTable.EffectsRemoved.Length; i++)
		{
			string effect_id = MassageTable.EffectsRemoved[i];
			component.Remove(effect_id);
		}
	}

	// Token: 0x0600341B RID: 13339 RVA: 0x00127C34 File Offset: 0x00125E34
	public new List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.STRESSREDUCEDPERMINUTE, GameUtil.GetFormattedPercent(this.stressModificationValue / 600f * 60f, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.STRESSREDUCEDPERMINUTE, GameUtil.GetFormattedPercent(this.stressModificationValue / 600f * 60f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		if (MassageTable.EffectsRemoved.Length != 0)
		{
			Descriptor item2 = default(Descriptor);
			item2.SetupDescriptor(UI.BUILDINGEFFECTS.REMOVESEFFECTSUBTITLE, UI.BUILDINGEFFECTS.TOOLTIPS.REMOVESEFFECTSUBTITLE, Descriptor.DescriptorType.Effect);
			list.Add(item2);
			for (int i = 0; i < MassageTable.EffectsRemoved.Length; i++)
			{
				string text = MassageTable.EffectsRemoved[i];
				string arg = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".NAME");
				string arg2 = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".CAUSE");
				Descriptor item3 = default(Descriptor);
				item3.IncreaseIndent();
				item3.SetupDescriptor("• " + string.Format(UI.BUILDINGEFFECTS.REMOVEDEFFECT, arg), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REMOVEDEFFECT, arg2), Descriptor.DescriptorType.Effect);
				list.Add(item3);
			}
		}
		return list;
	}

	// Token: 0x0600341C RID: 13340 RVA: 0x00127D94 File Offset: 0x00125F94
	protected override WorkChore<RelaxationPoint> CreateWorkChore()
	{
		WorkChore<RelaxationPoint> workChore = new WorkChore<RelaxationPoint>(Db.Get().ChoreTypes.StressHeal, this, null, true, null, null, null, false, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		workChore.AddPrecondition(MassageTable.IsStressAboveActivationRange, this);
		return workChore;
	}

	// Token: 0x17000335 RID: 821
	// (get) Token: 0x0600341D RID: 13341 RVA: 0x00127DE4 File Offset: 0x00125FE4
	// (set) Token: 0x0600341E RID: 13342 RVA: 0x00127DEC File Offset: 0x00125FEC
	public float ActivateValue
	{
		get
		{
			return this.activateValue;
		}
		set
		{
			this.activateValue = value;
		}
	}

	// Token: 0x17000336 RID: 822
	// (get) Token: 0x0600341F RID: 13343 RVA: 0x00127DF5 File Offset: 0x00125FF5
	// (set) Token: 0x06003420 RID: 13344 RVA: 0x00127DFD File Offset: 0x00125FFD
	public float DeactivateValue
	{
		get
		{
			return this.stopStressingValue;
		}
		set
		{
			this.stopStressingValue = value;
		}
	}

	// Token: 0x17000337 RID: 823
	// (get) Token: 0x06003421 RID: 13345 RVA: 0x00127E06 File Offset: 0x00126006
	public bool UseWholeNumbers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x06003422 RID: 13346 RVA: 0x00127E09 File Offset: 0x00126009
	public float MinValue
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000339 RID: 825
	// (get) Token: 0x06003423 RID: 13347 RVA: 0x00127E10 File Offset: 0x00126010
	public float MaxValue
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x1700033A RID: 826
	// (get) Token: 0x06003424 RID: 13348 RVA: 0x00127E17 File Offset: 0x00126017
	public string ActivationRangeTitleText
	{
		get
		{
			return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.NAME;
		}
	}

	// Token: 0x1700033B RID: 827
	// (get) Token: 0x06003425 RID: 13349 RVA: 0x00127E23 File Offset: 0x00126023
	public string ActivateSliderLabelText
	{
		get
		{
			return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.ACTIVATE;
		}
	}

	// Token: 0x1700033C RID: 828
	// (get) Token: 0x06003426 RID: 13350 RVA: 0x00127E2F File Offset: 0x0012602F
	public string DeactivateSliderLabelText
	{
		get
		{
			return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.DEACTIVATE;
		}
	}

	// Token: 0x04001F79 RID: 8057
	[Serialize]
	private float activateValue = 50f;

	// Token: 0x04001F7A RID: 8058
	private static readonly string[] EffectsRemoved = new string[]
	{
		"SoreBack"
	};

	// Token: 0x04001F7B RID: 8059
	private static readonly EventSystem.IntraObjectHandler<MassageTable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<MassageTable>(delegate(MassageTable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001F7C RID: 8060
	private static readonly Chore.Precondition IsStressAboveActivationRange = new Chore.Precondition
	{
		id = "IsStressAboveActivationRange",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_STRESS_ABOVE_ACTIVATION_RANGE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			IActivationRangeTarget activationRangeTarget = (IActivationRangeTarget)data;
			return Db.Get().Amounts.Stress.Lookup(context.consumerState.gameObject).value >= activationRangeTarget.ActivateValue;
		}
	};
}
