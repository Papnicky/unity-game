using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsPopup : MonoBehaviour
{
    private bool playerInRange;

    [Header("Control Popup")]
    [SerializeField] private GameObject ControlPopupPanel;

    // Start is called before the first frame update
    void Start()
    {
        playerInRange = false;
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
