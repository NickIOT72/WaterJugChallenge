using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.Xml;
using WaterJugChallenge.Models;
using WaterJugChallenge.Models.Entities;
using WaterJugChallenge.Services;

namespace WaterJugChallenge.Controllers
{

    //locahost:xxxx/api/bucket
    [Route("api/[controller]")]
    [ApiController]
    public class BucketController : ControllerBase
    {
        private readonly ISolutionCompleteService _solutionComplete;
        
        public BucketController(ISolutionCompleteService solutionComplete)
        {
            _solutionComplete = solutionComplete;
            //sol_Complete_array = _solutionComplete.GetSolutionComplete();
        }

        [HttpGet]
        public IActionResult GetAllSolutions()
        {
            List<SolutionComplete> sol_Complete_array = new List<SolutionComplete>();
            sol_Complete_array = _solutionComplete.GetSolutionComplete();
            return Ok(sol_Complete_array);
        }
        
        [HttpPost]
        public IActionResult AddBucket(AddBucketDto addBucketDto)
        {

            // Get information from JSON Payload
            int X_cap = addBucketDto.x_capacity;
            int Y_cap = addBucketDto.y_capacity;
            int Z_gallon = addBucketDto.z_amount_wanted;

            var bucket_income = new Bucket() { x_capacity = X_cap, y_capacity = Y_cap, z_amount_wanted = Z_gallon };

            List<SolutionComplete> sol_Complete_array = new List<SolutionComplete>();
            sol_Complete_array = _solutionComplete.GetSolutionComplete();

            if (sol_Complete_array.Count > 0)
            {
                foreach (var solComp in sol_Complete_array)
                {
                    // Create two lists of pets.
                    //List<Bucket> bk_1 = new List<Bucket> { solComp.Bk };
                    //List<Bucket> bk_2 = new List<Bucket> { bucket_income };
                    //bool eq = (solComp.Bk).SequenceEqual(bk_2);
                    bool eq = solComp.Bucket.x_capacity == bucket_income.x_capacity && solComp.Bucket.y_capacity == bucket_income.y_capacity && solComp.Bucket.z_amount_wanted == bucket_income.z_amount_wanted;
                    if (eq)
                    {
                        Console.WriteLine("Bucket list found");
                        return Ok(solComp.Solution);
                    }
                }
            }

            // Verify all values are greater than zero (0)
            if (X_cap % 2 != 0 || Y_cap % 2 != 0 || Z_gallon % 2 != 0)
            {
                var err_msg = new ErrorMessage();
                err_msg.message = "All values must be greater than zero (0)";
                return NotFound(err_msg);
            }

            // Verify all values are even
            if (X_cap % 2 != 0 || Y_cap % 2 != 0 || Z_gallon % 2 != 0) {
                var err_msg = new ErrorMessage();
                err_msg.message = "All inputs must be even. Not Solution can be found";
                return NotFound(err_msg);
            }

            // Verfiy Capacity of bucket X
            if (X_cap > 2)
            {
                var err_msg = new ErrorMessage();
                err_msg.message = "Capacity of Bucket X cannot be higher than 2";
                return NotFound(err_msg);
            }

            // Verfiy Capacity of Z galoons
            if (!(X_cap < Z_gallon && Z_gallon < Y_cap))
            {
                var err_msg = new ErrorMessage();
                err_msg.message = "Z gallonds must be higher than Capacity X and lower than Capacity Y";
                return NotFound(err_msg);
            }

            // Find the best solution
            bool ReferenceBucket = false;
            ReferenceBucket = Z_gallon - X_cap <  Y_cap - Z_gallon;

            int total_capacity = X_cap + Y_cap;
            int Bucket_X = 0;
            int Bucket_Y = 0;
            int step_num = 0;

            List<Steps> step_data = new List<Steps>();

            while ( Bucket_Y != Z_gallon)
            {
                int capacityToFill = step_num == 0 ? (ReferenceBucket ? X_cap : Y_cap) : 2;
                string act_msg = "";
                if (step_num == 0)
                {
                    Bucket_X = ReferenceBucket ? capacityToFill : 0;
                    Bucket_Y = !ReferenceBucket ? capacityToFill : 0;
                    act_msg = "Fill bucket " + (ReferenceBucket ? "x" : "y");
                }
                else
                {
                    if (ReferenceBucket)
                    {
                        if (Bucket_X == 0)
                        {
                            Bucket_X += capacityToFill;
                            act_msg = "Fill bucket x";
                        }
                        else if (Bucket_X == 2)
                        {
                            Bucket_Y += capacityToFill;
                            Bucket_X = 0;
                            act_msg = "Transfer from bucket x to bucket y";
                        }
                    }
                    else
                    {
                        if (Bucket_X == 0)
                        {
                            Bucket_Y -= capacityToFill;
                            Bucket_X += capacityToFill;
                            act_msg = "Fill bucket x";
                        }
                        else if (Bucket_X == 2)
                        {
                            Bucket_X = 0;
                            act_msg = "Empty bucket x";
                        }
                    }
                }
                step_num += 1;
                Steps step_new = new Steps() { step = step_num, bucket_x = Bucket_X, bucket_y = Bucket_Y, action_msg = act_msg, status = (Bucket_Y == Z_gallon)?"SOLVED":"" };
                step_data.Add(step_new);


            }

            Solution sol_data = new Solution() { steps = step_data.ToArray() };
            SolutionComplete sol_complete_data = new SolutionComplete() { Solution = sol_data, Bucket = bucket_income };

            List<SolutionComplete> sol_Complete_Save_array = new List<SolutionComplete>();
            sol_Complete_Save_array.Add(sol_complete_data); 
            _solutionComplete.SaveSolutionComplete(sol_Complete_Save_array);
            

            return Ok(sol_data);
        }

    }
}
