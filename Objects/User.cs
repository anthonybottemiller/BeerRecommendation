using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;

namespace BeerRecommendation.Objects
{
	public class User
	{
		private int _id;
		private string _name;

		public User(string name, int id = 0)
		{
			_name = name;
			_id = id;
		}

		//Static methods
		public static List<User> GetAll()
		{
			List<User> allUsers = new List<User>{};

			SqlConnection conn = DB.Connection();
			conn.Open();

			SqlCommand cmd = new SqlCommand("SELECT * FROM users ORDER BY name ASC;", conn);
			SqlDataReader rdr = cmd.ExecuteReader();
			while(rdr.Read())
			{
				int id = rdr.GetInt32(0);
				string name = rdr.GetString(1);

				User newUser = new User(name, id);
				allUsers.Add(newUser);
			}
			if (rdr != null) rdr.Close();
			if (conn != null) conn.Close();
			return allUsers;
		}

		public static User Find(int id)
		{
			SqlConnection conn = DB.Connection();
			conn.Open();
			SqlCommand cmd = new SqlCommand("SELECT name FROM users WHERE id = @Id;", conn);
			cmd.Parameters.AddWithValue("@Id", id);
			SqlDataReader rdr = cmd.ExecuteReader();
			string name = null;
			while (rdr.Read())
			{
				name = rdr.GetString(0);
			}
			if (rdr != null) rdr.Close();
			return new User(name, id);
		}

		public static bool UserExists(string name)
		{
			bool userExists = false;
			SqlConnection conn = DB.Connection();
			conn.Open();
			SqlCommand cmd = new SqlCommand("SELECT id FROM users WHERE name = @Name;", conn);
			cmd.Parameters.AddWithValue("@Name", name);
			SqlDataReader rdr = cmd.ExecuteReader();
			while (rdr.Read())
			{
				userExists = true;
			}
			if (rdr != null) rdr.Close();
			if (conn != null) conn.Close();
			return userExists;
		}

		public static void DeleteUser(int id)
		{
			SqlConnection conn = DB.Connection();
			conn.Open();
			SqlCommand cmd = new SqlCommand("DELETE FROM favorites WHERE user_id = @Id; DELETE FROM users WHERE id = @Id;", conn);
			cmd.Parameters.AddWithValue("@Id", id);
			cmd.ExecuteNonQuery();
			if (conn != null) conn.Close();
		}

		public static void DeleteAll()
		{
			SqlConnection conn = DB.Connection();
			conn.Open();
			SqlCommand cmd = new SqlCommand("DELETE FROM favorites; DELETE FROM users;", conn);
			cmd.ExecuteNonQuery();
			if (conn != null) conn.Close();
		}

		//Other methods
		public void Save()
		{
			SqlConnection conn = DB.Connection();
			conn.Open();

			SqlCommand cmd = new SqlCommand("INSERT INTO users (name) OUTPUT INSERTED.id VALUES (@Name);", conn);

			cmd.Parameters.AddWithValue("@Name", _name);

			SqlDataReader rdr = cmd.ExecuteReader();

			while(rdr.Read())
			{
				_id = rdr.GetInt32(0);
			}
			if (rdr != null)
			{
				rdr.Close();
			}
			if (conn != null)
			{
				conn.Close();
			}
		}

		public void Update(string name)
		{
			SqlConnection conn = DB.Connection();
			conn.Open();
			SqlCommand cmd = new SqlCommand("UPDATE users SET name = @Name WHERE id = @Id;", conn);
			cmd.Parameters.AddWithValue("@Name", name);
			cmd.Parameters.AddWithValue("@Id", _id);
			cmd.ExecuteNonQuery();
			if (conn != null) conn.Close();
			_name = name;
		}

		public void RateBeer(int beerId, int rating)
		{
			SqlConnection conn = DB.Connection();
			conn.Open();
			SqlCommand cmd = new SqlCommand("IF ((SELECT COUNT(*) FROM favorites WHERE user_id = @UserId AND beer_id = @BeerId) > 0) UPDATE favorites SET rating = @Rating WHERE user_id = @UserId AND beer_id = @BeerId ELSE INSERT INTO favorites (beer_id, user_id, rating) VALUES (@BeerId, @UserId, @Rating);", conn);
			cmd.Parameters.AddWithValue("@BeerId", beerId);
			cmd.Parameters.AddWithValue("@UserId", _id);
			cmd.Parameters.AddWithValue("@Rating", rating);
			cmd.ExecuteNonQuery();
			if (conn != null) conn.Close();
		}

