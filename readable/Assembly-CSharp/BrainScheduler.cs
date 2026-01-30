using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000488 RID: 1160
[AddComponentMenu("KMonoBehaviour/scripts/BrainScheduler")]
public class BrainScheduler : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x0600189B RID: 6299 RVA: 0x00088C58 File Offset: 0x00086E58
	public List<BrainScheduler.BrainGroup> debugGetBrainGroups()
	{
		return this.brainGroups;
	}

	// Token: 0x0600189C RID: 6300 RVA: 0x00088C60 File Offset: 0x00086E60
	protected override void OnPrefabInit()
	{
		this.brainGroups.Add(new BrainScheduler.DupeBrainGroup());
		this.brainGroups.Add(new BrainScheduler.CreatureBrainGroup());
		Components.Brains.Register(new Action<Brain>(this.OnAddBrain), new Action<Brain>(this.OnRemoveBrain));
	}

	// Token: 0x0600189D RID: 6301 RVA: 0x00088CB0 File Offset: 0x00086EB0
	private void OnAddBrain(Brain brain)
	{
		bool test = false;
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			if (brain.HasTag(brainGroup.tag))
			{
				brainGroup.AddBrain(brain);
				test = true;
			}
		}
		DebugUtil.Assert(test);
	}

	// Token: 0x0600189E RID: 6302 RVA: 0x00088D1C File Offset: 0x00086F1C
	private void OnRemoveBrain(Brain brain)
	{
		bool test = false;
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			if (brain.HasTag(brainGroup.tag))
			{
				test = true;
				brainGroup.RemoveBrain(brain);
			}
			Navigator component = brain.GetComponent<Navigator>();
			if (component != null)
			{
				component.executePathProbeTaskAsync = false;
			}
		}
		DebugUtil.Assert(test);
	}

	// Token: 0x0600189F RID: 6303 RVA: 0x00088DA0 File Offset: 0x00086FA0
	public void PrioritizeBrain(Brain brain)
	{
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			if (brain.HasTag(brainGroup.tag))
			{
				brainGroup.PrioritizeBrain(brain);
			}
		}
	}

	// Token: 0x060018A0 RID: 6304 RVA: 0x00088E04 File Offset: 0x00087004
	public void RenderEveryTick(float dt)
	{
		if (Game.IsQuitting() || KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			brainGroup.RenderEveryTick(dt);
		}
	}

	// Token: 0x060018A1 RID: 6305 RVA: 0x00088E64 File Offset: 0x00087064
	protected override void OnForcedCleanUp()
	{
		base.OnForcedCleanUp();
	}

	// Token: 0x04000E42 RID: 3650
	public const float millisecondsPerFrame = 33.33333f;

	// Token: 0x04000E43 RID: 3651
	public const float secondsPerFrame = 0.033333328f;

	// Token: 0x04000E44 RID: 3652
	public const float framesPerSecond = 30.000006f;

	// Token: 0x04000E45 RID: 3653
	private List<BrainScheduler.BrainGroup> brainGroups = new List<BrainScheduler.BrainGroup>();

	// Token: 0x020012A8 RID: 4776
	public abstract class BrainGroup
	{
		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x060088D4 RID: 35028 RVA: 0x0034F853 File Offset: 0x0034DA53
		// (set) Token: 0x060088D5 RID: 35029 RVA: 0x0034F85B File Offset: 0x0034DA5B
		public Tag tag { get; private set; }

		// Token: 0x060088D6 RID: 35030 RVA: 0x0034F864 File Offset: 0x0034DA64
		protected BrainGroup(Tag tag)
		{
			this.tag = tag;
			string str = tag.ToString();
			this.increaseLoadLabel = "IncLoad" + str;
			this.decreaseLoadLabel = "DecLoad" + str;
		}

		// Token: 0x060088D7 RID: 35031 RVA: 0x0034F8C4 File Offset: 0x0034DAC4
		public void AddBrain(Brain brain)
		{
			this.brains.Add(brain);
		}

		// Token: 0x060088D8 RID: 35032 RVA: 0x0034F8D4 File Offset: 0x0034DAD4
		public void RemoveBrain(Brain brain)
		{
			int num = this.brains.IndexOf(brain);
			if (num != -1)
			{
				this.brains.RemoveAt(num);
				this.OnRemoveBrain(num, ref this.nextUpdateBrain);
			}
			if (this.priorityBrains.Contains(brain))
			{
				List<Brain> list = new List<Brain>(this.priorityBrains);
				list.Remove(brain);
				this.priorityBrains = new Queue<Brain>(list);
			}
		}

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x060088D9 RID: 35033 RVA: 0x0034F939 File Offset: 0x0034DB39
		public int BrainCount
		{
			get
			{
				return this.brains.Count;
			}
		}

		// Token: 0x060088DA RID: 35034 RVA: 0x0034F946 File Offset: 0x0034DB46
		public void PrioritizeBrain(Brain brain)
		{
			if (!this.priorityBrains.Contains(brain))
			{
				this.priorityBrains.Enqueue(brain);
			}
		}

		// Token: 0x060088DB RID: 35035 RVA: 0x0034F962 File Offset: 0x0034DB62
		private void IncrementBrainIndex(ref int brainIndex)
		{
			brainIndex++;
			if (brainIndex == this.brains.Count)
			{
				brainIndex = 0;
			}
		}

		// Token: 0x060088DC RID: 35036 RVA: 0x0034F97C File Offset: 0x0034DB7C
		private void ClampBrainIndex(ref int brainIndex)
		{
			brainIndex = MathUtil.Clamp(0, this.brains.Count - 1, brainIndex);
		}

		// Token: 0x060088DD RID: 35037 RVA: 0x0034F995 File Offset: 0x0034DB95
		private void OnRemoveBrain(int removedIndex, ref int brainIndex)
		{
			if (removedIndex < brainIndex)
			{
				brainIndex--;
				return;
			}
			if (brainIndex == this.brains.Count)
			{
				brainIndex = 0;
			}
		}

		// Token: 0x060088DE RID: 35038 RVA: 0x0034F9B8 File Offset: 0x0034DBB8
		public void RenderEveryTick(float dt)
		{
			this.BeginBrainGroupUpdate();
			int num = this.InitialProbeCount();
			int num2 = 0;
			while (num2 != this.brains.Count && num != 0)
			{
				this.ClampBrainIndex(ref this.nextUpdateBrain);
				this.debugMaxPriorityBrainCountSeen = Mathf.Max(this.debugMaxPriorityBrainCountSeen, this.priorityBrains.Count);
				Brain brain;
				if (this.AllowPriorityBrains() && this.priorityBrains.Count > 0)
				{
					brain = this.priorityBrains.Dequeue();
				}
				else
				{
					brain = this.brains[this.nextUpdateBrain];
					this.IncrementBrainIndex(ref this.nextUpdateBrain);
				}
				if (brain.IsRunning())
				{
					brain.UpdateBrain();
					num--;
				}
				num2++;
			}
			this.EndBrainGroupUpdate();
		}

		// Token: 0x060088DF RID: 35039
		protected abstract int InitialProbeCount();

		// Token: 0x060088E0 RID: 35040
		protected abstract int InitialProbeSize();

		// Token: 0x060088E1 RID: 35041
		protected abstract int MinProbeSize();

		// Token: 0x060088E2 RID: 35042
		protected abstract int IdealProbeSize();

		// Token: 0x060088E3 RID: 35043
		protected abstract int ProbeSizeStep();

		// Token: 0x060088E4 RID: 35044
		public abstract float GetEstimatedFrameTime();

		// Token: 0x060088E5 RID: 35045
		public abstract float LoadBalanceThreshold();

		// Token: 0x060088E6 RID: 35046
		public abstract bool AllowPriorityBrains();

		// Token: 0x060088E7 RID: 35047 RVA: 0x0034FA76 File Offset: 0x0034DC76
		public virtual void BeginBrainGroupUpdate()
		{
		}

		// Token: 0x060088E8 RID: 35048 RVA: 0x0034FA78 File Offset: 0x0034DC78
		public virtual void EndBrainGroupUpdate()
		{
		}

		// Token: 0x04006876 RID: 26742
		protected List<Brain> brains = new List<Brain>();

		// Token: 0x04006877 RID: 26743
		protected Queue<Brain> priorityBrains = new Queue<Brain>();

		// Token: 0x04006878 RID: 26744
		private string increaseLoadLabel;

		// Token: 0x04006879 RID: 26745
		private string decreaseLoadLabel;

		// Token: 0x0400687A RID: 26746
		public bool debugFreezeLoadAdustment;

		// Token: 0x0400687B RID: 26747
		public int debugMaxPriorityBrainCountSeen;

		// Token: 0x0400687C RID: 26748
		private int nextUpdateBrain;
	}

	// Token: 0x020012A9 RID: 4777
	private class DupeBrainGroup : BrainScheduler.BrainGroup
	{
		// Token: 0x060088E9 RID: 35049 RVA: 0x0034FA7A File Offset: 0x0034DC7A
		public DupeBrainGroup() : base(GameTags.DupeBrain)
		{
		}

		// Token: 0x060088EA RID: 35050 RVA: 0x0034FA8E File Offset: 0x0034DC8E
		protected override int InitialProbeCount()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().initialProbeCount;
		}

		// Token: 0x060088EB RID: 35051 RVA: 0x0034FA9A File Offset: 0x0034DC9A
		protected override int InitialProbeSize()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().initialProbeSize;
		}

		// Token: 0x060088EC RID: 35052 RVA: 0x0034FAA6 File Offset: 0x0034DCA6
		protected override int MinProbeSize()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().minProbeSize;
		}

		// Token: 0x060088ED RID: 35053 RVA: 0x0034FAB2 File Offset: 0x0034DCB2
		protected override int IdealProbeSize()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().idealProbeSize;
		}

		// Token: 0x060088EE RID: 35054 RVA: 0x0034FABE File Offset: 0x0034DCBE
		protected override int ProbeSizeStep()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().probeSizeStep;
		}

		// Token: 0x060088EF RID: 35055 RVA: 0x0034FACA File Offset: 0x0034DCCA
		public override float GetEstimatedFrameTime()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().estimatedFrameTime;
		}

		// Token: 0x060088F0 RID: 35056 RVA: 0x0034FAD6 File Offset: 0x0034DCD6
		public override float LoadBalanceThreshold()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().loadBalanceThreshold;
		}

		// Token: 0x060088F1 RID: 35057 RVA: 0x0034FAE2 File Offset: 0x0034DCE2
		public override bool AllowPriorityBrains()
		{
			return this.usePriorityBrain;
		}

		// Token: 0x060088F2 RID: 35058 RVA: 0x0034FAEA File Offset: 0x0034DCEA
		public override void BeginBrainGroupUpdate()
		{
			base.BeginBrainGroupUpdate();
			this.usePriorityBrain = !this.usePriorityBrain;
		}

		// Token: 0x0400687D RID: 26749
		private bool usePriorityBrain = true;

		// Token: 0x0200278E RID: 10126
		public class Tuning : TuningData<BrainScheduler.DupeBrainGroup.Tuning>
		{
			// Token: 0x0400AF6F RID: 44911
			public int initialProbeCount = 1;

			// Token: 0x0400AF70 RID: 44912
			public int initialProbeSize = 1000;

			// Token: 0x0400AF71 RID: 44913
			public int minProbeSize = 100;

			// Token: 0x0400AF72 RID: 44914
			public int idealProbeSize = 1000;

			// Token: 0x0400AF73 RID: 44915
			public int probeSizeStep = 100;

			// Token: 0x0400AF74 RID: 44916
			public float estimatedFrameTime = 2f;

			// Token: 0x0400AF75 RID: 44917
			public float loadBalanceThreshold = 0.1f;
		}
	}

	// Token: 0x020012AA RID: 4778
	private class CreatureBrainGroup : BrainScheduler.BrainGroup
	{
		// Token: 0x060088F3 RID: 35059 RVA: 0x0034FB01 File Offset: 0x0034DD01
		public CreatureBrainGroup() : base(GameTags.CreatureBrain)
		{
		}

		// Token: 0x060088F4 RID: 35060 RVA: 0x0034FB0E File Offset: 0x0034DD0E
		protected override int InitialProbeCount()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().initialProbeCount;
		}

		// Token: 0x060088F5 RID: 35061 RVA: 0x0034FB1A File Offset: 0x0034DD1A
		protected override int InitialProbeSize()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().initialProbeSize;
		}

		// Token: 0x060088F6 RID: 35062 RVA: 0x0034FB26 File Offset: 0x0034DD26
		protected override int MinProbeSize()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().minProbeSize;
		}

		// Token: 0x060088F7 RID: 35063 RVA: 0x0034FB32 File Offset: 0x0034DD32
		protected override int IdealProbeSize()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().idealProbeSize;
		}

		// Token: 0x060088F8 RID: 35064 RVA: 0x0034FB3E File Offset: 0x0034DD3E
		protected override int ProbeSizeStep()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().probeSizeStep;
		}

		// Token: 0x060088F9 RID: 35065 RVA: 0x0034FB4A File Offset: 0x0034DD4A
		public override float GetEstimatedFrameTime()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().estimatedFrameTime;
		}

		// Token: 0x060088FA RID: 35066 RVA: 0x0034FB56 File Offset: 0x0034DD56
		public override float LoadBalanceThreshold()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().loadBalanceThreshold;
		}

		// Token: 0x060088FB RID: 35067 RVA: 0x0034FB62 File Offset: 0x0034DD62
		public override bool AllowPriorityBrains()
		{
			return true;
		}

		// Token: 0x0200278F RID: 10127
		public class Tuning : TuningData<BrainScheduler.CreatureBrainGroup.Tuning>
		{
			// Token: 0x0400AF76 RID: 44918
			public int initialProbeCount = 5;

			// Token: 0x0400AF77 RID: 44919
			public int initialProbeSize = 1000;

			// Token: 0x0400AF78 RID: 44920
			public int minProbeSize = 100;

			// Token: 0x0400AF79 RID: 44921
			public int idealProbeSize = 300;

			// Token: 0x0400AF7A RID: 44922
			public int probeSizeStep = 100;

			// Token: 0x0400AF7B RID: 44923
			public float estimatedFrameTime = 1f;

			// Token: 0x0400AF7C RID: 44924
			public float loadBalanceThreshold = 0.1f;
		}
	}
}
