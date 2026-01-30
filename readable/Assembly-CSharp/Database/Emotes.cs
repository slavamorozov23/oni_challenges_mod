using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02000F3A RID: 3898
	public class Emotes : ResourceSet<Resource>
	{
		// Token: 0x06007C63 RID: 31843 RVA: 0x00310D9B File Offset: 0x0030EF9B
		public Emotes(ResourceSet parent) : base("Emotes", parent)
		{
			this.Minion = new Emotes.MinionEmotes(this);
			this.Critter = new Emotes.CritterEmotes(this);
		}

		// Token: 0x06007C64 RID: 31844 RVA: 0x00310DC4 File Offset: 0x0030EFC4
		public void ResetProblematicReferences()
		{
			for (int i = 0; i < this.Minion.resources.Count; i++)
			{
				Emote emote = this.Minion.resources[i];
				for (int j = 0; j < emote.StepCount; j++)
				{
					emote[j].UnregisterAllCallbacks();
				}
			}
			for (int k = 0; k < this.Critter.resources.Count; k++)
			{
				Emote emote2 = this.Critter.resources[k];
				for (int l = 0; l < emote2.StepCount; l++)
				{
					emote2[l].UnregisterAllCallbacks();
				}
			}
		}

		// Token: 0x040059FC RID: 23036
		public Emotes.MinionEmotes Minion;

		// Token: 0x040059FD RID: 23037
		public Emotes.CritterEmotes Critter;

		// Token: 0x0200219B RID: 8603
		public class MinionEmotes : ResourceSet<Emote>
		{
			// Token: 0x0600BD86 RID: 48518 RVA: 0x00405251 File Offset: 0x00403451
			public MinionEmotes(ResourceSet parent) : base("Minion", parent)
			{
				this.InitializeCelebrations();
				this.InitializePhysicalStatus();
				this.InitializeEmotionalStatus();
				this.InitializeGreetings();
			}

			// Token: 0x0600BD87 RID: 48519 RVA: 0x00405278 File Offset: 0x00403478
			public void InitializeCelebrations()
			{
				this.ClapCheer = new Emote(this, "ClapCheer", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "clapcheer_pre"
					},
					new EmoteStep
					{
						anim = "clapcheer_loop"
					},
					new EmoteStep
					{
						anim = "clapcheer_pst"
					}
				}, "anim_clapcheer_kanim");
				this.Cheer = new Emote(this, "Cheer", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "cheer_pre"
					},
					new EmoteStep
					{
						anim = "cheer_loop"
					},
					new EmoteStep
					{
						anim = "cheer_pst"
					}
				}, "anim_cheer_kanim");
				this.ProductiveCheer = new Emote(this, "Productive Cheer", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "productive"
					}
				}, "anim_productive_kanim");
				this.ResearchComplete = new Emote(this, "ResearchComplete", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_research_complete_kanim");
				this.ThumbsUp = new Emote(this, "ThumbsUp", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_thumbsup_kanim");
			}

			// Token: 0x0600BD88 RID: 48520 RVA: 0x004053B8 File Offset: 0x004035B8
			private void InitializePhysicalStatus()
			{
				this.CloseCall_Fall = new Emote(this, "Near Fall", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_floor_missing_kanim");
				this.Cold = new Emote(this, "Cold", Emotes.MinionEmotes.DEFAULT_IDLE_STEPS, "anim_idle_cold_kanim");
				this.Cough = new Emote(this, "Cough", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_slimelungcough_kanim");
				this.Cough_Small = new Emote(this, "Small Cough", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_small"
					}
				}, "anim_slimelungcough_kanim");
				this.FoodPoisoning = new Emote(this, "Food Poisoning", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_contaminated_food_kanim");
				this.Hot = new Emote(this, "Hot", Emotes.MinionEmotes.DEFAULT_IDLE_STEPS, "anim_idle_hot_kanim");
				this.IritatedEyes = new Emote(this, "Irritated Eyes", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "irritated_eyes"
					}
				}, "anim_irritated_eyes_kanim");
				this.MorningStretch = new Emote(this, "Morning Stretch", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_morning_stretch_kanim");
				this.Radiation_Glare = new Emote(this, "Radiation Glare", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_radiation_glare"
					}
				}, "anim_react_radiation_kanim");
				this.Radiation_Itch = new Emote(this, "Radiation Itch", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_radiation_itch"
					}
				}, "anim_react_radiation_kanim");
				this.Sick = new Emote(this, "Sick", Emotes.MinionEmotes.DEFAULT_IDLE_STEPS, "anim_idle_sick_kanim");
				this.Sneeze = new Emote(this, "Sneeze", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "sneeze"
					},
					new EmoteStep
					{
						anim = "sneeze_pst"
					}
				}, "anim_sneeze_kanim");
				this.WaterDamage = new Emote(this, "WaterDamage", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "zapped"
					}
				}, "anim_bionic_kanim");
				this.GrindingGears = new Emote(this, "GrindingGears", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react"
					}
				}, "anim_bionic_react_grinding_gears_kanim");
				this.Sneeze_Short = new Emote(this, "Short Sneeze", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "sneeze_short"
					},
					new EmoteStep
					{
						anim = "sneeze_short_pst"
					}
				}, "anim_sneeze_kanim");
			}

			// Token: 0x0600BD89 RID: 48521 RVA: 0x00405654 File Offset: 0x00403854
			private void InitializeEmotionalStatus()
			{
				this.Concern = new Emote(this, "Concern", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_concern_kanim");
				this.Cringe = new Emote(this, "Cringe", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "cringe_pre"
					},
					new EmoteStep
					{
						anim = "cringe_loop"
					},
					new EmoteStep
					{
						anim = "cringe_pst"
					}
				}, "anim_cringe_kanim");
				this.Disappointed = new Emote(this, "Disappointed", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_disappointed_kanim");
				this.Shock = new Emote(this, "Shock", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_shock_kanim");
				this.Sing = new Emote(this, "Sing", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_singer_kanim");
			}

			// Token: 0x0600BD8A RID: 48522 RVA: 0x00405734 File Offset: 0x00403934
			private void InitializeGreetings()
			{
				this.FingerGuns = new Emote(this, "Finger Guns", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_fingerguns_kanim");
				this.Wave = new Emote(this, "Wave", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_wave_kanim");
				this.Wave_Shy = new Emote(this, "Shy Wave", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_wave_shy_kanim");
			}

			// Token: 0x04009A99 RID: 39577
			private static EmoteStep[] DEFAULT_STEPS = new EmoteStep[]
			{
				new EmoteStep
				{
					anim = "react"
				}
			};

			// Token: 0x04009A9A RID: 39578
			private static EmoteStep[] DEFAULT_IDLE_STEPS = new EmoteStep[]
			{
				new EmoteStep
				{
					anim = "idle_pre"
				},
				new EmoteStep
				{
					anim = "idle_default"
				},
				new EmoteStep
				{
					anim = "idle_pst"
				}
			};

			// Token: 0x04009A9B RID: 39579
			public Emote ClapCheer;

			// Token: 0x04009A9C RID: 39580
			public Emote Cheer;

			// Token: 0x04009A9D RID: 39581
			public Emote ProductiveCheer;

			// Token: 0x04009A9E RID: 39582
			public Emote ResearchComplete;

			// Token: 0x04009A9F RID: 39583
			public Emote ThumbsUp;

			// Token: 0x04009AA0 RID: 39584
			public Emote CloseCall_Fall;

			// Token: 0x04009AA1 RID: 39585
			public Emote Cold;

			// Token: 0x04009AA2 RID: 39586
			public Emote Cough;

			// Token: 0x04009AA3 RID: 39587
			public Emote Cough_Small;

			// Token: 0x04009AA4 RID: 39588
			public Emote FoodPoisoning;

			// Token: 0x04009AA5 RID: 39589
			public Emote Hot;

			// Token: 0x04009AA6 RID: 39590
			public Emote IritatedEyes;

			// Token: 0x04009AA7 RID: 39591
			public Emote MorningStretch;

			// Token: 0x04009AA8 RID: 39592
			public Emote Radiation_Glare;

			// Token: 0x04009AA9 RID: 39593
			public Emote Radiation_Itch;

			// Token: 0x04009AAA RID: 39594
			public Emote Sick;

			// Token: 0x04009AAB RID: 39595
			public Emote Sneeze;

			// Token: 0x04009AAC RID: 39596
			public Emote WaterDamage;

			// Token: 0x04009AAD RID: 39597
			public Emote Sneeze_Short;

			// Token: 0x04009AAE RID: 39598
			public Emote GrindingGears;

			// Token: 0x04009AAF RID: 39599
			public Emote Concern;

			// Token: 0x04009AB0 RID: 39600
			public Emote Cringe;

			// Token: 0x04009AB1 RID: 39601
			public Emote Disappointed;

			// Token: 0x04009AB2 RID: 39602
			public Emote Shock;

			// Token: 0x04009AB3 RID: 39603
			public Emote Sing;

			// Token: 0x04009AB4 RID: 39604
			public Emote FingerGuns;

			// Token: 0x04009AB5 RID: 39605
			public Emote Wave;

			// Token: 0x04009AB6 RID: 39606
			public Emote Wave_Shy;
		}

		// Token: 0x0200219C RID: 8604
		public class CritterEmotes : ResourceSet<Emote>
		{
			// Token: 0x0600BD8C RID: 48524 RVA: 0x00405817 File Offset: 0x00403A17
			public CritterEmotes(ResourceSet parent) : base("Critter", parent)
			{
				this.InitializeEmotes();
			}

			// Token: 0x0600BD8D RID: 48525 RVA: 0x0040582C File Offset: 0x00403A2C
			private void InitializeEmotes()
			{
				this.Positive = new Emote(this, "Positive", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_pos"
					}
				}, null);
				this.Negative = new Emote(this, "Negative", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_neg"
					}
				}, null);
				this.Roar = new Emote(this, "Roar", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "roar"
					}
				}, null);
				this.RaptorSignal = new Emote(this, "Signal", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "signal"
					}
				}, null);
			}

			// Token: 0x04009AB7 RID: 39607
			public Emote Positive;

			// Token: 0x04009AB8 RID: 39608
			public Emote Negative;

			// Token: 0x04009AB9 RID: 39609
			public Emote Roar;

			// Token: 0x04009ABA RID: 39610
			public Emote RaptorSignal;
		}
	}
}
