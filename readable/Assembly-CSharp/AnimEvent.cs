using System;
using UnityEngine;

// Token: 0x02000541 RID: 1345
[Serializable]
public class AnimEvent
{
	// Token: 0x170000CD RID: 205
	// (get) Token: 0x06001D15 RID: 7445 RVA: 0x0009E55F File Offset: 0x0009C75F
	// (set) Token: 0x06001D16 RID: 7446 RVA: 0x0009E567 File Offset: 0x0009C767
	[SerializeField]
	public string name { get; private set; }

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x06001D17 RID: 7447 RVA: 0x0009E570 File Offset: 0x0009C770
	// (set) Token: 0x06001D18 RID: 7448 RVA: 0x0009E578 File Offset: 0x0009C778
	[SerializeField]
	public string file { get; private set; }

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x06001D19 RID: 7449 RVA: 0x0009E581 File Offset: 0x0009C781
	// (set) Token: 0x06001D1A RID: 7450 RVA: 0x0009E589 File Offset: 0x0009C789
	[SerializeField]
	public int frame { get; private set; }

	// Token: 0x06001D1B RID: 7451 RVA: 0x0009E592 File Offset: 0x0009C792
	public AnimEvent()
	{
	}

	// Token: 0x06001D1C RID: 7452 RVA: 0x0009E59C File Offset: 0x0009C79C
	public AnimEvent(string file, string name, int frame)
	{
		this.file = ((file == "") ? null : file);
		if (this.file != null)
		{
			this.fileHash = new KAnimHashedString(this.file);
		}
		this.name = name;
		this.frame = frame;
	}

	// Token: 0x06001D1D RID: 7453 RVA: 0x0009E5F0 File Offset: 0x0009C7F0
	public void Play(AnimEventManager.EventPlayerData behaviour)
	{
		if (behaviour.previousFrame < behaviour.currentFrame)
		{
			if (behaviour.previousFrame < this.frame && behaviour.currentFrame >= this.frame)
			{
				this.OnPlay(behaviour);
				return;
			}
		}
		else if (behaviour.previousFrame > behaviour.currentFrame && (behaviour.previousFrame < this.frame || this.frame <= behaviour.currentFrame))
		{
			this.OnPlay(behaviour);
		}
	}

	// Token: 0x06001D1E RID: 7454 RVA: 0x0009E668 File Offset: 0x0009C868
	private void DebugAnimEvent(string ev_name, AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x06001D1F RID: 7455 RVA: 0x0009E66A File Offset: 0x0009C86A
	public virtual void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x06001D20 RID: 7456 RVA: 0x0009E66C File Offset: 0x0009C86C
	public virtual void OnUpdate(AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x06001D21 RID: 7457 RVA: 0x0009E66E File Offset: 0x0009C86E
	public virtual void Stop(AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x04001115 RID: 4373
	[SerializeField]
	private KAnimHashedString fileHash;

	// Token: 0x04001117 RID: 4375
	public bool OnExit;
}
