using System;
using System.Collections.Generic;
using System.Text;

namespace ExchCommonLib.Enums
{
    public enum ParseState : int
    {
        NotStarted = 0,
        InProccess = 1,
        Pause = 2,
        Stop = 3,
        End = 4,
        WaitContinue = 5,
        WaitNextUpdate = 6,
        ReachСurrentDate = 7,
    }
}
