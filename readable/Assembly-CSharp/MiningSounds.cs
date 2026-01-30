using System;
using FMODUnity;
using UnityEngine;

// Token: 0x02000606 RID: 1542
[AddComponentMenu("KMonoBehaviour/scripts/MiningSounds")]
public class MiningSounds : KMonoBehaviour
{
	// Token: 0x060023DE RID: 9182 RVA: 0x000CF727 File Offset: 0x000CD927
	protected override void OnPrefabInit()
	{
		base.Subscribe<MiningSounds>(-1762453998, MiningSounds.OnStartMiningSoundDelegate);
		base.Subscribe<MiningSounds>(939543986, MiningSounds.OnStopMiningSoundDelegate);
	}

	// Token: 0x060023DF RID: 9183 RVA: 0x000CF74C File Offset: 0x000CD94C
	private void OnStartMiningSound(object data)
	{
		if (this.miningSound == null)
		{
			Element element = data as Element;
			if (element != null)
			{
				string text = element.substance.GetMiningSound();
				if (text == null || text == "")
				{
					return;
				}
				text = "Mine_" + text;
				string sound = GlobalAssets.GetSound(text, false);
				this.miningSoundEvent = RuntimeManager.PathToEventReference(sound);
				if (!this.miningSoundEvent.IsNull)
				{
					this.loopingSounds.StartSound(this.miningSoundEvent);
				}
			}
		}
	}

	// Token: 0x060023E0 RID: 9184 RVA: 0x000CF7CD File Offset: 0x000CD9CD
	private void OnStopMiningSound(object data)
	{
		if (!this.miningSoundEvent.IsNull)
		{
			this.loopingSounds.StopSound(this.miningSoundEvent);
			this.miningSound = null;
		}
	}

	// Token: 0x060023E1 RID: 9185 RVA: 0x000CF7F4 File Offset: 0x000CD9F4
	public void SetPercentComplete(float progress)
	{
		if (!this.miningSoundEvent.IsNull)
		{
			this.loopingSounds.SetParameter(this.miningSoundEvent, MiningSounds.HASH_PERCENTCOMPLETE, progress);
		}
	}

	// Token: 0x040014E1 RID: 5345
	private static HashedString HASH_PERCENTCOMPLETE = "percentComplete";

	// Token: 0x040014E2 RID: 5346
	[MyCmpGet]
	private LoopingSounds loopingSounds;

	// Token: 0x040014E3 RID: 5347
	private FMODAsset miningSound;

	// Token: 0x040014E4 RID: 5348
	private EventReference miningSoundEvent;

	// Token: 0x040014E5 RID: 5349
	private static readonly EventSystem.IntraObjectHandler<MiningSounds> OnStartMiningSoundDelegate = new EventSystem.IntraObjectHandler<MiningSounds>(delegate(MiningSounds component, object data)
	{
		component.OnStartMiningSound(data);
	});

	// Token: 0x040014E6 RID: 5350
	private static readonly EventSystem.IntraObjectHandler<MiningSounds> OnStopMiningSoundDelegate = new EventSystem.IntraObjectHandler<MiningSounds>(delegate(MiningSounds component, object data)
	{
		component.OnStopMiningSound(data);
	});
}
