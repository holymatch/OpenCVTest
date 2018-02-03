﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.IO;
using System.Diagnostics;
using Emgu.CV.Tracking;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using RestfulClient;

namespace OpenCVTest
{
    public partial class Form1 : Form
    {

        private VideoCapture _capture = null; //Camera

        private static String _databasePath = Application.StartupPath + "\\Database\\facedb.db";
        private static String _trainerPath = Application.StartupPath + "\\..\\..\\TrainingImage";

        private static int RESIZE_IMAGE_HEIGHT = 75, RESIZE_IMAGE_WIIDTH = 75;

        private List<Person> people;
        private CaptureImageType captureImageType;

        enum CaptureImageType
        {
            None,
            NewPerson,
            CurrentPerson,
            UpdatePerson
        }

        MutliFaceTracker tracker;

        byte[] cropedFace;
        
        public Form1()
        {
            InitializeComponent();
            UpdateBtnStatus(CaptureImageType.None);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tracker.TrackingFaces.Count > 0)
            {
                //Rectangle cropRect = new Rectangle()
                //Rectanle cropRect = tracker.TrackingFaces.First.Value.humanFace.faceRectangle;
                imageBoxCapturedImage.Image = tracker.TrackingFaces.First.Value.humanFace.face;
                //cropedFace = tracker.TrackingFaces.First.Value.humanFace.face.ToJpegData();
                cropedFace = tracker.TrackingFaces.First.Value.humanFace.face.Resize(RESIZE_IMAGE_WIIDTH, RESIZE_IMAGE_HEIGHT, Emgu.CV.CvEnum.Inter.Cubic).ToJpegData();
            }
            
            //imageBoxCapturedImage.Image = _capture.QueryFrame();

        }

        void Tracking(object sender, EventArgs e)
        {
            using (var imageFrame = _capture.QueryFrame().ToImage<Bgr, Byte>())
            {
                if (imageFrame != null)
                {
                    //FaceDetector faceDetector = new FaceDetector();
                    //var faces = faceDetector.findHumanFace(imageFrame);
                    //if (faces.Count()>0)
                    //{
                        if (tracker == null)
                        {
                            tracker = new MutliFaceTracker();
                        }                        
                        tracker.trackFace(imageFrame);
                    //}
                    imgCamUser.Image = imageFrame;
                }                    
            }
                
        }

        //void Application_Idle(object sender, EventArgs e)
        //{
        //    //顯示影像到PictureBox上
        //    if (frameCount == 0)
        //    {
        //        using (var imageFrame = _capture.QueryFrame().ToImage<Bgr, Byte>())
        //        {
        //            if (imageFrame != null)
        //            {
        //                var faceDetector = new FaceDetector();
        //                var faces = faceDetector.findHumanFace(imageFrame);
                        
        //                if (faces.Count > 0)
        //                {
        //                    var grayframe = imageFrame.Convert<Gray, byte>();
        //                    frameCount++;
        //                    //tracker.Init(grayframe.Mat, faces[0].faceRectangle);
        //                }
        //                /*
        //                var grayframe = imageFrame.Convert<Gray, byte>();
        //                var faces = _cascadeClassifier.DetectMultiScale(grayframe, 1.1, 10, Size.Empty); //the actual face detection happens here

                        
                        
        //                foreach (var face in faces)
        //                {
        //                    Bitmap facetarget = new Bitmap(face.Width, face.Height);
        //                    using (Graphics g = Graphics.FromImage(facetarget))
        //                    {
        //                        g.DrawImage(grayframe.Bitmap, new Rectangle(0, 0, facetarget.Width, facetarget.Height), face, GraphicsUnit.Pixel);
        //                    }
        //                    Image<Gray, Byte> faceImage = new Image<Gray, Byte>(facetarget);
        //                    var eyes = _cascadeEyeClassifier.DetectMultiScale(faceImage, 1.3, 5, Size.Empty);

        //                    Debug.Print("Eye count : {0}", eyes.Count());
        //                    if (eyes.Count() == 2)
        //                    {
        //                        foreach (var eye in eyes)
        //                        {
        //                            imageFrame.Draw(new Rectangle(face.X + eye.X, face.Y + eye.Y, eye.Width, eye.Height), new Bgr(Color.Green), 1); //the detected face(s) is highlighted here using a box that is drawn around it/them
        //                            if (this.txtUsrename.Text.Trim() != String.Empty)
        //                            {

        //                                Bitmap target = new Bitmap(face.Width, face.Height);
        //                                using (Graphics g = Graphics.FromImage(target))
        //                                {
        //                                    g.DrawImage(grayframe.Bitmap, new Rectangle(0, 0, target.Width, target.Height), face, GraphicsUnit.Pixel);
        //                                }
        //                                //saveFace(target);
        //                            }
        //                        }
        //                        _face = face;
        //                        imageFrame.Draw(face, new Bgr(Color.BurlyWood), 3); //the detected face(s) is highlighted here using a box that is drawn around it/them
        //                        frameCount++;
        //                        tracker.Init(grayframe.Mat, face);
        //                    }
        //                }
        //                */
        //            }

