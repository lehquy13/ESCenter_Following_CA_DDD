using System.Diagnostics;
using Bogus;
using ESCenter.Domain.Aggregates.Subjects;
using ESCenter.Domain.Aggregates.Subjects.ValueObjects;
using ESCenter.Domain.Aggregates.TutorRequests;
using ESCenter.Domain.Aggregates.Tutors;
using ESCenter.Domain.Aggregates.Tutors.Entities;
using ESCenter.Domain.Aggregates.Users;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace ESCenter.DBMigrator;

internal static class SeedTutor
{
    public static async Task GetUserDataAndTutorData(
        JsonSerializerSettings somethingCalledMagic,
        UserManager<IdentityUser> userManager,
        List<IdentityRole> identityRoles,
        List<Customer> userData,
        List<Tutor> tutorData,
        List<TutorRequest> tutorRequestData,
        List<IdentityUserRole<string>> userRoles)

    {
        // Standard user
        var file = await File.ReadAllTextAsync(Path.GetFullPath("../../../2000_m_learner.json"));
        var learner2000M = JsonConvert.DeserializeObject<List<Customer>>(file, somethingCalledMagic) ??
                           throw new InvalidOperationException();

        userData.AddRange(learner2000M.Take(learner2000M.Count / 4));

        file = await File.ReadAllTextAsync(Path.GetFullPath("../../../2000_f_learner.json"));
        var learner2000F = JsonConvert.DeserializeObject<List<Customer>>(file, somethingCalledMagic) ??
                           throw new InvalidOperationException();

        userData.AddRange(learner2000F.Take(learner2000F.Count / 4));

        // Tutor
        file = await File.ReadAllTextAsync(Path.GetFullPath("../../../2000_f_tutor.json"));

        var femaleTutorCustomerData = JsonConvert.DeserializeObject<List<Customer>>(file, somethingCalledMagic) ??
                                      throw new InvalidOperationException();

        femaleTutorCustomerData = femaleTutorCustomerData.Take(femaleTutorCustomerData.Count / 8).ToList();

        var femaleTutorData = JsonConvert.DeserializeObject<List<Tutor>>(file, somethingCalledMagic) ??
                              throw new InvalidOperationException();

        file = await File.ReadAllTextAsync(Path.GetFullPath("../../../2000_m_tutor.json"));

        var maleTutorCustomerData = JsonConvert.DeserializeObject<List<Customer>>(file, somethingCalledMagic) ??
                                    throw new InvalidOperationException();

        maleTutorCustomerData = maleTutorCustomerData.Take(maleTutorCustomerData.Count / 8).ToList();

        var maleTutorData = JsonConvert.DeserializeObject<List<Tutor>>(file, somethingCalledMagic) ??
                            throw new InvalidOperationException();

        femaleTutorCustomerData.AddRange(maleTutorCustomerData);

        userData.AddRange(femaleTutorCustomerData);

        tutorData.AddRange(femaleTutorData.Take(femaleTutorData.Count / 8));
        tutorData.AddRange(maleTutorData.Take(maleTutorData.Count / 8));

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

        var i = 0;
        var faker = new Faker();

        foreach (var tutor in tutorData)
        {
            tutor.SetUserId(femaleTutorCustomerData[i++].Id);

            if (i > 50) continue;

            var random = new Random();

            for (var j = 0; j < random.Next(2, 7); j++)
            {
                var request = TutorRequest.Create(
                    tutor.Id,
                    userData[random.Next(0, 99)].Id,
                    faker.Lorem.Sentence(3, 5));

                var doneOrNot = random.Next(0, 10);

                if (doneOrNot % 2 == 0)
                {
                    request.Done();
                }

                tutorRequestData.Add(request);
            }
        }
    }

    public static void SeedMajor(List<Tutor> tutorData, IReadOnlyList<Subject> subjects)
    {
        var ii = tutorData.Count;

        foreach (var tutor in tutorData)
        {
            if (ii-- <= 0) break;

            List<int> addedList = [];
            var tmList = new List<TutorMajor>();
            var randomQuantity = new Random().Next(1, 4);

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
}