using RTSEngine.Cameras;
using RTSEngine.Effect;
using RTSEngine.EntityComponent;
using RTSEngine.Event;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AttackReticle : EntityComponentBase
{
    FactionEntityAttack factionEntityAttack;


    [SerializeField]
    public EffectObject AttackReticleEffect;
    protected IEffectObjectPool effectObjPool { private set; get; }
    protected IGlobalEventPublisher globalEvent { private set; get; }
    protected IMainCameraController cameraController { private set; get; }
    public bool doShow { get; private set; }

    protected override void OnInit()
    {
        globalEvent = gameMgr.GetService<IGlobalEventPublisher>();
        effectObjPool = gameMgr.GetService<IEffectObjectPool>();
        cameraController = gameMgr.GetService<IMainCameraController>();
        factionEntityAttack = GetComponent<FactionEntityAttack>();


        factionEntityAttack.TargetStop += FactionEntityAttack_TargetStop;

        base.OnInit();
    }

    private void FactionEntityAttack_TargetStop(IEntityTargetComponent sender, TargetDataEventArgs args)
    {
        HideReticle();
    }

    public void HideReticle()
    {
        if (spawnedEffectObject != null)
        {            
            effectObjPool.Despawn(spawnedEffectObject);
            _targetReticleManager = null;
            spawnedEffectObject = null;
        }
    }

    private IEffectObject spawnedEffectObject;
    private TargetReticleManager _targetReticleManager;

    public void ShowReticle()
    {
        spawnedEffectObject = effectObjPool.Spawn(AttackReticleEffect, new EffectObjectSpawnInput(this.transform, false, this.transform.position, Quaternion.identity));
        _targetReticleManager = spawnedEffectObject.GetComponent<TargetReticleManager>();

        // Set radius to the largest AoE size ( this is the last element in the array if setup correcly
        var aoeData = factionEntityAttack.Damage.AreaAttackData.ToArray();
        if (aoeData != null && aoeData.Length > 0)
        {
            _targetReticleManager.Init(aoeData[^1].range);
        }
    }

    private void Update()
    {
        if (spawnedEffectObject != null)
        {
            if (Input.GetMouseButtonDown(0))
            {                
                HideReticle();
                return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                HideReticle();
                return;
            }


            // Follow mouse
            spawnedEffectObject.transform.position = cameraController.ScreenToWorldPoint(Input.mousePosition, false);
        }
    }
}
