using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TargetReticleManager : MonoBehaviour
{
    [SerializeField]
    DecalProjector _projector;
    public void Init(float radius)
    {        
        _radius = radius;

        // Projector size need to be * 2 for visualisation
        _projector.size = new Vector3(radius*2, radius*2, 10);
    }

    private float _radius;

    private void Update()
    {
        transform.Rotate(Vector3.up * (5f * Time.deltaTime));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, _radius);
    }
}
