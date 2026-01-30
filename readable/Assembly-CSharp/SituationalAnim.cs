using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B4E RID: 2894
[AddComponentMenu("KMonoBehaviour/scripts/SituationalAnim")]
public class SituationalAnim : KMonoBehaviour
{
	// Token: 0x0600557A RID: 21882 RVA: 0x001F3158 File Offset: 0x001F1358
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SituationalAnim.Situation situation = this.GetSituation();
		DebugUtil.LogArgs(new object[]
		{
			"Situation is",
			situation
		});
		this.SetAnimForSituation(situation);
	}

	// Token: 0x0600557B RID: 21883 RVA: 0x001F3198 File Offset: 0x001F1398
	private void SetAnimForSituation(SituationalAnim.Situation situation)
	{
		foreach (global::Tuple<SituationalAnim.Situation, string> tuple in this.anims)
		{
			if ((tuple.first & situation) == tuple.first)
			{
				DebugUtil.LogArgs(new object[]
				{
					"Chose Anim",
					tuple.first,
					tuple.second
				});
				this.SetAnim(tuple.second);
				break;
			}
		}
	}

	// Token: 0x0600557C RID: 21884 RVA: 0x001F322C File Offset: 0x001F142C
	private void SetAnim(string animName)
	{
		base.GetComponent<KBatchedAnimController>().Play(animName, KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0600557D RID: 21885 RVA: 0x001F324C File Offset: 0x001F144C
	private SituationalAnim.Situation GetSituation()
	{
		SituationalAnim.Situation situation = (SituationalAnim.Situation)0;
		Extents extents = base.GetComponent<Building>().GetExtents();
		int x = extents.x;
		int num = extents.x + extents.width - 1;
		int y = extents.y;
		int num2 = extents.y + extents.height - 1;
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(x, num, y - 1, y - 1), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Bottom;
		}
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(x - 1, x - 1, y, num2), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Left;
		}
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(x, num, num2 + 1, num2 + 1), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Top;
		}
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(num + 1, num + 1, y, num2), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Right;
		}
		return situation;
	}

	// Token: 0x0600557E RID: 21886 RVA: 0x001F3320 File Offset: 0x001F1520
	private bool DoesSatisfy(SituationalAnim.MustSatisfy result, SituationalAnim.MustSatisfy requirement)
	{
		if (requirement == SituationalAnim.MustSatisfy.All)
		{
			return result == SituationalAnim.MustSatisfy.All;
		}
		if (requirement == SituationalAnim.MustSatisfy.Any)
		{
			return result > SituationalAnim.MustSatisfy.None;
		}
		return result == SituationalAnim.MustSatisfy.None;
	}

	// Token: 0x0600557F RID: 21887 RVA: 0x001F3338 File Offset: 0x001F1538
	private SituationalAnim.MustSatisfy GetSatisfactionForEdge(int minx, int maxx, int miny, int maxy)
	{
		bool flag = false;
		bool flag2 = true;
		for (int i = minx; i <= maxx; i++)
		{
			for (int j = miny; j <= maxy; j++)
			{
				int arg = Grid.XYToCell(i, j);
				if (this.test(arg))
				{
					flag = true;
				}
				else
				{
					flag2 = false;
				}
			}
		}
		if (flag2)
		{
			return SituationalAnim.MustSatisfy.All;
		}
		if (flag)
		{
			return SituationalAnim.MustSatisfy.Any;
		}
		return SituationalAnim.MustSatisfy.None;
	}

	// Token: 0x040039B1 RID: 14769
	public List<global::Tuple<SituationalAnim.Situation, string>> anims;

	// Token: 0x040039B2 RID: 14770
	public Func<int, bool> test;

	// Token: 0x040039B3 RID: 14771
	public SituationalAnim.MustSatisfy mustSatisfy;

	// Token: 0x02001CB5 RID: 7349
	[Flags]
	public enum Situation
	{
		// Token: 0x040088FB RID: 35067
		Left = 1,
		// Token: 0x040088FC RID: 35068
		Right = 2,
		// Token: 0x040088FD RID: 35069
		Top = 4,
		// Token: 0x040088FE RID: 35070
		Bottom = 8
	}

	// Token: 0x02001CB6 RID: 7350
	public enum MustSatisfy
	{
		// Token: 0x04008900 RID: 35072
		None,
		// Token: 0x04008901 RID: 35073
		Any,
		// Token: 0x04008902 RID: 35074
		All
	}
}
