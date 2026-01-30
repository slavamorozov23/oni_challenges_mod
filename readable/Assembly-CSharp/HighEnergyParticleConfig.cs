using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200098B RID: 2443
public class HighEnergyParticleConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06004636 RID: 17974 RVA: 0x00195AE2 File Offset: 0x00193CE2
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06004637 RID: 17975 RVA: 0x00195AE9 File Offset: 0x00193CE9
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06004638 RID: 17976 RVA: 0x00195AEC File Offset: 0x00193CEC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateBasicEntity("HighEnergyParticle", ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, ITEMS.RADIATION.HIGHENERGYPARITCLE.DESC, 1f, false, Assets.GetAnim("spark_radial_high_energy_particles_kanim"), "travel_pre", Grid.SceneLayer.FXFront2, SimHashes.Creature, null, 293f);
		EntityTemplates.AddCollision(gameObject, EntityTemplates.CollisionShape.CIRCLE, 0.2f, 0.2f);
		gameObject.AddOrGet<LoopingSounds>();
		RadiationEmitter radiationEmitter = gameObject.AddOrGet<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emitRadiusX = 3;
		radiationEmitter.emitRadiusY = 3;
		radiationEmitter.emitRads = 0.4f * ((float)radiationEmitter.emitRadiusX / 6f);
		gameObject.AddComponent<HighEnergyParticle>().speed = 8f;
		return gameObject;
	}

	// Token: 0x06004639 RID: 17977 RVA: 0x00195BA3 File Offset: 0x00193DA3
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600463A RID: 17978 RVA: 0x00195BA5 File Offset: 0x00193DA5
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04002F3D RID: 12093
	public const int PARTICLE_SPEED = 8;

	// Token: 0x04002F3E RID: 12094
	public const float PARTICLE_COLLISION_SIZE = 0.2f;

	// Token: 0x04002F3F RID: 12095
	public const float PER_CELL_FALLOFF = 0.1f;

	// Token: 0x04002F40 RID: 12096
	public const float FALLOUT_RATIO = 0.5f;

	// Token: 0x04002F41 RID: 12097
	public const int MAX_PAYLOAD = 500;

	// Token: 0x04002F42 RID: 12098
	public const int EXPLOSION_FALLOUT_TEMPERATURE = 5000;

	// Token: 0x04002F43 RID: 12099
	public const float EXPLOSION_FALLOUT_MASS_PER_PARTICLE = 0.001f;

	// Token: 0x04002F44 RID: 12100
	public const float EXPLOSION_EMIT_DURRATION = 1f;

	// Token: 0x04002F45 RID: 12101
	public const short EXPLOSION_EMIT_RADIUS = 6;

	// Token: 0x04002F46 RID: 12102
	public const string ID = "HighEnergyParticle";
}
