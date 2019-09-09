using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public static class ELUtils
{
	private struct SHA256_CTX
	{
		public byte[] data;

		public uint datalen;

		public uint[] bitlen;

		public uint[] state;
	}

	private static uint[] k = new uint[64]
	{
		1116352408u,
		1899447441u,
		3049323471u,
		3921009573u,
		961987163u,
		1508970993u,
		2453635748u,
		2870763221u,
		3624381080u,
		310598401u,
		607225278u,
		1426881987u,
		1925078388u,
		2162078206u,
		2614888103u,
		3248222580u,
		3835390401u,
		4022224774u,
		264347078u,
		604807628u,
		770255983u,
		1249150122u,
		1555081692u,
		1996064986u,
		2554220882u,
		2821834349u,
		2952996808u,
		3210313671u,
		3336571891u,
		3584528711u,
		113926993u,
		338241895u,
		666307205u,
		773529912u,
		1294757372u,
		1396182291u,
		1695183700u,
		1986661051u,
		2177026350u,
		2456956037u,
		2730485921u,
		2820302411u,
		3259730800u,
		3345764771u,
		3516065817u,
		3600352804u,
		4094571909u,
		275423344u,
		430227734u,
		506948616u,
		659060556u,
		883997877u,
		958139571u,
		1322822218u,
		1537002063u,
		1747873779u,
		1955562222u,
		2024104815u,
		2227730452u,
		2361852424u,
		2428436474u,
		2756734187u,
		3204031479u,
		3329325298u
	};

	public static T Clone<T>(this T source)
	{
		if (!typeof(T).IsSerializable)
		{
			throw new ArgumentException("The type must be serializable.", "source");
		}
		if (source == null)
		{
			return default(T);
		}
		IFormatter formatter = new BinaryFormatter();
		Stream stream = new MemoryStream();
		using (stream)
		{
			formatter.Serialize(stream, source);
			stream.Seek(0L, SeekOrigin.Begin);
			return (T)formatter.Deserialize(stream);
		}
	}

	public static long GetEpochTime()
	{
		DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		return (long)(DateTime.UtcNow - d).TotalSeconds;
	}

	private static void DBL_INT_ADD(ref uint a, ref uint b, uint c)
	{
		if (a > (uint)(-1 - (int)c))
		{
			b++;
		}
		a += c;
	}

	private static uint ROTLEFT(uint a, byte b)
	{
		return (a << (int)b) | (a >> 32 - b);
	}

	private static uint ROTRIGHT(uint a, byte b)
	{
		return (a >> (int)b) | (a << 32 - b);
	}

	private static uint CH(uint x, uint y, uint z)
	{
		return (x & y) ^ (~x & z);
	}

	private static uint MAJ(uint x, uint y, uint z)
	{
		return (x & y) ^ (x & z) ^ (y & z);
	}

	private static uint EP0(uint x)
	{
		return ROTRIGHT(x, 2) ^ ROTRIGHT(x, 13) ^ ROTRIGHT(x, 22);
	}

	private static uint EP1(uint x)
	{
		return ROTRIGHT(x, 6) ^ ROTRIGHT(x, 11) ^ ROTRIGHT(x, 25);
	}

	private static uint SIG0(uint x)
	{
		return ROTRIGHT(x, 7) ^ ROTRIGHT(x, 18) ^ (x >> 3);
	}

	private static uint SIG1(uint x)
	{
		return ROTRIGHT(x, 17) ^ ROTRIGHT(x, 19) ^ (x >> 10);
	}

	private static void SHA256Transform(ref SHA256_CTX ctx, byte[] data)
	{
		uint[] array = new uint[64];
		uint num = 0u;
		uint num2 = 0u;
		while (num < 16)
		{
			array[num] = (uint)((data[num2] << 24) | (data[num2 + 1] << 16) | (data[num2 + 2] << 8) | data[num2 + 3]);
			num++;
			num2 += 4;
		}
		for (; num < 64; num++)
		{
			array[num] = SIG1(array[num - 2]) + array[num - 7] + SIG0(array[num - 15]) + array[num - 16];
		}
		uint num3 = ctx.state[0];
		uint num4 = ctx.state[1];
		uint num5 = ctx.state[2];
		uint num6 = ctx.state[3];
		uint num7 = ctx.state[4];
		uint num8 = ctx.state[5];
		uint num9 = ctx.state[6];
		uint num10 = ctx.state[7];
		for (num = 0u; num < 64; num++)
		{
			uint num11 = num10 + EP1(num7) + CH(num7, num8, num9) + k[num] + array[num];
			uint num12 = EP0(num3) + MAJ(num3, num4, num5);
			num10 = num9;
			num9 = num8;
			num8 = num7;
			num7 = num6 + num11;
			num6 = num5;
			num5 = num4;
			num4 = num3;
			num3 = num11 + num12;
		}
		ctx.state[0] += num3;
		ctx.state[1] += num4;
		ctx.state[2] += num5;
		ctx.state[3] += num6;
		ctx.state[4] += num7;
		ctx.state[5] += num8;
		ctx.state[6] += num9;
		ctx.state[7] += num10;
	}

	private static void SHA256Init(ref SHA256_CTX ctx)
	{
		ctx.datalen = 0u;
		ctx.bitlen[0] = 0u;
		ctx.bitlen[1] = 0u;
		ctx.state[0] = 1779033703u;
		ctx.state[1] = 3144134277u;
		ctx.state[2] = 1013904242u;
		ctx.state[3] = 2773480762u;
		ctx.state[4] = 1359893119u;
		ctx.state[5] = 2600822924u;
		ctx.state[6] = 528734635u;
		ctx.state[7] = 1541459225u;
	}

	private static void SHA256Update(ref SHA256_CTX ctx, byte[] data, uint len)
	{
		for (uint num = 0u; num < len; num++)
		{
			ctx.data[ctx.datalen] = data[num];
			ctx.datalen++;
			if (ctx.datalen == 64)
			{
				SHA256Transform(ref ctx, ctx.data);
				DBL_INT_ADD(ref ctx.bitlen[0], ref ctx.bitlen[1], 512u);
				ctx.datalen = 0u;
			}
		}
	}

	private static void SHA256Final(ref SHA256_CTX ctx, byte[] hash)
	{
		uint num = ctx.datalen;
		if (ctx.datalen < 56)
		{
			ctx.data[num++] = 128;
			while (num < 56)
			{
				ctx.data[num++] = 0;
			}
		}
		else
		{
			ctx.data[num++] = 128;
			while (num < 64)
			{
				ctx.data[num++] = 0;
			}
			SHA256Transform(ref ctx, ctx.data);
		}
		DBL_INT_ADD(ref ctx.bitlen[0], ref ctx.bitlen[1], ctx.datalen * 8);
		ctx.data[63] = (byte)ctx.bitlen[0];
		ctx.data[62] = (byte)(ctx.bitlen[0] >> 8);
		ctx.data[61] = (byte)(ctx.bitlen[0] >> 16);
		ctx.data[60] = (byte)(ctx.bitlen[0] >> 24);
		ctx.data[59] = (byte)ctx.bitlen[1];
		ctx.data[58] = (byte)(ctx.bitlen[1] >> 8);
		ctx.data[57] = (byte)(ctx.bitlen[1] >> 16);
		ctx.data[56] = (byte)(ctx.bitlen[1] >> 24);
		SHA256Transform(ref ctx, ctx.data);
		for (num = 0u; num < 4; num++)
		{
			hash[num] = (byte)((ctx.state[0] >> (int)(24 - num * 8)) & 0xFF);
			hash[num + 4] = (byte)((ctx.state[1] >> (int)(24 - num * 8)) & 0xFF);
			hash[num + 8] = (byte)((ctx.state[2] >> (int)(24 - num * 8)) & 0xFF);
			hash[num + 12] = (byte)((ctx.state[3] >> (int)(24 - num * 8)) & 0xFF);
			hash[num + 16] = (byte)((ctx.state[4] >> (int)(24 - num * 8)) & 0xFF);
			hash[num + 20] = (byte)((ctx.state[5] >> (int)(24 - num * 8)) & 0xFF);
			hash[num + 24] = (byte)((ctx.state[6] >> (int)(24 - num * 8)) & 0xFF);
			hash[num + 28] = (byte)((ctx.state[7] >> (int)(24 - num * 8)) & 0xFF);
		}
	}

	public static string SHA256(string data)
	{
		SHA256_CTX ctx = default(SHA256_CTX);
		ctx.data = new byte[64];
		ctx.bitlen = new uint[2];
		ctx.state = new uint[8];
		byte[] array = new byte[32];
		string text = string.Empty;
		SHA256Init(ref ctx);
		SHA256Update(ref ctx, Encoding.Default.GetBytes(data), (uint)data.Length);
		SHA256Final(ref ctx, array);
		for (int i = 0; i < 32; i++)
		{
			text += $"{array[i]:X2}";
		}
		return text;
	}
}
