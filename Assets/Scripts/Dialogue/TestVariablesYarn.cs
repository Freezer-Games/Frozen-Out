using UnityEngine;
using Yarn.Unity;

namespace Assets.Scripts.Dialogue
{
    public class TestVariablesYarn : MonoBehaviour
    {
        VariableStorageYarn variableStorageYarn;
        // Start is called before the first frame update
        void Start()
        {
            variableStorageYarn = FindObjectOfType<VariableStorageYarn>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                variableStorageYarn.SetValue("reached_top", true);
            }
        }
    }
}
