using System;
using UnityEngine;

// Token: 0x02000908 RID: 2312
public class EffectPrefabs : MonoBehaviour
{
	// Token: 0x1700046E RID: 1134
	// (get) Token: 0x0600404D RID: 16461 RVA: 0x0016CBA5 File Offset: 0x0016ADA5
	// (set) Token: 0x0600404E RID: 16462 RVA: 0x0016CBAC File Offset: 0x0016ADAC
	public static EffectPrefabs Instance { get; private set; }

	// Token: 0x0600404F RID: 16463 RVA: 0x0016CBB4 File Offset: 0x0016ADB4
	private void Awake()
	{
		EffectPrefabs.Instance = this;
	}

	// Token: 0x040027E5 RID: 10213
	public GameObject DreamBubble;

	// Token: 0x040027E6 RID: 10214
	public GameObject ThoughtBubble;

	// Token: 0x040027E7 RID: 10215
	public GameObject ThoughtBubbleConvo;

	// Token: 0x040027E8 RID: 10216
	public GameObject MeteorBackground;

	// Token: 0x040027E9 RID: 10217
	public GameObject SparkleStreakFX;

	// Token: 0x040027EA RID: 10218
	public GameObject HappySingerFX;

	// Token: 0x040027EB RID: 10219
	public GameObject HugFrenzyFX;

	// Token: 0x040027EC RID: 10220
	public GameObject GameplayEventDisplay;

	// Token: 0x040027ED RID: 10221
	public GameObject OpenTemporalTearBeam;

	// Token: 0x040027EE RID: 10222
	public GameObject MissileSmokeTrailFX;

	// Token: 0x040027EF RID: 10223
	public GameObject LongRangeMissileSmokeTrailFX;

	// Token: 0x040027F0 RID: 10224
	public GameObject PlantPollinated;
}
