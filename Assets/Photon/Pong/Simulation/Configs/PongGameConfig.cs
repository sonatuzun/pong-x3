namespace Quantum.Pong 
{
  using Photon.Deterministic;
  using UnityEngine;

  /// <summary>
  /// The <c>AsteroidsGameConfig</c> class holds the configuration settings for the Pong game.
  /// These settings include parameters for pong!
  /// </summary>
  public class PongGameConfig: AssetObject
  {
    [Header("Paddle Configuration")]
    [Tooltip("The speed of the paddle")]
    public FP PaddleSpeed;
    [Tooltip("Distance to the center of the map. This value is the radius in a random circular location where the ship is spawned")]
    public FP ShipSpawnDistanceToCenter = 15;

    [Header("Ball configuration")]
    [Tooltip("Prototype reference to spawn ball")]
    public AssetRef<EntityPrototype> BallPrototype;
    [Tooltip("Base speed for the ball")]
    public FP BallBaseSpeed = 15;
    
    [Header("Map configuration")]
    [Tooltip("Total size of the map. This is used to calculate when an entity is outside de gameplay area and then wrap it to the other side")]
    public FPVector2 GameMapSize = new FPVector2(25, 25);

    /// <summary>
    /// Called when the asset is loaded. Initializes the map extents based on the game map size.
    /// </summary>
    /// <param name="resourceManager">The resource manager used to load assets.</param>
    /// <param name="allocator">The memory allocator used during loading.</param>
    public override void Loaded(IResourceManager resourceManager, Native.Allocator allocator)
    {
      base.Loaded(resourceManager, allocator);
    }
  }
}