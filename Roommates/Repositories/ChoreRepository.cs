using Microsoft.Data.SqlClient;
using Roommates.Models;

namespace Roommates.Repositories
{

    /// This class is responsible for interacting with Chore data.
    /// It inherits the BaseRepository class so that it can use the BaseRepository's Connection property.
    public class ChoreRepository : BaseRepository
    {

        /// When new ChoreRepository is instatiated, pass the connection string along to the BaseRepository.
        public ChoreRepository(string connectionString) : base(connectionString) { }

        /// Get a list of all Chores in the database
        public List<Chore> GetAll()
        {
            ////  We must "use" the database connection.
            ////  Because a database is a shared resource (other applications may be using it too) we must
            ////  be careful about how we interact with it. Specifically, we Open() connections when we need to
            ////  interact with the database and we Close() them when we're finished.
            ////  In C#, a "using" block ensures we correctly disconnect from a resource even if there is an error.
            ////  For database connections, this means the connection will be properly closed.

            using (SqlConnection conn = Connection)
            {

                /// We must Open() the connectoin, the "using block doesn't do that for us.
                conn.Open();

                /// We must "use" commands too.

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we setup command with the AQL we want to execuse before we execute it.
                    cmd.CommandText = "SELECT Id, Name FROM Chore";

                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // A list to hold the chores we retrieve from the database.
                    List<Chore> chores = new List<Chore>();

                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {
                        // The "ordinal" is the numeric position of the column in the query results.
                        // For our query, "Id" has an ordinal value of 0 and "Name" is 1.

                        int idColumnPosition = reader.GetOrdinal("Id");

                        // We use the reader's getXXX methods to get the value for a particular ordinal. 

                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPosition);

                        // Create a new chore object using the data from the database.

                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue
                        };

                        // ... and add the chore object to our list 
                        chores.Add(chore);
                    }

                    // Close() the reader. Unfortunately, a "using" block won't work here.
                    reader.Close();

                    // Return the list of chores to whomever called the method.
                    return chores;
                 
                }
            }
        }

        public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Chore chore = null;

                    // If we only expect a single row back from the database, we don't need a while loop.

                    if (reader.Read())
                    {
                        chore = new Chore
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }

                    reader.Close();

                    return chore;
                }
            }
        }

        ///  Add a new chore to the database
        ///   NOTE: This method sends data to the database,
        ///   it does not get anything from the database, so there is nothing to return.
        public void Insert(Chore chore)
        {
            using(SqlConnection conn = Connection) 
            {

                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name)
                                        OUTPUT INSERTED.Id
                                        VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
            }
        }

        public List<Chore> GetUnassignedChores()
        {
            List<Chore> unassignedChores = new List<Chore>();

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"Select Chore.Name, Chore.Id, RoommateChore.ChoreID from Chore Left Join RoommateChore on Chore.Id = RoommateChore.ChoreID Where RoommateChore.ChoreID IS NULL";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Chore chore = new Chore
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };

                            unassignedChores.Add(chore);
                        }
                    }
                }
            }

            return unassignedChores;
        }

        public void Update(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Chore
                                    SET Name = @name
                                    WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    cmd.Parameters.AddWithValue("@id", chore.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM RoommateChore WHERE ChoreID = @id
                                        DELETE FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}

