using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02000B1F RID: 2847
public class RoomDetails
{
	// Token: 0x0600532E RID: 21294 RVA: 0x001E4EB0 File Offset: 0x001E30B0
	public static string RoomDetailString(Room room)
	{
		string text = "";
		text = text + "<b>" + ROOMS.DETAILS.HEADER + "</b>";
		foreach (RoomDetails.Detail detail in room.roomType.display_details)
		{
			text = text + "\n    • " + detail.resolve_string_function(room);
		}
		return text;
	}

	// Token: 0x04003845 RID: 14405
	public static readonly RoomDetails.Detail AVERAGE_TEMPERATURE = new RoomDetails.Detail(delegate(Room room)
	{
		float num = 0f;
		if (num == 0f)
		{
			return string.Format(ROOMS.DETAILS.AVERAGE_TEMPERATURE.NAME, UI.OVERLAYS.TEMPERATURE.EXTREMECOLD);
		}
		return string.Format(ROOMS.DETAILS.AVERAGE_TEMPERATURE.NAME, GameUtil.GetFormattedTemperature(num, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
	});

	// Token: 0x04003846 RID: 14406
	public static readonly RoomDetails.Detail AVERAGE_ATMO_MASS = new RoomDetails.Detail(delegate(Room room)
	{
		float num = 0f;
		float num2 = 0f;
		if (num2 > 0f)
		{
			num /= num2;
		}
		else
		{
			num = 0f;
		}
		return string.Format(ROOMS.DETAILS.AVERAGE_ATMO_MASS.NAME, GameUtil.GetFormattedMass(num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	});

	// Token: 0x04003847 RID: 14407
	public static readonly RoomDetails.Detail ASSIGNED_TO = new RoomDetails.Detail(delegate(Room room)
	{
		string text = "";
		foreach (KPrefabID kprefabID in room.GetPrimaryEntities())
		{
			if (!(kprefabID == null))
			{
				Assignable component = kprefabID.GetComponent<Assignable>();
				if (!(component == null))
				{
					IAssignableIdentity assignee = component.assignee;
					if (assignee == null)
					{
						text += ((text == "") ? ("<color=#BCBCBC>    • " + kprefabID.GetProperName() + ": " + ROOMS.DETAILS.ASSIGNED_TO.UNASSIGNED) : ("\n<color=#BCBCBC>    • " + kprefabID.GetProperName() + ": " + ROOMS.DETAILS.ASSIGNED_TO.UNASSIGNED));
						text += "</color>";
					}
					else
					{
						text += ((text == "") ? ("    • " + kprefabID.GetProperName() + ": " + assignee.GetProperName()) : ("\n    • " + kprefabID.GetProperName() + ": " + assignee.GetProperName()));
					}
				}
			}
		}
		if (text == "")
		{
			text = ROOMS.DETAILS.ASSIGNED_TO.UNASSIGNED;
		}
		return string.Format(ROOMS.DETAILS.ASSIGNED_TO.NAME, text);
	});

	// Token: 0x04003848 RID: 14408
	public static readonly RoomDetails.Detail ORNAMENT_COUNT = new RoomDetails.Detail(delegate(Room room)
	{
		int num = 0;
		foreach (KPrefabID kprefabID in room.buildings)
		{
			if (!(kprefabID == null))
			{
				OrnamentReceptacle component = kprefabID.GetComponent<OrnamentReceptacle>();
				if (!(component == null) && component.IsHoldingOrnament && component.IsOperational)
				{
					num++;
				}
			}
		}
		foreach (KPrefabID kprefabID2 in room.otherEntities)
		{
			if (!(kprefabID2 == null))
			{
				OrnamentReceptacle component2 = kprefabID2.GetComponent<OrnamentReceptacle>();
				if (!(component2 == null) && component2.IsHoldingOrnament && component2.IsOperational)
				{
					num++;
				}
			}
		}
		return GameUtil.SafeStringFormat(ROOMS.DETAILS.ORNAMENT_COUNT.NAME, new object[]
		{
			num
		});
	});

	// Token: 0x04003849 RID: 14409
	public static readonly RoomDetails.Detail SIZE = new RoomDetails.Detail((Room room) => string.Format(ROOMS.DETAILS.SIZE.NAME, room.cavity.NumCells));

	// Token: 0x0400384A RID: 14410
	public static readonly RoomDetails.Detail BUILDING_COUNT = new RoomDetails.Detail((Room room) => string.Format(ROOMS.DETAILS.BUILDING_COUNT.NAME, room.buildings.Count));

	// Token: 0x0400384B RID: 14411
	public static readonly RoomDetails.Detail CREATURE_COUNT = new RoomDetails.Detail((Room room) => string.Format(ROOMS.DETAILS.CREATURE_COUNT.NAME, room.cavity.creatures.Count + room.cavity.eggs.Count));

	// Token: 0x0400384C RID: 14412
	public static readonly RoomDetails.Detail PLANT_COUNT = new RoomDetails.Detail(delegate(Room room)
	{
		int num = 0;
		using (List<KPrefabID>.Enumerator enumerator = room.cavity.plants.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.HasTag(GameTags.PlantBranch))
				{
					num++;
				}
			}
		}
		return string.Format(ROOMS.DETAILS.PLANT_COUNT.NAME, num);
	});

	// Token: 0x0400384D RID: 14413
	public static readonly RoomDetails.Detail EFFECT = new RoomDetails.Detail((Room room) => room.roomType.effect);

	// Token: 0x0400384E RID: 14414
	public static readonly RoomDetails.Detail EFFECTS = new RoomDetails.Detail((Room room) => room.roomType.GetRoomEffectsString());

	// Token: 0x02001C6C RID: 7276
	public class Detail
	{
		// Token: 0x0600AD97 RID: 44439 RVA: 0x003D0130 File Offset: 0x003CE330
		public Detail(Func<Room, string> resolve_string_function)
		{
			this.resolve_string_function = resolve_string_function;
		}

		// Token: 0x04008813 RID: 34835
		public Func<Room, string> resolve_string_function;
	}
}
