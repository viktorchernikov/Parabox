using System;

public interface IEnvironmentEntity
{
    /// <summary>
    /// Current environment of an entity
    /// </summary>
    public EnvironmentType environment { get; set; }
    /// <summary>
    /// Fired when entity's environment changed
    /// </summary>
    public event EventHandler<EnvironmentChangeEventArgs> onEnvironmentChange;


    /// <summary>
    /// Updates current environment
    /// </summary>
    public void SetEnvironment(object initializer, EnvironmentType newEnv);
}
