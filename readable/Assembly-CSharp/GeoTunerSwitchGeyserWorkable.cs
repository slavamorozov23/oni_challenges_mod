using System;

// Token: 0x02000769 RID: 1897
public class GeoTunerSwitchGeyserWorkable : Workable
{
	// Token: 0x06003022 RID: 12322 RVA: 0x00115F4C File Offset: 0x0011414C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_remote_kanim")
		};
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
	}

	// Token: 0x06003023 RID: 12323 RVA: 0x00115F80 File Offset: 0x00114180
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(3f);
	}

	// Token: 0x04001CAB RID: 7339
	private const string animName = "anim_use_remote_kanim";
}
