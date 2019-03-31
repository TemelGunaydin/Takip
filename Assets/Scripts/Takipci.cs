using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Takipci : MonoBehaviour
{
    public static event System.Action Detected;

    public Transform yol;
    
    [Header("Player Özellikleri")]
    public float speed = 5f;
    public float turnSpeed = 20f;
    public float gorusMesafesi = 12f;
    public LayerMask layer;
    public Light light;

    private int j = 1;
    private Animator anim;  
    private Color color;
    private float lightAngle;
    private GameObject player;
    private Player player_2;
    private AudioSource audio;


   

    private void Start()
    {

        audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();        
        color = light.color;
        lightAngle = light.spotAngle;
        player = GameObject.FindGameObjectWithTag("Player");
        player_2 = player.GetComponent<Player>();

        Vector3[] duraklar = new Vector3[yol.childCount];

        for (int i = 0; i < duraklar.Length; i++)
        {
            duraklar[i] = yol.GetChild(i).position;
            duraklar[i] = new Vector3(duraklar[i].x, transform.position.y, duraklar[i].z);
        }

        StartCoroutine(YolTakip(duraklar));


    }
    private void Update()
    {
       if(KarakterYakalandıMı())
        {
            light.color = Color.red;
            Siren();
            player_2.Dead();
           
        }
      
      

    }

    private void Siren()
    {
        audio.Play();
    }
    

    bool KarakterYakalandıMı()
    {
        RaycastHit hit;
        bool detect = false;

        if(Vector3.Distance(transform.position, player.transform.position) < gorusMesafesi)
        {
            Vector3 distance = (player.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, distance);
            if(angle < lightAngle /2)
            {
                if(Physics.Raycast(transform.position, player.transform.position,out hit,100,layer))
                {
                    detect = false;                   

                }

                else
                {                    
                    detect = true;
                }
            }           
        }

        return detect;
    }




    IEnumerator KarakterDonus(Vector3 hedef)
    {
            
            anim.SetBool("Walk", false);
            Vector3 direction = (hedef - transform.position).normalized;
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, angle)) > 0.2f)
            {
                float newAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, angle, turnSpeed * Time.deltaTime);
                transform.eulerAngles = Vector3.up * newAngle;

                yield return null;
            }
            anim.SetBool("Walk", true);
        
    }


    IEnumerator YolTakip(Vector3[] duraklar)
    {
       
            anim.SetBool("Walk", true);
            transform.position = duraklar[0];
            Vector3 hedef = duraklar[j];
            transform.LookAt(hedef);

            while (true)
            {

                for (int i = 0; i <= duraklar.Length; i++)
                {

                    Vector3 moveAmount = (hedef - transform.position).normalized * speed * Time.deltaTime;
                    transform.Translate(moveAmount, Space.World);

                    if (Vector3.Distance(hedef, transform.position) <= 0.2f)
                    {
                        j++;

                        if (j == duraklar.Length)
                        {
                            j = 0;
                        }

                        hedef = duraklar[j];
                        yield return new WaitForSeconds(0.5f);
                        yield return StartCoroutine(KarakterDonus(hedef));

                    }

                    yield return null;

                }


            }

        

    }



    
}
