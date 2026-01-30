using System;
using UnityEngine;

// Token: 0x02000BFD RID: 3069
[AddComponentMenu("KMonoBehaviour/scripts/UISounds")]
public class UISounds : KMonoBehaviour
{
	// Token: 0x170006AB RID: 1707
	// (get) Token: 0x06005C2E RID: 23598 RVA: 0x002159C5 File Offset: 0x00213BC5
	// (set) Token: 0x06005C2F RID: 23599 RVA: 0x002159CC File Offset: 0x00213BCC
	public static UISounds Instance { get; private set; }

	// Token: 0x06005C30 RID: 23600 RVA: 0x002159D4 File Offset: 0x00213BD4
	public static void DestroyInstance()
	{
		UISounds.Instance = null;
	}

	// Token: 0x06005C31 RID: 23601 RVA: 0x002159DC File Offset: 0x00213BDC
	protected override void OnPrefabInit()
	{
		UISounds.Instance = this;
	}

	// Token: 0x06005C32 RID: 23602 RVA: 0x002159E4 File Offset: 0x00213BE4
	public static void PlaySound(UISounds.Sound sound)
	{
		UISounds.Instance.PlaySoundInternal(sound);
	}

	// Token: 0x06005C33 RID: 23603 RVA: 0x002159F4 File Offset: 0x00213BF4
	private void PlaySoundInternal(UISounds.Sound sound)
	{
		for (int i = 0; i < this.soundData.Length; i++)
		{
			if (this.soundData[i].sound == sound)
			{
				if (this.logSounds)
				{
					DebugUtil.LogArgs(new object[]
					{
						"Play sound",
						this.soundData[i].name
					});
				}
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.soundData[i].name, false));
			}
		}
	}

	// Token: 0x04003D70 RID: 15728
	[SerializeField]
	private bool logSounds;

	// Token: 0x04003D71 RID: 15729
	[SerializeField]
	private UISounds.SoundData[] soundData;

	// Token: 0x02001D94 RID: 7572
	public enum Sound
	{
		// Token: 0x04008BA6 RID: 35750
		NegativeNotification,
		// Token: 0x04008BA7 RID: 35751
		PositiveNotification,
		// Token: 0x04008BA8 RID: 35752
		Select,
		// Token: 0x04008BA9 RID: 35753
		Negative,
		// Token: 0x04008BAA RID: 35754
		Back,
		// Token: 0x04008BAB RID: 35755
		ClickObject,
		// Token: 0x04008BAC RID: 35756
		HUD_Mouseover,
		// Token: 0x04008BAD RID: 35757
		Object_Mouseover,
		// Token: 0x04008BAE RID: 35758
		ClickHUD,
		// Token: 0x04008BAF RID: 35759
		Object_AutoSelected
	}

	// Token: 0x02001D95 RID: 7573
	[Serializable]
	private struct SoundData
	{
		// Token: 0x04008BB0 RID: 35760
		public string name;

		// Token: 0x04008BB1 RID: 35761
		public UISounds.Sound sound;
	}
}
