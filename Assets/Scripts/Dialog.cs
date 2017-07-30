using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour {
  public Button YourButton;
  public Text DialogText;
  public Canvas CanvasObject;
  
  private string[] _dialogs;
  private int dialogPos = 0;

  void Start() {
    Button btn = YourButton.GetComponent<Button>();
    btn.onClick.AddListener(TaskOnClick);

    _dialogs = new string[10];
    _dialogs[0] =
      "Thank you for coming,you are our only hope.\nAs you know we just started our new colonly here in this harsh environment";
    _dialogs[1] =
      "Without our shields we would not last a day on this planet, and here lays the problem, Yesterday you might have heard a loud explosion, this was our mining post that exploded";
    _dialogs[2] =
      "Unfortunatey we have not had any time to store any coal and without coal our PowerPlant wont generate any power and without power ... well guess what those shields need.";
    _dialogs[3] =
      "On top of that our chief PowerProduction died in that explosion while ironicaly checking up on the saftey of the place.";
    _dialogs[4] = "You are the only one in this colony that has any previous experience with power management.";
    _dialogs[5] = "Please help us, our lives depend on it. Without power this colony will be destroyed.";
    _dialogs[6] =
      "Start with creating a new mining facility, but be carefull we only have a limited amount of power stored.\n And everything you build will take away from that power.";
    _dialogs[7] =
      "Don't wait to long because we can last a bit with the power we have but every week we expect new colonist and every colonist will bring a PowerUsage Footprint.";
    _dialogs[8] = "Blank";
    _dialogs[9] = "Blank";
    DialogText.text = _dialogs[dialogPos++];
  }

  void TaskOnClick() {
    if (dialogPos == 7) {
      YourButton.GetComponentInChildren<Text>().text = "Go and hurry";
    }
    if (dialogPos == 8) {
      CanvasObject.enabled = false;
      dialogPos++;
      return;
    }
    
    DialogText.text = _dialogs[dialogPos];
    dialogPos++;
  }
}