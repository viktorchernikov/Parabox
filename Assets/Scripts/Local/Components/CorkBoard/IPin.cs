using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPin
{
    public bool attached { get; }
    public void Attach(Vector3 corkBoardDir, Vector3 pos);
    public void Detach();
}
