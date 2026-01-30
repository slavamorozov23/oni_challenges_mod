using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F62 RID: 3938
	public class TechItems : ResourceSet<TechItem>
	{
		// Token: 0x06007CF1 RID: 31985 RVA: 0x0031983F File Offset: 0x00317A3F
		public TechItems(ResourceSet parent) : base("TechItems", parent)
		{
		}

		// Token: 0x06007CF2 RID: 31986 RVA: 0x00319850 File Offset: 0x00317A50
		public void Init()
		{
			this.automationOverlay = this.AddTechItem("AutomationOverlay", RESEARCH.OTHER_TECH_ITEMS.AUTOMATION_OVERLAY.NAME, RESEARCH.OTHER_TECH_ITEMS.AUTOMATION_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_logic"), null, null, false);
			this.suitsOverlay = this.AddTechItem("SuitsOverlay", RESEARCH.OTHER_TECH_ITEMS.SUITS_OVERLAY.NAME, RESEARCH.OTHER_TECH_ITEMS.SUITS_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_suit"), null, null, false);
			this.betaResearchPoint = this.AddTechItem("BetaResearchPoint", RESEARCH.OTHER_TECH_ITEMS.BETA_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.BETA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_beta_icon"), null, null, false);
			this.gammaResearchPoint = this.AddTechItem("GammaResearchPoint", RESEARCH.OTHER_TECH_ITEMS.GAMMA_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.GAMMA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_gamma_icon"), null, null, false);
			this.orbitalResearchPoint = this.AddTechItem("OrbitalResearchPoint", RESEARCH.OTHER_TECH_ITEMS.ORBITAL_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.ORBITAL_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_orbital_icon"), null, null, false);
			this.conveyorOverlay = this.AddTechItem("ConveyorOverlay", RESEARCH.OTHER_TECH_ITEMS.CONVEYOR_OVERLAY.NAME, RESEARCH.OTHER_TECH_ITEMS.CONVEYOR_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_conveyor"), null, null, false);
			this.jetSuit = this.AddTechItem("JetSuit", RESEARCH.OTHER_TECH_ITEMS.JET_SUIT.NAME, RESEARCH.OTHER_TECH_ITEMS.JET_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Jet_Suit".ToTag()), null, null, false);
			if (this.jetSuit != null)
			{
				this.jetSuit.AddSearchTerms(SEARCH_TERMS.ATMOSUIT);
			}
			this.atmoSuit = this.AddTechItem("AtmoSuit", RESEARCH.OTHER_TECH_ITEMS.ATMO_SUIT.NAME, RESEARCH.OTHER_TECH_ITEMS.ATMO_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Atmo_Suit".ToTag()), null, null, false);
			if (this.atmoSuit != null)
			{
				this.atmoSuit.AddSearchTerms(SEARCH_TERMS.ATMOSUIT);
			}
			this.oxygenMask = this.AddTechItem("OxygenMask", RESEARCH.OTHER_TECH_ITEMS.OXYGEN_MASK.NAME, RESEARCH.OTHER_TECH_ITEMS.OXYGEN_MASK.DESC, this.GetPrefabSpriteFnBuilder("Oxygen_Mask".ToTag()), null, null, false);
			if (this.oxygenMask != null)
			{
				this.oxygenMask.AddSearchTerms(SEARCH_TERMS.OXYGEN);
			}
			this.superLiquids = this.AddTechItem("SUPER_LIQUIDS", RESEARCH.OTHER_TECH_ITEMS.SUPER_LIQUIDS.NAME, RESEARCH.OTHER_TECH_ITEMS.SUPER_LIQUIDS.DESC, this.GetPrefabSpriteFnBuilder(SimHashes.ViscoGel.CreateTag()), null, null, false);
			this.deltaResearchPoint = this.AddTechItem("DeltaResearchPoint", RESEARCH.OTHER_TECH_ITEMS.DELTA_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.DELTA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_delta_icon"), DlcManager.EXPANSION1, null, false);
			this.leadSuit = this.AddTechItem("LeadSuit", RESEARCH.OTHER_TECH_ITEMS.LEAD_SUIT.NAME, RESEARCH.OTHER_TECH_ITEMS.LEAD_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Lead_Suit".ToTag()), DlcManager.EXPANSION1, null, false);
			this.disposableElectrobankMetalOre = this.AddTechItem("DisposableElectrobank_RawMetal", RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_METAL_ORE.NAME, RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_METAL_ORE.DESC, this.GetPrefabSpriteFnBuilder("DisposableElectrobank_RawMetal".ToTag()), DlcManager.DLC3, null, false);
			if (this.disposableElectrobankMetalOre != null)
			{
				this.disposableElectrobankMetalOre.AddSearchTerms(SEARCH_TERMS.BATTERY);
			}
			this.lubricationStick = this.AddTechItem("LubricationStick", RESEARCH.OTHER_TECH_ITEMS.LUBRICATION_STICK.NAME, RESEARCH.OTHER_TECH_ITEMS.LUBRICATION_STICK.DESC, this.GetPrefabSpriteFnBuilder("LubricationStick".ToTag()), DlcManager.DLC3, null, false);
			if (this.lubricationStick != null)
			{
				this.lubricationStick.AddSearchTerms(SEARCH_TERMS.MEDICINE);
				this.lubricationStick.AddSearchTerms(SEARCH_TERMS.BIONIC);
			}
			this.disposableElectrobankUraniumOre = this.AddTechItem("DisposableElectrobank_UraniumOre", RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_URANIUM_ORE.NAME, RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_URANIUM_ORE.DESC, this.GetPrefabSpriteFnBuilder("DisposableElectrobank_UraniumOre".ToTag()), new string[]
			{
				"EXPANSION1_ID",
				"DLC3_ID"
			}, null, false);
			if (this.disposableElectrobankUraniumOre != null)
			{
				this.disposableElectrobankUraniumOre.AddSearchTerms(SEARCH_TERMS.BATTERY);
			}
			this.electrobank = this.AddTechItem("Electrobank", RESEARCH.OTHER_TECH_ITEMS.ELECTROBANK.NAME, RESEARCH.OTHER_TECH_ITEMS.ELECTROBANK.DESC, this.GetPrefabSpriteFnBuilder("Electrobank".ToTag()), DlcManager.DLC3, null, false);
			if (this.electrobank != null)
			{
				this.electrobank.AddSearchTerms(SEARCH_TERMS.BATTERY);
			}
			this.fetchDrone = this.AddTechItem("FetchDrone", RESEARCH.OTHER_TECH_ITEMS.FETCHDRONE.NAME, RESEARCH.OTHER_TECH_ITEMS.FETCHDRONE.DESC, this.GetPrefabSpriteFnBuilder("FetchDrone".ToTag()), DlcManager.DLC3, null, false);
			if (this.fetchDrone != null)
			{
				this.fetchDrone.AddSearchTerms(SEARCH_TERMS.ROBOT);
			}
			this.selfChargingElectrobank = this.AddTechItem("SelfChargingElectrobank", RESEARCH.OTHER_TECH_ITEMS.SELFCHARGINGELECTROBANK.NAME, RESEARCH.OTHER_TECH_ITEMS.SELFCHARGINGELECTROBANK.DESC, this.GetPrefabSpriteFnBuilder("SelfChargingElectrobank".ToTag()), new string[]
			{
				"EXPANSION1_ID",
				"DLC3_ID"
			}, null, false);
			if (this.selfChargingElectrobank != null)
			{
				this.selfChargingElectrobank.AddSearchTerms(SEARCH_TERMS.BATTERY);
			}
		}

		// Token: 0x06007CF3 RID: 31987 RVA: 0x00319D86 File Offset: 0x00317F86
		private Func<string, bool, Sprite> GetSpriteFnBuilder(string spriteName)
		{
			return (string anim, bool centered) => Assets.GetSprite(spriteName);
		}

		// Token: 0x06007CF4 RID: 31988 RVA: 0x00319D9F File Offset: 0x00317F9F
		private Func<string, bool, Sprite> GetPrefabSpriteFnBuilder(Tag prefabTag)
		{
			return (string anim, bool centered) => Def.GetUISprite(prefabTag, "ui", false).first;
		}

		// Token: 0x06007CF5 RID: 31989 RVA: 0x00319DB8 File Offset: 0x00317FB8
		[Obsolete("Used AddTechItem with requiredDlcIds and forbiddenDlcIds instead.")]
		public TechItem AddTechItem(string id, string name, string description, Func<string, bool, Sprite> getUISprite, string[] DLCIds, bool poi_unlock = false)
		{
			string[] requiredDlcIds;
			string[] forbiddenDlcIds;
			DlcManager.ConvertAvailableToRequireAndForbidden(DLCIds, out requiredDlcIds, out forbiddenDlcIds);
			return this.AddTechItem(id, name, description, getUISprite, requiredDlcIds, forbiddenDlcIds, poi_unlock);
		}

		// Token: 0x06007CF6 RID: 31990 RVA: 0x00319DE0 File Offset: 0x00317FE0
		public TechItem AddTechItem(string id, string name, string description, Func<string, bool, Sprite> getUISprite, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null, bool poi_unlock = false)
		{
			if (!DlcManager.IsCorrectDlcSubscribed(requiredDlcIds, forbiddenDlcIds))
			{
				return null;
			}
			if (base.TryGet(id) != null)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Tried adding a tech item called",
					id,
					name,
					"but it was already added!"
				});
				return base.Get(id);
			}
			Tech techFromItemID = this.GetTechFromItemID(id);
			if (techFromItemID == null)
			{
				return null;
			}
			TechItem techItem = new TechItem(id, this, name, description, getUISprite, techFromItemID.Id, requiredDlcIds, forbiddenDlcIds, poi_unlock);
			techFromItemID.unlockedItems.Add(techItem);
			return techItem;
		}

		// Token: 0x06007CF7 RID: 31991 RVA: 0x00319E60 File Offset: 0x00318060
		public bool IsTechItemComplete(string id)
		{
			bool result = true;
			foreach (TechItem techItem in this.resources)
			{
				if (techItem.Id == id)
				{
					result = techItem.IsComplete();
					break;
				}
			}
			return result;
		}

		// Token: 0x06007CF8 RID: 31992 RVA: 0x00319EC8 File Offset: 0x003180C8
		public Tech GetTechFromItemID(string itemId)
		{
			Techs techs = Db.Get().Techs;
			if (techs == null)
			{
				return null;
			}
			return techs.TryGetTechForTechItem(itemId);
		}

		// Token: 0x06007CF9 RID: 31993 RVA: 0x00319EE0 File Offset: 0x003180E0
		public int GetTechTierForItem(string itemId)
		{
			Tech techFromItemID = this.GetTechFromItemID(itemId);
			if (techFromItemID != null)
			{
				return Techs.GetTier(techFromItemID);
			}
			return 0;
		}

		// Token: 0x04005BC4 RID: 23492
		public const string AUTOMATION_OVERLAY_ID = "AutomationOverlay";

		// Token: 0x04005BC5 RID: 23493
		public TechItem automationOverlay;

		// Token: 0x04005BC6 RID: 23494
		public const string SUITS_OVERLAY_ID = "SuitsOverlay";

		// Token: 0x04005BC7 RID: 23495
		public TechItem suitsOverlay;

		// Token: 0x04005BC8 RID: 23496
		public const string JET_SUIT_ID = "JetSuit";

		// Token: 0x04005BC9 RID: 23497
		public TechItem jetSuit;

		// Token: 0x04005BCA RID: 23498
		public const string ATMO_SUIT_ID = "AtmoSuit";

		// Token: 0x04005BCB RID: 23499
		public TechItem atmoSuit;

		// Token: 0x04005BCC RID: 23500
		public const string OXYGEN_MASK_ID = "OxygenMask";

		// Token: 0x04005BCD RID: 23501
		public TechItem oxygenMask;

		// Token: 0x04005BCE RID: 23502
		public const string LEAD_SUIT_ID = "LeadSuit";

		// Token: 0x04005BCF RID: 23503
		public TechItem leadSuit;

		// Token: 0x04005BD0 RID: 23504
		public TechItem disposableElectrobankMetalOre;

		// Token: 0x04005BD1 RID: 23505
		public TechItem lubricationStick;

		// Token: 0x04005BD2 RID: 23506
		public TechItem disposableElectrobankUraniumOre;

		// Token: 0x04005BD3 RID: 23507
		public TechItem electrobank;

		// Token: 0x04005BD4 RID: 23508
		public TechItem fetchDrone;

		// Token: 0x04005BD5 RID: 23509
		public TechItem selfChargingElectrobank;

		// Token: 0x04005BD6 RID: 23510
		public TechItem superLiquids;

		// Token: 0x04005BD7 RID: 23511
		public const string BETA_RESEARCH_POINT_ID = "BetaResearchPoint";

		// Token: 0x04005BD8 RID: 23512
		public TechItem betaResearchPoint;

		// Token: 0x04005BD9 RID: 23513
		public const string GAMMA_RESEARCH_POINT_ID = "GammaResearchPoint";

		// Token: 0x04005BDA RID: 23514
		public TechItem gammaResearchPoint;

		// Token: 0x04005BDB RID: 23515
		public const string DELTA_RESEARCH_POINT_ID = "DeltaResearchPoint";

		// Token: 0x04005BDC RID: 23516
		public TechItem deltaResearchPoint;

		// Token: 0x04005BDD RID: 23517
		public const string ORBITAL_RESEARCH_POINT_ID = "OrbitalResearchPoint";

		// Token: 0x04005BDE RID: 23518
		public TechItem orbitalResearchPoint;

		// Token: 0x04005BDF RID: 23519
		public const string CONVEYOR_OVERLAY_ID = "ConveyorOverlay";

		// Token: 0x04005BE0 RID: 23520
		public TechItem conveyorOverlay;
	}
}
