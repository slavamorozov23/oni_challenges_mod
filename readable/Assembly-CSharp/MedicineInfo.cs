using System;
using System.Collections.Generic;

// Token: 0x020009FB RID: 2555
[Serializable]
public class MedicineInfo
{
	// Token: 0x06004AA6 RID: 19110 RVA: 0x001B01B3 File Offset: 0x001AE3B3
	public MedicineInfo(string id, string effect, MedicineInfo.MedicineType medicineType, string doctorStationId, string[] curedDiseases = null) : this(id, effect, medicineType, doctorStationId, curedDiseases, null)
	{
	}

	// Token: 0x06004AA7 RID: 19111 RVA: 0x001B01C4 File Offset: 0x001AE3C4
	public MedicineInfo(string id, string effect, MedicineInfo.MedicineType medicineType, string doctorStationId, string[] curedDiseases, string[] curedEffects)
	{
		Debug.Assert(!string.IsNullOrEmpty(effect) || (curedDiseases != null && curedDiseases.Length != 0), "Medicine should have an effect or cure diseases");
		this.id = id;
		this.effect = effect;
		this.medicineType = medicineType;
		this.doctorStationId = doctorStationId;
		if (curedDiseases != null)
		{
			this.curedSicknesses = new List<string>(curedDiseases);
		}
		else
		{
			this.curedSicknesses = new List<string>();
		}
		if (curedEffects != null)
		{
			this.curedEffects = new List<string>(curedEffects);
			return;
		}
		this.curedEffects = new List<string>();
	}

	// Token: 0x06004AA8 RID: 19112 RVA: 0x001B0251 File Offset: 0x001AE451
	public Tag GetSupplyTag()
	{
		return MedicineInfo.GetSupplyTagForStation(this.doctorStationId);
	}

	// Token: 0x06004AA9 RID: 19113 RVA: 0x001B0260 File Offset: 0x001AE460
	public static Tag GetSupplyTagForStation(string stationID)
	{
		Tag tag = TagManager.Create(stationID + GameTags.MedicalSupplies.Name);
		Assets.AddCountableTag(tag);
		return tag;
	}

	// Token: 0x04003176 RID: 12662
	public string id;

	// Token: 0x04003177 RID: 12663
	public string effect;

	// Token: 0x04003178 RID: 12664
	public MedicineInfo.MedicineType medicineType;

	// Token: 0x04003179 RID: 12665
	public List<string> curedSicknesses;

	// Token: 0x0400317A RID: 12666
	public List<string> curedEffects;

	// Token: 0x0400317B RID: 12667
	public string doctorStationId;

	// Token: 0x02001A61 RID: 6753
	public enum MedicineType
	{
		// Token: 0x0400817A RID: 33146
		Booster,
		// Token: 0x0400817B RID: 33147
		CureAny,
		// Token: 0x0400817C RID: 33148
		CureSpecific
	}
}
