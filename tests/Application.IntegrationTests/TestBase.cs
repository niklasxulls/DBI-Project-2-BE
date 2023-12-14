using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using AutoMapper;
using stackblob.Domain.Entities;
using stackblob.Domain.Enums;
using stackblob.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using stackblob.Infrastructure.Services;
using stackblob.Domain.Entities.Lookup;
using Bogus;
using NuGet.Packaging;
using stackblob.Domain.Util;
using stackblob.Infrastructure.Util;

namespace Application.IntegrationTests;

[Collection("global")]
public class TestBase : IAsyncLifetime
{
    private readonly SetupFixture _setup;
    protected readonly IServiceScope _scope;
    protected readonly StackblobDbContext _context;
    protected IAuthService _auth;
    public IFileService _fileService;
    protected readonly IMapper _mapper;
    protected Dictionary<int, string> UserPasswords = new();
    private User _defaultUser;
    public User DefaultUser
    { 
        private set
        {
           _defaultUser = value;
        }
        get
        {
            return _defaultUser;
        }
    } 


    public TestBase(SetupFixture setup)
    {
        _setup = setup;
        _scope = _setup._scopeFactory.CreateScope();
        _context = _scope.ServiceProvider.GetRequiredService<StackblobDbContext>();
        _auth = _scope.ServiceProvider.GetRequiredService<IAuthService>();
        _mapper = _scope.ServiceProvider.GetRequiredService<IMapper>();
        _fileService = _scope.ServiceProvider.GetRequiredService<IFileService>();
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    public async Task<TResponse> SendMediator<TResponse>(IRequest<TResponse> request, User? u = null, bool explicitNonUser = false, bool userIsNotVerified = false)
    {
        _setup.CurrentUserId = u == null && !explicitNonUser ? DefaultUser?.UserId ?? 0 : u?.UserId ?? 0;
        _setup.CurrentUserIsVerified = !explicitNonUser && !userIsNotVerified;

        var mediator = _scope.ServiceProvider.GetRequiredService<ISender>();
        _fileService = _scope.ServiceProvider.GetRequiredService<IFileService>();

        return await mediator.Send(request);
    }



    private async Task ClearFS()
    {
        await _fileService.ClearAll();
    }

    private async Task FillDB()
    {
        await StackblobDbContextSeed.SeedSampleData(_context);

        #region Populate Users DBContext Collection


        List<SocialTypeType> socialsTest = _context.SocialTypes.ToList() ?? new List<SocialTypeType>();

        Faker<SocialTypeType> socialFaker = new Faker<SocialTypeType>()
            .Rules((f, s) =>
            {
                var socialType = f.PickRandom(socialsTest);

                s.SocialTypeId = socialType!.SocialTypeId;
                s.Name = socialType!.Name;
                s.Desc = socialType!.Desc;
            });

        Faker<User> userFaker = new Faker<User>()
            .RuleFor(u => u.Firstname, f => f.Person.FirstName)
            .RuleFor(u => u.Lastname, f => f.Person.LastName)
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Socials, (f, u) => socialFaker.GenerateBetween(1, 3).DistinctBy(s => s.SocialTypeId).Select(s =>
                new UserSocialType()
                {

                    SocialTypeId = s.SocialTypeId,
                    User = u,
                    Url = f.Internet.Url()
                }).ToList())
            .RuleFor(u => u.Email, f => f.Internet.Email());


      
        _context.SaveChanges();

        foreach (var user in userFaker.Generate(20))
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            UserPasswords[user!.UserId] = user.Password;
            user.Salt = CryptoUtil.CreateSalt();
            user.Password = CryptoUtil.CreateHash(user.Salt + user.Password);
            user.EmailVerficiations.Add(new()
            {
                IsVerified = true,
                ExpiresAt = DateTimeUtil.Now(),
            });
            _context.SaveChanges();
        }

        DefaultUser = _context.Users.First();

        var tagNamePool = new List<string>() { "C#", "Java", "SQL", "Python", "AI", "PHP", "MonogDB", "Elastic-Search", "Rust", "C++" };
        var tagPool = tagNamePool.Select(t => new Tag() { Name = t });

