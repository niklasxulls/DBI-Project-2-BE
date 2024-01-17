using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using AutoMapper;
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
using Bogus;
using NuGet.Packaging;
using stackblob.Domain.Util;
using stackblob.Infrastructure.Util;
using MongoDB.Bson;
using stackblob.Domain.Settings;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using TagMongoREL = stackblob.Domain.Entities.MongoREL.TagMongoREL;
using stackblob.Domain.Entities.MongoREL;
using stackblob.Domain.Entities.SqlREL;
using stackblob.Domain.Entities.MongoFE;

namespace Application.IntegrationTests;

[Collection("global")]
public class TestBase : IAsyncLifetime
{
    private readonly SetupFixture _setup;
    protected readonly IServiceScope _scope;


    protected readonly IMapper _mapper;
    protected Dictionary<string, string> UserPasswords = new();
    private UserMongoREL _defaultUser;
    public UserMongoREL DefaultUserMongoREL
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

    protected ICollection<string> questionTitlePool = new List<string>()
         {
             "How can I optimize my code for better performance?","How do I implement a linked list in Python?","How do I debug a segmentation fault error in C++?","How do I create a RESTful API in Node.js?","How do I implement a binary search tree in Java?","How can I use regular expressions to parse text in Python?","How do I implement a stack data structure in C++?","How do I use the MVC design pattern in Ruby on Rails?","How do I use recursion to solve a problem in Python?","How do I implement a queue data structure in Java?","How do I use Git for version control?","How can I use machine learning to solve a problem in Python?","How do I use a makefile to build a C++ program?","How can I use SQL to query a database?","How do I use a debugger to find bugs in my code?","How can I use threading to improve the performance of my Python program?","How do I use a for loop to iterate through a list in Python?","How can I use a while loop to solve a problem in Python?","How do I use a switch statement in C++?","How can I use a try-catch block to handle exceptions in Java?","How do I use an if-else statement to control the flow of my program?","How can I use a function to solve a problem in Python?","How do I use an array to store data in C++?","How can I use a dictionary to store data in Python?","How do I use a class to structure my code in Java?","How can I use a module to organize my code in Python?","How do I use a pointer to manipulate data in C++?","How can I use an object to structure my code in Python?","How do I use inheritance to structure my code in Java?","How can I use polymorphism to structure my code in Python?","How do I use an abstract class to structure my code in C++?","How can I use an interface to structure my code in Java?","How do I use a constructor to initialize an object in C++?","How can I use a destructor to clean up resources in C++?","How do I use an operator overload to customize the behavior of operators in C++?","How can I use a template to create a generic function in C++?","How do I use a namespace to organize my code in C++?","How can I use an exception to handle errors in Python?","How do I use a lambda function to create a small anonymous function in Python?","How can I use a yield statement to create a generator in Python?","How do I use a decorator to add functionality to a function in Python?","How can I use a map function to transform a list in Python?","How do I use a filter function to select elements from a list in Python?","How can I use a reduce function to combine elements from a list in Python?","How do I use a zip function to combine elements from multiple lists in Python?","How can I use a set to store unique elements in Python?","How do I use a tuple to store multiple values","How can I use a named tuple to store multiple values in Python?","How do I use a list comprehension to create a list in Python?","How can I use a dictionary comprehension to create a dictionary in Python?","How do I use a set comprehension to create a set in Python?","How can I use a generator expression to create a generator in Python?","How do I use an async function to perform asynchronous tasks in Python?","How can I use an await statement to wait for a task to complete in Python?","How do I use a pandas dataframe to manipulate data in Python?","How can I use a numpy array to perform numerical computations in Python?","How do I use a matplotlib plot to visualize data in Python?","How can I use a seaborn plot to visualize data in Python?","How do I use a scikit-learn model to perform machine learning in Python?","How can I use a TensorFlow model to perform machine learning in Python?","How do I use a Keras model to perform deep learning in Python?","How can I use a PyTorch model to perform deep learning in Python?","How do I use a Flask framework to create a web application in Python?","How can I use a Django framework to create a web application in Python?","How do I use a Vue.js framework to create a web application in JavaScript?","How can I use a React.js framework to create a web application in JavaScript?","How do I use a Angular.js framework to create a web application in JavaScript?","How can I use a SASS preprocessor to style a web application?","How do I use a LESS preprocessor to style a web application?","How can I use a Gulp task runner to automate tasks in a web application?","How do I use a Webpack module bundler to optimize a web application?","How can I use a Jest testing framework to test a JavaScript application?","How do I use a Mocha testing framework to test a JavaScript application?","How can I use a Chai assertion library to test a JavaScript application?","How do I use a Cypress testing framework to test a web application?","How can I use an Enzyme testing library to test a React application?","How do I use a JUnit testing framework to test a Java application?","How can I use a TestNG testing framework to test a Java application?","How do I use a NUnit testing framework to test a .NET application?","How can I use a Cucumber BDD framework to write acceptance tests for a web application?","How do I use a Selenium webdriver to automate browser testing?","How can I use a Jenkins CI/CD pipeline to automate the deployment of my application?","How do I use a Docker container to deploy my application?","How can I use a Kubernetes cluster to manage my application's containers?","How do I use a Ansible script to automate server configuration?","How can I use a Terraform script to provision infrastructure?","How do I use a Gitlab CI/CD pipeline to automate the deployment of my application?","How can I use a AWS Lambda function to run serverless code?","How do I use a Azure Functions to run serverless code?","How can I use a Google Cloud Function to run serverless code?","How do I use a Firebase Cloud Firestore to store data in a web application?","How can I use a MongoDB database to store data in a web application?","How do I use a Cassandra database to store data in a web application?","How can I use a RabbitMQ message queue to handle async tasks in a web application?","How do I use a Kafka message queue to handle async tasks in a web application?","How can I use a Redis cache to improve the performance of a web application?","How do I use a Elasticsearch index to search data in a web application?","How can I use a Kafka Streams to process data streams in a web application?","How do I use a Apache Spark to process large data sets in a web application?","How can I use a TensorFlow.js to perform machine learning on the browser?"
         };
    protected ICollection<string> tagNamePool = new List<string>() { "C#", "Java", "SQL", "Python", "AI", "PHP", "MonogDB", "Elastic-Search", "Rust", "C++" };
    protected ICollection<string> answerDescPool = new List<string>()
         {
             "How can I optimize my code for better performance?","How do I implement a linked list in Python?","How do I debug a segmentation fault error in C++?","How do I create a RESTful API in Node.js?","How do I implement a binary search tree in Java?","How can I use regular expressions to parse text in Python?","How do I implement a stack data structure in C++?","How do I use the MVC design pattern in Ruby on Rails?","How do I use recursion to solve a problem in Python?","How do I implement a queue data structure in Java?","How do I use Git for version control?","How can I use machine learning to solve a problem in Python?","How do I use a makefile to build a C++ program?","How can I use SQL to query a database?","How do I use a debugger to find bugs in my code?","How can I use threading to improve the performance of my Python program?","How do I use a for loop to iterate through a list in Python?","How can I use a while loop to solve a problem in Python?","How do I use a switch statement in C++?","How can I use a try-catch block to handle exceptions in Java?","How do I use an if-else statement to control the flow of my program?","How can I use a function to solve a problem in Python?","How do I use an array to store data in C++?","How can I use a dictionary to store data in Python?","How do I use a class to structure my code in Java?","How can I use a module to organize my code in Python?","How do I use a pointer to manipulate data in C++?","How can I use an object to structure my code in Python?","How do I use inheritance to structure my code in Java?","How can I use polymorphism to structure my code in Python?","How do I use an abstract class to structure my code in C++?","How can I use an interface to structure my code in Java?","How do I use a constructor to initialize an object in C++?","How can I use a destructor to clean up resources in C++?","How do I use an operator overload to customize the behavior of operators in C++?","How can I use a template to create a generic function in C++?","How do I use a namespace to organize my code in C++?","How can I use an exception to handle errors in Python?","How do I use a lambda function to create a small anonymous function in Python?","How can I use a yield statement to create a generator in Python?","How do I use a decorator to add functionality to a function in Python?","How can I use a map function to transform a list in Python?","How do I use a filter function to select elements from a list in Python?","How can I use a reduce function to combine elements from a list in Python?","How do I use a zip function to combine elements from multiple lists in Python?","How can I use a set to store unique elements in Python?","How do I use a tuple to store multiple values","How can I use a named tuple to store multiple values in Python?","How do I use a list comprehension to create a list in Python?","How can I use a dictionary comprehension to create a dictionary in Python?","How do I use a set comprehension to create a set in Python?","How can I use a generator expression to create a generator in Python?","How do I use an async function to perform asynchronous tasks in Python?","How can I use an await statement to wait for a task to complete in Python?","How do I use a pandas dataframe to manipulate data in Python?","How can I use a numpy array to perform numerical computations in Python?","How do I use a matplotlib plot to visualize data in Python?","How can I use a seaborn plot to visualize data in Python?","How do I use a scikit-learn model to perform machine learning in Python?","How can I use a TensorFlow model to perform machine learning in Python?","How do I use a Keras model to perform deep learning in Python?","How can I use a PyTorch model to perform deep learning in Python?","How do I use a Flask framework to create a web application in Python?","How can I use a Django framework to create a web application in Python?","How do I use a Vue.js framework to create a web application in JavaScript?","How can I use a React.js framework to create a web application in JavaScript?","How do I use a Angular.js framework to create a web application in JavaScript?","How can I use a SASS preprocessor to style a web application?","How do I use a LESS preprocessor to style a web application?","How can I use a Gulp task runner to automate tasks in a web application?","How do I use a Webpack module bundler to optimize a web application?","How can I use a Jest testing framework to test a JavaScript application?","How do I use a Mocha testing framework to test a JavaScript application?","How can I use a Chai assertion library to test a JavaScript application?","How do I use a Cypress testing framework to test a web application?","How can I use an Enzyme testing library to test a React application?","How do I use a JUnit testing framework to test a Java application?","How can I use a TestNG testing framework to test a Java application?","How do I use a NUnit testing framework to test a .NET application?","How can I use a Cucumber BDD framework to write acceptance tests for a web application?","How do I use a Selenium webdriver to automate browser testing?","How can I use a Jenkins CI/CD pipeline to automate the deployment of my application?","How do I use a Docker container to deploy my application?","How can I use a Kubernetes cluster to manage my application's containers?","How do I use a Ansible script to automate server configuration?","How can I use a Terraform script to provision infrastructure?","How do I use a Gitlab CI/CD pipeline to automate the deployment of my application?","How can I use a AWS Lambda function to run serverless code?","How do I use a Azure Functions to run serverless code?","How can I use a Google Cloud Function to run serverless code?","How do I use a Firebase Cloud Firestore to store data in a web application?","How can I use a MongoDB database to store data in a web application?","How do I use a Cassandra database to store data in a web application?","How can I use a RabbitMQ message queue to handle async tasks in a web application?","How do I use a Kafka message queue to handle async tasks in a web application?","How can I use a Redis cache to improve the performance of a web application?","How do I use a Elasticsearch index to search data in a web application?","How can I use a Kafka Streams to process data streams in a web application?","How do I use a Apache Spark to process large data sets in a web application?","How can I use a TensorFlow.js to perform machine learning on the browser?"
         };


