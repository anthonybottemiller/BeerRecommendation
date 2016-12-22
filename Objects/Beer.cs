using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace BeerRecommendation.Objects
{
	public class Beer
	{
		private int _id;
		private string _name;
		private double _abv;
		private double _ibu;

		public Beer(string name, double abv, double ibu, int id = 0)
		{
			_name = name;
			_abv = Math.Round(abv, 2);
			_ibu = Math.Round(ibu, 2);
			_id = id;
		}

		//Static methods
		public static List<Beer> GetAll()
		{
			List<Beer> allBeers = new List<Beer>{};

			SqlConnection conn = DB.Connection();
			conn.Open();

			SqlCommand cmd = new SqlCommand("SELECT * FROM beers ORDER BY name ASC;", conn);
			SqlDataReader rdr = cmd.ExecuteReader();
			while(rdr.Read())
			{
				int id = rdr.GetInt32(0);
				string name = rdr.GetString(1);
				double abv = (rdr.IsDBNull(2))? 0.0 : rdr.GetDouble(2);
				double ibu = (rdr.IsDBNull(3))? 0.0 : rdr.GetDouble(3);

				Beer newBeer = new Beer(name, abv, ibu, id);
				allBeers.Add(newBeer);
			}
			if (rdr != null) rdr.Close();
			if (conn != null) conn.Close();
			return allBeers;
		}

		//Overload for different order-by parameters
		public static List<Beer> GetAll(string orderBy)
		{
			List<Beer> allBeers = new List<Beer>{};

			SqlConnection conn = DB.Connection();
			conn.Open();

			SqlCommand cmd = new SqlCommand();
			cmd.Connection = conn;

			switch (orderBy)
			{
				case "name":
						cmd.CommandText = "SELECT * FROM beers ORDER BY name ASC;";
						break;
				case "abv":
						cmd.CommandText = "SELECT * FROM beers ORDER BY abv ASC;";
						break;
				case "ibu":
						cmd.CommandText = "SELECT * FROM beers ORDER BY ibu ASC;";
						break;
				default:
					break;
			}

			SqlDataReader rdr = cmd.ExecuteReader();
			while(rdr.Read())
			{
				int id = rdr.GetInt32(0);
				string name = rdr.GetString(1);
				double abv = (rdr.IsDBNull(2))? 0.0 : rdr.GetDouble(2);
				double ibu = (rdr.IsDBNull(3))? 0.0 : rdr.GetDouble(3);

				Beer newBeer = new Beer(name, abv, ibu, id);
				allBeers.Add(newBeer);
			}

			if (rdr != null) rdr.Close();
			if (conn != null) conn.Close();

			return allBeers;
		}

		public static Beer Find(int id)
		{
			SqlConnection conn = DB.Connection();
			conn.Open();
			SqlCommand cmd = new SqlCommand("SELECT name, abv, ibu FROM beers WHERE id = @Id;", conn);
			cmd.Parameters.AddWithValue("@Id", id);
			SqlDataReader rdr = cmd.ExecuteReader();
			string name = null;
			double abv = 0.0;
			double ibu = 0.0;
			while (rdr.Read())
			{
				name = rdr.GetString(0);
				abv = (rdr.IsDBNull(1))? 0.0 : rdr.GetDouble(1);
				ibu = (rdr.IsDBNull(2))? 0.0 : rdr.GetDouble(2);
			}
			if (rdr != null) rdr.Close();
			return new Beer(name, abv, ibu, id);
		}

		public static void DeleteBeer(int id)
		{
			SqlConnection conn = DB.Connection();
			conn.Open();
			SqlCommand cmd = new SqlCommand("DELETE FROM beers_breweries WHERE beer_id = @Id; DELETE FROM favorites WHERE beer_id = @Id; DELETE FROM beers WHERE id = @Id;", conn);
			cmd.Parameters.AddWithValue("@Id", id);
			cmd.ExecuteNonQuery();
			if (conn != null) conn.Close();
		}

		public static void DeleteAll()
		{
			SqlConnection conn = DB.Connection();
			conn.Open();
			SqlCommand cmd = new SqlCommand("DELETE FROM beers; DELETE FROM favorites; DELETE FROM beers_breweries", conn);
			cmd.ExecuteNonQuery();
			if (conn != null) conn.Close();
		}

		//Other methods
		public void Save()
		{
			SqlConnection conn = DB.Connection();
			conn.Open();

			SqlCommand cmd = new SqlCommand("INSERT INTO beers (name, abv, ibu) OUTPUT INSERTED.id VALUES (@Name, @Abv, @Ibu);", conn);

			cmd.Parameters.AddWithValue("@Name", _name);
			cmd.Parameters.AddWithValue("@Abv", _abv);
			cmd.Parameters.AddWithValue("@Ibu", _ibu);

			SqlDataReader rdr = cmd.ExecuteReader();

			while(rdr.Read())
			{
				_id = rdr.GetInt32(0);
			}
			if (rdr != null) rdr.Close();
			if (conn != null) conn.Close();
		}

		public void Update(string name, double abv, double ibu)
		{
			SqlConnection conn = DB.Connection();
			conn.Open();
			SqlCommand cmd = new SqlCommand("UPDATE beers SET name = @Name, abv = @Abv, ibu = @Ibu WHERE id = @Id;", conn);
			cmd.Parameters.AddWithValue("@Name", name);
			cmd.Parameters.AddWithValue("@Abv", abv);
			cmd.Parameters.AddWithValue("@Ibu", ibu);
			cmd.Parameters.AddWithValue("@Id", _id);
			cmd.ExecuteNonQuery();
			if (conn != null) conn.Close();
			_name = name;
			_abv = abv;
			_ibu = ibu;
		}

		public float GetRating()
		{
			SqlConnection conn = DB.Connection();
			conn.Open();
			SqlCommand cmd = new SqlCommand("SELECT rating FROM favorites WHERE beer_id = @Id;", conn);
			cmd.Parameters.AddWithValue("@Id", _id);
			SqlDataReader rdr = cmd.ExecuteReader();
			int totalRating = 0;
			int counter = 0;
			while (rdr.Read())
			{
				counter++;
				totalRating += rdr.GetInt32(0);
			}
			if (rdr != null) rdr.Close();
			if (conn != null) conn.Close();
			if (counter == 0) return 0.0F;
			else return (float) Math.Round(((double) (((double) totalRating)/((double)counter))), 2);
		}

		public List<Brewery> GetBreweries()
		{
			List<Brewery> foundBreweries = new List<Brewery>{};
			SqlConnection conn = DB.Connection();
			conn.Open();
			SqlCommand cmd = new SqlCommand("SELECT breweries.* FROM beers JOIN beers_breweries ON (beers.id = beers_breweries.beer_id) JOIN breweries ON (beers_breweries.brewery_id = breweries.id) WHERE beers.id = @Id ORDER BY breweries.name;", conn);
			cmd.Parameters.AddWithValue("@Id", _id);
			SqlDataReader rdr = cmd.ExecuteReader();
			while (rdr.Read())
			{
				int breweryId = rdr.GetInt32(0);
				string breweryName = rdr.GetString(1);
				string breweryLocation = rdr.GetString(2);
				Brewery newBrewery = new Brewery(breweryName, breweryLocation, breweryId);
				foundBreweries.Add(newBrewery);
			}
			if (rdr != null) rdr.Close();
			if (conn != null) conn.Close();
			return foundBreweries;
		}

		public static List<Beer> Search(string searchBy, string searchInput)
		{
			List<Beer> foundBeers = new List<Beer> {};

			SqlConnection conn = DB.Connection();
			conn.Open();

			SqlCommand cmd = new SqlCommand();
			cmd.Connection = conn;

			string searchValue = "%" + searchInput + "%";

			switch (searchBy.ToLower())
			{
				case "name":
						cmd.CommandText = "SELECT * FROM beers WHERE name LIKE @searchValue";
						break;
				case "brewery":
						cmd.CommandText = "SELECT beers.* FROM breweries JOIN beers_breweries ON (breweries.id = beers_breweries.brewery_id) JOIN beers ON (beers_breweries.beer_id = beers.id) WHERE breweries.name LIKE @searchValue";
						break;
				default:
						break;
			}
			cmd.Parameters.AddWithValue("@searchValue", searchValue);

			SqlDataReader rdr = cmd.ExecuteReader();

			while (rdr.Read())
			{
				int foundId = rdr.GetInt32(0);
				string foundName = rdr.GetString(1);
				double foundAbv = (rdr.IsDBNull(2))? 0.0 : rdr.GetDouble(2);
				double foundIbu = (rdr.IsDBNull(3))? 0.0 : rdr.GetDouble(3);

				foundBeers.Add(new Beer(foundName, foundAbv, foundIbu, foundId));
			}

			if (rdr != null) rdr.Close();
			if (conn != null) conn.Close();

			return foundBeers;
		}

		public static List<Beer> Search(string searchBy, double searchInput, double searchRange = 0)
		{
			List<Beer> foundBeers = new List<Beer> {};

			SqlConnection conn = DB.Connection();
			conn.Open();

			SqlCommand cmd = new SqlCommand();
			cmd.Connection = conn;

			double searchRangeLowerBound = searchInput - searchRange;
			double searchRangeUpperBound = searchInput + searchRange;

			switch (searchBy.ToLower())
			{
				case "abv":
					cmd.CommandText = "SELECT * FROM beers WHERE abv BETWEEN @LowerBound AND @UpperBound;";
					break;
				case "ibu":
					cmd.CommandText = "SELECT * FROM beers WHERE ibu BETWEEN @LowerBound AND @UpperBound;";
					break;
				default:
					break;
			}

			cmd.Parameters.AddWithValue("@LowerBound", searchRangeLowerBound);
			cmd.Parameters.AddWithValue("@UpperBound", searchRangeUpperBound);

			SqlDataReader rdr = cmd.ExecuteReader();

			while (rdr.Read())
			{
				int foundId = rdr.GetInt32(0);
				string foundName = rdr.GetString(1);
				double foundAbv = (rdr.IsDBNull(2))? 0.0 : rdr.GetDouble(2);
				double foundIbu = (rdr.IsDBNull(3))? 0.0 : rdr.GetDouble(3);

				foundBeers.Add(new Beer(foundName, foundAbv, foundIbu, foundId));
			}

			if (rdr != null) rdr.Close();
			if (conn != null) conn.Close();

			return foundBeers;
		}

		//Overrides
		public override bool Equals(Object otherBeer)
		{
			if (!(otherBeer is Beer))
			{
				return false;
			}
			else
			{
				Beer newBeer = (Beer) otherBeer;
				bool idEquality = (_id == newBeer.GetId());
				bool nameEquality = (_name == newBeer.GetName());
				bool abvEquality = (_abv == newBeer.GetAbv());
				bool ibuEquality = (_ibu == newBeer.GetIbu());
				return (idEquality && nameEquality && abvEquality && ibuEquality);
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
		public double GetAbv()
		{
			return _abv;
		}
		public double GetIbu()
		{
			return _ibu;
		}
	}
}
