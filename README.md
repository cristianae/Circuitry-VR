# Circuitry VR

## How to Run (APK)
1. Download the APK from the Executable folder.
2. Install on Meta Quest 3.
3. Launch the application from Unity in the headset.

## How to Test
- Complete the wiring task.
- Observe flashing lights and audio feedback.
- Trigger the battery fire (by wiring the positive and negative ends of the battery) and extinguish it.
-To pick up components, aim at the center mass of the object and use the grab button to pick up.
- To create a wire connection, aim at the end of the component / wire. Use the grab button to select the end (the aim locator will lock to the attachment point) and while grabbed, use the trigger to activate. The first wire connection will begin at the component and remain attached to the controller until a second connection is made.
- To remove a wire connection, hover over the attachment point and activate with the trigger without grabbing the component.
- To use the fire extinguisher, grab the extinguisher and then use the trigger to activate the foam
- Locomotion is done using the joysticks to control movement and adjust the view of the headset 

## Demo Videos inside the Videos folder
- Outside View Video
- Inside Video (In-Headset Screen Recording)

## Setup instructions for Unity
To open and run this project in Unity, follow the steps below:

1. Install Unity Hub
   - Recommended Unity version: 2022.3 LTS

2. Download the XR packages
   In Unity, open **Window → Package Manager** and install:
   - `XR Interaction Toolkit`
   - `OpenXR Plugin`
   - `XR Plugin Management`
   - `Input System`

3. Clone or download this repository "git clone https://github.com/cristianae/Circuitry-VR.git"

4. Open this project in Unity Hub.

5. Set the build target to Android make sure to switch platforms.

6. In XR Plug-in Management, make sure OpenXR is enabled for Android.

7. Build & Run when you click at the top of the file. Make sure your Meta Quest 3 is connected with a USB Meta Quest Link.

## Features
- Circuit components
- Interactive circuit wiring system
- Visual feedback from flashing lights
- Audio feedback from fire and flashing lights
- Battery fire hazard triggered by incorrect wiring
- Fire extinguisher to extinguish the fire
- Polarity Endpoints

## Authors
- Cristiana Eagen
- Sage Abbott
- Nthati Lehloenya
