using System;
using UnityEngine;

// Token: 0x0200095C RID: 2396
public class LogCatcher : ILogHandler
{
	// Token: 0x060042F6 RID: 17142 RVA: 0x0017A818 File Offset: 0x00178A18
	public LogCatcher(ILogHandler old)
	{
		this.def = old;
	}

	// Token: 0x060042F7 RID: 17143 RVA: 0x0017A828 File Offset: 0x00178A28
	void ILogHandler.LogException(Exception exception, UnityEngine.Object context)
	{
		string a = exception.ToString();
		string a2 = (context != null) ? context.ToString() : null;
		if (a == "False" || a2 == "False")
		{
			global::Debug.LogError("False only message!");
		}
		this.def.LogException(exception, context);
	}

	// Token: 0x060042F8 RID: 17144 RVA: 0x0017A87E File Offset: 0x00178A7E
	void ILogHandler.LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
	{
		if (string.Format(format, args) == "False")
		{
			global::Debug.LogError("False only message!");
		}
		this.def.LogFormat(logType, context, format, args);
	}

	// Token: 0x04002A23 RID: 10787
	private ILogHandler def;
}
