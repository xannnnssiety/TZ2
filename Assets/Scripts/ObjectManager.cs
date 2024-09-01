using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();
    public GameObject controlPanel;
    public Button changeColorButton;
    public Button hideAllButton;
    public Slider transparencySlider;
    public Toggle toggleAllVisibility;
    public Toggle selectAllToggle;
    public CameraController cameraController;

    public Transform objectListParent;
    public GameObject objectListItemPrefab;

    private List<GameObject> selectedObjects = new List<GameObject>();

    void Start()
    {
        transparencySlider.value = 1.0f;
        selectAllToggle.isOn = false;
        toggleAllVisibility.isOn = true;

        hideAllButton.onClick.AddListener(ToggleControlPanel);
        changeColorButton.onClick.AddListener(ChangeColors);
        transparencySlider.onValueChanged.AddListener(ChangeTransparency);
        toggleAllVisibility.onValueChanged.AddListener(SetAllObjectsVisibility);
        selectAllToggle.onValueChanged.AddListener(ToggleSelectAllObjects);

        CreateObjectList();
    }

    void Update()
    {
        if (selectedObjects.Count == 1)
        {
            cameraController.FocusOnObject(selectedObjects[0].transform);
        }
        else
        {
            cameraController.ResetCameraPosition();
        }
    }


    public bool IsMultipleObjectsSelected()
    {
        return selectedObjects.Count > 1;
    }

    void ToggleControlPanel()
    {
        controlPanel.SetActive(!controlPanel.activeSelf);
    }

    void ChangeColors()
    {
        foreach (GameObject obj in selectedObjects)
        {
            if (obj.TryGetComponent<Renderer>(out Renderer renderer))
            {
                renderer.material.color = new Color(Random.value, Random.value, Random.value);
            }
        }
    }

    void ChangeTransparency(float value)
    {
        foreach (GameObject obj in selectedObjects)
        {
            if (obj.TryGetComponent<Renderer>(out Renderer renderer))
            {
                Color color = renderer.material.color;
                color.a = value;
                renderer.material.color = color;

                renderer.enabled = value > 0;
            }
        }
    }

    void SetAllObjectsVisibility(bool isVisible)
    {
        foreach (GameObject obj in objects)
        {
            if (obj.TryGetComponent<Renderer>(out Renderer renderer))
            {
                renderer.enabled = isVisible;
            }
        }
    }

    void ToggleSelectAllObjects(bool isOn)
    {
        if (isOn)
        {
            selectedObjects.Clear();
            foreach (GameObject obj in objects)
            {
                selectedObjects.Add(obj);
                UpdateListItemToggle(obj, true);
            }
        }
        else
        {
            selectedObjects.Clear();
            foreach (GameObject obj in objects)
            {
                UpdateListItemToggle(obj, false);
            }
        }
    }

    void UpdateListItemToggle(GameObject obj, bool isSelected)
    {
        foreach (Transform child in objectListParent)
        {
            Toggle toggle = child.GetComponentInChildren<Toggle>();
            if (toggle != null && child.name == obj.name)
            {
                toggle.isOn = isSelected;
            }
        }
    }

    void CreateObjectList()
    {
        foreach (GameObject obj in objects)
        {
            GameObject listItem = Instantiate(objectListItemPrefab, objectListParent);
            listItem.name = obj.name;
            Text itemText = listItem.GetComponentInChildren<Text>();
            itemText.text = obj.name;

            Toggle selectionToggle = listItem.transform.Find("SelectionToggle").GetComponent<Toggle>();
            Toggle visibilityToggle = listItem.transform.Find("VisibilityToggle").GetComponent<Toggle>();

            selectionToggle.isOn = false;
            visibilityToggle.isOn = true;

            selectionToggle.onValueChanged.AddListener((bool isOn) =>
            {
                if (isOn)
                {
                    if (!selectedObjects.Contains(obj))
                        selectedObjects.Add(obj);
                }
                else
                {
                    if (selectedObjects.Contains(obj))
                        selectedObjects.Remove(obj);
                }
            });

            visibilityToggle.onValueChanged.AddListener((bool isVisible) =>
            {
                if (obj.TryGetComponent<Renderer>(out Renderer renderer))
                {
                    renderer.enabled = isVisible;
                }
            });
        }
    }
}
