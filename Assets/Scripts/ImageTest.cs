using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageTest : MonoBehaviour
{
	public Text title, creatorInput, description, yearInput, tagsInput;
	public Image image;

	private const string imagePath = "Assets\\Menu Pictures\\image_spaceflight.jpg";

	void Start()
	{
		byte[] bytes = File.ReadAllBytes(imagePath);
		Texture2D texture = new Texture2D(0, 0);
		texture.LoadImage(bytes);

		Rect rect = new Rect(0, 0, texture.width, texture.height);

		image.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
	}
}
