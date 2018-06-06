using System.Collections;
using UnityEngine;

/* This class is to change some particular values of planets.
 */
public class ChangeValue : MonoBehaviour {
	//Change those planets'looking depending on their properites: name and year
	public void change(Renderer rend, string name, int year, bool renderImmediately){
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

		//Use hash functions to make different planets have different colors
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

		//Use hash functions to change the crackscale, crakwarping and waterlevel
		crackscale = (newname + year) % 30 * 2;
		crackwarping = newname % 100 / 100.0f;
		waterlevel = newname*2  % 100 / 100.0f;

		//Apply all the changes
		applyChange (rend,basecolor,peakcolor,sludgecolor,crackscale,crackwarping,waterlevel, renderImmediately);
	}


	//Apply changes to those planets
	void applyChange(Renderer rend,Color basecolor,Color peakcolor,Color sludgecolor,float crackscale,float crackwarping,float waterlevel, bool renderImmediately) {
		ProceduralMaterial substance = rend.sharedMaterial as ProceduralMaterial;

		//Set all those values.
		if (substance) {

			substance.SetProceduralColor("Base Color",basecolor);
			substance.SetProceduralColor("Peak Color",peakcolor);
			substance.SetProceduralColor("Sludge Color",sludgecolor);

			substance.SetProceduralFloat("Cracks Scale",crackscale);
			substance.SetProceduralFloat("Cracks warping",crackwarping);

			substance.SetProceduralFloat("Water Level",waterlevel);

            if (renderImmediately)
            {
                substance.RebuildTexturesImmediately();
            }
            else
            {
                substance.RebuildTextures();
            }

		}
	}

}

