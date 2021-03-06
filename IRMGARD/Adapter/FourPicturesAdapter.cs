﻿using System;
using IRMGARD.Models;
using Android.Widget;
using Android.Views;
using Android.Content;
using System.Collections.Generic;
using Android.Graphics.Drawables;

namespace IRMGARD
{
	public class FourPicturesAdapter : ArrayAdapter<FourPicturesOption>
	{
		private LayoutInflater layoutInflater;

		public FourPicturesAdapter (Context context, int resourceId, List<FourPicturesOption> items) : base (context, resourceId, items)
		{
			layoutInflater = LayoutInflater.From (context);
		}

		public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
            var view = convertView;
		    if (view != null)
		        ((BitmapDrawable) view.FindViewById<ImageView>(Resource.Id.ivMeidaElementImage).Drawable).Bitmap.Recycle();
		    else
		        view = layoutInflater.Inflate(Resource.Layout.MediaElement, null);

		    var imageView = view.FindViewById<ImageView>(Resource.Id.ivMeidaElementImage);
            imageView.SetImageBitmap(AssetHelper.GetBitmap(Context, GetItem(position).Media.ImagePath));
			return view;
		}
	}
}