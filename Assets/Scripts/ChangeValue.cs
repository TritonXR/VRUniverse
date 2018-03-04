using System.Collections;
using UnityEngine;

public class ChangeValue : MonoBehaviour {
//	public string name = "Vivace";
//	public int year = 2017;

//	public Color oricolor1 = Color.yellow;
//	public Color oricolor2 = Color.blue;
//	public Color basecolor;
//	public Color peakcolor;
//	public Color sludgecolor;
//	public float crackscale = 0;
//	public float crackwarping = 0;
//	public float waterlevel = 0;
//
//	private string property1 = "Base Color";
//	private string property2 = "Peak Color";
//	private string property3 = "Sludge Color";
//	private string property4 = "Cracks Scale";
//	private string property5 = "Cracks Scale";
//	private string property6 = "Water Level";
//	public Renderer rend;
//	void Start() {
//		rend = GetComponent<Renderer>();
//	}
//


	public void change(Renderer rend, string name, int year){
		Color oricolor1 = Color.yellow;
		Color oricolor2 = Color.blue;
	    Color basecolor;
	    Color peakcolor;
		Color sludgecolor;
	    float crackscale = 0;
		float crackwarping = 0;
		float waterlevel = 0;

		char[] name1 = name.ToCharArray ();
		int newname = 0;
		for (int i = 0; i < name.Length; i++) {
			newname += name1 [i];
		}

		//change color
		if (year % 3 == 0) {
			basecolor = new Color ((newname * year) % 255 /255.0f, (newname + year) % 255 /255.0f,(newname ^ year) % 255 /255.0f);
			peakcolor = new Color ((newname + year) % 255 /255.0f, oricolor1.g, oricolor1.b);
			sludgecolor = new Color (newname % 255/255.0f, oricolor2.g, oricolor2.b);
				
		} else if (year % 3 == 1) {
			basecolor = new Color ((newname ^ year) % 255 /255.0f, (newname * year) % 255/255.0f, (newname + year) % 255/255.0f);
			peakcolor = new Color (oricolor1.r, (newname + year) % 255 /255.0f, oricolor1.b);
			sludgecolor = new Color (oricolor2.r,  newname % 255/255.0f, oricolor2.b);

		} else {
			basecolor = new Color ((newname + year) % 255 /255.0f, (newname ^ year) % 255 /255.0f, (newname * year) % 255 /255.0f);
			peakcolor = new Color (oricolor1.r, oricolor1.g, (newname + year) % 255 /255.0f);
			sludgecolor = new Color (oricolor2.r, oricolor2.g,newname % 255/255.0f);
		}

		crackscale = (newname + year) % 30 * 2;
		print("The variable is :"+crackscale);
		crackwarping = newname % 100 / 100.0f;
		waterlevel = newname*2  % 100 / 100.0f;
		print ("case color 3: " + basecolor);

		applyChange (rend,basecolor,peakcolor,sludgecolor,crackscale,crackwarping,waterlevel);
	}

	void applyChange(Renderer rend,Color basecolor,Color peakcolor,Color sludgecolor,float crackscale,float crackwarping,float waterlevel) {
		ProceduralMaterial substance = rend.sharedMaterial as ProceduralMaterial;
		if (substance) {

			substance.SetProceduralColor("Base Color",basecolor);
			substance.SetProceduralColor("Peak Color",peakcolor);
			substance.SetProceduralColor("Sludge Color",sludgecolor);

			substance.SetProceduralFloat("Cracks Scale",crackscale);
			substance.SetProceduralFloat("Cracks warping",crackwarping);

			substance.SetProceduralFloat("Water Level",waterlevel);
			substance.RebuildTextures();

		}
	}

}

