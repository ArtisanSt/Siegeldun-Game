public interface IMoveable
{
    public UnityEngine.BoxCollider2D boxColl { get; }
    public UnityEngine.CapsuleCollider2D capColl { get; }
    public UnityEngine.Rigidbody2D rbody { get; }

    public MovementProp movementProp { get; }
    public UnityEngine.LayerMask groundLayer { get; }
    public UnityEngine.LayerMask jumpableLayers { get; }
}
