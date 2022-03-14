using System;
using System.Collections.Generic;
using System.Text;

namespace PotatoEngine.SceneManagement
{
    public static class SceneManager
    {
        public static List<Scene> Scenes = new List<Scene>();
        public static int ActiveSceneId { get; private set; }

        public static void ReloadScene()
        {
            //Scene newScene = WindowVariables.window.CurrentScene.ScenePrefab;
            Scene newScene = Scenes[ActiveSceneId].Clone();
            newScene.OnLoad();
            WindowVariables.window.CurrentScene.OnUnload();
            WindowVariables.window.CurrentScene = null;
            WindowVariables.window.CurrentScene = newScene;
            
           
            
        }

        public static void LoadScene(int id)
        {
            ActiveSceneId = id;
            Scene newScene = Scenes[id].Clone();
            if(WindowVariables.window.CurrentScene != null)
                WindowVariables.window.CurrentScene.OnUnload();
            WindowVariables.window.CurrentScene = null;
            WindowVariables.window.CurrentScene = newScene;
            WindowVariables.window.CurrentScene.OnLoad();
        }

        public static void PreloadScene(Window window, int id)
        {
            ActiveSceneId = id;
            Scene newScene = Scenes[id].Clone();
            window.CurrentScene = newScene;
        }
    }
}
