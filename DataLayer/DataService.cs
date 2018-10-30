﻿using DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
    public class DataService
    {

        //------------------------Questions and answers----------------------------
        public List<Post> GetPosts()
        {
            using (var db = new SOVAContext())


                return db.Posts.ToList();
        }
        //Full post including comments and tags
        //Get post by comment parentid. Only way we can find that gets question 
        //with comment and posttags
        public Post GetPost(int id)
        {
            using (var db = new SOVAContext())
            {
                var fullPost = db.Posts
                    .Where(x => x.PostId == id)
                    .Include(x => x.PostTags)
                    .FirstOrDefault();
                    
                    
                    //.FirstOrDefault(x => x.PostId == id);
                return fullPost;
            }
        }
        /*public List<Comment> GetPost(int id)
        {
            using (var db = new SOVAContext())
            {
                var fullPost = db.Comments.Include(x => x.Post)
                    .ThenInclude(x => x.PostTags)
                    .Where(x => x.Parent == id);
                    //.FirstOrDefault(x => x.Parent == id);
                return fullPost.ToList();
            }
        }*/

        public List<Post> GetQuestions()
        {
            using (var db = new SOVAContext())
            {
                var questions = db.Posts.
                    Where(x => x.ParentId == null);
                return questions.ToList();
            }
        }

        public Post GetQuestion(int id)
        {
            using (var db = new SOVAContext())
            {
                var question = db.Posts
                    .Where(x => x.ParentId == null)
                    .FirstOrDefault(x => x.PostId == id);
                return question;
            }
        }

        public List<Comment> GetQuestionComments(int id)
        {
            using (var db = new SOVAContext())
            {

                var commentsToQuestion = db.Comments
                      .Where(x => x.PostId == id)
                      .ToList();
                return commentsToQuestion;
            }
        }

        public List<Post> GetQuestionsByString(string title)
        {
            using (var db = new SOVAContext())
            {

                var question = db.Posts.Where(x => x.ParentId == null &&
                (x.Body.ToLower().Contains(title.ToLower()) || x.Title.ToLower().Contains(title.ToLower())));
                return question.ToList();

            }
        }
        public List<Post> GetAnswers()
        {
            using (var db = new SOVAContext())
            {
                var answers = db.Posts
                    .Where(x => x.ParentId != null);
                return answers.ToList();
            }
        }

        public Post GetAnswer(int id)
        {
            using (var db = new SOVAContext())
            {
                var answer = db.Posts
                    .Include(x => x.Comments)
                    .Where(x => x.ParentId != null)
                    .FirstOrDefault(x => x.PostId == id);

                return answer;
            }
        }
        //-------------------------------users----------------
        public List<User> GetUsers()
        {
            using (var db = new SOVAContext())
            {
                var userss = db.Users
                    .ToList();
                return userss;
            }


        }

        public User GetUser(int id)
        {
            using (var db = new SOVAContext())
            {
                var user = db.Users
                    .Where(x => x.Id == id)
                    .FirstOrDefault();


                return user;

            }
        }

        public User createUser(String name, string password)
        {
            using (var db = new SOVAContext())
            {


                var creationdate = DateTime.Now;
                var newUser = new User()
                {

                    UserName = name,
                    Password = password,
                    CreationDate = creationdate
                };
                db.Users.Add(newUser);
                db.SaveChanges();
                return newUser;


            }
        }
        public bool DeleteUser(int id)
        {

            try
            {
                using (var db = new SOVAContext())
                {
                    var deluser = new User()
                    {
                        Id = id
                    };
                    db.Users.Remove(deluser);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        // when update is run the createtionDate is updated to. Need to be fixed. 
        public bool UpdateUser(int userId, string newName, string newPassword)
        {
            using (var db = new SOVAContext())
            {
                var user = db.Users.FirstOrDefault(x => x.Id == userId);
                if (user != null)
                {
                    user.UserName = newName;
                    user.Password = newPassword;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

    }
}
