using System;
using System.Collections.Generic;

public interface IControllable
{
    public void Receiver(Dictionary<string, object> controls);
}
