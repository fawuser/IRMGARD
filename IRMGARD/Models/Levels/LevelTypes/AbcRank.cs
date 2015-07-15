using System;
using System.Collections.Generic;

namespace IRMGARD.Models
{
	public class AbcRank : ILesson
	{
		public string Title { get; set; }
		public string SoundPath { get; set; }
		public string Hint { get; set; }
		public LevelType TypeOfLevel { get; set; }
		public List<AbcRankOption> LettersToLearn { get; set; }


		public AbcRank () {}

		public AbcRank (string title, string soundPath, string hint, LevelType typeOfLevel, List<AbcRankOption> lettersToLearn) 
		{
			this.LettersToLearn = lettersToLearn;
		}
	}

	public class AbcRankOption
	{
		public string Name { get; set; }
		public LevelElement Element { get; set; }


		public AbcRankOption () {}

		public AbcRankOption (string name, LevelElement element)
		{
			this.Name = name;
			this.Element = element;
		}
	}
}

