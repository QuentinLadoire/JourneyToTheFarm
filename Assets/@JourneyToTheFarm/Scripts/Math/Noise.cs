using System;

public static class Noise
{
	static readonly int[] perm = {
		151, 160, 137,  91,  90,  15, 131,  13, 201,  95,  96,  53, 194, 233,
		  7, 225, 140,  36, 103,  30,  69, 142,   8,  99,  37, 240,  21,  10,
		 23, 190,   6, 148, 247, 120, 234,  75,   0,  26, 197,  62,  94, 252,
		219, 203, 117,  35,  11,  32,  57, 177,  33,  88, 237, 149,  56,  87,
		174,  20, 125, 136, 171, 168,  68, 175,  74, 165,  71, 134, 139,  48,
		 27, 166,  77, 146, 158, 231,  83, 111, 229, 122,  60, 211, 133, 230,
		220, 105,  92,  41,  55,  46, 245,  40, 244, 102, 143,  54,  65,  25,
		 63, 161,   1, 216,  80,  73, 209,  76, 132, 187, 208,  89,  18, 169,
		200, 196, 135, 130, 116, 188, 159,  86, 164, 100, 109, 198, 173, 186,
		  3,  64,  52, 217, 226, 250, 124, 123,   5, 202,  38, 147, 118, 126,
		255,  82,  85, 212, 207, 206,  59, 227,  47,  16,  58,  17, 182, 189,
		 28,  42, 223, 183, 170, 213, 119, 248, 152,   2,  44, 154, 163,  70,
		221, 153, 101, 155, 167,  43, 172,   9, 129,  22,  39, 253,  19,  98,
		108, 110,  79, 113, 224, 232, 178, 185, 112, 104, 218, 246,  97, 228,
		251,  34, 242, 193, 238, 210, 144,  12, 191, 179, 162, 241,  81,  51,
		145, 235, 249,  14, 239, 107,  49, 192, 214,  31, 181, 199, 106, 157,
		184,  84, 204, 176, 115, 121,  50,  45, 127,   4, 150, 254, 138, 236,
		205,  93, 222, 114,  67,  29,  24,  72, 243, 141, 128, 195,  78,  66,
		215,  61, 156, 180,

		151, 160, 137,  91,  90,  15, 131,  13, 201,  95,  96,  53, 194, 233,
		  7, 225, 140,  36, 103,  30,  69, 142,   8,  99,  37, 240,  21,  10,
		 23, 190,   6, 148, 247, 120, 234,  75,   0,  26, 197,  62,  94, 252,
		219, 203, 117,  35,  11,  32,  57, 177,  33,  88, 237, 149,  56,  87,
		174,  20, 125, 136, 171, 168,  68, 175,  74, 165,  71, 134, 139,  48,
		 27, 166,  77, 146, 158, 231,  83, 111, 229, 122,  60, 211, 133, 230,
		220, 105,  92,  41,  55,  46, 245,  40, 244, 102, 143,  54,  65,  25,
		 63, 161,   1, 216,  80,  73, 209,  76, 132, 187, 208,  89,  18, 169,
		200, 196, 135, 130, 116, 188, 159,  86, 164, 100, 109, 198, 173, 186,
		  3,  64,  52, 217, 226, 250, 124, 123,   5, 202,  38, 147, 118, 126,
		255,  82,  85, 212, 207, 206,  59, 227,  47,  16,  58,  17, 182, 189,
		 28,  42, 223, 183, 170, 213, 119, 248, 152,   2,  44, 154, 163,  70,
		221, 153, 101, 155, 167,  43, 172,   9, 129,  22,  39, 253,  19,  98,
		108, 110,  79, 113, 224, 232, 178, 185, 112, 104, 218, 246,  97, 228,
		251,  34, 242, 193, 238, 210, 144,  12, 191, 179, 162, 241,  81,  51,
		145, 235, 249,  14, 239, 107,  49, 192, 214,  31, 181, 199, 106, 157,
		184,  84, 204, 176, 115, 121,  50,  45, 127,   4, 150, 254, 138, 236,
		205,  93, 222, 114,  67,  29,  24,  72, 243, 141, 128, 195,  78,  66,
		215,  61, 156, 180
	};

	static float Fade(float t)
	{
		return t * t * t * (t * (t * 6 - 15) + 10);
	}
	static float Lerp(float a, float b, float t)
	{
		return a + t * (b - a);
	}

