using System.Collections;
using UnityEngine;

public class PICKUPS : MonoBehaviour
{
    float finalRotSpd;
    [SerializeField] float RotSpeed;
    [SerializeField] float PickupRotSpd;
    [SerializeField] float FloatUpSpd;
    [SerializeField] float DownScaleSpd;
    [SerializeField] AudioClip sound;
    AudioSource AS;
    GameObject gm;
    ENTITIES nt;
    float rspd;
    Vector3 eular;
    bool destroy;
    public int id;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AS = GetComponent<AudioSource>();
        gm = GameObject.Find("Game_Manager");
        nt = gm.GetComponent<ENTITIES>();
    }

    // Update is called once per frame
    void Update()
    {
        rspd = Mathf.Lerp(rspd, RotSpeed, 4f * Time.deltaTime);
        eular.y += rspd * Time.deltaTime;
        transform.rotation = Quaternion.Euler(eular);
        if (destroy)
        { transform.position = new Vector3(transform.position.x, transform.position.y + (FloatUpSpd * Time.deltaTime), transform.position.z);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, DownScaleSpd * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            rspd = PickupRotSpd;
            if (!destroy)
            {
                AS.PlayOneShot(sound);
            }
           StartCoroutine( Destroy());
        }
    }

    IEnumerator Destroy()
    {
        destroy = true;
        yield return new WaitForSeconds(1f);
        nt.NotifyPickup(id);
        GameObject.Destroy(gameObject);
    }
}
