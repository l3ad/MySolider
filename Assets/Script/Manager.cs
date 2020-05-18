using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Manager : MonoBehaviour
{
    public Transform target;
    public GameObject playerPrefab;
    public int populationSize=32;
    List<GameObject> population;
    private readonly int[] layers = new int[] {6,5,2};

    public int generation = 0;

    float timeElaps;

    bool allDie=false;
    // input makan target 2 makan ma va 2 velocety
    // Start is called before the first frame update
    void Start()
    {
        population = new List<GameObject>();
        Time.timeScale = 10;
        if (generation == 0)
        {

            for(int i = 0; i < populationSize; i++)
            {
                GameObject g= Instantiate(playerPrefab, Vector3.zero,Quaternion.identity);
                g.GetComponent<Characters>().SetNewNetwork(layers);
                population.Add(g);
            }
            generation++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (generation != 0)
        {
            timeElaps += Time.deltaTime;
            if (AllDied())
            {
                allDie = true;
            }
            if (timeElaps > 30 || allDie)
            {
                BreedNet();
                generation++;
                allDie = false;
                timeElaps = 0;
            }
        }
    }
    public bool AllDied()
    {
        for(int i = 0; i < populationSize; i++)
        {
            if( !population[i].GetComponent<Characters>().done)
            {
                return false;
            }
        }
        return true;
    }

    public void BreedNet()
    {
        if (AllDied())
        {
            for (int i = 0; i < populationSize; i++)
            {
                if (!population[i].GetComponent<Characters>().done)
                {
                    population[i].GetComponent<Characters>().net.AddFitness(-50);
                }
            }
        }

        List<NeuralNetwork> networks = new List<NeuralNetwork>();
        List<NeuralNetwork> ofspring = new List<NeuralNetwork>();
        for(int i = 0; i < populationSize; i++)
        {
            networks.Add(population[i].GetComponent<Characters>().net);
        }
        for(int i = 0; i < populationSize; i++)
        {
            Destroy(population[i]);
        }
        population.Clear();
        networks.Sort();
        networks.Reverse();
        for(int i = 0; i < populationSize; i++)
        {
            Debug.Log(networks[i].GetFitness()+" ["+i+"]");
        }
        for(int i = 0 ; i < populationSize/8; i++)
        {
            ofspring.Add(new NeuralNetwork(networks[i], networks[i+1]));
            ofspring.Add(new NeuralNetwork(networks[i+1], networks[i]));
            ofspring.Add(new NeuralNetwork(networks[i], networks[i + 1]));
            ofspring.Add(new NeuralNetwork(networks[i + 1], networks[i]));
            ofspring.Add(new NeuralNetwork(networks[i], networks[i + 1]));
            ofspring.Add(new NeuralNetwork(networks[i + 1], networks[i]));
            ofspring.Add(new NeuralNetwork(networks[i], networks[i + 1]));
            ofspring.Add(new NeuralNetwork(networks[i + 1], networks[i]));
            ofspring.Add(new NeuralNetwork(networks[i], networks[i + 1]));
            ofspring.Add(new NeuralNetwork(networks[i + 1], networks[i]));
            ofspring.Add(new NeuralNetwork(networks[i], networks[i + 1]));
            ofspring.Add(new NeuralNetwork(networks[i + 1], networks[i]));
            ofspring.Add(new NeuralNetwork(networks[i], networks[i + 1]));
            ofspring.Add(new NeuralNetwork(networks[i + 1], networks[i]));
            ofspring.Add(new NeuralNetwork(networks[i], networks[i + 1]));
            ofspring.Add(new NeuralNetwork(networks[i + 1], networks[i]));
        }
        for(int i = 0; i < populationSize; i++)
        {
            GameObject g = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            g.GetComponent<Characters>().SetOfspringNetwork(ofspring[i]);
            population.Add(g);
        }

    }



    public NeuralNetwork LoadNeuralNetwork()
    {
        string path = Application.dataPath + "/Weights.txt";
        if (File.Exists(path))
        {
            var sr = File.OpenText(path);
            string line = sr.ReadLine();
            string[] w = line.Split(',');
            float[] wg = new float[w.Length];
            for(int i = 0; i < w.Length; i++)
                wg[i] = System.Convert.ToSingle(w[i]);
            sr.Close();
            return new NeuralNetwork(layers, wg);
        }
        else return null;
    }
    public void SaveNeuralNetwork(NeuralNetwork network)
    {
        string path = Application.dataPath + "/Weights.txt";
        var sr = File.CreateText(path);
        sr.WriteLine(network.GetStringWeights());
        sr.Close();
    }








}
