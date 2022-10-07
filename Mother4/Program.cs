using System;
using System.IO;
using Carbine;
using Carbine.Audio;
using Carbine.Scenes;
using Mother4.Data;
using Mother4.Data.Enemies;
using Mother4.Scenes;
using Mother4.Scenes.Transitions;
using Carbine.Scenes.Transitions;

namespace Mother4
{
	internal class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
            Engine.Initialize(args);
            AudioManager.Instance.MusicVolume = Settings.MusicVolume;
            AudioManager.Instance.EffectsVolume = Settings.EffectsVolume;
            Scene newScene = new TitleScene();
            EnemyFile.Load();

            try
            {
                SceneManager.Instance.Push(newScene);
                while (Engine.Running)
                {
                    Engine.Update();
                }
            }
            catch (Exception value)
            {
                SceneManager.Instance.AbortTransition();
                SceneManager.Instance.Clear();
                SceneManager.Instance.Transition = new IrisTransition(0.2f);
                SceneManager.Instance.Push(new ErrorScene(value));

                StreamWriter streamWriter = new StreamWriter("error.log", true);
                streamWriter.WriteLine("At {0}:", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:fff"));
                streamWriter.WriteLine(value);
                streamWriter.WriteLine();
                streamWriter.Close();
            }
        }
    }
}
