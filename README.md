## Project Name

---

HydroShift (Formerly Exploring XR)


## Project Topic

---

### Problem we are trying to solve
After a few years of availability of XR-soft- and hardware, we see different approaches to using this technology. Common are cases in gaming (e.g., VR) and business (e.g., AR). The potential to use this immersive technology in a meaningful way has not yet been exhausted.
People are not aware of some impacts they are doing through consumption decisions. Even if they are searching for these information, they find abstract data which are not easy to imagine (e.g., water consumption or ingredients with unknown supply-chains.)

### Goal we want to achieve
In this mixed reality (MR) prototype project, we try to convert abstract data into concrete information. We refer to abstract data as the hidden consumption of water in manufacturing products. To make this more concrete, we use MR to visualize this amount of water in relation to the size of the product. The goal is to change grocery customers' thinking and behavior while shopping in grocery.


### Usecase
In this future related approach a customer is entering a grocery and stats shopping. While the customer is wearing  mixed reality glasses they can scan and select a product. The glasses are showing the hidden water consumption in a concrete manner. For example, the amount of needed water to produce one kilogram of cocoa can displayed right next to it. Through this they could get more affected. Based on this, they can overthink about their consumption decision. The solution can build a bridge between abstract data and influencing information. 

https://user-images.githubusercontent.com/79196690/216679214-5f8c41a8-c93f-432a-8f4c-0e98a37dfe45.mp4


## Project Phase and Focus

---
Domain: **Discover** the world of eXtended Reality (XR), **Learning** by applying hardware and software-environments, **Create** meaningful applications

The project is in the state after a first prototype. Research and Design is done in the first iteration. The goal is to pursuit the mentioned approach to use the potential of XR.

Next main steps will be:
- Improving visual effects
- Improving technology like using the ability of spacial awareness from Hololens 2
- Implementing Product selection via image recognition
- Deploying on Hololens 2 and maybe on Meta Quest 2

## Tech-Stack

---

We are using Microsoft Visual Studio 2019, [Unity](https://unity.com/) and [MRTK3](https://docs.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/mrtk3-overview/).
The MRTK3-approach keeps the door open to work crossplatform. With this, we can maybe avoid environmental changes. The downside of this is that we are more or less forced to use **Microsoft Windows**, but maybe we are able to enable our MacOS and Linux-Users through cloud-solutions like [shadow.tech](shadow.tech).
Hardwarewise we are focusing on Microsoft Hololens 2 primarly. Additionally we maybe expand it to Meta Quest 2.

### Computer Vision
In order to recognize what object is in front of the user we are using the lates state of the art object detection models from the YOLO (You Only Look Once) family.
Link to the Models on GitHub: https://github.com/ultralytics/ultralytics
We based our pipeline on the work of this paper: https://arxiv.org/pdf/2306.03537.pdf
We used YOLOv8n (nano) and YOLOv8s (small) and trained them on the COCO81 dataset for 6hrs+ on a GTX1080Ti.


## Assessments

---

The branch of interest is called **Level2Development**. 
The code of interest can be found in Assets/Scripts and the scripts that I (Paul) worked on this semester are:
- APIConnectionController & APIGroceryResponse
- ImageCapture
- ImageCropper
- ObjectClassification
- JsonManager

Clean Code:
- SmallCubeSpawner
- ObjectDetector
- COCONames, YoloItem
- JsonManager

Automated Software Testing can be found in Assets/Tests/:
- SmallCubeSpawnerPlayModeTests
- SmallCubeSpawnerEditModeTests
  
### Set Up Guide
In order to get started with this project, one should follow these steps:
- clone or download the branch **Level2Development**
- install the Unity Hub and login with your account, then locate the project on your disk
- install Unity 2021.3.27f1 LTS
- if not already satisfied, install either Visual Studio 2022, Visual Studio Code or JetBrains Rider to have an IDE
- run the Project from the Unity Hub
- locate the scene **PrototypeV2** in Assets/Scenes, double click and enter play mode

### Technical Documentation
The goal of this project is that we use the capabilites of the Hololens 2 to visualize the hidden water consumption of Grocery Items. 
In order to do that we need:
- get images from the main camera of the device
- run a computer vision algorithm on the images
- recognize which grocery item is on the image (in front of the user)
- look up the item in a database to retrieve the hidden water consumption
- use the hidden water consumption to visualize it

We are currently doing this with the following scripts located in Assets/Scripts:
- images are provided via **ImageCapture** and **ImageCropper**
- the computer vision algorithm is run via the **ObjectDetector**
- the data about selected grocery items is transfered to the application using the **APIConnectionController** and the **JsonManager**
- the water is visualized with the **SmallCubeSpawner** script

Due to development there are currently certain constraints:
- the data for the visualzation is hard coded (1700 liters of water) and not transferred
- the image is not taken from the camera of the HoloLens 2, but rather taken by Unity's scene Main Camera
- the computer vision algorithm is not able to detect all objects on given images
  
