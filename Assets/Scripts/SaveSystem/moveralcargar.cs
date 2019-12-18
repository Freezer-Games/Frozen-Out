using UnityEngine;

public class moveralcargar : MonoBehaviour
{
    public void mover(Vector3 newposition){
        gameObject.transform.position = newposition;
    }
    public void rotar(float[] newrotation) {
        gameObject.transform.rotation.Set(newrotation[0], newrotation[1], newrotation[2], newrotation[3]);
    }
}
