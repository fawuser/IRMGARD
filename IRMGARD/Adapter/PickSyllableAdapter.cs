﻿using System;
using IRMGARD.Models;
using Android.Widget;
using Android.Views;
using Android.Content;
using System.Collections.Generic;

namespace IRMGARD
{
    public class PickSyllableAdapter : ArrayAdapter<PickSyllableOption>
    {
        private LayoutInflater layoutInflater;

        public PickSyllableAdapter(Context context, int resourceId, List<PickSyllableOption> items) : base (context, resourceId, items)
        {
            layoutInflater = LayoutInflater.From(context);
        }

        public override View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = layoutInflater.Inflate(Resource.Layout.MediaElement, null);

            var bitmap = AssetHelper.GetBitmap(Context, "Aal.png");

            view.FindViewById<ImageView> (Resource.Id.ivMeidaElementImage).SetImageBitmap (bitmap);

            return view;
        }
    }
}
