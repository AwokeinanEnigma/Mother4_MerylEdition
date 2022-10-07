using System;
using System.IO;
using System.Xml.Serialization;
using Carbine;
using Carbine.Graphics;
using Mother4.Data;
using Mother4.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Battle.Background
{
	internal class BattleBackground
	{
		public BattleBackground(string file)
		{
       //     Console.WriteLine("1");
            LayerParams[] array;
          //  Console.WriteLine("2");
			using (Stream stream = File.OpenRead(file))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(LayerParams[]));
				array = (LayerParams[])xmlSerializer.Deserialize(stream);
			}
           // Console.WriteLine("3");
			for (int i = 0; i < array.Length; i++)
			{
				array[i].File = Paths.GRAPHICS + "BBG/" + Path.GetFileName(array[i].File);
			}
         //   Console.WriteLine("4");
			this.Initialize(array);
          //  Console.WriteLine("5");
		}

		public BattleBackground(LayerParams[] parameters)
		{
			this.Initialize(parameters);
		}

		private void Initialize(LayerParams[] parameters)
		{
			this.CreateLayers(parameters);
			uint num = 320U;
			uint num2 = 180U;
			this.buffers = new RenderTexture[]
			{
				new RenderTexture(num, num2),
				new RenderTexture(num, num2)
			};
			this.buffers[0].Texture.Repeated = true;
			this.buffers[1].Texture.Repeated = true;
			this.bbgVerts = new VertexArray(PrimitiveType.Quads, 4U);
			this.bbgVerts[0U] = new Vertex(new Vector2f(0f, 0f), new Vector2f(0f, 0f));
			this.bbgVerts[1U] = new Vertex(new Vector2f(num, 0f), new Vector2f(num, 0f));
			this.bbgVerts[2U] = new Vertex(new Vector2f(num, num2), new Vector2f(num, num2));
			this.bbgVerts[3U] = new Vertex(new Vector2f(0f, num2), new Vector2f(0f, num2));
			this.bbgStates = new RenderStates(BlendMode.Alpha, Transform.Identity, null, null);
			this.bbgStateTranslation = new Vector2f(0f, 0f);
		}

		private void CreateLayers(LayerParams[] parameters)
		{
			Shader shader = new Shader(EmbeddedResources.GetStream("Mother4.Resources.bbg.vert"), EmbeddedResources.GetStream("Mother4.Resources.bbg.frag"));
			this.layers = new BackgroundLayer[parameters.Length];
			int num = this.layers.Length;
			for (int i = 0; i < num; i++)
			{
				this.layers[i] = new BackgroundLayer(shader, parameters[i]);
			}
		}


        public void UpdateParameters(LayerParams parameters, int index)
        {
            layers[index].UpdateParameters(parameters);
        }

        public void UpdateAllParameters(LayerParams parameters)
        {
            foreach (var backgroundLayer in layers)
            {
                backgroundLayer.UpdateParameters(parameters);
            }
        }

        public LayerParams GetParameters(int index)
        {
            return layers[index].Parameters;
        }

		public void UpdateParams(LayerParams[] parameters)
		{
			if (parameters.Length == this.layers.Length)
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					this.layers[i].UpdateParameters(parameters[i]);
				}
				return;
			}
			if (parameters.Length > 0)
			{
				this.CreateLayers(parameters);
				return;
			}
			this.layers = new BackgroundLayer[0];
		}

		public void ResetTranslation()
		{
			int num = this.layers.Length;
			for (int i = 0; i < num; i++)
			{
				this.layers[i].ResetTranslation();
			}
		}

		public void AddTranslation(float x, float y, float xFactor, float yFactor)
		{
			int num = this.layers.Length;
			for (int i = 0; i < num; i++)
			{
				this.layers[i].AddTranslation(x, y, xFactor, yFactor);
			}
		}

		public void SetBackgroundPosition(Vector2f position)
		{
			this.bbgStates.Transform.Translate(-this.bbgStateTranslation);
			this.bbgStates.Transform.Translate(position);
			this.bbgStateTranslation = position;
		}

		public void Draw(RenderTarget target)
		{
			int num = 0;
			int num2 = 1;
			this.buffers[0].Clear(Color.Transparent);
			this.buffers[1].Clear(Color.Transparent);
			this.bbgStates.Transform = Transform.Identity;
			this.bbgStates.Transform.Scale(1f, -1f, 160f, 90f);
			this.bbgStates.Texture = Engine.FrameBuffer.Texture;
			this.buffers[1].Draw(this.bbgVerts, this.bbgStates);
			this.bbgStates.Transform = Transform.Identity;
			this.bbgStates.Transform.Translate(ViewManager.Instance.FinalCenter - Engine.HALF_SCREEN_SIZE);
			for (int i = 0; i < this.layers.Length; i++)
			{
				num = (num + 1) % 2;
				num2 = (num2 + 1) % 2;
				this.buffers[num2].Clear(Color.Transparent);
				this.layers[i].Draw(this.buffers[num], this.buffers[num2]);
			}
			this.bbgStates.Texture = this.buffers[num2].Texture;
			target.Draw(this.bbgVerts, this.bbgStates);
		}

        public BackgroundLayer[] Layers
        {
            get { return layers; }
        }

		private BackgroundLayer[] layers;

		private RenderTexture[] buffers;

		private VertexArray bbgVerts;

		private RenderStates bbgStates;

		private Vector2f bbgStateTranslation;
	}
}
