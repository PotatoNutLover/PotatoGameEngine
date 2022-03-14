using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using OpenTK.Mathematics;

namespace PotatoEngine
{
    public class Scene
    {
        public CameraObject CameraObject { get; private set; }

        public List<GameObject> GameObjects;
        public List<GameObject> QueueOpaque;
        public List<GameObject> QueueBeforeTransparent;
        public List<GameObject> QueueTransparent;
        public List<GameObject> QueueAfterTransparent;

        private int idCounter = 0;
        private bool isUnloading = false;

        //public Scene ScenePrefab { get; private set; }

        public Scene(CameraObject cameraObject)
        {
            CameraObject = cameraObject;
            cameraObject.ParentScene = this;
            QueueOpaque = new List<GameObject>();
            QueueTransparent = new List<GameObject>();
            QueueAfterTransparent = new List<GameObject>();
            QueueBeforeTransparent = new List<GameObject>();
            GameObjects = new List<GameObject>();
        }

        public void AddGameObject(GameObject gameObject)
        {
            if (gameObject.Texture.RenderQueue == RenderQueue.Transparent)
                QueueTransparent.Add(gameObject);
            else if (gameObject.Texture.RenderQueue == RenderQueue.AfterTransparent)
                QueueAfterTransparent.Add(gameObject);
            else if (gameObject.Texture.RenderQueue == RenderQueue.BeforeTransparent)
                QueueBeforeTransparent.Add(gameObject);
            else
                QueueOpaque.Add(gameObject);

            GameObjects.Add(gameObject);
            gameObject.ParentScene = this;
            gameObject.IdInScene = idCounter;
            idCounter++;
            
        }

        public void DeleteGameObject(GameObject gameObject)
        {
            DeleteFromQueue(GameObjects, gameObject.IdInScene);
            DeleteFromQueue(QueueOpaque, gameObject.IdInScene);
            DeleteFromQueue(QueueTransparent, gameObject.IdInScene);
            DeleteFromQueue(QueueAfterTransparent, gameObject.IdInScene);
            DeleteFromQueue(QueueBeforeTransparent, gameObject.IdInScene);
        }

        private void DeleteFromQueue(List<GameObject> list, int Id)
        {
            if (list.Where(x => x.IdInScene == Id).FirstOrDefault() != null)
                list.RemoveAt(list.FindIndex(x => x.IdInScene == Id));
        }

        public GameObject FindObjectByName(string name)
        {
            return GameObjects.Where(x => x.Name == name).FirstOrDefault();
        }

        public void OnLoad()
        {
            //ScenePrefab = this.Clone();
            CameraObject.OnLoad();
            foreach(GameObject gameObject in QueueOpaque)
            {
                gameObject.OnLoad();
            }
            foreach(GameObject gameObject in QueueBeforeTransparent)
            {
                gameObject.OnLoad();
            }
            foreach (GameObject gameObject in QueueTransparent)
            {
                gameObject.OnLoad();
            }
            foreach(GameObject gameObject in QueueAfterTransparent)
            {
                gameObject.OnLoad();
            }

            
        }

        public void OnRenderFrame()
        {
            foreach (GameObject gameObject in QueueOpaque)
            {
                gameObject.OnRenderFrame(CameraObject.Camera.GetViewMatrix(), CameraObject.Camera.GetProjectionMatrix());
            }
            FormQueueByDistance(QueueBeforeTransparent);
            foreach(GameObject gameObject in QueueBeforeTransparent)
            {
                gameObject.OnRenderFrame(CameraObject.Camera.GetViewMatrix(), CameraObject.Camera.GetProjectionMatrix());
            }
            FormQueueByDistance(QueueTransparent);
            foreach (GameObject gameObject in QueueTransparent)
            {
                gameObject.OnRenderFrame(CameraObject.Camera.GetViewMatrix(), CameraObject.Camera.GetProjectionMatrix());
            }
            FormQueueByDistance(QueueAfterTransparent);
            foreach(GameObject gameObject in QueueAfterTransparent)
            {
                gameObject.OnRenderFrame(CameraObject.Camera.GetViewMatrix(), CameraObject.Camera.GetProjectionMatrix());
            }
        }

        public void OnUpdateFrame()
        {
            CameraObject.OnUpdateFrame();
            /*foreach (GameObject gameObject in QueueOpaque)
            {
                gameObject.OnUpdateFrame();
            }
            foreach (GameObject gameObject in QueueTransparent)
            {
                gameObject.OnUpdateFrame();
            }*/
            for (int i = 0; i < QueueOpaque.Count; i++)
            {
                QueueOpaque[i].OnUpdateFrame();
            }
            for (int i = 0; i < QueueBeforeTransparent.Count; i++)
            {
                QueueBeforeTransparent[i].OnUpdateFrame();
            }
            for (int i = 0; i < QueueTransparent.Count; i++)
            {
                QueueTransparent[i].OnUpdateFrame();
            }
            for (int i = 0; i < QueueAfterTransparent.Count; i++)
            {
                QueueAfterTransparent[i].OnUpdateFrame();
            }

            if (isUnloading)
                UnloadScene();
        }

        private void UnloadScene()
        {
            CameraObject.OnUnload();
            CameraObject = null;
            foreach (GameObject gameObject in QueueOpaque)
            {
                gameObject.OnUnload();
            }
            QueueOpaque = null;
            foreach (GameObject gameObject in QueueBeforeTransparent)
            {
                gameObject.OnUnload();
            }
            QueueBeforeTransparent = null;
            foreach (GameObject gameObject in QueueTransparent)
            {
                gameObject.OnUnload();
            }
            QueueTransparent = null;
            foreach (GameObject gameObject in QueueAfterTransparent)
            {
                gameObject.OnUnload();
            }
            QueueAfterTransparent = null;
            GameObjects = null;
        }

        public void OnUnload()
        {
            isUnloading = true;
        }

        private void FormQueueByDistance(List<GameObject> queue)
        {
            
            for(int i = 0; i < queue.Count(); i++)
            {
                for(int j = 1; j < queue.Count(); j++)
                {
                    if(CalculateDistance(queue,j) > CalculateDistance(queue,j - 1))
                    {
                        GameObject swap = queue[j];
                        queue[j] = queue[j - 1];
                        queue[j - 1] = swap;
                    }
                }
            }
        }

        private float CalculateDistance(List<GameObject> queue, int i)
        {
            Vector3 cameraPosition = CameraObject.Transform.GetPosition();
            return Vector3.Distance(queue[i].Transform.GetPosition(), cameraPosition);
        }

        public Scene Clone()
        {
            Scene tempScene = new Scene(CameraObject.Clone());
            foreach(GameObject gameObject in GameObjects)
            {
                tempScene.AddGameObject(gameObject.Clone());
            }

            return tempScene;
        }
    }
}