        _context.Tags.AddRange(tagPool);
        _context.SaveChanges();




        var questionTitlePool = new List<string>()
         {
             "How can I optimize my code for better performance?","How do I implement a linked list in Python?","How do I debug a segmentation fault error in C++?","How do I create a RESTful API in Node.js?","How do I implement a binary search tree in Java?","How can I use regular expressions to parse text in Python?","How do I implement a stack data structure in C++?","How do I use the MVC design pattern in Ruby on Rails?","How do I use recursion to solve a problem in Python?","How do I implement a queue data structure in Java?","How do I use Git for version control?","How can I use machine learning to solve a problem in Python?","How do I use a makefile to build a C++ program?","How can I use SQL to query a database?","How do I use a debugger to find bugs in my code?","How can I use threading to improve the performance of my Python program?","How do I use a for loop to iterate through a list in Python?","How can I use a while loop to solve a problem in Python?","How do I use a switch statement in C++?","How can I use a try-catch block to handle exceptions in Java?","How do I use an if-else statement to control the flow of my program?","How can I use a function to solve a problem in Python?","How do I use an array to store data in C++?","How can I use a dictionary to store data in Python?","How do I use a class to structure my code in Java?","How can I use a module to organize my code in Python?","How do I use a pointer to manipulate data in C++?","How can I use an object to structure my code in Python?","How do I use inheritance to structure my code in Java?","How can I use polymorphism to structure my code in Python?","How do I use an abstract class to structure my code in C++?","How can I use an interface to structure my code in Java?","How do I use a constructor to initialize an object in C++?","How can I use a destructor to clean up resources in C++?","How do I use an operator overload to customize the behavior of operators in C++?","How can I use a template to create a generic function in C++?","How do I use a namespace to organize my code in C++?","How can I use an exception to handle errors in Python?","How do I use a lambda function to create a small anonymous function in Python?","How can I use a yield statement to create a generator in Python?","How do I use a decorator to add functionality to a function in Python?","How can I use a map function to transform a list in Python?","How do I use a filter function to select elements from a list in Python?","How can I use a reduce function to combine elements from a list in Python?","How do I use a zip function to combine elements from multiple lists in Python?","How can I use a set to store unique elements in Python?","How do I use a tuple to store multiple values","How can I use a named tuple to store multiple values in Python?","How do I use a list comprehension to create a list in Python?","How can I use a dictionary comprehension to create a dictionary in Python?","How do I use a set comprehension to create a set in Python?","How can I use a generator expression to create a generator in Python?","How do I use an async function to perform asynchronous tasks in Python?","How can I use an await statement to wait for a task to complete in Python?","How do I use a pandas dataframe to manipulate data in Python?","How can I use a numpy array to perform numerical computations in Python?","How do I use a matplotlib plot to visualize data in Python?","How can I use a seaborn plot to visualize data in Python?","How do I use a scikit-learn model to perform machine learning in Python?","How can I use a TensorFlow model to perform machine learning in Python?","How do I use a Keras model to perform deep learning in Python?","How can I use a PyTorch model to perform deep learning in Python?","How do I use a Flask framework to create a web application in Python?","How can I use a Django framework to create a web application in Python?","How do I use a Vue.js framework to create a web application in JavaScript?","How can I use a React.js framework to create a web application in JavaScript?","How do I use a Angular.js framework to create a web application in JavaScript?","How can I use a SASS preprocessor to style a web application?","How do I use a LESS preprocessor to style a web application?","How can I use a Gulp task runner to automate tasks in a web application?","How do I use a Webpack module bundler to optimize a web application?","How can I use a Jest testing framework to test a JavaScript application?","How do I use a Mocha testing framework to test a JavaScript application?","How can I use a Chai assertion library to test a JavaScript application?","How do I use a Cypress testing framework to test a web application?","How can I use an Enzyme testing library to test a React application?","How do I use a JUnit testing framework to test a Java application?","How can I use a TestNG testing framework to test a Java application?","How do I use a NUnit testing framework to test a .NET application?","How can I use a Cucumber BDD framework to write acceptance tests for a web application?","How do I use a Selenium webdriver to automate browser testing?","How can I use a Jenkins CI/CD pipeline to automate the deployment of my application?","How do I use a Docker container to deploy my application?","How can I use a Kubernetes cluster to manage my application's containers?","How do I use a Ansible script to automate server configuration?","How can I use a Terraform script to provision infrastructure?","How do I use a Gitlab CI/CD pipeline to automate the deployment of my application?","How can I use a AWS Lambda function to run serverless code?","How do I use a Azure Functions to run serverless code?","How can I use a Google Cloud Function to run serverless code?","How do I use a Firebase Cloud Firestore to store data in a web application?","How can I use a MongoDB database to store data in a web application?","How do I use a Cassandra database to store data in a web application?","How can I use a RabbitMQ message queue to handle async tasks in a web application?","How do I use a Kafka message queue to handle async tasks in a web application?","How can I use a Redis cache to improve the performance of a web application?","How do I use a Elasticsearch index to search data in a web application?","How can I use a Kafka Streams to process data streams in a web application?","How do I use a Apache Spark to process large data sets in a web application?","How can I use a TensorFlow.js to perform machine learning on the browser?"
         };

