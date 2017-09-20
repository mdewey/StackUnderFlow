using System;
using System.Collections.Generic;

namespace StackUnderFlow.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }   
        public string QuestionText { get; set; }
        public DateTime DatePosted { get; set; } = DateTime.Now;

        public int UpVotes {get;set;}
        public int DownVotes{get;set;}

        // Foreign Keys
        // User
        public string ApplicationUserId {get;set;}
        public ApplicationUser ApplicationUser {get;set;}
        // Answers
        public ICollection<Answer> Answers { get; set; } = new HashSet<Answer>();

    }
}