		public Dictionary<int, List<object>> GetRated()
		{
			Dictionary<int, List<object>> ratedBeers = new Dictionary<int, List<object>>();
			SqlConnection conn = DB.Connection();
			conn.Open();
			SqlCommand cmd = new SqlCommand("SELECT beers.*, favorites.rating FROM favorites JOIN beers ON (favorites.beer_id = beers.id) WHERE favorites.user_id = @Id ORDER BY favorites.rating DESC;", conn);
			cmd.Parameters.AddWithValue("@Id", _id);
			SqlDataReader rdr = cmd.ExecuteReader();
			while (rdr.Read())
			{
				int beerId = rdr.GetInt32(0);
				string beerName = rdr.GetString(1);
				double beerAbv = (rdr.IsDBNull(2))? 0.0 : rdr.GetDouble(2);
				double beerIbu = (rdr.IsDBNull(3))? 0.0 : rdr.GetDouble(3);
				int rating = rdr.GetInt32(4);
				Beer foundBeer = new Beer(beerName, beerAbv, beerIbu, beerId);
				ratedBeers[foundBeer.GetId()] = new List<object> {foundBeer, rating};
			}
			if (rdr != null) rdr.Close();
			if (conn != null) conn.Close();
			return ratedBeers;
		}

		public static int CheckUserName(string userName)
		{
			SqlConnection conn = DB.Connection();
			conn.Open();

			SqlCommand cmd = new SqlCommand("SELECT id FROM users WHERE name = @userName;", conn);
			cmd.Parameters.AddWithValue("@userName", userName);

			var queryResult = cmd.ExecuteScalar();

			int foundId = (queryResult != null) ? (Int32) queryResult : 0;

			if (conn != null) conn.Close();
			return foundId;
		}

		public List<Beer> GetRecommendations(int baseBeerId, int listSize = 5, double ibuModifierIncrement = 2.0, double abvModifierIncrement = 0.1)
		{
			List<Beer> chosenBeers = new List<Beer>{};
			Beer baseBeer = Beer.Find(baseBeerId);
			double abvModifier = 0.0;
			double ibuModifier = 0.0;

			SqlConnection conn = DB.Connection();
			conn.Open();
			while (chosenBeers.Count < listSize)
			{
				double baseAbv = baseBeer.GetAbv();
				double baseIbu = baseBeer.GetIbu();
				double abvNegative = baseAbv - abvModifier;
				double abvPositive = baseAbv + abvModifier;
				double ibuNegative = baseIbu - ibuModifier;
				double ibuPositive = baseIbu + ibuModifier;

				//Get all beers within range for ibu and abv, and where the beer isn't given and hasn't been rated by the user
				SqlCommand cmd = new SqlCommand("SELECT beers.* FROM beers LEFT JOIN favorites ON (beers.id = favorites.beer_id) WHERE (beers.abv BETWEEN @AbvNegative AND @AbvPositive) AND (beers.ibu BETWEEN @IbuNegative AND @IbuPositive) AND (beers.id != @BeerId) ORDER BY beers.name ASC;", conn);
				cmd.Parameters.AddWithValue("@AbvNegative", abvNegative);
				cmd.Parameters.AddWithValue("@AbvPositive", abvPositive);
				cmd.Parameters.AddWithValue("@IbuNegative", ibuNegative);
				cmd.Parameters.AddWithValue("@IbuPositive", ibuPositive);
				cmd.Parameters.AddWithValue("@BeerId", baseBeer.GetId());
				cmd.Parameters.AddWithValue("@UserId", _id);
				SqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					int beerId = rdr.GetInt32(0);
					string beerName = rdr.GetString(1);
					double beerAbv = (rdr.IsDBNull(2))? 0.0 : rdr.GetDouble(2);
					double beerIbu = (rdr.IsDBNull(3))? 0.0 : rdr.GetDouble(3);
					Beer foundBeer = new Beer(beerName, beerAbv, beerIbu, beerId);
					if (!(chosenBeers.Contains(foundBeer)))
					{
						chosenBeers.Add(foundBeer);
					}
				}
				if (rdr != null) rdr.Close();

				abvModifier += abvModifierIncrement;
				ibuModifier += ibuModifierIncrement;
			}

			if (chosenBeers.Count > listSize)
			{
				chosenBeers.RemoveRange(listSize, (chosenBeers.Count - listSize));
			}

			if (conn != null) conn.Close();
			return chosenBeers;
		}

		//Overrides
		public override bool Equals(Object otherUser)
		{
			if (!(otherUser is User))
			{
				return false;
			}
			else
			{
				User newUser = (User) otherUser;
				bool idEquality = (_id == newUser.GetId());
				bool nameEquality = (_name == newUser.GetName());
				return (idEquality && nameEquality);
			}
		}

		public override int GetHashCode()
		{
			return _name.GetHashCode();
		}

		//Getters & Setters
		public int GetId()
		{
			return _id;
		}
		public string GetName()
		{
			return _name;
		}
	}
}
