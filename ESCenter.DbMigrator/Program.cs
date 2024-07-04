using System.Reflection;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.ValueObjects;
using ESCenter.Domain.Aggregates.Discoveries;
using ESCenter.Domain.Aggregates.DiscoveryUsers;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.TutorRequests;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Persistence.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable PossibleMultipleEnumeration

namespace ESCenter.DBMigrator;

internal static class Program
{
    private static readonly JsonSerializerSettings SomethingCalledMagic = new()
    {
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        ContractResolver = new PrivateResolver()
    };

    public static async Task Main(string[] args)
    {
        var choice = 0;

        while (choice != 6)
        {
            Console.WriteLine("database: ");
            var dbName = Console.ReadLine();

            Console.WriteLine("isLocal: ");
            bool.TryParse(Console.ReadLine(), out var isLocal);

            var factory = new AppDbContextFactory();
            var context = factory.CreateDbContext(string.IsNullOrWhiteSpace(dbName) ? "esmssql" : dbName, isLocal);

            Console.WriteLine("1. Delete database");
            Console.WriteLine("2. Migrate database");
            Console.WriteLine("3. Seed database");
            Console.WriteLine("4. Delete then migrate then seed database");
            Console.WriteLine("5. Migrate then seed database");
            Console.WriteLine("6. Cancel...");

            Console.WriteLine("Enter your choice: ");

            try
            {
                choice = int.Parse(Console.ReadLine()!);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                choice = 7;
            }

            switch (choice)
            {
                case 1:
                    await context.Database.EnsureDeletedAsync();
                    break;
                case 2:
                    await context.Database.MigrateAsync();
                    await SeedData(context);
                    break;
                case 3:
                    await SeedData(context);
                    break;
                case 4:
                    await context.Database.EnsureDeletedAsync();
                    await context.Database.MigrateAsync();
                    await SeedData(context);
                    break;
                case 5:
                    await context.Database.MigrateAsync();
                    await SeedData(context);
                    break;
                case 6:
                    Console.WriteLine("Cancel");
                    break;
                default:
                    Console.WriteLine("Invalid");
                    break;
            }
        }
    }

