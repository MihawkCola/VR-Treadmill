using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    private GameObject mykey;
    bool move;
    bool rotate;
    bool dissolve;
    public Vector3 rotation;
    private float oldDissolve;
    private float dissolveSpeed; 
    private float timer;
    public float rotationTime;
    private Material keyMaterial;
    [SerializeField]
    private keyColor Key_Color;
    public enum keyColor{
        Blue,
        Red,
        Green,
        Yellow,
        Purple
    }

    // Start is called before the first frame update
    void Start()
    {
        move = true;
        rotate = false;
        dissolve = false;
        timer = 0;;
        keyMaterial = gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //movment to lock (destroy later)
        if(move){
            transform.position += new Vector3(-1,0,0) * Time.deltaTime;
        }

        //rotation in lock
        if(rotate){
            transform.Rotate(rotation, Space.World);
            timer += Time.deltaTime;
            if(timer > rotationTime){
                rotate = false;
                timer = 0;
                dissolve = true;
            }
        }

        //dissolve key
        if(dissolve){
            oldDissolve = gameObject.transform.GetComponent<Renderer>().material.GetFloat("_DissolveAmount");
            gameObject.transform.GetComponent<Renderer>().material.SetFloat("_DissolveAmount", oldDissolve + (dissolveSpeed * Time.deltaTime));
            if(oldDissolve > 1){
                gameObject.active = false;
                dissolve = false;
            }
        }

        //change Color
        if(keyMaterial.GetColor("_KeyLockColor") != getCurrentColor()){
            keyMaterial.SetColor("_KeyLockColor", getCurrentColor());
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(getCurrentColor() == other.gameObject.GetComponent<LockController>().getCurrentColor()){
            gameObject.GetComponent<Collider>().enabled = false;
            dissolveSpeed = other.gameObject.GetComponent<LockController>().dissolveSpeed;
            move = false;
            rotate = true;
        }
    }

    public Color getCurrentColor(){
        if(Key_Color == keyColor.Red){
            return Color.red;
        }
        if(Key_Color == keyColor.Blue){
            return Color.blue;
        }
        if(Key_Color == keyColor.Green){
            return Color.green;
        }
        if(Key_Color == keyColor.Yellow){
            return Color.yellow;
        }
        if(Key_Color == keyColor.Purple){
            return Color.magenta;
        } 
        return Color.white;
    }
}