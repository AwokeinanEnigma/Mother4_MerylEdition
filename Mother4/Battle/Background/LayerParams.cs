using System;

namespace Mother4.Battle.Background
{
	public class LayerParams
	{
		public LayerParams()
		{
			this.Variation = new LayerVariation[8];
			for (int i = 0; i < 8; i++)
			{
				this.Variation[i].Mode = 0;
				this.Variation[i].A = 1f;
				this.Variation[i].B = 1f;
				this.Variation[i].C = 1f;
				this.Variation[i].D = 1f;
				this.Variation[i].E = 1f;
			}
			this.File = string.Empty;
			this.Name = string.Empty;
			this.Amplitude = 0f;
			this.Scale = 0f;
			this.Frequency = 0f;
			this.Compression = 0f;
			this.Speed = 0f;
			this.Opacity = 0f;
			this.Xtrans = 0f;
			this.Ytrans = 0f;
			this.Palette = new PaletteChange[0];
		}

		private const int LAYER_VARIATION_TYPE_COUNT = 8;

		public string File;

		public string Name;

		public int Mode;

		public int Blend;

		public float Amplitude;

		public float Scale;

		public float Frequency;

		public float Compression;

		public float Speed;

		public float Opacity;

		public float Xtrans;

		public float Ytrans;

		public LayerVariation[] Variation;

		public PaletteChange[] Palette;
	}
}
