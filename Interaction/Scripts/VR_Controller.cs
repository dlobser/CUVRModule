using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace CUVR
{
    public class VR_Controller : MonoBehaviour
    {

        static List<InputDevice> devices = new List<InputDevice>();
        public enum Controller { LEFT, RIGHT };
        public Controller controller = Controller.LEFT;
        Vector3 prev;
        public float speed { get; set; }
        public TextMesh debugText;

        private void Start()
        {
            if (debugText != null)
            {
                var inputDevices = new List<UnityEngine.XR.InputDevice>();
                InputDevices.GetDevices(inputDevices);
                string s = "";
                foreach (var device in inputDevices)
                {
                    s += string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString());
                    s += "\n";
                }
                debugText.text = s;
            }
        }
        void Update()
        {
            switch (controller)
            {
                case Controller.LEFT:
                    var desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
                    InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, devices);

                    break;
                case Controller.RIGHT:
                    var desiredCharacteristics2 = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
                    InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics2, devices);
                    break;
            }

            if (devices.Count > 0)
            {
                InputDevice device = devices[0];
                Vector3 position;
                if (device.TryGetFeatureValue(CommonUsages.devicePosition, out position))
                    this.transform.localPosition = position;
                Quaternion rotation;
                if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out rotation))
                    this.transform.localRotation = rotation;
            }

            speed = Mathf.Lerp(speed, Vector3.Distance(prev, this.transform.position), .6f);
            prev = this.transform.position;
        }
    }
}