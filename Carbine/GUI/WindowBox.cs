using SFML.Graphics;
using SFML.System;
using Carbine.Graphics;
using Carbine.Utility;

namespace Carbine.GUI
{
	public class WindowBox : Renderable
	{
		public override Vector2f Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
				this.ConfigureTransform();
			}
		}

		public override Vector2f Origin
		{
			get
			{
				return this.origin;
			}
			set
			{
				this.origin = value;
				this.ConfigureQuads();
			}
		}

		public override Vector2f Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
				this.ConfigureQuads();
			}
		}

		public WindowBox.Style FrameStyle
		{
			get
			{
				return this.style;
			}
			set
			{
				this.SetStyle(value);
			}
		}

		public uint Palette
		{
			get
			{
				return this.palette;
			}
			set
			{
				this.palette = value;
			}
		}

		public WindowBox(WindowBox.Style style, uint palette, Vector2f position, Vector2f size, int depth)
		{
			this.style = style;
			this.palette = palette;
			this.position = position;
			this.size = size;
			this.depth = depth;
			this.SetStyle(this.style);
		}

		private void SetStyle(WindowBox.Style newStyle)
		{
			this.style = newStyle;
			string resource;
			switch (this.style)
			{
				case WindowBox.Style.Classic:
					resource = "Resources/Graphics/window2.dat";
					this.beamRepeat = false;
					goto IL_4D;
				case WindowBox.Style.Telepathy:
					resource = "Resources/Graphics/window3.dat";
					this.beamRepeat = true;
					goto IL_4D;
			}
			resource = "Resources/Graphics/window1.dat";
			this.beamRepeat = false;
		IL_4D:
			this.frame = new IndexedColorGraphic(resource, "center", this.position, this.depth);
			this.frame.CurrentPalette = this.palette;
			((IndexedTexture)this.frame.Texture).CurrentPalette = this.palette;
			this.shader = new Shader(EmbeddedResources.GetStream("Carbine.Resources.pal.vert"), EmbeddedResources.GetStream("Carbine.Resources.pal.frag"));
			this.shader.SetParameter("image", this.frame.Texture.Image);
			this.shader.SetParameter("palette", ((IndexedTexture)this.frame.Texture).Palette);
			this.shader.SetParameter("palIndex", ((IndexedTexture)this.frame.Texture).CurrentPaletteFloat);
			this.shader.SetParameter("palSize", ((IndexedTexture)this.frame.Texture).PaletteSize);
			this.shader.SetParameter("blend", Color.White);
			this.shader.SetParameter("blendMode", 1f);
			this.states = new RenderStates(BlendMode.Alpha, this.transform, this.frame.Texture.Image, this.shader);
			this.verts = new VertexArray(PrimitiveType.Quads);
			this.ConfigureQuads();
			this.ConfigureTransform();
			//foreach (var thing in frame.defini)
		}

		private void ConfigureTransform()
		{
			this.transform = new Transform(1f, 0f, this.position.X, 0f, 1f, this.position.Y, 0f, 0f, 1f);
			this.states.Transform = this.transform;
		}

		private void ConfigureQuads()
		{
			SpriteDefinition spriteDefinition = this.frame.GetSpriteDefinition("topleft");
			SpriteDefinition spriteDefinition2 = this.frame.GetSpriteDefinition("topright");
			SpriteDefinition spriteDefinition3 = this.frame.GetSpriteDefinition("bottomleft");
			SpriteDefinition spriteDefinition4 = this.frame.GetSpriteDefinition("bottomright");
			SpriteDefinition spriteDefinition5 = this.frame.GetSpriteDefinition("top");
			SpriteDefinition spriteDefinition6 = this.frame.GetSpriteDefinition("bottom");
			SpriteDefinition spriteDefinition7 = this.frame.GetSpriteDefinition("left");
			SpriteDefinition spriteDefinition8 = this.frame.GetSpriteDefinition("right");
			SpriteDefinition spriteDefinition9 = this.frame.GetSpriteDefinition("center");
			Vector2i bounds = spriteDefinition.Bounds;
			Vector2i bounds2 = spriteDefinition5.Bounds;
			Vector2i bounds3 = spriteDefinition2.Bounds;
			Vector2i bounds4 = spriteDefinition8.Bounds;
			Vector2i bounds5 = spriteDefinition4.Bounds;
			Vector2i bounds6 = spriteDefinition6.Bounds;
			Vector2i bounds7 = spriteDefinition3.Bounds;
			Vector2i bounds8 = spriteDefinition7.Bounds;
			Vector2i bounds9 = spriteDefinition9.Bounds;
			int num = (int)this.Size.X - bounds.X - bounds3.X;
			int num2 = (int)this.Size.X - bounds7.X - bounds5.X;
			int num3 = (int)this.Size.Y - bounds3.Y - bounds5.Y;
			int num4 = (int)this.Size.Y - bounds.Y - bounds7.Y;
			this.verts.Clear();
			Vector2f vector2f = default(Vector2f);
			Vector2f vector2f2 = vector2f;
			this.verts.Append(new Vertex(vector2f2, new Vector2f((float)spriteDefinition.Coords.X, (float)spriteDefinition.Coords.Y)));
			this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds.X, 0f), new Vector2f((float)(spriteDefinition.Coords.X + bounds.X), (float)spriteDefinition.Coords.Y)));
			this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds.X, (float)bounds.Y), new Vector2f((float)(spriteDefinition.Coords.X + bounds.X), (float)(spriteDefinition.Coords.Y + bounds.Y))));
			this.verts.Append(new Vertex(vector2f2 + new Vector2f(0f, (float)bounds.Y), new Vector2f((float)spriteDefinition.Coords.X, (float)(spriteDefinition.Coords.Y + bounds.Y))));
			vector2f2 += new Vector2f((float)(num + bounds.X), 0f);
			this.verts.Append(new Vertex(vector2f2, new Vector2f((float)spriteDefinition2.Coords.X, (float)spriteDefinition2.Coords.Y)));
			this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds3.X, 0f), new Vector2f((float)(spriteDefinition2.Coords.X + bounds3.X), (float)spriteDefinition2.Coords.Y)));
			this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds3.X, (float)bounds3.Y), new Vector2f((float)(spriteDefinition2.Coords.X + bounds3.X), (float)(spriteDefinition2.Coords.Y + bounds3.Y))));
			this.verts.Append(new Vertex(vector2f2 + new Vector2f(0f, (float)bounds3.Y), new Vector2f((float)spriteDefinition2.Coords.X, (float)(spriteDefinition2.Coords.Y + bounds3.Y))));
			vector2f2 += new Vector2f(0f, (float)(num3 + bounds3.Y));
			this.verts.Append(new Vertex(vector2f2, new Vector2f((float)spriteDefinition4.Coords.X, (float)spriteDefinition4.Coords.Y)));
			this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds5.X, 0f), new Vector2f((float)(spriteDefinition4.Coords.X + bounds5.X), (float)spriteDefinition4.Coords.Y)));
			this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds5.X, (float)bounds5.Y), new Vector2f((float)(spriteDefinition4.Coords.X + bounds5.X), (float)(spriteDefinition4.Coords.Y + bounds5.Y))));
			this.verts.Append(new Vertex(vector2f2 + new Vector2f(0f, (float)bounds5.Y), new Vector2f((float)spriteDefinition4.Coords.X, (float)(spriteDefinition4.Coords.Y + bounds5.Y))));
			vector2f2 -= new Vector2f((float)(num2 + bounds7.X), 0f);
			this.verts.Append(new Vertex(vector2f2, new Vector2f((float)spriteDefinition3.Coords.X, (float)spriteDefinition3.Coords.Y)));
			this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds7.X, 0f), new Vector2f((float)(spriteDefinition3.Coords.X + bounds7.X), (float)spriteDefinition3.Coords.Y)));
			this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds7.X, (float)bounds7.Y), new Vector2f((float)(spriteDefinition3.Coords.X + bounds7.X), (float)(spriteDefinition3.Coords.Y + bounds7.Y))));
			this.verts.Append(new Vertex(vector2f2 + new Vector2f(0f, (float)bounds7.Y), new Vector2f((float)spriteDefinition3.Coords.X, (float)(spriteDefinition3.Coords.Y + bounds7.Y))));
			vector2f2 += new Vector2f((float)bounds8.X, (float)(-(float)num4));
			this.verts.Append(new Vertex(vector2f2, new Vector2f((float)spriteDefinition9.Coords.X, (float)spriteDefinition9.Coords.Y)));
			this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)num, 0f), new Vector2f((float)(spriteDefinition9.Coords.X + bounds9.X), (float)spriteDefinition9.Coords.Y)));
			this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)num, (float)num4), new Vector2f((float)(spriteDefinition9.Coords.X + bounds9.X), (float)(spriteDefinition9.Coords.Y + bounds9.Y))));
			this.verts.Append(new Vertex(vector2f2 + new Vector2f(0f, (float)num4), new Vector2f((float)spriteDefinition9.Coords.X, (float)(spriteDefinition9.Coords.Y + bounds9.Y))));
			if (!this.beamRepeat)
			{
				vector2f2 = vector2f;
				vector2f2 += new Vector2f((float)bounds.X, 0f);
				this.verts.Append(new Vertex(vector2f2, new Vector2f((float)spriteDefinition5.Coords.X, (float)spriteDefinition5.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)num, 0f), new Vector2f((float)(spriteDefinition5.Coords.X + bounds2.X), (float)spriteDefinition5.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)num, (float)bounds2.Y), new Vector2f((float)(spriteDefinition5.Coords.X + bounds2.X), (float)(spriteDefinition5.Coords.Y + bounds2.Y))));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f(0f, (float)bounds2.Y), new Vector2f((float)spriteDefinition5.Coords.X, (float)(spriteDefinition5.Coords.Y + bounds2.Y))));
				vector2f2 = vector2f;
				vector2f2 += new Vector2f((float)(bounds.X + num), (float)bounds3.Y);
				this.verts.Append(new Vertex(vector2f2, new Vector2f((float)spriteDefinition8.Coords.X, (float)spriteDefinition8.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds4.X, 0f), new Vector2f((float)(spriteDefinition8.Coords.X + bounds4.X), (float)spriteDefinition8.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds3.X, (float)num3), new Vector2f((float)(spriteDefinition8.Coords.X + bounds4.X), (float)(spriteDefinition8.Coords.Y + bounds4.Y))));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f(0f, (float)num3), new Vector2f((float)spriteDefinition8.Coords.X, (float)(spriteDefinition8.Coords.Y + bounds4.Y))));
				vector2f2 = vector2f;
				vector2f2 += new Vector2f((float)bounds7.X, (float)(bounds.Y + num4));
				this.verts.Append(new Vertex(vector2f2, new Vector2f((float)spriteDefinition6.Coords.X, (float)spriteDefinition6.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)num2, 0f), new Vector2f((float)(spriteDefinition6.Coords.X + bounds6.X), (float)spriteDefinition6.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)num2, (float)bounds6.Y), new Vector2f((float)(spriteDefinition6.Coords.X + bounds6.X), (float)(spriteDefinition6.Coords.Y + bounds6.Y))));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f(0f, (float)bounds6.Y), new Vector2f((float)spriteDefinition6.Coords.X, (float)(spriteDefinition6.Coords.Y + bounds6.Y))));
				vector2f2 = vector2f;
				vector2f2 += new Vector2f(0f, (float)bounds.Y);
				this.verts.Append(new Vertex(vector2f2, new Vector2f((float)spriteDefinition7.Coords.X, (float)spriteDefinition7.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds8.X, 0f), new Vector2f((float)(spriteDefinition7.Coords.X + bounds8.X), (float)spriteDefinition7.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds8.X, (float)num4), new Vector2f((float)(spriteDefinition7.Coords.X + bounds8.X), (float)(spriteDefinition7.Coords.Y + bounds8.Y))));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f(0f, (float)num4), new Vector2f((float)spriteDefinition7.Coords.X, (float)(spriteDefinition7.Coords.Y + bounds8.Y))));
				return;
			}
			int num5 = num / bounds2.X;
			int num6 = num % bounds2.X;
			vector2f2 = vector2f + new Vector2f((float)bounds.X, 0f);
			for (int i = 0; i < num5; i++)
			{
				this.verts.Append(new Vertex(vector2f2, new Vector2f((float)spriteDefinition5.Coords.X, (float)spriteDefinition5.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds2.X, 0f), new Vector2f((float)(spriteDefinition5.Coords.X + bounds2.X), (float)spriteDefinition5.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds2.X, (float)bounds2.Y), new Vector2f((float)(spriteDefinition5.Coords.X + bounds2.X), (float)(spriteDefinition5.Coords.Y + bounds2.Y))));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f(0f, (float)bounds2.Y), new Vector2f((float)spriteDefinition5.Coords.X, (float)(spriteDefinition5.Coords.Y + bounds2.Y))));
				vector2f2 += new Vector2f((float)bounds2.X, 0f);
			}
			if (num6 != 0)
			{
				this.verts.Append(new Vertex(vector2f2, new Vector2f((float)spriteDefinition5.Coords.X, (float)spriteDefinition5.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)num6, 0f), new Vector2f((float)(spriteDefinition5.Coords.X + num6), (float)spriteDefinition5.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)num6, (float)bounds2.Y), new Vector2f((float)(spriteDefinition5.Coords.X + num6), (float)(spriteDefinition5.Coords.Y + bounds2.Y))));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f(0f, (float)bounds2.Y), new Vector2f((float)spriteDefinition5.Coords.X, (float)(spriteDefinition5.Coords.Y + bounds2.Y))));
			}
			int num7 = num2 / bounds6.X;
			int num8 = num2 % bounds6.X;
			vector2f2 = vector2f + new Vector2f((float)bounds.X, (float)(bounds.Y + num4));
			for (int j = 0; j < num7; j++)
			{
				this.verts.Append(new Vertex(vector2f2, new Vector2f((float)spriteDefinition6.Coords.X, (float)spriteDefinition6.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds6.X, 0f), new Vector2f((float)(spriteDefinition6.Coords.X + bounds6.X), (float)spriteDefinition6.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds6.X, (float)bounds6.Y), new Vector2f((float)(spriteDefinition6.Coords.X + bounds6.X), (float)(spriteDefinition6.Coords.Y + bounds6.Y))));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f(0f, (float)bounds6.Y), new Vector2f((float)spriteDefinition6.Coords.X, (float)(spriteDefinition6.Coords.Y + bounds6.Y))));
				vector2f2 += new Vector2f((float)bounds6.X, 0f);
			}
			if (num8 != 0)
			{
				this.verts.Append(new Vertex(vector2f2, new Vector2f((float)spriteDefinition6.Coords.X, (float)spriteDefinition6.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)num8, 0f), new Vector2f((float)(spriteDefinition6.Coords.X + num8), (float)spriteDefinition6.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)num8, (float)bounds6.Y), new Vector2f((float)(spriteDefinition6.Coords.X + num8), (float)(spriteDefinition6.Coords.Y + bounds6.Y))));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f(0f, (float)bounds6.Y), new Vector2f((float)spriteDefinition6.Coords.X, (float)(spriteDefinition6.Coords.Y + bounds6.Y))));
			}
			int num9 = num4 / bounds8.Y;
			int num10 = num4 % bounds8.Y;
			vector2f2 = vector2f + new Vector2f(0f, (float)bounds.Y);
			for (int k = 0; k < num9; k++)
			{
				this.verts.Append(new Vertex(vector2f2, new Vector2f((float)spriteDefinition7.Coords.X, (float)spriteDefinition7.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds8.X, 0f), new Vector2f((float)(spriteDefinition7.Coords.X + bounds8.X), (float)spriteDefinition7.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds8.X, (float)bounds8.Y), new Vector2f((float)(spriteDefinition7.Coords.X + bounds8.X), (float)(spriteDefinition7.Coords.Y + bounds8.Y))));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f(0f, (float)bounds8.Y), new Vector2f((float)spriteDefinition7.Coords.X, (float)(spriteDefinition7.Coords.Y + bounds8.Y))));
				vector2f2 += new Vector2f(0f, (float)bounds8.Y);
			}
			if (num10 != 0)
			{
				this.verts.Append(new Vertex(vector2f2, new Vector2f((float)spriteDefinition7.Coords.X, (float)spriteDefinition7.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds8.X, 0f), new Vector2f((float)(spriteDefinition7.Coords.X + bounds8.X), (float)spriteDefinition7.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds8.X, (float)num10), new Vector2f((float)(spriteDefinition7.Coords.X + bounds8.X), (float)(spriteDefinition7.Coords.Y + num10))));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f(0f, (float)num10), new Vector2f((float)spriteDefinition7.Coords.X, (float)(spriteDefinition7.Coords.Y + num10))));
			}
			int num11 = num3 / bounds4.Y;
			int num12 = num3 % bounds4.Y;
			vector2f2 = vector2f + new Vector2f((float)(bounds.X + num), (float)bounds.Y);
			for (int l = 0; l < num11; l++)
			{
				this.verts.Append(new Vertex(vector2f2, new Vector2f((float)spriteDefinition8.Coords.X, (float)spriteDefinition8.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds4.X, 0f), new Vector2f((float)(spriteDefinition8.Coords.X + bounds4.X), (float)spriteDefinition8.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds4.X, (float)bounds4.Y), new Vector2f((float)(spriteDefinition8.Coords.X + bounds4.X), (float)(spriteDefinition8.Coords.Y + bounds4.Y))));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f(0f, (float)bounds4.Y), new Vector2f((float)spriteDefinition8.Coords.X, (float)(spriteDefinition8.Coords.Y + bounds4.Y))));
				vector2f2 += new Vector2f(0f, (float)bounds4.Y);
			}
			if (num12 != 0)
			{
				this.verts.Append(new Vertex(vector2f2, new Vector2f((float)spriteDefinition8.Coords.X, (float)spriteDefinition8.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds4.X, 0f), new Vector2f((float)(spriteDefinition8.Coords.X + bounds4.X), (float)spriteDefinition8.Coords.Y)));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f((float)bounds4.X, (float)num12), new Vector2f((float)(spriteDefinition8.Coords.X + bounds4.X), (float)(spriteDefinition8.Coords.Y + num12))));
				this.verts.Append(new Vertex(vector2f2 + new Vector2f(0f, (float)num12), new Vector2f((float)spriteDefinition8.Coords.X, (float)(spriteDefinition8.Coords.Y + num12))));
			}
		}

		public override void Draw(RenderTarget target)
		{
			target.Draw(this.verts, this.states);
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.frame.Dispose();
			}
			this.disposed = true;
		}

		private bool beamRepeat;

		private uint palette;

		private WindowBox.Style style;

		private IndexedColorGraphic frame;

		private RenderStates states;

		private Transform transform;

		private VertexArray verts;

		private Shader shader;

		public enum Style
		{
			Normal,
			Classic,
			Telepathy
		}
	}
}