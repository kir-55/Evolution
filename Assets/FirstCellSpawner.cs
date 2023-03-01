using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstCellSpawner : MonoBehaviour
{
    [SerializeField]private GameObject cell;
    private GameObject cell1;
    public string[] dna;
    private string DNA;
    private Cell.FunctionInfoList fil;
    private Cell.FunctionInfo[] fi;
    private char sectionChar;
    public int codeCount = 11;
    private bool redy = false;

    void Start()
    {
        cell1 = Instantiate(cell,transform.position,transform.rotation,transform);
        sectionChar = cell1.GetComponent<Cell>().sectionChar;
        
        DNA = cell1.GetComponent<Cell>().DNA;
        if(DNA != "")
            dna = DNA.Replace(" ","").Split(sectionChar);
    }
    private void Update()
    {
        if(!redy)
        {
            DNA = GetModyfyDNA();
            if (cell1 != null)
                cell1.GetComponent<Cell>().DNA = DNA;
            else
                Destroy(gameObject);
        }
    }
    private string GetModyfyDNA()
    {
        if (cell1 != null)
        {
            fil = cell1.GetComponent<Cell>().myFunctionInfoList;
            fi = fil.functionInfo;
            try
            {
                Debug.Log(fi[2].tupe);
                string newDNA = DNA;

                for (int j = 0; j < dna.Length; j++)
                {
                    string function = "";
                    string[] newdna = dna;
                    float value = 0;
                    for (int i = 0; i < fi.Length; i++)
                    {
                        if (dna[j].Contains(fi[i].function))
                        {
                            function = fi[i].function;
                            value = float.Parse(dna[j].Replace(function, ""));
                            int rnd = UnityEngine.Random.Range(0, codeCount);
                            int rnd2 = UnityEngine.Random.Range(0, 360);
                            float rnd1 = UnityEngine.Random.Range(0f, 5f);
                            float rnd3 = UnityEngine.Random.Range(0f, 1.7f);
                            string tupe = fi[i].tupe;

                            if (tupe == "TaskNumber")
                                value = rnd;
                            else if (tupe == "FloatValue")
                                value = rnd1;
                            else if (tupe == "IntValue")
                                value = rnd2;
                            else if (tupe == "DeciFloatValue")
                                value = rnd3;

                            newdna[j] = function + value.ToString();
                        }
                        newDNA = String.Join(sectionChar, newdna);
                    }
                }
                redy = true;
                return newDNA;
            }
            catch (InvalidCastException e)
            {
                return DNA;
            }
        }
        else 
            return DNA;
    } 
}
