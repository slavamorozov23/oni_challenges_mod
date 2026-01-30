using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000907 RID: 2311
[AddComponentMenu("KMonoBehaviour/scripts/EdiblesManager")]
public class EdiblesManager : KMonoBehaviour
{
	// Token: 0x06004046 RID: 16454 RVA: 0x0016CAC4 File Offset: 0x0016ACC4
	public static List<EdiblesManager.FoodInfo> GetAllLoadedFoodTypes()
	{
		return EdiblesManager.s_allFoodTypes.Where(new Func<EdiblesManager.FoodInfo, bool>(DlcManager.IsCorrectDlcSubscribed)).ToList<EdiblesManager.FoodInfo>();
	}

	// Token: 0x06004047 RID: 16455 RVA: 0x0016CAE1 File Offset: 0x0016ACE1
	public static void ClearSaveFoodCache()
	{
		EdiblesManager.s_loadedFoodTypes = null;
	}

	// Token: 0x06004048 RID: 16456 RVA: 0x0016CAEC File Offset: 0x0016ACEC
	public static List<EdiblesManager.FoodInfo> GetAllFoodTypes()
	{
		if (EdiblesManager.s_loadedFoodTypes == null)
		{
			EdiblesManager.s_loadedFoodTypes = EdiblesManager.s_allFoodTypes.Where(new Func<EdiblesManager.FoodInfo, bool>(Game.IsCorrectDlcActiveForCurrentSave)).ToList<EdiblesManager.FoodInfo>();
		}
		global::Debug.Assert(SaveLoader.Instance != null, "Call GetAllLoadedFoodTypes from the frontend");
		return EdiblesManager.s_loadedFoodTypes;
	}

	// Token: 0x06004049 RID: 16457 RVA: 0x0016CB3C File Offset: 0x0016AD3C
	public static EdiblesManager.FoodInfo GetFoodInfo(string foodID)
	{
		string key = foodID.Replace("Compost", "");
		EdiblesManager.FoodInfo result = null;
		EdiblesManager.s_allFoodMap.TryGetValue(key, out result);
		return result;
	}

	// Token: 0x0600404A RID: 16458 RVA: 0x0016CB6B File Offset: 0x0016AD6B
	public static bool TryGetFoodInfo(string foodID, out EdiblesManager.FoodInfo info)
	{
		info = null;
		if (string.IsNullOrEmpty(foodID))
		{
			return false;
		}
		info = EdiblesManager.GetFoodInfo(foodID);
		return info != null;
	}

	// Token: 0x040027E1 RID: 10209
	private static List<EdiblesManager.FoodInfo> s_allFoodTypes = new List<EdiblesManager.FoodInfo>();

	// Token: 0x040027E2 RID: 10210
	private static Dictionary<string, EdiblesManager.FoodInfo> s_allFoodMap = new Dictionary<string, EdiblesManager.FoodInfo>();

	// Token: 0x040027E3 RID: 10211
	private static List<EdiblesManager.FoodInfo> s_loadedFoodTypes;

	// Token: 0x0200190F RID: 6415
	public class FoodInfo : IConsumableUIItem, IHasDlcRestrictions
	{
		// Token: 0x0600A144 RID: 41284 RVA: 0x003AB61B File Offset: 0x003A981B
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x0600A145 RID: 41285 RVA: 0x003AB623 File Offset: 0x003A9823
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x0600A146 RID: 41286 RVA: 0x003AB62C File Offset: 0x003A982C
		[Obsolete("Use constructor with required/forbidden instead")]
		public FoodInfo(string id, string dlcId, float caloriesPerUnit, int quality, float preserveTemperatue, float rotTemperature, float spoilTime, bool can_rot) : this(id, caloriesPerUnit, quality, preserveTemperatue, rotTemperature, spoilTime, can_rot, null, null)
		{
			if (dlcId != "")
			{
				this.requiredDlcIds = new string[]
				{
					dlcId
				};
			}
		}

		// Token: 0x0600A147 RID: 41287 RVA: 0x003AB66C File Offset: 0x003A986C
		public FoodInfo(string id, float caloriesPerUnit, int quality, float preserveTemperatue, float rotTemperature, float spoilTime, bool can_rot, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
		{
			this.Id = id;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
			this.CaloriesPerUnit = caloriesPerUnit;
			this.Quality = quality;
			this.PreserveTemperature = preserveTemperatue;
			this.RotTemperature = rotTemperature;
			this.StaleTime = spoilTime / 2f;
			this.SpoilTime = spoilTime;
			this.CanRot = can_rot;
			this.Name = Strings.Get("STRINGS.ITEMS.FOOD." + id.ToUpper() + ".NAME");
			this.Description = Strings.Get("STRINGS.ITEMS.FOOD." + id.ToUpper() + ".DESC");
			this.Effects = new List<string>();
			EdiblesManager.s_allFoodTypes.Add(this);
			EdiblesManager.s_allFoodMap[this.Id] = this;
		}

		// Token: 0x0600A148 RID: 41288 RVA: 0x003AB743 File Offset: 0x003A9943
		public EdiblesManager.FoodInfo AddEffects(List<string> effects, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
		{
			if (DlcManager.IsCorrectDlcSubscribed(requiredDlcIds, forbiddenDlcIds))
			{
				this.Effects.AddRange(effects);
			}
			return this;
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x0600A149 RID: 41289 RVA: 0x003AB75B File Offset: 0x003A995B
		public string ConsumableId
		{
			get
			{
				return this.Id;
			}
		}

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x0600A14A RID: 41290 RVA: 0x003AB763 File Offset: 0x003A9963
		public string ConsumableName
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x0600A14B RID: 41291 RVA: 0x003AB76B File Offset: 0x003A996B
		public int MajorOrder
		{
			get
			{
				return this.Quality;
			}
		}

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x0600A14C RID: 41292 RVA: 0x003AB773 File Offset: 0x003A9973
		public int MinorOrder
		{
			get
			{
				return (int)this.CaloriesPerUnit;
			}
		}

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x0600A14D RID: 41293 RVA: 0x003AB77C File Offset: 0x003A997C
		public bool Display
		{
			get
			{
				return this.CaloriesPerUnit != 0f;
			}
		}

		// Token: 0x04007CBD RID: 31933
		public string Id;

		// Token: 0x04007CBE RID: 31934
		public string Name;

		// Token: 0x04007CBF RID: 31935
		public string Description;

		// Token: 0x04007CC0 RID: 31936
		public float CaloriesPerUnit;

		// Token: 0x04007CC1 RID: 31937
		public float PreserveTemperature;

		// Token: 0x04007CC2 RID: 31938
		public float RotTemperature;

		// Token: 0x04007CC3 RID: 31939
		public float StaleTime;

		// Token: 0x04007CC4 RID: 31940
		public float SpoilTime;

		// Token: 0x04007CC5 RID: 31941
		public bool CanRot;

		// Token: 0x04007CC6 RID: 31942
		public int Quality;

		// Token: 0x04007CC7 RID: 31943
		public List<string> Effects;

		// Token: 0x04007CC8 RID: 31944
		private string[] requiredDlcIds;

		// Token: 0x04007CC9 RID: 31945
		private string[] forbiddenDlcIds;
	}
}
