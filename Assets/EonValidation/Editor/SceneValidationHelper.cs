using System;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace EonValidation.Editor
{
    public class SceneValidationHelper
    {
        private class OpenedScene : IDisposable
        {
            private readonly Scene scene;
            private readonly bool shouldClose;

            public OpenedScene(Scene scene, bool shouldClose)
            {
                this.scene = scene;
                this.shouldClose = shouldClose;
            }
            
            public void Dispose()
            {
                if (shouldClose)
                {
                    EditorSceneManager.CloseScene(scene, true);
                }
            }
        }
        
        public static IDisposable OpenScene(string scenePath, out Scene scene)
        {
            var shouldClose = false;
            scene = SceneManager.GetSceneByPath(scenePath);
            if (!scene.IsValid())
            {
                scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
                shouldClose = true;
            }
            
            return new OpenedScene(scene, shouldClose);
        }
    }
}