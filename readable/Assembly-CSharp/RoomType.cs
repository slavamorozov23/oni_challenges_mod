using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

// Token: 0x0200067E RID: 1662
public class RoomType : Resource
{
	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x060028BF RID: 10431 RVA: 0x000E99E0 File Offset: 0x000E7BE0
	// (set) Token: 0x060028C0 RID: 10432 RVA: 0x000E99E8 File Offset: 0x000E7BE8
	public string tooltip { get; private set; }

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x060028C1 RID: 10433 RVA: 0x000E99F1 File Offset: 0x000E7BF1
	// (set) Token: 0x060028C2 RID: 10434 RVA: 0x000E99F9 File Offset: 0x000E7BF9
	public string description { get; set; }

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x060028C3 RID: 10435 RVA: 0x000E9A02 File Offset: 0x000E7C02
	// (set) Token: 0x060028C4 RID: 10436 RVA: 0x000E9A0A File Offset: 0x000E7C0A
	public string effect { get; private set; }

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x060028C5 RID: 10437 RVA: 0x000E9A13 File Offset: 0x000E7C13
	// (set) Token: 0x060028C6 RID: 10438 RVA: 0x000E9A1B File Offset: 0x000E7C1B
	public RoomConstraints.Constraint primary_constraint { get; private set; }

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x060028C7 RID: 10439 RVA: 0x000E9A24 File Offset: 0x000E7C24
	// (set) Token: 0x060028C8 RID: 10440 RVA: 0x000E9A2C File Offset: 0x000E7C2C
	public RoomConstraints.Constraint[] additional_constraints { get; private set; }

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x060028C9 RID: 10441 RVA: 0x000E9A35 File Offset: 0x000E7C35
	// (set) Token: 0x060028CA RID: 10442 RVA: 0x000E9A3D File Offset: 0x000E7C3D
	public int priority { get; private set; }

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x060028CB RID: 10443 RVA: 0x000E9A46 File Offset: 0x000E7C46
	// (set) Token: 0x060028CC RID: 10444 RVA: 0x000E9A4E File Offset: 0x000E7C4E
	public bool single_assignee { get; private set; }

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x060028CD RID: 10445 RVA: 0x000E9A57 File Offset: 0x000E7C57
	// (set) Token: 0x060028CE RID: 10446 RVA: 0x000E9A5F File Offset: 0x000E7C5F
	public RoomDetails.Detail[] display_details { get; private set; }

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x060028CF RID: 10447 RVA: 0x000E9A68 File Offset: 0x000E7C68
	// (set) Token: 0x060028D0 RID: 10448 RVA: 0x000E9A70 File Offset: 0x000E7C70
	public bool priority_building_use { get; private set; }

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x060028D1 RID: 10449 RVA: 0x000E9A79 File Offset: 0x000E7C79
	// (set) Token: 0x060028D2 RID: 10450 RVA: 0x000E9A81 File Offset: 0x000E7C81
	public RoomTypeCategory category { get; private set; }

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x060028D3 RID: 10451 RVA: 0x000E9A8A File Offset: 0x000E7C8A
	// (set) Token: 0x060028D4 RID: 10452 RVA: 0x000E9A92 File Offset: 0x000E7C92
	public RoomType[] upgrade_paths { get; private set; }

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x060028D5 RID: 10453 RVA: 0x000E9A9B File Offset: 0x000E7C9B
	// (set) Token: 0x060028D6 RID: 10454 RVA: 0x000E9AA3 File Offset: 0x000E7CA3
	public string[] effects { get; private set; }

	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x060028D7 RID: 10455 RVA: 0x000E9AAC File Offset: 0x000E7CAC
	// (set) Token: 0x060028D8 RID: 10456 RVA: 0x000E9AB4 File Offset: 0x000E7CB4
	public int sortKey { get; private set; }