    /*
     * Mongo REL faker
     */
    protected readonly StackblobMongoRELDbContext _mongoContext;

    protected Faker<AnswerMongoREL> answerFakerMongoREL { get; set; }
    protected Faker<QuestionMongoREL> questionFakerMongoREL { get; set; }


    public ICollection<TagMongoREL> tagPoolMongoREL { get; set; }
    public ICollection<UserMongoREL> usersPoolMongoREL { get; set; }


    /*
    * Mongo FE faker 
    */
    protected Faker<QuestionMongoREL> questionFakerMongoFE { get; set; }


    public ICollection<TagMongoREL> tagPoolMongoFE { get; set; }
    public ICollection<UserMongoREL> usersPoolMongoFE { get; set; }



    /*
     * SQL REL faker
     */
    protected readonly StackblobSqlRELDbContext _sqlContext;

    protected Faker<AnswerSqlREL> answerFakerSqlREL { get; set; }
    protected Faker<QuestionSqlREL> questionFakerSqlREL { get; set; }
    public ICollection<TagSqlREL> tagPoolSqlREL { get; set; }
    public ICollection<UserSqlREL> usersPoolSqlREL { get; set; }



    public TestBase(SetupFixture setup)
    {
        _setup = setup;
        _scope = _setup._scopeFactory.CreateScope();

        _mongoContext = _scope.ServiceProvider.GetRequiredService<StackblobMongoRELDbContext>();
        _sqlContext = _scope.ServiceProvider.GetRequiredService<StackblobSqlRELDbContext>();

        _mapper = _scope.ServiceProvider.GetRequiredService<IMapper>();



        /*
         * Mongo REL
         */
        answerFakerMongoREL = new Faker<AnswerMongoREL>()
            .Rules((f, a) =>
            {
                var desc = f.PickRandom(answerDescPool);

                a.Description = desc;
                a.Title = desc.Substring(0, Math.Min(desc.Length, 30));
                a.CreatedById = f.PickRandom(usersPoolMongoREL).UserId;

            });

        questionFakerMongoREL = new Faker<QuestionMongoREL>()
            .Rules((f, s) =>
            {
                s.Title = f.PickRandom(questionTitlePool);
                s.Description = f.PickRandom(questionTitlePool);
                //s.Answers = answerFakerMongoREL.GenerateBetween(1, 3).ToList();
                s.CreatedById = f.PickRandom(usersPoolMongoREL).UserId;

                var tags = f.PickRandom(tagPoolMongoREL, new Random().Next(1, 5)).ToList();

                s.TagIds.AddRange(tags.Select(t => t.TagId));
            });



        /*
        * Mongo FE
        */
        questionFakerMongoFE = new Faker<QuestionMongoFE>()
            .Rules((f, s) =>
            {
                s.Title = f.PickRandom(questionTitlePool);
                s.Description = f.PickRandom(questionTitlePool);
                s.Answers = answerFakerMongoREL.GenerateBetween(1, 3).ToList();
                s.CreatedById = f.PickRandom(usersPoolMongoREL).UserId;

                var tags = f.PickRandom(tagPoolMongoREL, new Random().Next(1, 5)).ToList();

                s.TagIds.AddRange(tags.Select(t => t.TagId));
            });


        /*
        * Clear MONGO DB
        **/
        var client = new MongoClient(GlobalUtil.ConnectionString);

        var database = client.GetDatabase(GlobalUtil.MongoDbName);

        var allCollections = database.ListCollectionNames();
        allCollections.MoveNext();

        foreach (var collection in allCollections.Current)
        {
            database.DropCollection(collection);
        }


        /*
        * SQL REL
        */
        answerFakerSqlREL = new Faker<AnswerSqlREL>()
            .Rules((f, a) =>
            {
                var desc = f.PickRandom(answerDescPool);

                a.Description = desc;
                a.Title = desc.Substring(0, Math.Min(desc.Length, 30));
                a.CreatedBy = f.PickRandom(usersPoolSqlREL);

            });

        questionFakerSqlREL = new Faker<QuestionSqlREL>()
            .Rules((f, s) =>
            {
                s.Title = f.PickRandom(questionTitlePool);
                s.Description = f.PickRandom(questionTitlePool);
                s.Answers = answerFakerSqlREL.GenerateBetween(1, 3).ToList();
                s.CreatedBy = f.PickRandom(usersPoolSqlREL);

                var tags = f.PickRandom(tagPoolSqlREL, new Random().Next(1, 5)).ToList();

                s.Tags.AddRange(tags.Select(t =>
                {
                    return new QuestionTagSqlREL()
                    {
                        Tag = t,
                    };
                }));
            });

        _sqlContext.Database.EnsureDeleted();
        _sqlContext.Database.EnsureCreated();
    }

