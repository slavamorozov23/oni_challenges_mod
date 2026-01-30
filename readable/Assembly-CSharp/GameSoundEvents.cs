using System;

// Token: 0x02000963 RID: 2403
public static class GameSoundEvents
{
	// Token: 0x04002CAA RID: 11434
	public static GameSoundEvents.Event BatteryFull = new GameSoundEvents.Event("game_triggered.battery_full");

	// Token: 0x04002CAB RID: 11435
	public static GameSoundEvents.Event BatteryWarning = new GameSoundEvents.Event("game_triggered.battery_warning");

	// Token: 0x04002CAC RID: 11436
	public static GameSoundEvents.Event BatteryDischarged = new GameSoundEvents.Event("game_triggered.battery_drained");

	// Token: 0x02001976 RID: 6518
	public class Event
	{
		// Token: 0x0600A26B RID: 41579 RVA: 0x003AE9EF File Offset: 0x003ACBEF
		public Event(string name)
		{
			this.Name = name;
		}

		// Token: 0x04007DDF RID: 32223
		public HashedString Name;
	}
}