        var answerDescPool = new List<string>()
         {
             "How can I optimize my code for better performance?","How do I implement a linked list in Python?","How do I debug a segmentation fault error in C++?","How do I create a RESTful API in Node.js?","How do I implement a binary search tree in Java?","How can I use regular expressions to parse text in Python?","How do I implement a stack data structure in C++?","How do I use the MVC design pattern in Ruby on Rails?","How do I use recursion to solve a problem in Python?","How do I implement a queue data structure in Java?","How do I use Git for version control?","How can I use machine learning to solve a problem in Python?","How do I use a makefile to build a C++ program?","How can I use SQL to query a database?","How do I use a debugger to find bugs in my code?","How can I use threading to improve the performance of my Python program?","How do I use a for loop to iterate through a list in Python?","How can I use a while loop to solve a problem in Python?","How do I use a switch statement in C++?","How can I use a try-catch block to handle exceptions in Java?","How do I use an if-else statement to control the flow of my program?","How can I use a function to solve a problem in Python?","How do I use an array to store data in C++?","How can I use a dictionary to store data in Python?","How do I use a class to structure my code in Java?","How can I use a module to organize my code in Python?","How do I use a pointer to manipulate data in C++?","How can I use an object to structure my code in Python?","How do I use inheritance to structure my code in Java?","How can I use polymorphism to structure my code in Python?","How do I use an abstract class to structure my code in C++?","How can I use an interface to structure my code in Java?","How do I use a constructor to initialize an object in C++?","How can I use a destructor to clean up resources in C++?","How do I use an operator overload to customize the behavior of operators in C++?","How can I use a template to create a generic function in C++?","How do I use a namespace to organize my code in C++?","How can I use an exception to handle errors in Python?","How do I use a lambda function to create a small anonymous function in Python?","How can I use a yield statement to create a generator in Python?","How do I use a decorator to add functionality to a function in Python?","How can I use a map function to transform a list in Python?","How do I use a filter function to select elements from a list in Python?","How can I use a reduce function to combine elements from a list in Python?","How do I use a zip function to combine elements from multiple lists in Python?","How can I use a set to store unique elements in Python?","How do I use a tuple to store multiple values","How can I use a named tuple to store multiple values in Python?","How do I use a list comprehension to create a list in Python?","How can I use a dictionary comprehension to create a dictionary in Python?","How do I use a set comprehension to create a set in Python?","How can I use a generator expression to create a generator in Python?","How do I use an async function to perform asynchronous tasks in Python?","How can I use an await statement to wait for a task to complete in Python?","How do I use a pandas dataframe to manipulate data in Python?","How can I use a numpy array to perform numerical computations in Python?","How do I use a matplotlib plot to visualize data in Python?","How can I use a seaborn plot to visualize data in Python?","How do I use a scikit-learn model to perform machine learning in Python?","How can I use a TensorFlow model to perform machine learning in Python?","How do I use a Keras model to perform deep learning in Python?","How can I use a PyTorch model to perform deep learning in Python?","How do I use a Flask framework to create a web application in Python?","How can I use a Django framework to create a web application in Python?","How do I use a Vue.js framework to create a web application in JavaScript?","How can I use a React.js framework to create a web application in JavaScript?","How do I use a Angular.js framework to create a web application in JavaScript?","How can I use a SASS preprocessor to style a web application?","How do I use a LESS preprocessor to style a web application?","How can I use a Gulp task runner to automate tasks in a web application?","How do I use a Webpack module bundler to optimize a web application?","How can I use a Jest testing framework to test a JavaScript application?","How do I use a Mocha testing framework to test a JavaScript application?","How can I use a Chai assertion library to test a JavaScript application?","How do I use a Cypress testing framework to test a web application?","How can I use an Enzyme testing library to test a React application?","How do I use a JUnit testing framework to test a Java application?","How can I use a TestNG testing framework to test a Java application?","How do I use a NUnit testing framework to test a .NET application?","How can I use a Cucumber BDD framework to write acceptance tests for a web application?","How do I use a Selenium webdriver to automate browser testing?","How can I use a Jenkins CI/CD pipeline to automate the deployment of my application?","How do I use a Docker container to deploy my application?","How can I use a Kubernetes cluster to manage my application's containers?","How do I use a Ansible script to automate server configuration?","How can I use a Terraform script to provision infrastructure?","How do I use a Gitlab CI/CD pipeline to automate the deployment of my application?","How can I use a AWS Lambda function to run serverless code?","How do I use a Azure Functions to run serverless code?","How can I use a Google Cloud Function to run serverless code?","How do I use a Firebase Cloud Firestore to store data in a web application?","How can I use a MongoDB database to store data in a web application?","How do I use a Cassandra database to store data in a web application?","How can I use a RabbitMQ message queue to handle async tasks in a web application?","How do I use a Kafka message queue to handle async tasks in a web application?","How can I use a Redis cache to improve the performance of a web application?","How do I use a Elasticsearch index to search data in a web application?","How can I use a Kafka Streams to process data streams in a web application?","How do I use a Apache Spark to process large data sets in a web application?","How can I use a TensorFlow.js to perform machine learning on the browser?"
         };

