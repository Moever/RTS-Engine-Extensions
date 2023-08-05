using System;
/// Component created by Marius van den Oever for RTS Engine 2023.0.1 and Pixel Perfect Fog Of war 1.6.5
/// Component version 1.1 (2023-08-05)
/// Engine 2023.0.1 - https://assetstore.unity.com/packages/tools/game-toolkits/rts-engine-2023-79732
/// Pixel Perfect Fog Of war 1.6.5 https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/pixel-perfect-fog-of-war-229484
/// Instructions
///   Add this component to your entity. It will add the required components automatically.
///   Make sure to configure your hider and revealer manually 
///   If you already have a hider or revealer on your unit or buildig, make sure to either remove them OR add this component on the same component
using FOW;
using RTSEngine;
using RTSEngine.Entities;
using RTSEngine.EntityComponent;
using UnityEngine;

[RequireComponent(typeof(FogOfWarRevealer3D))]
[RequireComponent(typeof(FogOfWarHider))]
public class PixelPerfectFogOfWarComponent
    : EntityComponentBase
{
    FogOfWarRevealer _revealer;
    FogOfWarHider _hider;

    [SerializeField, Tooltip("Do you want to completely hide this entity if it's in the fog and not visible? If false, it will be shown according to your Fog of War world setttings.")]
    private bool _hideInFog = true;

    protected override void OnInit()
    {
        _revealer = GetComponent<FogOfWarRevealer>();
        _hider = GetComponent<FogOfWarHider>();

        _hider.OnActiveChanged += _hider_OnActiveChanged;
        Entity.EntityInitiated += Entity_EntityInitiated;
        Entity.FactionUpdateComplete += Entity_FactionUpdateComplete;

        base.OnInit();
    }

    private void OnDestroy()
    {
        if (Entity == null) { return; }

        Entity.EntityInitiated -= Entity_EntityInitiated;
        Entity.FactionUpdateComplete -= Entity_FactionUpdateComplete;
    }

    #region Event listener implementation
    private void Entity_EntityInitiated(IEntity sender, EventArgs args)
    {
        _reEvaluateVisibility();
    }

    private void Entity_FactionUpdateComplete(IEntity sender, RTSEngine.Event.FactionUpdateArgs args)
    {
        _reEvaluateVisibility();
    }
    #endregion

    /// <summary>
    /// This method will do the actual check and set the components active/ inactive based on the settings
    /// </summary>
    private void _reEvaluateVisibility()
    {
        // Default behaviour : show for local player and hide if it's not the local player
        if (Entity.IsLocalPlayerFaction())
        {
            _revealer.enabled = true;
            _hider.enabled = false;
        }
        else
        {
            _revealer.enabled = false;

            if (_hideInFog)
            {
                // Simple trick :)
                _hider.enabled = false;
                _hider.enabled = true;
            }
        }

    }

    /// <summary>
    /// This method currently disables the full model, which is not -always- what you want
    /// Will have to iterate on this later
    /// </summary>
    /// <param name="isActive"></param>
    private void _hider_OnActiveChanged(bool isActive)
    {
        // Should not be visible..
        Entity.Model.SetActive(isActive);
    }
}

