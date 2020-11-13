using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UINavigation : MonoBehaviour
{
    public enum Direction
    {
        HORIZONTAL,
        VERTICAL
    }

    public static UINavigation instance;
    private bool activeNavigate;
    private int navigateIndex;
    [SerializeField] private NavigationData[] navigationData;
    
    private Joystick joystick;
    private bool flag;
    private int currentIndexButton = 0;
    private Button currentButton;
    [SerializeField] private Button acceptButton;

    private void Start()
    {
        instance = this;
        joystick = FindObjectOfType<Joystick>();
        
        acceptButton.onClick.AddListener(() => {
            AcceptButton();
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (activeNavigate) Navigate();
    }

    public void ActiveNavigation(bool flag, string navigation = null)
    {
        activeNavigate = flag;

        if (flag)
        {
            navigateIndex = navigationData.First(e => e.navigationName == navigation).indexId;
            currentIndexButton = 0;
            currentButton = navigationData[navigateIndex].buttonRegister[currentIndexButton];
            currentButton.image.color = Color.red;
        }
    }

    void Navigate()
    {
        switch (navigationData[navigateIndex].directionNavigation)
        {
            case Direction.HORIZONTAL:
                HorizontalNavigation();
                break;
            case Direction.VERTICAL:
                VerticalNavigation();
                break;
        }
    }

    void HorizontalNavigation() 
    { 
        if (joystick.Horizontal > 0.5f && !flag)
        {
            flag = true;
            currentButton.image.color = Color.white;
            GetNextButton();
        } else if (joystick.Horizontal < -0.5f && !flag)
        {
            flag = true;
            currentButton.image.color = Color.white;
            GetPrevButton();
        } else if (joystick.Horizontal == 0 && flag)
        {
            flag = false;
        }
    }

    void VerticalNavigation()
    {
        if (joystick.Vertical < -0.5f && !flag)
        {
            flag = true;
            currentButton.image.color = Color.white;
            GetNextButton();
        }
        else if (joystick.Vertical > 0.5f && !flag)
        {
            flag = true;
            currentButton.image.color = Color.white;
            GetPrevButton();
        }
        else if (joystick.Vertical == 0 && flag)
        {
            flag = false;
        }
    }

    void GetNextButton()
    {
        currentIndexButton++;

        if (currentIndexButton >= navigationData[navigateIndex].buttonRegister.Length) currentIndexButton = 0;
        currentButton = navigationData[navigateIndex].buttonRegister[currentIndexButton];
        currentButton.image.color = Color.red;
    }

    void GetPrevButton()
    {
        currentIndexButton--;

        if (currentIndexButton < 0) currentIndexButton = navigationData[navigateIndex].buttonRegister.Length - 1;
        currentButton = navigationData[navigateIndex].buttonRegister[currentIndexButton];
        currentButton.image.color = Color.red;
    }

    void AcceptButton()
    {
        if (activeNavigate) currentButton.onClick.Invoke();
    }
}

[System.Serializable]
public class NavigationData
{
    public string navigationName;
    public int indexId;
    public Button[] buttonRegister;
    public UINavigation.Direction directionNavigation;
}