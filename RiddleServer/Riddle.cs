using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiddleServer
{
	public class Riddle
	{
		public string Content { get; private set; }
		public string Answer { get; private set; }

		public Riddle(string content, string answer) {
			Content = content;
			Answer = answer;
		}

		public async Task UpdateRiddle() {
			Riddle newRiddle = await RiddleApi.RequestNewRiddle();
			Content = newRiddle.Content;
			Answer = newRiddle.Answer;
		}

		public bool IsCorrectAnswer(string answer) {
			return answer == Answer.ToLower();
		}
	}
}
