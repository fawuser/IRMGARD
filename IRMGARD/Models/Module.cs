using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace IRMGARD.Models
{
	public class Module
	{
<<<<<<< Upstream, based on origin/master
		public List<Lesson> LessonsList;
=======
		[JsonProperty(ItemTypeNameHandling = TypeNameHandling.Auto)]
		public List<ILesson> LessonsList;
		public int NumberOfLessons;
>>>>>>> 2e23853 now lessons can be serialized as there original type
		public int NumberOfLessonsDone;

		public Module () {}

<<<<<<< Upstream, based on origin/master
		public Module (List<Lesson> lessonsList, int numberOfLessonsDone)
=======
		public Module (List<ILesson> lessonsList, int numberOfLessons, int numberOfLessonsDone)
>>>>>>> 2e23853 now lessons can be serialized as there original type
		{
			this.LessonsList = lessonsList;
			this.NumberOfLessonsDone = numberOfLessonsDone;
		}

		/// <summary>
		/// Determines whether there is a next lesson available from the provided lesson
		/// </summary>
		/// <returns><c>true</c> if this module has a next lesson; otherwise, <c>false</c>.</returns>
		/// <param name="currentLesson">Current lesson.</param>
		public bool HasNextLesson(Lesson currentLesson)
		{
			var index = LessonsList.IndexOf(currentLesson);
			return LessonsList.Count - 1 > index;
		}

		/// <summary>
		/// Gets the next lesson if available.
		/// </summary>
		/// <returns>The next lesson.</returns>
		/// <param name="currentLesson">Current lesson.</param>
		public Lesson GetNextLesson(Lesson currentLesson)
		{
			if (HasNextLesson(currentLesson))
			{
				var index = LessonsList.IndexOf(currentLesson) + 1;
				return LessonsList.ElementAt(index); 
			}

			return null;
		}

		/// <summary>
		/// Determines whether there is a previous lesson available from the provided lesson
		/// </summary>
		/// <returns><c>true</c> if this module has a previous lesson; otherwise, <c>false</c>.</returns>
		/// <param name="currentLesson">Current lesson.</param>
		public bool HasPreviousLesson(Lesson currentLesson)
		{
			var index = LessonsList.IndexOf(currentLesson);
			return index > 0;
		}

		/// <summary>
		/// Gets the previoust lesson if available.
		/// </summary>
		/// <returns>The previoust lesson.</returns>
		/// <param name="currentLesson">Current lesson.</param>
		public Lesson GetPrevioustLesson(Lesson currentLesson)
		{
			if (HasPreviousLesson(currentLesson))
			{
				var index = LessonsList.IndexOf(currentLesson) - 1;
				return LessonsList.ElementAt(index);
			}

			return null;
		}
	}
}
