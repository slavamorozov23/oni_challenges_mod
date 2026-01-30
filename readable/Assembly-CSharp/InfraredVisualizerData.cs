using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020005ED RID: 1517
public struct InfraredVisualizerData
{
	// Token: 0x06002319 RID: 8985 RVA: 0x000CBA70 File Offset: 0x000C9C70
	public void Update()
	{
		float num = 0f;
		if (this.temperatureAmount != null)
		{
			num = this.temperatureAmount.value;
		}
		else if (this.structureTemperature.IsValid())
		{
			num = GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
		}
		else if (this.primaryElement != null)
		{
			num = this.primaryElement.Temperature;
		}
		else if (this.temperatureVulnerable != null)
		{
			num = this.temperatureVulnerable.InternalTemperature;
		}
		else if (this.critterTemperatureMonitorInstance != null)
		{
			num = this.critterTemperatureMonitorInstance.GetTemperatureInternal();
		}
		if (num < 0f)
		{
			return;
		}
		Color32 c = SimDebugView.Instance.NormalizedTemperature(num);
		this.controller.OverlayColour = c;
	}

	// Token: 0x0600231A RID: 8986 RVA: 0x000CBB38 File Offset: 0x000C9D38
	public InfraredVisualizerData(GameObject go)
	{
		this.controller = go.GetComponent<KBatchedAnimController>();
		if (this.controller != null)
		{
			this.temperatureAmount = Db.Get().Amounts.Temperature.Lookup(go);
			this.structureTemperature = GameComps.StructureTemperatures.GetHandle(go);
			this.primaryElement = go.GetComponent<PrimaryElement>();
			this.temperatureVulnerable = go.GetComponent<TemperatureVulnerable>();
			this.critterTemperatureMonitorInstance = go.GetSMI<CritterTemperatureMonitor.Instance>();
			return;
		}
		this.temperatureAmount = null;
		this.structureTemperature = HandleVector<int>.InvalidHandle;
		this.primaryElement = null;
		this.temperatureVulnerable = null;
		this.critterTemperatureMonitorInstance = null;
	}

	// Token: 0x04001489 RID: 5257
	public KAnimControllerBase controller;

	// Token: 0x0400148A RID: 5258
	public AmountInstance temperatureAmount;

	// Token: 0x0400148B RID: 5259
	public HandleVector<int>.Handle structureTemperature;

	// Token: 0x0400148C RID: 5260
	public PrimaryElement primaryElement;

	// Token: 0x0400148D RID: 5261
	public TemperatureVulnerable temperatureVulnerable;

	// Token: 0x0400148E RID: 5262
	public CritterTemperatureMonitor.Instance critterTemperatureMonitorInstance;
}