	static float Grad(int hash, float x, float y)
	{
		return (hash & 0x3) switch
		{
			0x0 => x + y,
			0x1 => -x + y,
			0x2 => x - y,
			0x3 => -x - y,
			_ => 0 // never happens
		};
	}
	static float Grad(int hash, float x, float y, float z)
	{
		return (hash & 0xF) switch
		{
			0x0 => x + y,
			0x1 => -x + y,
			0x2 => x - y,
			0x3 => -x - y,
			0x4 => x + z,
			0x5 => -x + z,
			0x6 => x - z,
			0x7 => -x - z,
			0x8 => y + z,
			0x9 => -y + z,
			0xA => y - z,
			0xB => -y - z,
			0xC => y + x,
			0xD => -y + z,
			0xE => y - x,
			0xF => -y - z,
			_ => 0 // never happens
		};
	}

	public static float SimpleNoise2D(float x, float y)
	{
		int x0 = (int)Math.Floor(x);
		int y0 = (int)Math.Floor(y);

		int xi = x0 & 255;
		int yi = y0 & 255;

		int aa = perm[perm[xi    ] + yi    ];
		int ba = perm[perm[xi + 1] + yi    ];
		int ab = perm[perm[xi    ] + yi + 1];
		int bb = perm[perm[xi + 1] + yi + 1];

		float dx0 = (x - x0);
		float dx1 = (x - (x0 + 1));
		float dy0 = (y - y0);
		float dy1 = (y - (y0 + 1));

		float u = Fade(dx0);
		return Lerp(Lerp(Grad(aa, dx0, dy0), Grad(ba, dx1, dy0), u),
						  Lerp(Grad(ab, dx0, dy1), Grad(bb, dx1, dy1), u),
						  Fade(dy0));

		//int x0 = (int)Math.Floor(x);
		//int y0 = (int)Math.Floor(y);
		//int x1 = x0 + 1;
		//int y1 = y0 + 1;

		//Gradient vector
		//int aa = perm[perm[x0 & 255] + y0 & 255];
		//int ba = perm[perm[x1 & 255] + y0 & 255];
		//int ab = perm[perm[x0 & 255] + y1 & 255];
		//int bb = perm[perm[x1 & 255] + y1 & 255];

		//Distance vector
		//float dx0 = x - x0;
		//float dy0 = y - y0;
		//
		//float dx1 = x - x1;
		//float dy1 = y - y0;
		//
		//float dx2 = x - x0;
		//float dy2 = y - y1;
		//
		//float dx3 = x - x1;
		//float dy3 = y - y1;

		//Dot
		//float dot0 = Grad(aa, dx0, dy0);
		//float dot1 = Grad(ba, dx1, dy1);
		//float dot2 = Grad(ab, dx2, dy2);
		//float dot3 = Grad(bb, dx3, dy3);

		//Lerp
		//float lerp1 = Lerp(dot0, dot1, Fade(x - x0));
		//float lerp2 = Lerp(dot2, dot3, Fade(x - x0));
		//float lerp3 = Lerp(lerp1, lerp2, Fade(y - y0));

		//return lerp3;
	}
	public static float SimpleNoise2DNormalized(float x, float y)
	{
		return (SimpleNoise2D(x, y) + 1) / 2;
	}

