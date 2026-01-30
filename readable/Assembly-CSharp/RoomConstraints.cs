using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x02000B1E RID: 2846
public static class RoomConstraints
{
	// Token: 0x0600532A RID: 21290 RVA: 0x001E3FE3 File Offset: 0x001E21E3
	public static Tag AddAndReturn(this List<Tag> list, Tag tag)
	{
		list.Add(tag);
		return tag;
	}

	// Token: 0x0600532B RID: 21291 RVA: 0x001E3FF0 File Offset: 0x001E21F0
	public static string RoomCriteriaString(Room room)
	{
		string text = "";
		RoomType roomType = room.roomType;
		if (roomType != Db.Get().RoomTypes.Neutral)
		{
			text = text + "<b>" + ROOMS.CRITERIA.HEADER + "</b>";
			text = text + "\n    • " + roomType.primary_constraint.name;
			if (roomType.additional_constraints != null)
			{
				foreach (RoomConstraints.Constraint constraint in roomType.additional_constraints)
				{
					if (constraint.isSatisfied(room))
					{
						text = text + "\n    • " + constraint.name;
					}
					else
					{
						text = text + "\n<color=#F44A47FF>    • " + constraint.name + "</color>";
					}
				}
			}
			return text;
		}
		RoomTypes.RoomTypeQueryResult[] possibleRoomTypes = Db.Get().RoomTypes.GetPossibleRoomTypes(room);
		text += ((possibleRoomTypes.Length > 1) ? ("<b>" + ROOMS.CRITERIA.POSSIBLE_TYPES_HEADER + "</b>") : "");
		foreach (RoomTypes.RoomTypeQueryResult roomTypeQueryResult in possibleRoomTypes)
		{
			RoomType type = roomTypeQueryResult.Type;
			if (type != Db.Get().RoomTypes.Neutral)
			{
				if (text != "")
				{
					text += "\n";
				}
				text = string.Concat(new string[]
				{
					text,
					"<b><color=#BCBCBC>    • ",
					type.Name,
					"</b> (",
					type.primary_constraint.conflictDescription,
					")</color>"
				});
				if (roomTypeQueryResult.SatisfactionRating == RoomType.RoomIdentificationResult.all_satisfied)
				{
					bool flag = false;
					RoomTypes.RoomTypeQueryResult[] array2 = possibleRoomTypes;
					for (int j = 0; j < array2.Length; j++)
					{
						RoomType type2 = array2[j].Type;
						if (type2 != type && type2 != Db.Get().RoomTypes.Neutral && Db.Get().RoomTypes.HasAmbiguousRoomType(room, type, type2))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						text += string.Format("\n<color=#F44A47FF>{0}{1}{2}</color>", "    ", "    • ", ROOMS.CRITERIA.NO_TYPE_CONFLICTS);
					}
				}
				else
				{
					foreach (RoomConstraints.Constraint constraint2 in type.additional_constraints)
					{
						if (!constraint2.isSatisfied(room))
						{
							string str = string.Empty;
							if (constraint2.building_criteria != null)
							{
								str = string.Format(ROOMS.CRITERIA.CRITERIA_FAILED.MISSING_BUILDING, constraint2.name);
							}
							else
							{
								str = string.Format(ROOMS.CRITERIA.CRITERIA_FAILED.FAILED, constraint2.name);
							}
							text = text + "\n<color=#F44A47FF>        • " + str + "</color>";
						}
					}
				}
			}
		}
		return text;
	}

	// Token: 0x0600532C RID: 21292 RVA: 0x001E42A4 File Offset: 0x001E24A4
	private static bool CheckOrnament(KPrefabID entityOrBuilding)
	{
		if (entityOrBuilding == null)
		{
			return false;
		}
		if (!entityOrBuilding.HasTag(GameTags.OrnamentDisplayer))
		{
			return false;
		}
		OrnamentReceptacle component = entityOrBuilding.GetComponent<OrnamentReceptacle>();
		return component.Occupant != null && component.Occupant.HasTag(GameTags.Ornament) && (component.operational == null || component.operational.IsOperational);
	}