	// Token: 0x060028D9 RID: 10457 RVA: 0x000E9AC0 File Offset: 0x000E7CC0
	public RoomType(string id, string name, string description, string tooltip, string effect, RoomTypeCategory category, RoomConstraints.Constraint primary_constraint, RoomConstraints.Constraint[] additional_constraints, RoomDetails.Detail[] display_details, int priority = 0, RoomType[] upgrade_paths = null, bool single_assignee = false, bool priority_building_use = false, string[] effects = null, int sortKey = 0) : base(id, name)
	{
		this.tooltip = tooltip;
		this.description = description;
		this.effect = effect;
		this.category = category;
		this.primary_constraint = primary_constraint;
		this.additional_constraints = additional_constraints;
		this.display_details = display_details;
		this.priority = priority;
		this.upgrade_paths = upgrade_paths;
		this.single_assignee = single_assignee;
		this.priority_building_use = priority_building_use;
		this.effects = effects;
		this.sortKey = sortKey;
		if (this.upgrade_paths != null)
		{
			RoomType[] upgrade_paths2 = this.upgrade_paths;
			for (int i = 0; i < upgrade_paths2.Length; i++)
			{
				Debug.Assert(upgrade_paths2[i] != null, name + " has a null upgrade path. Maybe it wasn't initialized yet.");
			}
		}
	}

	// Token: 0x060028DA RID: 10458 RVA: 0x000E9B70 File Offset: 0x000E7D70
	public RoomType.RoomIdentificationResult isSatisfactory(Room candidate_room)
	{
		if (this.primary_constraint != null && !this.primary_constraint.isSatisfied(candidate_room))
		{
			return RoomType.RoomIdentificationResult.primary_unsatisfied;
		}
		if (this.additional_constraints != null)
		{
			RoomConstraints.Constraint[] additional_constraints = this.additional_constraints;
			for (int i = 0; i < additional_constraints.Length; i++)
			{
				if (!additional_constraints[i].isSatisfied(candidate_room))
				{
					return RoomType.RoomIdentificationResult.primary_satisfied;
				}
			}
		}
		return RoomType.RoomIdentificationResult.all_satisfied;
	}

	// Token: 0x060028DB RID: 10459 RVA: 0x000E9BC0 File Offset: 0x000E7DC0
	public string GetCriteriaString()
	{
		string text = string.Concat(new string[]
		{
			"<b>",
			this.Name,
			"</b>\n",
			this.tooltip,
			"\n\n",
			ROOMS.CRITERIA.HEADER
		});
		if (this == Db.Get().RoomTypes.Neutral)
		{
			text = text + "\n    • " + ROOMS.CRITERIA.NEUTRAL_TYPE;
		}
		text += ((this.primary_constraint == null) ? "" : ("\n    • " + this.primary_constraint.name));
		if (this.additional_constraints != null)
		{
			foreach (RoomConstraints.Constraint constraint in this.additional_constraints)
			{
				text = text + "\n    • " + constraint.name;
			}
		}
		return text;
	}

	// Token: 0x060028DC RID: 10460 RVA: 0x000E9C98 File Offset: 0x000E7E98
	public string GetRoomEffectsString()
	{
		if (this.effects != null && this.effects.Length != 0)
		{
			string text = ROOMS.EFFECTS.HEADER;
			foreach (string id in this.effects)
			{
				Effect effect = Db.Get().effects.Get(id);
				text += Effect.CreateTooltip(effect, false, "\n    • ", false);
			}
			return text;
		}
		return null;
	}

	// Token: 0x060028DD RID: 10461 RVA: 0x000E9D04 File Offset: 0x000E7F04
	public void TriggerRoomEffects(KPrefabID triggerer, Effects target, out List<EffectInstance> result)
	{
		result = null;
		if (this.primary_constraint == null)
		{
			return;
		}
		if (triggerer == null)
		{
			return;
		}
		if (this.effects == null)
		{
			return;
		}
		if (this.primary_constraint.building_criteria(triggerer))
		{
			result = new List<EffectInstance>();
			foreach (string effect_id in this.effects)
			{
				result.Add(target.Add(effect_id, true));
			}
		}
	}

	// Token: 0x060028DE RID: 10462 RVA: 0x000E9D74 File Offset: 0x000E7F74
	public void TriggerRoomEffects(KPrefabID triggerer, Effects target)
	{
		List<EffectInstance> list;
		this.TriggerRoomEffects(triggerer, target, out list);
	}

	// Token: 0x02001554 RID: 5460
	public enum RoomIdentificationResult
	{
		// Token: 0x04007180 RID: 29056
		all_satisfied,
		// Token: 0x04007181 RID: 29057
		primary_satisfied,
		// Token: 0x04007182 RID: 29058
		primary_unsatisfied
	}
}
