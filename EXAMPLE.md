public class User : RavenObject<User>
{
	public string UserName {get;set;}
	public string UserPassword {get;set;}
	public string UserRole {get; set;}

    public override string ToString()
    {
      return UserName + " " + UserPassword + " " + UserRole + " " + string.Format("ID {0}", Id);
    }
}

public class Users : RavenObjectsDatabaseAsync<User>
{
    public Users(string DBCollectionName, string dbname, string dbhostname, int dbport) : base(DBCollectionName, dbname, dbhostname, dbport)
    {
    }
}

//The program itself
public static async void TestFunc(){
	Users users = new Users("users", "test", "localhost", 5000);
	List<IRavenObject> all_users = await users.LoadMultipleObjectsFromDBAsync()
	
    List<IRavenObject> all_users = await users.LoadMultipleObjectsFromDBAsync();
    Console.Write("Thread is done! Loaded objects : {0}", all_users.Count);
    await Task.Delay(10);
    Console.Write("Deleting all objects from DB....");
    bool t = await users.DeleteMultipleObjectsFromDBAsync(all_users);
    if (t)
      Console.WriteLine("Deleteion complete!");
	
}
