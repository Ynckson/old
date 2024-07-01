using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int minXZ = -20000;
    public int maxXZ = 20000;
    public int minY = -20000;
    public int maxY = 20000;
    public int minScale = 50;
    public int maxScale = 1000;
    public int planetCount = 10;
    public GameObject planetPrefab;
    public GameObject moonPrefab;
    private List<Planet> planets = new List<Planet>();

    void Start()
    {
        GenerateUniverse();
    }

    void GenerateUniverse()
    {
        for (int i = 0; i < planetCount; i++)
        {
            GeneratePlanet();
        }
        foreach (Planet planet in planets)
        {
            int scale = Random.Range(minScale, maxScale);
            planet.transform.localScale = new Vector3(scale, scale, scale);
            if(Random.Range(0, 10) > 5){ 
                Moon planetMoon = GenerateMoon(planet);
                planetMoon.transform.localScale = new Vector3(scale / 4, scale / 4, scale / 4);
                planetMoon.transform.parent = planet.transform;
            }
        }
    }

        void GeneratePlanet()
    {
        GameObject planet = Instantiate(planetPrefab);

        Planet newPlanet = planet.GetComponent<Planet>();

        PlanetShapeSettings shapeSettings = ScriptableObject.CreateInstance<PlanetShapeSettings>();
        shapeSettings.planetRadius = Random.Range(minScale, maxScale);
        shapeSettings.noiseLayers = GenerateNoiseLayers();

        newPlanet.shapeSettings = shapeSettings;
        newPlanet.Generate();

        planets.Add(newPlanet);

        int coordX = Random.Range(minXZ, maxXZ);
        while (coordX < -2500 && coordX > 2500)
        {
            coordX = Random.Range(minXZ, maxXZ);
        }
        int coordZ = Random.Range(minXZ, maxXZ);
        while (coordZ < -2500 && coordZ > 2500)
        {
            coordZ = Random.Range(minXZ, maxXZ);
        }
        int coordY = Random.Range(minY, maxY);

        planet.transform.position = new Vector3(coordX, coordY, coordZ);
    }

    PlanetShapeSettings.NoiseLayer[] GenerateNoiseLayers()
    {
        PlanetShapeSettings.NoiseLayer[] noiseLayers = new PlanetShapeSettings.NoiseLayer[1];
        noiseLayers[0] = new PlanetShapeSettings.NoiseLayer
        {
            enabled = true,
            useFirstLayerAsMask = false,
            noiseSettings = new NoiseSettings
            {
                filterType = NoiseSettings.FilterType.Simple,
                simpleNoiseSettings = new NoiseSettings.SimpleNoiseSettings
                {
                    strength = Random.Range(0.05f, 0.1f),
                    numLayers = Random.Range(1, 8),
                    baseRoughness = Random.Range(0.5f, 1.5f),
                    roughness = Random.Range(1.5f, 3.0f),
                    persistence = Random.Range(0.4f, 0.6f),
                    center = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f)),
                    minValue = Random.Range(0.1f, 2.0f)
                }
            }
        };
        return noiseLayers;
    }

    Moon GenerateMoon(Planet planet)
    {
        GameObject moon = Instantiate(moonPrefab);
        Moon newMoon = moon.GetComponent<Moon>();
        newMoon.Generate();

        float moonDistance = Random.Range(planet.transform.localScale.x, planet.transform.localScale.x * 2);
        moon.transform.position = planet.transform.position + new Vector3(moonDistance , moonDistance , moonDistance);

        return newMoon;
    }
}
