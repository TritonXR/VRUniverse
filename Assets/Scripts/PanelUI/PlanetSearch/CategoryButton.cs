using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// used to allow users to toggle whether the search results select for a category or not
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

    // called when user starts pointing at button
    public void PointerEnter()
	{
        // highlight the planet if the category is not selected
        CategoryManager manager = CategoryManager.GetInstance();
        if (!manager.CheckIfSelected(categoryName))
        {
            categoryIcon.color = highlightColor;
        }
	}

    // called when user pulls trigger while pointing at this button
    public void PointerClick()
	{
        // toggle the category
		CategoryManager manager = CategoryManager.GetInstance();
        manager.ToggleSelected(categoryName);

        // override highlight for new state
        if (manager.CheckIfSelected(categoryName))
            categoryIcon.color = selectedColor;
        else
            categoryIcon.color = defaultColor;
    }

    // called when user stops pointing at button
    public void PointerExit()
	{
        // override highlight for new state
		if (CategoryManager.GetInstance().CheckIfSelected(categoryName))
			categoryIcon.color = selectedColor;
		else
			categoryIcon.color = defaultColor;
	}

    // force the button back to default color
	public void Deselect(){
		categoryIcon.color = defaultColor;
	}

    // get the category attached to this button
    public string GetCategory()
    {
		return categoryName;
    }

    // update the category text with the number of planets in the category
	public void SetCount(int num)
	{
		count = num;
		info = GetComponentInChildren<Text> ();
		info.text = categoryName.ToUpper() + " ("+count+")";
	}
}
