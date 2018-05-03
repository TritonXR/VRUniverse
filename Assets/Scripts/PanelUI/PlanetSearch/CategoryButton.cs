using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButton : MonoBehaviour, PointableObject {

	[SerializeField] private Image categoryIcon;
	[SerializeField] private Color defaultColor;
	[SerializeField] private Color highlightColor;
	[SerializeField] private Color selectedColor;

	[SerializeField] string categoryName;
	[SerializeField] public int count;
	[SerializeField] private Text info;

	// Use this for initialization
	void Start () {
		categoryIcon.color = defaultColor;

	}

	// Update is called once per frame
	void Update () {
		
	}

	public void PointerEnter()
	{
        CategoryManager manager = CategoryManager.GetInstance();
        if (!manager.CheckIfSelected(categoryName))
        {
            categoryIcon.color = highlightColor;
        }
	}

	public void PointerClick()
	{

		CategoryManager manager = CategoryManager.GetInstance();
        manager.ToggleSelected(categoryName);

        if (manager.CheckIfSelected(categoryName))
            categoryIcon.color = selectedColor;
        else
            categoryIcon.color = defaultColor;
    }

	public void PointerExit()
	{
		if (CategoryManager.GetInstance().CheckIfSelected(categoryName))
			categoryIcon.color = selectedColor;
		else
			categoryIcon.color = defaultColor;
	}

	public void Deselect(){
		categoryIcon.color = defaultColor;
	}

    public string GetCategory()
    {
		return categoryName;
    }

	public void setCount(int num)
	{
		this.count = num;
		info = GetComponentInChildren<Text> ();
		info.text = categoryName.ToUpper() + " ("+count+")";
	}
}