        Faker<Answer> answerFaker = new Faker<Answer>()
         .Rules((f, a) =>
         {
             var desc = f.PickRandom(answerDescPool);

             a.Description = desc;
             a.Title = desc.Substring(0, Math.Min(desc.Length, 30));
         });

        var usersPool = _context.Users.ToList();

        Faker<Question> questionFaker = new Faker<Question>()
            .Rules((f, s) =>
            {
                s.Title = f.PickRandom(questionTitlePool);
                s.Description = f.PickRandom(questionTitlePool);
                //s.CreatedBy = f.PickRandom(_context.Users.ToList());
                //s.Answers = answerFaker.GenerateBetween(1, 3).DistinctBy(a => a.CreatedById).ToList();
                //s.QuestionVotes = f.PickRandom(usersPool, new Random().Next(1, usersPool.Count - 1)).DistinctBy(a => a.UserId).Select(u => new Vote()
                //{
                //    IsUpVote = new Random().Next(1, 4) < 70,
                //    CreateByInQuestionId = u.UserId
                //}).ToList();
                //s.CorrectAnswer = new Random().Next(1, 100) < 55 ? s.Answers.ToArray()[new Random().Next(0, s.Answers.Count)] : null;
                s.Tags = f.PickRandom(tagPool, new Random().Next(1, 5)).ToList();
            });

        foreach (var user in usersPool)
        {
            var questions = questionFaker.Generate(2);
            foreach(var question in questions) { question.CreatedBy = user; };
            user.Question.AddRange(questions);
        }
        _context.SaveChanges();

        var noCorrectAnswers = _context.Users.Select(q => q.Question.First().QuestionId).ToList();

