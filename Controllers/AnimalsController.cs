using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Zadanie4.DTOs;

namespace Zadanie4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get([FromQuery]string param)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=db-mssql16;Initial Catalog=s22136;Integrated Security=True"))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand($"SELECT * FROM Animal ORDER BY {param}", connection);

                SqlDataReader reader = command.ExecuteReader();
                //List of animals
                List<AnimalDTO> animalDtos = new List<AnimalDTO>();
                while (reader.Read())
                {
                    var id = (int)reader["IdAnimal"];
                    var name = (string)reader["Name"];
                    var description = (string)reader["Description"];
                    var category = (string)reader["Category"];
                    var area = (string)reader["Area"];
                    animalDtos.Add(new AnimalDTO(id, name, description, category, area));
                }
                
                reader.Close();
                connection.Close();

                if (animalDtos.Count > 0)
                {
                  //  var jsonString = Json.Encode(animalDtos);
                    return Ok(animalDtos);
                }
                else
                {
                    return BadRequest("Not enough data");
                }
            }
        }


        [HttpPut]
        public IActionResult UpdateAnimal(int id, [FromBody] AnimalDTO updatedAnimal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (SqlConnection connection = new SqlConnection("Data Source=db-mssql16;Initial Catalog=s22136;Integrated Security=True"))
            {
                connection.Open();
                string query = $"UPDATE Animal SET Name = '{updatedAnimal.Name}', Description = '{updatedAnimal.Description}', Category = '{updatedAnimal.Category}', Area = '{updatedAnimal.Area}' WHERE IdAnimal = {id}";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
        }


        [HttpDelete]
        public ActionResult DeletAnimal(int id)
        {
            using (SqlConnection connection =
                   new SqlConnection("Data Source=db-mssql16;Initial Catalog=s22136;Integrated Security=True"))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand($"DELETE FROM Animal WHERE IdAnimal = {id}", connection);

                var numberOfRows = (int?)command.ExecuteNonQuery();
                connection.Close();

                if (numberOfRows == 0)
                {
                    return NotFound();
                }
                
            }

            return Ok("animal was deleted");
        }
        

        [HttpPost]
        public ActionResult Post(AnimalDTO animal)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=db-mssql16;Initial Catalog=s22136;Integrated Security=True"))
            {
                connection.Open();

                using SqlCommand command = new SqlCommand("INSERT INTO Animal VALUES (@Name, @Description, @Category, @Area)", connection);

                command.Parameters.AddWithValue("@Name", animal.Name);
                command.Parameters.AddWithValue("@Description", animal.Description);
                command.Parameters.AddWithValue("@Category", animal.Category);
                command.Parameters.AddWithValue("@Area", animal.Area);
                
                var numberOfRows = (int?)command.ExecuteNonQuery();
                connection.Close();
                if (numberOfRows == 0)
                {
                    return NotFound();
                }
                return Ok();
            }
        }
    }
}
