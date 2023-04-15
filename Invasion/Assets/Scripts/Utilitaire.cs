
using UnityEngine;

public class Utilitaire
{
    public static void SetLayerRecursively(GameObject obj, int newlayer)
    {
        obj.layer = newlayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newlayer);
        }
    }
}
