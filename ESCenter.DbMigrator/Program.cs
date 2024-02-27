using System.Reflection;
using ESCenter.Domain.Aggregates.Courses;
using ESCenter.Domain.Aggregates.Discoveries;
using ESCenter.Domain.Aggregates.Discoveries.Entities;
using ESCenter.Domain.Aggregates.Discoveries.ValueObjects;
using ESCenter.Domain.Aggregates.DiscoveryUsers;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Users;
using ESCenter.Domain.Aggregates.Users.Identities;
using ESCenter.Domain.Shared.Courses;
using ESCenter.Persistence.Entity_Framework_Core;
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

        int choice = 0;

        Console.WriteLine("1. Delete database");
        Console.WriteLine("2. Migrate database");
        Console.WriteLine("3. Seed database");
        Console.WriteLine("4. Delete then migrate then seed database");
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
            default:
                Console.WriteLine("Cancel");
                return;
        }
    }

    private static async Task SeedData(AppDbContext context)
    {
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

                var file = await File.ReadAllTextAsync(Path.GetFullPath("../../../3_role.json"));
                var identityRoles = JsonConvert.DeserializeObject<List<IdentityRole>>(file, somethingCalledMagic);

                if (identityRoles == null)
                {
                    return;
                }

                context.IdentityRoles.AddRange(identityRoles);

                #endregion

                await context.SaveChangesAsync();

                #region Discovery

                var discoveries = Discoveries(subjects);

                context.Discoveries.AddRange(discoveries);

                #endregion

                #region Account

                GetUserDataAndTutorData(
                    somethingCalledMagic,
                    identityRoles,
                    out var userData,
                    out var tutorData,
                    out var identityUsers);


                SeedTutorMajor(tutorData, subjects);

                #region Seed user discoveries

                // TODO: seed discovery for user
                var i = userData.Count;
                var duList = (from user in userData.TakeWhile(user => i-- != 0)
                    let random = new Random().Next(0, 6)
                    select DiscoveryUser.Create(discoveries[random].Id, user.Id)).ToList();

                context.DiscoveryUsers.AddRange(duList);

                #endregion


                context.Users.AddRange(userData);
                context.Tutors.AddRange(tutorData);
                context.IdentityUsers.AddRange(identityUsers);

                #endregion

                #region courses

                file = await File.ReadAllTextAsync(
                    Path.GetFullPath("../../../15_random_courses_female_noAcc.json"));

                var courseData = JsonConvert.DeserializeObject<List<Course>>(
                    file, somethingCalledMagic);

                if (courseData == null)
                {
                    return;
                }

                file = await File.ReadAllTextAsync(Path.GetFullPath("../../../15_random_courses_male_noAcc.json"));
                courseData.AddRange(JsonConvert.DeserializeObject<List<Course>>(file, somethingCalledMagic) ??
                                    throw new InvalidOperationException());

                file = await File.ReadAllTextAsync(Path.GetFullPath("../../../30_random_course_having_account.json"));
                var random100Courses = JsonConvert.DeserializeObject<List<Course>>(file, somethingCalledMagic);
                if (random100Courses == null)
                {
                    return;
                }

                // handle 100 course that have account
                foreach (var course in random100Courses)
                {
                    var randomNumber = new Random().Next(0, 50);
                    var user = userData[randomNumber];
                    course.SetLearner(user.GetFullName(), user.Gender, user.PhoneNumber!);
                    course.SetLearnerId(user.Id);

                    if (course.Status != Status.Confirmed) continue;
                    var randomTutor = new Random().Next(0, 50);
                    var tutor = tutorData[randomTutor];
                    course.AssignTutor(tutor.Id);
                }

                courseData.AddRange(random100Courses);
                courseData = courseData.OrderBy(x => x.CreationTime).ToList();
                context.Courses.AddRange(courseData);

                #endregion

                # region seed course request

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

    private static void GetUserDataAndTutorData(
        JsonSerializerSettings somethingCalledMagic,
        List<IdentityRole> identityRoles,
        out List<User> userData,
        out List<Tutor> tutorData,
        out List<IdentityUser> identityUsers)
    {
        // Standard user
        var file = File.ReadAllText(Path.GetFullPath("../../../150_random_female_account.json"));
        var identityUser1 = JsonConvert.DeserializeObject<List<IdentityUser>>(file, somethingCalledMagic)!;
        userData = JsonConvert.DeserializeObject<List<User>>(file, somethingCalledMagic)!;
        if (userData == null || identityUser1 == null)
        {
            throw new InvalidOperationException();
        }

        file = File.ReadAllText(Path.GetFullPath("../../../200_random_male_account.json"));
        var userData1 = JsonConvert.DeserializeObject<List<User>>(file, somethingCalledMagic);
        var identityUser2 = JsonConvert.DeserializeObject<List<IdentityUser>>(file, somethingCalledMagic)!;
        if (userData1 == null || identityUser2 == null)
        {
            throw new InvalidOperationException();
        }

        // Update password


        userData.AddRange(userData1);
        userData = userData.OrderBy(x => x.CreationTime).ToList();

        // Tutor
        file = File.ReadAllText(Path.GetFullPath("../../../200_random_tutor.json"));
        var tutorUSerData = JsonConvert.DeserializeObject<List<User>>(file, somethingCalledMagic);
        var identityUser3 = JsonConvert.DeserializeObject<List<IdentityUser>>(file, somethingCalledMagic)!;

        if (tutorUSerData == null || identityUser3 == null)
        {
            throw new InvalidOperationException();
        }

        userData.AddRange(tutorUSerData);
        
        identityUsers = HandlePassword(identityUser1, identityUser2, identityUser3,identityRoles);

        tutorData = JsonConvert.DeserializeObject<List<Tutor>>(file, somethingCalledMagic)!;
        if (tutorData == null)
        {
            throw new InvalidOperationException();
        }

        var i = 0;
        foreach (var tutor in tutorData)
        {
            tutor.SetUserId(tutorUSerData[i++].Id);
        }

        tutorData = tutorData.OrderBy(x => x.CreationTime).ToList();
    }

    private static List<IdentityUser> HandlePassword(
        List<IdentityUser> identityUsers1, 
        List<IdentityUser> identityUsers2,
        List<IdentityUser> tutorIdentityUsers3,
        List<IdentityRole> identityRoles)
    {
        var realOnes = new List<IdentityUser>();

        foreach (var identityUser in identityUsers1)
        {
            var newOne = IdentityUser.Create(
                identityUser.UserName,
                identityUser.Email,
                "1q2w3E*", // Default password
                identityUser.PhoneNumber,
                identityUser.Id,
                identityRoles.Last().Id);

            realOnes.Add(newOne);
        }

        foreach (var identityUser in identityUsers2)
        {
            var newOne = IdentityUser.Create(
                identityUser.UserName,
                identityUser.Email,
                "1q2w3E*", // Default password
                identityUser.PhoneNumber,
                identityUser.Id,
                identityRoles.Last().Id);

            realOnes.Add(newOne);
        }
        
        foreach (var identityUser in tutorIdentityUsers3)
        {
            var newOne = IdentityUser.Create(
                identityUser.UserName,
                identityUser.Email,
                "1q2w3E*", // Default password
                identityUser.PhoneNumber,
                identityUser.Id,
                identityRoles[1].Id);

            realOnes.Add(newOne);
        }

        // var superUser = IdentityUser.Create(
        //     "admin",
        //     "escenter@gmail.com",
        //     "1q2w3E*", // Default password
        //     "0123456789",
        //     identityRoles[0].Id);
        // var superUser1 = IdentityUser.Create(
        //     "handieu",
        //     "handieu@gmail.com",
        //     "1q2w3E*", // Default password
        //     "0123456789",
        //     identityRoles[0].Id);

        // realOnes.Add(superUser);
        // realOnes.Add(superUser1);

        return realOnes;
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

            if (!prop.Writable)
            {
                var property = member as PropertyInfo;
                if (property != null)
                {
                    prop.Writable = property.GetSetMethod(true) != null;
                    var hasPrivateSetter = property?.GetSetMethod(true) != null;
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