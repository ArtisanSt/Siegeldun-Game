public interface IMoveable
{
    public MovementProp movementProp { get; }

    public UnityEngine.Rigidbody2D rbody { get; }

    public UnityEngine.LayerMask groundLayer { get; }
    public UnityEngine.LayerMask jumpableLayers { get; }
}
