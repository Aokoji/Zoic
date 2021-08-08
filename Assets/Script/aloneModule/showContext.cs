using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showContext : MonoBehaviour
{
    public GameObject showObj;
    private void Start()
    {
        if (showObj != null) showObj.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("innn");
        if (collision.tag == "Player")
        {
            showObj.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            showObj.SetActive(false);
        }
    }
}
