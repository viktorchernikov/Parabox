using System;

public class EnvironmentChangeEventArgs : EventArgs
{
    public EnvironmentType previous { get; set; }
    public EnvironmentType current { get; set; }
}
