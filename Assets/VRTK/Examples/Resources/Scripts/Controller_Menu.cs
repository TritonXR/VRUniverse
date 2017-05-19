namespace VRTK.Examples
{
    using UnityEngine;

    public class Controller_Menu : MonoBehaviour
    {
        public GameObject menuObject;

        private GameObject clonedMenuObject;

        private bool menuInit = false;
        private bool menuActive = false;

        private void Start()
        {
            //GetComponent<VRTK_ControllerEvents>().AliasMenuOn += new ControllerInteractionEventHandler(DoMenuOn);
            //GetComponent<VRTK_ControllerEvents>().AliasMenuOff += new ControllerInteractionEventHandler(DoMenuOff);
            GetComponent<VRTK_ControllerEvents>().AliasMenuOn += new ControllerInteractionEventHandler(DoMenuOn);
            //GetComponent<VRTK_ControllerEvents>().AliasMenuOff += new ControllerInteractionEventHandler(DoMenuOff);
            menuInit = false;
            menuActive = false;
        }

        private void InitMenu()
        {
            clonedMenuObject = Instantiate(menuObject, transform.position, Quaternion.identity) as GameObject;
            clonedMenuObject.SetActive(true);
            menuInit = true;
        }

        private void DoMenuOn(object sender, ControllerInteractionEventArgs e)
        {
            if (!menuInit)
            {
                InitMenu();
            }
            clonedMenuObject.SetActive(true);
            menuActive = true;

            GetComponent<VRTK_ControllerEvents>().AliasMenuOn -= new ControllerInteractionEventHandler(DoMenuOn);
            GetComponent<VRTK_ControllerEvents>().AliasMenuOn += new ControllerInteractionEventHandler(DoMenuOff);

        }

        private void DoMenuOff(object sender, ControllerInteractionEventArgs e)
        {
            clonedMenuObject.SetActive(false);
            menuActive = false;

            GetComponent<VRTK_ControllerEvents>().AliasMenuOn -= new ControllerInteractionEventHandler(DoMenuOff);
            GetComponent<VRTK_ControllerEvents>().AliasMenuOn += new ControllerInteractionEventHandler(DoMenuOn);
        }

        private void Update()
        {
            if (menuActive)
            {
                clonedMenuObject.transform.rotation = transform.rotation;
                clonedMenuObject.transform.position = transform.position;
            }
        }
    }
}