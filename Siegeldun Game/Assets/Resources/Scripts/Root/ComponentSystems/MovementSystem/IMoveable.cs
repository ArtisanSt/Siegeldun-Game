public interface IMoveable
{
    public MovementProp movementProp { get; }
    public UnityEngine.BoxCollider2D boxColl { get; }
    public UnityEngine.CapsuleCollider2D capColl { get; }
    public UnityEngine.Rigidbody2D rbody { get; }
}
