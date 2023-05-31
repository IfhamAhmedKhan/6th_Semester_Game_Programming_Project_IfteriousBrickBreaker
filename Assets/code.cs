using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class code : MonoBehaviour
{
   void Awake() {
    DontDestroyOnLoad(transform.gameObject);
}
}
