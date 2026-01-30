using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;

// Token: 0x02000970 RID: 2416
[AddComponentMenu("KMonoBehaviour/scripts/GeyserConfigurator")]
public class GeyserConfigurator : KMonoBehaviour
{
	// Token: 0x060044DB RID: 17627 RVA: 0x0018D37C File Offset: 0x0018B57C
	public static GeyserConfigurator.GeyserType FindType(HashedString typeId)
	{
		GeyserConfigurator.GeyserType geyserType = null;
		if (typeId != HashedString.Invalid)
		{
			geyserType = GeyserConfigurator.geyserTypes.Find((GeyserConfigurator.GeyserType t) => t.id == typeId);
		}
		if (geyserType == null)
		{
			global::Debug.LogError(string.Format("Tried finding a geyser with id {0} but it doesn't exist!", typeId.ToString()));
		}
		return geyserType;
	}

	// Token: 0x060044DC RID: 17628 RVA: 0x0018D3E5 File Offset: 0x0018B5E5
	public GeyserConfigurator.GeyserInstanceConfiguration MakeConfiguration()
	{
		return this.CreateRandomInstance(this.presetType, this.presetMin, this.presetMax);
	}

	// Token: 0x060044DD RID: 17629 RVA: 0x0018D400 File Offset: 0x0018B600
	private GeyserConfigurator.GeyserInstanceConfiguration CreateRandomInstance(HashedString typeId, float min, float max)
	{
		KRandom randomSource = new KRandom(SaveLoader.Instance.clusterDetailSave.globalWorldSeed + (int)base.transform.GetPosition().x + (int)base.transform.GetPosition().y);
		return new GeyserConfigurator.GeyserInstanceConfiguration
		{
			typeId = typeId,
			rateRoll = this.Roll(randomSource, min, max),
			iterationLengthRoll = this.Roll(randomSource, 0f, 1f),
			iterationPercentRoll = this.Roll(randomSource, min, max),
			yearLengthRoll = this.Roll(randomSource, 0f, 1f),
			yearPercentRoll = this.Roll(randomSource, min, max)
		};
	}

	// Token: 0x060044DE RID: 17630 RVA: 0x0018D4AD File Offset: 0x0018B6AD
	private float Roll(KRandom randomSource, float min, float max)
	{
		return (float)(randomSource.NextDouble() * (double)(max - min)) + min;
	}

	// Token: 0x04002E33 RID: 11827
	private static List<GeyserConfigurator.GeyserType> geyserTypes;

	// Token: 0x04002E34 RID: 11828
	public HashedString presetType;

	// Token: 0x04002E35 RID: 11829
	public float presetMin;

	// Token: 0x04002E36 RID: 11830
	public float presetMax = 1f;

	// Token: 0x020019A9 RID: 6569
	public enum GeyserShape
	{
		// Token: 0x04007EEA RID: 32490
		Gas,
		// Token: 0x04007EEB RID: 32491
		Liquid,
		// Token: 0x04007EEC RID: 32492
		Molten
	}

	// Token: 0x020019AA RID: 6570
	public class GeyserType : IHasDlcRestrictions
	{
		// Token: 0x0600A2B9 RID: 41657 RVA: 0x003B0474 File Offset: 0x003AE674
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x0600A2BA RID: 41658 RVA: 0x003B047C File Offset: 0x003AE67C
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x0600A2BB RID: 41659 RVA: 0x003B0484 File Offset: 0x003AE684
		public GeyserType(string id, SimHashes element, GeyserConfigurator.GeyserShape shape, float temperature, float minRatePerCycle, float maxRatePerCycle, float maxPressure, string[] requiredDlcIds, string[] forbiddenDlcIds = null, float minIterationLength = 60f, float maxIterationLength = 1140f, float minIterationPercent = 0.1f, float maxIterationPercent = 0.9f, float minYearLength = 15000f, float maxYearLength = 135000f, float minYearPercent = 0.4f, float maxYearPercent = 0.8f, float geyserTemperature = 372.15f)
		{
			this.id = id;
			this.idHash = id;
			this.element = element;
			this.shape = shape;
			this.temperature = temperature;
			this.minRatePerCycle = minRatePerCycle;
			this.maxRatePerCycle = maxRatePerCycle;
			this.maxPressure = maxPressure;
			this.minIterationLength = minIterationLength;
			this.maxIterationLength = maxIterationLength;
			this.minIterationPercent = minIterationPercent;
			this.maxIterationPercent = maxIterationPercent;
			this.minYearLength = minYearLength;
			this.maxYearLength = maxYearLength;
			this.minYearPercent = minYearPercent;
			this.maxYearPercent = maxYearPercent;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
			this.geyserTemperature = geyserTemperature;
			if (GeyserConfigurator.geyserTypes == null)
			{
				GeyserConfigurator.geyserTypes = new List<GeyserConfigurator.GeyserType>();
			}
			GeyserConfigurator.geyserTypes.Add(this);
		}

