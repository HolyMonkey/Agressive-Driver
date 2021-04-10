using UnityEngine;
using System.Collections;

public class CarSwitch : MonoBehaviour
{

    public Transform[] Cars;
    public Transform MyCamera;


    public void CurrentCarActive(int current)
    {

        int amount = 0;

        foreach (Transform Car in Cars)
        {
            if (current == amount)
            {
                MyCamera.GetComponent<VehicleCamera>().target = Car;

                MyCamera.GetComponent<VehicleCamera>().Switch = 0;
                MyCamera.GetComponent<VehicleCamera>().cameraSwitchView = Car.GetComponent<PlayerMover>().carSetting.cameraSwitchView;
                Car.GetComponent<PlayerMover>().activeControl = true;
            }
            else
            {
                Car.GetComponent<PlayerMover>().activeControl = false;
            }

            amount++;
        }
    }

}
