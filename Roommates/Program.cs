using Roommates.Models;
using Roommates.Repositories;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true; TrustServerCertificate=True;";

        static void Main(string[] args)
        {

            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} has an ID of {c.Id}");
                        }
                        Console.Write("Press any ket to continue");
                        Console.ReadKey();
                        break;

                    case ("Show all unassigned chores"):

                        List<Chore> unassignedChores = choreRepo.GetUnassignedChores();

                        foreach (Chore c in unassignedChores)
                        {
                            Console.WriteLine($"{c.Name} is not assigned.");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Show all roommates"):
                        List<Roommate> roommates = roommateRepo.GetAll();

                        foreach (Roommate r in roommates)
                        {
                            Console.WriteLine($"{r.FirstName} {r.LastName} pays {r.RentPortion}.");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Show all roommates who share a room"):
                        Console.Write("Room Id: ");
                        int roomShareid = int.Parse(Console.ReadLine());

                        List<Roommate> roommatesShare = roommateRepo.GetRoommatebyRoomId(roomShareid);

                        foreach (Roommate r in roommatesShare)
                        {
                            Console.WriteLine($"{r.FirstName} {r.LastName} pays {r.RentPortion}.");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for chore"):
                        Console.Write("Chore Id: ");
                        id = int.Parse(Console.ReadLine());

                        Chore chore = choreRepo.GetById(id);

                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for roomate"):
                        Console.Write("Roommate Id: ");
                        id = int.Parse(Console.ReadLine());

                        Roommate roommate = roommateRepo.GetById(id);

                        Console.WriteLine($"{roommate.FirstName} {roommate.LastName} pays {roommate.RentPortion} dollars per month and they live in the {roommate.Room.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Add a chore"):
                        Console.Write("Chore name: ");
                        name = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = name,
                        };

                        choreRepo.Insert(choreToAdd);

                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Add a roommate"):
                        Console.Write("Roommate First Name: ");
                        string FirstName = Console.ReadLine();

                        Console.Write("Roommate Last Name: ");
                        string LastName = Console.ReadLine();

                        Console.WriteLine("Move In Date:");
                        DateTime MoveDate = DateTime.Parse(Console.ReadLine());

                        Console.Write("Roommate Rent Portion: ");
                        int RentPortion = int.Parse(Console.ReadLine());

                        Console.Write("Roommate Room Assignment: ");
                        int RoomId = int.Parse(Console.ReadLine());

                        Roommate roommateToAdd = new Roommate()
                        {
                            FirstName = FirstName,
                            LastName = LastName,
                            RentPortion = RentPortion,
                            Room = roomRepo.GetById(RoomId),
                            MoveInDate = MoveDate
                        };

                        roommateRepo.Insert(roommateToAdd);

                        Console.WriteLine($"{roommateToAdd.FirstName} has been added and assigned the room of {roommateToAdd.Room.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Update a roommate"):
                        List<Roommate> roommateOptions = roommateRepo.GetAll();
                        foreach (Roommate r in roommateOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.FirstName} {r.LastName} moved in on {r.MoveInDate} lives in {r.RoomName} and pays {r.RentPortion}");
                        }

                        Console.Write("Which roommate would you like to update? ");
                        int selectedRoommateId = int.Parse(Console.ReadLine());
                        Roommate selectedRoommate = roommateOptions.FirstOrDefault(r => r.Id == selectedRoommateId);

                        Console.Write("New First Name: ");
                        selectedRoommate.FirstName = Console.ReadLine();

                        Console.Write("New Last Name: ");
                        selectedRoommate.LastName = Console.ReadLine();

                        Console.Write("New Rent Portion: ");
                        selectedRoommate.RentPortion = int.Parse(Console.ReadLine());

                        Console.Write("New Moved In Date: ");
                        selectedRoommate.MoveInDate = DateTime.Parse(Console.ReadLine());

                        Console.Write("New Room Assignment: ");
                        int newRoomId = int.Parse(Console.ReadLine());
                        selectedRoommate.Room = roomRepo.GetById(newRoomId);
                        
                        roommateRepo.Update(selectedRoommate);

                        Console.WriteLine("Roommate has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Update a chore"):
                        List<Chore> choreOptions = choreRepo.GetAll();
                        foreach (Chore c in choreOptions)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }

                        Console.Write("Which chore would you like to update? ");
                        int selectedChoreId = int.Parse(Console.ReadLine());
                        Chore selectedChore = choreOptions.FirstOrDefault(r => r.Id == selectedChoreId);

                        Console.Write("New Chore: ");
                        selectedChore.Name = Console.ReadLine();

                        choreRepo.Update(selectedChore);

                        Console.WriteLine("Chore has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
               
                    case ("Delete a room"):
                        List<Room> deleteRoomOptions = roomRepo.GetAll();
                        foreach (Room r in deleteRoomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to delete? ");
                        int selectedDeleteRoomId = int.Parse(Console.ReadLine());
                        roomRepo.Delete(selectedDeleteRoomId);

                        Console.WriteLine("Room has been successfully deleted");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Delete a roommate"):
                        List<Roommate> DeleteRoommateOptions = roommateRepo.GetAll();
                        foreach (Roommate r in DeleteRoommateOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.FirstName} {r.LastName})");
                        }

                        Console.Write("Which roommate would you like to delete? ");
                        int selectedDeleteRoommateId = int.Parse(Console.ReadLine());
                        Roommate selectedDeleteRoommate = DeleteRoommateOptions.FirstOrDefault(r => r.Id == selectedDeleteRoommateId);

                        Console.Write("Confirm the room id for me: ");
                        selectedDeleteRoommate.Id = int.Parse(Console.ReadLine());

                        roommateRepo.Delete(selectedDeleteRoommate.Id);

                        Console.WriteLine("Roommate has been successfully deleted");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Delete a chore"):
                        List<Chore> deleteChoreOptions = choreRepo.GetAll();
                        foreach (Chore c in deleteChoreOptions)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }

                        Console.Write("Which chore would you like to delete? ");
                        int selectedDeleteChoreId = int.Parse(Console.ReadLine());
                        choreRepo.Delete(selectedDeleteChoreId);

                        Console.WriteLine("Chore has been successfully deleted");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Update a room",
                "Delete a room",
                "Show all chores",
                "Search for chore",
                "Add a chore",
                "Update a chore",
                "Delete a chore",
                "Show all unassigned chores",
                "Show all roommates",
                "Show all roommates who share a room",
                "Search for roomate",
                "Add a roommate",
                "Delete a roommate",
                "Update a roommate",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}