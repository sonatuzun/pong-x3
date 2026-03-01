namespace Quantum
{
  /// <summary>
  /// Partial class <c>RuntimeConfig</c> contains configuration settings for the runtime behavior of the game.
  /// </summary>
  public partial class RuntimeConfig
  {
    /// <summary>
    /// Reference to the game configuration asset, specifying various settings for the Asteroids game.
    /// </summary>
    public AssetRef<Pong.PongGameConfig> GameConfig;

    public AssetRef<EntityPrototype> DefaultPlayerInfo;
    public AssetRef<EntityPrototype> DefaultPaddle;
  }
}