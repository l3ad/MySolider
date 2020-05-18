using System.Collections.Generic;
using System;
using UnityEngine;

public class NeuralNetwork 
{
    //varibale
    private int[] layers; //layers
    private float[][] neurons; //neuron matix
    private float[][][] weights; //weight matrix
    private float fitness; //fitness of the network

    //constroctors
    public NeuralNetwork(int[] layers)
    {
        InitLayers(layers);
        InitNeurons();
        InitWeights();
    }
    public NeuralNetwork(int[] layers,float[] weight)
    {
        InitLayers(layers);
        InitNeurons();
        InitWeights();
        SetWeights(weight);
    }
    public NeuralNetwork(NeuralNetwork copyNetwork)
    {
        InitLayers(copyNetwork.layers);
        InitNeurons();
        InitWeights();
        CopyWeights(copyNetwork.weights);
    }
    public NeuralNetwork(NeuralNetwork parent1, NeuralNetwork parent2)
    {
        InitLayers(parent1.layers);
        InitNeurons();
        InitWeights();
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    if (UnityEngine.Random.Range(0.01f, 100f) < 5)
                        Mutate(i,j, k, parent1.GetWeight(i, j, k), parent2.GetWeight(i, j, k));
                    else
                    {
                        if(UnityEngine.Random.Range(0, 100) < 50)
                            SetWeight(i, j, k, parent1.GetWeight(i, j, k));
                        else
                            SetWeight(i, j, k, parent2.GetWeight(i, j, k));
                    }
                }
            }
        }
    }
    
    //inits
    private void InitLayers(int[] layer)
    {
        this.layers = new int[layer.Length];
        for (int i = 0; i < layer.Length; i++)
        {
            this.layers[i] = layer[i];
        }
    }
    private void InitNeurons()
    {
        //Neuron Initilization
        List<float[]> neuronsList = new List<float[]>();

        for (int i = 0; i < layers.Length; i++) //run through all layers
        {
            neuronsList.Add(new float[layers[i]]); //add layer to neuron list
        }

        neurons = neuronsList.ToArray(); //convert list to array
    }
    private void InitWeights()
    {

        List<float[][]> weightsList = new List<float[][]>(); //weights list which will later will converted into a weights 3D array

        //itterate over all neurons that have a weight connection
        for (int i = 1; i < layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>(); //layer weight list for this current layer (will be converted to 2D array)

            int neuronsInPreviousLayer = layers[i - 1];

            //itterate over all neurons in this current layer
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer]; //neruons weights

                //itterate over all neurons in the previous layer and set the weights randomly between 0.5f and -0.5
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    //give random weights to neuron weights
                    neuronWeights[k] = UnityEngine.Random.Range(-10f, 10f);
                }

                layerWeightsList.Add(neuronWeights); //add neuron weights of this current layer to layer weights
            }

            weightsList.Add(layerWeightsList.ToArray()); //add this layers weights converted into 2D array into weights list
        }

        weights = weightsList.ToArray(); //convert to 3D array
    }

    public float[] FeedForward(float[] inputs)
    {
        //Add inputs to the neuron matrix
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }

        //itterate over all neurons and compute feedforward values 
        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0f;

                for (int k = 0; k < neurons[i - 1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k]; //sum off all weights connections of this neuron weight their values in previous layer
                }

                neurons[i][j] = ActivationOne(value); //Hyperbolic tangent activation
            }
        }

        return neurons[neurons.Length - 1]; //return output layer
    }
    //mutate this
    public void Mutate()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];

                    //mutate weight value 
                    float randomNumber = UnityEngine.Random.Range(0f, 100f);

                    if (randomNumber <= 2f)
                    { //if 1
                      //flip sign of weight
                        weight *= -1f;
                    }
                    else if (randomNumber <= 4f)
                    { //if 2
                      //pick random weight between -1 and 1
                        weight = UnityEngine.Random.Range(-10, 10f);
                    }
                    else if (randomNumber <= 6f)
                    { //if 3
                      //randomly increase by 0% to 100%
                        float factor = UnityEngine.Random.Range(0f, 1f) + 1f;
                        weight *= factor;
                    }
                    else if (randomNumber <= 8f)
                    { //if 4
                      //randomly decrease by 0% to 100%
                        float factor = UnityEngine.Random.Range(0f, 1f);
                        weight *= factor;
                    }
                    weights[i][j][k] = weight;
                }
            }
        }
    }
    public void Mutate(int i,int j,int k,float parent1,float parent2)
    {
        float weight = weights[i][j][k];
        float randomNumber = UnityEngine.Random.Range(0f, 100f);
        if (randomNumber <= 14f)
        { 
            weight=parent1+parent2;
        }
        else if (randomNumber <= 28f)
        { 
            weight = UnityEngine.Random.Range(-10, 10f);
        }
        else if (randomNumber <= 42f)
        {
            if(UnityEngine.Random.Range(0, 10f)>5) weight = parent1 - parent2;
            else weight = parent2 - parent1;
        }
        else if (randomNumber <= 64f)
        {
            if (UnityEngine.Random.Range(0, 10f) > 5) weight = parent1 * (UnityEngine.Random.Range(0, 1f) + 1f);
            else weight = parent2 * (UnityEngine.Random.Range(0, 1f) + 1f);
        }
        else 
        {
            if (UnityEngine.Random.Range(0, 10f) > 5) weight = parent1 * UnityEngine.Random.Range(-1f, 1f);
            else weight = parent2 * UnityEngine.Random.Range(-1f, 1f) ;
        }
        weights[i][j][k] = weight;
    }
    //activation methods
    private float ActivationOne(float value)
    {
        return (float)Math.Tanh(value);
    }
    // get and seter weight
    public float GetWeight(int i, int j, int k)
    {
        return weights[i][j][k];
    }
    public void SetWeight(int i, int j, int k, float value)
    {
        weights[i][j][k] = value;
    }
    public void SetWeights(float[] w)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = w[WhichWeightss(i, j, k)];
                }
            }
        }
    }
    //fitness
    public void AddFitness(float fit)
    {
        fitness += fit;
    }
    public void SetFitness(float fit)
    {
        fitness = fit;
    }
    public float GetFitness()
    {
        return fitness;
    }
    //Get string for Save neuralnetwork
    public string GetStringWeights()
    {
        string w = "";
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    if(i== weights.Length-1 && j== weights[i].Length-1 && k== weights[i][j].Length-1) w = w + weights[i][j][k];
                    else w =w+weights[i][j][k]+",";
                }
            }
        }
        return w;
    }
    //Set weight in loaded String for new neuralnetwork
    private void CopyWeights(float[][][] copyWeights)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = copyWeights[i][j][k];
                }
            }
        }
    }
    public int WhichWeightss(int i, int j, int k)
    {
        int result=0;
        for (int a = 0; a < weights.Length; a++)
        {
            for (int b = 0; b < weights[a].Length; b++)
            {
                for (int c = 0; c < weights[a][b].Length; c++)
                {
                    result ++;
                }
            }
        }
        return result;
    }
}
