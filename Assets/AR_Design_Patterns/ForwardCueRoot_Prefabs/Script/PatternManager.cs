using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{

    public delegate void ResetAllPatterns();
    public static event ResetAllPatterns resetAllPatterns;

    public void resetPatterns() {
        resetAllPatterns();
    }

}
