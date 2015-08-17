﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using IRMGARD.Models;

namespace IRMGARD
{
    public class FindMissingLetterFragment : LessonFragment<FindMissingLetter>
    {
        LinearLayout llTaskItems;
        FlowLayout flLetters;
        ImageButton btnCheck;

        public FindMissingLetterFragment(Lesson lesson) : base(lesson)
        {
        }       

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Prepare view
            var view = inflater.Inflate(Resource.Layout.FindMissingLetter, container, false);
            llTaskItems = view.FindViewById<LinearLayout>(Resource.Id.llTaskItems);
            flLetters = view.FindViewById<FlowLayout>(Resource.Id.flLetters);
            btnCheck = view.FindViewById<ImageButton>(Resource.Id.btnCheck);
            btnCheck.Click += BtnCheck_Click;

            // Initialize iteration
            InitIteration();
            return view;
        }            

        private void InitIteration()
        {
            var currentIteration = GetCurrentIteration<FindMissingLetterIteration>();

            // Create lesson
            BuildTaskLetters(currentIteration.TaskLetters);

            var letterAdapter = new LetterAdapter(Activity.BaseContext, 0, currentIteration.Options);
            for (int i = 0; i < currentIteration.Options.Count; i++)
            {
                // Add letter to view
                var view = letterAdapter.GetView(i, null, null);
                var letter = currentIteration.Options.ElementAt(i).Letter;

                // Add drag 
                view.Touch += (sender, e) => {
                    var data = ClipData.NewPlainText("letter", letter);
                    (sender as View).StartDrag(data, new View.DragShadowBuilder(sender as View), null, 0);
                };

                flLetters.AddView(view);
            }

            btnCheck.Enabled = false;
        }            

        void BuildTaskLetters(List<FindMissingLetterTaskLetter> letters)
        {
            llTaskItems.RemoveAllViews();
            var taskItemAdapter = new FindMissingLetterTaskItemAdapter(Activity.BaseContext, 0, letters);
            for (int i = 0; i < letters.Count; i++)
            {
                var view = taskItemAdapter.GetView(i, null, null);

                // Define searched letters as drop zone
                if (letters.ElementAt(i).IsSearched)
                    view.Drag += View_Drag;                

                // Add letter to view
                llTaskItems.AddView(view);
            }
        }

        void LvLetters_ItemLongClick (object sender, AdapterView.ItemLongClickEventArgs e)
        {
            // Generate clip data package to attach it to the drag
            var data = ClipData.NewPlainText("letter", GetCurrentIteration<FindMissingLetterIteration>().Options.ElementAt(e.Position).Letter);

            // Start dragging and pass data
            (e.View).StartDrag(data, new View.DragShadowBuilder(e.View), null, 0);
        }

        void View_Drag (object sender, View.DragEventArgs e)
        {
            // React on different dragging events
            var evt = e.Event;
            switch (evt.Action) 
            {
                case DragAction.Ended:  
                case DragAction.Started:
                    e.Handled = true;
                    break;           

                // Dragged element enters the drop zone
                case DragAction.Entered:                                       
                    break;

                // Dragged element exits the drop zone
                case DragAction.Exited:                                       
                    break;

                // Dragged element has been dropped at the drop zone
                case DragAction.Drop:
                    e.Handled = true;

                    // Try to get clip data
                    var data = e.Event.ClipData;
                    if (data != null)
                    {
                        var taskLetters = GetCurrentIteration<FindMissingLetterIteration>().TaskLetters;
                        var draggedLetter = data.GetItemAt(0).Text;
                        var position = llTaskItems.IndexOfChild(sender as View);

                        if (taskLetters[position].IsSearched)
                            taskLetters[position].Letter = draggedLetter;
                                                       
                        BuildTaskLetters(taskLetters);
                    }

                    btnCheck.Enabled = true;
                    break;
            }
        }           

        void BtnCheck_Click (object sender, EventArgs e)
        {
            var isCorrect = false;
            var currentIteration = GetCurrentIteration<FindMissingLetterIteration>();
            for(var i = 0; i < currentIteration.TaskLetters.Count; i++ )
            {
                var taskLetter = currentIteration.TaskLetters[i];
                if (taskLetter.IsSearched)
                {
                    var droppedLetter = currentIteration.Options.FirstOrDefault(o => o.Letter.Equals(taskLetter.Letter));
                    isCorrect = droppedLetter != null && droppedLetter.CorrectPos == i;

                    if (!isCorrect)
                        break;
                }
            }

            if (isCorrect)
            {
                Toast.MakeText(Activity.BaseContext, "Rrrrichtiiig", ToastLength.Short).Show();
                if (currentIterationIndex == lesson.Iterations.Count - 1)
                {
                    // All iterations done. Finish lesson
                    LessonFinished();
                }
                else
                {
                    currentIterationIndex++;
                    InitIteration();
                }
            }
            else
            {
                Toast.MakeText(Activity.BaseContext, "Leider verloren", ToastLength.Short).Show();
            }
        }
    }
}