/// Component created by Marius van den Oever for RTS Engine 2023.0.1 and Pixel Perfect Fog Of war 1.6.5
/// Component version 1.2 (2023-08-15)
/// Engine 2023.0.1 - https://assetstore.unity.com/packages/tools/game-toolkits/rts-engine-2023-79732
/// Pixel Perfect Fog Of war 1.6.5 https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/pixel-perfect-fog-of-war-229484
/// Instructions
///   Add this component to the game Game Object where you added Fog of War World

using UnityEngine;

namespace Assets._GAME.Scripts.Components
{
    public class PixelPerfectFogOfWarWorldComponent : MonoBehaviour
    {
        public static bool ShowAllEntities = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                ShowAllEntities = !ShowAllEntities;

            }
        }
    }
}