    private static async Task SeedData(AppDbContext context)
    {
        var userStore = new UserStore<IdentityUser>(context)
        {
            AutoSaveChanges = false
        };
        UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(
            userStore,
            null!,
            new PasswordHasher<IdentityUser>(),
            null!,
            null!,
            null!,
            null!,
            null!,
            null!);
        try
        {
            Console.WriteLine("Checking subject table is migrated or not...");

            // Look for any subjects.
            if (!context.Subjects.Any())
            {
                Console.WriteLine("No subjects found. Seeding subjects...");

                #region subject data

                var programming = Subject.Create("Programming", "Basic principles and concepts of programming");
                var java = Subject.Create("Java programming", "Object-oriented programming language");
                var informatics = Subject.Create("Informatics", "Study of information and computational systems");
                var otherSubject = Subject.Create("Other", "General category for other subjects");
                var korean = Subject.Create("Korean", "Language and culture of Korea");
                var spanish = Subject.Create("Spanish", "Language and culture of Spain");
                var vietnamese = Subject.Create("Vietnamese for foreigners",
                    "Teaching Vietnamese to non-native speakers");
                var german = Subject.Create("German", "Language and culture of Germany");
                var english = Subject.Create("English", "International language for communication");
                var guitar = Subject.Create("Guitar", "Musical instrument - Guitar playing and techniques");
                var chemistry = Subject.Create("Chemistry", "Study of matter, its properties, and transformations");
                var dance = Subject.Create("Dance", "Art form involving movement of the body");
                var piano = Subject.Create("Piano", "Musical instrument - Piano playing and techniques");
                var fitness = Subject.Create("Fitness", "Physical health and exercises");
                var painting = Subject.Create("Painting", "Art of applying paint, pigment, color");
                var mathematics = Subject.Create("Mathematics", "Study of numbers, quantity, shapes, and patterns");
                var physics = Subject.Create("Physics", "The study of matter, motion, energy, and force");
                var biology = Subject.Create("Biology",
                    "The study of living organisms and their interactions with one another and their environments");
                var computerS = Subject.Create("Computer Science", "The study of computers and computational systems");
                var fineArt = Subject.Create("Fine Art",
                    "The expression or application of human creative skill and imagination");
                var literature = Subject.Create("Literature",
                    "Written works, especially those considered of superior or lasting artistic merit");
                var history = Subject.Create("History", "The study of past events, particularly in human affairs");
                var engineering = Subject.Create("Engineering",
                    "The application of scientific and mathematical principles to design and build machines and structures");
                var technology = Subject.Create("Technology",
                    "The application of scientific knowledge for practical purposes");
                var politics = Subject.Create("Politics",
                    "The activities associated with governance and decision-making within groups or states");
                var psychology = Subject.Create("Psychology", "The study of behavior and mind");
                var economics = Subject.Create("Economics",
                    "The study of how societies allocate resources and make choices to satisfy their needs and wants");
                var physicalEducation =
                    Subject.Create("Physical Education", "Instruction in physical exercise and games");
                var csharp = Subject.Create("C# programming",
                    "A powerful, object-oriented programming language for building applications");
                var python = Subject.Create("Python programming",
                    "A high-level, interpreted language for general-purpose programming");
                var webProgramming = Subject.Create("Web programming",
                    "Creating websites and applications for the internet");
                var htmlCssJs = Subject.Create("HTML,CSS & Javascript",
                    "Languages used for building web pages and web applications");


                var subjects = new List<Subject>()
                {
                    informatics, // Id = 1, Index = 0, Name = "Informatics"
                    programming, // Id = 2, Index = 1, Name = "Programming"
                    java, // Id = 3, Index = 2, Name = "Java programming"
                    csharp, // Id = 4, Index = 3, Name = "C# programming"
                    python, // Id = 5, Index = 4, Name = "Python programming"
                    webProgramming, // Id = 6, Index = 5, Name = "Web programming"
                    htmlCssJs, // Id = 7, Index = 6, Name = "HTML,CSS & Javascript"
                    computerS, // Id = 8, Index = 7, Name = "Computer Science"
                    technology, // Id = 9, Index = 8, Name = "Technology"
                    german, // Id = 10, Index = 9, Name = "German"
                    korean, // Id = 11, Index = 10, Name = "Korean"
                    vietnamese, // Id = 12, Index = 11, Name = "Vietnamese for foreigners"
                    spanish, // Id = 13, Index = 12, Name = "Spanish"
                    english, // Id = 14, Index = 13, Name = "English"
                    guitar, // Id = 15, Index = 14, Name = "Guitar"
                    dance, // Id = 16, Index = 15, Name = "Dance"
                    piano, // Id = 17, Index = 16, Name = "Piano"
                    fineArt, // Id = 18, Index = 17, Name = "Fine Art"
                    literature, // Id = 19, Index = 18, Name = "Literature"
                    painting, // Id = 20, Index = 19, Name = "Painting"
                    physicalEducation, // Id = 21, Index = 20, Name = "Physical Education"
                    fitness, // Id = 22, Index = 21, Name = "Fitness"
                    biology, // Id = 23, Index = 22, Name = "Biology"
                    chemistry, // Id = 24, Index = 23, Name = "Chemistry"
                    mathematics, // Id = 25, Index = 24, Name = "Mathematics"
                    physics, // Id = 26, Index = 25, Name = "Physics"
                    engineering, // Id = 27, Index = 26, Name = "Engineering"
                    history, // Id = 28, Index = 27, Name = "History"
                    politics, // Id = 29, Index = 28, Name = "Politics"
                    psychology, // Id = 30, Index = 29, Name = "Psychology"
                    economics, // Id = 31, Index = 30, Name = "Economics"
                    otherSubject,
                };

                context.Subjects.AddRange(subjects);

                #endregion

                #region roles

                var identityRoles = new List<IdentityRole>
                {
                    new("Admin")
                    {
                        NormalizedName = "ADMIN"
                    },
                    new("Tutor")
                    {
                        NormalizedName = "TUTOR"
                    },
                    new("Learner")
                    {
                        NormalizedName = "LEARNER"
                    },
                    new("SuperAdmin")
                    {
                        NormalizedName = "SUPERADMIN"
                    }
                };

                context.Roles.AddRange(identityRoles);

                #endregion

                await context.SaveChangesAsync();

                #region Discovery

                var discoveries = Discovery.Discoveries();

                context.Discoveries.AddRange(discoveries);

                #endregion

                #region Account

                List<Customer> userData = [];
                List<Tutor> tutorData = [];
                List<TutorRequest> tutorRequestDatas = [];
                List<IdentityUserRole<string>> userRoles = [];

                await SeedTutor.GetUserDataAndTutorData(
                    SomethingCalledMagic,
                    userManager,
                    identityRoles,
                    userData,
                    tutorData,
                    tutorRequestDatas,
                    userRoles);

                SeedTutor.SeedMajor(tutorData, subjects);

                #region Seed user discoveries

                var duList = new List<DiscoveryUser>();

                foreach (var user in userData)
                {
                    var random = new Random();
                    var existing = new List<int>();
                    for (var j = 0; j < random.Next(1, 3); j++)
                    {
                        var index = random.Next(0, 6);
                        if (existing.Contains(index))
                        {
                            j--;
                            continue;
                        }

                        existing.Add(index);
                        duList.Add(DiscoveryUser.Create(discoveries.ElementAt(index).Id, user.Id));
                    }
                }

                context.DiscoveryUsers.AddRange(duList);

                #endregion

                context.Customers.AddRange(userData);
                await context.SaveChangesAsync();

                context.Tutors.AddRange(tutorData);
                context.UserRoles.AddRange(userRoles);
                context.TutorRequests.AddRange(tutorRequestDatas);

                #endregion

                #region Courses

                var file = await File.ReadAllTextAsync(
                    Path.GetFullPath("../../../1000_course.json"));

                var courseData = JsonConvert.DeserializeObject<List<Course>>(file, SomethingCalledMagic);

                if (courseData == null)
                {
                    Console.WriteLine("Course data is null");
                    return;
                }

                file = await File.ReadAllTextAsync(
                    Path.GetFullPath("../../../1000_confirmedCourse.json"));

                courseData.AddRange(JsonConvert.DeserializeObject<List<Course>>(file, SomethingCalledMagic)!);

                file = await File.ReadAllTextAsync(
                    Path.GetFullPath("../../../100_review.json"));

                var reviews = JsonConvert.DeserializeObject<List<Review>>(file, SomethingCalledMagic);

                if (reviews == null)
                {
                    return;
                }

                int courseCount = 1;
                var seedTutor =  tutorData.Take(tutorData.Count * 2 / 3).ToList();
                var seedTutorNumbers =  tutorData.Count * 2 / 3;
                // handle 100 course that have account
                foreach (var course in courseData)
                {
                    Console.WriteLine($"{courseCount++} course: {course.Title}");
                    // set learner
                    var randomNumber = new Random().Next(0, 50);
                    var user = userData[randomNumber];
                    course.SetLearner(user.GetFullName(), user.Gender, user.PhoneNumber);
                    course.SetLearnerId(user.Id);
                    
                    if (course.Status != Status.Confirmed) continue;

                    // assign tutor
                    var randomTutor = new Random().Next(0, seedTutorNumbers);
                    var tutor = seedTutor[randomTutor];
                    course.AssignTutor(tutor.Id);
                    course.ConfirmedCourse();
                    // review
                    var randomReview = reviews[new Random().Next(0, 99)];
                    course.ReviewCourse(
                        randomReview.Rate,
                        randomReview.Detail);
                }

                // Iterate through first 50 tutors to calculate the average rate
                for (var index = 0; index < seedTutorNumbers; index++)
                {
                    var tutor = seedTutor[index];
                    var totalRate = 0;
                    var totalReview = 0;
                    foreach (var course in courseData
                                 .Where(course => course.TutorId == tutor.Id
                                                  && course.Review != null))
                    {
                        totalRate += course.Review!.Rate;
                        totalReview++;
                    }

                    if (totalReview == 0) continue;

                    tutor.UpdateRate((float)totalRate / totalReview);
                }

                context.Courses.AddRange(courseData);

                #endregion

                # region Seed course request

                // Currently, we not handle this
                // file = File.ReadAllText(Path.GetFullPath("../../../request_course_random.json"));
                // var requestData = JsonConvert.DeserializeObject<List<CourseRequest>>(file, somethingCalledMagic);
                // if (requestData == null)
                // {
                //     return;
                // }
                //
                // requestData = requestData.OrderBy(x => x.TutorId).ToList();
                //
                // context.CourseRequests.AddRange(requestData);

                #endregion

                await context.SaveChangesAsync();

                Console.WriteLine("All done! Enjoy my website!");
            }
            else
            {
                Console.WriteLine("Nothing is added");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await context.Database.EnsureDeletedAsync();
        }
    }

    private class PrivateResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (prop.Writable)
            {
                return prop;
            }

            var property = member as PropertyInfo;

            if (property != null)
            {
                prop.Writable = property.GetSetMethod(true) != null;
                var hasPrivateSetter = property.GetSetMethod(true) != null;
                prop.Writable = hasPrivateSetter;

                // If property is CreationTime, LastModificationTime, etc. then make it random from 2 months ago to now
                if (property.Name is "CreationTime" or "LastModificationTime")
                {
                    var defaultValueProvider = prop.ValueProvider;

                    prop.ValueProvider = new MyValueProvider(defaultValueProvider!);
                }
            }
            else
            {
                var fieldInfo = member as FieldInfo;
                if (fieldInfo != null)
                {
                    prop.Writable = true;
                }
            }

            return prop;
        }

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            // Include both properties and private fields
            return base.GetSerializableMembers(objectType)
                .Concat(objectType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                .ToList();
        }
    }

    private class MyValueProvider(IValueProvider valueProvider) : IValueProvider
    {
        public void SetValue(object target, object? value)
        {
            //This is not executed during deserialization. Why?
            var randomDay = RandomDay();
            valueProvider.SetValue(target, randomDay);
            Console.WriteLine($"Value set: {value}");
        }

        public object? GetValue(object target)
        {
            var value = valueProvider.GetValue(target);
            Console.WriteLine($"Value get: {value}");
            return value;
        }

        private static readonly Random Gen = new Random();

        private DateTime RandomDay()
        {
            DateTime start = DateTime.Now.AddMonths(-2);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(Gen.Next(range));
        }
    }
}