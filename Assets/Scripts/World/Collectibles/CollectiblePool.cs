using UnityEngine;

public class CollectiblePool : MonoBehaviour
{
    [Header("Área:0, 1 ou 2")]
    public int area;

    private Collectible[] collectibles;

    private void Awake()
    {
        collectibles = GetComponentsInChildren<Collectible>(true);
        SetupCollectibles();
    }

    private void SetupCollectibles()
    {
        for (int i = 0; i < collectibles.Length; i++)
        {
            collectibles[i].id = i;
            collectibles[i].area = area;

            collectibles[i].SetCollected(CollectibleManager.IsCollected(area, i));
        }
    }
}