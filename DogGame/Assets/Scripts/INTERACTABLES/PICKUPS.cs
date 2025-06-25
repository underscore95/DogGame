using System.Collections;
using UnityEngine;

public class PICKUPS : MonoBehaviour
{
    float finalRotSpd;
    [SerializeField] float RotSpeed;
    [SerializeField] float PickupRotSpd;
    [SerializeField] float FloatUpSpd;
    GameObject gm;
    ENTITIES nt;
    float rspd;
    Vector3 eular;
    bool destroy;
    public int id;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        { transform.position = new Vector3(transform.position.x, transform.position.y + (FloatUpSpd * Time.deltaTime), transform.position.z); }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Player")
        {
            rspd = PickupRotSpd;
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
