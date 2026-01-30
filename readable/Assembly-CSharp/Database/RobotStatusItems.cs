using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F51 RID: 3921
	public class RobotStatusItems : StatusItems
	{
		// Token: 0x06007CB7 RID: 31927 RVA: 0x00315EE5 File Offset: 0x003140E5
		public RobotStatusItems(ResourceSet parent) : base("RobotStatusItems", parent)
		{
			this.CreateStatusItems();
		}

		// Token: 0x06007CB8 RID: 31928 RVA: 0x00315EFC File Offset: 0x003140FC
		private void CreateStatusItems()
		{
			this.CantReachStation = new StatusItem("CantReachStation", "ROBOTS", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.CantReachStation.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.LowBattery = new StatusItem("LowBattery", "ROBOTS", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.LowBattery.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.LowBatteryNoCharge = new StatusItem("LowBatteryNoCharge", "ROBOTS", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.LowBatteryNoCharge.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.DeadBattery = new StatusItem("DeadBattery", "ROBOTS", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.DeadBattery.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.DeadBatteryFlydo = new StatusItem("DeadBatteryFlydo", "ROBOTS", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.DeadBatteryFlydo.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.DustBinFull = new StatusItem("DustBinFull", "ROBOTS", "status_item_pending_clear", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.DustBinFull.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.Working = new StatusItem("Working", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.Working.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.MovingToChargeStation = new StatusItem("MovingToChargeStation", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.MovingToChargeStation.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.UnloadingStorage = new StatusItem("UnloadingStorage", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.UnloadingStorage.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.ReactPositive = new StatusItem("ReactPositive", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.ReactPositive.resolveStringCallback = ((string str, object data) => str);
			this.ReactNegative = new StatusItem("ReactNegative", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.ReactNegative.resolveStringCallback = ((string str, object data) => str);
		}

		// Token: 0x04005B2F RID: 23343
		public StatusItem LowBattery;

		// Token: 0x04005B30 RID: 23344
		public StatusItem LowBatteryNoCharge;

		// Token: 0x04005B31 RID: 23345
		public StatusItem DeadBattery;

		// Token: 0x04005B32 RID: 23346
		public StatusItem DeadBatteryFlydo;

		// Token: 0x04005B33 RID: 23347
		public StatusItem CantReachStation;

		// Token: 0x04005B34 RID: 23348
		public StatusItem DustBinFull;

		// Token: 0x04005B35 RID: 23349
		public StatusItem Working;

		// Token: 0x04005B36 RID: 23350
		public StatusItem UnloadingStorage;

		// Token: 0x04005B37 RID: 23351
		public StatusItem ReactPositive;

		// Token: 0x04005B38 RID: 23352
		public StatusItem ReactNegative;

		// Token: 0x04005B39 RID: 23353
		public StatusItem MovingToChargeStation;
	}
}
