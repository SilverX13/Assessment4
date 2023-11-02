using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI roundStartCountdown;
    public GameObject pacStudent;

    private void Start()
    {
        StartCoroutine(StartRoundCountdown());
    }

    void Update()
    {
       
    }

    IEnumerator StartRoundCountdown()
    {
        PacStudentMovement pacStudentMovement = pacStudent.GetComponent<PacStudentMovement>();
        pacStudentMovement.enabled = false; 

       
        for (int i = 3; i > 0; i--)
        {
            roundStartCountdown.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        roundStartCountdown.text = "GO!";
        yield return new WaitForSeconds(1);

       
        pacStudentMovement.enabled = true;
        pacStudentMovement.StartGame(); 

        roundStartCountdown.gameObject.SetActive(false); 
    }
}
