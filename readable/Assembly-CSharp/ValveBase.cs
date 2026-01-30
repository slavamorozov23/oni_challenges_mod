using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000825 RID: 2085
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ValveBase")]
public class ValveBase : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x060038CE RID: 14542 RVA: 0x0013DCC3 File Offset: 0x0013BEC3
	// (set) Token: 0x060038CD RID: 14541 RVA: 0x0013DCBA File Offset: 0x0013BEBA
	public float CurrentFlow
	{
		get
		{
			return this.currentFlow;
		}
		set
		{
			this.currentFlow = value;
		}
	}

	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x060038CF RID: 14543 RVA: 0x0013DCCB File Offset: 0x0013BECB
	public HandleVector<int>.Handle AccumulatorHandle
	{
		get
		{
			return this.flowAccumulator;
		}
	}

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x060038D0 RID: 14544 RVA: 0x0013DCD3 File Offset: 0x0013BED3
	public float MaxFlow
	{
		get
		{
			return this.maxFlow;
		}
	}

	// Token: 0x060038D1 RID: 14545 RVA: 0x0013DCDB File Offset: 0x0013BEDB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.flowAccumulator = Game.Instance.accumulators.Add("Flow", this);
	}

	// Token: 0x060038D2 RID: 14546 RVA: 0x0013DD00 File Offset: 0x0013BF00
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		Conduit.GetFlowManager(this.conduitType).AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.UpdateAnim();
		this.OnCmpEnable();
	}

	// Token: 0x060038D3 RID: 14547 RVA: 0x0013DD5B File Offset: 0x0013BF5B
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.flowAccumulator);
		Conduit.GetFlowManager(this.conduitType).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x060038D4 RID: 14548 RVA: 0x0013DD98 File Offset: 0x0013BF98
	private void ConduitUpdate(float dt)
	{
		ConduitFlow flowManager = Conduit.GetFlowManager(this.conduitType);
		ConduitFlow.Conduit conduit = flowManager.GetConduit(this.inputCell);
		if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell))
		{
			this.OnMassTransfer(0f);
			this.UpdateAnim();
			return;
		}
		ConduitFlow.ConduitContents contents = conduit.GetContents(flowManager);
		float num = Mathf.Min(contents.mass, this.currentFlow * dt);
		float num2 = 0f;
		if (num > 0f)
		{
			int disease_count = (int)(num / contents.mass * (float)contents.diseaseCount);
			num2 = flowManager.AddElement(this.outputCell, contents.element, num, contents.temperature, contents.diseaseIdx, disease_count);
			Game.Instance.accumulators.Accumulate(this.flowAccumulator, num2);
			if (num2 > 0f)
			{
				if (this.lastElementTransfered != contents.element && contents.element != SimHashes.Vacuum && this.conduitType == ConduitType.Liquid)
				{
					Element element = ElementLoader.FindElementByHash(contents.element);
					if (element != null)
					{
						Color color = element.substance.colour;
						color.a = 1f;
						this.controller.SetSymbolTint(new KAnimHashedString("water_color"), color);
					}
				}
				this.lastElementTransfered = contents.element;
				flowManager.RemoveElement(this.inputCell, num2);
			}
		}
		this.OnMassTransfer(num2);
		this.UpdateAnim();
	}

	// Token: 0x060038D5 RID: 14549 RVA: 0x0013DF0A File Offset: 0x0013C10A
	protected virtual void OnMassTransfer(float amount)
	{
	}

	// Token: 0x060038D6 RID: 14550 RVA: 0x0013DF0C File Offset: 0x0013C10C
	public virtual void UpdateAnim()
	{
		float averageRate = Game.Instance.accumulators.GetAverageRate(this.flowAccumulator);
		if (averageRate > 0f)
		{
			int i = 0;
			while (i < this.animFlowRanges.Length)
			{
				if (averageRate <= this.animFlowRanges[i].minFlow)
				{
					if (this.curFlowIdx != i)
					{
						this.curFlowIdx = i;
						this.controller.Play(this.animFlowRanges[i].animName, (averageRate <= 0f) ? KAnim.PlayMode.Once : KAnim.PlayMode.Loop, 1f, 0f);
						return;
					}
					return;
				}
				else
				{
					i++;
				}
			}
			return;
		}
		this.controller.Play("off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x040022A5 RID: 8869
	[SerializeField]
	public ConduitType conduitType;

	// Token: 0x040022A6 RID: 8870
	[SerializeField]
	public float maxFlow = 0.5f;

	// Token: 0x040022A7 RID: 8871
	[Serialize]
	private float currentFlow;

	// Token: 0x040022A8 RID: 8872
	[MyCmpGet]
	protected KBatchedAnimController controller;

	// Token: 0x040022A9 RID: 8873
	protected HandleVector<int>.Handle flowAccumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x040022AA RID: 8874
	private int curFlowIdx = -1;

	// Token: 0x040022AB RID: 8875
	private int inputCell;

	// Token: 0x040022AC RID: 8876
	private int outputCell;

	// Token: 0x040022AD RID: 8877
	[SerializeField]
	public ValveBase.AnimRangeInfo[] animFlowRanges;

	// Token: 0x040022AE RID: 8878
	private SimHashes lastElementTransfered = SimHashes.Vacuum;

	// Token: 0x020017CB RID: 6091
	[Serializable]
	public struct AnimRangeInfo
	{
		// Token: 0x06009C95 RID: 40085 RVA: 0x0039A029 File Offset: 0x00398229
		public AnimRangeInfo(float min_flow, string anim_name)
		{
			this.minFlow = min_flow;
			this.animName = anim_name;
		}

		// Token: 0x040078C2 RID: 30914
		public float minFlow;

		// Token: 0x040078C3 RID: 30915
		public string animName;
	}
}
