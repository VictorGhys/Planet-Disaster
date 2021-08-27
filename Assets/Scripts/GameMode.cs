using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MilkShake;
using UnityEngine;
using UnityEngine.UI;
using static Disaster.DisasterType;
using Random = UnityEngine.Random;

public class GameMode : MonoBehaviour
{
    [SerializeField] private float earthRadius = 20;
    [SerializeField] private Transform disasterPF;
    [SerializeField] private Transform earthParent;
    [SerializeField] private Texture2D landWaterTex;
    [SerializeField] private Camera camera;
    [SerializeField] private Slider populationSlider;
    [SerializeField] private float populationRegainRate;
    [SerializeField] private float disasterSpawnInterval;
    [SerializeField] private DialogueTrigger worsenDialogue;
    [SerializeField] private DialogueTrigger gameEndDialogue;
    [SerializeField] private DialogueTrigger gameOverDialogue;
    [SerializeField] private AudioSource matchPositiveSFX;
    [SerializeField] private AudioSource matchWrongSFX;
    [SerializeField] private AudioSource matchErrorSFX;
    [SerializeField] private ShakePreset errorShakePreset;
    [SerializeField] private Wave[] waves;
    [SerializeField] private int currentWave = 0;

    private Disaster selectedDisaster = null;
    private bool gameOver = false;
    private bool hasGameEnded = false;
    private float spawnTime;
    private int disastersToSpawnThisWave = 0;
    private int disastersSpawned = 0;
    private List<Disaster> disasters;
    private bool isPuzzleSolvable;

    private Dictionary<Disaster.DisasterType, List<Disaster.DisasterType>> disasterMatches =
        new Dictionary<Disaster.DisasterType, List<Disaster.DisasterType>> {
            { Flood, new List<Disaster.DisasterType>{Fire}},
            { Fire, new List<Disaster.DisasterType>{Earthquake, Winterstorm}},
            { Earthquake, new List<Disaster.DisasterType>{Flood, Tornado}},
            { Tornado, new List<Disaster.DisasterType>{Fire, Winterstorm}},
            { Winterstorm, new List<Disaster.DisasterType>{Flood, Fire}},
        };

    private Dictionary<Disaster.DisasterType, List<Disaster.DisasterType>> disasterWorseners =
        new Dictionary<Disaster.DisasterType, List<Disaster.DisasterType>> {
            { Flood, new List<Disaster.DisasterType>{Flood, Tornado}},
            { Fire, new List<Disaster.DisasterType>{Fire, Tornado}},
            { Earthquake, new List<Disaster.DisasterType>{Earthquake, Winterstorm}},
            { Tornado, new List<Disaster.DisasterType>{Tornado, Earthquake}},
            { Winterstorm, new List<Disaster.DisasterType>{Winterstorm, Tornado}},
        };

