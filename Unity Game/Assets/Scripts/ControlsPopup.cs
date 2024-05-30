using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsPopup : MonoBehaviour
{


    [Header("Control Popup")]
    [SerializeField] private GameObject ControlPopupPanel;

    // Start is called before the first frame update
    void Start()
    {
        ControlPopupPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            ControlPopupPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ControlPopupPanel.SetActive(false);
        }
    }
}