	public static float SimpleNoise3D(float x, float y, float z)
	{
		int x0 = (int)Math.Floor(x);
		int y0 = (int)Math.Floor(y);
		int z0 = (int)Math.Floor(z);

		int xi = x0 & 255;
		int yi = y0 & 255;
		int zi = z0 & 255;

		int aaa = perm[perm[perm[xi    ] + yi    ] + zi    ];
		int baa = perm[perm[perm[xi + 1] + yi    ] + zi    ];
		int aba = perm[perm[perm[xi    ] + yi + 1] + zi    ];
		int bba = perm[perm[perm[xi + 1] + yi + 1] + zi    ];

		int aab = perm[perm[perm[xi    ] + yi    ] + zi + 1];
		int bab = perm[perm[perm[xi + 1] + yi    ] + zi + 1];
		int abb = perm[perm[perm[xi    ] + yi + 1] + zi + 1];
		int bbb = perm[perm[perm[xi + 1] + yi + 1] + zi + 1];

		float dx0 = x - x0;
		float dx1 = x - (x0 + 1);
		float dy0 = y - y0;
		float dy1 = y - (y0 + 1);
		float dz0 = z - z0;
		float dz1 = z - (z0 + 1);

		float u = Fade(dx0);
		float v = Fade(dy0);

		return Lerp(Lerp(Lerp(Grad(aaa, dx0, dy0, dz0), Grad(baa, dx1, dy0, dz0), u),
						  Lerp(Grad(aba, dx0, dy1, dz0), Grad(bba, dx1, dy1, dz0), u),
						  v),

					 Lerp(Lerp(Grad(aab, dx0, dy0, dz1), Grad(bab, dx1, dy0, dz1), u),
						  Lerp(Grad(abb, dx0, dy1, dz1), Grad(bbb, dx1, dy1, dz1), u),
						  v),
					 Fade(dz0));

		//int x0 = (int)Math.Floor(x);
		//int y0 = (int)Math.Floor(y);
		//int z0 = (int)Math.Floor(z);
		//int x1 = x0 + 1;
		//int y1 = y0 + 1;
		//int z1 = z0 + 1;
		//
		////Gradient vector
		//int aaa = perm[perm[perm[x0 & 255] + y0 & 255] + z0 & 255];
		//int aba = perm[perm[perm[x0 & 255] + y1 & 255] + z0 & 255];
		//int baa = perm[perm[perm[x1 & 255] + y0 & 255] + z0 & 255];
		//int bba = perm[perm[perm[x1 & 255] + y1 & 255] + z0 & 255];
		//int aab = perm[perm[perm[x0 & 255] + y0 & 255] + z1 & 255];
		//int abb = perm[perm[perm[x0 & 255] + y1 & 255] + z1 & 255];
		//int bab = perm[perm[perm[x1 & 255] + y0 & 255] + z1 & 255];
		//int bbb = perm[perm[perm[x1 & 255] + y1 & 255] + z1 & 255];
		//
		//
		////Distance vector
		//float dx0 = x - x0;
		//float dy0 = y - y0;
		//float dz0 = z - z0;
		//
		//float dx1 = x - x1;
		//float dy1 = y - y0;
		//float dz1 = z - z0;
		//
		//float dx2 = x - x0;
		//float dy2 = y - y1;
		//float dz2 = z - z0;
		//
		//float dx3 = x - x1;
		//float dy3 = y - y1;
		//float dz3 = z - z0;
		//
		//float dx4 = x - x0;
		//float dy4 = y - y0;
		//float dz4 = z - z1;
		//
		//float dx5 = x - x1;
		//float dy5 = y - y0;
		//float dz5 = z - z1;
		//
		//float dx6 = x - x0;
		//float dy6 = y - y1;
		//float dz6 = z - z1;
		//
		//float dx7 = x - x1;
		//float dy7 = y - y1;
		//float dz7 = z - z1;
		//
		////Dot
		//float dot0 = Grad(aaa, dx0, dy0, dz0);
		//float dot1 = Grad(baa, dx1, dy1, dz1);
		//float dot2 = Grad(aba, dx2, dy2, dz2);
		//float dot3 = Grad(bba, dx3, dy3, dz3);
		//float dot4 = Grad(aab, dx4, dy4, dz4);
		//float dot5 = Grad(bab, dx5, dy5, dz5);
		//float dot6 = Grad(abb, dx6, dy6, dz6);
		//float dot7 = Grad(bbb, dx7, dy7, dz7);
		//
		////Lerp
		//float lerp1 = Lerp(dot0, dot1, Fade(x - x0));
		//float lerp2 = Lerp(dot2, dot3, Fade(x - x0));
		//float lerp3 = Lerp(lerp1, lerp2, Fade(y - y0));
		//
		//float lerp4 = Lerp(dot4, dot5, Fade(x - x0));
		//float lerp5 = Lerp(dot6, dot7, Fade(x - x0));
		//float lerp6 = Lerp(lerp4, lerp5, Fade(y - y0));
		//
		//float lerp7 = Lerp(lerp3, lerp6, Fade(z - z0));
		//
		//return lerp7;
	}
	public static float SimpleNoise3DNormalized(float x, float y, float z)
	{
		return (SimpleNoise3D(x, y, z) + 1) / 2;
	}

