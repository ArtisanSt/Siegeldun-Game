public interface IMoveable
{
    public MovementProp movementProp { get; }
    public MovementSystem movementSystem { get; }
    public UnityEngine.Rigidbody2D rbody { get; }
}