    public async Task<TResponse> SendMediator<TResponse>(IRequest<TResponse> request, UserMongoREL? u = null, bool explicitNonUser = false, bool userIsNotVerified = false)
    {
        _setup.CurrentUserId = u == null && !explicitNonUser ? "" : "";
        _setup.CurrentUserIsVerified = !explicitNonUser && !userIsNotVerified;

        var mediator = _scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }



    private async Task ClearFS()
    {
    }

    private async Task FillDB()
    {
        //await StackblobDbContextSeed.SeedSampleData(_mongoContext);


        /*
         *  Mongo REL
         */
        // users
        Faker<UserMongoREL> userFakerMongo = new Faker<UserMongoREL>()
            .RuleFor(u => u.Firstname, f => f.Person.FirstName)
            .RuleFor(u => u.Lastname, f => f.Person.LastName)
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Email, f => f.Internet.Email());


        foreach (var user in userFakerMongo.Generate(20))
        {
            try
            {
                _mongoContext.UsersMongoREL.Add(user);
                await _mongoContext.SaveChangesAsync(default);

                UserPasswords[user!.UserId] = user.Password;
                user.Salt = CryptoUtil.CreateSalt();
                user.Password = CryptoUtil.CreateHash(user.Salt + user.Password);
                _mongoContext.SaveChanges();
            } catch(Exception e)
            {
                var x = 0;
            }
        }

