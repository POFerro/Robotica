//------------------------------------------------------------------------------
//  <copyright file="FramePreProcessor.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation.  All rights reserved.
//  </copyright>
//------------------------------------------------------------------------------

namespace POFerro.Robotics.SuricataDashboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using Microsoft.Ccr.Core;
    using Microsoft.Kinect;
    using Microsoft.Robotics;

	using ccr = Microsoft.Ccr.Core;
    using common = Microsoft.Robotics.Common;
    using kinect = Microsoft.Robotics.Services.Sensors.Kinect;
    using kinectProxy = Microsoft.Robotics.Services.Sensors.Kinect.Proxy;
    using pm = Microsoft.Robotics.PhysicalModel;

    /// <summary>
    /// Responsibility of this class is to take a raw frame and turn it into a format that can be consumed by UI w/o having to do any service calls in the process
    /// For Video - it menas nothing to do (as RawFrame's planar image for RGB is directly consumable by UI)
    /// For Depth - it means turning Kinect Depth frame into a grayscale (with optional coloring of players)
    /// For Skeletons - it means pre-calculating all Joint positions and storing those in JointPoints container such that UI only needs to connect the dots
    /// </summary>
    public class FramePreProcessor
    {
        /// <summary>
        /// Blue byte offset in Color stream.
        /// </summary>
        private const int BlueIndex = 0;

        /// <summary>
        /// Green byte offset in Color stream.
        /// </summary>
        private const int GreenIndex = 1;

        /// <summary>
        /// Red byte offset in Color stream.
        /// </summary>
        private const int RedIndex = 2;

        /// <summary>
        /// Need to interact with main Kinect service for coordinate calculations of skeletal data
        /// </summary>
        private kinectProxy.KinectOperations kinectPort;

        /// <summary>
        /// We need to initialize Kinect port - since we'll be talking to the service
        /// </summary>
        /// <param name="kinectPort">Kinect port</param>
        public FramePreProcessor(kinectProxy.KinectOperations kinectPort)
        {
            this.kinectPort = kinectPort;
        }

        /// <summary>
        /// Cached raw frames as they were read from the Kinect Service
        /// </summary>
        public kinect.RawKinectFrames RawFrames;

        /// <summary>
        /// Processed depth image - ready to be consumed by the UI
        /// </summary>
        public byte[] DepthColordBytes;

        /// <summary>
        /// Joint coordinates that will be used by UI to draw the seletons. 7 skeleton structs are preallocated
        /// </summary>
        public List<VisualizableSkeletonInformation> AllSkeletons = new List<VisualizableSkeletonInformation>()
        {
            new VisualizableSkeletonInformation(),
            new VisualizableSkeletonInformation(),
            new VisualizableSkeletonInformation(),
            new VisualizableSkeletonInformation(),
            new VisualizableSkeletonInformation(),
            new VisualizableSkeletonInformation(),
            new VisualizableSkeletonInformation()
        };
       
        /// <summary>
        /// Used to pass a point from JointToPointCoordinates() to ProcessSkeletons()
        /// </summary>
        private Point cachedJointPoint;

        /// <summary>
        /// Invoked right after a raw frame was obtained from Kinect service
        /// </summary>
        /// <param name="frames">Raw frame as recieved from Kinect sensor</param>
        public void SetRawFrame(kinect.RawKinectFrames frames)
        {
            this.RawFrames = frames;

            if (null == this.RawFrames.RawDepthFrameData)
            {
                // could be that depth frame was not requested
                return;
            }            
        }       

        /// <summary>
        /// Convert Kinect depth array to color bytes (grayscale with optional player coloring)
        /// </summary>
        /// <returns>Depth image array</returns>
        public byte[] GetDepthBytes()
        {
            int height = this.RawFrames.RawDepthFrameInfo.Height;
            int width = this.RawFrames.RawDepthFrameInfo.Width;

            short[] depthData = this.RawFrames.RawDepthFrameData;

            if (null == this.DepthColordBytes)
            {
                this.DepthColordBytes =
                    new byte[this.RawFrames.RawDepthFrameInfo.Height *
                        this.RawFrames.RawDepthFrameInfo.Width * 4];
            }

            int maxDistSquaredNormalized =
                ((int)SuricataDashboardWPF.MaxValidDepth *
				(int)SuricataDashboardWPF.MaxValidDepth) / 255;

            var depthIndex = 0;
            for (var y = 0; y < height; y++)
            {
                var heightOffset = y * width;

                for (var x = 0; x < width; x++)
                {
                    int index = 0;

                    index = (x + heightOffset) * 4;

                    var distance = this.GetDistanceWithPlayerIndex(depthData[depthIndex]);

                    // we square the distance to optimize for farther objects (i.e. can distingush 
                    // between objects at 2m and 2.1m quite easily
                    this.DepthColordBytes[index + BlueIndex] =
                        this.DepthColordBytes[index + GreenIndex] =
                        this.DepthColordBytes[index + RedIndex] = (byte)((distance * distance) / maxDistSquaredNormalized);

                    this.ColorPlayers(depthData[depthIndex], this.DepthColordBytes, index);

                    depthIndex++;
                }
            }

            return this.DepthColordBytes;
        }

        /// <summary>
        /// Use different colors for different players. 
        /// </summary>
        /// <param name="depthReadingToExamine">Depth reading to examine</param>
        /// <param name="colorFrame">Color image frame</param>
        /// <param name="index">Player index</param>
        internal void ColorPlayers(short depthReadingToExamine, byte[] colorFrame, int index)
        {
            int player = this.GetPlayerIndex(depthReadingToExamine);

            switch (player)
            {
                case 1:
                    colorFrame[index + GreenIndex] = 200;
                    break;
                case 2:
                    colorFrame[index + BlueIndex] = 200;
                    break;
                case 3:
                    colorFrame[index + RedIndex] = 200;
                    break;
                case 4:
                    colorFrame[index + GreenIndex] = 200;
                    colorFrame[index + RedIndex] = 200;
                    break;
                case 5:
                    colorFrame[index + BlueIndex] = 200;
                    colorFrame[index + RedIndex] = 200;
                    break;
                case 6:
                    colorFrame[index + BlueIndex] = 200;
                    colorFrame[index + GreenIndex] = 200;
                    break;
                case 7:
                    colorFrame[index + RedIndex] = 100;
                    break;
            }
        }

        /// <summary>
        /// Depth bytes to millimeter in 'PlayerIndex' format
        /// </summary>
        /// <param name="depth">Depth value to extract distance out of</param>        
        /// <returns>Distance in millimeter</returns>
        private int GetDistanceWithPlayerIndex(short depth)
        {
            int distance = (int)depth >> 3;
            return distance;
        }               

        /// <summary>
        /// Self explanatory
        /// </summary>
        /// <param name="depth">Depth value to extract player index out of</param>
        /// <returns>0 = no player, 1 = 1st player, 2 = 2nd player... </returns>
        private int GetPlayerIndex(short depth)
        {
            return (int)depth & 7;
        }

        /// <summary>
        /// Convert raw skeletal structure into one we can visualize (with window coordinates)
        /// </summary>
        /// <returns>CCR Iterator</returns>
        public IEnumerator<ITask> ProcessSkeletons()
        {
            int skeletonIndex = 0;            

            foreach (Skeleton data in this.RawFrames.RawSkeletonFrameData.SkeletonData)
            {
                this.AllSkeletons[skeletonIndex].IsSkeletonActive = false;
                this.AllSkeletons[skeletonIndex].SkeletonQuality = string.Empty;

                if (SkeletonTrackingState.Tracked == data.TrackingState)
                {
                    this.AllSkeletons[skeletonIndex].IsSkeletonActive = true;
                    this.AllSkeletons[skeletonIndex].SkeletonQuality = data.ClippedEdges.ToString();

                    // Populate joint poitns 
                    foreach (Joint joint in data.Joints)
                    {
                        yield return new IterativeTask<Joint>(joint, this.JointToPointCoordinates);
                        this.AllSkeletons[skeletonIndex].JointPoints[joint.JointType].JointCoordiantes = this.cachedJointPoint;
                        this.AllSkeletons[skeletonIndex].JointPoints[joint.JointType].TrackingState = joint.TrackingState;
                    }
                }

                skeletonIndex++;
            }

            yield break;
        }
        
        /// <summary>
        /// The skeleton data, the color image data, and the depth data are based on different 
        /// coordinate systems. To show consistent images from all three streams in the sample’s 
        /// display window, we need to convert coordinates in skeleton space to image space by 
        /// following steps
        /// /// </summary>
        /// <param name="joint">Joint to get coordinates for</param>
        /// <returns>CCR Iterator</returns>
        private IEnumerator<ITask> JointToPointCoordinates(Joint joint)
        {
            int colorX = 0;
            int colorY = 0;            

            kinectProxy.SkeletonToColorImageRequest request = new kinectProxy.SkeletonToColorImageRequest();
            request.SkeletonVector = joint.Position;

            yield return this.kinectPort.SkeletonToColorImage(request).Choice(
                    SkeletonToColorImageResponse =>
                    {
                        colorX = SkeletonToColorImageResponse.X;
                        colorY = SkeletonToColorImageResponse.Y;
                    },
                    failure =>
                    {
                        // high freq call - no logging
                    });

            // Clip the values
			colorX = Math.Min(colorX, SuricataDashboardWPF.ColorImageWidth);
			colorY = Math.Min(colorY, SuricataDashboardWPF.ColorImageHeight);

            // Scale the color image coordinates to the size of the skeleton display on the screen 
            this.cachedJointPoint = new Point(
				((SuricataDashboardWPF.DisplayWindowWidth * colorX) / SuricataDashboardWPF.ColorImageWidth),
				((SuricataDashboardWPF.DisplayWindowHeight * colorY) / SuricataDashboardWPF.ColorImageHeight));
        }
    }
}