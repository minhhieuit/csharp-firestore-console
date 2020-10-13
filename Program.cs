using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace TestFirebase
{
    class Program
    {
        static async Task Main(string[] args)
        {
            FirestoreDb db = InitDb("blazor-assembly-firebase");

            // await AddDataAsync(db);

            await RetrieveAllDocuments(db);
            Console.ReadLine();
        }

        private static FirestoreDb InitDb(string project)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),@"C:\blazor-assembly-firebase-firebase-adminsdk-9tbks-737f14975d.json");
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",path);

            FirestoreDb db = FirestoreDb.Create(project);
            Console.WriteLine("Init db success");
            return  db;
        }

        private static async Task AddDataAsync(FirestoreDb db)
        {
            DocumentReference docRef = db.Collection("users").Document("jacky");
            Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "First", "Jacky" },
                { "Last", "Nguyen" },
                { "Born", 1991 }
            };

            await docRef.SetAsync(user);
        }

         private static async Task RetrieveAllDocuments(FirestoreDb db)
        {
            CollectionReference usersRef = db.Collection("users");
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Console.WriteLine("User: {0}", document.Id);
                Dictionary<string, object> documentDictionary = document.ToDictionary();
                Console.WriteLine("First: {0}", documentDictionary["First"]);
                if (documentDictionary.ContainsKey("Middle"))
                {
                    Console.WriteLine("Middle: {0}", documentDictionary["Middle"]);
                }
                Console.WriteLine("Last: {0}", documentDictionary["Last"]);
                Console.WriteLine("Born: {0}", documentDictionary["Born"]);
                Console.WriteLine();
            }
        }

    }
}
