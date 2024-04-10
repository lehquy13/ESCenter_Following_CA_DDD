using System.Diagnostics;
using System.Reflection;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Courses.Entities;
using ESCenter.Domain.Aggregates.Discoveries;
using ESCenter.Domain.Aggregates.Discoveries.Entities;
using ESCenter.Domain.Aggregates.Discoveries.ValueObjects;
using ESCenter.Domain.Aggregates.DiscoveryUsers;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Persistence.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ESCenter.DBMigrator;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var factory = new AppDbContextFactory();
        var context = factory.CreateDbContext(args);

        var choice = 0;

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
        }

        switch (choice)
        {
            case 1:
                await context.Database.EnsureDeletedAsync();
                return;
            case 2:
                await context.Database.MigrateAsync();
                await SeedData(context);
                return;
            case 3:
                await SeedData(context);
                return;
            case 4:
                await context.Database.EnsureDeletedAsync();
                await context.Database.MigrateAsync();
                await SeedData(context);
                return;
            case 5:
                await context.Database.MigrateAsync();
                await SeedData(context);
                return;
            default:
                Console.WriteLine("Cancel");
                return;
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

                var somethingCalledMagic = new JsonSerializerSettings
                {
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                    ContractResolver = new PrivateResolver()
                };

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
                };

                context.Roles.AddRange(identityRoles);

                #endregion

                await context.SaveChangesAsync();

                #region Discovery

                var discoveries = Discoveries(subjects);

                context.Discoveries.AddRange(discoveries);

                #endregion

                #region Account

                List<Customer> userData = [];
                List<Tutor> tutorData = [];
                List<IdentityUserRole<string>> userRoles = [];

                await GetUserDataAndTutorData(
                    somethingCalledMagic,
                    userManager,
                    identityRoles,
                    userData,
                    tutorData,
                    userRoles);

                SeedTutorMajor(tutorData, subjects);

                #region Seed user discoveries

                var i = userData.Count;
                var duList = new List<DiscoveryUser>();

                foreach (var user in userData)
                {
                    var random = new Random();
                    var existing = new List<int>();
                    for (var j = 0; j < random.Next(0, 3); j++)
                    {
                        var index = random.Next(0, 6);
                        if (existing.Contains(index))
                        {
                            j--;
                            continue;
                        }

                        existing.Add(index);
                        duList.Add(DiscoveryUser.Create(discoveries[index].Id, user.Id));
                    }
                }

                context.DiscoveryUsers.AddRange(duList);

                #endregion

                context.Customers.AddRange(userData);
                await context.SaveChangesAsync();

                context.Tutors.AddRange(tutorData);
                context.UserRoles.AddRange(userRoles);

                #endregion

                #region Courses

                var file = await File.ReadAllTextAsync(
                    Path.GetFullPath("../../../1000_course.json"));

                var courseData = JsonConvert.DeserializeObject<List<Course>>(file, somethingCalledMagic);

                file = await File.ReadAllTextAsync(
                    Path.GetFullPath("../../../100_review.json"));

                var reviews = JsonConvert.DeserializeObject<List<Review>>(file, somethingCalledMagic);

                if (courseData == null || reviews == null)
                {
                    return;
                }

                // handle 100 course that have account
                foreach (var course in courseData)
                {
                    // set learner
                    var randomNumber = new Random().Next(0, 50);
                    var user = userData[randomNumber];
                    course.SetLearner(user.GetFullName(), user.Gender, user.PhoneNumber);
                    course.SetLearnerId(user.Id);

                    if (course.Status != Status.Confirmed) continue;

                    // assign tutor
                    var randomTutor = new Random().Next(0, 50);
                    var tutor = tutorData[randomTutor];
                    course.AssignTutor(tutor.Id);

                    // review?
                    var randomReview = reviews[new Random().Next(0, 99)];
                    course.ReviewCourse(
                        randomReview.Rate,
                        randomReview.Detail);
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

    private static void SeedTutorMajor(List<Tutor> tutorData, IReadOnlyList<Subject> subjects)
    {
        var ii = tutorData.Count;

        foreach (var tutor in tutorData)
        {
            if (ii-- <= 0) break;
            var tmList = new List<TutorMajor>();

            List<int> addedList = [];
            var randomQuantity = new Random().Next(1, subjects.Count / 2);

            for (var j = 0; j < randomQuantity; j++)
            {
                var random = new Random().Next(0, subjects.Count - 1);

                if (addedList.Contains(random))
                {
                    j--;
                    continue;
                }

                tmList.Add(
                    TutorMajor.Create(
                        tutor.Id,
                        SubjectId.Create(random + 1),
                        subjects[random].Name
                    )
                );

                // Ensure no duplicate
                addedList.Add(random);
            }

            tutor.UpdateAllMajor(tmList);
        }
    }

    private static async Task GetUserDataAndTutorData(
        JsonSerializerSettings somethingCalledMagic,
        UserManager<IdentityUser> userManager,
        List<IdentityRole> identityRoles,
        List<Customer> userData,
        List<Tutor> tutorData,
        List<IdentityUserRole<string>> userRoles)

    {
        // Standard user
        var file = await File.ReadAllTextAsync(Path.GetFullPath("../../../2000_m_learner.json"));
        var learner2000M = JsonConvert.DeserializeObject<List<Customer>>(file, somethingCalledMagic)!;

        if (userData == null)
        {
            throw new InvalidOperationException();
        }

        userData.AddRange(learner2000M);

        file = await File.ReadAllTextAsync(Path.GetFullPath("../../../2000_f_learner.json"));
        var learner2000F = JsonConvert.DeserializeObject<List<Customer>>(file, somethingCalledMagic);

        if (learner2000F == null)
        {
            throw new InvalidOperationException();
        }

        userData.AddRange(learner2000F);

        // Tutor
        file = await File.ReadAllTextAsync(Path.GetFullPath("../../../2000_f_tutor.json"));
        var customerTutorData1 = JsonConvert.DeserializeObject<List<Customer>>(file, somethingCalledMagic);

        if (customerTutorData1 == null)
        {
            throw new InvalidOperationException();
        }

        var tutorData2 = JsonConvert.DeserializeObject<List<Tutor>>(file, somethingCalledMagic)!;

        if (tutorData2 == null)
        {
            throw new InvalidOperationException();
        }

        file = await File.ReadAllTextAsync(Path.GetFullPath("../../../2000_m_tutor.json"));
        var customerTutorData2 = JsonConvert.DeserializeObject<List<Customer>>(file, somethingCalledMagic);

        if (customerTutorData2 == null)
        {
            throw new InvalidOperationException();
        }

        var tutorData1 = JsonConvert.DeserializeObject<List<Tutor>>(file,
            new JsonSerializerSettings()
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = new PrivateResolver()
            }
        )!;

        if (tutorData1 == null)
        {
            throw new InvalidOperationException();
        }


        customerTutorData1.AddRange(customerTutorData2);
        userData.AddRange(customerTutorData1);

        var identities = new List<IdentityUser>();
        var count = 1;

        await Task.WhenAll(userData.Select(async customer =>
        {
            var watch = Stopwatch.StartNew();

            var identityUser = new IdentityUser
            {
                UserName = customer.Email,
                NormalizedUserName = customer.Email.ToUpper(),
                Email = customer.Email,
                NormalizedEmail = customer.Email.ToUpper(),
                PhoneNumber = customer.PhoneNumber,
                Id = customer.Id.ToString()
            };

            // Perform async user creation
            await userManager.CreateAsync(identityUser, "1q2w3E**");

            // Add identityUser to the list in a thread-safe manner
            lock (identities)
            {
                identities.Add(identityUser);
            }

            // Increment count atomically
            Console.WriteLine($"{count++}. User {identityUser.Email} created in {watch.ElapsedMilliseconds}ms");
        }));

        userRoles.AddRange(identities.Select((identityUser, index) => new IdentityUserRole<string>
        {
            UserId = identityUser.Id,
            RoleId = index >= userData.Count ? identityRoles[1].Id : identityRoles.Last().Id
        }));


        tutorData.AddRange(tutorData1);
        tutorData.AddRange(tutorData2);

        var i = 0;
        foreach (var tutor in tutorData)
        {
            tutor.SetUserId(customerTutorData1[i++].Id);
        }
    }

    private static IList<Discovery> Discoveries(IReadOnlyList<Subject> subjects)
    {
        var discovery1 = Discovery.Create("Information Technology",
            "The study of information and computational systems",
            [
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(1), subjects[0].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(2), subjects[1].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(3), subjects[2].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(4), subjects[3].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(5), subjects[4].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(6), subjects[5].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(7), subjects[6].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(8), subjects[7].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(12), subjects[11].Name),
            ]);
        var discovery2 = Discovery.Create("Programming",
            "Basic principles and concepts of programming",
            [
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(2), subjects[1].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(3), subjects[2].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(4), subjects[3].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(5), subjects[4].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(6), subjects[5].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(7), subjects[6].Name),
            ]);

        var discovery3 = Discovery.Create("Language",
            "Study of language and culture",
            [
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(10), subjects[9].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(11), subjects[10].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(12), subjects[11].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(13), subjects[12].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(14), subjects[13].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(15), subjects[14].Name),
            ]);

        var discovery4 = Discovery.Create("Art",
            "Study of art and culture",
            [
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(16), subjects[15].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(17), subjects[16].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(18), subjects[17].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(19), subjects[18].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(20), subjects[19].Name),
            ]);

        var discovery5 = Discovery.Create("Fitness and Health",
            "Study of fitness and health",
            [
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(21), subjects[20].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(22), subjects[21].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(23), subjects[22].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(24), subjects[23].Name),
            ]);

        var discovery6 = Discovery.Create("Science",
            "Study of science",
            [
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(24), subjects[23].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(25), subjects[24].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(26), subjects[25].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(27), subjects[26].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(29), subjects[28].Name),
            ]);

        var discovery7 = Discovery.Create("Social",
            "Study of social",
            [
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(28), subjects[27].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(29), subjects[28].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(30), subjects[29].Name),
                DiscoverySubject.Create(DiscoveryId.Create(), SubjectId.Create(31), subjects[30].Name),
            ]);

        var discoveries = new List<Discovery>()
        {
            discovery1,
            discovery2,
            discovery3,
            discovery4,
            discovery5,
            discovery6,
            discovery7
        };
        return discoveries;
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
}