using System;
using System.Runtime.InteropServices;
using System.Diagnostics.Contracts;
using Unity.Networking;
using Unity.Networking.Transport;

/// <summary>
/// Utiltity struct which its only purpose is to create a netcode, burst compatible version of the System.guid
/// This class avoids the need to cast to byte array to write to the data stream whichs is incompatible with burst,
/// Allow to implement methods to write to the dataSteam accessing the internal ints and bytes
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[Serializable]
public struct SandboxGuid : IComparable
	, IComparable<SandboxGuid>, IEquatable<SandboxGuid>
{
	public static readonly SandboxGuid Empty = new SandboxGuid();

	////////////////////////////////////////////////////////////////////////////////
	//  Member variables
	////////////////////////////////////////////////////////////////////////////////
	private int _a;
	private short _b;
	private short _c;
	private byte _d;
	private byte _e;
	private byte _f;
	private byte _g;
	private byte _h;
	private byte _i;
	private byte _j;
	private byte _k;

	////////////////////////////////////////////////////////////////////////////////
	//  Constructors
	////////////////////////////////////////////////////////////////////////////////

	// Creates a new SandboxGuid from an array of bytes.
	//
	public SandboxGuid(byte[] b)
	{
		if (b == null)
			throw new ArgumentNullException("b");
		if (b.Length != 16)
			throw new ArgumentException(("Arg_GuidArrayCtor"));
		Contract.EndContractBlock();

		_a = ((int) b[3] << 24) | ((int) b[2] << 16) | ((int) b[1] << 8) | b[0];
		_b = (short) (((int) b[5] << 8) | b[4]);
		_c = (short) (((int) b[7] << 8) | b[6]);
		_d = b[8];
		_e = b[9];
		_f = b[10];
		_g = b[11];
		_h = b[12];
		_i = b[13];
		_j = b[14];
		_k = b[15];
	}

	public static implicit operator SandboxGuid(System.Guid value)
	{
		return new SandboxGuid(value.ToByteArray());
	}

	public SandboxGuid Parse(string str)
	{
		var stringArray = str.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
		byte[] g = new byte[16];
		for (int i = 0; i < 16; ++i)
		{
			g[i] = byte.Parse(stringArray[i]);
		}

		return new SandboxGuid(g);
	}

	public void WriteDataStream(ref DataStreamWriter writer)
	{
		WriteInt(ref writer, _a);
		WriteShort(ref writer, _b);
		WriteShort(ref writer, _c);
		writer.WriteByte(_d);
		writer.WriteByte(_e);
		writer.WriteByte(_f);
		writer.WriteByte(_g);
		writer.WriteByte(_h);
		writer.WriteByte(_i);
		writer.WriteByte(_j);
		writer.WriteByte(_k);
	}

	public void ReadDataStream(ref DataStreamReader reader)
	{
		_a = ReadInt(ref reader);
		_b = ReadShort(ref reader);
		_c = ReadShort(ref reader);
		_d = reader.ReadByte();
		_e = reader.ReadByte();
		_f = reader.ReadByte();
		_g = reader.ReadByte();
		_h = reader.ReadByte();
		_i = reader.ReadByte();
		_j = reader.ReadByte();
		_k = reader.ReadByte();
	}

	private void WriteInt(ref DataStreamWriter writer, int value)
	{
		writer.WriteByte((byte) value);
		writer.WriteByte((byte) (value >> 8));
		writer.WriteByte((byte) (value >> 16));
		writer.WriteByte((byte) (value >> 24));
	}

	private void WriteShort(ref DataStreamWriter writer, short value)
	{
		writer.WriteByte((byte) value);
		writer.WriteByte((byte) (value >> 8));
	}

	private int ReadInt(ref DataStreamReader reader)
	{
		byte a = reader.ReadByte();
		byte b = reader.ReadByte();
		byte c = reader.ReadByte();
		byte d = reader.ReadByte();

		return ((int) d << 24) | ((int) c << 16) | ((int) b << 8) | a;
	}

	private short ReadShort(ref DataStreamReader reader)
	{
		byte a = reader.ReadByte();
		byte b = reader.ReadByte();

		return (short) (((int) b << 8) | a);
	}

	// Returns true if and only if the SandboxGuid represented
	//  by o is the same as this instance.
	public override bool Equals(Object o)
	{
		SandboxGuid g;
		// Check that o is a SandboxGuid first
		if (o == null || !(o is SandboxGuid))
			return false;
		else g = (SandboxGuid) o;

		// Now compare each of the elements
		if (g._a != _a)
			return false;
		if (g._b != _b)
			return false;
		if (g._c != _c)
			return false;
		if (g._d != _d)
			return false;
		if (g._e != _e)
			return false;
		if (g._f != _f)
			return false;
		if (g._g != _g)
			return false;
		if (g._h != _h)
			return false;
		if (g._i != _i)
			return false;
		if (g._j != _j)
			return false;
		if (g._k != _k)
			return false;

		return true;
	}

	public bool Equals(SandboxGuid g)
	{
		// Now compare each of the elements
		if (g._a != _a)
			return false;
		if (g._b != _b)
			return false;
		if (g._c != _c)
			return false;
		if (g._d != _d)
			return false;
		if (g._e != _e)
			return false;
		if (g._f != _f)
			return false;
		if (g._g != _g)
			return false;
		if (g._h != _h)
			return false;
		if (g._i != _i)
			return false;
		if (g._j != _j)
			return false;
		if (g._k != _k)
			return false;

		return true;
	}

	private int GetResult(uint me, uint them)
	{
		if (me < them)
		{
			return -1;
		}

		return 1;
	}

	public int CompareTo(Object value)
	{
		if (value == null)
		{
			return 1;
		}

		if (!(value is SandboxGuid))
		{
			throw new ArgumentException(("Arg_MustBeGuid"));
		}

		SandboxGuid g = (SandboxGuid) value;

		if (g._a != this._a)
		{
			return GetResult((uint) this._a, (uint) g._a);
		}

		if (g._b != this._b)
		{
			return GetResult((uint) this._b, (uint) g._b);
		}

		if (g._c != this._c)
		{
			return GetResult((uint) this._c, (uint) g._c);
		}

		if (g._d != this._d)
		{
			return GetResult((uint) this._d, (uint) g._d);
		}

		if (g._e != this._e)
		{
			return GetResult((uint) this._e, (uint) g._e);
		}

		if (g._f != this._f)
		{
			return GetResult((uint) this._f, (uint) g._f);
		}

		if (g._g != this._g)
		{
			return GetResult((uint) this._g, (uint) g._g);
		}

		if (g._h != this._h)
		{
			return GetResult((uint) this._h, (uint) g._h);
		}

		if (g._i != this._i)
		{
			return GetResult((uint) this._i, (uint) g._i);
		}

		if (g._j != this._j)
		{
			return GetResult((uint) this._j, (uint) g._j);
		}

		if (g._k != this._k)
		{
			return GetResult((uint) this._k, (uint) g._k);
		}

		return 0;
	}


	public int CompareTo(SandboxGuid value)
	{
		if (value._a != this._a)
		{
			return GetResult((uint) this._a, (uint) value._a);
		}

		if (value._b != this._b)
		{
			return GetResult((uint) this._b, (uint) value._b);
		}

		if (value._c != this._c)
		{
			return GetResult((uint) this._c, (uint) value._c);
		}

		if (value._d != this._d)
		{
			return GetResult((uint) this._d, (uint) value._d);
		}

		if (value._e != this._e)
		{
			return GetResult((uint) this._e, (uint) value._e);
		}

		if (value._f != this._f)
		{
			return GetResult((uint) this._f, (uint) value._f);
		}

		if (value._g != this._g)
		{
			return GetResult((uint) this._g, (uint) value._g);
		}

		if (value._h != this._h)
		{
			return GetResult((uint) this._h, (uint) value._h);
		}

		if (value._i != this._i)
		{
			return GetResult((uint) this._i, (uint) value._i);
		}

		if (value._j != this._j)
		{
			return GetResult((uint) this._j, (uint) value._j);
		}

		if (value._k != this._k)
		{
			return GetResult((uint) this._k, (uint) value._k);
		}

		return 0;
	}


	public static bool operator ==(SandboxGuid a, SandboxGuid b)
	{
		// Now compare each of the elements
		if (a._a != b._a)
			return false;
		if (a._b != b._b)
			return false;
		if (a._c != b._c)
			return false;
		if (a._d != b._d)
			return false;
		if (a._e != b._e)
			return false;
		if (a._f != b._f)
			return false;
		if (a._g != b._g)
			return false;
		if (a._h != b._h)
			return false;
		if (a._i != b._i)
			return false;
		if (a._j != b._j)
			return false;
		if (a._k != b._k)
			return false;

		return true;
	}

	public static bool operator !=(SandboxGuid a, SandboxGuid b)
	{
		return !(a == b);
	}

	public override int GetHashCode()
	{
		return _a ^ (((int) _b << 16) | (int) (ushort) _c) ^ (((int) _f << 24) | _k);
	}

	// Returns an unsigned byte array containing the SandboxGuid.
	public byte[] ToByteArray()
	{
		byte[] g = new byte[16];

		g[0] = (byte) (_a);
		g[1] = (byte) (_a >> 8);
		g[2] = (byte) (_a >> 16);
		g[3] = (byte) (_a >> 24);
		g[4] = (byte) (_b);
		g[5] = (byte) (_b >> 8);
		g[6] = (byte) (_c);
		g[7] = (byte) (_c >> 8);
		g[8] = _d;
		g[9] = _e;
		g[10] = _f;
		g[11] = _g;
		g[12] = _h;
		g[13] = _i;
		g[14] = _j;
		g[15] = _k;

		return g;
	}
}