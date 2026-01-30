using System;

namespace Klei.Actions
{
	// Token: 0x0200106B RID: 4203
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class ActionTypeAttribute : Attribute
	{
		// Token: 0x060081FD RID: 33277 RVA: 0x00341378 File Offset: 0x0033F578
		public ActionTypeAttribute(string groupName, string typeName, bool generateConfig = true)
		{
			this.TypeName = typeName;
			this.GroupName = groupName;
			this.GenerateConfig = generateConfig;
		}

		// Token: 0x060081FE RID: 33278 RVA: 0x00341398 File Offset: 0x0033F598
		public static bool operator ==(ActionTypeAttribute lhs, ActionTypeAttribute rhs)
		{
			bool flag = object.Equals(lhs, null);
			bool flag2 = object.Equals(rhs, null);
			if (flag || flag2)
			{
				return flag == flag2;
			}
			return lhs.TypeName == rhs.TypeName && lhs.GroupName == rhs.GroupName;
		}

		// Token: 0x060081FF RID: 33279 RVA: 0x003413E5 File Offset: 0x0033F5E5
		public static bool operator !=(ActionTypeAttribute lhs, ActionTypeAttribute rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06008200 RID: 33280 RVA: 0x003413F1 File Offset: 0x0033F5F1
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x06008201 RID: 33281 RVA: 0x003413FA File Offset: 0x0033F5FA
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04006251 RID: 25169
		public readonly string TypeName;

		// Token: 0x04006252 RID: 25170
		public readonly string GroupName;

		// Token: 0x04006253 RID: 25171
		public readonly bool GenerateConfig;
	}
}