    private void CreateDisaster(Disaster.DisasterType disasterType = None)
    {
        Vector3 pos = GetRandomDisasterSpawnPos();
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, (pos / earthRadius));
        //Quaternion quarterTurn = Quaternion.Euler(90, 0, 0);
        Transform disasterGo = Instantiate(disasterPF, pos, rot, earthParent);
        Disaster disaster = disasterGo.GetComponent<Disaster>();
        if (disasterType == None)
        {
            disaster.SetDisasterType(GetRandomDisasterType());
        }
        else
        {
            disaster.SetDisasterType(disasterType);
        }
        disaster.GetComponent<Disaster>().slider = populationSlider;
        disasters.Add(disaster.GetComponent<Disaster>());
        isPuzzleSolvable = false;
    }

    private Disaster.DisasterType GetRandomDisasterType()
    {
        Array values = Enum.GetValues(typeof(Disaster.DisasterType));
        Disaster.DisasterType randomType = (Disaster.DisasterType)values.GetValue(Random.Range(1, values.Length));
        return randomType;
    }

    private Vector3 GetRandomDisasterSpawnPos()
    {
        Vector3 rand;
        Vector3 randPos;
        RaycastHit hit;
        int n = 0;
        while (n < 100)
        {
            rand = Random.onUnitSphere;
            randPos = rand * earthRadius;
            if (Physics.Raycast(randPos + rand, -rand, out hit, earthRadius))
            {
                Vector2 texCoord = hit.textureCoord;
                float sample = landWaterTex.GetPixelBilinear(texCoord.x, texCoord.y).a;
                if (sample < 0.5f)
                {
                    return randPos;
                }
            }
            n++;
        }
        Debug.Log("failed!");
        return Vector3.back;
    }

    // Start is called before the first frame update
    private void Start()
    {
        disasters = new List<Disaster>();
        waves[currentWave].startDialogue.TriggerDialogue();
        disastersToSpawnThisWave = waves[currentWave].amountOfDisastersToSpawn;
        disasterSpawnInterval = waves[currentWave].spawnInterval;
    }

    // Update is called once per frame
    private void Update()
    {
        //regain population
        if (populationSlider.value < populationSlider.maxValue)
        {
            if (populationSlider.value <= 0)
            {
                //game over restart wave
                gameOver = true;
            }
            populationSlider.value += populationRegainRate * populationSlider.maxValue * Time.deltaTime;
        }
        //spawning
        if (disastersSpawned < disastersToSpawnThisWave)
        {
            spawnTime += Time.deltaTime;
            if (spawnTime > disasterSpawnInterval)
            {
                spawnTime = 0;
                CreateDisaster();
                disastersSpawned++;
            }
        }
        else
        {
            //check if solved
            if (disasters.Count == 0 && !gameOver)
            {
                if (currentWave == waves.Length)
                {
                    //end the game
                    hasGameEnded = true;
                }
                else
                {
                    //next wave
                    NextWave();
                }
            }
            else
            {
                Solve();
            }
        }

        //click disasters to select them if an other is selected match them
        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0 && !gameOver)
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                //Debug.Log(hit.transform.gameObject.name);
                Disaster hitDisaster = hit.transform.gameObject.GetComponent<Disaster>();
                if (hitDisaster != null)
                {
                    if (selectedDisaster != null)
                    {
                        //match
                        //Debug.Log("match " + selectedDisaster.GetDisasterType() + " with " + hitDisaster.GetDisasterType());
                        Match(selectedDisaster, hitDisaster);
                        selectedDisaster.DisableOutline();
                        selectedDisaster = null;
                        //GetComponent<DialogueTrigger>().TriggerDialogue();
                    }
                    else
                    {
                        //select
                        selectedDisaster = hit.transform.gameObject.GetComponent<Disaster>();
                        selectedDisaster.EnableOutline();
                        //Debug.Log("selected " + selectedDisaster.GetDisasterType());
                    }
                }
                else
                {
                    //unselect
                    if (selectedDisaster)
                    {
                        selectedDisaster.DisableOutline();
                        selectedDisaster = null;
                    }
                }
            }
        }
    }

    private void NextWave()
    {
        if (currentWave < waves.Length - 1)
        {
            currentWave++;
            disastersToSpawnThisWave = waves[currentWave].amountOfDisastersToSpawn;
            disastersSpawned = 0;
            disasterSpawnInterval = waves[currentWave].spawnInterval;
            spawnTime = 0;
            populationSlider.value = populationSlider.maxValue;
            //dialogue
            waves[currentWave].startDialogue.TriggerDialogue();
        }
        else
        {
            Debug.Log("Game has ended");
            currentWave++;
            gameEndDialogue.TriggerDialogue();
        }
    }

    private void Solve()
    {
        if (isPuzzleSolvable)
            return;

        var disastersThatNeedToBeSolved = CheckSolvable();
        if (disastersThatNeedToBeSolved.Count == 0)
            return;

        //solve
        Debug.Log("puzzle needs to be solved!");
        //create for each of the disasters a new one that gets fixed by it
        foreach (var disaster in disastersThatNeedToBeSolved)
        {
            var fixes = disasterMatches[disaster.GetDisasterType()];
            int r = Random.Range(0, fixes.Count);
            CreateDisaster(fixes[r]);
        }
    }

    private List<Disaster> CheckSolvable()
    {
        List<Disaster> disastersCopy = new List<Disaster>(disasters);
        //check if each disaster fixes or gets fixed by an other disaster
        for (int i = 0; i < disastersCopy.Count; i++)
        {
            if (disastersCopy[i] == null)
                continue;

            var fixes = disasterMatches[disastersCopy[i].GetDisasterType()];
            foreach (var disasterType in fixes)
            {
                //var gotFixed = disastersCopy.FirstOrDefault(d => d.GetDisasterType() == disasterType);
                int gotFixedIndex = disastersCopy.FindIndex(d => d?.GetDisasterType() == disasterType);
                if (gotFixedIndex >= 0)
                {
                    //disastersCopy.Remove(gotFixed);
                    //disastersCopy.Remove(disastersCopy[i]);
                    disastersCopy[gotFixedIndex] = null;
                    disastersCopy[i] = null;
                    break;
                }
            }
        }

        disastersCopy.RemoveAll(d => d == null);
        //if each disaster gets fixed then the puzzle is solved
        if (disastersCopy.Count == 0)
        {
            isPuzzleSolvable = true;
            return disastersCopy;
        }

        return disastersCopy;
    }

    private void Match(Disaster disaster1, Disaster disaster2)
    {
        // can't match a disaster with itself
        if (disaster1 == disaster2)
        {
            return;
        }
        Disaster.DisasterType type1 = disaster1.GetDisasterType();
        Disaster.DisasterType type2 = disaster2.GetDisasterType();
        if (disasterMatches.ContainsKey(type1))
        {
            List<Disaster.DisasterType> matches = disasterMatches[type1];
            if (matches.Contains(type2))
            {
                //succesfull match
                //disaster1.Relocate(disaster2.transform);
                //disaster1.ResetSize();
                disasters.Remove(disaster1);
                disasters.Remove(disaster2);
                disaster2.Destroy();
                disaster1.Destroy();
                matchPositiveSFX.Play();
                isPuzzleSolvable = false;
            }
            else
            {
                // check for worsener
                List<Disaster.DisasterType> worseners = disasterWorseners[type1];
                if (worseners.Contains(type2))
                {
                    //succesfull worsen
                    disasters.Remove(disaster1);
                    disaster1.Destroy();
                    disaster2.size *= 2;
                    worsenDialogue.TriggerDialogue();
                    matchErrorSFX.Play();
                    //camera.transform.parent.GetComponent<Shaker>().Shake(errorShakePreset);
                    camera.GetComponent<Shaker>().Shake(errorShakePreset);
                    isPuzzleSolvable = false;
                }
                else
                {
                    //unsuccesfull match
                    matchWrongSFX.Play();
                }
            }
        }
    }

    public void RemoveDisaster(Disaster disaster)
    {
        disasters.Remove(disaster);
    }

    public bool GetIsGameOver()
    {
        return gameOver;
    }

    public bool GetHasGameEnded()
    {
        return hasGameEnded;
    }

    public void RestartWave()
    {
        currentWave--;
        NextWave();
        gameOver = false;

        //Array to hold all disasters obj
        Disaster[] allDisasters = new Disaster[disasters.Count];

        int i = 0;
        //Find all disaster obj and store to that array
        foreach (Disaster disaster in disasters)
        {
            allDisasters[i] = disaster;
            i += 1;
        }

        //Now destroy them
        foreach (Disaster disaster in allDisasters)
        {
            disaster.Destroy();
        }
    }
}