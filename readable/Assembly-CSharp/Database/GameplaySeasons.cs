using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02000F40 RID: 3904
	public class GameplaySeasons : ResourceSet<GameplaySeason>
	{
		// Token: 0x06007C7B RID: 31867 RVA: 0x00312A59 File Offset: 0x00310C59
		public GameplaySeasons(ResourceSet parent) : base("GameplaySeasons", parent)
		{
			this.VanillaSeasons();
			this.Expansion1Seasons();
			this.DLCSeasons();
			this.UnusedSeasons();
		}

		// Token: 0x06007C7C RID: 31868 RVA: 0x00312A80 File Offset: 0x00310C80
		private void VanillaSeasons()
		{
			this.MeteorShowers = base.Add(new MeteorShowerSeason("MeteorShowers", GameplaySeason.Type.World, 14f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, -1f, null, null).AddEvent(Db.Get().GameplayEvents.MeteorShowerIronEvent).AddEvent(Db.Get().GameplayEvents.MeteorShowerGoldEvent).AddEvent(Db.Get().GameplayEvents.MeteorShowerCopperEvent));
		}

		// Token: 0x06007C7D RID: 31869 RVA: 0x00312B00 File Offset: 0x00310D00
		private void Expansion1Seasons()
		{
			this.RegolithMoonMeteorShowers = base.Add(new MeteorShowerSeason("RegolithMoonMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.MeteorShowerDustEvent).AddEvent(Db.Get().GameplayEvents.ClusterIronShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower));
			this.TemporalTearMeteorShowers = base.Add(new MeteorShowerSeason("TemporalTearMeteorShowers", GameplaySeason.Type.World, 1f, false, 0f, false, -1, 0f, float.PositiveInfinity, 1, false, -1f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.MeteorShowerFullereneEvent));
			this.GassyMooteorShowers = base.Add(new MeteorShowerSeason("GassyMooteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, false, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.GassyMooteorEvent));
			this.SpacedOutStyleStartMeteorShowers = base.Add(new MeteorShowerSeason("SpacedOutStyleStartMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null));
			this.SpacedOutStyleRocketMeteorShowers = base.Add(new MeteorShowerSeason("SpacedOutStyleRocketMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower));
			this.SpacedOutStyleWarpMeteorShowers = base.Add(new MeteorShowerSeason("SpacedOutStyleWarpMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower));
			this.ClassicStyleStartMeteorShowers = base.Add(new MeteorShowerSeason("ClassicStyleStartMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower));
			this.ClassicStyleWarpMeteorShowers = base.Add(new MeteorShowerSeason("ClassicStyleWarpMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower).AddEvent(Db.Get().GameplayEvents.ClusterIronShower));
			this.TundraMoonletMeteorShowers = base.Add(new MeteorShowerSeason("TundraMoonletMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null));
			this.MarshyMoonletMeteorShowers = base.Add(new MeteorShowerSeason("MarshyMoonletMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null));
			this.NiobiumMoonletMeteorShowers = base.Add(new MeteorShowerSeason("NiobiumMoonletMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null));
			this.WaterMoonletMeteorShowers = base.Add(new MeteorShowerSeason("WaterMoonletMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null));
			this.MiniMetallicSwampyMeteorShowers = base.Add(new MeteorShowerSeason("MiniMetallicSwampyMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower));
			this.MiniForestFrozenMeteorShowers = base.Add(new MeteorShowerSeason("MiniForestFrozenMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower));
			this.MiniBadlandsMeteorShowers = base.Add(new MeteorShowerSeason("MiniBadlandsMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.ClusterIceShower));
			this.MiniFlippedMeteorShowers = base.Add(new MeteorShowerSeason("MiniFlippedMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null));
			this.MiniRadioactiveOceanMeteorShowers = base.Add(new MeteorShowerSeason("MiniRadioactiveOceanMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1, null).AddEvent(Db.Get().GameplayEvents.ClusterUraniumShower));
		}

		// Token: 0x06007C7E RID: 31870 RVA: 0x00313074 File Offset: 0x00311274
		private void DLCSeasons()
		{
			this.CeresMeteorShowers = base.Add(new MeteorShowerSeason("CeresMeteorShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 10f, float.PositiveInfinity, 1, true, 6000f, DlcManager.DLC2, null).AddEvent(Db.Get().GameplayEvents.ClusterIceAndTreesShower));
			this.MiniCeresStartShowers = base.Add(new MeteorShowerSeason("MiniCeresStartShowers", GameplaySeason.Type.World, 20f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.EXPANSION1.Append(DlcManager.DLC2), null).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower).AddEvent(Db.Get().GameplayEvents.ClusterSnowShower));
			this.LargeImpactor = base.Add(new GameplaySeason("LargeImpactor", GameplaySeason.Type.World, 1f, false, -1f, true, 1, 0f, float.PositiveInfinity, 1, DlcManager.DLC4, null).AddEvent(Db.Get().GameplayEvents.LargeImpactor));
			this.PrehistoricMeteorShowers = base.Add(new MeteorShowerSeason("PrehistoricMeteorShowers", GameplaySeason.Type.World, 50f, false, -1f, true, -1, 0f, float.PositiveInfinity, 1, true, 6000f, DlcManager.DLC4, null).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIronShower).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower));
		}

		// Token: 0x06007C7F RID: 31871 RVA: 0x003131FD File Offset: 0x003113FD
		private void UnusedSeasons()
		{
		}

		// Token: 0x04005A6E RID: 23150
		public GameplaySeason NaturalRandomEvents;

		// Token: 0x04005A6F RID: 23151
		public GameplaySeason DupeRandomEvents;

		// Token: 0x04005A70 RID: 23152
		public GameplaySeason PrickleCropSeason;

		// Token: 0x04005A71 RID: 23153
		public GameplaySeason BonusEvents;

		// Token: 0x04005A72 RID: 23154
		public GameplaySeason MeteorShowers;

		// Token: 0x04005A73 RID: 23155
		public GameplaySeason TemporalTearMeteorShowers;

		// Token: 0x04005A74 RID: 23156
		public GameplaySeason SpacedOutStyleStartMeteorShowers;

		// Token: 0x04005A75 RID: 23157
		public GameplaySeason SpacedOutStyleRocketMeteorShowers;

		// Token: 0x04005A76 RID: 23158
		public GameplaySeason SpacedOutStyleWarpMeteorShowers;

		// Token: 0x04005A77 RID: 23159
		public GameplaySeason ClassicStyleStartMeteorShowers;

		// Token: 0x04005A78 RID: 23160
		public GameplaySeason ClassicStyleWarpMeteorShowers;

		// Token: 0x04005A79 RID: 23161
		public GameplaySeason TundraMoonletMeteorShowers;

		// Token: 0x04005A7A RID: 23162
		public GameplaySeason MarshyMoonletMeteorShowers;

		// Token: 0x04005A7B RID: 23163
		public GameplaySeason NiobiumMoonletMeteorShowers;

		// Token: 0x04005A7C RID: 23164
		public GameplaySeason WaterMoonletMeteorShowers;

		// Token: 0x04005A7D RID: 23165
		public GameplaySeason GassyMooteorShowers;

		// Token: 0x04005A7E RID: 23166
		public GameplaySeason RegolithMoonMeteorShowers;

		// Token: 0x04005A7F RID: 23167
		public GameplaySeason MiniMetallicSwampyMeteorShowers;

		// Token: 0x04005A80 RID: 23168
		public GameplaySeason MiniForestFrozenMeteorShowers;

		// Token: 0x04005A81 RID: 23169
		public GameplaySeason MiniBadlandsMeteorShowers;

		// Token: 0x04005A82 RID: 23170
		public GameplaySeason MiniFlippedMeteorShowers;

		// Token: 0x04005A83 RID: 23171
		public GameplaySeason MiniRadioactiveOceanMeteorShowers;

		// Token: 0x04005A84 RID: 23172
		public GameplaySeason MiniCeresStartShowers;

		// Token: 0x04005A85 RID: 23173
		public GameplaySeason CeresMeteorShowers;

		// Token: 0x04005A86 RID: 23174
		public GameplaySeason LargeImpactor;

		// Token: 0x04005A87 RID: 23175
		public GameplaySeason PrehistoricMeteorShowers;
	}
}
