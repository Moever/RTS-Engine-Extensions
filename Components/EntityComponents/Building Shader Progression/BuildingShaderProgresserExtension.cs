using System.Collections.Generic;
using RTSEngine.EntityComponent;
using RTSEngine.Model;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MAIN.Scripts.Extensions
{
    public class BuildingShaderProgresserExtension : EntityComponentBase
    {
        public List<Renderer> progressRenderers = new List<Renderer>();
        private static readonly int alphaCutValue = Shader.PropertyToID("AlphaCutValue");
        protected override void OnInit()
        {
            base.OnInit();
            foreach (Renderer progressMaterial in progressRenderers)
            {
                foreach (Material material in progressMaterial.materials)
                {
                    if (material.HasProperty(alphaCutValue))
                        material.SetFloat(alphaCutValue, 0);
                }
            }
        }

        private void Update()
        {
            if(Entity is { Health: { } })
            {
                foreach (Renderer progressMaterial in progressRenderers)
                {
                    foreach (Material material in progressMaterial.materials)
                    {
                        if (material.HasProperty(alphaCutValue))
                        {
                            material.SetFloat(alphaCutValue, Mathf.Lerp(material.GetFloat(alphaCutValue), 14 * (Entity.Health.HealthRatio), Time.deltaTime));
                        }
                    }
                }
            }
        }
        
#if UNITY_EDITOR
        [ContextMenu("AddSelectedModels")]
        public void AddModels()
        {
            foreach (GameObject o in Selection.gameObjects)
            {
                progressRenderers.Add(o.GetComponent<Renderer>());
            }
        }
#endif
    }

}