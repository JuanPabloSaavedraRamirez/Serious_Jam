using System.Threading.Tasks;
using UnityEngine;

public class LoadExample : MonoBehaviour, IAsyncInitializable
{
    public async Task InitializeAsync()
    {
        //Se agregaria la logica aca
        await Task.Delay(500); //En ves de solo esperar
        Debug.Log("Prefab listo.");
    }
}