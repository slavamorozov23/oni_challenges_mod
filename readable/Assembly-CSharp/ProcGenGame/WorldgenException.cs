using System;

namespace ProcGenGame
{
	// Token: 0x02000EE6 RID: 3814
	public class WorldgenException : Exception
	{
		// Token: 0x06007A3F RID: 31295 RVA: 0x002F5ADD File Offset: 0x002F3CDD
		public WorldgenException(string message, string userMessage) : base(message)
		{
			this.userMessage = userMessage;
		}

		// Token: 0x04005564 RID: 21860
		public readonly string userMessage;
	}
}
