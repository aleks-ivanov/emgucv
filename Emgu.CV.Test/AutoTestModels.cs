﻿//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;  
using System.Runtime.Serialization;

using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.Aruco;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Flann;
using Emgu.CV.Shape;
using Emgu.CV.Stitching;
using Emgu.CV.Text;
using Emgu.CV.Structure;
using Emgu.CV.Bioinspired;
using Emgu.CV.Dpm;
using Emgu.CV.ImgHash;
using Emgu.CV.Face;
using Emgu.CV.Freetype;

using Emgu.CV.Dnn;
using Emgu.CV.Cuda;
using Emgu.CV.Dai;
using Emgu.CV.Mcc;
using Emgu.CV.Models;
using Emgu.CV.Tiff;
//using Emgu.CV.UI;
using Emgu.CV.Util;
using Emgu.CV.VideoStab;
using Emgu.CV.XFeatures2D;
using Emgu.CV.XImgproc;
using Emgu.Util;

using System.Threading.Tasks;

//using Newtonsoft.Json;
using DetectorParameters = Emgu.CV.Aruco.DetectorParameters;
using DistType = Emgu.CV.CvEnum.DistType;
#if VS_TEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
#elif NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
#else
using Emgu.CV.ML;
using NUnit.Framework;
#endif


namespace Emgu.CV.Test
{
    [TestFixture]
    public class AutoTestModels
    {
        public static void DownloadManager_OnDownloadProgressChanged(long? totalBytesToReceive, long bytesReceived, double? progressPercentage)
        {
            if (totalBytesToReceive != null) 
                Trace.WriteLine(String.Format("{0} bytes downloaded.", bytesReceived));
            else
                Trace.WriteLine(String.Format("{0} of {1} bytes downloaded ({2}%)", bytesReceived, totalBytesToReceive, progressPercentage));
        }

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestWeChatQRCode()
        {
            using (Mat m = EmguAssert.LoadMat("link_github_ocv.jpg"))
            using (Emgu.CV.Models.WeChatQRCodeDetector detector = new WeChatQRCodeDetector())
            {
                await detector.Init(DownloadManager_OnDownloadProgressChanged);
                String text = detector.ProcessAndRender(m, m);
            }

        }

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestYolo()
        {
            using (Mat m = EmguAssert.LoadMat("pedestrian.png"))
            using (Emgu.CV.Models.Yolo detector = new Yolo())
            {
                await detector.Init(DownloadManager_OnDownloadProgressChanged, "YoloV8N");
                String text = detector.ProcessAndRender(m, m);
            }

        }

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestPedestrianDetector()
        {
            using (Mat m = EmguAssert.LoadMat("pedestrian.png"))
            using (Emgu.CV.Models.PedestrianDetector detector = new PedestrianDetector())
            {
                await detector.Init(DownloadManager_OnDownloadProgressChanged);
                String text = detector.ProcessAndRender(m, m);
            }

        }

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestDnnSuperres()
        {
            using (Mat m = EmguAssert.LoadMat("pedestrian"))
            using (Emgu.CV.Models.Superres detector = new Models.Superres())
            {
                await detector.Init(Models.Superres.SuperresVersion.EdsrX2, DownloadManager_OnDownloadProgressChanged);
                String text = detector.ProcessAndRender(m, m);
            }

        }

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestDnnSSDFaceDetect()
        {
            using (Emgu.CV.Models.FaceAndLandmarkDetector detector = new Models.FaceAndLandmarkDetector())
            using (Mat img = EmguAssert.LoadMat("lena.jpg"))
            using (Mat result = new Mat())
            {
                detector.Init(AutoTestModels.DownloadManager_OnDownloadProgressChanged);
                img.CopyTo(result);
                detector.ProcessAndRender(img, result);
                CvInvoke.Imwrite("rgb_ssd_facedetect.jpg", result);
            }
        }

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public async Task TestMACE()
        {
            using (MACE mace = new MACE(64))
            using (FaceDetectorYNModel detector = new FaceDetectorYNModel())
            {
                await detector.Init();
                using (VectorOfMat trainingFaces = new VectorOfMat())
                {
                    using (Mat img1 = EmguAssert.LoadMat("lena.jpg"))
                    {
                        foreach (var face in detector.Detect(img1))
                        {
                            using (Mat faceRegion = new Mat(img1, Rectangle.Round(face.Region)))
                            {
                                trainingFaces.Push(faceRegion);
                                using (Mat blurredFace1 = new Mat())
                                {
                                    CvInvoke.GaussianBlur(faceRegion, blurredFace1, new Size(3, 3), 1);
                                    trainingFaces.Push(blurredFace1);
                                }
                            }
                        }
                    }

                    mace.Train(trainingFaces);

                    using (Mat trainingImg1 = trainingFaces[0])
                    {
                        EmguAssert.IsTrue(mace.Same(trainingImg1));

                    }

                    String filePath = Path.Combine(Path.GetTempPath(), "mace.xml");
                    mace.Save(filePath);
                }
            }
        }
    }
}