		// Token: 0x0600A2BC RID: 41660 RVA: 0x003B0558 File Offset: 0x003AE758
		[Obsolete]
		public GeyserType(string id, SimHashes element, GeyserConfigurator.GeyserShape shape, float temperature, float minRatePerCycle, float maxRatePerCycle, float maxPressure, float minIterationLength = 60f, float maxIterationLength = 1140f, float minIterationPercent = 0.1f, float maxIterationPercent = 0.9f, float minYearLength = 15000f, float maxYearLength = 135000f, float minYearPercent = 0.4f, float maxYearPercent = 0.8f, float geyserTemperature = 372.15f, string DlcID = "")
		{
			this.id = id;
			this.idHash = id;
			this.element = element;
			this.shape = shape;
			this.temperature = temperature;
			this.minRatePerCycle = minRatePerCycle;
			this.maxRatePerCycle = maxRatePerCycle;
			this.maxPressure = maxPressure;
			this.minIterationLength = minIterationLength;
			this.maxIterationLength = maxIterationLength;
			this.minIterationPercent = minIterationPercent;
			this.maxIterationPercent = maxIterationPercent;
			this.minYearLength = minYearLength;
			this.maxYearLength = maxYearLength;
			this.minYearPercent = minYearPercent;
			this.maxYearPercent = maxYearPercent;
			this.requiredDlcIds = new string[]
			{
				DlcID
			};
			this.geyserTemperature = geyserTemperature;
			if (GeyserConfigurator.geyserTypes == null)
			{
				GeyserConfigurator.geyserTypes = new List<GeyserConfigurator.GeyserType>();
			}
			GeyserConfigurator.geyserTypes.Add(this);
		}

		// Token: 0x0600A2BD RID: 41661 RVA: 0x003B062C File Offset: 0x003AE82C
		public GeyserConfigurator.GeyserType AddDisease(SimUtil.DiseaseInfo diseaseInfo)
		{
			this.diseaseInfo = diseaseInfo;
			return this;
		}

		// Token: 0x0600A2BE RID: 41662 RVA: 0x003B0638 File Offset: 0x003AE838
		public GeyserType()
		{
			this.id = "Blank";
			this.element = SimHashes.Void;
			this.temperature = 0f;
			this.minRatePerCycle = 0f;
			this.maxRatePerCycle = 0f;
			this.maxPressure = 0f;
			this.minIterationLength = 0f;
			this.maxIterationLength = 0f;
			this.minIterationPercent = 0f;
			this.maxIterationPercent = 0f;
			this.minYearLength = 0f;
			this.maxYearLength = 0f;
			this.minYearPercent = 0f;
			this.maxYearPercent = 0f;
			this.geyserTemperature = 0f;
		}

		// Token: 0x04007EED RID: 32493
		public string id;

		// Token: 0x04007EEE RID: 32494
		public HashedString idHash;

		// Token: 0x04007EEF RID: 32495
		public SimHashes element;

		// Token: 0x04007EF0 RID: 32496
		public GeyserConfigurator.GeyserShape shape;

		// Token: 0x04007EF1 RID: 32497
		public float temperature;

		// Token: 0x04007EF2 RID: 32498
		public float minRatePerCycle;

		// Token: 0x04007EF3 RID: 32499
		public float maxRatePerCycle;

		// Token: 0x04007EF4 RID: 32500
		public float maxPressure;

		// Token: 0x04007EF5 RID: 32501
		public SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;

		// Token: 0x04007EF6 RID: 32502
		public float minIterationLength;

		// Token: 0x04007EF7 RID: 32503
		public float maxIterationLength;

		// Token: 0x04007EF8 RID: 32504
		public float minIterationPercent;

		// Token: 0x04007EF9 RID: 32505
		public float maxIterationPercent;

		// Token: 0x04007EFA RID: 32506
		public float minYearLength;

		// Token: 0x04007EFB RID: 32507
		public float maxYearLength;

		// Token: 0x04007EFC RID: 32508
		public float minYearPercent;

		// Token: 0x04007EFD RID: 32509
		public float maxYearPercent;

		// Token: 0x04007EFE RID: 32510
		public float geyserTemperature;

