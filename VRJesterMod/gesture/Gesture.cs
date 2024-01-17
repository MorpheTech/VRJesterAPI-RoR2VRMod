using System.Collections.Generic;
using VRJester.Utils.VRData;
using VRJester.Vox;


namespace VRJester.Core {

    public class Gesture {
        // Class that handles compiling the GestureComponent list for each VRDevice
        // Note: A list of GestureComponent's represents a gesture of an individual VRDevice

        private readonly IList<Vhere> vhereList = [];
        public List<GestureComponent> hmdGesture = [];
        public List<GestureComponent> rcGesture = [];
        public List<GestureComponent> lcGesture = [];
        public IList<string> validDevices = [];

        // Initialize the first gesture trace and continue tracking until completion of gesture
        public Gesture(VRDataState vrDataState) {
            // Note: Facing direction is set here, meaning all movements after tracing this Gesture object are relative to that
            VRPose hmdOrigin = vrDataState.Hmd, rcOrigin = vrDataState.Rc, lcOrigin = vrDataState.Lc;
            Vhere hmdVhere = new(VRDevice.HEAD_MOUNTED_DISPLAY, hmdOrigin, hmdOrigin.Direction, Constants.CONFIG_PATH);
            Vhere rcVhere = new(VRDevice.RIGHT_CONTROLLER, rcOrigin, hmdOrigin.Direction, Constants.CONFIG_PATH);
            Vhere lcVhere = new(VRDevice.LEFT_CONTROLLER, lcOrigin, hmdOrigin.Direction, Constants.CONFIG_PATH);
            vhereList.Add(hmdVhere);
            vhereList.Add(rcVhere);
            vhereList.Add(lcVhere);
        }

        // Initialize Gesture with already set gestures for each VRDevice
        public Gesture(List<GestureComponent> hmdGesture, List<GestureComponent> rcGesture, List<GestureComponent> lcGesture) {
            if (hmdGesture != null) {
                this.hmdGesture = hmdGesture;
            }
            if (rcGesture != null) {
                this.rcGesture = rcGesture;
            }
            if (lcGesture != null) {
                this.lcGesture = lcGesture;
            }
        }

        // Initialize Gesture with already set gestures for each VRDevice (for loading from GestureStore file)
        public Gesture(Dictionary<string, List<GestureComponent>> gesture) {
            foreach (string vrDevice in gesture.Keys) {
                IList<string> devices = vrDevice.Split('|');
                if (devices.Contains(Constants.HMD)) {
                    IDictionary<string, string> newValues = new Dictionary<string, string> {
                        ["VrDevice"] = Constants.HMD
                    };
                    if (devices.Count > 1) {
                        validDevices.Add(Constants.HMD);
                    }
                    hmdGesture = GestureComponent.Copy(gesture[vrDevice], newValues);
                }
                if (devices.Contains(Constants.RC)) {
                    IDictionary<string, string> newValues = new Dictionary<string, string> {
                        ["VrDevice"] = Constants.RC
                    };
                    if (devices.Count > 1) {
                        validDevices.Add(Constants.RC);
                    }
                    rcGesture = GestureComponent.Copy(gesture[vrDevice], newValues);
                }
                if (devices.Contains(Constants.LC)) {
                    IDictionary<string, string> newValues = new Dictionary<string, string> {
                        ["VrDevice"] = Constants.LC
                    };
                    if (devices.Count > 1) {
                        validDevices.Add(Constants.LC);
                    }
                    lcGesture = GestureComponent.Copy(gesture[vrDevice], newValues);
                }
            }
        }

        public override string ToString() {
            return "Gesture:" + "\r\n  hmdGesture: " + string.Join(",", hmdGesture) + "\r\n  rcGesture: " + string.Join(",", rcGesture) + "\r\n  lcGesture: " + string.Join(",", lcGesture);
        }

        // Record the Vox trace of each VRDevice and store the resulting data
        public void Track(VRDataState vrDataRoomPre) {
            foreach (Vhere vhere in vhereList) { // Loop through each VRDevice's Vox
                VRPose currentPoint = vhere.GenerateVhere(vrDataRoomPre);
                int currentId = vhere.Id;
                if (vhere.PreviousId != currentId) { // Check if a VRDevice exited Vox
                    vhere.PreviousId = currentId;
                    GestureTrace gestureTrace = vhere.Trace;
                    gestureTrace.CompleteTrace(currentPoint);
                    // Log.Info("COMPLETE TRACE: " + vhere.getId() + ": " + gestureTrace);
                    vhere.BeginTrace(currentPoint);
                    switch (vhere.VrDevice) { // Append a Vhere trace's new GestureComponent object per VRDevice
                        case VRDevice.HEAD_MOUNTED_DISPLAY:
                            hmdGesture.Add(gestureTrace.ToGestureComponent());
                            break;
                        case VRDevice.RIGHT_CONTROLLER:
                            rcGesture.Add(gestureTrace.ToGestureComponent());
                            break;
                        case VRDevice.LEFT_CONTROLLER:
                            lcGesture.Add(gestureTrace.ToGestureComponent());
                            break;
                    }
                }
            }
        }

        public List<GestureComponent> GetGesture(string vrDevice) {
            List<GestureComponent> gesture = [];
            switch (vrDevice) {
                case Constants.HMD:
                    gesture = hmdGesture;
                    break;
                case Constants.RC:
                    gesture = rcGesture;
                    break;
                case Constants.LC:
                    gesture = lcGesture;
                    break;
            }
            return gesture;
        }
    }
}
