using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D0F RID: 3343
public class FrontEndBackground : UIDupeRandomizer
{
	// Token: 0x0600676E RID: 26478 RVA: 0x0026FFE4 File Offset: 0x0026E1E4
	protected override void Start()
	{
		this.tuning = TuningData<FrontEndBackground.Tuning>.Get();
		base.Start();
		for (int i = 0; i < this.anims.Length; i++)
		{
			int minionIndex = i;
			KBatchedAnimController kbatchedAnimController = this.anims[i].minions[0];
			if (kbatchedAnimController.gameObject.activeInHierarchy)
			{
				kbatchedAnimController.onAnimComplete += delegate(HashedString name)
				{
					this.WaitForABit(minionIndex, name);
				};
				this.WaitForABit(i, HashedString.Invalid);
			}
		}
		this.dreckoController = base.transform.GetChild(0).Find("startmenu_drecko").GetComponent<KBatchedAnimController>();
		if (this.dreckoController.gameObject.activeInHierarchy)
		{
			this.dreckoController.enabled = false;
			this.nextDreckoTime = UnityEngine.Random.Range(this.tuning.minFirstDreckoInterval, this.tuning.maxFirstDreckoInterval) + Time.unscaledTime;
		}
	}

	// Token: 0x0600676F RID: 26479 RVA: 0x002700D2 File Offset: 0x0026E2D2
	protected override void Update()
	{
		base.Update();
		this.UpdateDrecko();
	}

	// Token: 0x06006770 RID: 26480 RVA: 0x002700E0 File Offset: 0x0026E2E0
	private void UpdateDrecko()
	{
		if (this.dreckoController.gameObject.activeInHierarchy && Time.unscaledTime > this.nextDreckoTime)
		{
			this.dreckoController.enabled = true;
			this.dreckoController.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
			this.nextDreckoTime = UnityEngine.Random.Range(this.tuning.minDreckoInterval, this.tuning.maxDreckoInterval) + Time.unscaledTime;
		}
	}

	// Token: 0x06006771 RID: 26481 RVA: 0x0027015F File Offset: 0x0026E35F
	private void WaitForABit(int minion_idx, HashedString name)
	{
		base.StartCoroutine(this.WaitForTime(minion_idx));
	}

	// Token: 0x06006772 RID: 26482 RVA: 0x0027016F File Offset: 0x0026E36F
	private IEnumerator WaitForTime(int minion_idx)
	{
		this.anims[minion_idx].lastWaitTime = UnityEngine.Random.Range(this.anims[minion_idx].minSecondsBetweenAction, this.anims[minion_idx].maxSecondsBetweenAction);
		yield return new WaitForSecondsRealtime(this.anims[minion_idx].lastWaitTime);
		base.GetNewBody(minion_idx);
		using (List<KBatchedAnimController>.Enumerator enumerator = this.anims[minion_idx].minions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KBatchedAnimController kbatchedAnimController = enumerator.Current;
				kbatchedAnimController.ClearQueue();
				kbatchedAnimController.Play(this.anims[minion_idx].anim_name, KAnim.PlayMode.Once, 1f, 0f);
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x040046DF RID: 18143
	private KBatchedAnimController dreckoController;

	// Token: 0x040046E0 RID: 18144
	private float nextDreckoTime;

	// Token: 0x040046E1 RID: 18145
	private FrontEndBackground.Tuning tuning;

	// Token: 0x02001F43 RID: 8003
	public class Tuning : TuningData<FrontEndBackground.Tuning>
	{
		// Token: 0x0400920C RID: 37388
		public float minDreckoInterval;

		// Token: 0x0400920D RID: 37389
		public float maxDreckoInterval;

		// Token: 0x0400920E RID: 37390
		public float minFirstDreckoInterval;

		// Token: 0x0400920F RID: 37391
		public float maxFirstDreckoInterval;
	}
}