		// Token: 0x04007EFF RID: 32511
		[Obsolete]
		public string DlcID;

		// Token: 0x04007F00 RID: 32512
		public string[] requiredDlcIds;

		// Token: 0x04007F01 RID: 32513
		public string[] forbiddenDlcIds;

		// Token: 0x04007F02 RID: 32514
		public const string BLANK_ID = "Blank";

		// Token: 0x04007F03 RID: 32515
		public const SimHashes BLANK_ELEMENT = SimHashes.Void;
	}

	// Token: 0x020019AB RID: 6571
	[Serializable]
	public class GeyserInstanceConfiguration
	{
		// Token: 0x0600A2BF RID: 41663 RVA: 0x003B06FB File Offset: 0x003AE8FB
		public Geyser.GeyserModification GetModifier()
		{
			return this.modifier;
		}

		// Token: 0x0600A2C0 RID: 41664 RVA: 0x003B0704 File Offset: 0x003AE904
		public void Init(bool reinit = false)
		{
			if (this.didInit && !reinit)
			{
				return;
			}
			this.didInit = true;
			this.scaledRate = this.Resample(this.rateRoll, this.geyserType.minRatePerCycle, this.geyserType.maxRatePerCycle);
			this.scaledIterationLength = this.Resample(this.iterationLengthRoll, this.geyserType.minIterationLength, this.geyserType.maxIterationLength);
			this.scaledIterationPercent = this.Resample(this.iterationPercentRoll, this.geyserType.minIterationPercent, this.geyserType.maxIterationPercent);
			this.scaledYearLength = this.Resample(this.yearLengthRoll, this.geyserType.minYearLength, this.geyserType.maxYearLength);
			this.scaledYearPercent = this.Resample(this.yearPercentRoll, this.geyserType.minYearPercent, this.geyserType.maxYearPercent);
		}

		// Token: 0x0600A2C1 RID: 41665 RVA: 0x003B07EC File Offset: 0x003AE9EC
		public void SetModifier(Geyser.GeyserModification modifier)
		{
			this.modifier = modifier;
		}

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x0600A2C2 RID: 41666 RVA: 0x003B07F5 File Offset: 0x003AE9F5
		public GeyserConfigurator.GeyserType geyserType
		{
			get
			{
				return GeyserConfigurator.FindType(this.typeId);
			}
		}

		// Token: 0x0600A2C3 RID: 41667 RVA: 0x003B0804 File Offset: 0x003AEA04
		private float GetModifiedValue(float geyserVariable, float modifier, Geyser.ModificationMethod method)
		{
			float num = geyserVariable;
			if (method != Geyser.ModificationMethod.Values)
			{
				if (method == Geyser.ModificationMethod.Percentages)
				{
					num += geyserVariable * modifier;
				}
			}
			else
			{
				num += modifier;
			}
			return num;
		}

		// Token: 0x0600A2C4 RID: 41668 RVA: 0x003B0827 File Offset: 0x003AEA27
		public float GetMaxPressure()
		{
			return this.GetModifiedValue(this.geyserType.maxPressure, this.modifier.maxPressureModifier, Geyser.maxPressureModificationMethod);
		}

		// Token: 0x0600A2C5 RID: 41669 RVA: 0x003B084A File Offset: 0x003AEA4A
		public float GetIterationLength()
		{
			this.Init(false);
			return this.GetModifiedValue(this.scaledIterationLength, this.modifier.iterationDurationModifier, Geyser.IterationDurationModificationMethod);
		}

		// Token: 0x0600A2C6 RID: 41670 RVA: 0x003B086F File Offset: 0x003AEA6F
		public float GetIterationPercent()
		{
			this.Init(false);
			return Mathf.Clamp(this.GetModifiedValue(this.scaledIterationPercent, this.modifier.iterationPercentageModifier, Geyser.IterationPercentageModificationMethod), 0f, 1f);
		}

		// Token: 0x0600A2C7 RID: 41671 RVA: 0x003B08A3 File Offset: 0x003AEAA3
		public float GetOnDuration()
		{
			return this.GetIterationLength() * this.GetIterationPercent();
		}

		// Token: 0x0600A2C8 RID: 41672 RVA: 0x003B08B2 File Offset: 0x003AEAB2
		public float GetOffDuration()
		{
			return this.GetIterationLength() * (1f - this.GetIterationPercent());
		}

