using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public ItemSlot[] itemSlots { get; private set; }
    public List<ItemInventory> inventoryItem => ItemManager.Instance.inventoryItems;
    protected int currentPage;
    protected int slotsPerPage =>itemSlots.Length;

    [SerializeField] private Button rightButton;
    [SerializeField] private Button leftButton;
    protected bool isRightButtonActive;
    protected bool isLeftButtonActive;
    protected Animator rightAnim;
    protected Animator leftAnim;
    [SerializeField] protected TextMeshProUGUI pageCountTMP;


    [SerializeField] protected Button sortButton;
    [SerializeField] protected TMP_Dropdown sortDropdown;

    protected List<string> sortOptionList;
    protected int sortOptionIndex;



    protected void Awake()
    {
        itemSlots = GetComponentsInChildren<ItemSlot>();
        rightButton.onClick.AddListener(RightButtonClick);
        leftButton.onClick.AddListener(LeftButtonClick);
        rightAnim = rightButton.GetComponent<Animator>();
        leftAnim = leftButton.GetComponent<Animator>();
        sortButton.onClick.AddListener(Sort);
        InitDropdown();
    }

    protected virtual void InitDropdown()
    {
        sortOptionList = new List<string>();
        sortOptionList.Add("Sort by item type");
        sortOptionList.Add("Sort by item quality");
        foreach (var sortOption in sortOptionList)
        {
            var option = new TMP_Dropdown.OptionData(sortOption);
            sortDropdown.options.Add(option);
        }
        sortDropdown.onValueChanged.AddListener(delegate { SelectSortOption(); });
    }

    protected virtual void SetUpButton()
    {
        if (currentPage == 1)
        {
            isLeftButtonActive = false;
            isRightButtonActive = true;
        }
        else if (currentPage == getTotalPage())
        {
            isLeftButtonActive = true;
            isRightButtonActive = false;
        }
        else
        {
            isLeftButtonActive = true;
            isRightButtonActive = true;
        }
        if (getTotalPage() <= 1)
        {
            isLeftButtonActive = false;
            isRightButtonActive = false;
        }
        rightAnim.SetBool("active", isRightButtonActive);
        leftAnim.SetBool("active", isLeftButtonActive);
    }

    public virtual void UpdateItemSlot()
    {
        int slotCount = currentPage != getTotalPage()?slotsPerPage:(getInventorySize() - 24*(currentPage-1));

        for (int i = 0; i < slotsPerPage; i++)
        {
            if(i<slotCount)
                itemSlots[i].gameObject.SetActive(true);
            else
                itemSlots[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < slotCount; i++)
        {
            itemSlots[i].SetItem(inventoryItem[(currentPage-1)*24 + i]);
        }
        pageCountTMP.text = $"{currentPage}/{getTotalPage()}";
        SetUpButton();
    }

    protected virtual void RightButtonClick()
    {
        if (!isRightButtonActive)
            return;
        currentPage++;
        UpdateItemSlot();
    }

    protected virtual void LeftButtonClick()
    {
        if (!isLeftButtonActive)
            return;
        currentPage--;
        UpdateItemSlot();
    }

    protected int getInventorySize() => ItemManager.Instance.inventorySize;

    protected int getTotalPage() => Mathf.CeilToInt(ItemManager.Instance.inventorySize * 1.0f / 24);

    protected virtual void Sort()
    {
        if (sortOptionIndex == 0)
            ItemManager.Instance.SortItemByItemType();
        else if (sortOptionIndex == 1)
            ItemManager.Instance.SortItemByItemQuality();
        UpdateItemSlot();
    }

    protected virtual void SelectSortOption()
    {
        sortOptionIndex = sortDropdown.value;
    }

    public virtual void SwitchToFirstPage()
    {
        currentPage = 1;
    }    
}