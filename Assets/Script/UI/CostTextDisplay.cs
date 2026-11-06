using TMPro;
using UnityEngine;

public class CostTextDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Storage storage;
    [SerializeField] private PathStorage pathStorage;
    [SerializeField] private string format = "Cost: {0}/{1}";

    private void Awake()
    {
        if (costText == null)
            costText = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        ResolveStorage();
        RefreshText();
    }

    private void Update()
    {
        ResolveStorage();
        RefreshText();
    }

    private void ResolveStorage()
    {
        // PathStorage can receive a different Storage asset when the next level loads.
        // Always refresh this reference so the displayed cost belongs to the active level.
        if (pathStorage != null && pathStorage.storageAsset != null)
            storage = pathStorage.storageAsset;

        if (pathStorage == null)
        {
            var manager = FindObjectOfType<LevelManager>();
            if (manager != null && manager.pathStorage != null)
            {
                pathStorage = manager.pathStorage;
                storage = manager.pathStorage.storageAsset;
            }
        }
    }

    private void RefreshText()
    {
        if (costText == null || storage == null)
            return;

        costText.text = string.Format(format, storage.CurrentCost, storage.maxCost);
    }
}
