using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Sprite[] images; 
    public Image targetImage; 
    public GameObject panel; 
    public Image radialSlider; 

    public float fillSpeed = 1f; 

    private int currentIndex = 0;
    private bool isFilling = false; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && panel.activeSelf && !isFilling)
        {
            currentIndex++;

            if (currentIndex < images.Length)
            {
                targetImage.sprite = images[currentIndex];
            }
            else
            {
                panel.SetActive(false);
            }
        }

        if (Input.GetKey(KeyCode.Space) && panel.activeSelf)
        {
            isFilling = true;
            radialSlider.fillAmount += fillSpeed * Time.deltaTime;

            if (radialSlider.fillAmount >= 1f)
            {
                panel.SetActive(false);
                isFilling = false;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space)) 
        {
            isFilling = false;
            radialSlider.fillAmount = 0f;
        }
    }
}


