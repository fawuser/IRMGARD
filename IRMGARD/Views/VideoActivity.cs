﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.Res;
using Android.Media;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;
using System.Drawing;

namespace IRMGARD
{
    [Activity(Label = "IRMGARD", NoHistory = true)]
    public class VideoActivity : AppCompatActivity, MediaPlayer.IOnPreparedListener, ISurfaceHolderCallback
    {
        MediaPlayer mediaPlayer;
        VideoView videoView;
        string nextView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Video);
            this.CompatMode();

            // Read context
            nextView = Intent.Extras.GetString("nextView");

            FindViewById<FloatingActionButton>(Resource.Id.btnNext).Click += BtnNext_Click;
            FindViewById<FloatingActionButton>(Resource.Id.btnRepeat).Click += BtnRepeat_Click;
            videoView = FindViewById<VideoView>(Resource.Id.videoView);

            var holder = videoView.Holder;
            holder.AddCallback(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            mediaPlayer = new MediaPlayer();
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            if (hasFocus)
            {
                // Adjust video size to 9:16
                var layoutParams = videoView.LayoutParameters;
                layoutParams.Width = Convert.ToInt32(videoView.MeasuredHeight * 0.5625);
                videoView.LayoutParameters = layoutParams;

                // Play Video
                Play();
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            mediaPlayer.Stop();
            mediaPlayer.Release();
        }

        protected void Play()
        {
            if (!String.IsNullOrEmpty(DataHolder.Current.CurrentModule.VideoPath))
            {
                var descriptor = Assets.OpenFd(DataHolder.Current.CurrentModule.VideoPath);
                mediaPlayer.SetDataSource(descriptor.FileDescriptor, descriptor.StartOffset, descriptor.Length);
                mediaPlayer.Prepare();
                mediaPlayer.Start();
            }
        }

        void BtnNext_Click (object sender, EventArgs e)
        {
            // Navigate to next activity according to the given bundle
            Intent intent = null;
            if (nextView.Equals("LessonFrameActivity"))
                intent = new Intent(this, typeof(LessonFameActivity));
            else if (nextView.Equals("ModuleSelectActivity"))
                intent = new Intent(this, typeof(ModuleSelectActivity));

            if (intent != null)
                StartActivity(intent);
        }

        void BtnRepeat_Click (object sender, EventArgs e)
        {
            mediaPlayer.Stop();
            mediaPlayer.Prepare();
            mediaPlayer.Start();
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            mediaPlayer.SetDisplay(holder);
        }

        public void SurfaceDestroyed(ISurfaceHolder holder) {}
        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int w, int h) {}
        public void OnPrepared(MediaPlayer player) {}
    }
}