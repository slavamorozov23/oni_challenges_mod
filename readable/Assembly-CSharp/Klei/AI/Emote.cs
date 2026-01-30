using System;
using System.Collections.Generic;

namespace Klei.AI
{
	// Token: 0x02001054 RID: 4180
	public class Emote : Resource
	{
		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06008171 RID: 33137 RVA: 0x0033F01D File Offset: 0x0033D21D
		public int StepCount
		{
			get
			{
				if (this.emoteSteps != null)
				{
					return this.emoteSteps.Count;
				}
				return 0;
			}
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06008172 RID: 33138 RVA: 0x0033F034 File Offset: 0x0033D234
		public KAnimFile AnimSet
		{
			get
			{
				if (this.animSetName != HashedString.Invalid && this.animSet == null)
				{
					this.animSet = Assets.GetAnim(this.animSetName);
				}
				return this.animSet;
			}
		}

		// Token: 0x06008173 RID: 33139 RVA: 0x0033F06D File Offset: 0x0033D26D
		public Emote(ResourceSet parent, string emoteId, EmoteStep[] defaultSteps, string animSetName = null) : base(emoteId, parent, null)
		{
			this.emoteSteps.AddRange(defaultSteps);
			this.animSetName = animSetName;
		}

		// Token: 0x06008174 RID: 33140 RVA: 0x0033F0A8 File Offset: 0x0033D2A8
		public bool IsValidForController(KBatchedAnimController animController)
		{
			bool flag = true;
			int num = 0;
			while (flag && num < this.StepCount)
			{
				flag = animController.HasAnimation(this.emoteSteps[num].anim);
				num++;
			}
			KAnimFileData kanimFileData = (this.animSet == null) ? null : this.animSet.GetData();
			int num2 = 0;
			while (kanimFileData != null && flag && num2 < this.StepCount)
			{
				bool flag2 = false;
				int num3 = 0;
				while (!flag2 && num3 < kanimFileData.animCount)
				{
					flag2 = (kanimFileData.GetAnim(num2).id == this.emoteSteps[num2].anim);
					num3++;
				}
				flag = flag2;
				num2++;
			}
			return flag;
		}

		// Token: 0x06008175 RID: 33141 RVA: 0x0033F160 File Offset: 0x0033D360
		public void ApplyAnimOverrides(KBatchedAnimController animController, KAnimFile overrideSet)
		{
			KAnimFile kanimFile = (overrideSet != null) ? overrideSet : this.AnimSet;
			if (kanimFile == null || animController == null)
			{
				return;
			}
			animController.AddAnimOverrides(kanimFile, 0f);
		}

		// Token: 0x06008176 RID: 33142 RVA: 0x0033F1A0 File Offset: 0x0033D3A0
		public void RemoveAnimOverrides(KBatchedAnimController animController, KAnimFile overrideSet)
		{
			KAnimFile kanimFile = (overrideSet != null) ? overrideSet : this.AnimSet;
			if (kanimFile == null || animController == null)
			{
				return;
			}
			animController.RemoveAnimOverrides(kanimFile);
		}

		// Token: 0x06008177 RID: 33143 RVA: 0x0033F1DC File Offset: 0x0033D3DC
		public void CollectStepAnims(out HashedString[] emoteAnims, int iterations)
		{
			emoteAnims = new HashedString[this.emoteSteps.Count * iterations];
			for (int i = 0; i < emoteAnims.Length; i++)
			{
				emoteAnims[i] = this.emoteSteps[i % this.emoteSteps.Count].anim;
			}
		}

		// Token: 0x06008178 RID: 33144 RVA: 0x0033F231 File Offset: 0x0033D431
		public bool IsValidStep(int stepIdx)
		{
			return stepIdx >= 0 && stepIdx < this.emoteSteps.Count;
		}

		// Token: 0x1700092A RID: 2346
		public EmoteStep this[int stepIdx]
		{
			get
			{
				if (!this.IsValidStep(stepIdx))
				{
					return null;
				}
				return this.emoteSteps[stepIdx];
			}
		}

		// Token: 0x0600817A RID: 33146 RVA: 0x0033F260 File Offset: 0x0033D460
		public int GetStepIndex(HashedString animName)
		{
			int i = 0;
			bool condition = false;
			while (i < this.emoteSteps.Count)
			{
				if (this.emoteSteps[i].anim == animName)
				{
					condition = true;
					break;
				}
				i++;
			}
			Debug.Assert(condition, string.Format("Could not find emote step {0} for emote {1}!", animName, this.Id));
			return i;
		}

		// Token: 0x040061FB RID: 25083
		private HashedString animSetName = null;

		// Token: 0x040061FC RID: 25084
		private KAnimFile animSet;

		// Token: 0x040061FD RID: 25085
		private List<EmoteStep> emoteSteps = new List<EmoteStep>();
	}
}