	public static float FractalNoise2D(float x, float y, int layer, float lacunarity, float persistance, int seed)
	{
		Random random = new Random(seed);
		var seedOffsetX = random.Next(-100000, 100000);
		var seedOffsetY = random.Next(-100000, 100000);

		float frequency = 1.0f;
		float amplitude = 1.0f;
		float noise = 0.0f;
		float maxNoise = 0.0f;

		for (int i = 0; i < layer; i++)
		{
			noise += SimpleNoise2D((x + seedOffsetX) * frequency,
							 (y + seedOffsetY) * frequency) * amplitude;
			maxNoise += amplitude;

			frequency *= lacunarity;
			amplitude *= persistance;
		}

		return noise / maxNoise;
	}
	public static float FractalNoise2DNormalized(float x, float y, int layer, float lacunarity, float persistance, int seed)
	{
		return (FractalNoise2D(x, y, layer, lacunarity, persistance, seed) + 1) / 2.0f;
	}

	public static float FractalNoise3D(float x, float y, float z, int layer, float lacunarity, float persistance, int seed)
	{
		Random random = new Random(seed);
		var seedOffsetX = random.Next(-100000, 100000);
		var seedOffsetY = random.Next(-100000, 100000);
		var seedOffsetZ = random.Next(-100000, 100000);

		float frequency = 1.0f;
		float amplitude = 1.0f;
		float noise = 0.0f;
		float maxNoise = 0.0f;

		for (int i = 0; i < layer; i++)
		{
			noise += SimpleNoise3D((x + seedOffsetX) * frequency,
							 (y + seedOffsetY) * frequency,
							 (z + seedOffsetZ) * frequency) * amplitude;
			maxNoise += amplitude;

			frequency *= lacunarity;
			amplitude *= persistance;
		}

		return noise / maxNoise;
	}
	public static float FractalNoise3DNormalized(float x, float y, float z, int layer, float lacunarity, float persistance, int seed)
	{
		return (FractalNoise3D(x, y, z, layer, lacunarity, persistance, seed) + 1) / 2.0f;
	}

	public static float TurbulenceNoise2D(float x, float y, int layer, float lacunarity, float persistance, int seed)
	{
		Random random = new Random(seed);
		var seedOffsetX = random.Next(-100000, 100000);
		var seedOffsetY = random.Next(-100000, 100000);

		x += seedOffsetX;
		y += seedOffsetY;

		float frequency = 1.0f;
		float amplitude = 1.0f;
		float noise = 0.0f;
		float maxNoise = 0.0f;

		for (int i = 0; i < layer; i++)
		{
			noise += Math.Abs(SimpleNoise2D(x * frequency, y * frequency)) * amplitude;
			maxNoise += amplitude;

			frequency *= lacunarity;
			amplitude *= persistance;
		}

		return noise / maxNoise;
	}

	public static float MarbleNoise2D(float x, float y, float scaleX, float scaleY, int layer, float lacunarity, float persistance, int seed)
	{
		Random random = new Random(seed);
		var seedOffsetX = random.Next(-100000, 100000);
		var seedOffsetY = random.Next(-100000, 100000);

		x += seedOffsetX;
		y += seedOffsetY;

		float frequency = 1.0f;
		float amplitude = 1.0f;
		float noise = 0.0f;

		for (int i = 0; i < layer; i++)
		{
			noise += SimpleNoise2D(x * scaleX * frequency, y * scaleY * frequency) * amplitude;

			frequency *= lacunarity;
			amplitude *= persistance;
		}

		return (float)Math.Sin((x + noise * 100.0f) * 2.0f * Math.PI / 200.0f);
	}
	public static float MarbleNoise2DNormalized(float x, float y, float scaleX, float scaleY, int layer, float lacunarity, float persistance, int seed)
	{
		return (MarbleNoise2D(x, y, scaleX, scaleY, layer, lacunarity, persistance, seed) + 1.0f) / 2.0f;
	}

	public static float WoodNoise2D(float x, float y, int layer, float lacunarity, float persistance, int seed)
	{
		Random random = new Random(seed);
		var seedOffsetX = random.Next(-100000, 100000);
		var seedOffsetY = random.Next(-100000, 100000);

		x += seedOffsetX;
		y += seedOffsetY;

		float frequency = 1.0f;
		float amplitude = 10.0f;
		float noise = 0.0f;

		for (int i = 0; i < layer; i++)
		{
			noise += SimpleNoise2DNormalized(x * frequency, y * frequency) * amplitude;

			frequency *= lacunarity;
			amplitude *= persistance;
		}

		return noise - (int)noise;
	}
}
