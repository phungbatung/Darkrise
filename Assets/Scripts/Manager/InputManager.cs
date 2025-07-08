using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public bool isRightButtonPress { get; set; }
    public bool isLeftButtonPress { get; set; }
    public float horizontalInput { get; set; }

    public bool isUpButtonPress { get; set; }
    public float verticalInput { get; set; }

    private void Awake()
    {
        //skillSlots = skillSlotParent.GetComponentsInChildren<SkillSlot>().ToList();
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        isRightButtonPress = false;
        isLeftButtonPress = false;
        horizontalInput = 0f;

        isUpButtonPress = false;
        verticalInput = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //
        EditorInputCheck();

        HorizontalInputCheck();
        VerticalInputCheck();
    }

    private void EditorInputCheck()
    {
        if (Input.GetKeyDown(KeyCode.A))
            isLeftButtonPress = true;
        if (Input.GetKeyUp(KeyCode.A))
            isLeftButtonPress = false;

        if (Input.GetKeyDown(KeyCode.D))
            isRightButtonPress = true;
        if (Input.GetKeyUp(KeyCode.D))
            isRightButtonPress = false;


        if (Input.GetKeyDown(KeyCode.Space))
            isUpButtonPress = true;
        if (Input.GetKeyUp(KeyCode.Space))
            isUpButtonPress = false;
    }

    private void HorizontalInputCheck()
    {
        if (!isLeftButtonPress && !isRightButtonPress)
            horizontalInput = 0f;
        else if (isLeftButtonPress && !isRightButtonPress)
            horizontalInput = -1f;
        else if (!isLeftButtonPress && isRightButtonPress)
            horizontalInput = 1f;
    }

    private void VerticalInputCheck()
    {
        verticalInput = isUpButtonPress ? 1 : 0;
    }
}
