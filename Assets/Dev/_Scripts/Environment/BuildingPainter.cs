using UnityEngine;
using System.Collections.Generic;

public class BuildingPainter
{
    private List<Material> _buildingMaterials;

    public BuildingPainter(List<Material> buildingMaterials)
    {
        _buildingMaterials = buildingMaterials;
    }

    public void PaintBuildings(GameObject roadSegment)
    {
        List<Transform> buildings = new List<Transform>();
        FindBuildings(roadSegment.transform, buildings);

        if (buildings.Count == 0)
        {
            Debug.LogWarning($"No buildings found in road segment {roadSegment.name}.");
            return;
        }

        Dictionary<Transform, Material> houseMaterials = new Dictionary<Transform, Material>();

        foreach (Transform building in buildings)
        {
            Renderer renderer = building.GetComponent<Renderer>();
            if (renderer != null && _buildingMaterials.Count > 0)
            {
                if (houseMaterials.ContainsKey(building.parent))
                {
                    renderer.material = houseMaterials[building.parent];
                }
                else
                {
                    Material randomMaterial = _buildingMaterials[Random.Range(0, _buildingMaterials.Count)];
                    renderer.material = randomMaterial;
                    houseMaterials[building.parent] = randomMaterial;
                }
            }
        }
    }

    private void FindBuildings(Transform parent, List<Transform> buildings)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag("Building"))
            {
                buildings.Add(child);
            }
            FindBuildings(child, buildings);
        }
    }
}