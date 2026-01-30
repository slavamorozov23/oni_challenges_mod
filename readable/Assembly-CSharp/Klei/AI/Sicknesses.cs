using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001031 RID: 4145
	public class Sicknesses : Modifications<Sickness, SicknessInstance>
	{
		// Token: 0x060080AD RID: 32941 RVA: 0x0033ACAF File Offset: 0x00338EAF
		public Sicknesses(GameObject go) : base(go, Db.Get().Sicknesses)
		{
		}

		// Token: 0x060080AE RID: 32942 RVA: 0x0033ACC4 File Offset: 0x00338EC4
		public void Infect(SicknessExposureInfo exposure_info)
		{
			Sickness modifier = Db.Get().Sicknesses.Get(exposure_info.sicknessID);
			if (!base.Has(modifier))
			{
				this.CreateInstance(modifier).ExposureInfo = exposure_info;
			}
		}

		// Token: 0x060080AF RID: 32943 RVA: 0x0033AD00 File Offset: 0x00338F00
		public override SicknessInstance CreateInstance(Sickness sickness)
		{
			SicknessInstance sicknessInstance = new SicknessInstance(base.gameObject, sickness);
			this.Add(sicknessInstance);
			base.Trigger(GameHashes.SicknessAdded, sicknessInstance);
			ReportManager.Instance.ReportValue(ReportManager.ReportType.DiseaseAdded, 1f, base.gameObject.GetProperName(), null);
			return sicknessInstance;
		}

		// Token: 0x060080B0 RID: 32944 RVA: 0x0033AD4B File Offset: 0x00338F4B
		public bool IsInfected()
		{
			return base.Count > 0;
		}

		// Token: 0x060080B1 RID: 32945 RVA: 0x0033AD56 File Offset: 0x00338F56
		public bool Cure(Sickness sickness)
		{
			return this.Cure(sickness.Id);
		}

		// Token: 0x060080B2 RID: 32946 RVA: 0x0033AD64 File Offset: 0x00338F64
		public bool Cure(string sickness_id)
		{
			SicknessInstance sicknessInstance = null;
			foreach (SicknessInstance sicknessInstance2 in this)
			{
				if (sicknessInstance2.modifier.Id == sickness_id)
				{
					sicknessInstance = sicknessInstance2;
					break;
				}
			}
			if (sicknessInstance != null)
			{
				this.Remove(sicknessInstance);
				base.Trigger(GameHashes.SicknessCured, sicknessInstance);
				ReportManager.Instance.ReportValue(ReportManager.ReportType.DiseaseAdded, -1f, base.gameObject.GetProperName(), null);
				return true;
			}
			return false;
		}
	}
}
