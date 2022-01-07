using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public void MoveCamera(float amountX, float amountY) {
        transform.position = new Vector3(amountX, amountY, 0);
    }
}