        //            imgCamUser.Image = imageFrame;
        //        }
        //    } else
        //    {
        //        Debug.Print("Carmer size: {0}", _capture.QueryFrame().Size);
        //        using (var imageFrame = _capture.QueryFrame().ToImage<Bgr, Byte>())
        //        {
        //            // Read frame. 
        //            if (imageFrame != null)
        //            {
        //                var grayframe = imageFrame.Convert<Gray, byte>();
        //                /*
        //                try
        //                {

        //                    var result = tracker.Update(grayframe.Mat, out trackingFace);
        //                    Debug.Print("Tracking result : {0}", result.ToString());
        //                    if (!result)
        //                    {
        //                        frameCount = 0;
        //                        var faceDetector = new FaceDetector();
        //                        faceDetector.getImageWithFace(imageFrame);
        //                        imgCamUser.Image = imageFrame;
        //                        return;
        //                    }                            
        //                    imageFrame.Draw(trackingFace, new Bgr(Color.Red), 3);
        //                } catch (Exception ex)
        //                {
        //                    Debug.Print(ex.ToString());
        //                }
        //                */
                        
        //            }
        //            frameCount++;
        //            imgCamUser.Image = imageFrame;
        //            if (frameCount > 27)
        //            {
        //                frameCount = 0;
        //            }
        //        }

        //    }       

        //}

        private void button2_Click(object sender, EventArgs e)
        {
            FaceRecognizer recognizer = new FaceRecognizer();
           
        }

        private void saveFace(Bitmap face)
        {
            
            if (txtUsrename.Text.Trim() != String.Empty)
            {
                var username = txtUsrename.Text.Trim().Trim().ToLower();
                //MessageBox.Show(result, "Save Result", MessageBoxButtons.OK);
            }
        }

