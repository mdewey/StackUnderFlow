using System;

namespace StackUnderFlow.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string AnswerText { get; set; }
        public DateTime TimeAnswered { get; set; } = DateTime.Now;

        public int UpVote { get; set; }
        public int DownVote { get; set; }

        public bool IsCorrectAnswer { get; set; }

        // FK : User
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        // FK : Question
        public int QuestionId { get; set; }

        public Question Question { get; set; }



    }
}