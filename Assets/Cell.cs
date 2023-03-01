using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Cell : MonoBehaviour
{
    public string DNA;
    [SerializeField]private TextAsset functionsInfoFile;
    private Rigidbody2D rb;
    public string[] dna;
    public char sectionChar = '|';
    public int energy = 100,codeCount = 11,atackDamage;
    [SerializeField]private float speed,minDistanceToHit,speedReaction = 1;
    [SerializeField]private GameObject eyePoint,cell,slider, slider1,enemy;
    private bool collisionEat;
    private Color oldColor;
    public FunctionInfoList myFunctionInfoList = new FunctionInfoList();
    private int mutation—hance;

    [System.Serializable]
    public class FunctionInfo
    {
        public string function;
        public string tupe;
    }
    [System.Serializable]
    public class FunctionInfoList
    {
        public FunctionInfo[] functionInfo;
    }

    private void Start()
    {
        oldColor = GetComponent<SpriteRenderer>().color;
        StartCoroutine(CheckAll());
        myFunctionInfoList = JsonUtility.FromJson<FunctionInfoList>(functionsInfoFile.text);
        rb = GetComponent<Rigidbody2D>();
        slider = GameObject.Find("Slider of speed");
        slider.GetComponent<Slider>().onValueChanged.AddListener(delegate {OnSpeedChanged();});
        slider1 = GameObject.Find("Slider of mutation");
        slider1.GetComponent<Slider>().onValueChanged.AddListener(delegate { OnMutationChanged(); });
        //DNA = "OnSeeWall 6,OnSeeEnemy 6,OnSeeChilld 1,OnNothing 8";
    }

    private void OnMutationChanged()
    {
        mutation—hance = (int)(slider1.GetComponent<Slider>().value * 100f);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Eat")
        {
            energy += 20;
            Destroy(coll.gameObject);
        }
    }
    private void CodeToFunction(int code)
    {
        switch(code)
        {
            case 0:
            break;
            case 1:
                transform.Rotate(0,0,45);
                energy--;
            break;
            case 2:
                transform.Rotate(0,0,1);
            break;
            case 3:
                transform.Rotate(0,0,90);
                energy--;
            break;
            case 4:
                transform.Rotate(0,0,181);
                energy--;
            break;
            case 5:
                transform.Rotate(0,0,15);
            break;
            case 6:
                if(energy > 50)
                {
                    GameObject child = Instantiate(cell, transform.position + new Vector3(0,-0.5f,0),Quaternion.identity);
                    string newDna = GetModyfyDNA(); 
                    child.GetComponent<Cell>().DNA = newDna;
                    child.GetComponent<Cell>().energy = energy/2;
                    if(DNA != newDna)
                    {
                        child.GetComponent<SpriteRenderer>().color = oldColor + new Color(UnityEngine.Random.Range(0,0.1f),UnityEngine.Random.Range(0,0.1f),UnityEngine.Random.Range(0,0.1f),0);
                    }
                    energy/=2;
                }
                else
                {
                    transform.Rotate(0,0,90);
                    energy--;
                }
                
            break;
            case 7:
                rb.velocity = transform.up * (-speed - 1f);
            break;
            case 8:
                if(speed > 0)
                {
                    rb.velocity = transform.up * speed;
                    energy--;
                }
            break;
            case 9:
                //atack
                if (enemy) 
                {
                    enemy.GetComponent<Cell>().GetAtack(atackDamage);
                    energy--;
                    enemy = null;
                }
            break;
            case 10:
                
            break;
            case 11:
                
            break;
        }
    }
    public int GetAtack(int damage)
    {
        if(energy - damage > 0)
        {
            energy -= damage;
            StartCoroutine(SetRed(0.2f, 1));
            return damage;
        }
        else 
        {
            StartCoroutine(SetRed(0.2f, 1));
            Destroy(gameObject,0.2f);
            return energy;
        }
    }
    private IEnumerator SetRed(float waitTime, float colorIntensity)
    {
        oldColor = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(oldColor.r, oldColor.g - colorIntensity, oldColor.b - colorIntensity);
        yield return new WaitForSeconds(waitTime);
        GetComponent<SpriteRenderer>().color = new Color(oldColor.r, oldColor.g, oldColor.b);
    }
    private string GetModyfyDNA()
    {
        ///*if()
        //Need add chenging DNA
        string newDNA = DNA; 
       
        for(int j = 0; j < dna.Length; j++)
        { 
            if(UnityEngine.Random.Range(1, 100) <= mutation—hance)
            {   
                string function = "";
                string[] newdna = dna;
                float value = 0;
                for(int i = 0; i < myFunctionInfoList.functionInfo.Length; i++)
                {
                    if(dna[j].Contains(myFunctionInfoList.functionInfo[i].function))
                    {       
                        function = myFunctionInfoList.functionInfo[i].function;
                        value = float.Parse(dna[j].Replace(function, ""));
                        int rnd = UnityEngine.Random.Range(0, 2);
                        string tupe = myFunctionInfoList.functionInfo[i].tupe;

                        if (UnityEngine.Random.Range(0, 40) == 1)
                        {
                            if (tupe == "TaskNumber")
                                value = UnityEngine.Random.Range(0, codeCount);
                            else if (tupe == "FloatValue" || tupe == "DeciFloatValue")
                                value += (rnd == 1 ? UnityEngine.Random.Range(0.1f, 1f) : UnityEngine.Random.Range(-0.1f, -1f));
                            else if (tupe == "IntValue")
                                value += (rnd == 1 ? UnityEngine.Random.Range(1, 50) : UnityEngine.Random.Range(-1,-50));
                            Debug.Log("hura hura hura hura hura hura!!!!!!");
                        }
                        else
                        {
                            if (tupe == "TaskNumber")
                                value += (rnd == 1 ? (value < codeCount ? 1 : -1) : (value > 0 ? -1 : 1));
                            else if (tupe == "FloatValue" || tupe == "DeciFloatValue")
                            {
                                value += (rnd == 1 ? 0.1f : -0.1f);
                            }
                            else if (tupe == "IntValue")
                                value += (rnd == 1 ? 1 : 2);
                        }
                        newdna[j] = function + value.ToString();
                    }
                }   
                newDNA = String.Join(sectionChar, newdna);
                Debug.Log(newDNA);
            }
        }
        return newDNA;

    }
    private void Update()
    {
        if(DNA != "")
            dna = DNA.Replace(" ","").Split(sectionChar);
    }
    public void OnSpeedChanged()
    {
        speedReaction = slider.GetComponent<Slider>().value;
    }
    IEnumerator CheckAll()
    {
        while(true)
        {
            yield return new WaitForSeconds(speedReaction);
            if(energy <= 0)
                Destroy(gameObject);
            RaycastHit2D hit = Physics2D.Raycast(eyePoint.transform.position,transform.up);
            Vector2 hitVector = new Vector2(hit.point.x, hit.point.y);
            float distanceTohit = Vector2.Distance(hitVector, transform.position);
            foreach(string func in dna)
            {
                if(distanceTohit < minDistanceToHit)
                {
                    if(hit.collider.gameObject.tag == "Child")
                    {
                        int code = 0;
                        enemy = hit.collider.gameObject;
                        if (func.Contains("OnSeeChild") && DNA == hit.collider.gameObject.GetComponent<Cell>().DNA)
                        {
                            int.TryParse(func.Replace("OnSeeChild", ""), out code);
                            CodeToFunction(code);
                        }
                        else if (func.Contains("OnSeeEnemy") && DNA != hit.collider.gameObject.GetComponent<Cell>().DNA)
                        {
                            int.TryParse(func.Replace("OnSeeEnemy", ""), out code);
                            CodeToFunction(code);
                        } 
                        
                    }
                    if(func.Contains("OnSeeWall") && hit.collider.gameObject.tag == "Wall")
                    {
                        enemy = null;
                        rb.velocity = Vector3.zero;
                        int code;
                        int.TryParse(func.Replace("OnSeeWall",""),out code);
                        CodeToFunction(code);
                        
                    }
                    if(func.Contains("OnSeeEat"))
                    {
                        enemy = null;
                        int code;
                        int.TryParse(func.Replace("OnSeeEat",""),out code);
                        CodeToFunction(code);
                    }
                }
                else
                {
                    if(func.Contains("OnNothing"))
                    {
                        enemy = null;
                        int code;
                        int.TryParse(func.Replace("OnNothing",""),out code);
                        CodeToFunction(code);
                    }
                }
                if(func.Contains("Speed"))
                {
                    float value;
                    value = float.Parse(func.Replace("Speed",""));
                    speed = value + 5 * (1-speedReaction);
                }
                if (func.Contains("Damage"))
                {
                    int value;
                    value = int.Parse(func.Replace("Damage", ""));
                    atackDamage = value;
                }
                if (func.Contains("MinDistanceToHit"))
                {
                    float value;
                    value = float.Parse(func.Replace("MinDistanceToHit",""));
                    minDistanceToHit = value;
                }
                if (func.Contains("Size"))
                {
                    float value;
                    value = float.Parse(func.Replace("Size", ""));
                    transform.localScale = new Vector3(value, value, 1);
                }

            }
            energy -= 1;    
        }
        
    }
}
