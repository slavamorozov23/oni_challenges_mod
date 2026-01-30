using System;
using STRINGS;

namespace Klei.CustomSettings
{
	// Token: 0x0200101B RID: 4123
	public static class CustomMixingSettingsConfigs
	{
		// Token: 0x040060FC RID: 24828
		public static SettingConfig DLC2Mixing = new DlcMixingSettingConfig("DLC2_ID", UI.DLC2.NAME, UI.DLC2.MIXING_TOOLTIP, 5L, false, DlcManager.DLC2, "DLC2_ID", "");

		// Token: 0x040060FD RID: 24829
		public static SettingConfig DLC3Mixing = new DlcMixingSettingConfig("DLC3_ID", UI.DLC3.NAME, UI.DLC3.MIXING_TOOLTIP, 5L, false, DlcManager.DLC3, "DLC3_ID", "");

		// Token: 0x040060FE RID: 24830
		public static SettingConfig DLC4Mixing = new DlcMixingSettingConfig("DLC4_ID", UI.DLC4.NAME, UI.DLC4.MIXING_TOOLTIP, 5L, false, DlcManager.DLC4, "DLC4_ID", "");

		// Token: 0x040060FF RID: 24831
		public static SettingConfig CeresAsteroidMixing = new WorldMixingSettingConfig("CeresAsteroidMixing", "dlc2::worldMixing/CeresMixingSettings", DlcManager.DLC2, "DLC2_ID", true, 5L);

		// Token: 0x04006100 RID: 24832
		public static SettingConfig PrehistoricAsteroidMixing = new WorldMixingSettingConfig("PrehistoricAsteroidMixing", "dlc4::worldMixing/PrehistoricMixingSettings", DlcManager.DLC4, "DLC4_ID", true, 5L);

		// Token: 0x04006101 RID: 24833
		public static SettingConfig IceCavesMixing = new SubworldMixingSettingConfig("IceCavesMixing", "dlc2::subworldMixing/IceCavesMixingSettings", DlcManager.DLC2, "DLC2_ID", true, 5L);

		// Token: 0x04006102 RID: 24834
		public static SettingConfig CarrotQuarryMixing = new SubworldMixingSettingConfig("CarrotQuarryMixing", "dlc2::subworldMixing/CarrotQuarryMixingSettings", DlcManager.DLC2, "DLC2_ID", true, 5L);

		// Token: 0x04006103 RID: 24835
		public static SettingConfig SugarWoodsMixing = new SubworldMixingSettingConfig("SugarWoodsMixing", "dlc2::subworldMixing/SugarWoodsMixingSettings", DlcManager.DLC2, "DLC2_ID", true, 5L);

		// Token: 0x04006104 RID: 24836
		public static SettingConfig GardenMixing = new SubworldMixingSettingConfig("GardenMixing", "dlc4::subworldMixing/GardenMixingSettings", DlcManager.DLC4, "DLC4_ID", true, 5L);

		// Token: 0x04006105 RID: 24837
		public static SettingConfig RaptorMixing = new SubworldMixingSettingConfig("RaptorMixing", "dlc4::subworldMixing/RaptorMixingSettings", DlcManager.DLC4, "DLC4_ID", true, 5L);

		// Token: 0x04006106 RID: 24838
		public static SettingConfig WetlandsMixing = new SubworldMixingSettingConfig("WetlandsMixing", "dlc4::subworldMixing/WetlandsMixingSettings", DlcManager.DLC4, "DLC4_ID", true, 5L);
	}
}
