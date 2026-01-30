using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;

// Token: 0x02000478 RID: 1144
[DebuggerDisplay("has_value={hasValue} {value}")]
[Serializable]
public readonly struct Option<T> : IEquatable<Option<T>>, IEquatable<T>
{
	// Token: 0x06001803 RID: 6147 RVA: 0x00087895 File Offset: 0x00085A95
	public Option(T value)
	{
		this.value = value;
		this.hasValue = true;
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x06001804 RID: 6148 RVA: 0x000878A5 File Offset: 0x00085AA5
	public bool HasValue
	{
		get
		{
			return this.hasValue;
		}
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x06001805 RID: 6149 RVA: 0x000878AD File Offset: 0x00085AAD
	public T Value
	{
		get
		{
			return this.Unwrap();
		}
	}

	// Token: 0x06001806 RID: 6150 RVA: 0x000878B5 File Offset: 0x00085AB5
	public T Unwrap()
	{
		if (!this.hasValue)
		{
			throw new Exception("Tried to get a value for a Option<" + typeof(T).FullName + ">, but hasValue is false");
		}
		return this.value;
	}

	// Token: 0x06001807 RID: 6151 RVA: 0x000878E9 File Offset: 0x00085AE9
	public T UnwrapOr(T fallback_value, string warn_on_fallback = null)
	{
		if (!this.hasValue)
		{
			if (warn_on_fallback != null)
			{
				DebugUtil.DevAssert(false, "Failed to unwrap a Option<" + typeof(T).FullName + ">: " + warn_on_fallback, null);
			}
			return fallback_value;
		}
		return this.value;
	}

	// Token: 0x06001808 RID: 6152 RVA: 0x00087924 File Offset: 0x00085B24
	public T UnwrapOrElse(Func<T> get_fallback_value_fn, string warn_on_fallback = null)
	{
		if (!this.hasValue)
		{
			if (warn_on_fallback != null)
			{
				DebugUtil.DevAssert(false, "Failed to unwrap a Option<" + typeof(T).FullName + ">: " + warn_on_fallback, null);
			}
			return get_fallback_value_fn();
		}
		return this.value;
	}

	// Token: 0x06001809 RID: 6153 RVA: 0x00087964 File Offset: 0x00085B64
	public T UnwrapOrDefault()
	{
		if (!this.hasValue)
		{
			return default(T);
		}
		return this.value;
	}

	// Token: 0x0600180A RID: 6154 RVA: 0x00087989 File Offset: 0x00085B89
	public T Expect(string msg_on_fail)
	{
		if (!this.hasValue)
		{
			throw new Exception(msg_on_fail);
		}
		return this.value;
	}

	// Token: 0x0600180B RID: 6155 RVA: 0x000879A0 File Offset: 0x00085BA0
	public bool IsSome()
	{
		return this.hasValue;
	}

	// Token: 0x0600180C RID: 6156 RVA: 0x000879A8 File Offset: 0x00085BA8
	public bool IsNone()
	{
		return !this.hasValue;
	}

	// Token: 0x0600180D RID: 6157 RVA: 0x000879B3 File Offset: 0x00085BB3
	public Option<U> AndThen<U>(Func<T, U> fn)
	{
		if (this.IsNone())
		{
			return Option.None;
		}
		return Option.Maybe<U>(fn(this.value));
	}

	// Token: 0x0600180E RID: 6158 RVA: 0x000879D9 File Offset: 0x00085BD9
	public Option<U> AndThen<U>(Func<T, Option<U>> fn)
	{
		if (this.IsNone())
		{
			return Option.None;
		}
		return fn(this.value);
	}

	// Token: 0x0600180F RID: 6159 RVA: 0x000879FA File Offset: 0x00085BFA
	public static implicit operator Option<T>(T value)
	{
		return Option.Maybe<T>(value);
	}

	// Token: 0x06001810 RID: 6160 RVA: 0x00087A02 File Offset: 0x00085C02
	public static explicit operator T(Option<T> option)
	{
		return option.Unwrap();
	}

	// Token: 0x06001811 RID: 6161 RVA: 0x00087A0C File Offset: 0x00085C0C
	public static implicit operator Option<T>(Option.Internal.Value_None value)
	{
		return default(Option<T>);
	}

	// Token: 0x06001812 RID: 6162 RVA: 0x00087A22 File Offset: 0x00085C22
	public static implicit operator Option.Internal.Value_HasValue(Option<T> value)
	{
		return new Option.Internal.Value_HasValue(value.hasValue);
	}

	// Token: 0x06001813 RID: 6163 RVA: 0x00087A2F File Offset: 0x00085C2F
	public void Deconstruct(out bool hasValue, out T value)
	{
		hasValue = this.hasValue;
		value = this.value;
	}

	// Token: 0x06001814 RID: 6164 RVA: 0x00087A45 File Offset: 0x00085C45
	public bool Equals(Option<T> other)
	{
		return EqualityComparer<bool>.Default.Equals(this.hasValue, other.hasValue) && EqualityComparer<T>.Default.Equals(this.value, other.value);
	}

	// Token: 0x06001815 RID: 6165 RVA: 0x00087A78 File Offset: 0x00085C78
	public override bool Equals(object obj)
	{
		if (obj is Option<T>)
		{
			Option<T> other = (Option<T>)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x06001816 RID: 6166 RVA: 0x00087A9D File Offset: 0x00085C9D
	public static bool operator ==(Option<T> lhs, Option<T> rhs)
	{
		return lhs.Equals(rhs);
	}

	// Token: 0x06001817 RID: 6167 RVA: 0x00087AA7 File Offset: 0x00085CA7
	public static bool operator !=(Option<T> lhs, Option<T> rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x06001818 RID: 6168 RVA: 0x00087AB4 File Offset: 0x00085CB4
	public override int GetHashCode()
	{
		return (-363764631 * -1521134295 + this.hasValue.GetHashCode()) * -1521134295 + EqualityComparer<T>.Default.GetHashCode(this.value);
	}

	// Token: 0x06001819 RID: 6169 RVA: 0x00087AF2 File Offset: 0x00085CF2
	public override string ToString()
	{
		if (!this.hasValue)
		{
			return "None";
		}
		return string.Format("{0}", this.value);
	}

	// Token: 0x0600181A RID: 6170 RVA: 0x00087B17 File Offset: 0x00085D17
	public static bool operator ==(Option<T> lhs, T rhs)
	{
		return lhs.Equals(rhs);
	}

	// Token: 0x0600181B RID: 6171 RVA: 0x00087B21 File Offset: 0x00085D21
	public static bool operator !=(Option<T> lhs, T rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x0600181C RID: 6172 RVA: 0x00087B2D File Offset: 0x00085D2D
	public static bool operator ==(T lhs, Option<T> rhs)
	{
		return rhs.Equals(lhs);
	}

	// Token: 0x0600181D RID: 6173 RVA: 0x00087B37 File Offset: 0x00085D37
	public static bool operator !=(T lhs, Option<T> rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x0600181E RID: 6174 RVA: 0x00087B43 File Offset: 0x00085D43
	public bool Equals(T other)
	{
		return this.HasValue && EqualityComparer<T>.Default.Equals(this.value, other);
	}

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x0600181F RID: 6175 RVA: 0x00087B60 File Offset: 0x00085D60
	public static Option<T> None
	{
		get
		{
			return default(Option<T>);
		}
	}

	// Token: 0x04000E23 RID: 3619
	[Serialize]
	private readonly bool hasValue;

	// Token: 0x04000E24 RID: 3620
	[Serialize]
	private readonly T value;
}
