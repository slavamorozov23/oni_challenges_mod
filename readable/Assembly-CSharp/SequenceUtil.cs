using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000473 RID: 1139
public static class SequenceUtil
{
	// Token: 0x17000069 RID: 105
	// (get) Token: 0x060017D5 RID: 6101 RVA: 0x00086F78 File Offset: 0x00085178
	public static YieldInstruction WaitForNextFrame
	{
		get
		{
			return null;
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x060017D6 RID: 6102 RVA: 0x00086F7B File Offset: 0x0008517B
	public static YieldInstruction WaitForEndOfFrame
	{
		get
		{
			if (SequenceUtil.waitForEndOfFrame == null)
			{
				SequenceUtil.waitForEndOfFrame = new WaitForEndOfFrame();
			}
			return SequenceUtil.waitForEndOfFrame;
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x060017D7 RID: 6103 RVA: 0x00086F93 File Offset: 0x00085193
	public static YieldInstruction WaitForFixedUpdate
	{
		get
		{
			if (SequenceUtil.waitForFixedUpdate == null)
			{
				SequenceUtil.waitForFixedUpdate = new WaitForFixedUpdate();
			}
			return SequenceUtil.waitForFixedUpdate;
		}
	}

	// Token: 0x060017D8 RID: 6104 RVA: 0x00086FAC File Offset: 0x000851AC
	public static YieldInstruction WaitForSeconds(float duration)
	{
		WaitForSeconds result;
		if (!SequenceUtil.scaledTimeCache.TryGetValue(duration, out result))
		{
			result = (SequenceUtil.scaledTimeCache[duration] = new WaitForSeconds(duration));
		}
		return result;
	}

	// Token: 0x060017D9 RID: 6105 RVA: 0x00086FE0 File Offset: 0x000851E0
	public static WaitForSecondsRealtime WaitForSecondsRealtime(float duration)
	{
		WaitForSecondsRealtime result;
		if (!SequenceUtil.reailTimeWaitCache.TryGetValue(duration, out result))
		{
			result = (SequenceUtil.reailTimeWaitCache[duration] = new WaitForSecondsRealtime(duration));
		}
		return result;
	}

	// Token: 0x04000DF8 RID: 3576
	private static WaitForEndOfFrame waitForEndOfFrame = null;

	// Token: 0x04000DF9 RID: 3577
	private static WaitForFixedUpdate waitForFixedUpdate = null;

	// Token: 0x04000DFA RID: 3578
	private static Dictionary<float, WaitForSeconds> scaledTimeCache = new Dictionary<float, WaitForSeconds>();

	// Token: 0x04000DFB RID: 3579
	private static Dictionary<float, WaitForSecondsRealtime> reailTimeWaitCache = new Dictionary<float, WaitForSecondsRealtime>();
}
