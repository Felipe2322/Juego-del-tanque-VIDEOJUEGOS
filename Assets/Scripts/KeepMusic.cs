using UnityEngine;

public class KeepMusic : MonoBehaviour
{
    private static KeepMusic inst;

    void Awake()
    {
        if (inst != null)
        {
            Destroy(gameObject); // evita duplicados si recargas la escena
            return;
        }
        inst = this;
        DontDestroyOnLoad(gameObject); // mantiene la música al cambiar de escena
    }
}
