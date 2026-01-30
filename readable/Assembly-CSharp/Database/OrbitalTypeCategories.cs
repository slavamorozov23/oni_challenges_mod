using System;

namespace Database
{
	// Token: 0x02000F44 RID: 3908
	public class OrbitalTypeCategories : ResourceSet<OrbitalData>
	{
		// Token: 0x06007C8F RID: 31887 RVA: 0x003143B8 File Offset: 0x003125B8
		public OrbitalTypeCategories(ResourceSet parent) : base("OrbitalTypeCategories", parent)
		{
			this.backgroundEarth = new OrbitalData("backgroundEarth", this, "earth_kanim", "", OrbitalData.OrbitalType.world, 1f, 0.5f, 0.95f, 10f, 10f, 1.05f, true, 0.05f, 25f, 1f);
			this.backgroundEarth.GetRenderZ = (() => Grid.GetLayerZ(Grid.SceneLayer.Background) + 0.9f);
			this.frozenOre = new OrbitalData("frozenOre", this, "starmap_frozen_ore_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1f, true, 0.05f, 25f, 1f);
			this.heliumCloud = new OrbitalData("heliumCloud", this, "starmap_helium_cloud_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.iceCloud = new OrbitalData("iceCloud", this, "starmap_ice_cloud_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.iceRock = new OrbitalData("iceRock", this, "starmap_ice_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.purpleGas = new OrbitalData("purpleGas", this, "starmap_purple_gas_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.radioactiveGas = new OrbitalData("radioactiveGas", this, "starmap_radioactive_gas_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.rocky = new OrbitalData("rocky", this, "starmap_rocky_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.gravitas = new OrbitalData("gravitas", this, "starmap_space_junk_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.orbit = new OrbitalData("orbit", this, "starmap_orbit_kanim", "", OrbitalData.OrbitalType.inOrbit, 1f, 0.25f, 0.5f, -350f, 350f, 1.05f, false, 0.05f, 4f, 1f);
			this.landed = new OrbitalData("landed", this, "starmap_landed_surface_kanim", "", OrbitalData.OrbitalType.landed, 0f, 0.5f, 0.35f, -350f, 350f, 1.05f, false, 0.05f, 4f, 1f);
		}

		// Token: 0x04005ACB RID: 23243
		public OrbitalData backgroundEarth;

		// Token: 0x04005ACC RID: 23244
		public OrbitalData frozenOre;

		// Token: 0x04005ACD RID: 23245
		public OrbitalData heliumCloud;

		// Token: 0x04005ACE RID: 23246
		public OrbitalData iceCloud;

		// Token: 0x04005ACF RID: 23247
		public OrbitalData iceRock;

		// Token: 0x04005AD0 RID: 23248
		public OrbitalData purpleGas;

		// Token: 0x04005AD1 RID: 23249
		public OrbitalData radioactiveGas;

		// Token: 0x04005AD2 RID: 23250
		public OrbitalData rocky;

		// Token: 0x04005AD3 RID: 23251
		public OrbitalData gravitas;

		// Token: 0x04005AD4 RID: 23252
		public OrbitalData orbit;

		// Token: 0x04005AD5 RID: 23253
		public OrbitalData landed;
	}
}
