using ESCenter.Domain.Aggregates.Subjects;

namespace ESCenter.DBMigrator;

public class SubjectDataBuilder
{
    public static List<Subject> Subjects { get; }

    static SubjectDataBuilder()
    {
        Subjects = Build();
    }

    private static List<Subject> Build()
    {
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

        #endregion

        return subjects;
    }
}