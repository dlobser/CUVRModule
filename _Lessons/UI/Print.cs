using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUVR{
    public class Print : MonoBehaviour
    {
        public string toPrint;

        public void PrintString(){
            print(toPrint);
        }


    }
}