        public static byte[] ImageToByte(Bitmap img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _capture = new VideoCapture();
            imgCamUser.Image = _capture.QueryFrame();

            FaceRecognizer recognizer = new FaceRecognizer();
            recognizer.LoadRecognizerData();
            var result = recognizer.RecognizeUser(_capture.QueryFrame().ToImage<Gray, Byte>());
            var resultMsg = "Face not recongize";
            if (result != -1)
            {
                
            }
            MessageBox.Show(resultMsg, "Result", MessageBoxButtons.OK);

            _capture.Dispose();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            var directories = new List<String>(Directory.GetDirectories(_trainerPath).ToList());
            for (var i = 0; i < directories.Count(); i++)
            {
                DirectoryInfo d = new DirectoryInfo(directories[i]);
                FileInfo[] Files = d.GetFiles("*.jpg");
                for (var j = 0; j < Files.ToList().Count(); j++)
                {
                    //Debug.Print("File name : {0}", Files[i].FullName);
                    //Debug.Print("Dir name : {0}", d.Name);

                    Bitmap image = (Bitmap)Image.FromFile(Files[j].FullName, true);
                    FaceDetector faceDetector = new FaceDetector();
                    var humanFaces = faceDetector.findHumanFace(image);
                    if ( humanFaces.Count() == 0 )  {
                        Debug.Print("Human Faces not found : {0}", Files[j].Name);
                    } else if (humanFaces.Count() == 1)
                    {

                        Image<Gray, Byte> faceImage = new Image<Gray, Byte>(humanFaces[0].face.ToBitmap()); 
                        
                    }
                    /*
                    Byte[] file;

                    using (var stream = new FileStream(Files[i].FullName, FileMode.Open, FileAccess.Read))
                    {
                        
                        using (var reader = new BinaryReader(stream))
                        {
                            file = reader.ReadBytes((int)stream.Length);
                        }
                    }
                    */
                    //_dataStoreAccess.SaveFace(d.Name, file);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var myForm = new frmOverlapTest();
            myForm.Show();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _capture = new VideoCapture();
            imgCamUser.Image = _capture.QueryFrame();
            UpdateBtnStatus(CaptureImageType.NewPerson);
            Application.Idle += Tracking;
        }

        private void StopPreview()
        {
            Application.Idle -= Tracking;
            if (_capture != null && _capture.IsOpened)
            {
                _capture.Stop();
                _capture.Dispose();
                _capture = null;
            }
            if (imgCamUser.Image != null)
            {
                imgCamUser.Image.Dispose();
                imgCamUser.Image = null;
            }            
        }

        private async void btnCheckFace_ClickAsync(object sender, EventArgs e)
        {
            txtUsrename.Text = "";
            txtScore.Text = "";
            Stopwatch timer = Stopwatch.StartNew();
            var face_data = new WebEntity.Face(Convert.ToBase64String(cropedFace));
            try
            {
                var response = await RestfulClient.Recognize(face_data);
                if (response.ReturnCode == 200)
                {
                    txtUsrename.Text = response.Content.name;
                    txtScore.Text = response.Content.score.ToString();
                    txtErrorMessage.Text = "";
                }
                else
                {
                    txtErrorMessage.Text = response.Message;
                }
            } catch(Exception ex)
            {
                Debug.WriteLine("Error on connecting server: " + ex.Message);
                txtErrorMessage.Text = "Error on connecting server: " + ex.Message;
            }
            
            
            timer.Stop();
            TimeSpan timespan = timer.Elapsed;

            Console.WriteLine(String.Format("{0:00}:{1:00}:{2:00}", timespan.Minutes, timespan.Seconds, timespan.Milliseconds / 10));
        }

        private async void btnListPeople_Click(object sender, EventArgs e)
        {
            try
            {
                txtErrorMessage.Text = "";
                var response = await RestfulClient.ListPeople();
                if (response.ReturnCode == 200)
                {
                    people = response.Content;
                    var imageList = new ImageList();
                    imageList.ImageSize = new Size(72, 72);
                    imageList.ColorDepth = ColorDepth.Depth32Bit;
                    lvFaceList.Clear();
                    foreach (Person person in people )
                    {
                        imageList.Images.Add(person.id.ToString(), Base64ToImage(person.faceData));
                        lvFaceList.LargeImageList = imageList;
                        lvFaceList.Items.Add(person.name, person.id.ToString());
                    }
                    lvFaceList.LargeImageList = imageList;
                    txtErrorMessage.Text = "Success";
                }
                else
                {
                    txtErrorMessage.Text = response.Message;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error on connecting server: " + ex.Message);
                txtErrorMessage.Text = "Error on connecting server: " + ex.Message;
            }
        }

        private async void btnAddFace_ClickAsync(object sender, EventArgs e)
        {
            if (cropedFace != null)
            {
                var person = new Person(txtUsrename.Text, txtDetail.Text, Convert.ToBase64String(cropedFace));

                try
                {
                    txtErrorMessage.Text = "";
                    var response = await RestfulClient.CreatePerson(person);
                    if (response.ReturnCode == 200)
                    {
                        txtErrorMessage.Text = "Success";
                    }
                    else
                    {
                        txtErrorMessage.Text = response.Message;
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error on connecting server: " + ex.Message);
                    txtErrorMessage.Text = "Error on connecting server: " + ex.Message;
                }
            } else
            {
                MessageBox.Show("Please capture face first.","No face found",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void lvFaceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvFaceList.SelectedItems.Count > 0)
            {
                var imageIndex = lvFaceList.LargeImageList.Images.IndexOfKey(lvFaceList.SelectedItems[0].ImageKey);
                if (imageIndex > -1 && imageIndex < lvFaceList.LargeImageList.Images.Count)
                {
                    if (people[imageIndex].id.ToString() == lvFaceList.SelectedItems[0].ImageKey)
                    {
                        txtUsrename.Text = people[imageIndex].name;
                        txtDetail.Text = people[imageIndex].detail;
                    }
                    Bitmap bmpImage = new Bitmap(lvFaceList.LargeImageList.Images[imageIndex]);
                    imageBoxCapturedImage.Image = new Image<Bgr, Byte>(bmpImage);
                    captureImageType = CaptureImageType.CurrentPerson;
                    bmpImage.Dispose();
                }
                UpdateBtnStatus(CaptureImageType.CurrentPerson);
            } else
            {
                if (imageBoxCapturedImage.Image != null)
                {
                    imageBoxCapturedImage.Image.Dispose();
                    imageBoxCapturedImage.Image = null;
                }
                UpdateBtnStatus(CaptureImageType.None);
            }                  
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            UpdateBtnStatus(CaptureImageType.None);
        }

        private void UpdateBtnStatus(CaptureImageType newStatus)
        {
            captureImageType = newStatus;
            switch (captureImageType)
            {
                case CaptureImageType.None:
                    StopPreview();
                    txtDetail.Text = "";
                    txtScore.Text = "";
                    txtUsrename.Text = "";
                    lvFaceList.SelectedItems.Clear();
                    lvFaceList.Enabled = true;
                    imageBoxCapturedImage.Image = null;
                    btnCancel.Enabled = false;
                    btnStartPreview.Enabled = true;
                    btnStartPreview.Text = "New Person";
                    btnSaveFace.Enabled = false;
                    btnCheckFace.Enabled = false;
                    btnListPeople.Enabled = true;
                    btnCaptureFace.Enabled = false;
                    break;
                case CaptureImageType.NewPerson:
                    lvFaceList.SelectedItems.Clear();
                    lvFaceList.Enabled = false;
                    btnCancel.Enabled = true;
                    btnStartPreview.Enabled = false;
                    btnSaveFace.Enabled = true;
                    btnCheckFace.Enabled = true;
                    btnListPeople.Enabled = false;
                    btnCaptureFace.Enabled = true;
                    break;
                case CaptureImageType.CurrentPerson:
                    btnCancel.Enabled = true;
                    btnStartPreview.Enabled = true;
                    btnStartPreview.Text = "Start Preview";
                    btnSaveFace.Enabled = true;
                    btnCheckFace.Enabled = false;
                    btnListPeople.Enabled = false;
                    btnCaptureFace.Enabled = false;
                    break;
                case CaptureImageType.UpdatePerson:
                    break;
            }
        }

        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
    }
}
