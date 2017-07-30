using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class Dialog : MonoBehaviour {
  public Button YourButton;
  public Text DialogText;
  public Canvas CanvasObject;

  private string[] _dialogs;
  private string[] _buttons;
  private int dialogPos = 0;
  private State _state;

  void Update() {
    if (_state.FirstTimePowerEqual && _state.FirstTimePowerEqualMsgShow == false) {
      _state.FirstTimePowerEqualMsgShow = true;
      dialogPos = 10;
      SetGuiTexts(dialogPos++);
      CanvasObject.enabled = true;

      GameObject.Find("Scene").GetComponent<MapCreator>().City1.addResidents(10);
    }

    if (_state.Lost) {
      CanvasObject.enabled = true;
      DialogText.text = "NOOOOOOOOOO!!!! your incompetence killed us all";
      YourButton.GetComponentInChildren<Text>().text = "Game over";
    }

    Debug.Log(_state.Research2Done);
    if (_state.Research2Done) {
      CanvasObject.enabled = true;
      DialogText.text = "YAAAY!!!! Unlimited power we made it, Well done";
      YourButton.GetComponentInChildren<Text>().text = "Game finished";
    }
    
    if (_state.FirstHouseBuild && _state.FirstHouseBuildMsgShow == false && _state.FirstTimePowerEqualMsgShow) {
      _state.FirstTimePowerEqualMsgShow = true;
      dialogPos = 15;
      SetGuiTexts(dialogPos++);
      CanvasObject.enabled = true;
      _state.FirstHouseBuildMsgShow = true;
    }
    
    if (_state.PowerPlants >=2  && _state.FirstHouseBuildMsgShow && _state.PowerPlantMsgShown == false) {
      _state.PowerPlantMsgShown = true;
      dialogPos = 18;
      SetGuiTexts(dialogPos++);
      CanvasObject.enabled = true;
    }
  }

  void Start() {
    _state = GameObject.Find("Scene").GetComponent<State>();

    Button btn = YourButton.GetComponent<Button>();
    btn.onClick.AddListener(TaskOnClick);

    _dialogs = new string[22];
    _buttons = new string[22];
    _dialogs[0] =
      "Thank you for coming,you are our only hope.\nAs you know we just started our new colonly here in this harsh environment";
    _dialogs[1] =
      "Without our shields we would not last a day on this planet, and therein lays the problem. \n\n Yesterday you might have heard a loud explosion, this was our mining post that exploded";
    _dialogs[2] =
      "Unfortunatey we have not had any time to store any coal and without coal our PowerPlant wont generate any power and without power\n\n... well guess what those shields need.";
    _dialogs[3] =
      "On top of that our chief PowerProduction died in that explosion while ironicaly checking up on the saftey of the place.";
    _dialogs[4] = "You are the only one in this colony that has any previous experience with power management.";
    _dialogs[5] = "Please help us, our lives depend on it. Without power this colony will be destroyed.";
    _dialogs[6] =
      "Start with creating a new mining facility, but be carefull we only have a limited amount of power stored. Keep in mind everything you build will use some of that power.\nAlso do not spend too much if you try to build something and we dont have the energy the shields will fail!!";
    _dialogs[7] =
      "Don't wait to long because while we can last a fair bit with the power we have stored, but every week we expect new colonist. \n\n Every colonist will bring his or her own PowerUsage Footprint.";
    _dialogs[8] = "Blank";
    _dialogs[9] = "Blank";

    _dialogs[10] = "Great i see we are stable again for now. But dont forget new settlers will come soon.";
    _dialogs[11] =
      "Actually i see they are there already. See the power drops into the red numbers again. You should build an eco appartment for them.";
    _dialogs[12] =
      "An eco appartment. New colonist that have no house will stay in the community center. This however takes a toll on the Power.";
    _dialogs[13] =
      "Of course an appartment will cost energy as well however it can house 14 colonists for only 6 power\n\n In comparison each colonist in the center will use 1 power";
    _dialogs[14] = "blank";

    _dialogs[15] = "That is better, however we are still losing power.";
    _dialogs[16] =
      "Luckily the coal mine can support up to 3 energy plants. Hurry build one before we don't have the energy for it anymore";
    _dialogs[17] = "blank";
    
    _dialogs[18] = "Great we are stable again, and since we still have room for a new PowerPlant";
    _dialogs[19] = "If you manage the power good it might be a good idea to build a ScienceStation \n\n With the science station we can Research better or cleaner energy sources.";
    _dialogs[20] = "Science stations will have an energy upkeep like all buildings, however research needs energy to so think carefully before you research something";
    _dialogs[21] = "blank";
    
    
    _buttons[0] =
      "Why what is wrong?";
    _buttons[1] = "I already wondered.";
    _buttons[2] =
      "Power!";
    _buttons[3] =
      "That is horrible";
    _buttons[4] = "That was long ago";
    _buttons[5] = "Of course";
    _buttons[6] =
      "i will";
    _buttons[7] =
      "I understand";
    _buttons[8] = "Blank";
    _buttons[9] = "Blank";
    _buttons[10] = "Yes, it was hard work";
    _buttons[11] = "Oh no!!";
    _buttons[12] = "A what?";
    _buttons[13] = "I will start with it";
    _buttons[14] = "blanl";

    _buttons[15] = "What can we do?";
    _buttons[16] = "I'm on it";
    _buttons[17] = "Blank";

    _buttons[18] = "FINALLY!!";
    _buttons[19] = "But that will need more power";
    _buttons[20] = "Let me think about it";
    _buttons[21] = "blank";
    
    SetGuiTexts(dialogPos++);
  }

  private void SetGuiTexts(int textPos) {
    YourButton.GetComponentInChildren<Text>().text = _buttons[textPos];
    DialogText.text = _dialogs[textPos];
  }

  void TaskOnClick() {
    if (dialogPos == 8 || dialogPos == 14 || dialogPos == 17 || dialogPos == 21) {
      CanvasObject.enabled = false;
      dialogPos++;
      return;
    }

    SetGuiTexts(dialogPos++);
  }
}