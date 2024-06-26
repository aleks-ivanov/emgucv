﻿using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Models;
using Emgu.CV.Platform.Maui.UI;

namespace BuildInfo;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();

        Emgu.CV.Platform.Maui.MauiInvoke.Init();
        
#if DEBUG
        CvInvoke.LogLevel = LogLevel.Verbose; //LogLevel.Debug;
#endif
        
        DetailLabel.Text = CvInvoke.BuildInformation;

        //If you swap out the mini runtime nuget package with the full runtime package, you will be able to test Face Landmark detection using DNN module.
        var openCVConfigDict = CvInvoke.ConfigDict;
        bool haveDNN = (openCVConfigDict["HAVE_OPENCV_DNN"] != 0);
        FaceDetectBtn.IsVisible = haveDNN;

	String aboutIcon = null;
	ToolbarItem aboutItem = new ToolbarItem("About", aboutIcon,
						() =>
						{
						    this.Navigation.PushAsync(new AboutPage());
						}
	);
	this.ToolbarItems.Add(aboutItem);
    }


    private async void OnFaceDetectClicked(object sender, EventArgs e)
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                return;
            }
        }
        
        ProcessAndRenderPage faceLandmarkDetectionPage = new ProcessAndRenderPage(
            new FaceAndLandmarkDetector(),
            "Perform Face Landmark Detection",
            "lena.jpg",
            "");
        await this.Navigation.PushAsync(faceLandmarkDetectionPage);
    }
}

