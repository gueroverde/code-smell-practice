/*
    Clean Code - Chapter 17: Smells and Heuristics - Functions

    F1: Too Many Arguments
    F2: Output Arguments
    F3: Flag Arguments
    F4: Dead Function
*/

using System.Text;

public class Program {
    public static void Main(string[] args) {
        DisplayMessage(getCenterMessage("*** A very sophisticated student grades dashboard ***"), ConsoleColor.Green);

        displaySemesterReport();

        Console.Write("\n\n\nPress any key to close...");
        Console.ReadKey();
    }

    private static void displaySemesterReport()
    {
        DisplayGrades(createSemesterReport("Cleo", "Strong"), new DisplayConfiguration(lastNameFirst: true, showLetterGrade: false));
        DisplayGrades(createSemesterReport("Olivia", "Allen"), new DisplayConfiguration(lastNameFirst: false, showLetterGrade: false));
        DisplayGrades(createSemesterReport("Fred", "Cisneros"), new DisplayConfiguration(lastNameFirst: true, showLetterGrade: true));
        DisplayGrades(createSemesterReport("Julia", "Pacheco"), new DisplayConfiguration(lastNameFirst: false, showLetterGrade: false));
        DisplayGrades(createSemesterReport("Gene", "Dixon"), new DisplayConfiguration(lastNameFirst: true, showLetterGrade: true));
    }

    private static Semester createSemesterReport(string firstName, string lastName)
    {
         return new Semester(new Student(firstName, lastName), new List<Grade> {
            new Grade(Subject.Math, new Random().Next(5, 11)),
            new Grade(Subject.Coding, new Random().Next(5, 11)),
            new Grade(Subject.Science, new Random().Next(5, 11))
        });
    }

    private static void DisplayGrades(Semester semester, DisplayConfiguration displayConfiguration) {
        string message = getMessage(semester, displayConfiguration);

        DisplayMessage(message);
    }

    private static string getMessage(Semester semester, DisplayConfiguration displayConfiguration)
    {
        string fullName = displayConfiguration.LastNameFirst ? semester.Student.inversedFullName() : semester.Student.fullName();
        StringBuilder message = new StringBuilder($"Student: {fullName}\n");
        message.AppendJoin("", semester.Grades.Select(g => GetLineMessageByGrade(g, displayConfiguration, semester.maxSubjectLength())));

        return message.ToString();
    }

    private static string GetLineMessageByGrade(Grade grade, DisplayConfiguration displayConfiguration, int maxSubjectLength)
    {
        string formattedGrade = displayConfiguration.ShowLetterGrade ? grade.letter() : grade.Value.ToString();
        string padding = new string(' ', maxSubjectLength - grade.Subject.ToString().Length);
        return $"\tSubject: {grade.Subject.ToString()}{padding} - Grade: {formattedGrade}\n";
    }

    private static string getCenterMessage(string message)
    {
        return message.PadLeft((Console.WindowWidth + message.Length) / 2);
    }


    private static void DisplayMessage(string message, ConsoleColor color = ConsoleColor.White) {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }    
}

#region services

#endregion

#region types
public enum Subject {
    Math,
    Coding,
    Science
}

public enum LetterGrade {
    A = 10,
    B = 9,
    C = 8,
    D = 7,
    E = 6,
    F = 5
}

public class Semester{
    public Semester(Student student, List<Grade> grades) {
        Student = student;
        Grades = grades;
    }

     public Student Student { get; private set; }
     public List<Grade> Grades { get; private set; }

     public int maxSubjectLength()
     {
        return Grades.Select(grade => grade.Subject.ToString())
            .ToList()
            .Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur)
            .Length;
     }
}

public class Student {
    public Student(string firstName, string lastname) {
        FirstName = firstName;
        LastName = lastname;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public string fullName() {
        return $"{FirstName} {LastName}";
    }

    public string inversedFullName() {
        return $"{LastName} {FirstName}";
    }
}

public class DisplayConfiguration {
    public DisplayConfiguration(bool lastNameFirst, bool showLetterGrade)
    {
        LastNameFirst = lastNameFirst;
        ShowLetterGrade = showLetterGrade;
    }

         public bool LastNameFirst { get; private set; }
         public bool ShowLetterGrade { get; private set; }
}

public class Grade {
    public Grade(Subject subject, int value) {
        Subject = subject;
        Value = value;
    }

    public Subject Subject { get; private set; }
    public int Value { get; private set; }

    public string letter()
    {
        if (Enum.IsDefined(typeof(LetterGrade), Value)) {
            LetterGrade letter = (LetterGrade)Value;
            return letter.ToString();
        }
         return $"!INVALID";
    }
}
#endregion