using CCAS.Application.Common.Entities;

namespace CCAS.Application.Common.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        // Seed Companies, if necessary
        if (context.Companies.Any())
        {
            return;   // DB has been seeded
        }

        var companies = new List<Company>
        {
            new Company
            {
                Name = "ABC Trading",
                PostalAddress = "P.O. Box 987, Kranskop, 3268",
                PhysicalAddress = "Shop 1, Kranskop, 3268",
                ContactNumber = "033 123 1234",
                Email = "manager@abctrading.co.za",
                Website = "http://www.abctrading.co.za",
                InceptionDate = new DateTime(2020, 2, 25)
            },
            new Company
            {
                Name = "Aheers Kranskop",
                PostalAddress = "P.O. Box 1125, Kranskop, 3268",
                PhysicalAddress = "Shop 5a, Kranskop, 3268",
                ContactNumber = "033 999 1112",
                Email = "manager@aheerskranskop.co.za",
                Website = "http://www.aheerskranskop.co.za",
                InceptionDate = new DateTime(1985, 10, 1)
            },
            new Company
            {
                Name = "Emseni Saverite",
                PostalAddress = "P/Bag 260, Kranskop, 3268",
                PhysicalAddress = "Silverstream Farm, Kranskop, 3268",
                ContactNumber = "032 481 5500",
                Email = "manager@emseni.co.za",
                Website = "http://www.emsenisaverite.co.za",
                InceptionDate = new DateTime(1992, 6, 5)
            },
        };
        context.AddRange(companies);

        var ECNM111 = new Subject
        {
            Name = "Economics: Introduction to Economics",
            Code = "ECNM111",
            Course = "Senior and FET Phase",
            Credits = "10",
            MethodofDelivery = "Full-time contact",
            NQFLevel = "5",
            Year = "1",
            Semester = "1"
        };
        var MTHM111 = new Subject
        {
            Name = "Mathematics: Functions, Models and Limits of Functions",
            Code = "MTHM111",
            Course = "Senior and FET Phase",
            Credits = "10",
            MethodofDelivery = "Full-time contact",
            NQFLevel = "5",
            Year = "1",
            Semester = "1"
        };
        var PHSM111 = new Subject
        {
            Name = "Physical Sciences: Introduction to Mechanics and Optics",
            Code = "PHSM111",
            Course = "Senior and FET Phase",
            Credits = "10",
            MethodofDelivery = "Full-time contact",
            NQFLevel = "5",
            Year = "1",
            Semester = "1"
        };

        var andries = new Lecturer
        {
            Name = "Andries van Tonder",
            LecturerNumber = "L001",
            Email = "andriesvt@cedar.ac.za",
            ContactNumber = "0828278542",
            PhysicalAddress = "KwaSizabantu Mission, Silverstream Farm, Kranskop, 3268",
            PostalAddress = "PO Box 141, Silverstream Farm, Kranskop, 3268",
            InceptionDate = new DateTime(2008, 03, 01)
        };
        var giel = new Lecturer
        {
            Name = "Giel Schoombee",
            LecturerNumber = "L002",
            Email = "giels@cedar.ac.za",
            ContactNumber = "0842002505",
            PhysicalAddress = "KwaSizabantu Mission, Silverstream Farm, Kranskop, 3268",
            PostalAddress = "PO Box ???, Silverstream Farm, Kranskop, 3268",
            InceptionDate = new DateTime(2010, 02, 01)
        };
        var jan = new Lecturer
        {
            Name = "Jan Pienaar",
            LecturerNumber = "L003",
            Email = "janp@cedar.ac.za",
            ContactNumber = "0833744347",
            PhysicalAddress = "KwaSizabantu Mission, Silverstream Farm, Kranskop, 3268",
            PostalAddress = "PO Box ???, Silverstream Farm, Kranskop, 3268",
            InceptionDate = new DateTime(2005, 02, 01)
        };
        context.Add(jan);

        var jessica = new Student
        {
            Name = "Jessica Celick",
            StudentNumber = "20220003",
            Email = "jessicac@student.cedar.ac.za",
            ContactNumber = "072 202 0720",
            PhysicalAddress = "KwaSizabantu Mission, Silverstream Farm, Kranskop, 3268",
            PostalAddress = "PO Box ???, Silverstream Farm, Kranskop, 3268",
            InceptionDate = new DateTime(2022, 02, 01)
        };
        var naledi = new Student
        {
            Name = "Naledi Masibi",
            StudentNumber = "20220004",
            Email = "NalediM@student.cedar.ac.za",
            ContactNumber = "076 246 5567",
            PhysicalAddress = "KwaSizabantu Mission, Silverstream Farm, Kranskop, 3268",
            PostalAddress = "PO Box ???, Silverstream Farm, Kranskop, 3268",
            InceptionDate = new DateTime(2022, 02, 01)
        };
        var jennifer = new Student
        {
            Name = "Jennifer Bazima",
            StudentNumber = "20210003",
            Email = "jenniferb@student.cedar.ac.za",
            ContactNumber = "078 940 6338",
            PhysicalAddress = "KwaSizabantu Mission, Silverstream Farm, Kranskop, 3268",
            PostalAddress = "PO Box ???, Silverstream Farm, Kranskop, 3268",
            InceptionDate = new DateTime(2021, 02, 01)
        };
        context.Add(jennifer);

        var precalculus = new Assessment
        {
            Name = "PRECALCULUS FUNDAMENTALS AND INTRODUCTION TO FUNCTIONS",
            AssessmentCode = "S1",
            Author = "Mr A v Tonder",
            Moderator = "Dr I Vermaak",
            MaxMark = 75,
            Details = "PRECALCULUS FUNDAMENTALS AND INTRODUCTION TO FUNCTIONS",
            Weighting = "15",
            ModerationSubmitDate = new DateTime(2021, 02, 15),
            ModerationCompleteDate = new DateTime(2021, 03, 15),
            DueDate = new DateTime(2022, 03, 15),
            Subject = MTHM111,
        };
        var exponential = new Assessment
        {
            Name = "EXPONENTIAL AND LOGARITHMIC FUNCTIONS",
            AssessmentCode = "S2",
            Author = "Mr A v Tonder",
            Moderator = "Dr I Vermaak",
            MaxMark = 85,
            Details = "EXPONENTIAL AND LOGARITHMIC FUNCTIONS",
            Weighting = "20",
            ModerationSubmitDate = new DateTime(2021, 02, 15),
            ModerationCompleteDate = new DateTime(2021, 03, 15),
            DueDate = new DateTime(2022, 04, 15),
            Subject = MTHM111,
        };
        var trigonometric = new Assessment
        {
            Name = "TRIGONOMETRIC FUNCTIONS",
            AssessmentCode = "S3",
            Author = "Mr A v Tonder",
            Moderator = "Dr I Vermaak",
            MaxMark = 90,
            Details = "TRIGONOMETRIC FUNCTIONS",
            Weighting = "20",
            ModerationSubmitDate = new DateTime(2021, 02, 15),
            ModerationCompleteDate = new DateTime(2021, 03, 15),
            DueDate = new DateTime(2022, 05, 15),
            Subject = MTHM111,
        };

        var assessmentmarks = new List<AssessmentMark>
        {
            new AssessmentMark
            {
                Student = jessica,
                Assessment = precalculus,
                Mark = 53,
            },
            new AssessmentMark
            {
                Student = jessica,
                Assessment = exponential,
                Mark = 67,
            },
            new AssessmentMark
            {
                Student = jessica,
                Assessment = trigonometric,
                Mark = 44,
            },
        };
        context.AddRange(assessmentmarks);

        var ssjoins = new List<SSJoin>
        {
            new SSJoin
            {
                Student = jessica,
                Subject = ECNM111,
            },
            new SSJoin
            {
                Student = jessica,
                Subject = MTHM111,
            },
            new SSJoin
            {
                Student = jessica,
                Subject = PHSM111,
            },
            new SSJoin
            {
                Student = naledi,
                Subject = ECNM111,
            },

        };
        context.AddRange(ssjoins);

        var lsujoins = new List<LSuJoin>
        {
            new LSuJoin
            {
                Lecturer = andries,
                Subject = ECNM111,
            },
            new LSuJoin
            {
                Lecturer = andries,
                Subject = MTHM111,
            },
            new LSuJoin
            {
                Lecturer = andries,
                Subject = PHSM111,
            },
            new LSuJoin
            {
                Lecturer = giel,
                Subject = ECNM111,
            },
        };
        context.AddRange(lsujoins);

        context.SaveChanges();
    }
}
