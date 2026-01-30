using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F52 RID: 3922
	public class RoomTypeCategories : ResourceSet<RoomTypeCategory>
	{
		// Token: 0x06007CB9 RID: 31929 RVA: 0x0031629C File Offset: 0x0031449C
		private RoomTypeCategory Add(string id, string name, string colorName, string icon)
		{
			RoomTypeCategory roomTypeCategory = new RoomTypeCategory(id, name, colorName, icon);
			base.Add(roomTypeCategory);
			return roomTypeCategory;
		}

		// Token: 0x06007CBA RID: 31930 RVA: 0x003162C0 File Offset: 0x003144C0
		public RoomTypeCategories(ResourceSet parent) : base("RoomTypeCategories", parent)
		{
			base.Initialize();
			this.None = this.Add("None", ROOMS.CATEGORY.NONE.NAME, "roomNone", "unknown");
			this.Food = this.Add("Food", ROOMS.CATEGORY.FOOD.NAME, "roomFood", "ui_room_food");
			this.Sleep = this.Add("Sleep", ROOMS.CATEGORY.SLEEP.NAME, "roomSleep", "ui_room_sleep");
			this.Recreation = this.Add("Recreation", ROOMS.CATEGORY.RECREATION.NAME, "roomRecreation", "ui_room_recreational");
			if (DlcManager.IsContentSubscribed("DLC3_ID"))
			{
				this.Bionic = this.Add("Bionic", ROOMS.CATEGORY.BIONIC.NAME, "roomBionic", "ui_room_bionicupkeep");
			}
			this.Bathroom = this.Add("Bathroom", ROOMS.CATEGORY.BATHROOM.NAME, "roomBathroom", "ui_room_bathroom");
			this.Hospital = this.Add("Hospital", ROOMS.CATEGORY.HOSPITAL.NAME, "roomHospital", "ui_room_hospital");
			this.Industrial = this.Add("Industrial", ROOMS.CATEGORY.INDUSTRIAL.NAME, "roomIndustrial", "ui_room_industrial");
			this.Agricultural = this.Add("Agricultural", ROOMS.CATEGORY.AGRICULTURAL.NAME, "roomAgricultural", "ui_room_agricultural");
			this.Park = this.Add("Park", ROOMS.CATEGORY.PARK.NAME, "roomPark", "ui_room_park");
			this.Science = this.Add("Science", ROOMS.CATEGORY.SCIENCE.NAME, "roomScience", "ui_room_science");
		}

		// Token: 0x04005B3A RID: 23354
		public RoomTypeCategory None;

		// Token: 0x04005B3B RID: 23355
		public RoomTypeCategory Food;

		// Token: 0x04005B3C RID: 23356
		public RoomTypeCategory Sleep;

		// Token: 0x04005B3D RID: 23357
		public RoomTypeCategory Recreation;

		// Token: 0x04005B3E RID: 23358
		public RoomTypeCategory Bathroom;

		// Token: 0x04005B3F RID: 23359
		public RoomTypeCategory Bionic;

		// Token: 0x04005B40 RID: 23360
		public RoomTypeCategory Hospital;

		// Token: 0x04005B41 RID: 23361
		public RoomTypeCategory Industrial;

		// Token: 0x04005B42 RID: 23362
		public RoomTypeCategory Agricultural;

		// Token: 0x04005B43 RID: 23363
		public RoomTypeCategory Park;

		// Token: 0x04005B44 RID: 23364
		public RoomTypeCategory Science;
	}
}
