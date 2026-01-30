using System;
using UnityEngine;

// Token: 0x02000B8C RID: 2956
public class LargeImpactorVanillaConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06005837 RID: 22583 RVA: 0x00200A07 File Offset: 0x001FEC07
	public string[] GetRequiredDlcIds()
	{
		return new string[]
		{
			"",
			"DLC4_ID"
		};
	}

	// Token: 0x06005838 RID: 22584 RVA: 0x00200A1F File Offset: 0x001FEC1F
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06005839 RID: 22585 RVA: 0x00200A22 File Offset: 0x001FEC22
	GameObject IEntityConfig.CreatePrefab()
	{
		return LargeImpactorVanillaConfig.ConfigCommon(LargeImpactorVanillaConfig.ID, LargeImpactorVanillaConfig.NAME);
	}

	// Token: 0x0600583A RID: 22586 RVA: 0x00200A34 File Offset: 0x001FEC34
	public static GameObject ConfigCommon(string id, string name)
	{
		GameObject gameObject = EntityTemplates.CreateEntity(id, name, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<StateMachineController>();
		gameObject.AddOrGet<Notifier>();
		gameObject.AddOrGet<LoopingSounds>();
		LargeImpactorStatus.Def def = gameObject.AddOrGetDef<LargeImpactorStatus.Def>();
		def.MAX_HEALTH = 1000;
		def.EventID = "LargeImpactor";
		gameObject.AddOrGet<LargeImpactorVisualizer>();
		gameObject.AddOrGet<LargeImpactorCrashStamp>().largeStampTemplate = "dlc4::poi/asteroid_impacts/potato_large";
		gameObject.AddOrGetDef<LargeImpactorNotificationMonitor.Def>();
		gameObject.AddOrGet<ParallaxBackgroundObject>().Initialize("Demolior_final_whole");
		return gameObject;
	}

	// Token: 0x0600583B RID: 22587 RVA: 0x00200AAE File Offset: 0x001FECAE
	void IEntityConfig.OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600583C RID: 22588 RVA: 0x00200AB0 File Offset: 0x001FECB0
	private static LargeImpactorStatus.Instance GetStatusMonitor()
	{
		return ((LargeImpactorEvent.StatesInstance)GameplayEventManager.Instance.GetGameplayEventInstance(Db.Get().GameplayEvents.LargeImpactor.Id, -1).smi).impactorInstance.GetSMI<LargeImpactorStatus.Instance>();
	}

	// Token: 0x0600583D RID: 22589 RVA: 0x00200AEC File Offset: 0x001FECEC
	public static void SpawnCommon(GameObject inst)
	{
		ParallaxBackgroundObject component = inst.GetComponent<ParallaxBackgroundObject>();
		component.motion = new LargeImpactorVanillaConfig.BackgroundMotion();
		LargeImpactorStatus.Instance statusMonitor = LargeImpactorVanillaConfig.GetStatusMonitor();
		if (statusMonitor != null)
		{
			LargeImpactorStatus.Instance instance = statusMonitor;
			instance.OnDamaged = (Action<int>)Delegate.Combine(instance.OnDamaged, new Action<int>(component.TriggerShaderDamagedEffect));
		}
	}

	// Token: 0x0600583E RID: 22590 RVA: 0x00200B36 File Offset: 0x001FED36
	void IEntityConfig.OnSpawn(GameObject inst)
	{
		LargeImpactorVanillaConfig.SpawnCommon(inst);
	}

	// Token: 0x04003B47 RID: 15175
	public static string ID = "LargeImpactorVanilla";

	// Token: 0x04003B48 RID: 15176
	public static string NAME = "LargestPotaytoeVanilla";

	// Token: 0x02001D13 RID: 7443
	public class BackgroundMotion : ParallaxBackgroundObject.IMotion
	{
		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x0600AFEE RID: 45038 RVA: 0x003D7F79 File Offset: 0x003D6179
		private LargeImpactorStatus.Instance StatusMonitor
		{
			get
			{
				if (this.statusMonitor == null)
				{
					this.statusMonitor = LargeImpactorVanillaConfig.GetStatusMonitor();
				}
				return this.statusMonitor;
			}
		}

		// Token: 0x0600AFEF RID: 45039 RVA: 0x003D7F94 File Offset: 0x003D6194
		public float GetETA()
		{
			if (!this.StatusMonitor.IsRunning())
			{
				return this.GetDuration();
			}
			return this.StatusMonitor.TimeRemainingBeforeCollision;
		}

		// Token: 0x0600AFF0 RID: 45040 RVA: 0x003D7FB5 File Offset: 0x003D61B5
		public float GetDuration()
		{
			return LargeImpactorEvent.GetImpactTime();
		}

		// Token: 0x0600AFF1 RID: 45041 RVA: 0x003D7FBC File Offset: 0x003D61BC
		public void OnNormalizedDistanceChanged(float normalizedDistance)
		{
			AmbienceManager.Quadrant[] quadrants = Game.Instance.GetComponent<AmbienceManager>().quadrants;
			for (int i = 0; i < quadrants.Length; i++)
			{
				quadrants[i].spaceLayer.SetCustomParameter("distanceToMeteor", normalizedDistance);
			}
		}

		// Token: 0x04008A40 RID: 35392
		private LargeImpactorStatus.Instance statusMonitor;
	}
}
