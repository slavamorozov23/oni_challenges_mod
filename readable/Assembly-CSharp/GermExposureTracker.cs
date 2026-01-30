using System;
using System.Collections.Generic;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x02000972 RID: 2418
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/GermExposureTracker")]
public class GermExposureTracker : KMonoBehaviour
{
	// Token: 0x060044EF RID: 17647 RVA: 0x0018D9C4 File Offset: 0x0018BBC4
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(GermExposureTracker.Instance == null);
		GermExposureTracker.Instance = this;
	}

	// Token: 0x060044F0 RID: 17648 RVA: 0x0018D9DC File Offset: 0x0018BBDC
	protected override void OnSpawn()
	{
		this.rng = new SeededRandom(GameClock.Instance.GetCycle());
	}

	// Token: 0x060044F1 RID: 17649 RVA: 0x0018D9F3 File Offset: 0x0018BBF3
	protected override void OnForcedCleanUp()
	{
		GermExposureTracker.Instance = null;
	}

	// Token: 0x060044F2 RID: 17650 RVA: 0x0018D9FC File Offset: 0x0018BBFC
	public void AddExposure(ExposureType exposure_type, float amount)
	{
		float num;
		this.accumulation.TryGetValue(exposure_type.germ_id, out num);
		float num2 = num + amount;
		if (num2 > 1f)
		{
			using (List<MinionIdentity>.Enumerator enumerator = Components.LiveMinionIdentities.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MinionIdentity cmp = enumerator.Current;
					GermExposureMonitor.Instance smi = cmp.GetSMI<GermExposureMonitor.Instance>();
					if (smi.GetExposureState(exposure_type.germ_id) == GermExposureMonitor.ExposureState.Exposed)
					{
						float exposureWeight = cmp.GetSMI<GermExposureMonitor.Instance>().GetExposureWeight(exposure_type.germ_id);
						if (exposureWeight > 0f)
						{
							this.exposure_candidates.Add(new GermExposureTracker.WeightedExposure
							{
								weight = exposureWeight,
								monitor = smi
							});
						}
					}
				}
				goto IL_F8;
			}
			IL_AF:
			num2 -= 1f;
			if (this.exposure_candidates.Count > 0)
			{
				GermExposureTracker.WeightedExposure weightedExposure = WeightedRandom.Choose<GermExposureTracker.WeightedExposure>(this.exposure_candidates, this.rng);
				this.exposure_candidates.Remove(weightedExposure);
				weightedExposure.monitor.ContractGerms(exposure_type.germ_id);
			}
			IL_F8:
			if (num2 > 1f)
			{
				goto IL_AF;
			}
		}
		this.accumulation[exposure_type.germ_id] = num2;
		this.exposure_candidates.Clear();
	}

	// Token: 0x04002E3A RID: 11834
	public static GermExposureTracker Instance;

	// Token: 0x04002E3B RID: 11835
	[Serialize]
	private Dictionary<HashedString, float> accumulation = new Dictionary<HashedString, float>();

	// Token: 0x04002E3C RID: 11836
	private SeededRandom rng;

	// Token: 0x04002E3D RID: 11837
	private List<GermExposureTracker.WeightedExposure> exposure_candidates = new List<GermExposureTracker.WeightedExposure>();

	// Token: 0x020019AE RID: 6574
	private class WeightedExposure : IWeighted
	{
		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x0600A2DB RID: 41691 RVA: 0x003B0ADC File Offset: 0x003AECDC
		// (set) Token: 0x0600A2DC RID: 41692 RVA: 0x003B0AE4 File Offset: 0x003AECE4
		public float weight { get; set; }

		// Token: 0x04007F15 RID: 32533
		public GermExposureMonitor.Instance monitor;
	}
}
