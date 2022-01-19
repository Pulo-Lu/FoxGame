using UnityEngine;

public class EnterDiglog : MonoBehaviour
{
    public GameObject Enterdialog;
    public Animator dialogAnim;

    private void Start()
    {
        dialogAnim = Enterdialog.gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Enterdialog.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            dialogAnim.Play("ExitDialog");

            Enterdialog.SetActive(false);
        }
    }
}