		// Token: 0x0600A2C9 RID: 41673 RVA: 0x003B08C7 File Offset: 0x003AEAC7
		public float GetMassPerCycle()
		{
			this.Init(false);
			return this.GetModifiedValue(this.scaledRate, this.modifier.massPerCycleModifier, Geyser.massModificationMethod);
		}

		// Token: 0x0600A2CA RID: 41674 RVA: 0x003B08EC File Offset: 0x003AEAEC
		public float GetEmitRate()
		{
			float num = 600f / this.GetIterationLength();
			return this.GetMassPerCycle() / num / this.GetOnDuration();
		}

		// Token: 0x0600A2CB RID: 41675 RVA: 0x003B0915 File Offset: 0x003AEB15
		public float GetYearLength()
		{
			this.Init(false);
			return this.GetModifiedValue(this.scaledYearLength, this.modifier.yearDurationModifier, Geyser.yearDurationModificationMethod);
		}

		// Token: 0x0600A2CC RID: 41676 RVA: 0x003B093A File Offset: 0x003AEB3A
		public float GetYearPercent()
		{
			this.Init(false);
			return Mathf.Clamp(this.GetModifiedValue(this.scaledYearPercent, this.modifier.yearPercentageModifier, Geyser.yearPercentageModificationMethod), 0f, 1f);
		}

		// Token: 0x0600A2CD RID: 41677 RVA: 0x003B096E File Offset: 0x003AEB6E
		public float GetYearOnDuration()
		{
			return this.GetYearLength() * this.GetYearPercent();
		}

		// Token: 0x0600A2CE RID: 41678 RVA: 0x003B097D File Offset: 0x003AEB7D
		public float GetYearOffDuration()
		{
			return this.GetYearLength() * (1f - this.GetYearPercent());
		}

		// Token: 0x0600A2CF RID: 41679 RVA: 0x003B0992 File Offset: 0x003AEB92
		public SimHashes GetElement()
		{
			if (!this.modifier.modifyElement || this.modifier.newElement == (SimHashes)0)
			{
				return this.geyserType.element;
			}
			return this.modifier.newElement;
		}

		// Token: 0x0600A2D0 RID: 41680 RVA: 0x003B09C5 File Offset: 0x003AEBC5
		public float GetTemperature()
		{
			return this.GetModifiedValue(this.geyserType.temperature, this.modifier.temperatureModifier, Geyser.temperatureModificationMethod);
		}

		// Token: 0x0600A2D1 RID: 41681 RVA: 0x003B09E8 File Offset: 0x003AEBE8
		public byte GetDiseaseIdx()
		{
			return this.geyserType.diseaseInfo.idx;
		}

		// Token: 0x0600A2D2 RID: 41682 RVA: 0x003B09FA File Offset: 0x003AEBFA
		public int GetDiseaseCount()
		{
			return this.geyserType.diseaseInfo.count;
		}

		// Token: 0x0600A2D3 RID: 41683 RVA: 0x003B0A0C File Offset: 0x003AEC0C
		public float GetAverageEmission()
		{
			float num = this.GetEmitRate() * this.GetOnDuration();
			return this.GetYearOnDuration() / this.GetIterationLength() * num / this.GetYearLength();
		}

		// Token: 0x0600A2D4 RID: 41684 RVA: 0x003B0A40 File Offset: 0x003AEC40
		private float Resample(float t, float min, float max)
		{
			float num = 6f;
			float num2 = 0.002472623f;
			float num3 = t * (1f - num2 * 2f) + num2;
			return (-Mathf.Log(1f / num3 - 1f) + num) / (num * 2f) * (max - min) + min;
		}

		// Token: 0x04007F04 RID: 32516
		public HashedString typeId;

		// Token: 0x04007F05 RID: 32517
		public float rateRoll;

		// Token: 0x04007F06 RID: 32518
		public float iterationLengthRoll;

		// Token: 0x04007F07 RID: 32519
		public float iterationPercentRoll;

		// Token: 0x04007F08 RID: 32520
		public float yearLengthRoll;

		// Token: 0x04007F09 RID: 32521
		public float yearPercentRoll;

		// Token: 0x04007F0A RID: 32522
		public float scaledRate;

		// Token: 0x04007F0B RID: 32523
		public float scaledIterationLength;

		// Token: 0x04007F0C RID: 32524
		public float scaledIterationPercent;

		// Token: 0x04007F0D RID: 32525
		public float scaledYearLength;

		// Token: 0x04007F0E RID: 32526
		public float scaledYearPercent;

		// Token: 0x04007F0F RID: 32527
		private bool didInit;

		// Token: 0x04007F10 RID: 32528
		private Geyser.GeyserModification modifier;
	}
}
