using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200054E RID: 1358
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/KAnimSequencer")]
public class KAnimSequencer : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06001DD8 RID: 7640 RVA: 0x000A1BED File Offset: 0x0009FDED
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.kbac = base.GetComponent<KBatchedAnimController>();
		this.mb = base.GetComponent<MinionBrain>();
		if (this.autoRun)
		{
			this.PlaySequence();
		}
	}

	// Token: 0x06001DD9 RID: 7641 RVA: 0x000A1C1B File Offset: 0x0009FE1B
	public void Reset()
	{
		this.currentIndex = 0;
	}

	// Token: 0x06001DDA RID: 7642 RVA: 0x000A1C24 File Offset: 0x0009FE24
	public void PlaySequence()
	{
		if (this.sequence != null && this.sequence.Length != 0)
		{
			if (this.mb != null)
			{
				this.mb.Suspend("AnimSequencer");
			}
			this.kbac.onAnimComplete += this.PlayNext;
			this.PlayNext(null);
		}
	}

	// Token: 0x06001DDB RID: 7643 RVA: 0x000A1C84 File Offset: 0x0009FE84
	private void PlayNext(HashedString name)
	{
		if (this.sequence.Length > this.currentIndex)
		{
			this.kbac.Play(new HashedString(this.sequence[this.currentIndex].anim), this.sequence[this.currentIndex].mode, this.sequence[this.currentIndex].speed, 0f);
			this.currentIndex++;
			return;
		}
		this.kbac.onAnimComplete -= this.PlayNext;
		if (this.mb != null)
		{
			this.mb.Resume("AnimSequencer");
		}
	}

	// Token: 0x04001175 RID: 4469
	[Serialize]
	public bool autoRun;

	// Token: 0x04001176 RID: 4470
	[Serialize]
	public KAnimSequencer.KAnimSequence[] sequence = new KAnimSequencer.KAnimSequence[0];

	// Token: 0x04001177 RID: 4471
	private int currentIndex;

	// Token: 0x04001178 RID: 4472
	private KBatchedAnimController kbac;

	// Token: 0x04001179 RID: 4473
	private MinionBrain mb;

	// Token: 0x020013EC RID: 5100
	[SerializationConfig(MemberSerialization.OptOut)]
	[Serializable]
	public class KAnimSequence
	{
		// Token: 0x04006CB2 RID: 27826
		public string anim;

		// Token: 0x04006CB3 RID: 27827
		public float speed = 1f;

		// Token: 0x04006CB4 RID: 27828
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;
	}
}