        DefaultUserMongoREL = _mongoContext.UsersMongoREL.First();
        usersPoolMongoREL = _mongoContext.UsersMongoREL.ToList();

        // tags
        tagPoolMongoREL = tagNamePool.Select(t => new TagMongoREL() { Name = t }).ToList();
        _mongoContext.TagsMongoREL.AddRange(tagPoolMongoREL);
        _mongoContext.SaveChanges();


        /*
         *  SQL REL
         */
        // users
        Faker<UserSqlREL> userFakerSql = new Faker<UserSqlREL>()
            .RuleFor(u => u.Firstname, f => f.Person.FirstName)
            .RuleFor(u => u.Lastname, f => f.Person.LastName)
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Email, f => f.Internet.Email());


        foreach (var user in userFakerSql.Generate(20))
        {
            try
            {
                _sqlContext.UsersSqlREL.Add(user);
                await _sqlContext.SaveChangesAsync(default);

                user.Salt = CryptoUtil.CreateSalt();
                user.Password = CryptoUtil.CreateHash(user.Salt + user.Password);
                _sqlContext.SaveChanges();
            }
            catch (Exception e)
            {
                var x = 0;
            }
        }
        usersPoolSqlREL = _sqlContext.UsersSqlREL.ToList();

        // tags
        tagPoolSqlREL = tagNamePool.Select(t => new TagSqlREL() { Name = t }).ToList();
        _sqlContext.TagsSqlREL.AddRange(tagPoolSqlREL);
        _sqlContext.SaveChanges();
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