        foreach(var user in usersPool)
        {
            var answers = answerFaker.Generate(3);
            user.Answers.AddRange(answers);

            var questions = _context.Questions.Where(q => q.CreatedById != user.UserId).OrderBy(q => q.Answers.Count).Take(3).ToList();


            bool hasCorrectQuestion = false;
            
            for(int i=0; i<questions.Count; i++)
            {
                answers[i].Comments.Add(new Comment()
                {
                    CreatedByInAnswerId = user.UserId,
                    Answer = answers[i],
                    Question = questions[i],
                    Description = "Test answer comment"
                });
                questions[i].Answers.Add(answers[i]);

                questions[i].QuestionVotes.Add(new Vote()
                {
                    IsUpVote = new Random().Next(1, 10) < 7,
                    CreateByInQuestionId = user.UserId
                })
                    ;
                questions[i].Comments.Add(new Comment()
                {
                    Question = questions[i],
                    CreatedByInQuestionId = user.UserId,
                    Description = "Test question comment"
                });

                if (!hasCorrectQuestion && !noCorrectAnswers.Contains(questions[i].QuestionId) && questions[i].CorrectAnswer == null) {
                    questions[i].CorrectAnswer = answers[i];
                    hasCorrectQuestion = true;
                }
            }
            _context.SaveChanges();
        }

        _context.SaveChanges();

        //foreach (var question in questionFaker.Generate(20))
        //{
        //    _context.Questions.Add(question);
        //}
        //_context.SaveChanges();

        var quests = _context.Questions.ToList();
        #region Populate Tags DBContext Collection

        _context.SaveChanges();
        #endregion

        //var answer5 = new Answer() { CreatedBy = user2, Title = "Answer Five", Description = "Content of Answer Five" };


        #region Populate Questions DBContext Collection
        //var question1 = new Question() { Title = "Question One", Description = "Content of Question one", CreatedBy = DefaultUser };
        //var question2 = new Question() { Title = "Question Two", Description = "Content of Question two", CreatedBy = DefaultUser };
        //var question3 = new Question() { Title = "Question Three", Description = "Content of Question three", CreatedBy = user2 };
        //_context.Questions.AddRange(question1, question2, question3);
        _context.SaveChanges();


        #region Populate Answers 

        //var answer1 = new Answer() { CreatedBy = user2, Title = "Answer One", Description = "Content of Answer One", Question = question1 };
        //var answer2 = new Answer() { CreatedBy = user3, Title = "Answer Two", Description = "Content of Answer Two", Question = question1 };
        //var answer3 = new Answer() { CreatedBy = user4, Title = "Answer Three", Description = "Content of Answer Three", Question = question1 };
        //var answer4 = new Answer() { CreatedBy = user5, Title = "Answer Four", Description = "Content of Answer Four", Question = question1 };

        //var answer5 = new Answer() { CreatedBy = user2, Title = "Answer 5", Description = "Content of 5", Question = question2 };
        //var answer6 = new Answer() { CreatedBy = user3, Title = "Answer 6", Description = "Content of Answer 6", Question = question2 };
        //var answer7 = new Answer() { CreatedBy = user4, Title = "Answer 7", Description = "Content of Answer 7", Question = question2 };
        //var answer8 = new Answer() { CreatedBy = user5, Title = "Answer 8", Description = "Content of Answer 8", Question = question2 };

        //var answer9 = new Answer() { CreatedBy = user1, Title = "Answer 9", Description = "Content of Answer 9", Question = question3 };
        //var answer10 = new Answer() { CreatedBy = user3, Title = "Answer 10", Description = "Content of Answer 10", Question = question3 };
        //var answer11 = new Answer() { CreatedBy = user4, Title = "Answer 11", Description = "Content of Answer 11", Question = question3 };
        //var answer12 = new Answer() { CreatedBy = user5, Title = "Answer 12", Description = "Content of Answer 12", Question = question3 };


        //question2.CorrectAnswer = answer7;

        //_context.Answers.AddRange(answer1, answer2, answer3, answer4, answer5, answer6, answer7, answer8, answer9, answer10, answer11, answer12);
        //_context.SaveChanges();

        #endregion

        #endregion
        #endregion


    }

    public async Task InitializeAsync()
    {
        await FillDB();
        await ClearFS();
    }

    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await Task.FromResult(0);
    }
}