	// Token: 0x04003811 RID: 14353
	public static RoomConstraints.Constraint CEILING_HEIGHT_4 = new RoomConstraints.Constraint(null, (Room room) => 1 + room.cavity.maxY - room.cavity.minY >= 4, 1, string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.NAME, "4"), string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.DESCRIPTION, "4"), null, null);

	// Token: 0x04003812 RID: 14354
	public static RoomConstraints.Constraint CEILING_HEIGHT_6 = new RoomConstraints.Constraint(null, (Room room) => 1 + room.cavity.maxY - room.cavity.minY >= 6, 1, string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.NAME, "6"), string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.DESCRIPTION, "6"), null, null);

	// Token: 0x04003813 RID: 14355
	public static RoomConstraints.Constraint MINIMUM_SIZE_12 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.NumCells >= 12, 1, string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.NAME, "12"), string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, "12"), null, null);

	// Token: 0x04003814 RID: 14356
	public static RoomConstraints.Constraint MINIMUM_SIZE_24 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.NumCells >= 24, 1, string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.NAME, "24"), string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, "24"), null, null);

	// Token: 0x04003815 RID: 14357
	public static RoomConstraints.Constraint MINIMUM_SIZE_32 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.NumCells >= 32, 1, string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.NAME, "32"), string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, "32"), null, null);

	// Token: 0x04003816 RID: 14358
	public static RoomConstraints.Constraint MAXIMUM_SIZE_64 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.NumCells <= 64, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "64"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "64"), null, null);

	// Token: 0x04003817 RID: 14359
	public static RoomConstraints.Constraint MAXIMUM_SIZE_96 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.NumCells <= 96, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "96"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "96"), null, null);

	// Token: 0x04003818 RID: 14360
	public static RoomConstraints.Constraint MAXIMUM_SIZE_120 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.NumCells <= 120, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "120"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "120"), null, null);

	// Token: 0x04003819 RID: 14361
	public static RoomConstraints.Constraint NO_INDUSTRIAL_MACHINERY = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		using (List<KPrefabID>.Enumerator enumerator = room.buildings.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasTag(RoomConstraints.ConstraintTags.IndustrialMachinery))
				{
					return false;
				}
			}
		}
		return true;
	}, 1, ROOMS.CRITERIA.NO_INDUSTRIAL_MACHINERY.NAME, ROOMS.CRITERIA.NO_INDUSTRIAL_MACHINERY.DESCRIPTION, null, null);

	// Token: 0x0400381A RID: 14362
	public static RoomConstraints.Constraint NO_COTS = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		foreach (KPrefabID kprefabID in room.buildings)
		{
			if (kprefabID.HasTag(RoomConstraints.ConstraintTags.BedType) && !kprefabID.HasTag(RoomConstraints.ConstraintTags.LuxuryBedType))
			{
				return false;
			}
		}
		return true;
	}, 1, ROOMS.CRITERIA.NO_COTS.NAME, ROOMS.CRITERIA.NO_COTS.DESCRIPTION, null, null);

	// Token: 0x0400381B RID: 14363
	public static RoomConstraints.Constraint NO_LUXURY_BEDS = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		using (List<KPrefabID>.Enumerator enumerator = room.buildings.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasTag(RoomConstraints.ConstraintTags.LuxuryBedType))
				{
					return false;
				}
			}
		}
		return true;
	}, 1, ROOMS.CRITERIA.NO_COTS.NAME, ROOMS.CRITERIA.NO_COTS.DESCRIPTION, null, null);

	// Token: 0x0400381C RID: 14364
	public static RoomConstraints.Constraint NO_OUTHOUSES = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		foreach (KPrefabID kprefabID in room.buildings)
		{
			if (kprefabID.HasTag(RoomConstraints.ConstraintTags.ToiletType) && !kprefabID.HasTag(RoomConstraints.ConstraintTags.FlushToiletType))
			{
				return false;
			}
		}
		return true;
	}, 1, ROOMS.CRITERIA.NO_OUTHOUSES.NAME, ROOMS.CRITERIA.NO_OUTHOUSES.DESCRIPTION, null, null);

	// Token: 0x0400381D RID: 14365
	public static RoomConstraints.Constraint NO_MESS_STATION = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		bool flag = false;
		int num = 0;
		while (!flag && num < room.buildings.Count)
		{
			flag = room.buildings[num].HasTag(RoomConstraints.ConstraintTags.MessTable);
			num++;
		}
		return !flag;
	}, 1, ROOMS.CRITERIA.NO_MESS_STATION.NAME, ROOMS.CRITERIA.NO_MESS_STATION.DESCRIPTION, null, null);

	// Token: 0x0400381E RID: 14366
	public static RoomConstraints.Constraint NO_BASIC_MESS_STATIONS = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		bool flag = false;
		int num = 0;
		while (!flag && num < room.buildings.Count)
		{
			flag = (room.buildings[num].PrefabID() == "DiningTable");
			if (flag)
			{
				break;
			}
			num++;
		}
		return !flag;
	}, 1, ROOMS.CRITERIA.NO_BASIC_MESS_STATIONS.NAME, ROOMS.CRITERIA.NO_BASIC_MESS_STATIONS.DESCRIPTION, null, null);

	// Token: 0x0400381F RID: 14367
	public static RoomConstraints.Constraint HAS_LUXURY_BED = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.LuxuryBedType), null, 1, ROOMS.CRITERIA.HAS_LUXURY_BED.NAME, ROOMS.CRITERIA.HAS_LUXURY_BED.DESCRIPTION, null, null);

	// Token: 0x04003820 RID: 14368
	public static RoomConstraints.Constraint HAS_BED = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.BedType) && !bc.HasTag(RoomConstraints.ConstraintTags.Clinic), null, 1, ROOMS.CRITERIA.HAS_BED.NAME, ROOMS.CRITERIA.HAS_BED.DESCRIPTION, null, null);

	// Token: 0x04003821 RID: 14369
	public static RoomConstraints.Constraint SCIENCE_BUILDINGS = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.ScienceBuilding), null, 2, ROOMS.CRITERIA.SCIENCE_BUILDINGS.NAME, ROOMS.CRITERIA.SCIENCE_BUILDINGS.DESCRIPTION, null, null);

	// Token: 0x04003822 RID: 14370
	public static RoomConstraints.Constraint BED_SINGLE = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.BedType) && !bc.HasTag(RoomConstraints.ConstraintTags.Clinic), delegate(Room room)
	{
		short num = 0;
		int num2 = 0;
		while (num < 2 && num2 < room.buildings.Count)
		{
			if (room.buildings[num2].HasTag(RoomConstraints.ConstraintTags.BedType))
			{
				num += 1;
			}
			num2++;
		}
		return num == 1;
	}, 1, ROOMS.CRITERIA.BED_SINGLE.NAME, ROOMS.CRITERIA.BED_SINGLE.DESCRIPTION, null, null);

	// Token: 0x04003823 RID: 14371
	public static RoomConstraints.Constraint LUXURY_BED_SINGLE = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.LuxuryBedType), delegate(Room room)
	{
		short num = 0;
		int num2 = 0;
		while (num <= 2 && num2 < room.buildings.Count)
		{
			if (room.buildings[num2].HasTag(RoomConstraints.ConstraintTags.LuxuryBedType))
			{
				num += 1;
			}
			num2++;
		}
		return num == 1;
	}, 1, ROOMS.CRITERIA.LUXURYBEDTYPE.NAME, ROOMS.CRITERIA.LUXURYBEDTYPE.DESCRIPTION, null, null);

	// Token: 0x04003824 RID: 14372
	public static RoomConstraints.Constraint BUILDING_DECOR_POSITIVE = new RoomConstraints.Constraint(delegate(KPrefabID bc)
	{
		DecorProvider component = bc.GetComponent<DecorProvider>();
		return component != null && component.baseDecor > 0f;
	}, null, 1, ROOMS.CRITERIA.BUILDING_DECOR_POSITIVE.NAME, ROOMS.CRITERIA.BUILDING_DECOR_POSITIVE.DESCRIPTION, null, null);

	// Token: 0x04003825 RID: 14373
	public static RoomConstraints.Constraint DECORATIVE_ITEM = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(GameTags.Decoration), null, 1, string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.NAME, 1), string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.DESCRIPTION, 1), null, null);

	// Token: 0x04003826 RID: 14374
	public static RoomConstraints.Constraint DECORATIVE_ITEM_2 = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(GameTags.Decoration), null, 2, string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.NAME, 2), string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.DESCRIPTION, 2), null, null);

	// Token: 0x04003827 RID: 14375
	public static RoomConstraints.Constraint ORNAMENTDISPLAYED = new RoomConstraints.Constraint(null, null, delegate(Room room)
	{
		for (int i = 0; i < room.buildings.Count; i++)
		{
			if (RoomConstraints.CheckOrnament(room.buildings[i]))
			{
				return true;
			}
		}
		for (int j = 0; j < room.otherEntities.Count; j++)
		{
			if (RoomConstraints.CheckOrnament(room.otherEntities[j]))
			{
				return true;
			}
		}
		return false;
	}, 1, ROOMS.CRITERIA.ORNAMENT.NAME, ROOMS.CRITERIA.ORNAMENT.DESCRIPTION, null, null);

	// Token: 0x04003828 RID: 14376
	public static RoomConstraints.Constraint POWER_STATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType), delegate(Room room)
	{
		int num = 0;
		bool flag = false;
		foreach (KPrefabID kprefabID in room.buildings)
		{
			flag = (flag || kprefabID.HasTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType));
			num += (kprefabID.HasTag(RoomConstraints.ConstraintTags.PowerBuilding) ? 1 : 0);
		}
		return flag && num >= 2;
	}, 1, ROOMS.CRITERIA.POWERPLANT.NAME, ROOMS.CRITERIA.POWERPLANT.DESCRIPTION, null, ROOMS.CRITERIA.POWERPLANT.CONFLICT_DESCRIPTION);

	// Token: 0x04003829 RID: 14377
	public static RoomConstraints.Constraint FARM_STATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.FarmStationType), null, 1, ROOMS.CRITERIA.FARMSTATIONTYPE.NAME, ROOMS.CRITERIA.FARMSTATIONTYPE.DESCRIPTION, null, null);

	// Token: 0x0400382A RID: 14378
	public static RoomConstraints.Constraint RANCH_STATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.RanchStationType), null, 1, ROOMS.CRITERIA.RANCHSTATIONTYPE.NAME, ROOMS.CRITERIA.RANCHSTATIONTYPE.DESCRIPTION, null, null);

	// Token: 0x0400382B RID: 14379
	public static RoomConstraints.Constraint SPICE_STATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.SpiceStation), null, 1, ROOMS.CRITERIA.SPICESTATION.NAME, ROOMS.CRITERIA.SPICESTATION.DESCRIPTION, null, null);

	// Token: 0x0400382C RID: 14380
	public static RoomConstraints.Constraint COOK_TOP = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.CookTop), null, 1, ROOMS.CRITERIA.COOKTOP.NAME, ROOMS.CRITERIA.COOKTOP.DESCRIPTION, null, null);

	// Token: 0x0400382D RID: 14381
	public static RoomConstraints.Constraint REFRIGERATOR = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.Refrigerator), null, 1, ROOMS.CRITERIA.REFRIGERATOR.NAME, ROOMS.CRITERIA.REFRIGERATOR.DESCRIPTION, null, null);

	// Token: 0x0400382E RID: 14382
	public static RoomConstraints.Constraint REC_BUILDING = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.RecBuilding), null, 1, ROOMS.CRITERIA.RECBUILDING.NAME, ROOMS.CRITERIA.RECBUILDING.DESCRIPTION, null, null);

	// Token: 0x0400382F RID: 14383
	public static RoomConstraints.Constraint MACHINE_SHOP = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.MachineShopType), null, 1, ROOMS.CRITERIA.MACHINESHOPTYPE.NAME, ROOMS.CRITERIA.MACHINESHOPTYPE.DESCRIPTION, null, null);

	// Token: 0x04003830 RID: 14384
	[Obsolete("The light requirement constraint in rooms has been removed. This is retained solely to avoid breaking mods")]
	public static RoomConstraints.Constraint LIGHT = new RoomConstraints.Constraint(null, null, 1, ROOMS.CRITERIA.LIGHTSOURCE.NAME, ROOMS.CRITERIA.LIGHTSOURCE.DESCRIPTION, null, null);

	// Token: 0x04003831 RID: 14385
	public static RoomConstraints.Constraint DESTRESSING_BUILDING = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.DeStressingBuilding), null, 1, ROOMS.CRITERIA.DESTRESSINGBUILDING.NAME, ROOMS.CRITERIA.DESTRESSINGBUILDING.DESCRIPTION, null, null);

	// Token: 0x04003832 RID: 14386
	public static RoomConstraints.Constraint MASSAGE_TABLE = new RoomConstraints.Constraint((KPrefabID bc) => bc.IsPrefabID(RoomConstraints.ConstraintTags.MassageTable), null, 1, ROOMS.CRITERIA.MASSAGE_TABLE.NAME, ROOMS.CRITERIA.MASSAGE_TABLE.DESCRIPTION, null, null);

	// Token: 0x04003833 RID: 14387
	public static RoomConstraints.Constraint DINING_TABLE = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.DiningTableType), null, 1, ROOMS.CRITERIA.DININGTABLETYPE.NAME, ROOMS.CRITERIA.DININGTABLETYPE.DESCRIPTION, new List<RoomConstraints.Constraint>
	{
		RoomConstraints.REC_BUILDING,
		RoomConstraints.MESS_STATION_SINGLE,
		RoomConstraints.MULTI_MINION_DINING_TABLE
	}, null);

	// Token: 0x04003834 RID: 14388
	public static RoomConstraints.Constraint MESS_STATION_SINGLE = new RoomConstraints.Constraint((KPrefabID bc) => bc.IsPrefabID("DiningTable"), null, 1, ROOMS.CRITERIA.DININGTABLETYPE.NAME, ROOMS.CRITERIA.DININGTABLETYPE.DESCRIPTION, new List<RoomConstraints.Constraint>
	{
		RoomConstraints.REC_BUILDING,
		RoomConstraints.DINING_TABLE
	}, null);

	// Token: 0x04003835 RID: 14389
	public static RoomConstraints.Constraint MULTI_MINION_DINING_TABLE = new RoomConstraints.Constraint((KPrefabID bc) => bc.IsPrefabID("MultiMinionDiningTable") || bc.gameObject.name == "MultiMinionDiningSeat", null, 1, ROOMS.CRITERIA.MULTI_MINION_DINING_TABLE.NAME, ROOMS.CRITERIA.MULTI_MINION_DINING_TABLE.DESCRIPTION, new List<RoomConstraints.Constraint>
	{
		RoomConstraints.REC_BUILDING,
		RoomConstraints.DINING_TABLE
	}, null);

	// Token: 0x04003836 RID: 14390
	public static RoomConstraints.Constraint TOILET = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.ToiletType), null, 1, ROOMS.CRITERIA.TOILETTYPE.NAME, ROOMS.CRITERIA.TOILETTYPE.DESCRIPTION, null, null);

	// Token: 0x04003837 RID: 14391
	public static RoomConstraints.Constraint BIONICUPKEEP = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.BionicUpkeepType), null, 2, ROOMS.CRITERIA.BIONICUPKEEP.NAME, ROOMS.CRITERIA.BIONICUPKEEP.DESCRIPTION, null, null);

	// Token: 0x04003838 RID: 14392
	public static RoomConstraints.Constraint BIONIC_LUBRICATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag("OilChanger"), null, 1, ROOMS.CRITERIA.BIONIC_LUBRICATION.NAME, ROOMS.CRITERIA.BIONIC_LUBRICATION.DESCRIPTION, null, null);

	// Token: 0x04003839 RID: 14393
	public static RoomConstraints.Constraint BIONIC_GUNKEMPTIER = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag("GunkEmptier"), null, 1, ROOMS.CRITERIA.BIONIC_GUNKEMPTIER.NAME, ROOMS.CRITERIA.BIONIC_GUNKEMPTIER.DESCRIPTION, null, null);

	// Token: 0x0400383A RID: 14394
	public static RoomConstraints.Constraint FLUSH_TOILET = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.FlushToiletType), null, 1, ROOMS.CRITERIA.FLUSHTOILETTYPE.NAME, ROOMS.CRITERIA.FLUSHTOILETTYPE.DESCRIPTION, null, null);

	// Token: 0x0400383B RID: 14395
	public static RoomConstraints.Constraint WASH_STATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.WashStation), null, 1, ROOMS.CRITERIA.WASHSTATION.NAME, ROOMS.CRITERIA.WASHSTATION.DESCRIPTION, null, null);

	// Token: 0x0400383C RID: 14396
	public static RoomConstraints.Constraint ADVANCEDWASHSTATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.AdvancedWashStation), null, 1, ROOMS.CRITERIA.ADVANCEDWASHSTATION.NAME, ROOMS.CRITERIA.ADVANCEDWASHSTATION.DESCRIPTION, null, null);

	// Token: 0x0400383D RID: 14397
	public static RoomConstraints.Constraint CLINIC = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.Clinic), null, 1, ROOMS.CRITERIA.CLINIC.NAME, ROOMS.CRITERIA.CLINIC.DESCRIPTION, new List<RoomConstraints.Constraint>
	{
		RoomConstraints.TOILET,
		RoomConstraints.FLUSH_TOILET,
		RoomConstraints.MESS_STATION_SINGLE
	}, null);

	// Token: 0x0400383E RID: 14398
	public static RoomConstraints.Constraint PARK_BUILDING = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.Park), null, 1, ROOMS.CRITERIA.PARK.NAME, ROOMS.CRITERIA.PARK.DESCRIPTION, null, null);

	// Token: 0x0400383F RID: 14399
	public static RoomConstraints.Constraint ORIGINALTILES = new RoomConstraints.Constraint(null, (Room room) => 1 + room.cavity.maxY - room.cavity.minY >= 4, 1, "", "", null, null);

	// Token: 0x04003840 RID: 14400
	public static RoomConstraints.Constraint IS_BACKWALLED = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		bool flag = true;
		int num = (room.cavity.maxX - room.cavity.minX + 1) / 2 + 1;
		int num2 = 0;
		while (flag && num2 < num)
		{
			int x = room.cavity.minX + num2;
			int x2 = room.cavity.maxX - num2;
			int num3 = room.cavity.minY;
			while (flag && num3 <= room.cavity.maxY)
			{
				int cell = Grid.XYToCell(x, num3);
				int cell2 = Grid.XYToCell(x2, num3);
				if (Game.Instance.roomProber.GetCavityForCell(cell) == room.cavity)
				{
					GameObject gameObject = Grid.Objects[cell, 2];
					flag &= (gameObject != null && !gameObject.HasTag(GameTags.UnderConstruction));
				}
				if (Game.Instance.roomProber.GetCavityForCell(cell2) == room.cavity)
				{
					GameObject gameObject2 = Grid.Objects[cell2, 2];
					flag &= (gameObject2 != null && !gameObject2.HasTag(GameTags.UnderConstruction));
				}
				if (!flag)
				{
					return false;
				}
				num3++;
			}
			num2++;
		}
		return flag;
	}, 1, ROOMS.CRITERIA.IS_BACKWALLED.NAME, ROOMS.CRITERIA.IS_BACKWALLED.DESCRIPTION, null, null);

	// Token: 0x04003841 RID: 14401
	public static RoomConstraints.Constraint WILDANIMAL = new RoomConstraints.Constraint(null, (Room room) => room.cavity.creatures.Count + room.cavity.eggs.Count > 0, 1, ROOMS.CRITERIA.WILDANIMAL.NAME, ROOMS.CRITERIA.WILDANIMAL.DESCRIPTION, null, null);

	// Token: 0x04003842 RID: 14402
	public static RoomConstraints.Constraint WILDANIMALS = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		int num = 0;
		using (List<KPrefabID>.Enumerator enumerator = room.cavity.creatures.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasTag(GameTags.Creatures.Wild))
				{
					num++;
				}
			}
		}
		return num >= 2;
	}, 1, ROOMS.CRITERIA.WILDANIMALS.NAME, ROOMS.CRITERIA.WILDANIMALS.DESCRIPTION, null, null);

	// Token: 0x04003843 RID: 14403
	public static RoomConstraints.Constraint WILDPLANT = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		int num = 0;
		foreach (KPrefabID kprefabID in room.cavity.plants)
		{
			if (kprefabID != null && !kprefabID.HasTag(GameTags.PlantBranch))
			{
				BasicForagePlantPlanted component = kprefabID.GetComponent<BasicForagePlantPlanted>();
				ReceptacleMonitor component2 = kprefabID.GetComponent<ReceptacleMonitor>();
				if (component2 != null && !component2.Replanted)
				{
					num++;
				}
				else if (component != null)
				{
					num++;
				}
			}
		}
		return num >= 2;
	}, 1, ROOMS.CRITERIA.WILDPLANT.NAME, ROOMS.CRITERIA.WILDPLANT.DESCRIPTION, null, null);

	// Token: 0x04003844 RID: 14404
	public static RoomConstraints.Constraint WILDPLANTS = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		int num = 0;
		foreach (KPrefabID kprefabID in room.cavity.plants)
		{
			if (kprefabID != null && !kprefabID.HasTag(GameTags.PlantBranch))
			{
				BasicForagePlantPlanted component = kprefabID.GetComponent<BasicForagePlantPlanted>();
				ReceptacleMonitor component2 = kprefabID.GetComponent<ReceptacleMonitor>();
				if (component2 != null && !component2.Replanted)
				{
					num++;
				}
				else if (component != null)
				{
					num++;
				}
			}
		}
		return num >= 4;
	}, 1, ROOMS.CRITERIA.WILDPLANTS.NAME, ROOMS.CRITERIA.WILDPLANTS.DESCRIPTION, null, null);

	// Token: 0x02001C69 RID: 7273
	public static class ConstraintTags
	{
		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x0600AD58 RID: 44376 RVA: 0x003CF28A File Offset: 0x003CD48A
		// (set) Token: 0x0600AD59 RID: 44377 RVA: 0x003CF291 File Offset: 0x003CD491
		public static Tag DecorFancy { get; internal set; }

		// Token: 0x0600AD5A RID: 44378 RVA: 0x003CF29C File Offset: 0x003CD49C
		public static string GetRoomConstraintLabelText(Tag tag)
		{
			StringEntry entry = null;
			string text = "STRINGS.ROOMS.CRITERIA." + tag.ToString().ToUpper() + ".NAME";
			if (!Strings.TryGet(new StringKey(text), out entry))
			{
				return ROOMS.CRITERIA.IN_CODE_ERROR.text.Replace("{0}", text);
			}
			return entry;
		}

		// Token: 0x040087E8 RID: 34792
		public static List<Tag> AllTags = new List<Tag>();

		// Token: 0x040087E9 RID: 34793
		public static Tag BedType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("BedType".ToTag());

		// Token: 0x040087EA RID: 34794
		public static Tag LuxuryBedType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("LuxuryBedType".ToTag());

		// Token: 0x040087EB RID: 34795
		public static Tag ToiletType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("ToiletType".ToTag());

		// Token: 0x040087EC RID: 34796
		public static Tag BionicUpkeepType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("BionicUpkeep".ToTag());

		// Token: 0x040087ED RID: 34797
		public static Tag FlushToiletType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("FlushToiletType".ToTag());

		// Token: 0x040087EE RID: 34798
		public static Tag MessTable = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("MessTable".ToTag());

		// Token: 0x040087EF RID: 34799
		public static Tag DiningTableType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("DiningTableType".ToTag());

		// Token: 0x040087F0 RID: 34800
		public static Tag Clinic = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("Clinic".ToTag());

		// Token: 0x040087F1 RID: 34801
		public static Tag WashStation = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("WashStation".ToTag());

		// Token: 0x040087F2 RID: 34802
		public static Tag AdvancedWashStation = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("AdvancedWashStation".ToTag());

		// Token: 0x040087F3 RID: 34803
		public static Tag ScienceBuilding = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("ScienceBuilding".ToTag());

		// Token: 0x040087F4 RID: 34804
		public static Tag MassageTable = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("MassageTable".ToTag());

		// Token: 0x040087F5 RID: 34805
		public static Tag DeStressingBuilding = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("DeStressingBuilding".ToTag());

		// Token: 0x040087F6 RID: 34806
		public static Tag IndustrialMachinery = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("IndustrialMachinery".ToTag());

		// Token: 0x040087F7 RID: 34807
		public static Tag GeneratorType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("GeneratorType".ToTag());

		// Token: 0x040087F8 RID: 34808
		public static Tag HeavyDutyGeneratorType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("HeavyDutyGeneratorType".ToTag());

		// Token: 0x040087F9 RID: 34809
		public static Tag LightDutyGeneratorType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("LightDutyGeneratorType".ToTag());

		// Token: 0x040087FA RID: 34810
		public static Tag PowerBuilding = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("PowerBuilding".ToTag());

		// Token: 0x040087FB RID: 34811
		public static Tag FarmStationType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("FarmStationType".ToTag());

		// Token: 0x040087FC RID: 34812
		public static Tag RanchStationType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("RanchStationType".ToTag());

		// Token: 0x040087FD RID: 34813
		public static Tag SpiceStation = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("SpiceStation".ToTag());

		// Token: 0x040087FE RID: 34814
		public static Tag CookTop = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("CookTop".ToTag());

		// Token: 0x040087FF RID: 34815
		public static Tag Refrigerator = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("Refrigerator".ToTag());

		// Token: 0x04008800 RID: 34816
		public static Tag RecBuilding = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("RecBuilding".ToTag());

		// Token: 0x04008801 RID: 34817
		public static Tag MachineShopType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("MachineShopType".ToTag());

		// Token: 0x04008802 RID: 34818
		public static Tag Park = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("Park".ToTag());

		// Token: 0x04008803 RID: 34819
		public static Tag NatureReserve = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("NatureReserve".ToTag());

		// Token: 0x04008804 RID: 34820
		public static Tag RocketInterior = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("RocketInterior".ToTag());

		// Token: 0x04008805 RID: 34821
		public static Tag Decoration = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(GameTags.Decoration);

		// Token: 0x04008806 RID: 34822
		public static Tag Ornament = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("Ornament".ToTag());

		// Token: 0x04008807 RID: 34823
		public static Tag WarmingStation = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("WarmingStation".ToTag());

		// Token: 0x04008808 RID: 34824
		[Obsolete("The light requirement constraint in rooms has been removed. Please update any references of RoomConstraints.LightSource to GameTags.Lightsource")]
		public static Tag LightSource = "LightSource".ToTag();
	}

	// Token: 0x02001C6A RID: 7274
	public class Constraint
	{
		// Token: 0x0600AD5C RID: 44380 RVA: 0x003CF620 File Offset: 0x003CD820
		public Constraint(Func<KPrefabID, bool> building_criteria, Func<Room, bool> room_criteria, int times_required = 1, string name = "", string description = "", List<RoomConstraints.Constraint> stomp_in_conflict = null, string overrideConstraintConflictName = null) : this(null, building_criteria, room_criteria, times_required, name, description, stomp_in_conflict, overrideConstraintConflictName)
		{
		}

		// Token: 0x0600AD5D RID: 44381 RVA: 0x003CF640 File Offset: 0x003CD840
		public Constraint(Func<KPrefabID, bool> creature_criteria, Func<KPrefabID, bool> building_criteria, Func<Room, bool> room_criteria, int times_required = 1, string name = "", string description = "", List<RoomConstraints.Constraint> stomp_in_conflict = null, string overrideConstraintConflictName = null)
		{
			this.creature_criteria = creature_criteria;
			this.room_criteria = room_criteria;
			this.building_criteria = building_criteria;
			this.times_required = times_required;
			this.name = name;
			this.description = description;
			this.stomp_in_conflict = stomp_in_conflict;
			this.conflictDescription = ((overrideConstraintConflictName == null) ? name : overrideConstraintConflictName);
		}

		// Token: 0x0600AD5E RID: 44382 RVA: 0x003CF6A0 File Offset: 0x003CD8A0
		public bool isSatisfied(Room room)
		{
			int num = 0;
			if (this.room_criteria != null && !this.room_criteria(room))
			{
				return false;
			}
			if (this.building_criteria != null)
			{
				int num2 = 0;
				while (num < this.times_required && num2 < room.buildings.Count)
				{
					KPrefabID kprefabID = room.buildings[num2];
					if (!(kprefabID == null) && this.building_criteria(kprefabID))
					{
						num++;
					}
					num2++;
				}
				int num3 = 0;
				while (num < this.times_required && num3 < room.plants.Count)
				{
					KPrefabID kprefabID2 = room.plants[num3];
					if (!(kprefabID2 == null) && this.building_criteria(kprefabID2))
					{
						num++;
					}
					num3++;
				}
				return num >= this.times_required;
			}
			Func<KPrefabID, bool> func = this.creature_criteria;
			return true;
		}

		// Token: 0x0400880A RID: 34826
		public string name;

		// Token: 0x0400880B RID: 34827
		public string description;

		// Token: 0x0400880C RID: 34828
		public string conflictDescription;

		// Token: 0x0400880D RID: 34829
		public int times_required = 1;

		// Token: 0x0400880E RID: 34830
		public Func<Room, bool> room_criteria;

		// Token: 0x0400880F RID: 34831
		public Func<KPrefabID, bool> building_criteria;

		// Token: 0x04008810 RID: 34832
		public Func<KPrefabID, bool> creature_criteria;

		// Token: 0x04008811 RID: 34833
		public List<RoomConstraints.Constraint> stomp_in_conflict;
	}
}
