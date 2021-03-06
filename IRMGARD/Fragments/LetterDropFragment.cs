﻿using System;
using IRMGARD.Models;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Renderscripts;

namespace IRMGARD
{
    public class LetterDropFragment : LessonFragment<LetterDrop>
    {
        LinearLayout llTaskItems;
        FlowLayout flLetters;
        ImageButton btnCheck;

        public LetterDropFragment(Lesson lesson) : base(lesson)
        {
        }

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
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

        protected override void InitIteration()
        {
            base.InitIteration();

            var currentIteration = GetCurrentIteration<LetterDropIteration>();

            // Randomize font case
            var random = new Random();
            var fontCase = (Case)(random.Next(2) + 1);

            // Generate Options
            currentIteration.Options = GenerateOptions(currentIteration, 10, fontCase);

            // Generate Task letters
            currentIteration.TaskLetters = new List<LetterDropTaskLetter>();
            foreach (var letter in currentIteration.LettersToLearn)
            {
                currentIteration.TaskLetters.Add(new LetterDropTaskLetter(letter.ToCase(fontCase)));
            }

            // Add options to view
            var letterAdapter = new LetterAdapter(Activity.BaseContext, 0, currentIteration.Options);
            for (var i = 0; i < currentIteration.Options.Count; i++)
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

            // Add task letters to view
            BuildTaskLetters(currentIteration.TaskLetters);

            btnCheck.Enabled = false;
        }

        private List<LetterBase> GenerateOptions(LetterDropIteration iteration, int numberOfOptions, Case fontCase)
        {
            var random = new Random();

            // Add correct options
            var options = iteration.LettersToLearn.Select(letter => new LetterBase(letter.ToNegativeCase(fontCase))).ToList();

            // Add false options with random cases
            while (options.Count() < numberOfOptions)
            {
                var letter = Alphabet.GetRandomLetter().ToCase((Case)(random.Next(2) + 1));
                while (options.FirstOrDefault(o => o.Letter.Equals(letter)) != null)
                    letter = Alphabet.GetRandomLetter().ToCase((Case)(random.Next(2) + 1));

                options.Add(new LetterBase(letter));
            }

            options.Shuffle();
            return options;
        }

        void BuildTaskLetters(List<LetterDropTaskLetter> taskLetters)
        {
            llTaskItems.RemoveAllViews();
            var taskItemAdapter = new TaskLetterAdapter(Activity.BaseContext, 0, taskLetters.Cast<TaskLetter>().ToList());

            for (var i = 0; i < taskLetters.Count; i++)
            {
                var view = taskItemAdapter.GetView(i, null, null);

                // Define searched letters as drop zone
                if (taskLetters.ElementAt(i).IsSearched)
                    view.Drag += View_Drag;

                // Add letter to view
                llTaskItems.AddView(view);
            }
        }

        void View_Drag(object sender, View.DragEventArgs e)
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
                        var taskLetters = GetCurrentIteration<LetterDropIteration>().TaskLetters;
                        var draggedLetter = data.GetItemAt(0).Text;
                        var position = llTaskItems.IndexOfChild(sender as View);

                        // Get case of dragged letter
                        var fontCase = draggedLetter.GetCase(); 

                        // Check if selection is correct
                        if (taskLetters[position].IsSearched && taskLetters[position].CorrectLetter.Equals(draggedLetter.ToNegativeCase(fontCase)))
                            taskLetters[position].IsCorrect = true;

                        // Rebuild task letters
                        if (taskLetters[position].Letter.Length > 1)
                            taskLetters[position].Letter = taskLetters[position].Letter.Remove(1);
                        
                        taskLetters[position].Letter += draggedLetter;
                        BuildTaskLetters(taskLetters);
                        btnCheck.Enabled = true;
                    }

                    break;
            }
        }

        void BtnCheck_Click (object sender, EventArgs e)
        {
            CheckSolution();
        }

        protected override void CheckSolution()
        {
            var success = true;
            foreach (var taskLetter in GetCurrentIteration<LetterDropIteration>().TaskLetters)
            {
                if (!taskLetter.IsCorrect)
                {
                    success = false;                  
                    break;
                }
            }

            if (success)
                FinishIteration(true);
            else
            {                
                FinishIteration(false);
            }
        }
    }
}

