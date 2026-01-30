using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02000F4D RID: 3917
	public class Personalities : ResourceSet<Personality>
	{
		// Token: 0x06007CA7 RID: 31911 RVA: 0x00315128 File Offset: 0x00313328
		public Personalities()
		{
			foreach (Personalities.PersonalityInfo personalityInfo in AsyncLoadManager<IGlobalAsyncLoader>.AsyncLoader<Personalities.PersonalityLoader>.Get().entries)
			{
				if (string.IsNullOrEmpty(personalityInfo.RequiredDlcId) || DlcManager.IsContentSubscribed(personalityInfo.RequiredDlcId) || this.IsCosmeticPersonality(personalityInfo.RequiredDlcId))
				{
					Personality personality = new Personality(personalityInfo.Name.ToUpper(), Strings.Get(string.Format("STRINGS.DUPLICANTS.PERSONALITIES.{0}.NAME", personalityInfo.Name.ToUpper())), personalityInfo.Gender.ToUpper(), personalityInfo.PersonalityType, personalityInfo.StressTrait, personalityInfo.JoyTrait, personalityInfo.StickerType, personalityInfo.CongenitalTrait, personalityInfo.HeadShape, personalityInfo.Mouth, personalityInfo.Neck, personalityInfo.Eyes, personalityInfo.Hair, personalityInfo.Body, personalityInfo.Belt, personalityInfo.Cuff, personalityInfo.Foot, personalityInfo.Hand, personalityInfo.Pelvis, personalityInfo.Leg, personalityInfo.Leg_Skin, personalityInfo.Arm_Skin, Strings.Get(string.Format("STRINGS.DUPLICANTS.PERSONALITIES.{0}.DESC", personalityInfo.Name.ToUpper())), personalityInfo.ValidStarter, personalityInfo.Grave, personalityInfo.Model, personalityInfo.SpeechMouth);
					personality.requiredDlcId = personalityInfo.RequiredDlcId;
					if (!DlcManager.IsContentSubscribed(personalityInfo.RequiredDlcId) && this.IsCosmeticPersonality(personalityInfo.RequiredDlcId))
					{
						personality.Disabled = true;
					}
					base.Add(personality);
				}
			}
		}

		// Token: 0x06007CA8 RID: 31912 RVA: 0x003152A8 File Offset: 0x003134A8
		private void AddTrait(Personality personality, string trait_name)
		{
			Trait trait = Db.Get().traits.TryGet(trait_name);
			if (trait != null)
			{
				personality.AddTrait(trait);
			}
		}

		// Token: 0x06007CA9 RID: 31913 RVA: 0x003152D0 File Offset: 0x003134D0
		private void SetAttribute(Personality personality, string attribute_name, int value)
		{
			Klei.AI.Attribute attribute = Db.Get().Attributes.TryGet(attribute_name);
			if (attribute == null)
			{
				Debug.LogWarning("Attribute does not exist: " + attribute_name);
				return;
			}
			personality.SetAttribute(attribute, value);
		}

		// Token: 0x06007CAA RID: 31914 RVA: 0x0031530A File Offset: 0x0031350A
		public List<Personality> GetStartingPersonalities()
		{
			return this.resources.FindAll((Personality x) => x.startingMinion);
		}

		// Token: 0x06007CAB RID: 31915 RVA: 0x00315338 File Offset: 0x00313538
		public List<Personality> GetAll(bool onlyEnabledMinions, bool onlyStartingMinions)
		{
			return this.resources.FindAll((Personality personality) => (!onlyStartingMinions || personality.startingMinion) && (!onlyEnabledMinions || !personality.Disabled) && (!(Game.Instance != null) || Game.IsDlcActiveForCurrentSave(personality.requiredDlcId)));
		}

		// Token: 0x06007CAC RID: 31916 RVA: 0x00315370 File Offset: 0x00313570
		public Personality GetRandom(bool onlyEnabledMinions, bool onlyStartingMinions)
		{
			return this.GetAll(onlyEnabledMinions, onlyStartingMinions).GetRandom<Personality>();
		}

		// Token: 0x06007CAD RID: 31917 RVA: 0x00315380 File Offset: 0x00313580
		public Personality GetRandom(Tag model, bool onlyEnabledMinions, bool onlyStartingMinions)
		{
			return this.GetAll(onlyEnabledMinions, onlyStartingMinions).FindAll((Personality personality) => personality.model == model || model == null).GetRandom<Personality>();
		}

		// Token: 0x06007CAE RID: 31918 RVA: 0x003153B8 File Offset: 0x003135B8
		public Personality GetRandom(List<Tag> models, bool onlyEnabledMinions, bool onlyStartingMinions)
		{
			return this.GetAll(onlyEnabledMinions, onlyStartingMinions).FindAll((Personality personality) => models.Contains(personality.model)).GetRandom<Personality>();
		}

		// Token: 0x06007CAF RID: 31919 RVA: 0x003153F0 File Offset: 0x003135F0
		public Personality GetPersonalityFromNameStringKey(string name_string_key)
		{
			foreach (Personality personality in Db.Get().Personalities.resources)
			{
				if (personality.nameStringKey.Equals(name_string_key, StringComparison.CurrentCultureIgnoreCase))
				{
					return personality;
				}
			}
			return null;
		}

		// Token: 0x06007CB0 RID: 31920 RVA: 0x0031545C File Offset: 0x0031365C
		private bool IsCosmeticPersonality(string required_dlc_id)
		{
			return !string.IsNullOrEmpty(required_dlc_id) && DlcManager.CONTENT_ONLY_DLC_IDS.Contains(required_dlc_id);
		}

		// Token: 0x020021A3 RID: 8611
		public class PersonalityLoader : AsyncCsvLoader<Personalities.PersonalityLoader, Personalities.PersonalityInfo>
		{
			// Token: 0x0600BDBA RID: 48570 RVA: 0x00406313 File Offset: 0x00404513
			public PersonalityLoader() : base(Assets.instance.personalitiesFile)
			{
			}

			// Token: 0x0600BDBB RID: 48571 RVA: 0x00406325 File Offset: 0x00404525
			public override void Run()
			{
				base.Run();
			}
		}

		// Token: 0x020021A4 RID: 8612
		public class PersonalityInfo : Resource
		{
			// Token: 0x04009AE9 RID: 39657
			public int HeadShape;

			// Token: 0x04009AEA RID: 39658
			public int Mouth;

			// Token: 0x04009AEB RID: 39659
			public int Neck;

			// Token: 0x04009AEC RID: 39660
			public int Eyes;

			// Token: 0x04009AED RID: 39661
			public int Hair;

			// Token: 0x04009AEE RID: 39662
			public int Body;

			// Token: 0x04009AEF RID: 39663
			public int Belt;

			// Token: 0x04009AF0 RID: 39664
			public int Cuff;

			// Token: 0x04009AF1 RID: 39665
			public int Foot;

			// Token: 0x04009AF2 RID: 39666
			public int Hand;

			// Token: 0x04009AF3 RID: 39667
			public int Pelvis;

			// Token: 0x04009AF4 RID: 39668
			public int Leg;

			// Token: 0x04009AF5 RID: 39669
			public int Arm_Skin;

			// Token: 0x04009AF6 RID: 39670
			public int Leg_Skin;

			// Token: 0x04009AF7 RID: 39671
			public int SpeechMouth;

			// Token: 0x04009AF8 RID: 39672
			public string Gender;

			// Token: 0x04009AF9 RID: 39673
			public string PersonalityType;

			// Token: 0x04009AFA RID: 39674
			public string StressTrait;

			// Token: 0x04009AFB RID: 39675
			public string JoyTrait;

			// Token: 0x04009AFC RID: 39676
			public string StickerType;

			// Token: 0x04009AFD RID: 39677
			public string CongenitalTrait;

			// Token: 0x04009AFE RID: 39678
			public string Design;

			// Token: 0x04009AFF RID: 39679
			public bool ValidStarter;

			// Token: 0x04009B00 RID: 39680
			public string Grave;

			// Token: 0x04009B01 RID: 39681
			public string Model;

			// Token: 0x04009B02 RID: 39682
			public string RequiredDlcId;
		}
	}
}
