using System;
using System.Runtime.CompilerServices;

// Token: 0x020000BC RID: 188
internal class OilFloaterMovementSound : KMonoBehaviour
{
	// Token: 0x0600036D RID: 877 RVA: 0x0001BC84 File Offset: 0x00019E84
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.sound = GlobalAssets.GetSound(this.sound, false);
		base.Subscribe<OilFloaterMovementSound>(1027377649, OilFloaterMovementSound.OnObjectMovementStateChangedDelegate);
		this.cellChangedHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, OilFloaterMovementSound.UpdateSoundDispatcher, this, "OilFloaterMovementSound");
	}

	// Token: 0x0600036E RID: 878 RVA: 0x0001BCDC File Offset: 0x00019EDC
	private void OnObjectMovementStateChanged(object data)
	{
		GameHashes gameHashes = Boxed<GameHashes>.Unbox(data);
		this.isMoving = (gameHashes == GameHashes.ObjectMovementWakeUp);
		this.UpdateSound();
	}

	// Token: 0x0600036F RID: 879 RVA: 0x0001BD04 File Offset: 0x00019F04
	private void UpdateSound()
	{
		bool flag = this.isMoving && base.GetComponent<Navigator>().CurrentNavType != NavType.Swim;
		if (flag == this.isPlayingSound)
		{
			return;
		}
		LoopingSounds component = base.GetComponent<LoopingSounds>();
		if (flag)
		{
			component.StartSound(this.sound);
		}
		else
		{
			component.StopSound(this.sound);
		}
		this.isPlayingSound = flag;
	}

	// Token: 0x06000370 RID: 880 RVA: 0x0001BD64 File Offset: 0x00019F64
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandlerID);
	}

	// Token: 0x04000276 RID: 630
	public string sound;

	// Token: 0x04000277 RID: 631
	public bool isPlayingSound;

	// Token: 0x04000278 RID: 632
	public bool isMoving;

	// Token: 0x04000279 RID: 633
	private ulong cellChangedHandlerID;

	// Token: 0x0400027A RID: 634
	private static readonly EventSystem.IntraObjectHandler<OilFloaterMovementSound> OnObjectMovementStateChangedDelegate = new EventSystem.IntraObjectHandler<OilFloaterMovementSound>(delegate(OilFloaterMovementSound component, object data)
	{
		component.OnObjectMovementStateChanged(data);
	});

	// Token: 0x0400027B RID: 635
	private static readonly Action<object> UpdateSoundDispatcher = delegate(object obj)
	{
		Unsafe.As<OilFloaterMovementSound>(obj).UpdateSound();
	};
}
